import { Unsubscribable as AnonymousSubscription } from 'rxjs/index';
import { OnDestroy } from '@angular/core';
import { EventBus } from './service/event-bus';
import { ErrorInfo } from './model/error-info';
import { HttpServiceError } from './httpService/HttpServiceError';

export class BaseComponent implements OnDestroy {
  private subscriptions: AnonymousSubscription[] = [];
  constructor(protected readonly className: string) {}

  public ngOnDestroy(): void {
    this.subscriptions.forEach(s => s.unsubscribe());
  }

  protected addSubcription(subscription: AnonymousSubscription) {
    this.subscriptions.push(subscription);
  }

  protected errorOccurred(eventBus: EventBus, error: string|HttpServiceError) {
    eventBus.errorOccurredEventEmitter.emit(new ErrorInfo(this.className, error));
  }
}
