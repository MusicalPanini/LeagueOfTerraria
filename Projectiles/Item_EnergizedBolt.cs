using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TerraLeague.Projectiles.Homing;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class Item_EnergizedBolt : RichochetProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Energized Bolt");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.timeLeft = 300;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.alpha = 255;
            projectile.extraUpdates = 8;
            projectile.netImportant = true;
            projectile.ranged = true;
            projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;

            projectile.penetrate = (int)projectile.ai[1] == 1 ? 9 : 6;
            SetHomingDefaults(true, 480, 301);
            CanOnlyHitTarget = true;
            NPC_CanTargetDummy = true;
            NPC_CanTargetCritters = true;
            MaxVelocity = 6;
        }

        public override void AI()
        {
            if (projectile.timeLeft == 300)
            {
                Main.PlaySound(new LegacySoundStyle(3, 53), projectile.position);
            }

            for (int i = 0; i < 4; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.AncientLight, 0, 0, 0, new Color(255, 255, 0, 150), 1f);
                dust.velocity *= i == 3? 3 : 0.3f;
                dust.scale = i == 4 ? 1.25f : 1;
                dust.noGravity = true;
            }

            Lighting.AddLight(projectile.position, 1f, 1f, 0f);
            base.AI();
        }

        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);
        }
    }
}
