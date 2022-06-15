namespace Fanex.AffiliateAdmin.Core.Constants
{
    public static class ErrorCodes
    {
        public const int Success = 0;
        public const int GeneralErrorCode = 9999;
        public const int InvalidRequest = 16;
        public const int AccessDeniedErrorCode = 500;
        public const int WrongUserPasswordErrorCode = 500;

        //One Authorization ErrorCodes
        public const int TokenWasExpired = 100;
        public const int LicenseeIsNotExisted = 200;
        public const int TokenIsInValid = 300;
        public const int LocationRestrictionMsg = 400;
        public const int LicenseeNotOfferedCurrency = 600;
        public const int YouAreNotAllowToAccessThisPage = 700;
        public const int NotFoundUser = 800;
    }
}
