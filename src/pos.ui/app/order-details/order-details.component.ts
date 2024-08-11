import { Component, Inject, Injectable, OnInit } from '@angular/core';
import { DatePipe } from '@angular/common';
import { HttpClient, HttpParams } from '@angular/common/http';
import { FormGroup, FormControl, ValidatorFn, Validators, ValidationErrors, AbstractControl, AsyncValidator } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import * as moment from 'moment';

import { EventsService } from '../events.service';
import {
  CreateOrderRequest, UpdateOrderRequest, OrderDetails, OrderItem,
  ProviderInfo, ProviderListResponse, ExistsResponse
} from '../models/order';
import { Observable, catchError, first, map, of, tap } from 'rxjs';

@Component({
  selector: '[pos-order-details]',
  templateUrl: './order-details.component.html',
  styleUrls: ['./order-details.component.css']
})
export class OrderDetailsComponent implements OnInit {
  public orderID: number;
  public formOrder: FormGroup;
  public number: FormControl;
  public date: FormControl;
  public providerID: FormControl;
  public formItem: FormGroup;
  public itemName: FormControl;
  public itemQuantity: FormControl;
  public itemUnit: FormControl;
  public items: OrderItem[];
  public is_loading: boolean = true;
  public providers: ProviderInfo[];

  constructor(
    private events: EventsService,
    private http: HttpClient,
    private datepipe: DatePipe,
    @Inject('BASE_URL') private baseUrl: string,
    private router: Router,
    route: ActivatedRoute,
    orderNumberUniqueValidator: OrderNumberIsUniqueValidator,
    providerExistsValidator: ProviderExistsValidator
  ) {
    this.items = [];
    this.orderID = parseInt(route.snapshot.paramMap.get('orderID') || '0');
    orderNumberUniqueValidator.setOrderID(this.orderID);
    this.number = new FormControl('', {
      validators:[
        Validators.required,
        Validators.minLength(1),
        Validators.maxLength(128),
        Validators.pattern('^[a-zA-Z0-9 \-_]+$'),
        oneOfValidator(this.items, item => item.name),
      ],
      asyncValidators: [
        orderNumberUniqueValidator.validate.bind(orderNumberUniqueValidator),
      ]
    });
    this.date = new FormControl(datepipe.transform(moment().startOf('day').toDate(), 'yyyy-MM-dd'));
    this.providerID = new FormControl('', {
      validators: [
        Validators.required,
      ],
      asyncValidators: [
        providerExistsValidator.validate.bind(providerExistsValidator)
      ]
    })
    this.formOrder = new FormGroup({number: this.number, date: this.date, providerID: this.providerID, })
    this.itemName = new FormControl('', [
      Validators.required,
      Validators.minLength(1),
      Validators.maxLength(128),
      Validators.pattern('^[a-zA-Z0-9 \-_]+$'),
      valuesMatchValidator(this.number),
      oneOfValidator(this.items, item => item.name),
    ]);
    this.itemQuantity = new FormControl('1'),
    this.itemUnit = new FormControl('item')
    this.formItem = new FormGroup({name: this.itemName, quantity: this.itemQuantity, unit: this.itemUnit});
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

  public isInvalid(control: FormControl): boolean {
    return control.invalid && (control.dirty || control.touched);
  }

  public isInvalidGroup(group: FormGroup): boolean {
    return Object.values(group.controls).some(e => { return (e instanceof FormControl) && this.isInvalid(e as FormControl); });
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

function str_cmp(first: any, second: any): boolean {
    if (!first || !second || 'string' != typeof first || 'string' != typeof second) {
      return false;
    }

    // `localCompare` does not work in firefox, so use toLowerCase comparison
    return first.toLowerCase() == second.toLowerCase();
}

function oneOfValidator<T>(items: T[], projection: (item: T) => string): ValidatorFn {
  return (control: AbstractControl<any, any>): ValidationErrors | null => {
    return items.some(i => { return str_cmp(control.value, projection(i)); })
      ? { oneOf: { value: control.value } }
      : null;
  };
}

function valuesMatchValidator(reference: FormControl): ValidatorFn {
  return (control: AbstractControl<any, any>): ValidationErrors | null => {
    return str_cmp(reference.value, control.value)
      ? { valuesMatch: { value: control.value } }
      : null;
  };
}

class ExistsValidatorBase {
  constructor(
    private baseUrl: string,
    private resource: string,
    private events: EventsService,
    private http: HttpClient,
    private existsIsAnError: boolean
  )
  {
  }

  protected checkExists(filter: string, value: any): Observable<ValidationErrors | null>  {
    let params = new HttpParams()
      .set('$filter', filter)
      .set('$top', 0)
      .set('$count', true);
    this.events.add(`checking ${this.resource} existence [${params.toString()}]`)
    return this.http.get<ExistsResponse>(`${this.baseUrl}api/${this.resource}`, { params: params })
      .pipe(
        tap({
          next: () => { this.events.add(`${this.resource} existence check completed`); },
          error: () => { this.events.add(`${this.resource} existence check failed`); },
        }),
        map(response => this.existsIsAnError === (0 < response.count) ? { exists: { value: value } } : null),
        catchError(_ => of(null)),
        first()
      );
  }
}

@Injectable({ providedIn: 'root' })
export class ProviderExistsValidator extends ExistsValidatorBase implements AsyncValidator {
  constructor(
    @Inject('BASE_URL') baseUrl: string,
    events: EventsService,
    http: HttpClient
  )
  {
    super(baseUrl, 'providers', events, http, false);
  }

  validate(control: AbstractControl): Observable<ValidationErrors | null> {
    let providerID = control.value;
    let filter = `id eq ${providerID}`;
    return super.checkExists(filter, providerID);
  }
}

@Injectable({ providedIn: 'root' })
export class OrderNumberIsUniqueValidator extends ExistsValidatorBase implements AsyncValidator {
  private orderID: number = 0;
  constructor(
    @Inject('BASE_URL') baseUrl: string,
    events: EventsService,
    http: HttpClient
  )
  {
    super(baseUrl, 'orders', events, http, true);
  }

  setOrderID(orderID: number) {
    this.orderID = orderID;
  }

  validate(control: AbstractControl): Observable<ValidationErrors | null> {
    let orderNumber = control.value;
    let providerID = control.parent?.get('providerID')?.value;
    if (!orderNumber || !providerID) {
      return of(null);
    }

    let searchValue = orderNumber.replace("'", "''").toLowerCase();
    let filter = `id ne ${this.orderID} and tolower(number) eq '${searchValue}' and providerID eq ${providerID}`;
    return super.checkExists(filter, orderNumber);
  }
}
