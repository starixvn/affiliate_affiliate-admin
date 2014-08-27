// --------------------------------------------------------------------------
// <copyright file="DeductionType.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Joseph.Pham" date="2013.9.9">Created</change>
// </history>

using System;

namespace Fanex.AffiliateAdmin.DTO
{
    /// <summary>
    /// Deduction Type class.
    /// </summary>
    public class DeductionType
    {
        public byte DTypeID { get; set; }

        public byte ProductID { get; set; }

        public string DTypeName { get; set; }

        public string DTypeDesc { get; set; }

        public double DeductedPercentage { get; set; }

        public DateTime? RemovedDate { get; set; }
    }
}