using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class SecMasterFuturesBaseDto
    {
        public SecMasterFuturesBaseDto(string assetType, string aqrBaseCode, string baseDescription)
        {
            AssetType = assetType;
            AQRBaseCode = aqrBaseCode;
            BaseDescription = baseDescription;
        }

        public string AssetType { get; }
        public string AQRBaseCode { get; }
        public string BaseDescription { get; }
    }
}
