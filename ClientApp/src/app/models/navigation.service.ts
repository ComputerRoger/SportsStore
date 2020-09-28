import { Component } from '@angular/core';
import { Injectable } from "@angular/core";
import { Router, ActivatedRoute, NavigationEnd } from "@angular/router";
import { Repository } from "../models/repository";
import { filter } from "rxjs/operators";

//	This is a service so it needs to be registered.
//	This service is registered in model.module.ts.
@Injectable()
export class NavigationService {

	//Inject the model container.
	constructor(private repository: Repository, private router: Router, private activedRoute: ActivatedRoute) {
		//	Subscribe to router events.
		//	Angular emits a routing event when the application first starts,
		//	which means that the HTTP request of the repository constructor can be removed.
		router.events
			.pipe(filter(event => event instanceof NavigationEnd))
			.subscribe(foo => this.handleNavigationChange());
	}

	//////////////////////////	  Properties.		////////////////////

	get categories(): string[] {
		return this.repository.categories;
	}
	get currentCategory(): string {
		return this.repository.filter.category || "";
	}
	set currentCategory(newCategory: string) {
		this.router.navigateByUrl(`/store/${(newCategory || "").toLowerCase()}`);
	}

	//	Provide properties related to navigation with pagination.
	get currentPage(): number {
		return this.repository.pagination.currentPage;
	}
	set currentPage(pageNumber: number) {
		if (this.currentCategory === "") {
			this.router.navigateByUrl(`/store/${pageNumber}`);
		}
		else {
			this.router.navigateByUrl(`/store/${this.currentCategory}/${pageNumber}`);
		}
	}

	get productsPerPage(): number {
		return this.repository.pagination.productsPerPage;
	}

	get productCount(): number {
		return (this.repository.products || []).length;
	}


	///////////////////////////		Methods.		///////////////////

	private handleNavigationChange() {
		let activatedRouteFirstChild = this.activedRoute.firstChild.snapshot;
		if (activatedRouteFirstChild.url.length > 0 &&
			activatedRouteFirstChild.url[0].path === "store") {
			//	A store route.
			let activatedCategoryOrPage = activatedRouteFirstChild.params["categoryOrPage"];
			if (activatedCategoryOrPage !== undefined) {
				let pageValue = Number.parseInt(activatedCategoryOrPage);
				if (!Number.isNaN(pageValue)) {
					//	Page value is a number.
					this.repository.filter.category = "";
					this.repository.pagination.currentPage = pageValue;
				}
				else {
					//	Page value is not a number.
					this.repository.filter.category = activatedCategoryOrPage;
					this.repository.pagination.currentPage = 1;
				}
			}
			else {
				//	Category with Page.
				let category = activatedRouteFirstChild.params["category"];
				this.repository.filter.category = category || "";
				this.repository.pagination.currentPage = Number.parseInt(activatedRouteFirstChild.params["page"]) || 1
			}
			this.repository.getProducts();
			console.log("Product Data Received.");
		}
		else {
			//	Not a store route.
		}
	}
}
