import { Component, OnInit } from "@angular/core";
import { Pagination, PaginatedResult } from "../_models/pagination";
import { AuthService } from "../_services/auth.service";
import { ProductService } from "../_services/product.service";
import { AlertifyService } from "../_services/alertify.service";
import { Product } from "../_models/product";
import { ActivatedRoute } from "@angular/router";

@Component({
  selector: "app-products",
  templateUrl: "./products.component.html",
  styleUrls: ["./products.component.css"]
})
export class ProductsComponent implements OnInit {
  products: Product[];
  productParams: any = {};
  pagination: Pagination;

  constructor(
    private productService: ProductService,
    private alertify: AlertifyService,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.products = data["products"].result;
      this.pagination = data["products"].pagination;
    });
    this.productParams.orderBy = "price";
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadProducts();
  }

  loadProducts() {
    this.productService
      .getProducts(
        this.pagination.currentPage,
        this.pagination.itemsPerPage,
        this.productParams
      )
      .subscribe(
        (res: PaginatedResult<Product[]>) => {
          this.products = res.result;
          this.pagination = res.pagination;
        },
        error => {
          this.alertify.error(error);
        }
      );
  }
}
