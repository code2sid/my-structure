using System;
using System.Collections.Generic;
using System.Linq;

namespace Model
{
    public abstract class ApprovalBase<T> where T : SubApproval
    {
        protected ApprovalBase(
            int id, int level, string name, string status,
            User updateUser, IEnumerable<User> availableApprovers, string note, DateTime updateDate,
            int entitlementGroupId, IEnumerable<T> subApprovals, string approvalSettingId, double percentComplete)
        {
            Id = id;
            Level = level;
            Name = name;
            Status = status;
            UpdateUser = updateUser;
            AvailableApprovers = availableApprovers;
            Note = note;
            UpdateDate = updateDate;
            EntitlementGroupId = entitlementGroupId;
            SubApprovals = subApprovals;
            ApprovalSettingId = approvalSettingId;
            PercentComplete = percentComplete;
        }

        public int Id { get; }
        public string ApprovalSettingId { get; }
        public int Level { get; }
        public string Name { get; }
        public string Status { get; }
        public User UpdateUser { get; }
        public IEnumerable<User> AvailableApprovers { get; }
        public string Note { get; }
        public DateTime UpdateDate { get; }
        public int EntitlementGroupId { get; }
        public IEnumerable<T> SubApprovals { get; }
        public bool IsApproved => Status != null;
        public double PercentComplete { get; }

        public bool IsAvailableApprover(string userId)
        {
            return AvailableApprovers.Select(u => u.Id).Contains(userId);
        }

        public string DisplayName => "";
    }
}
