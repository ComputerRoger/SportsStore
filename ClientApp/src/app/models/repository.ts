import { Injectable } from "@angular/core";
import { HttpClient } from "@Angular/common/http";
import { SignalRConnectionService } from "../websockets/signalRConnection.service";

import { Product } from './product.model';
import { Supplier } from './supplier.model';
import { Filter, Pagination } from './configClasses.repository';
import { Observable } from "rxjs";
import { Order, OrderConfirmation } from "./order.model";

//	npm install @aspnet/signalr --save --force

//import * as signalR from '@aspnet/signalr';
import { HubConnectionBuilder, LogLevel, HubConnection, HubConnectionState } from '@aspnet/signalr';

const productsUrl = "/api/productvalues";
const suppliersUrl = "/api/suppliervalues";
const sessionUrl = "/api/session";
const ordersUrl = "/api/orders";
const accountUrl = "/api/account";

//	The following must match Startup.cs - Configure().
//	endpoints.MapHub<ChatHub>("/chat");
const signalRHubUrl = "/chat";

const signalRHubMethodName = "SendMessage";
const signalRClientMethodName = "newMessage";

//	The client and server keys of this type must match.
type ProductsMetadata =
	{
		products: Product[],
		categories: string[];
	}

//	REPORTING:			HTTP verb API	>>	Repository properties	>>	Component properties	>>	Template controls
//	PERSISTING:			Template events	>>	Component handlers	>>	Repository methods	>>	HTTP verb API

@Injectable()
export class Repository {
	productData: Product;
	products: Product[];
	supplierData: Supplier;
	suppliers: Supplier[] = [];
	filter: Filter = new Filter();
	categories: string[] = [];
	pagination = new Pagination();
	orders: Order[] = [];
	hubConnection: HubConnection;

	constructor(private httpClient: HttpClient) {
		//	Set the filters.
		//this.filter.category = "soccer";
		this.filter.related = true;

		//	Angular emits a routing event when the application first starts,
		//	which means that the HTTP request of the repository constructor can be removed.

		//	Get all the products.
		// this.getProducts(true);
		//this.getProducts();
		this.initializeSignalR();
	}

	initializeSignalR() {

		//	Startup.cs in the server has this endpoint:
		//	endpoints.MapHub<ChatHub>("/chat");

		this.hubConnection = new HubConnectionBuilder()
			.configureLogging(LogLevel.Information)
			.withUrl(signalRHubUrl)
			.build();

		//	Start the hubConnection before using it.
		this.hubConnection.start().then(() => {
			//	Log the message received to the console.
			console.log("The hub connection has started.");
			this.hubConnection.on(signalRClientMethodName, (sender, messageText) => {
				console.log();
				console.log("Success!  Message received from the hub! " + `${sender}:${messageText}`);
				console.log();
			});
		});

		console.log("End of ngOnInit().");
	}

	public broadcastMessage(message: string) {
		if (this.hubConnection) {
			console.log("signalR hubConnection is defined.");
			if (this.hubConnection.state == HubConnectionState.Connected) {
				console.log("signalR hubConnection is Connected.");

				this.hubConnection.invoke(signalRHubMethodName, "This is an invoked message via hub method: " + signalRHubMethodName);
				console.log("done broadcasting: " + message);
				this.hubConnection.send(signalRHubMethodName, "Using send() with " + signalRHubMethodName);
			}
			else if (this.hubConnection.state == HubConnectionState.Disconnected) {
				console.log("signalR hubConnection is Disconnected.");
			}
			else {
				console.log("signalR hubConnection state is not known.");
			}
		}
		else {
			console.log("signalR hubConnection is null.");
		}
	}

	//////////////////////		Authentication Methods			////////////////////////

	login(name: string, password: string): Observable<boolean> {
		let actionPath = accountUrl + "/login";
		return this.httpClient.post<boolean>(actionPath,
			{
				name: name,
				password: password
			});
	}

	logout() {
		let actionPath = accountUrl + "/logout";
		this.httpClient.post(actionPath, null).subscribe(() => { });
	}

	//////////////////////		Session Methods			////////////////////////

	storeSessionData<T>(dataType: string, data: T) {
		let endPoint = `${sessionUrl}/${dataType}`;
		return this.httpClient.post(endPoint, data)
			.subscribe(foo => { });
	}

	getSessionData<T>(dataType: string): Observable<T> {
		let endPoint = `${sessionUrl}/${dataType}`;
		let data = this.httpClient.get<T>(endPoint);
		return data;
	}

	///////////////////////		Orders 					////////////////////////

	getOrders() {
		this.httpClient.get<Order[]>(ordersUrl)
			.subscribe(data => this.orders = data);
	}

	createOrder(order: Order) {
		this.httpClient.post<OrderConfirmation>(ordersUrl,
			{
				name: order.name,
				address: order.address,
				payment: order.payment,
				products: order.products
			}).subscribe(data => {
				//	Get the observable result.
				order.orderConfirmation = data;

				//	Clear the cart.
				order.cartService.clear();
				order.clear();
			});
	}

