import { Product } from './product.model';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { DataTablesResponse } from './dataTablesResponse';
import { Observable } from 'rxjs';

@Injectable()
export class Repository {

  product: Product;
  products: Product[];

  constructor(private http: HttpClient) {
    
  }

  getProduct(id: number) {
    this.http.get('/api/products/' + id).subscribe(response => this.product = response);
  }

  getProducts(datatablesParameters: any, related = false): Observable<DataTablesResponse> {
    return this.http
      .post<DataTablesResponse>('/api/products/?related=' + related, datatablesParameters, {});
  }

}
