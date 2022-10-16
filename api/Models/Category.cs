using System;
using System.Collections.Generic;

namespace api
{
    public partial class Category
    {
        public Category()
        {
            RailroadPoints = new HashSet<RailroadPoint>();
        }

        public int Idcategory { get; set; }
        public string Discriminant { get; set; } = null!;
        public string Fullname { get; set; } = null!;

        public virtual ICollection<RailroadPoint> RailroadPoints { get; set; }
    }
}
