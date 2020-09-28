
//  This is a feature module.
import { NgModule } from "@angular/core";
import { Repository } from "./repository";
import { HttpClientModule } from "@angular/common/http";
import { NavigationService } from "./navigation.service";
import { CartService } from "./cart.service";

//  Repository is an injectable service.
@NgModule({
	imports: [HttpClientModule],
	providers: [
		Repository,
		NavigationService,
		CartService
	]
})
export class ModelModule { }
