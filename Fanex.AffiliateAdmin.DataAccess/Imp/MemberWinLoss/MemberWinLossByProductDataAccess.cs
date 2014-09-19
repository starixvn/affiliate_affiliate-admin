// --------------------------------------------------------------------------
// <copyright file="MemberWinLossByProductDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.9.19">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.DataAccess
{
    using System.Data;
    using Common.Extensions;
    using Criterias;
    using Data;
    using DTO;

    /// <summary>
    /// Account data access.
    /// </summary>
    public class MemberWinLossByProductDataAccess : MemberWinLossDataAccess, IMemberWinLossByProductDataAccess
    {
        /// <summary>
        /// Get data for Member win-loss.
        /// </summary>
        /// <param name="criteria">Criteria input.</param>
        /// <returns>Member win-loss report.</returns>
        public MemberWinLossByProductResult GetMemberWinLossByProduct(MemberWinlossByProductCriteria criteria)
        {
            var memberWinlossReport = new MemberWinLossByProductResult();

            if (criteria.IsValid())
            {
                using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    var param = criteria.GetSpParams();

                    var reader = objectDb.ExecuteReader(param);

                    memberWinlossReport.MemberWinlossList = GetMemberWinlossList(reader);

                    memberWinlossReport.TotalMemberWinlossInMemberCurrencies = GetTotalMemberWinlossInMemberCurrency(reader);

                    memberWinlossReport.TotalMemberWinlossInSystemCurrency = GetTotalMemberWinlossInSystemCurrency(reader);

                    reader.Close();

                    GetOutputParams(criteria, objectDb);
                }
            }

            return memberWinlossReport;
        }

        protected override void GetColumnExtra(IDataReader reader, MemberWinLoss memberWinLoss)
        {
            memberWinLoss.ProductId = reader.ToInt("ProductID");
            memberWinLoss.ProductName = reader.ToString("ProductName");
        }

        protected override void GetColumnExtraInTotalResult(IDataReader reader, MemberWinLoss memberWinLoss)
        {
            GetColumnExtra(reader, memberWinLoss);
        }
    }
}