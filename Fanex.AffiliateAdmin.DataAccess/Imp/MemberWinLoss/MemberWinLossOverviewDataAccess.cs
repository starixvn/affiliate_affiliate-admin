// --------------------------------------------------------------------------
// <copyright file="MemberWinLossOverviewDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.9.19">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.DataAccess
{
    using System.Data;
    using System.Linq;
    using Criterias;
    using DTO;
    using Fanex.Common.Extensions;
    using Fanex.Data;

    /// <summary>
    /// Account data access.
    /// </summary>
    public class MemberWinLossOverviewDataAccess : MemberWinLossDataAccess, IMemberWinLossOverviewDataAccess
    {
        /// <summary>
        /// Get Member Win Loss Overview.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Member Win Loss By Overview Result.</returns>
        public MemberWinLossByOverviewResult GetMemberWinLossOverview(MemberWinLossOverviewCriteria criteria)
        {
            var memberWinlossReport = new MemberWinLossByOverviewResult();

            if (criteria.IsValid())
            {
                using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    var param = criteria.GetSpParams();

                    var reader = objectDb.ExecuteReader(param);

                    memberWinlossReport.MemberWinlossList = GetMemberWinlossList(reader);

                    memberWinlossReport.TotalMemberWinlossInMemberCurrencies = GetTotalMemberWinlossInMemberCurrency(reader);

                    memberWinlossReport.TotalMemberWinlossInSystemCurrency = GetTotalMemberWinlossInSystemCurrency(reader).FirstOrDefault();

                    reader.Close();

                    GetOutputParams(criteria, objectDb);
                }
            }

            return memberWinlossReport;
        }

        /// <summary>
        /// Get Column Extra.
        /// </summary>
        /// <param name="reader">Data Reader.</param>
        /// <param name="memberWinLoss">Member Win Loss.</param>
        /// <param name="colSuffix">Column Suffix.</param>
        protected override void GetColumnExtra(IDataReader reader, MemberWinLoss memberWinLoss, string colSuffix)
        {
            SetPropertyValue(reader, memberWinLoss, string.Format("Deposit{0}", colSuffix));
            SetPropertyValue(reader, memberWinLoss, string.Format("Withdrawal{0}", colSuffix));
            SetPropertyValue(reader, memberWinLoss, string.Format("PSP{0}", colSuffix));
            SetPropertyValue(reader, memberWinLoss, string.Format("Bonus{0}", colSuffix));
            SetPropertyValue(reader, memberWinLoss, string.Format("Adjustment{0}", colSuffix));
        }
    }
}