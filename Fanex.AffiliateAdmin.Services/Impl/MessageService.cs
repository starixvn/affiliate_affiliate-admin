// --------------------------------------------------------------------------
// <copyright file="MessageService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.7.31">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Services
{
    using System.Collections.Generic;
    using Core.Services;
    using Criterias;
    using DTO;
    using Fanex.AffiliateAdmin.DataAccess;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MessageService : QueryService, IMessageService
    {
        private readonly IMessageDataAccess _messageDataAccess = new MessageDataAccess();

        public IEnumerable<Message> GetMessages(MessageCriteria criteria)
        {
            return GetDataOutput<Message>(criteria);
        }

        public void AddMessage(MessageAddCriteria criteria)
        {
            ExecuteOutput(criteria);
        }

        public void UpdateMessage(MessageUpdateCriteria criteria)
        {
            ExecuteOutput(criteria);
        }

        public void DeleteMessage(MessageDeleteCriteria criteria)
        {
            ExecuteOutput(criteria);
        }

        public MessageDetail MessageDetail(MessageDetailCriteria criteria)
        {
            return _messageDataAccess.MessageDetail(criteria);
        }
    }
}