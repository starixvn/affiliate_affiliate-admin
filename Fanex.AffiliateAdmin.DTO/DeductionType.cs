// --------------------------------------------------------------------------
// <copyright file="DeductionType.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Joseph.Pham" date="2013.9.9">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.DTO
{
    using System;

    /// <summary>
    /// Deduction Type class.
    /// </summary>
    public class DeductionType
    {
        public double DeductedPercentage { get; set; }

        public string DTypeDesc { get; set; }

        public byte DTypeID { get; set; }

        public string DTypeName { get; set; }

        public byte ProductID { get; set; }

        public DateTime? RemovedDate { get; set; }
    }
}