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