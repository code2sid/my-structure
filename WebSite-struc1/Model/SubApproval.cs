using Model.Interfaces;

namespace Model
{
    public class SubApproval : ISubApprovalSetting
    {
        private readonly string _status;
        public const string CompletedStatus = "Completed";

        public SubApproval(string subApprovalName, string subApprovalStatus, int id, int approvalLevel,
            int entitlementGroupId, bool isNa, bool canBeNa, bool isRequiredByDefault)
        {
            Name = subApprovalName;
            _status = subApprovalStatus;
            Id = id;
            ApprovalLevel = approvalLevel;
            EntitlementGroupId = entitlementGroupId;
            IsNa = isNa;
            CanBeNa = canBeNa;
            IsRequiredByDefault = isRequiredByDefault;
        }

        public int Id { get; }
        public string Status => IsNa ? CompletedStatus : _status;
        public string Name { get; }
        public int ApprovalLevel { get; }
        public int EntitlementGroupId { get; }
        public bool IsComplete => Status != null;
        public bool IsRequired => IsRequiredByDefault && !IsNa;
        public bool IsNa { get; }
        public bool CanBeNa { get; }
        public bool IsRequiredByDefault { get; }
    }
}
