using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.Projectiles.Whip;

namespace TerraLeague.Projectiles
{
    public class StrangleThorn_Whip : WhipProjectile
    {
        internal override Rectangle HandleFrame => new Rectangle(0, 0, 22, 22);
        internal override Rectangle Segment1Frame => new Rectangle(0, 32, 22, 18);
        internal override Rectangle Segment2Frame => new Rectangle(0, 60, 22, 18);
        internal override Rectangle Segment3Frame => new Rectangle(0, 88, 22, 18);
        internal override Rectangle TipFrame => new Rectangle(0, 110, 22, 20);

        public override void AI()
        {
            WhipAI();
        }
    }
}
