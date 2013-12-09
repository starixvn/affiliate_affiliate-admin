// --------------------------------------------------------------------------
// <copyright file="ResultViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.06.07">Created</change>
// </history>
namespace Fanex.AffiliateAdmin.Models
{
    /// <summary>
    /// Result View Model.
    /// </summary>
    public class ResultViewModel
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }
    }
}