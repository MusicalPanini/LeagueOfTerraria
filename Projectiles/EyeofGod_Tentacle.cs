using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class EyeofGod_Tentacle : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tentacle");
            Main.projFrames[Projectile.type] = 8;
        }

        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 108;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.alpha = 100;
            Projectile.scale = 1f;
            Projectile.timeLeft = Projectile.SentryLifeTime;
            Projectile.sentry = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.tileCollide = true;
            Projectile.friendly = false;
        }

        public override void AI()
        {
            if (Projectile.ai[1] >= 0)
            {
                Projectile.timeLeft = (int)Projectile.ai[1];
                Projectile.ai[1] = -1;
            }
            if (Projectile.timeLeft == Projectile.SentryLifeTime)
            {
                for (int i = 0; i < 20; i++)
                {
                    Dust dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y + 16), Projectile.width, Projectile.height, DustID.BlueTorch, 0f, -2f, 200, new Color(0, 255, 201), 5f);
                    dust.noGravity = true;
                    dust.velocity.Y -= 2;

                    dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y + 16), Projectile.width, Projectile.height, DustID.BlueTorch, 0f, -1f, 200, new Color(0, 255, 201), 4f);
                    dust.noGravity = true;
                    dust.velocity.Y -= 3;
                }
            }

            Dust dust2 = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.Bottom.Y - 4), Projectile.width, 4, DustID.BubbleBlock, 0f, 0, 100, new Color(0, 255, 201), 1f);
            dust2.fadeIn = 1.3f;
            dust2.noGravity = true;
            dust2.velocity.Y *= 0.2f;

            if (Projectile.ai[0] <= 0)
            {
                for (int i = 0; i < Main.npc.Length; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.active && !npc.friendly && !npc.immortal)
                    {
                        if (npc.Hitbox.Intersects(new Rectangle((int)Projectile.Left.X - 128, (int)Projectile.Top.Y + 32, Projectile.width + 272, Projectile.height)))
                        {
                            int direction = npc.Center.X > Projectile.Center.X ? 1 : -1;

                            Projectile.ai[1] = Projectile.timeLeft;
                            Projectile.netUpdate = true;
                            Projectile proj = Projectile.NewProjectileDirect(Projectile.GetProjectileSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<EyeofGod_TentacleAttack>(), Projectile.damage, Projectile.knockBack, Projectile.owner, direction, Projectile.ai[1]);
                            proj.originalDamage = Projectile.originalDamage;
                            Projectile.Kill();
                            break;
                        }
                    }
                }
            }
            else
            {
                Projectile.ai[0]--;
            }

            Lighting.AddLight(Projectile.Center, 0, 0.1f, 0.05f);

            AnimateProjectile();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);
        }

        public void AnimateProjectile()
        {
            Projectile.frameCounter++;
            //if (Projectile.frameCounter >= ((Projectile.frame == 3 || Projectile.frame == 7) ? 12 : 8))
            if (Projectile.frameCounter >= 12)
            {
                Projectile.frame++;
                Projectile.frame %= 8;
                Projectile.frameCounter = 0;
            }
        }
    }
}
