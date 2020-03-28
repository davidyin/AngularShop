import { Component, OnInit } from "@angular/core";
import { UserService } from "../_services/user.service";
import { AlertifyService } from "../_services/alertify.service";
import { User } from "../_models/user";

@Component({
  selector: "app-customers",
  templateUrl: "./customers.component.html",
  styleUrls: ["./customers.component.css"]
})
export class CustomersComponent implements OnInit {
  users: User[];
  constructor(
    private userService: UserService,
    private alertify: AlertifyService
  ) {}

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers() {
    this.userService.getUsers().subscribe(
      (users: User[]) => {
        this.users = users;
        console.log(users);
      },
      error => {
        this.alertify.error(error);
      }
    );
  }
}
