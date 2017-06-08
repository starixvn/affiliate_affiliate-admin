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

        public double NextDeductedPercentage { get; set; }
    }
}