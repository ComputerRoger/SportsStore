import { Component } from '@angular/core';
import { Repository } from "./models/repository";
import { Product } from "./models/product.model";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'SportsStore';

    //Inject the model container.
  constructor(private repository: Repository) { }

  //  Properties.
  get product(): Product {
    return this.repository.product;
  }
}
