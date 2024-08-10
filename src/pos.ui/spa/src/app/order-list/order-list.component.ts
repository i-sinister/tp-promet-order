import { Component, Inject, Input, OnInit } from '@angular/core';
import { DatePipe } from '@angular/common';
import { HttpClient, HttpParams } from '@angular/common/http';
import { FormControl } from '@angular/forms';
import { Observable, of, Subscription } from 'rxjs';
import {
  tap, map, startWith, debounceTime, distinctUntilChanged, switchMap, catchError
} from 'rxjs/operators';
import * as moment from 'moment';


import { EventsService } from '../events.service';
import { OrderListResponse, OrderInfo } from '../models/orderInfo';
import { AutoCompleteResponse } from '../models/autocomplete';

enum SortDirection {
  None,
  Asc,
  Desc
}

abstract class Column {
  constructor (
    public propertyName: string = '',
    public sorting: SortDirection = SortDirection.None,
  )
  {
  }

  public abstract render() : (string | null)[];
}

class TextColumn extends Column {
  constructor (
    propertyName: string = '',
    sorting: SortDirection = SortDirection.None,
    public filter: FormControl<string | null>
  )
  {
    super(propertyName, sorting);
  }

  public render(): (string | null)[] {
    return [TextColumn.renderPart(this.propertyName, this.filter.value)];
  }

  public static renderPart(prop: string, search: string | null): string | null{
    if (!search || !search.length) {
      return null;
    }

    var value = search.replace("'", "''").toLowerCase();
    return `contains(tolower(${prop}),'${value}')`;
  }
}

class DateColumn extends Column {
  constructor (
    propertyName: string = '',
    sorting: SortDirection = SortDirection.None,
    private datepipe: DatePipe,
    public from: Date | null = null,
    public to: Date | null = null
  )
  {
    super(propertyName, sorting);
  }

  public render(): (string | null)[] {
    return [this.renderPart(this.from, 'ge'), this.renderPart(this.to, 'lt')];
  }

  private renderPart(date: Date | null, op: string): string | null {
    if (null == date) {
      return null;
    }

    var prop = this.propertyName;
    var value = this.datepipe.transform(date, 'yyyy-MM-ddTHH:mm:00ZZZZZ');
    return `${prop} ${op} ${value}`;
  }

  public clear(): void {
    this.from = null;
    this.to = null;
  }
}

// HACK: there is a bug in OData - when `$select` is used it loses name convension and `DataMember` attribute names
function capitalizeFirstLetter(str: string) {
  return str.charAt(0).toUpperCase() + str.slice(1);
}

@Component({
  selector: '[pos-order-list]',
  templateUrl: './order-list.component.html',
})
export class OrderListComponent implements OnInit {
  SD = SortDirection;

  private sortOrder: Column[] = [];
  @Input() public pageSize: number = 4;
  @Input() public currentPage: number = 1;
  public pageCount: number = 0;
  public number: TextColumn;
  public date: DateColumn;
  public provider: TextColumn;
  public orders: OrderInfo[] = [];
  public numberFilter: FormControl<(string | null)>;
  public numberOptions: Observable<string[]>;
  public providerFilter: FormControl<(string | null)>;
  public providerOptions: Observable<string[]>;
  public is_loading: boolean = false;

  constructor(
    private events: EventsService,
    private http: HttpClient,
    private datepipe: DatePipe,
    @Inject('BASE_URL') private baseUrl: string
  ) {
    this.events = events;
    this.baseUrl = baseUrl;
    this.http = http;
    this.numberFilter = new FormControl<string | null>('')
    this.providerFilter = new FormControl<string | null>('');
    this.number = new TextColumn('number', SortDirection.None, this.numberFilter);
    this.date = new DateColumn('date', SortDirection.None, datepipe);
    this.provider = new TextColumn('providerName', SortDirection.None, this.providerFilter);
    this.date.from = moment().add(1, 'day').startOf('day').add(-1, 'month').toDate();
    this.date.to = moment().add(1, 'day').startOf('day').toDate();

    let makeAutocomplete = (control: FormControl, projection: (val: string) => Observable<string[]>) => {
      return control.valueChanges.pipe(
        startWith(''),
        debounceTime(400),
        distinctUntilChanged(),
        switchMap(projection)
      );
    }

    this.numberOptions = makeAutocomplete(this.numberFilter, (val) => this.loadOrderNumbers(val || ''));
    this.providerOptions = makeAutocomplete(this.providerFilter, (val) => this.loadProviderNames(val || ''));
    this.events.add(`loaded at ${new Date().toISOString()}`);
  }

  ngOnInit() {
    this.loadOrders();
  }

