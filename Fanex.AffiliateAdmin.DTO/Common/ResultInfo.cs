namespace Fanex.AffiliateAdmin.DTO.Common
{
    using System;

    [Serializable]
    public class ResultInfo
    {
        public bool IsSuccessful
        {
            get { return string.IsNullOrEmpty(Message) || (Code == 0); }
        }

        public int Code { get; set; }

        public string Message { get; set; }

        public string ExtendedData { get; set; }
    }
}