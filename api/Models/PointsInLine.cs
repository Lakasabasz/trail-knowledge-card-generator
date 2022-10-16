using System;
using System.Collections.Generic;

namespace api
{
    public partial class PointsInLine
    {
        public int Linenr { get; set; }
        public int Postid { get; set; }
        public double Kilometer { get; set; }

        public virtual RailroadLine LinenrNavigation { get; set; } = null!;
        public virtual RailroadPoint Post { get; set; } = null!;
    }
}
