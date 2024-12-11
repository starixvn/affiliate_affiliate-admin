// -----------------------------------------------------------------------
// <copyright file="PolicyMultiUserViewModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Models
{
    using DTO;
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PolicyMultiUserViewModel
    {
        public string CustIds { get; set; }

        public IEnumerable<FunctionPolicy> PolicyMultiUserList { get; set; }
    }
}
