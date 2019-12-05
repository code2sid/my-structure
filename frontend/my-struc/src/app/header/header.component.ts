import { Component, OnInit } from '@angular/core';

import { UserProviderBase } from '../service/user-provider';
import { ErrorInfo } from '../model/error-info';
import { EventBus } from '../service/event-bus';
import { BaseComponent } from '../base-component';

@Component({
  selector: 'app-header',
  templateUrl: './aqr-header.component.html',
  styleUrls: ['./aqr-header.component.scss']
})
export class HeaderComponent extends BaseComponent implements OnInit {
  public isAdmin: boolean|null = null;
  constructor(
    private userProvider: UserProviderBase,
    private eventBus: EventBus) {
      super('HeaderComponent');
    }

  ngOnInit() {

    const s = this.userProvider.isAdmin()
    .subscribe(
      result => this.isAdmin = result,
      error => this.eventBus.errorOccurredEventEmitter.emit(new ErrorInfo(this.className, error)),
      () => { if (s) { s.unsubscribe(); }}
    );
  }
}
