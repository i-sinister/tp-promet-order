import { Component, ViewEncapsulation } from '@angular/core';

@Component({
  selector: '[pos-app-root]',
  templateUrl: './app.component.html',
  styleUrls: [ './app.component.css' ],
  encapsulation: ViewEncapsulation.None
})
export class AppComponent {
  title = 'app';
}
