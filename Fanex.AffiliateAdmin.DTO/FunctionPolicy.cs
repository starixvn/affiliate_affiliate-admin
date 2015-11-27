// --------------------------------------------------------------------------
// <copyright file="FunctionPolicy.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Johnny.Trinh" date="2013.07.30">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.DTO
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Function policy info.
    /// </summary>
    [Serializable]
    public class FunctionPolicy
    {
        private const int ReadPolicy = 1;

        private const int HostPolicy = 3;

        public int FuncID { get; set; }

        public string FuncName { get; set; }

        public string FuncDesc { get; set; }

        public string FileNames { get; set; }

        public int ParentId { get; set; }

        public int DisplayOrder { get; set; }

        public int SubFuncCount { get; set; }

        public string System { get; set; }

        public string Username { get; set; }

        public byte Policy { get; set; }

        public byte GPolicy { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "Accept contain underscores")]
        public byte Actual_Policy { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime ModifyDate { get; set; }

        public string GroupName { get; set; }

        public bool CanRead
        {
            get
            {
                return this.Actual_Policy == ReadPolicy || this.Actual_Policy == HostPolicy;
            }
        }

        public bool CanHost
        {
            get
            {
                return this.Actual_Policy == HostPolicy;
            }
        }

        public bool IndividualRead 
        {
            get 
            {
                return this.Policy == ReadPolicy || this.Policy == HostPolicy;
            }
        }

        public bool IndividualHost
        {
            get
            {
                return this.Policy == HostPolicy;
            }
        }

        public bool GroupRead
        {
            get
            {
                return this.GPolicy == ReadPolicy;
            }
        }

        public bool GroupHost
        {
            get
            {
                return this.GPolicy == HostPolicy;
            }
        }
    }
}
