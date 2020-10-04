import { Component } from '@angular/core';
import { Repository } from "./models/repository";
import { Product } from "./models/product.model";
import { Supplier } from "./models/supplier.model";
import { ErrorHandlerService } from "./errorHandler.service";

@Component({
	selector: 'app-root',
	templateUrl: './app.component.html',
	styleUrls: ['./app.component.css']
})
export class AppComponent
{
	title = 'SportsStore';
	private lastError: string[];

	//Inject the model container.
	constructor(private repository: Repository, errorService: ErrorHandlerService) {
		errorService.errors.subscribe(error => {
			this.lastError = error;
		})
	}

	//////////////////////////	  Properties.		////////////////////

	get error(): string[] {
		return this.lastError;
	}

	get product(): Product
	{
		return this.repository.productData;
	}
	get products(): Product[]
	{
		return this.repository.products;
	}

	//////////////////////////	  Event handlers.		////////////////////

	clearError() {
		this.lastError = null;
	}

	///////////////////////////		Event handlers to initiate POSTs.		///////////////////

	createProduct()
	{
		//	Test with a legitimate value.
		let ratings = this.repository.products[0].ratings;
		let product = new Product(
			0,
			"X-Ray Scuba Mask",
			"Watersports",
			"See what the fish are hiding",
			49.99,
			ratings,
			this.repository.products[0].supplier);

		//	Send the new product to the webservice.
		this.repository.createProduct(product);
	}

	createProductAndSupplier()
	{
		//	Test with a legitimate value.
		let ratings = this.repository.products[0].ratings;
		let supplier = new Supplier(0, "Rocket Shoe Corp", "Boston", "MA");
		let product = new Product(0, "Rocket-Powered Shoes", "Running", "Set a new record.", 100.00, ratings, supplier);
		this.repository.createProductAndSupplier(product, supplier);
	}

	///////////////////////////		Event handlers to initiate PUTs.		///////////////////

	replaceProduct()
	{
		//	Test with a legitimate value.
		let product = this.repository.products[0];
		product.name = "6 Modified Product";
		product.category = "Modified Category";
		this.repository.replaceProduct(product);
	}

	replaceSupplier()
	{
		//	Test with a legitimate value.
		let supplier = new Supplier(3, "6 Modified Supplier", "New York", "NY");
		this.repository.replaceSupplier(supplier);
	}

	///////////////////////////		Event handlers to initiate PATCHs.		///////////////////

	updateProduct() {
		//	Test with a legitimate value.
		let changes = new Map<string, any>();
		changes.set("name", "Green Kayak");
		changes.set("supplier", null);
		this.repository.updateProduct(1, changes);
	}

	///////////////////////////		Event handlers to initiate DELETEs.		///////////////////

	deleteProduct() {
		//	Test with a legitimate value.
		this.repository.deleteProduct(1);
	}

	deleteSupplier() {
		//	Test with a legitimate value.
		this.repository.deleteSupplier(2);	}
}
