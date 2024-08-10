import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-order-not-found',
  templateUrl: './order-not-found.component.html',
  styleUrls: ['./order-not-found.component.css']
})
export class OrderNotFoundComponent {
  public orderID: number | null;
  constructor(router: Router)
  {
    this.orderID = router.getCurrentNavigation()?.extras?.state?.orderID;
  }
}
