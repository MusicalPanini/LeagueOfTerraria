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
            Projectile.arrow = true;
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.alpha = 0;
            Projectile.timeLeft = 240;
            Projectile.penetrate = -1;
            Projectile.hostile = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, TargonBossNPC.PanthColor.ToVector3() * (1 - (Projectile.alpha / 255f)));

            if (Projectile.ai[0] == 0)
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            }
            else
            {
                //Projectile.ai[1]++;
                //if ((int)Projectile.ai[1] == 90)
                //    Prime();

                if (Projectile.timeLeft < 51)
                {
                    Projectile.alpha += 5;
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

        public void Prime()
        {
            Projectile.tileCollide = false;
            Projectile.velocity = Vector2.Zero;
            Projectile.alpha = 255;
            Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y + (float)(Projectile.height / 2);
            Projectile.width = 1280;
            Projectile.height = 1280;
            Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
            Projectile.timeLeft = 2;
        }
    }
}
