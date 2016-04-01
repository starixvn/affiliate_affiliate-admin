// --------------------------------------------------------------------------
// <copyright file="ReportService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2013.10.16">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Services
{
    using System.Collections.Generic;
    using Core.Services;
    using Criterias;
    using DTO;
    using Fanex.AffiliateAdmin.DataAccess;
    using Fanex.Common.Extensions;
    using Fanex.Common.Helper;
    using Fanex.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public sealed class ReportService : QueryService, IReportService
    {
        private readonly ICompanyWinLossByProductDataAccess _companyWinLossByProductDataAccess;

        private readonly ICompanyWinLossOverviewDataAccess _companyWinLossOverviewDataAccess;

        private readonly IMemberWinLossByProductDataAccess _memberWinLossByProductDataAccess;

        private readonly IMemberWinLossOverviewDataAccess _memberWinLossOverviewDataAccess;

        private readonly IAffiliateWinLossOverviewDataAccess _affiliateWinLossOverviewDataAccess;

        private readonly IAffiliateWinLossByProductDataAccess _affiliateWinLossByProductDataAccess;

        private readonly IAffiliateCommissionDataAccess _affiliateCommissionDataAccess;

        public ReportService()
        {
            _companyWinLossByProductDataAccess = new CompanyWinLossByProductDataAccess();

            _companyWinLossOverviewDataAccess = new CompanyWinLossOverviewDataAccess();

            _memberWinLossByProductDataAccess = new MemberWinLossByProductDataAccess();

            _memberWinLossOverviewDataAccess = new MemberWinLossOverviewDataAccess();

            _affiliateWinLossOverviewDataAccess = new AffiliateWinLossOverviewDataAccess();

            _affiliateWinLossByProductDataAccess = new AffiliateWinLossByProductDataAccess();

            _affiliateCommissionDataAccess = new AffiliateCommissionByProductDataAccess();
        }

        public CompanyWinlossByProductResult GetCompanyWinlossByProduct(CompanyWinlossByProductCriteria criteria)
        {
            return _companyWinLossByProductDataAccess.GetCompanyWinLossByProduct(criteria);
        }

        public CompanyWinlossByOverviewResult GetCompanyWinlossOverview(CompanyWinlossOverviewCriteria criteria)
        {
            return _companyWinLossOverviewDataAccess.GetCompanyWinLossOverview(criteria);
        }

        public IEnumerable<MemberStatistic> GetMemberStatistics(MemberStatisticReportCriteria criteria)
        {
            return GetData<MemberStatistic>(criteria);
        }

        public AffiliateWinLossByOverviewResult GetAffiliateWinLossOverview(AffiliateWinLossOverviewCriteria criteria)
        {
            return _affiliateWinLossOverviewDataAccess.GetAffiliateWinLossOverview(criteria);
        }

        public AffiliateWinLossByProductResult GetAffiliateWinLossByProduct(AffiliateWinLossByProductCriteria criteria)
        {
            return _affiliateWinLossByProductDataAccess.GetAffiliateWinLossByProduct(criteria);
        }

        public MemberWinLossByProductResult GetMemberWinlossByProduct(MemberWinlossByProductCriteria criteria)
        {
            return _memberWinLossByProductDataAccess.GetMemberWinLossByProduct(criteria);
        }

        public MemberWinLossByOverviewResult GetMemberWinlossOverview(MemberWinLossOverviewCriteria criteria)
        {
            return _memberWinLossOverviewDataAccess.GetMemberWinLossOverview(criteria);
        }

        public IEnumerable<QualifiedMember> GetQualifiedMembers(QualifiedMembersReportCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            if (!criteria.IsValid())
            {
                return new List<QualifiedMember>();
            }

            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objectDb.ExecuteReader(criteria.GetSpParams()))
                {
                    var dataList = new List<QualifiedMember>();
                    QualifiedMember member = null;

                    while (reader.Read())
                    {
                        var username = reader.ToString("Username");
                        if (member == null || !member.Username.Equals(username, System.StringComparison.OrdinalIgnoreCase))
                        {
                            member = new QualifiedMember();
                            member.Username = username;
                            member.Currency = reader.ToString("Currency");
                            member.QualifiedAmount = reader.ToDecimal("QualifiedAmount");
                            member.QualifiedAmountBase = reader.ToDouble("QualifiedAmountBase");
                            dataList.Add(member);
                        }

                        var productId = reader.ToInt("ProductId");
                        member.QualifiedMemberProducts.Add(
                            productId,
                            new QualifiedMemberProduct()
                        {
                            ProductId = productId,
                            ProductName = reader.ToString("ProductName"),
                            Turnover = reader.ToDouble("Turnover"),
                            TurnoverBase = reader.ToDouble("TurnoverBase")
                        });
                    }

                    return dataList;
                }
            }
        }

        public AffiliateCommissionResult GetAffiliateCommission(AffiliateCommissionCriteria criteria)
        {
            return _affiliateCommissionDataAccess.GetAffiliateCommissionByProduct(criteria);
        }
    }
}