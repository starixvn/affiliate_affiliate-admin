// --------------------------------------------------------------------------
// <copyright file="MessageDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.7.31">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.DataAccess.Imp
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Common.Helper;
    using Core.Configurations;
    using Criterias;
    using Data;
    using DTO;
    using Interface;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MessageDataAccess : IMessageDataAccess
    {
        /// <summary>
        /// Get message detail.
        /// </summary>
        /// <param name="criteria">Message Detail Criteria.</param>
        /// <returns>Message detail.</returns>
        public MessageDetail MessageDetail(MessageDetailCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            if (criteria.IsValid())
            {
                using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    objectDb.AddInParameter("msgid", DbType.Int32, criteria.MsgId);
                    using (var reader = objectDb.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var creator = Convert.ToInt32(reader["creator"]);
                            var postedDate = Convert.ToDateTime(reader["postedDate"]);
                            var msgId = Convert.ToInt32(reader["msgid"]);
                            var important = Convert.ToBoolean(reader["important"]);
                            var creatorName = Convert.ToString(reader["resourcekey"]);
                            var langMessages = new Dictionary<string, string>();
                            foreach (var language in AppSettings.AffLanguages)
                            {
                                langMessages[language.LanguageID] = Convert.ToString(reader[language.LanguageID]);
                            }

                            return new MessageDetail
                            {
                                Creator = creator,
                                PostedDate = postedDate,
                                MsgId = msgId,
                                Important = important,
                                CreatorName = creatorName,
                                LangMessages = langMessages
                            };
                        }
                    }
                }
            }

            return null;
        }
    }
}