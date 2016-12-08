namespace Fanex.AffiliateAdmin.Services
{
    using System.Collections.Generic;
    using Criterias;
    using DTO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IMessageService
    {
        IEnumerable<Message> GetMessages(MessageCriteria criteria);

        void AddMessage(MessageAddCriteria criteria);

        void UpdateMessage(MessageUpdateCriteria criteria);

        void DeleteMessage(MessageDeleteCriteria criteria);

        MessageDetail MessageDetail(MessageDetailCriteria criteria);
    }
}