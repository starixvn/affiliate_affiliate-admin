namespace Fanex.AffiliateAdmin.Services
{
    using Criterias;
    using DTO;
    using System.Collections.Generic;

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