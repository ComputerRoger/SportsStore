import { Product } from './product.model';

export class Repository {

  product: Product;

  constructor() {
    this.product = JSON.parse(document.getElementById("data").textContent);

    //  Test JSON.parse.
    //this.product = JSON.parse('{"productId":1,"name":"Kayak","category":"Watersports","description":"A boat for one persion","price":275.00,"supplier":null,"ratings":null}')

    //  Test product construction.
    //this.product = {
    //  productId: 0,
    //  price: 3.14,
    //  category: "The Category",
    //  name: "The Product Name",
    //  description: "The Product Description"
    //}
  }
}
