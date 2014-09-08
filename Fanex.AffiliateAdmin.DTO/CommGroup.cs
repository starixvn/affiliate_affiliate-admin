// --------------------------------------------------------------------------
// <copyright file="CommGroup.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Joseph.Pham" date="2013.9.9">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.DTO
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Commission Group class.
    /// </summary>
    public class CommGroup
    {
        #region Fields

        private Dictionary<string, decimal> _generalDeductions;

        private Dictionary<string, object> _products;

        private IEnumerable<CommGroupTierSetting> _details;

        #endregion

        #region Properties

        public int GroupID { get; set; }

        public string GroupName { get; set; }

        public string Description { get; set; }

        public string Creator { get; set; }

        public DateTime CreatedDate { get; set; }

        public int NoOfAffiliate { get; set; }

        public short CurrencyID { get; set; }

        public string Currency { get; set; }

        public decimal QualifiedAmount { get; set; }

        public bool IsAllowedToDelete { get; set; }

        public decimal Deduction { get; set; }

        public byte HasCashback { get; set; }

        public Dictionary<string, decimal> GeneralDeductions
        {
            get
            {
                return _generalDeductions;
            }
        }

        public Dictionary<string, object> Products
        {
            get
            {
                return _products;
            }
        }

        public IEnumerable<CommGroupTierSetting> Details
        {
            get
            {
                return _details;
            }
        } 

        #endregion

        #region Methods

        public void AddGeneralDeductions(Dictionary<string, decimal> deductions)
        {
            _generalDeductions = deductions;
        }

        public void AddProducts(Dictionary<string, object> products)
        {
            _products = products;
        }

        public void AddDetails(IEnumerable<CommGroupTierSetting> details)
        {
            _details = details;
        }

        #endregion
    }
}