  public loadOrders() {
    let [options, pageSize] = this.buildOrderListOptions();
    let logOptions = options.params?.toString();
    this.is_loading = true;
    this.events.add(`loading orders [${logOptions}] ...`);
    this.http
      .get<OrderListResponse>(this.baseUrl + 'api/orders', options)
      .subscribe({
        next: result => {
          this.is_loading = false;
          this.events.add("loading orders completed");
          this.orders = result.items;
          this.pageCount = Math.ceil(result.count / pageSize);
        },
        error: _ => {
          this.is_loading = false;
          this.events.add("loading orders failed");
          this.orders = [];
        }
      });
  }

  private loadOrderNumbers(search: string): Observable<string[]> {
    return this.loadAutoComplete('orders', 'number', search, 'order numbers');
  }

  private loadProviderNames(search: string): Observable<string[]> {
    return this.loadAutoComplete('providers', 'name', search, 'provider names');
  }

  private loadAutoComplete(
    service: string,
    property: string,
    search: string,
    logType: string
  ) {
    if (!search || !search.length) {
      return of([]);
    }

    let params = new HttpParams()
      .set('$select', property)
      .set('$filter', TextColumn.renderPart(property, search) || '')
      .set("$orderby", property)
      .set("$top", 10)
      .set("$distinct", true);
    let options = { params: params };
    let logOptions = options.params?.toString();
    this.events.add(`loading ${logType} [${logOptions}] ...`);
    return this.http
      .get<AutoCompleteResponse>(`${this.baseUrl}api/${service}`, options)
      .pipe(
        tap({
          next: () => { this.events.add(`loading ${logType} completed`); },
          error: () => { this.events.add(`loading ${logType} failed`); },
        }),
        map(response => response.items.map(item => item[capitalizeFirstLetter(property)])),
        catchError(_ => of([]))
      );
  }

  public clearFilters() {
    this.numberFilter.setValue('');
    this.date.clear();
    this.providerFilter.setValue('');
    this.currentPage = 1;
    this.loadOrders();
  }

  public goToFirstPage() {
    this.currentPage = 1;
    this.loadOrders();
  }

  public goToPrevPage() {
    if (1 >= this.currentPage)
    {
      return;
    }

    this.currentPage--;
    this.loadOrders();
  }

  public goToNextPage() {
    this.currentPage++;
    this.loadOrders();
  }

  public goToLastPage() {
    if (this.pageCount)
    {
      this.currentPage = this.pageCount;
      this.loadOrders();
    }
  }

  public toggleSort(column: Column)
  {
    switch (column.sorting)
    {
      case SortDirection.None:
        column.sorting = SortDirection.Asc;
        this.sortOrder.push(column);
        break;
      case SortDirection.Asc:
        column.sorting = SortDirection.Desc;
        break;
      case SortDirection.Desc:
        column.sorting = SortDirection.None;
        const index = this.sortOrder.indexOf(column, 0);
        if (index > -1) {
           this.sortOrder.splice(index, 1);
        }

        break;
    }

    this.loadOrders();
  }

  public removeOrder(order: OrderInfo) {
    if (!confirm(`Are you sure you want to delete order '${order.number}'`)) {
      return;
    }

    this.events.add(`reming order ${order.id}`);
    return this.http
      .delete(`${this.baseUrl}api/orders/${order.id}`)
      .subscribe({
        next: _ => {
          this.events.add(`orders ${order.id} removed`);
          this.loadOrders();
        },
        error: _ => {
          this.events.add(`orders ${order.id} could not be removed`);
        }
      });
  }

  private buildOrderListOptions() : [options: { params?: HttpParams }, pageSize: number] {
    let buildFilter = () : string => {
      return [this.number, this.date, this.provider]
        .flatMap(column => column.render())
        .filter(filter => null !== filter)
        .join(" and ");
    }

    let buildOrderBy = () : string => {
      return this.sortOrder
        .map(
          column => {
            switch (column.sorting)
            {
              case SortDirection.Asc:
                return `${column.propertyName}`;
              case SortDirection.Desc:
                return `${column.propertyName} desc`;
              default:
                return null;
            };
          })
        .filter(part => null != part)
        .join(',');
    }

    let filter = buildFilter();
    let orderby = buildOrderBy();

    let params = new HttpParams();
    if (filter.length) {
      params = params.set('$filter', filter);
    }

    if (orderby.length) {
      params = params.set("$orderby", orderby)
    }

    let pageSize = Math.min(10, Math.max(0, this.pageSize));
    params = params
      .set("$top", pageSize)
      .set("$skip", Math.max(0, this.currentPage - 1) * pageSize)
      .set("$count", true);
    return [params.keys().length ? { params: params } : { }, pageSize];
  }
}
