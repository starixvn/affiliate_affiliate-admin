namespace Fanex.AffiliateAdmin.DTO.OneAuth
{
    using System.Runtime.Serialization;

    [DataContract]
    public class OneAuthPermission
    {
        [DataMember]
        public string FunctionKey { get; set; }

        [DataMember]
        public string[] SubFunctionKeys { get; set; }
    }
}