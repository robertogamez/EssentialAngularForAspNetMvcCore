import { Product } from './product.model';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable()
export class Repository {

  product: Product;

  constructor(private http: HttpClient) {
    this.getProduct(1);
  }

  getProduct(id: number) {
    this.http.get('/api/products/' + id).subscribe(response => this.product = response);
  }

}
