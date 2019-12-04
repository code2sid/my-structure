using Model;
using System.Collections.Generic;
using System.Linq;
using Service.Interfaces.Authorization;
using Common2;

namespace Authorization
{
    internal class RequestDetailsPermissionsService : IRequestDetailsPermissionsService
    {
        public RequestDetailsPermissions GetDetailsPermissions(RequestDetails details, IEnumerable<Approval> approvals, string userId)
        {
            var approvalsList = approvals.QuickToList();
            var isNotCanceled = !details.IsCanceled;
            var canUserApproveClientAdmin = IsAvailableApproverForAny(userId, approvalsList, "");
            var isRequestOpen = isNotCanceled && !details.IsApproved;
            var canSaveNote = isRequestOpen && canUserApproveClientAdmin;
            var canAppendNote = isRequestOpen && IsAvailableApproverForAny(userId, approvalsList, "");
            var canSaveTradingDocs = isRequestOpen && IsAvailableApproverForAny(userId, approvalsList, "");
            var canUserApprovePMs = false;
            var canUserApprovePortfolioFinance = IsAvailableApproverForAny(userId, approvalsList, "");
            var canUploadSamplePortfolio = isRequestOpen && (canUserApprovePMs || canUserApproveClientAdmin || canUserApprovePortfolioFinance);
            var canSaveDueDate = isRequestOpen && canUserApproveClientAdmin;
            var canAddApproval = isRequestOpen && canUserApproveClientAdmin;
            return new RequestDetailsPermissions(canSaveNote, canSaveTradingDocs, canUploadSamplePortfolio, canSaveDueDate, canAppendNote, canAddApproval);
        }

        private static bool IsAvailableApproverForAny(string userId, IEnumerable<Approval> approvals, string approvalSettingId)
        {
            return approvals.Any(a => a.ApprovalSettingId == approvalSettingId && a.IsAvailableApprover(userId));
        }
    }
}
