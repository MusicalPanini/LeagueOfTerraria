using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    class HeartoftheTempest_LightningRush : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lightning Rush");
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.DontAttachHideToAlpha[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.timeLeft = 1000;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.scale = 1;
            Projectile.hide = true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (player.active && modPlayer.lightningRush || Projectile.timeLeft > 990)
                Projectile.Center = player.MountedCenter;
            else
                Projectile.Kill();

            Lighting.AddLight(Projectile.Center, 0f, 1f, 1f);

            Dust dust = Dust.NewDustPerfect(Projectile.Center, 226, null, 0, default, 2);
            dust.noGravity = true;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}
