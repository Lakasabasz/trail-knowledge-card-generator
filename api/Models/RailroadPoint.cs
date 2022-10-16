using System;
using System.Collections.Generic;

namespace api
{
    public partial class RailroadPoint
    {
        public RailroadPoint()
        {
            PointsInLines = new HashSet<PointsInLine>();
        }

        public int Postid { get; set; }
        public string Postname { get; set; } = null!;
        public bool Platform { get; set; }
        public bool Requeststop { get; set; }
        public bool Loadingpoint { get; set; }
        public int Idcategory { get; set; }

        public virtual Category IdcategoryNavigation { get; set; } = null!;
        public virtual ICollection<PointsInLine> PointsInLines { get; set; }
    }
}
