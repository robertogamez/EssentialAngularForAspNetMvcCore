import { Component, OnInit } from '@angular/core';
import { Repository } from './models/repository';
import { Product } from './models/product.model';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'Angular & ASP.NET Core MVC';
  dtOptions: DataTables.Settings = {};
  products: Product[];

  ngOnInit(): void {
    const that = this;

    this.dtOptions = {
      pagingType: 'full_numbers',
      pageLength: 2,
      serverSide: true,
      processing: true,
      ajax: (datatablesParameters: any, callback) => {
        that.repo.getProducts(datatablesParameters)
          .subscribe(response => {
            that.products = <Product[]>response.data;

            callback({
              recordsTotal: response.recordsTotal,
              recordsFiltered: response.recordsFiltered,
              data: []
            });

          })
      },
      columns: [
        { data: 'name' },
        { data: 'category' },
        { data: 'price' }
      ]
    };

  }

  constructor(
    private repo: Repository
  ) {

  }

  get product(): Product {
    return this.repo.product;
  }
}
