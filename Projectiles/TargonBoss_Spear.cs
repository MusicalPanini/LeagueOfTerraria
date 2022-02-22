using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TerraLeague.NPCs;
using TerraLeague.NPCs.TargonBoss;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class TargonBoss_Spear : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Skyfall of Areion");
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.alpha = 0;
            Projectile.timeLeft = 50 + 30;
            Projectile.penetrate = -1;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, TargonBossNPC.PanthColor.ToVector3() * (1 - (Projectile.alpha / 255f)));
            if (Projectile.ai[0] == 0)
            {
                if (Projectile.timeLeft > 50)
                {
                    if (Projectile.velocity.Length() > 0)
                    {
                        Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
                        Projectile.velocity *= 0;
                    }

                    TerraLeague.DustLine(Projectile.Center, Projectile.Center + new Vector2(50 * 32, 0).RotatedBy(Projectile.rotation - MathHelper.PiOver2), DustID.PortalBolt, 0.05f, 1, TargonBossNPC.PanthColor);
                }
                else
                {
                    TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 1, -0.5f);
                    Projectile.velocity = new Vector2(32, 0).RotatedBy(Projectile.rotation - MathHelper.PiOver2);
                }
            }
            else
            {
                if (Projectile.timeLeft < 10)
                {
                    Projectile.alpha += 26;
                }
            }

            base.AI();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
            Projectile.velocity *= 0;
            Projectile.position += oldVelocity;
            Projectile.ai[0] = 1;
            return false;
        }

        public override void Kill(int timeLeft)
        {
            //TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 14, 0);

            //TerraLeague.DustBorderRing(Projectile.width / 2, Projectile.Center, 6, default, 2);
        }
    }
}
