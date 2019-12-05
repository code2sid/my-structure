import { Injectable } from '@angular/core';
import {HttpService} from '../httpService/HttpService';
import { environment } from '../../environments/environment';

import { CustomObservable } from '../httpService/CustomObserverable';
import { HttpServiceError } from '../httpService/HttpServiceError';

@Injectable()
export abstract class UserProviderBase {
  public abstract isAdmin(): CustomObservable<boolean, HttpServiceError>;
  public abstract canCreateNewRequest(): CustomObservable<boolean, HttpServiceError>;
  public abstract doesUserRequireNotificationAboutCustomSetup(): CustomObservable<boolean, HttpServiceError>;
  public abstract canEditGenevaRules(): CustomObservable<boolean, HttpServiceError>;
}

@Injectable()
export class UserProvider extends UserProviderBase {
    constructor(private dataService: HttpService) {
      super();
    }

    public isAdmin(): CustomObservable<boolean, HttpServiceError> {
        return this.dataService
            .GET<boolean>(`${environment.userUrl}/isAdmin`);
    }

    public canCreateNewRequest(): CustomObservable<boolean, HttpServiceError> {
      return this.dataService
          .GET<boolean>(`${environment.userUrl}/canCreateNewRequest`);
  }

  public doesUserRequireNotificationAboutCustomSetup(): CustomObservable<boolean, HttpServiceError> {
    return this.dataService
    .GET<boolean>(`${environment.userUrl}/isCustomSetupRequired`);
  }

  public canEditGenevaRules(): CustomObservable<boolean, HttpServiceError> {
    return this.dataService
      .GET<boolean>(`${environment.userUrl}/canEditGenevaRules`);
  }
}
