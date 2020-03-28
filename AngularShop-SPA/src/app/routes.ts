import { Routes } from "@angular/router";
import { HomeComponent } from "./home/home.component";
import { AuthGuard } from "./_guards/auth.guard";
import { ProductsComponent } from "./products/products.component";
import { ProductDetailResolver } from "./_resolvers/product_detail.resolver";
import { PurchasesComponent } from "./purchases/purchases.component";
import { CustomersComponent } from "./customers/customers.component";
import { ProductDetailComponent } from "./products/product-detail/product-detail.component";
import { ProductListResolver } from "./_resolvers/product-list.resolver";
import { PurchasesResolver } from "./_resolvers/purchases.resolver";

export const appRoutes: Routes = [
  { path: "", component: HomeComponent },

  {
    path: "products",
    component: ProductsComponent,
    resolve: { products: ProductListResolver }
  },
  {
    path: "products/:id",
    component: ProductDetailComponent,
    resolve: { product: ProductDetailResolver }
  },
  {
    path: "",
    runGuardsAndResolvers: "always",
    canActivate: [AuthGuard],
    children: [
      {
        path: "purchases",
        component: PurchasesComponent,
        resolve: { purchases: PurchasesResolver }
      },
      {
        path: "customers",
        component: CustomersComponent
      }
    ]
  },
  { path: "**", redirectTo: "", pathMatch: "full" }
];
