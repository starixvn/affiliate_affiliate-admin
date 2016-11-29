namespace Fanex.AffiliateAdmin.Core.Constants
{
    public static class OneAuthorizationErrorCode
    {
        public const string TokenWasExpired = "100";
        public const string LicenseeIsNotExisted = "200";
        public const string TokenIsInValid = "300";
        public const string LocationRestrictionMsg = "400";
        public const string LicenseeNotOfferedCurrency = "600";
        public const string YouAreNotAllowToAccessThisPage = "700";

        public static string NotFoundUser { get; set; }
    }
}