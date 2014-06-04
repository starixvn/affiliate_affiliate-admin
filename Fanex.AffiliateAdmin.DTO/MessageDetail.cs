// --------------------------------------------------------------------------
// <copyright file="MessageDetail.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Johnny.Trinh" date="2013.7.31">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.DTO
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MessageDetail
    {
        private Dictionary<string, string> _langMessages;

        public int Creator { get; set; }

        public DateTime PostedDate { get; set; }

        public int MsgId { get; set; }

        public bool Important { get; set; }

        public string CreatorName { get; set; }

        public int ResourceKey { get; set; }

        public Dictionary<string, string> LangMessages
        {
            get
            {
                return _langMessages;
            }
        }

        public void AddLangMessage(Dictionary<string, string> langMessages)
        {
            _langMessages = langMessages;
        }
    }
}