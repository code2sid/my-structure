using System;
using System.Collections.Generic;

namespace Model
{
    public class RequestDetails
    {
        public const string CanceledStatus = "Canceled";
        public const string ApprovedStatus = "Approved";
        public int RequestId { get; }
        public string Note { get; }
        public string CRDCode { get; }
        public string GenevaId { get; }
        public string CrdCodeForGeneva => IsNonTradingEntity ? GenevaId : CRDCode;
        public DateTime? DueDate { get; }
        public DateTime? LaunchDate { get; }
        public string TradingDocs { get; }
        public string IsNewAccount { get; }
        public bool IsNonTradingEntity { get; }
        public bool? IsFinancingRequired { get; }
        public bool IsCanceled => Status == CanceledStatus;
        public bool IsApproved => Status == ApprovedStatus;
        public string Status { get; }
        public bool? AreThereTriparties { get; }
        public bool? ShareClassHedging { get; }
        public static DateTime Phase1LiveDate { get; } = new DateTime(2018, 1, 26);
        public DateTime RequestDate { get; }
        public bool HasPushedCashFlowLocationAccountToRms { get; }
        public IEnumerable<string> Events { get; }
        public bool WasCreatedInOldTool => RequestDate < Phase1LiveDate;
        public bool IsArchived => DueDate.HasValue && DueDate < DateTime.Today;
        public string RequestType { get; }
        public bool? IsFXSubaccounts { get; }
        public string OpsAccountNumbers { get; }
        public bool HasParent { get; }

        public RequestDetails(int requestId,
             string note,
            string crdCode, string genevaId,
            DateTime? dueDate, DateTime? launchDate, string tradingDocs, 
            bool isNonTradingEntity, string isNewAccount, 
            bool? isFinancingRequired, string status, bool? areThereTriparties, bool? shareClassHedging,
            DateTime requestDate, bool hasPushedCashFlowLocationAccountToRms, IEnumerable<string> events,
            bool? isFXSubaccounts, string opsAccountNumbers, bool hasParent)
        {
            RequestId = requestId;
            Note = note;
            CRDCode = crdCode;
            GenevaId = genevaId;
            DueDate = dueDate;
            LaunchDate = launchDate;
            TradingDocs = tradingDocs;
            IsNonTradingEntity = isNonTradingEntity;
            IsNewAccount = isNewAccount;
            IsFinancingRequired = isFinancingRequired;
            Status = status;
            AreThereTriparties = areThereTriparties;
            ShareClassHedging = shareClassHedging;
            RequestDate = requestDate;
            HasPushedCashFlowLocationAccountToRms = hasPushedCashFlowLocationAccountToRms;
            Events = events;
            RequestType = "";
            IsFXSubaccounts = isFXSubaccounts;
            OpsAccountNumbers = opsAccountNumbers;
            HasParent = hasParent;
        }

        public RequestDetails WithIsNonTradingEntity(bool isNonTradingEntity)
        {
            return new RequestDetails(RequestId, Note, CRDCode, GenevaId, DueDate,
                LaunchDate, TradingDocs, isNonTradingEntity, IsNewAccount,
                IsFinancingRequired, Status, AreThereTriparties, ShareClassHedging, RequestDate, HasPushedCashFlowLocationAccountToRms, Events,
                IsFXSubaccounts, OpsAccountNumbers, HasParent);
        }

        public RequestDetails WithCrdCode(string crdId)
        {
            return new RequestDetails(RequestId, Note, crdId, GenevaId, DueDate,
                LaunchDate, TradingDocs,  IsNonTradingEntity, IsNewAccount,
                 IsFinancingRequired, Status, AreThereTriparties, ShareClassHedging, RequestDate, HasPushedCashFlowLocationAccountToRms, Events,
                IsFXSubaccounts, OpsAccountNumbers, HasParent);
        }
    }
}
