
//  This is a feature module.
import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { FormsModule } from "@angular/forms";
import { Repository } from "../models/repository";
import { ProductAdminComponent } from './productAdmin.component';
import { OrderAdminComponent } from './orderAdmin.component';
import { OverviewComponent } from './overview.component';
import { AdminComponent } from './admin.component';

const routes: Routes = [
	{
		path: "", component: AdminComponent,
		children: [
			{
				path: "products", component: ProductAdminComponent
			},
			{
				path: "orders", component: OrderAdminComponent
			},
			{
				path: "overview", component: OverviewComponent
			},
			{
				path: "", component: OverviewComponent
			}
		]
	}
];


//  Repository is an injectable service.
@NgModule({
	imports: [
		RouterModule,
		FormsModule,
		RouterModule.forChild(routes)
	],
	providers: [
		Repository
	],
	declarations: [
		AdminComponent,
		OverviewComponent,
		ProductAdminComponent,
		OrderAdminComponent
	]
})
export class AdminModule { }
