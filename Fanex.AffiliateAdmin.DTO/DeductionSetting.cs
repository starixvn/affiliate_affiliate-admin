// --------------------------------------------------------------------------
// <copyright file="DeductionSetting.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Joseph.Pham" date="2013.9.10">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.DTO
{
    /// <summary>
    /// Deduction Setting class.
    /// </summary>
    public class DeductionSetting
    {
        public byte DeductionID { get; set; }

        public byte ProductID { get; set; }

        public int GroupID { get; set; }

        public decimal DeductedPercentage { get; set; }
    }
}