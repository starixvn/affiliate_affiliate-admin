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