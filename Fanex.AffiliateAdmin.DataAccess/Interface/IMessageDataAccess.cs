namespace Fanex.AffiliateAdmin.DataAccess
{
    using Criterias;
    using DTO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IMessageDataAccess
    {
        MessageDetail MessageDetail(MessageDetailCriteria criteria);
    }
}