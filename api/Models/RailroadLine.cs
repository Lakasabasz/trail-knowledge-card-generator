using System;
using System.Collections.Generic;

namespace api
{
    public partial class RailroadLine
    {
        public RailroadLine()
        {
            PointsInLines = new HashSet<PointsInLine>();
        }

        public int Linenr { get; set; }
        public string Linename { get; set; } = null!;

        public virtual ICollection<PointsInLine> PointsInLines { get; set; }
    }
}
