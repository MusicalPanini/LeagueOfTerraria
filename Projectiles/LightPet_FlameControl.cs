using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.Buffs;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    class LightPet_FlameControl : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Eternal Flame");
            ProjectileID.Sets.LightPet[Projectile.type] = true;
            Main.projPet[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.timeLeft *= 5;
            Projectile.penetrate = -1;
            Projectile.netImportant = true;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.alpha = 255;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.Center = player.MountedCenter;
            if (!player.active)
            {
                Projectile.active = false;
                return;
            }
            if (player.HasBuff(ModContent.BuffType<CandlePet>()))
            {
                Projectile.timeLeft = 2;
            }

            if (Main.LocalPlayer.whoAmI == player.whoAmI)
            {
                if (Projectile.soundDelay == 0)
                {
                    Projectile.NewProjectileDirect(Projectile.GetProjectileSource_FromThis(), Projectile.Center, new Vector2(Main.rand.NextFloat(0, 5f), 0).RotatedByRandom(MathHelper.TwoPi), ModContent.ProjectileType<LightPet_Flame>(), 0, 0, player.whoAmI);
                    Projectile.soundDelay = Main.rand.Next(150, 300);
                }
            }
        }
    }
}
