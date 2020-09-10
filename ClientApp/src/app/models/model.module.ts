
//  This is a feature module.
import { NgModule } from "@angular/core";
import { Repository } from "./repository";

//  Repository is an injectable service.
@NgModule({
  providers: [Repository]
})
export class ModelModule {}
