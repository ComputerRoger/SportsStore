import { Injectable } from "@angular/core";
import { HttpClient } from "@Angular/common/http";

import { Product } from './product.model';
import { Supplier } from './supplier.model';
import { Filter, Pagination } from './configClasses.repository';
import { Observable } from "rxjs";
import { Order, OrderConfirmation } from './order.model';
import { SignalRService } from "./signalR.service";

//	Define the MVC Route patterns associated with Controller prefixes.
const productsUrl = "/api/productvalues";			//	ProductValuesController
const suppliersUrl = "/api/suppliervalues";			//	SupplierValuesController
const sessionUrl = "/api/session";					//	SessionValuesController
const ordersUrl = "/api/orders";					//	OrderValuessController
const accountUrl = "/api/account";					//	AccountController
//const queryBrowserUrl = "/api/queryBrowser";		//	QueryBrowserController
const queryBrowserUrl = "/api/queryBrowser";		//	QueryBrowserController

//	The client and server keys of this type must match.
type ProductsMetadata =
	{
		products: Product[],
		categories: string[]
	}

//	Define the container for posting data to the web API.
export class PostQueryBody
{
	public receiveUrl: string;
	public receiveSearch: string;

	constructor(receiveUrl: string, receiveSearch: string)
	{
		this.receiveUrl = receiveUrl;
		this.receiveSearch = receiveSearch;
	}
}

export class QueryBrowserResult
{
	constructor(public textArray: string[]
		, public isError: boolean
		, public isSuccess: boolean
		) { }
}

//	REPORTING:			HTTP verb API	>>	Repository properties	>>	Component properties	>>	Template controls
//	PERSISTING:			Template events	>>	Component handlers	>>	Repository methods	>>	HTTP verb API

@Injectable()
export class Repository
{
	productData: Product;
	products: Product[];
	supplierData: Supplier;
	suppliers: Supplier[] = [];
	filter: Filter = new Filter();
	categories: string[] = [];
	pagination = new Pagination();
	orders: Order[] = [];

	constructor(private httpClient: HttpClient, private signalRService: SignalRService)
	{
		//	Set the filters.
		//this.filter.category = "soccer";

		this.filter.related = true;
	}

	queryBrowserGet()
	{
		let actionPath = queryBrowserUrl + "/Test";
		this.httpClient.get<String>(actionPath).subscribe(() => { });
	}


	queryBrowserPost(postQueryBody: PostQueryBody)
	{
		let actionPath = queryBrowserUrl + "/Test";
		let queryBrowserResult = this.httpClient.post<QueryBrowserResult>(actionPath, postQueryBody);
		return queryBrowserResult;
	}

	//////////////////////		Authentication Methods			////////////////////////

	login(name: string, password: string): Observable<boolean>
	{
		let actionPath = accountUrl + "/login";
		return this.httpClient.post<boolean>(actionPath,
			{
				name: name,
				password: password
			});
	}

	logout()
	{
		let actionPath = accountUrl + "/logout";
		this.httpClient.post(actionPath, null).subscribe(() => { });
	}

	//////////////////////		Session Methods			////////////////////////

	storeSessionData<T>(dataType: string, data: T)
	{
		let endPoint = `${sessionUrl}/${dataType}`;
		return this.httpClient.post(endPoint, data)
			.subscribe(foo => { });
	}

	getSessionData<T>(dataType: string): Observable<T>
	{
		let endPoint = `${sessionUrl}/${dataType}`;
		let data = this.httpClient.get<T>(endPoint);
		return data;
	}

	///////////////////////		Orders 					////////////////////////

	getOrders()
	{
		this.httpClient.get<Order[]>(ordersUrl)
			.subscribe(data => this.orders = data);
	}


	// export class OrderConfirmation
	// {
	// 	constructor(public orderId: number,
	// 		public authCode: string,
	// 		public amount: number) { }
	// }

	createOrder(order: Order)
	{
		this.httpClient.post<OrderConfirmation>(ordersUrl,
			{
				name: order.name,
				address: order.address,
				payment: order.payment,
				products: order.products
			}).subscribe(data =>
			{
				//	Get the observable result.
				order.orderConfirmation = data;

				//	Clear the cart.
				order.cartService.clear();
				order.clear();
			});
	}

