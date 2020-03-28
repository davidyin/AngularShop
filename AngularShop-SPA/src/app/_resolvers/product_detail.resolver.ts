import { Injectable, Inject } from "@angular/core";
import { Resolve, Router, ActivatedRouteSnapshot } from "@angular/router";
import { AlertifyService } from "../_services/alertify.service";
import { AuthService } from "../_services/auth.service";
import { Product } from "../_models/product";
import { UserService } from "../_services/user.service";
import { Observable, of } from "rxjs";
import { ProductService } from "../_services/product.service";
import { catchError } from "rxjs/operators";

@Injectable()
export class ProductDetailResolver implements Resolve<Product> {
  constructor(
    private productService: ProductService,
    private route: Router,
    private alertify: AlertifyService
  ) {}

  resolve(route: ActivatedRouteSnapshot): Observable<Product> {
    return this.productService.getProduct(route.params[`id`]).pipe(
      catchError(error => {
        this.alertify.error("Problem retrieving data");
        this.route.navigate(["/products"]);
        return of(null);
      })
    );
  }
}
