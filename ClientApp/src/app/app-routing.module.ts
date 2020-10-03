import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
//	Import the components associated with routes.
// import { ProductTableComponent } from "./structure/productTable.component";
import { CartDetailComponent } from './store/cart-detail.component';
import { ProductSelectionComponent } from './store/productSelection.component';

import { CheckoutDetailsComponent } from './store/checkout/checkoutDetails.component';
import { CheckoutPaymentComponent } from './store/checkout/checkoutPayment.component';
import { CheckoutSummaryComponent } from './store/checkout/checkoutSummary.component';
import { OrderConfirmationComponent } from './store/checkout/orderConfirmation.component';

//	Define the routes supported by the Angular application.
//	Routes associate path patterns with components.
//	Note: segment variables are denoted with the ':' character.
const routes: Routes = [
	{
		path: "admin", loadChildren: () => import("./admin/admin.module").then(module => module.AdminModule)
	},
	{ path: "checkout/step1", component: CheckoutDetailsComponent },
	{ path: "checkout/step2", component: CheckoutPaymentComponent },
	{ path: "checkout/step3", component: CheckoutSummaryComponent },
	{ path: "checkout/confirmation", component: OrderConfirmationComponent },
	{ path: "checkout", redirectTo: "/checkout/step1", pathMatch: "full" },

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
