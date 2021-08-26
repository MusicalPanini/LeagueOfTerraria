using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.Achievements;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class LightPistol_PiercingLight : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Piercing Light");
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.timeLeft = 300;
            Projectile.friendly = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = false;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if ((int)Projectile.ai[1] == 1)
            {
                Lighting.AddLight(Projectile.Center, Color.White.ToVector3());
                for (int i = 0; i < 3; i++)
                {
                    Vector2 dustBoxPosition = new Vector2(Projectile.position.X + 6, Projectile.position.Y + 6);
                    int dustBoxWidth = Projectile.width - 12;
                    int dustBoxHeight = Projectile.height - 12;
                    Dust dust = Dust.NewDustDirect(dustBoxPosition, dustBoxWidth, dustBoxHeight, 264, 0f, 0f, 100, default, 1 * (Projectile.timeLeft/50f) + 1);
                    dust.noGravity = true;
                    dust.velocity *= 0.1f;
                    dust.velocity += Projectile.velocity * 0.1f;
                    dust.position.X -= Projectile.velocity.X / 3f * (float)i;
                    dust.position.Y -= Projectile.velocity.Y / 3f * (float)i;
                }
            }
            else
            {
                //int dir = player.Center.X > Projectile.Center.X ? -1 : 1;
                //player.ChangeDir(dir);

                Projectile.Center = player.MountedCenter + new Vector2(0, -6) + new Vector2(38,0).RotatedBy(Projectile.velocity.ToRotation());

                for (int k = 0; k < 2 + 1; k++)
                {
                    float scale = 0.8f;
                    if (k % 2 == 1)
                    {
                        scale = 0.6f;
                    }
                    
                    Vector2 position = Projectile.Center + ((float)Main.rand.NextDouble() * 6.28318548f).ToRotationVector2() * (12f - (float)(2 * 2));
                    Dust dust = Dust.NewDustDirect(position - Vector2.One * 8f, 16, 16, 264, 0, 0, 0, default, scale);
                    dust.velocity = Vector2.Normalize(Projectile.Center - position) * 1.5f * (10f - (float)2 * 2f) / 10f;
                    dust.noGravity = true;
                    dust.customData = player;
                }

                Projectile.localAI[0] ++;
                if (Projectile.localAI[0]  > 20)
                {
                    Projectile.ai[1] = 1;
                    Projectile.friendly = true;
                    Projectile.timeLeft = 50;
                    Projectile.extraUpdates = 16;
                    //Projectile.velocity = new Vector2(10, 0).RotatedBy(Projectile.rotation);

                    TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 12, -1);
                }
            }

            base.AI();
        }

        public override void Kill(int timeLeft)
        {
            if ((int)Projectile.ai[1] == 1 && timeLeft > 0)
            {
                for (int i = 0; i < 12; i++)
                {
                    Dust dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 264, Projectile.velocity.X * 0.25f, Projectile.velocity.Y * 0.25f, 0);
                    dust.noGravity = true;
                    dust.scale = 1 * (timeLeft / 80f) + 1;
                }
            }
            base.Kill(timeLeft);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 10;
            return true;
        }
    }
}
