import { Injectable } from "@angular/core";
import { Product } from "./product.model";
import { ProductSelection } from './ProductSelection';
import { Repository } from "./repository";

@Injectable()
export class CartService
{
	readonly cartKey = "cart";

	/////////////////	Properties		//////////////////////

	productSelections: ProductSelection[] = [];
	itemCount: number = 0;
	totalPrice: number = 0;

	constructor(private repository: Repository)
	{
		repository.getSessionData<ProductSelection[]>(this.cartKey)
			.subscribe(cartData =>
			{
				if (cartData != null)
				{
					cartData.forEach(item =>
					{
						//	Transform the data structure into a full instance with methods.
						let ps = new ProductSelection(this,
							item.productId,
							item.name,
							item.price,
							item.quantity);
						//	Push the instance into the collection.
						//	Two-way binding will now work because properties and methods exist.
						//	Pushing the bare structure fails to provide methods and properties.
						this.productSelections.push(ps);
					});

					this.update(false);
				}
			})
	}


	/////////////////	Methods		//////////////////////

	clear()
	{
		this.productSelections = [];
		this.update();
	}

	addProduct(product: Product)
	{
		//	Get the selected item.
		let productSelection = this.productSelections
			.find(selection => selection.productId == product.productId);
		if (productSelection)
		{
			//	The selected item was previously made.
			productSelection.quantity++;
		}
		else
		{
			//	The selected item will be new.
			let quantityValue = 1;
			this.productSelections.push(new ProductSelection(this,
				product.productId,
				product.name,
				product.price,
				quantityValue));
		}
		this.update();
	}

	update(isPersistData: boolean = true)
	{
		//	Recompute the item count property.
		this.itemCount = this.productSelections.map(productSelection => productSelection.quantity)
			.reduce((previous, current) => previous + current, 0);
		//	Recompute the total price property.
		this.totalPrice = this.productSelections
			.map(productSelection => productSelection.price * productSelection.quantity)
			.reduce((previous, current) => previous + current, 0);
		if (isPersistData)
		{
			this.repository.storeSessionData(this.cartKey,
				this.productSelections
					.map(s =>
					{
						return {
							productId: s.productId,
							name: s.name,
							price: s.price,
							quantity: s.quantity
						}
					}
					));
		}
	}

	//	Assign the quantity of the item.
	updateQuantity(productId: number, quantity: number)
	{
		let productSelection = this.productSelections
			.find(productSelection => productSelection.productId == productId);
		if (productSelection)
		{
			//	The product item was found in the cart.
			if (quantity >= 0)
			{
				productSelection.quantity = quantity;
			}
			else
			{
				productSelection.quantity = 0;
			}
		}
		else
		{
			//	The product is not yet in the cart.
			//	The quantity cannot be assigned to a nonexistent item.
		}
		this.update();
	}
}
