namespace Fanex.AffiliateAdmin.Core.Mailinator
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class NormalAffiliateMailinator : AffiliateMailinator
    {
        public override void Approve()
        {
            base.Approve();

            // 3. WithdrawalEmail
            ResolvePath("WithdrawalEmail");
            Send(EmailOfAffiliate, EmailTemplateInfo);
        }
    }
}
