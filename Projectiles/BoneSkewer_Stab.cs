using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.Buffs;
using TerraLeague.Projectiles.Spear;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class BoneSkewer_Stab : SpearProjectile
    {
        protected override float HoldoutRangeMax => 64;
        protected override float HoldoutRangeMin => 30;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bone Skewer");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.scale = 0.75f;
        }
        public override void AI()
        {
            
        }
    }
}
