// --------------------------------------------------------------------------
// <copyright file="IMessageService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.7.31">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Services.Interface
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