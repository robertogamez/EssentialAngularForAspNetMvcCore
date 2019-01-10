import { Product } from './product.model';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable()
export class Repository {

  product: Product;
  products: Product[];

  constructor(private http: HttpClient) {
    this.getProducts(true);
  }

  getProduct(id: number) {
    this.http.get('/api/products/' + id).subscribe(response => this.product = response);
  }

  getProducts(related = false) {
    this.http.get('/api/products?related=' + related).subscribe(response => this.products = <Product[]>response);
  }

}
