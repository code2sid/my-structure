import { Injectable } from '@angular/core';

import { Observable, Subject } from 'rxjs';
import { StringDictionary } from './model/dictionary';

class SubscribableItem<T> {
  constructor(
    public data: T,
    public subscription: Subject<T>
  ) { }
}

@Injectable()
export class AppStateManager {
  private detailsViewWithSelectedApproval: any | null = null;
  private requestId: number | null = null;
  private globalMarketSelections: {
    [CrdIdAndMarketType: string]: SubscribableItem<any[]>
  } = {};
  private childApprovals: StringDictionary<any[]>|null = null;

  public setGlobalMarketSelections(crdId: string, marketType: string, marketSelections: any[]) {
    this.ensureMarketTypeInitialized(crdId, marketType);

    const subscription = this.globalMarketSelections[`${crdId}~${marketType}`].subscription;
    this.globalMarketSelections[`${crdId}~${marketType}`].data = marketSelections;
    subscription.next(marketSelections);
  }

  public updateNewSelections(crdId: string, marketType: string, newSelections: any[]) {
    this.ensureMarketTypeInitialized(crdId, marketType);

    const existingSelections = this.globalMarketSelections[`${crdId}~${marketType}`]
      .data
      .filter(s => s.existing || s.isFromExistingRequest);
    const filteredSelections = newSelections.filter(newSel => !existingSelections.some(exSel => exSel.id === newSel.id));
    this.setGlobalMarketSelections(crdId, marketType, existingSelections.concat(filteredSelections));
  }

  private ensureMarketTypeInitialized(crdId: string, marketType: string) {
    if (!Object.keys(this.globalMarketSelections).includes(`${crdId}~${marketType}`)) {
      this.globalMarketSelections[`${crdId}~${marketType}`] = new SubscribableItem([], new Subject<any[]>());
    }
  }

  public getGlobalMarketSelections(crdId: string, marketType: string): [any[], Observable<any[]>] {
    const item = this.globalMarketSelections[`${crdId}~${marketType}`];
    if (item) {
      return [item.data, <Observable<any[]>>item.subscription];
    } else {
      this.setGlobalMarketSelections(crdId, marketType, []);
      return this.getGlobalMarketSelections(crdId, marketType);
    }
  }

  public setRequestId(requestId: number) {
    this.requestId = requestId;
  }

  public readonly getRequestId = () => this.requestId;

  public getCrdIdRequestId(crdId: string): number {
    if (!this.detailsViewWithSelectedApproval) {
      return 0;
    }
    return this.detailsViewWithSelectedApproval.detailsView.requestIds[crdId];
  }

  public readonly getDetailsViewWithSelectedApproval = () => this.detailsViewWithSelectedApproval;

  public setDetailsViewWithSelectedApproval(value: any) {
    this.detailsViewWithSelectedApproval = value;
  }

  public getDetailsView() {
    return this.detailsViewWithSelectedApproval ? this.detailsViewWithSelectedApproval.detailsView : null;
  }

  public setDetailsView(detailsView: any) {
    if (this.detailsViewWithSelectedApproval) {
      this.detailsViewWithSelectedApproval.detailsView = detailsView;
      const selectedApproval = this.getSelectedApproval();
      if (selectedApproval) {
        this.setSelectedApproval(detailsView.approvalsWithPermissions[0]);
      }
    }
  }

  public getRequestDetailsWithPermissions() {
    const detailsView = this.getDetailsView();
    return detailsView ? detailsView.requestDetailsWithPermissions : null;
  }

  public getRequestDetailsPermissions() {
    const requestDetailsWithPermissions = this.getRequestDetailsWithPermissions();
    return requestDetailsWithPermissions ? requestDetailsWithPermissions.permissions : null;
  }

  public getSelectedApproval() {
    return this.detailsViewWithSelectedApproval ? this.detailsViewWithSelectedApproval.selectedApproval : null;
  }

  public setSelectedApproval(approval: any) { //ApprovalWithPermissions
    if (this.detailsViewWithSelectedApproval) {
      this.detailsViewWithSelectedApproval.selectedApproval = approval;
    }
  }

  public setChildApprovals(childApprovals: StringDictionary<any[]>) { //ApprovalWithPermissions
    this.childApprovals = childApprovals;
  }

  public getChildApprovals(): StringDictionary<any[]>|null { //ApprovalWithPermissions
    return this.childApprovals;
  }
}
