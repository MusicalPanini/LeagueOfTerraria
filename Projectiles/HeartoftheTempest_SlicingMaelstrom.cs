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
    class HeartoftheTempest_SlicingMaelstrom : ModProjectile
    {
        int framecount2 = 29;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Slicing Maelstrom");
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.DontAttachHideToAlpha[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 150;
            Projectile.height = 150;
            Projectile.timeLeft = 150;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.scale = 1;
        }

        public override void AI()
        {
            if (Main.projectile[(int)Projectile.ai[0]].active)
                Projectile.Center = Main.projectile[(int)Projectile.ai[0]].Center;
            else
                Projectile.Kill();

            Lighting.AddLight(Projectile.Center, 0f, 1f, 1f);
            Projectile.rotation += 0.05f;

            Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Electric, 0, 0, 0, new Color(0, 255, 255), 1f);
            dust.noGravity = true;

            AnimateProjectile();
        }

        public void AnimateProjectile()
        {
            Projectile.frameCounter++;
            framecount2++;
            if (Projectile.frameCounter >= 3)
            {
                Projectile.frame++;
                Projectile.frame %= 4; 
                Projectile.frameCounter = 0;
            }
            if (framecount2 >= 30)
            {
                framecount2 = 0;
                TerraLeague.PlaySoundWithPitch(Projectile.Center, 3, 53, 0.25f);
            }
        }
    }
}
