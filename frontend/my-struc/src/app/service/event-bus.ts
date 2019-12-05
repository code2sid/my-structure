import { Injectable } from '@angular/core';
import { ErrorInfo } from '../model/error-info';
import { Subject } from 'rxjs';

export class MessageSender<T> {
  private readonly sender: Subject<T>;

  constructor() {
    this.sender = new Subject<T>();
  }

  public subscribe(next?: (value: T) => void, error?: (error: any) => void, complete?: () => void) {
    return this.sender.subscribe(next, error, complete);
  }

  public emit = (value?: T) => this.sender.next(value);
}

@Injectable()
export class EventBus {
  public readonly errorOccurredEventEmitter = new MessageSender<ErrorInfo>();
}

