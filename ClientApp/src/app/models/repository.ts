import { Injectable } from "@angular/core";
import { HttpClient } from "@Angular/common/http";

import { Product } from './product.model';
import { Supplier } from './supplier.model';
import { Filter, Pagination } from './configClasses.repository';
import { Observable } from "rxjs";

const productsUrl = "/api/productvalues";
const suppliersUrl = "/api/suppliervalues";
const sessionUrl = "/api/session";

//	The client and server keys of this type must match.
type ProductsMetadata =
	{
		products: Product[],
		categories: string[];
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

	constructor(private httpClient: HttpClient)
	{
		//	Set the filters.
		//this.filter.category = "soccer";
		this.filter.related = true;

		//	Angular emits a routing event when the application first starts,
		//	which means that the HTTP request of the repository constructor can be removed.

		//	Get all the products.
		// this.getProducts(true);
		//this.getProducts();

		//	During development, only retrieve a single product.
		// this.getProduct(2);
		//this.product = JSON.parse(document.getElementById("data").textContent);

		//  Test JSON.parse.
		//this.product = JSON.parse('{"productId":1,"name":"Kayak","category":"Watersports","description":"A boat for one persion","price":275.00,"supplier":null,"ratings":null}')

		//  Test product construction.
		//this.product = {
		//  productId: 0,
		//  price: 3.14,
		//  category: "The Category",
		//  name: "The Product Name",
		//  description: "The Product Description"
		//}
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
