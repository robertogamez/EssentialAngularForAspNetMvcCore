import { Product } from './product.model';

export class Repository {

  product: Product;

  constructor() {
    this.product = new Product(2, "Coca-cola", "Refrescos", "", 20.50, null, null);
  }

}
