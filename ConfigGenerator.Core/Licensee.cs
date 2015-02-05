// --------------------------------------------------------------------------
// <copyright file="Licensee.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2015.1.29">Created</change>
// </history>

namespace ConfigGenerator.Core
{
    using System;

    /// <summary>
    /// License Class.
    /// </summary>
    [Serializable]
    public class Licensee
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