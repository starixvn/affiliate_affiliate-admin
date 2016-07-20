namespace Fanex.AffiliateAdmin.Core.Enumerations
{
    public enum ErrorCode : int
    {
        /// <summary>
        /// Unknown error.
        /// </summary>
        None,

        /// <summary>
        /// Wrong username or password.
        /// </summary>
        WrongUserOrPassword = 1001,

        /// <summary>
        /// The A/C is closed due to 5 wrong password attempts.
        /// </summary>
        LoginFailedFiveTimes = 1002,

        /// <summary>
        /// Your account is closed.
        /// </summary>
        AccountClosed = 1003
    }
}