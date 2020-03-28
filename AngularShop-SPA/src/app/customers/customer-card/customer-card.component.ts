import { Component, OnInit, Input } from "@angular/core";
import { User } from "src/app/_models/user";

@Component({
  selector: "app-customer-card",
  templateUrl: "./customer-card.component.html",
  styleUrls: ["./customer-card.component.css"]
})
export class CustomerCardComponent implements OnInit {
  @Input() customer: User;

  constructor() {}

  ngOnInit() {}
}
