import { of as observableOf } from 'rxjs';
import { Observable } from 'rxjs';

import { Result } from '../model/result';
import { AppStateManager } from '../app-state-manager';


import { UserProviderBase } from '../service/user-provider';

import {RequestOptions} from "../httpService/HttpService";
import {CustomObservable} from "../httpService/CustomObserverable";
import {HttpServiceError} from "../httpService/HttpServiceError";


export class HttpClientMock {
  get(url: string, options?: RequestOptions): CustomObservable<Response, HttpServiceError> {
    return CustomObservable.of({} as Response);
  }
}

export class AppStateManagerMock extends AppStateManager {
  public getRequestId = () => 123;
}

export class UserProviderMock extends UserProviderBase {
  public isAdmin(): CustomObservable<boolean, HttpServiceError> {
    return CustomObservable.of(true);
  }

  public canCreateNewRequest(): CustomObservable<boolean, HttpServiceError> {
    return CustomObservable.of(true);
  }
  public doesUserRequireNotificationAboutCustomSetup(): CustomObservable<boolean, HttpServiceError> {
    return CustomObservable.of(true);
  }

  public canEditGenevaRules(): CustomObservable<boolean, HttpServiceError> {
    return CustomObservable.of(true);
  }
}
