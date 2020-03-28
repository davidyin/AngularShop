import { Injectable } from "@angular/core";
import { environment } from "../../environments/environment";
import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";
import { Observable } from "rxjs";
import { PaginatedResult } from "../_models/pagination";
import { map } from "rxjs/operators";
import { Product } from "../_models/product";

@Injectable({
  providedIn: "root"
})
export class ProductService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}
  getProducts(
    page?,
    itemsPerPage?,
    productParams?
  ): Observable<PaginatedResult<Product[]>> {
    const paginatedResult: PaginatedResult<Product[]> = new PaginatedResult<
      Product[]
    >();
    let params = new HttpParams();
    if (page != null && itemsPerPage != null) {
      params = params.append("pageNumber", page);
      params = params.append("pageSize", itemsPerPage);
    }

    if (productParams != null) {
      params = params.append("orderBy", productParams.orderBy);
    }

    return this.http
      .get<Product[]>(this.baseUrl + "products", {
        observe: "response",
        params
      })
      .pipe(
        map(response => {
          paginatedResult.result = response.body;
          if (response.headers.get("Pagination") != null) {
            paginatedResult.pagination = JSON.parse(
              response.headers.get("Pagination")
            );
          }
          return paginatedResult;
        })
      );
  }

  getProduct(id): Observable<Product> {
    return this.http.get<Product>(this.baseUrl + "products/" + id);
  }
}
