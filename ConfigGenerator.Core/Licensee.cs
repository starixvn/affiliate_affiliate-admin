// --------------------------------------------------------------------------
// <copyright file="SiteInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.8.19">Created</change>
// </history>

using System;

namespace ConfigGenerator.Core
{
    /// <summary>
    /// Site information.
    /// </summary>
    [Serializable]
    public class Licensee : MarshalByRefObject
    {
        public string Name { get; set; }

        public bool Status { get; set; }

        public string Environment { get; set; }

        public string Model { get; set; }
        
        public Licensee Clone()
        {
            return new Licensee
                {
                    Name = this.Name,
                    Environment = this.Environment,
                    Status = this.Status,
                    Model = Model
                };
        }
    }
}