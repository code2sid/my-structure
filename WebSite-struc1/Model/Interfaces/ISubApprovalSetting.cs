namespace Model.Interfaces
{
    public interface ISubApprovalSetting
    {
        int ApprovalLevel { get; }
        int EntitlementGroupId { get; }
        string Name { get; }
    }
}
