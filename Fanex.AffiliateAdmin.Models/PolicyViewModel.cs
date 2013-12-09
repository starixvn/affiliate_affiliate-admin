// -----------------------------------------------------------------------
// <copyright file="PolicyViewModel.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// --------------------------------------------------------------------------
// <history>
//   <change who="Muller.Nguyen" date="2013.09.01">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Models
{
    using System.Collections.Generic;
    using DTO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PolicyViewModel
    {
        public int CustId { get; set; }

        public int GroupId { get; set; }

        public IEnumerable<FunctionPolicy> PolicyUserList { get; set; }
    }
}
