import { Component, OnInit } from "@angular/core";
import { Purchase } from "../_models/purchase";
import { Pagination, PaginatedResult } from "../_models/pagination";
import { ActivatedRoute } from "@angular/router";
import { AlertifyService } from "../_services/alertify.service";
import { UserService } from "../_services/user.service";
import { AuthService } from "../_services/auth.service";

@Component({
  selector: "app-purchases",
  templateUrl: "./purchases.component.html",
  styleUrls: ["./purchases.component.css"]
})
export class PurchasesComponent implements OnInit {
  purchases: Purchase[];
  pagination: Pagination;

  purchaseContainer = "";

  constructor(
    private userService: UserService,
    private authService: AuthService,
    private route: ActivatedRoute,
    private alertify: AlertifyService
  ) {}

  ngOnInit(): void {
    this.route.data.subscribe(data => {
      this.purchases = data["purchases"].result;
      this.pagination = data["purchases"].pagination;
    });
  }

  loadPurchases() {
    this.userService
      .getPurchases(
        this.authService.decodedToken.nameid,
        this.pagination.currentPage,
        this.pagination.itemsPerPage,
        this.purchaseContainer
      )
      .subscribe(
        (res: PaginatedResult<Purchase[]>) => {
          this.purchases = res.result;
          this.pagination = res.pagination;
        },
        error => {
          this.alertify.error(error);
        }
      );
  }

  cancelPurchase(id: number) {
    this.alertify.confirm("Are you sure want to cancel this purchase", () => {
      this.userService
        .cancelPurchase(id, this.authService.decodedToken.nameid)
        .subscribe(
          () => {
            this.purchases.splice(
              this.purchases.findIndex(m => m.id === id),
              1
            );
            this.alertify.success("Purchase has been deleted ");
          },
          error => {
            this.alertify.error("Failed to delete the purchase");
          }
        );
    });
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadPurchases();
  }
}
