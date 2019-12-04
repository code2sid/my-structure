using System;
using System.Collections.Generic;

namespace Model
{
    public class Approval : ApprovalBase<SubApproval>
    {
        public Approval(int id, int level, string name, string status, User updateUser,
            IEnumerable<User> availableApprovers, string note, DateTime updateDate, int entitlementGroupId,
            IEnumerable<SubApproval> subApprovals, string approvalSettingId, double percentComplete) :
            base(id, level, name, status, updateUser, availableApprovers, note, updateDate, entitlementGroupId, subApprovals, approvalSettingId, percentComplete)
        {
        }

        public static Approval None { get; } = new Approval(0, 0, null, null, null, null, null, DateTime.Now, 0, null, null, 0);

        public Approval WithApprovalSettingId(string approvalSettingId)
        {
            return new Approval(Id, Level, Name, Status, UpdateUser, AvailableApprovers, Note, UpdateDate, EntitlementGroupId, SubApprovals, approvalSettingId, PercentComplete);
        }

        public Approval WithAvailableApprovers(IEnumerable<User> approvers)
        {
            return new Approval(Id, Level, Name, Status, UpdateUser, approvers, Note, UpdateDate, EntitlementGroupId, SubApprovals, ApprovalSettingId, PercentComplete);
        }

        public Approval WithStatus(string status)
        {
            return new Approval(Id, Level, Name, status, UpdateUser, AvailableApprovers, Note, UpdateDate, EntitlementGroupId, SubApprovals, ApprovalSettingId, PercentComplete);
        }

        public override bool Equals(object obj) => obj != null && obj is Approval a && ToTuple().Equals(a.ToTuple());

        public override int GetHashCode() => ToTuple().GetHashCode();

        private (string, int, int) ToTuple() => (ApprovalSettingId, EntitlementGroupId, Level);
    }
}
