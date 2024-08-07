import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class EventsService {

  public events: string[] = [];

  add(event: string) {
    if (200 >= this.events.length) {
      this.events = this.events.slice(0, 199);
    }

    this.events.unshift(event);
  }
}
