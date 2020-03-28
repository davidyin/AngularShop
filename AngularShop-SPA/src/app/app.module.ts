import { BrowserModule } from "@angular/platform-browser";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { NgModule, Pipe } from "@angular/core";
import { HttpClientModule } from "@angular/common/http";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";

import { AppComponent } from "./app.component";
import { AuthService } from "./_services/auth.service";
import { RegisterComponent } from "./register/register.component";
import { HomeComponent } from "./home/home.component";
import { ErrorInterceptorProvider } from "./_services/error.interceptor";

import {
  BsDropdownModule,
  ButtonsModule,
  PaginationModule
} from "ngx-bootstrap";

import { AlertifyService } from "./_services/alertify.service";
import { RouterModule } from "@angular/router";
import { JwtModule } from "@auth0/angular-jwt";
import { UserService } from "./_services/user.service";
import { AuthGuard } from "./_guards/auth.guard";

import { TimeagoPipe } from "./_pipes/timeago.pipe";
import { NavComponent } from "./nav/nav.component";
import { appRoutes } from "./routes";
import { PurchasesComponent } from "./purchases/purchases.component";
import { CustomersComponent } from "./customers/customers.component";
import { ProductsComponent } from "./products/products.component";
import { CustomerCardComponent } from "./customers/customer-card/customer-card.component";
import { ProductCardComponent } from "./products/product-card/product-card.component";
import { CustomerDetailResolver } from "./_resolvers/customer-detail.resolver";
import { ProductDetailComponent } from "./products/product-detail/product-detail.component";
import { ProductDetailResolver } from "./_resolvers/product_detail.resolver";
import { ProductListResolver } from "./_resolvers/product-list.resolver";
import { PurchasesResolver } from "./_resolvers/purchases.resolver";

export function tokenGetter() {
  return localStorage.getItem("token");
}

@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    RegisterComponent,
    HomeComponent,
    TimeagoPipe,
    PurchasesComponent,
    ProductsComponent,
    CustomersComponent,
    CustomerCardComponent,
    ProductCardComponent,
    ProductDetailComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    BsDropdownModule.forRoot(),
    PaginationModule.forRoot(),
    ButtonsModule,
    RouterModule.forRoot(appRoutes),
    JwtModule.forRoot({
      config: {
        tokenGetter,
        whitelistedDomains: ["localhost:5000"],
        blacklistedRoutes: ["localhost:5000/api/auth"]
      }
    })
  ],
  providers: [
    AuthService,
    ErrorInterceptorProvider,
    AlertifyService,
    UserService,
    AuthGuard,
    CustomerDetailResolver,
    ProductListResolver,
    ProductDetailResolver,
    PurchasesResolver
  ],
  bootstrap: [AppComponent]
})
export class AppModule {}
