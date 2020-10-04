import { Component, OnInit } from '@angular/core';
import { Repository } from "../models/repository";
import { Product } from "../models/product.model";
import { Supplier } from "../models/supplier.model";
import { Order } from "../models/order.model";

@Component({
	selector: 'productAdmin',
	templateUrl: 'productAdmin.component.html'
})
export class ProductAdminComponent implements OnInit
{
	tableMode: boolean = true;

	constructor(private repository: Repository) { }

	ngOnInit(): void {

	}

	//////////////////////////	  Properties.		////////////////////

	get product(): Product {
		return this.repository.productData;
	}

	get products(): Product[] {
		return this.repository.products;
	}

	//////////////////////////	  Methods.		////////////////////

	///////////////////////////		Event handlers.		///////////////////

	selectProduct(id: number) {
		this.repository.getProduct(id);
	}

	saveProduct() {
		if (this.repository.productData.productId == null) {
			this.repository.createProduct(this.repository.productData);
		}
		else {
			this.repository.replaceProduct(this.repository.productData);
		}
		this.clearProduct();
		this.tableMode = true;
	}

	deleteProduct(id: number) {
		this.repository.deleteProduct(id);
	}

	clearProduct() {
		this.repository.productData = new Product();
		this.tableMode = true;
	}

}
