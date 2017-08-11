namespace Fanex.AffiliateAdmin.DTO.OneAuth
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    [DataContract]
    public class OneAuthPermission
    {
        [DataMember]
        public string FunctionKey { get; set; }

        [DataMember]
        public string[] SubFunctionKeys { get; set; }
    }
}