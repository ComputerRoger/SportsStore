import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
//	Import the components associated with routes.
import { ProductTableComponent } from "./structure/productTable.component";
import { ProductDetailComponent } from './structure/productDetail.component';

//	Define the routes supported by the Angular application.
//	Routes associate path patterns with components.
//	Note: segment variables are denoted with the ':' character.
const routes: Routes = [
	{ path: "table", component: ProductTableComponent },
	{ path: "detail", component: ProductDetailComponent },
	{ path: "detail/:id", component: ProductDetailComponent },
	{ path: "", component: ProductTableComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
