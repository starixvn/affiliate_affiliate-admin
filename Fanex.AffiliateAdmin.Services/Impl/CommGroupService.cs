namespace Fanex.AffiliateAdmin.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common.Helper;
    using Core.Configurations;
    using Core.Services;
    using Criterias;
    using DTO;
    using Fanex.AffiliateAdmin.Resources;
    using Fanex.Data.Repository;

    public class CommGroupService : QueryService, ICommGroupService
    {
        public CommGroupService()
            : base(null)
        {
        }

        public IEnumerable<CommGroupTierSetting> GetTierSettings(TierSettingCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            var settings = GetDataOutput<CommGroupTierSetting>(criteria);

            foreach (var setting in settings)
            {
                var productName = setting.ProductName;
                var translatedProduct = Lang.ResourceManager.GetString(productName.Replace(" ", string.Empty));
                setting.ProductName = string.IsNullOrEmpty(translatedProduct) ? productName : translatedProduct;
            }

            return settings;
        }

        public IEnumerable<T> GetListData<T>(ICriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            return GetDataOutput<T>(criteria);
        }

        public IEnumerable<CommGroup> GetById(CommGroupSelByIdCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            return GetDataOutput<CommGroup>(criteria);
        }

        public void BuildGroupData(IEnumerable<CommGroup> groups, DateTime date, string groupIDList)
        {
            var detailCriteria = new TierSettingCriteria { GroupIDList = groupIDList, Date = date };

            var settingCriteria = new DeductionSettingCriteria { GroupIDList = groupIDList, Date = date };

            var tierSettings = GetTierSettings(detailCriteria);

            var deductionSettings = GetDeductionSettings(settingCriteria);

            foreach (var group in groups)
            {
                var details = tierSettings.Where(x => x.GroupId == group.GroupID).ToList();

                var products = new Dictionary<string, object>();

                GetCommGroupSettings(details, products, group, deductionSettings);
            }
        }

        private static void GetCommGroupSettings(
            List<CommGroupTierSetting> tierSettings,
            Dictionary<string, object> products,
            CommGroup group,
            IEnumerable<DeductionSetting> deductionSettings)
        {
            var allDeductions = AppSettings.DeductionTypes;

            foreach (var detail in tierSettings)
            {
                var id = detail.ProductID.ToString();
                if (!products.ContainsKey(id))
                {
                    products.Add(
                        id,
                        new
                        {
                            NumOfItems = tierSettings.Count(x => x.GroupId == group.GroupID && x.ProductID == detail.ProductID),
                            Deductions = GetDeductionSettings(deductionSettings, allDeductions, group, detail)
                        });
                }
            }

            group.IsAllowedToDelete = group.NoOfAffiliate == 0 && group.CurrencyID != 0;
            group.AddProducts(products.ToDictionary(x => x.Key, x => x.Value));
            group.AddDetails(tierSettings.ToList());

            if (group.HasCashback > 0)
            {
                var deduction = new Dictionary<string, decimal>();
                var deductions = allDeductions.Where(x => x.ProductID == 0);
                if (deductions.Count() > 0)
                {
                    var settings = deductionSettings.Where(x => x.GroupID == group.GroupID).ToList();

                    foreach (var item in deductions)
                    {
                        var setting = settings.Where(x => x.DeductionID == item.DTypeID).FirstOrDefault();
                        deduction.Add(Lang.ResourceManager.GetString(item.DTypeName.Replace(" ", string.Empty)), setting == null ? -1 : setting.DeductedPercentage);
                    }
                }

                group.AddGeneralDeductions(deduction);
            }
        }

        private static Dictionary<string, decimal> GetDeductionSettings(
            IEnumerable<DeductionSetting> deductionSettings,
            IEnumerable<DeductionType> allDeductions,
            CommGroup group,
            CommGroupTierSetting detail)
        {
            var deduction = new Dictionary<string, decimal>();
            var deductions = allDeductions.Where(x => x.ProductID == detail.ProductID);

            if (deductions.Count() > 0)
            {
                var settings = deductionSettings.Where(x => x.GroupID == group.GroupID);

                foreach (var item in deductions)
                {
                    var setting = settings.Where(x => x.DeductionID == item.DTypeID).FirstOrDefault();
                    deduction.Add(Lang.ResourceManager.GetString(item.DTypeName.Replace(" ", string.Empty)), setting == null ? -1 : setting.DeductedPercentage);
                }
            }

            return deduction;
        }

        private IEnumerable<DeductionSetting> GetDeductionSettings(DeductionSettingCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            return GetDataOutput<DeductionSetting>(criteria);
        }
    }
}