	shipOrder(order: Order) {
		this.httpClient.post(`${ordersUrl}/${order.orderId}`,
			{}).subscribe(() => this.getOrders());
	}


	//////////////////////		Read = HTTP GET			////////////////////////

	//  Get a product.
	getProduct(idProduct: number) {
		let endPoint = `${productsUrl}/${idProduct}`;
		this.httpClient.get<Product>(endPoint).subscribe(p => this.productData = p);
		console.log("Product Data Received.");
	}

	getProducts() {
		console.log("Products Requested.");
		this.broadcastMessage("A message broadcast during getProducts.");

		let endPoint = `${productsUrl}?isRelatedRequired=${this.filter.related}`;
		if (this.filter.category) {
			endPoint += `&category=${this.filter.category}`;
		}
		if (this.filter.search) {
			endPoint += `&search=${this.filter.search}`;
		}

		//	Include categories as auxillary data.
		endPoint += "&isMetadata=true";
		//endPoint += "&isMetadata=false";

		//	Get the composite data.
		this.httpClient.get<ProductsMetadata>(endPoint)
			.subscribe(productsMetadata => {
				this.products = productsMetadata.products;
				this.categories = productsMetadata.categories;
			});
		return this.products;
	}

	//  Get a supplier.
	getSupplier(idSupplier: number) {
		let endPoint = `${suppliersUrl}/${idSupplier}`;
		this.httpClient.get<Supplier>(endPoint).subscribe(s => this.supplierData = s);
		console.log("Supplier Data Received.");
	}

	getSuppliers() {
		console.log("Suppliers Requested.");
		let endPoint = `${suppliersUrl}`;

		this.httpClient.get<Supplier[]>(endPoint)
			.subscribe(suppliers => this.suppliers = suppliers);
	}

	///////////////		Create = HTTP POST		//////////////////////

	createProduct(product: Product) {
		//	Initialize the api buffer.
		let productData =
		{
			name: product.name,
			category: product.category,
			description: product.description,
			price: product.price,
			supplier: product.supplier ? product.supplier.supplierId : 0
		}

		//	Post the object.
		this.httpClient.post<number>(productsUrl, productData)
			.subscribe(id => {
				//	The new primary key is returned.
				product.productId = id;

				//	push() method appends the given element(s) in the last of the array
				//	and returns the length of the new array.
				this.products.push(product);
			});
	}

	createProductAndSupplier(product: Product, supplier: Supplier) {
		//	Initialize the api buffer.
		let supplierData =
		{
			name: supplier.name,
			city: supplier.city,
			state: supplier.state
		};

		//	Post the foreign key object first.
		this.httpClient.post<number>(suppliersUrl, supplierData)
			.subscribe(id => {
				supplier.supplierId = id;
				product.supplier = supplier;
				this.suppliers.push(supplier);
				if (product != null) {
					//	Post the parent after posting the foreign key object.
					this.createProduct(product);
				}
			});
	}

	///////////////		Replace = HTTP PUT		//////////////////////

	replaceProduct(product: Product) {
		//	Initialize the api buffer.
		let productData =
		{
			name: product.name,
			category: product.category,
			description: product.description,
			price: product.price,
			supplier: product.supplier ? product.supplier.supplierId : 0
		}

		//	HTTP PUT the object with the id attached to the URL.
		this.httpClient.put(productsUrl + `/${product.productId}`, productData)
			.subscribe(() => {
				//	Refresh the products property.
				this.getProducts();
			});
	}

	replaceSupplier(supplier: Supplier) {
		//	Initialize the api buffer.
		let supplierData =
		{
			name: supplier.name,
			city: supplier.city,
			state: supplier.state
		}

		//	HTTP PUT the object with the id attached to the URL.
		this.httpClient.put(suppliersUrl + `/${supplier.supplierId}`, supplierData)
			.subscribe(() => {
				//	Refresh the suppliers property.
				//this.getSuppliers();
				this.getProducts();
			});
	}

	///////////////		Update = HTTP PATCH		//////////////////////

	updateProduct(id: number, changes: Map<string, any>) {

		//	Initialize the api buffer.
		let patch = [];

		//	Accumulate the patch operations.
		changes.forEach((value, key) =>
			patch.push(
				{
					op: "replace", path: key, value: value
				}));

		//	Send the patch and update the repository.
		this.httpClient.patch(`${productsUrl}/${id}`, patch)
			.subscribe(() => this.getProducts());
	}

	///////////////		Delete = HTTP DELETE		//////////////////////

	deleteProduct(id: number) {

		//	Send the patch and update the repository.
		this.httpClient.delete(`${productsUrl}/${id}`)
			.subscribe(() => this.getProducts());
	}

	deleteSupplier(id: number) {

		//	Send the patch and update the repository.
		this.httpClient.delete(`${suppliersUrl}/${id}`)
			.subscribe(() => {
				this.getProducts();
				this.getSuppliers()
			});
	}

	//	End of the class.
}
