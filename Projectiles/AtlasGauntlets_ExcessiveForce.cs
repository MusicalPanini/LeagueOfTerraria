using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class AtlasGauntlets_ExcessiveForce : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Excessive Force");
        }

        public override void SetDefaults()
        {
            Projectile.width = 15;
            Projectile.height = 15;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 3;
            Projectile.alpha = 255;
            Projectile.scale = 1f;
            Projectile.timeLeft = 60;
            Projectile.extraUpdates = 4;
            //Projectile.usesLocalNPCImmunity = true;
            //Projectile.localNPCHitCooldown = -1;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            Dust dust;
            if (Projectile.soundDelay == 0)
            {
                Terraria.Audio.SoundEngine.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 14), Projectile.Center);
                dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, 0, 0, 0, default, 1f);
                dust.noGravity = true;
                dust.noLight = true;
            }
            Projectile.soundDelay = 100;

            dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, 0, 0, 0, default, 2f);
            dust.noGravity = true;
            dust.noLight = true;

            if (Main.rand.Next(0, 3) == 0)
            {
                dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6, 0, 3, 0, default, 1f);
                //dust.noLight = true;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }

        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);
        }

        public override bool? CanHitNPC(NPC target)
        {
            if ((int)Projectile.ai[0] == target.whoAmI)
                return false;
            return base.CanHitNPC(target);
        }
    }
}
