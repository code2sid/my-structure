import { Observable , Subscription } from 'rxjs';
export declare class CustomObservable<TResult, TError> {
  private observable;
  static of<T>(value?: T): CustomObservable<T, any>;
  static forkJoin<TResult, TError>(observables: CustomObservable<TResult, TError>[]): CustomObservable<TResult[], TError>;
  constructor(observable: Observable<TResult>);
  subscribe(onSuccess: (result: TResult) => any, onError?: (error: TError) => any, onFinally?: () => any): Subscription;
  map<TMapped>(mapper: (item: TResult) => TMapped): CustomObservable<TMapped, TError>;
  toObservable(): Observable<TResult>;
}
