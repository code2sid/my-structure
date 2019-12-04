namespace Model
{
    public class RequestDetailsPermissions
    {
        public RequestDetailsPermissions(
            bool canSaveNote, bool canSaveTradingDocs, bool canUploadSamplePortfolio, bool canSaveDueDate, bool canAppendNote, bool canAddApproval)
        {
            CanSaveNote = canSaveNote;
            CanSaveTradingDocs = canSaveTradingDocs;
            CanUploadSamplePortfolio = canUploadSamplePortfolio;
            CanSaveDueDate = canSaveDueDate;
            CanAppendNote = canAppendNote;
            CanAddApproval = canAddApproval;
        }

        public bool CanAppendNote { get; }
        public bool CanSaveNote { get; }
        public bool CanSaveTradingDocs { get; }
        public bool CanUploadSamplePortfolio { get; }
        public bool CanSaveDueDate { get; }
        public bool CanAddApproval { get; }
        public static RequestDetailsPermissions None { get; } = new RequestDetailsPermissions(false, false, false, false, false, false);
    }
}
