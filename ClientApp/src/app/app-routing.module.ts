import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
//	Import the components associated with routes.
// import { ProductTableComponent } from "./structure/productTable.component";
import { CartDetailComponent } from './store/cart-detail.component';
import { ProductSelectionComponent } from './store/productSelection.component';

//	Define the routes supported by the Angular application.
//	Routes associate path patterns with components.
//	Note: segment variables are denoted with the ':' character.
const routes: Routes = [
	// { path: "table", component: ProductTableComponent },
	// { path: "detail", component: ProductDetailComponent },
	// { path: "detail/:id", component: ProductDetailComponent },
	// { path: "", component: ProductTableComponent }
	// { path: "store/:category", component: ProductSelectionComponent },

	{ path: "cart", component: CartDetailComponent },
	{ path: "store/:category/:page", component: ProductSelectionComponent },
	{ path: "store/:categoryOrPage", component: ProductSelectionComponent },
	{ path: "store", component: ProductSelectionComponent },
	{ path: "", redirectTo: "/store", pathMatch: "full" }
];

@NgModule({
	imports: [RouterModule.forRoot(routes)],
	exports: [RouterModule]
})
export class AppRoutingModule { }
