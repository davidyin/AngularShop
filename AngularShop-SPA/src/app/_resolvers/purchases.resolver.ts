import { Injectable } from "@angular/core";
import { Resolve, Router, ActivatedRouteSnapshot } from "@angular/router";
import { AlertifyService } from "../_services/alertify.service";
import { catchError } from "rxjs/operators";
import { Observable, of } from "rxjs";
import { Product } from "../_models/product";
import { ProductService } from "../_services/product.service";
import { Purchase } from "../_models/purchase";
import { UserService } from "../_services/user.service";
import { AuthService } from "../_services/auth.service";

@Injectable()
export class PurchasesResolver implements Resolve<Purchase[]> {
  pageNumber = 1;
  pageSize = 5;
  purchaseContainer = "";
  constructor(
    private userService: UserService,
    private authService: AuthService,
    private router: Router,
    private alertify: AlertifyService
  ) {}

  resolve(route: ActivatedRouteSnapshot): Observable<Purchase[]> {
    return this.userService
      .getPurchases(
        this.authService.decodedToken.nameid,
        this.pageNumber,
        this.pageSize,
        this.purchaseContainer
      )
      .pipe(
        catchError(error => {
          this.alertify.error("Problem retrieving purchases");
          this.router.navigate(["/home"]);
          return of(null);
        })
      );
  }
}
