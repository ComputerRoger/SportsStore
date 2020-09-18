
//  This is a feature module.
import { NgModule } from "@angular/core";
import { Repository } from "./repository";
import { HttpClientModule } from "@angular/common/http";

//  Repository is an injectable service.
@NgModule({
	imports: [HttpClientModule],
	providers: [Repository]
})
export class ModelModule { }
