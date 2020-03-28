import { Component, OnInit } from "@angular/core";
import { AlertifyService } from "src/app/_services/alertify.service";
import { ActivatedRoute } from "@angular/router";
import { Product } from "src/app/_models/product";
import { UserService } from "src/app/_services/user.service";
import { AuthService } from "src/app/_services/auth.service";

@Component({
  selector: "app-product-detail",
  templateUrl: "./product-detail.component.html",
  styleUrls: ["./product-detail.component.css"]
})
export class ProductDetailComponent implements OnInit {
  product: Product;
  constructor(
    private alertify: AlertifyService,
    private authService: AuthService,
    private userService: UserService,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.product = data["product"];
    });
  }

  purchase(id: number) {
    this.userService
      .purchase(this.authService.decodedToken.nameid, id)
      .subscribe(
        () => {
          this.alertify.success(
            "Purchase " + this.product.name + " successfully!"
          );
        },
        error => {
          this.alertify.error("Failed to create the purchase");
        }
      );
  }
}
