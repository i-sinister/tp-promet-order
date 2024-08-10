import { Component, Inject, OnInit } from '@angular/core';
import { DatePipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { FormGroup, FormControl } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import * as moment from 'moment';

import { EventsService } from '../events.service';
import {
  CreateOrderRequest, UpdateOrderRequest, OrderDetails, OrderItem,
  ProviderInfo, ProviderListResponse
} from '../models/order';

@Component({
  selector: '[pos-order-details]',
  templateUrl: './order-details.component.html',
  styleUrls: ['./order-details.component.css']
})
export class OrderDetailsComponent implements OnInit {
  public orderID: number;
  public formOrder: FormGroup;
  public formItem: FormGroup;
  public items: OrderItem[];
  public is_loading: boolean = true;
  public providers: ProviderInfo[];

  constructor(
    private events: EventsService,
    private http: HttpClient,
    private datepipe: DatePipe,
    @Inject('BASE_URL') private baseUrl: string,
    private router: Router,
    route: ActivatedRoute
  ) {
    this.orderID = parseInt(route.snapshot.paramMap.get('orderID') || '0');
    this.formOrder = new FormGroup({
      number: new FormControl(''),
      date: new FormControl(datepipe.transform(moment().startOf('day').toDate(), 'yyyy-MM-dd')),
      providerID: new FormControl('')
    })
    this.formItem = new FormGroup({
      name: new FormControl(''),
      quantity: new FormControl(''),
      unit: new FormControl('')
    })
    this.items = [];
    this.providers = [];
  }

  ngOnInit(): void {
    this.events.add(`loading providers`);
    this.http.get<ProviderListResponse>(`${this.baseUrl}api/providers`)
      .subscribe({
        next: result => {
          this.events.add(`providers loaded`);
          this.providers = result.items;
        },
        error: _ => {
          this.events.add(`loading providers failed`);
          this.providers = [];
        }
      });

      let orderID = this.orderID;
      if (0 == orderID){
        return;
      }

      this.is_loading = true;
      this.events.add(`loading order ID ${orderID}`);
      this.http.get<OrderDetails>(`${this.baseUrl}api/orders/${orderID}`)
        .subscribe({
          next: result => {
            this.is_loading = false;
            this.events.add(`order ID ${orderID} loaded`);
            let dto = {
              number: result.number,
              date: this.datepipe.transform(result.date, 'yyyy-MM-dd'),
              providerID: result.providerID
            };
            this.formOrder.setValue(dto);
            this.items.push(...result.items);
          },
          error: result => {
            this.is_loading = false;
            this.events.add(`loading order ID ${orderID} failed`);
            if (404 == result.error.status) {
              this.router.navigate(['/order/not-found'], { state: { orderID: orderID } });
            }
          }
        });
  }

  public removeItem(item: OrderItem) {
    const index = this.items.indexOf(item, 0);
    if (-1 < index) {
      this.events.add(`removing item #${index}`);
      this.items.splice(index, 1);
    }
  }

  public addItem() {
    let form = this.formItem.value;
    let newItem: OrderItem = {
      id: 0,
      orderID: this.orderID,
      name: form.name,
      quantity: form.quantity,
      unit: form.unit
    };

    this.items.push(newItem);
    this.formItem.reset();
  }

  public saveOrder() {
    let value = this.formOrder.value;
    if (0 == this.orderID) {
      this.createOrder(value);
    } else {
      this.updateOrder(value);
    }
  }

  public removeOrder() {
    if (!confirm(`Are you sure you want to delete order '${this.formOrder.value.number}'`)) {
      return;
    }

    let orderID = this.orderID;
    this.events.add(`reming order ${orderID}`);
    return this.http
      .delete(`${this.baseUrl}api/orders/${orderID}`)
      .subscribe({
        next: _ => {
          this.events.add(`orders ${orderID} removed`);
          this.router.navigate(['/'])
        },
        error: _ => {
          this.events.add(`orders ${orderID} could not be removed`);
        }
      });
  }

  private createOrder(value: any) {
    let order: CreateOrderRequest = value;
    order.items = this.items;
    this.http.post<CreateOrderRequest>(`${this.baseUrl}api/orders`, order)
      .subscribe({
        next: orderID => {
          this.events.add(`order #${orderID} created`);
          alert(`Order ${order.number}(#${orderID}) created`);
          this.router.navigate(['/'])
        },
        error: _ => {
          this.events.add(`order ${order.number} could not be created`);
          alert(`Order ${order.number} could not be created`);
          // TODO: handle validation errors
        }
      });
  }

  private updateOrder(value: OrderDetails) {
    let order: UpdateOrderRequest = value;
    order.items = this.items;
    let orderID = this.orderID;
    this.http.post<CreateOrderRequest>(`${this.baseUrl}api/orders/${orderID}`, order)
      .subscribe({
        next: _ => {
          this.events.add(`order ${orderID} updated`);
          alert(`Order ${order.number}(#${orderID}) updated`);
          this.router.navigate(['/'])
        },
        error: _ => {
          this.events.add(`order ${order.number}(#${orderID}) could not be created`);
          alert(`Order ${order.number}(#${orderID}) could not be updated`);
          // TODO: handle validation errors
        }
      });
  }
}
