import { Component } from '@angular/core';
import { EventsService } from '../events.service';

@Component({
  selector: '[pos-event-list]',
  templateUrl: './event-list.component.html',
})
export class EventListComponent {
  public events: EventsService;

  constructor(events: EventsService) {

    this.events = events;
  }
}
