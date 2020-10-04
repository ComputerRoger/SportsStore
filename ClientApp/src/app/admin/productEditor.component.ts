import { Component, OnInit } from '@angular/core';
import { Repository } from "../models/repository";
import { Product } from "../models/product.model";
import { Supplier } from "../models/supplier.model";
import { Order } from "../models/order.model";


@Component({
	selector: 'admin-product-editor',
	templateUrl: 'productEditor.component.html'
})
export class ProductEditorComponent implements OnInit {
	constructor(private repository: Repository) {
	}

	ngOnInit(): void {
	}

	//////////////////////////	  Properties.		////////////////////

	//	Provide convenient access.

	//	Note:  The repository product is assigned by other components.
	get product(): Product {
		return this.repository.productData;
	}

	get suppliers(): Supplier[] {
		return this.repository.suppliers;
	}

	//////////////////////////	  Methods.		////////////////////

	//	The SELECT element uses a function to determine which OPTION is the current selection.
	compareSuppliers(supplierLeft: Supplier, supplierRight: Supplier) {
		let isSameSupplier = false;

		if (supplierLeft && supplierRight) {
			isSameSupplier = supplierLeft.name == supplierRight.name;
		}
		return isSameSupplier;
	}

	///////////////////////////		Event handlers.		///////////////////

}