	shipOrder(order: Order)
	{
		this.httpClient.post(`${ordersUrl}/${order.orderId}`,
			{}).subscribe(() => this.getOrders());
	}


	//////////////////////		Read = HTTP GET			////////////////////////

	//  Get a product.
	getProduct(idProduct: number)
	{
		let endPoint = `${productsUrl}/${idProduct}`;
		this.httpClient.get<Product>(endPoint).subscribe(p => this.productData = p);
		console.log("Product Data Received.");
	}

	getProducts()
	{
		console.log("Products Requested.");
		this.signalRService.broadcastMessage("A message broadcast during getProducts.");

		let endPoint = `${productsUrl}?isRelatedRequired=${this.filter.related}`;
		if (this.filter.category)
		{
			endPoint += `&category=${this.filter.category}`;
		}
		if (this.filter.search)
		{
			endPoint += `&search=${this.filter.search}`;
		}

		//	Include categories as auxillary data.
		endPoint += "&isMetadata=true";
		//endPoint += "&isMetadata=false";

		//	Get the composite data.
		this.httpClient.get<ProductsMetadata>(endPoint)
			.subscribe(productsMetadata =>
			{
				this.products = productsMetadata.products;
				this.categories = productsMetadata.categories;
			});
		return this.products;
	}

	//  Get a supplier.
	getSupplier(idSupplier: number)
	{
		let endPoint = `${suppliersUrl}/${idSupplier}`;
		this.httpClient.get<Supplier>(endPoint).subscribe(s => this.supplierData = s);
		console.log("Supplier Data Received.");
	}

	getSuppliers()
	{
		console.log("Suppliers Requested.");
		let endPoint = `${suppliersUrl}`;

		this.httpClient.get<Supplier[]>(endPoint)
			.subscribe(suppliers => this.suppliers = suppliers);
	}

	///////////////		Create = HTTP POST		//////////////////////

	createProduct(product: Product)
	{
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
			.subscribe(id =>
			{
				//	The new primary key is returned.
				product.productId = id;

				//	push() method appends the given element(s) in the last of the array
				//	and returns the length of the new array.
				this.products.push(product);
			});
	}

	createProductAndSupplier(product: Product, supplier: Supplier)
	{
		//	Initialize the api buffer.
		let supplierData =
		{
			name: supplier.name,
			city: supplier.city,
			state: supplier.state
		};

		//	Post the foreign key object first.
		this.httpClient.post<number>(suppliersUrl, supplierData)
			.subscribe(id =>
			{
				supplier.supplierId = id;
				product.supplier = supplier;
				this.suppliers.push(supplier);
				if (product != null)
				{
					//	Post the parent after posting the foreign key object.
					this.createProduct(product);
				}
			});
	}

	///////////////		Replace = HTTP PUT		//////////////////////

	replaceProduct(product: Product)
	{
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
			.subscribe(() =>
			{
				//	Refresh the products property.
				this.getProducts();
			});
	}

	replaceSupplier(supplier: Supplier)
	{
		//	Initialize the api buffer.
		let supplierData =
		{
			name: supplier.name,
			city: supplier.city,
			state: supplier.state
		}

		//	HTTP PUT the object with the id attached to the URL.
		this.httpClient.put(suppliersUrl + `/${supplier.supplierId}`, supplierData)
			.subscribe(() =>
			{
				//	Refresh the suppliers property.
				//this.getSuppliers();
				this.getProducts();
			});
	}

	///////////////		Update = HTTP PATCH		//////////////////////

	updateProduct(id: number, changes: Map<string, any>)
	{

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

	deleteProduct(id: number)
	{

		//	Send the patch and update the repository.
		this.httpClient.delete(`${productsUrl}/${id}`)
			.subscribe(() => this.getProducts());
	}

	deleteSupplier(id: number)
	{

		//	Send the patch and update the repository.
		this.httpClient.delete(`${suppliersUrl}/${id}`)
			.subscribe(() =>
			{
				this.getProducts();
				this.getSuppliers()
			});
	}

	//	End of the class.
}
