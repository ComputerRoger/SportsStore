import { Component } from '@angular/core';
import { Input } from '@angular/core';	//	This component will be a child that will inherit an Input property describing its parent.
import { Repository } from "../models/repository";
import { Product } from "../models/product.model";
import { Supplier } from "../models/supplier.model";

@Component({
	selector: 'store-ratings',
	templateUrl: 'ratings.component.html'
})
export class RatingsComponent
{
	STAR_SIZE = 5;

	//Inject the model container.
	constructor(private repository: Repository) { }

	//////////////////////////	  Properties.		////////////////////
	@Input()
	product: Product;

	get stars(): boolean[]
	{
		if (this.product != null &&
			this.product.ratings != null)
		{
			//	Transform rating objects to stars count, then "reduce" by applying
			//	the function to each item emitted by an Observable, sequentially, and emit the final value
			let sumOfStars = this.product.ratings.map(rating => rating.stars)
				.reduce((previous, current) => previous + current, 0);
			let averageStars = Math.round(sumOfStars / this.product.ratings.length);
			let starRating = Array(5).fill(false).map((value, index) =>
			{
				//	The Array would have "any" type, but the type is explicitly set by the return of the lambda.
				//	The type of each element of the array is provided by the lambda "return" values.
				return index < averageStars;
			});
			return starRating;
		}
		else
		{
			return [];
		}
	}

	//////////////////////////	  Methods.		////////////////////

	///////////////////////////		Event handlers.		///////////////////

}
