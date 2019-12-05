import { Observable } from 'rxjs/Rx';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { HttpServiceError } from 'HttpServiceError';
import { CustomObservable } from '../../models/custom-observable';
import { JsonMapper } from '../../models/json-mapper';
import { JsonObjectService } from '../json/json-object.service';
export declare class RequestOptions {
  headers: HttpHeaders;
  withCredentials: boolean;
  responseType: 'json';
  constructor(headers: HttpHeaders, withCredentials: boolean, responseType: 'json');
}
export declare abstract class HttpServiceListener {
  abstract webServiceCallStarted(requestId: number): any;
  abstract webServiceCallFinished(requestId: number): any;
}
/**
 * @deprecated WebServiceCallListenerBase. Use HttpServiceListener instead.
 */
export declare abstract class WebServiceCallListenerBase extends HttpServiceListener {
}
export declare class RetryConfig {
  retryInterval: number;
  shouldRetry: (error: HttpErrorResponse, tryAttempt: number, description: string) => boolean;
  constructor(retryInterval: number, shouldRetry: (error: HttpErrorResponse, tryAttempt: number, description: string) => boolean);
}
export declare class HttpService {
  private http;
  private eventBus;
  private retryConfig;
  private requestOptions;
  private jsonService;
  /**
   * @deprecated headers is deprecated.
   */
  private headers;
  /**
   * @deprecated CONTENT_TYPE_JSON is deprecated.
   */
  static CONTENT_TYPE_JSON: {
    [key: string]: string;
  };
  static handleError(error: HttpErrorResponse): Observable<never>;
  constructor(http: HttpClient, eventBus: WebServiceCallListenerBase, retryConfig: RetryConfig, requestOptions: RequestOptions, jsonService: JsonObjectService);
  /**
   * @deprecated withHeaders is deprecated.
   */
  withHeaders(headers: Headers | {
    [name: string]: any;
  }): this;
  getRequestOptions(): RequestOptions;
  /**
   * @deprecated doGetRequest is deprecated, use GET instead.
   */
  doGetRequest<TResponse>(url: string, getData: (data: any) => TResponse, defaultValue?: TResponse): Observable<TResponse>;
  /**
   * @deprecated doPostRequest is deprecated, use POST instead.
   */
  doPostRequest<TRequest>(url: string, data: TRequest): Observable<TRequest>;
  GET<T>(url: string, options?: RequestOptions): CustomObservable<T, HttpServiceError>;
  GET<T>(url: string, template?: T, constructor?: JsonMapper<T> | (new (...args: any[]) => T), options?: RequestOptions): CustomObservable<T, HttpServiceError>;
  GET<TItem, T extends TItem[]>(url: string, template?: T, constructor?: JsonMapper<TItem> | (new (...args: any[]) => TItem), options?: RequestOptions): CustomObservable<T, HttpServiceError>;
  DELETE<T>(url: string, options?: RequestOptions): CustomObservable<T, HttpServiceError>;
  POST<T>(url: string, data?: any, options?: RequestOptions): CustomObservable<T, HttpServiceError>;
  PUT<T>(url: string, data: any, options?: RequestOptions): CustomObservable<T, HttpServiceError>;
  private callService;
  private genericRetryStrategy;
}
