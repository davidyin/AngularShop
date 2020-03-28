import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment";
import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";
import { Observable } from "rxjs";
import { User } from "../_models/user";
import { PaginatedResult } from "../_models/pagination";
import { map } from "rxjs/operators";
import { Purchase } from "../_models/purchase";

@Injectable({
  providedIn: "root"
})
export class UserService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(this.baseUrl + "users");
  }
  getUser(id): Observable<User> {
    return this.http.get<User>(this.baseUrl + "users/" + id);
  }

  purchase(id: number, productId: number) {
    console.log("purchase :::: " + productId);

    return this.http.post(this.baseUrl + "users/" + id + "/purchases/create", {
      productId
    });
  }

  getPurchases(id: number, page?, itemsPerPage?, purchaseContainer?) {
    const paginatedResult: PaginatedResult<Purchase[]> = new PaginatedResult<
      Purchase[]
    >();
    let params = new HttpParams();
    params = params.append("PurchaseContainer", purchaseContainer);

    if (page != null && itemsPerPage != null) {
      params = params.append("pageNumber", page);
      params = params.append("pageSize", itemsPerPage);
    }

    return this.http
      .get<Purchase[]>(this.baseUrl + "users/" + id + "/purchases", {
        observe: "response",
        params
      })
      .pipe(
        map(response => {
          paginatedResult.result = response.body;

          if (response.headers.get("Pagination") !== null) {
            paginatedResult.pagination = JSON.parse(
              response.headers.get("Pagination")
            );
          }
          return paginatedResult;
        })
      );
  }

  cancelPurchase(id: number, userId: number) {
    return this.http.post(
      this.baseUrl + "users/" + userId + "/purchases/" + id,
      {}
    );
  }
}
