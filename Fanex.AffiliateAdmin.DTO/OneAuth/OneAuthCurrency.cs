namespace Fanex.AffiliateAdmin.DTO.OneAuth
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    [DataContract]
    public class OneAuthCurrency
    {
        [DataMember]
        public int[] AgentId { get; set; }

        [DataMember]
        public string Currency { get; set; }

        [DataMember]
        public int CurrencyId { get; set; }
    }
}