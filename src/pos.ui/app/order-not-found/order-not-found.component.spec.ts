import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OrderNotFoundComponent } from './order-not-found.component';

describe('OrderNotFoundComponent', () => {
  let component: OrderNotFoundComponent;
  let fixture: ComponentFixture<OrderNotFoundComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ OrderNotFoundComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(OrderNotFoundComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
