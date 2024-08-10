import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule  } from '@angular/forms';
import { DatePipe } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { OrderListComponent } from './order-list/order-list.component';
import { EventListComponent } from './event-list/event-list.component';
import { OrderDetailsComponent } from './order-details/order-details.component';
import { OrderNotFoundComponent } from './order-not-found/order-not-found.component';

let routes = [
  { path: '', component: OrderListComponent },
  { path: 'order', component: OrderDetailsComponent },
  { path: 'order/not-found', component: OrderNotFoundComponent },
  { path: 'order/:orderID', component: OrderDetailsComponent },
];

@NgModule({
  declarations: [
    AppComponent,
    OrderListComponent,
    EventListComponent,
    OrderDetailsComponent,
    OrderNotFoundComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    MatAutocompleteModule,
    RouterModule.forRoot(routes)
  ],
  providers: [DatePipe],
  bootstrap: [AppComponent]
})
export class AppModule { }
