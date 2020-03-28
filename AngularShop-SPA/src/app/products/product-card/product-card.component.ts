import { Component, OnInit, Input } from "@angular/core";
import { Product } from "src/app/_models/product";
import { AuthService } from "src/app/_services/auth.service";
import { AlertifyService } from "src/app/_services/alertify.service";
import { UserService } from "src/app/_services/user.service";

@Component({
  selector: "app-product-card",
  templateUrl: "./product-card.component.html",
  styleUrls: ["./product-card.component.css"]
})
export class ProductCardComponent implements OnInit {
  @Input() product: Product;
  constructor(
    private authService: AuthService,
    private userService: UserService,
    private alertify: AlertifyService
  ) {}

  ngOnInit() {}
  purchase(id: number) {
    this.userService
      .purchase(this.authService.decodedToken.nameid, id)
      .subscribe(
        () => {
          this.alertify.success("Purchase has been created ");
        },
        error => {
          this.alertify.error("Failed to create the purchase");
        }
      );
    //this.alertify.success("Purchase " + this.product.name + " successfully!");
  }
}
