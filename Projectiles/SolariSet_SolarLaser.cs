using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.Projectiles.Beam;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class SolariSet_SolarLaser : UncenteredPlayerBeamProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 88;
            projectile.height = 10;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.magic = true;
            projectile.hide = false;
            projectile.timeLeft = 300;

            dust1 = DustID.AmberBolt;
            dust2 = DustID.AmberBolt;
            dustScale = 3;

            soundID = 2;
            soundStyle = 15;
            soundPitch = 0;
            soundDelay = 25;

            trackMouse = false;
            turningFactor = 70;
            TargetImmunityFrames = 10;

            SpriteStart = new Rectangle(0, 0, 88, 22);
            SpriteMid = new Rectangle(0, 24, 88, 22);
            SpriteEnd = new Rectangle(0, 56, 88, 22);

            MaxDistance = 2000;
            maxCharge = 0;
            moveDistance = 30;
            lightColor = Color.Yellow;
        }

        public override void AI()
        {
            Projectile sigil = Main.projectile[(int)projectile.ai[0]];
            projectile.Center = sigil.Center;

            if (projectile.timeLeft <= 17)
                projectile.alpha += 15;

            BeamAI(projectile.Center, projectile.Center + Vector2.UnitY);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Buffs.Stunned>(), 60);
            base.OnHitNPC(target, damage, knockback, crit);
        }

        protected override void SetLaserPosition(Vector2 Center)
        {
            bool enableTileCollision = false;

            for (Distance = moveDistance; Distance <= MaxDistance; Distance += 5f)
            {
                var start = projectile.Left + projectile.velocity * Distance;
                if (enableTileCollision)
                {
                    if (Collision.SolidCollision(start, 80, 1))
                    {
                        if (enableTileCollision)
                        {
                            Distance -= 4f;
                            break;
                        }
                    }
                }
                else if (!enableTileCollision)
                {
                    bool collision = Collision.SolidCollision(start, 80, 1);
                    if (!collision)
                    {
                        enableTileCollision = true;
                    }
                }
            }
        }

        protected override void SpawnDusts(Vector2 Center)
        {
            for (float i = moveDistance; i <= Distance; i += 4)
            {
                Vector2 origin = projectile.Center + i * projectile.velocity;
                if (Main.rand.NextBool(5))
                {
                    Dust dust = Dust.NewDustDirect(origin - new Vector2(44, 0), 88, 10, DustID.AmberBolt, 0, -3, 0, default, 1.5f * (1 - (projectile.alpha / 255f)));
                    dust.noGravity = true;
                }
            }

            for (int i = 0; i < 2; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.Left + (Distance + 4) * projectile.velocity, 88, 10, dust1, 0, -3, 0, default, 4 * (1 - (projectile.alpha / 255f)));
                dust.noGravity = true;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            bool preDraw = base.PreDraw(spriteBatch, lightColor);

            Projectile sigil = Main.projectile[(int)projectile.ai[0]];
            Texture2D texture = Main.projectileTexture[sigil.type];
            if (texture != null)
            {
                spriteBatch.Draw
                (
                    texture,
                    new Vector2
                    (
                        sigil.position.X - Main.screenPosition.X + sigil.width * 0.5f,
                        sigil.position.Y - Main.screenPosition.Y + sigil.height - (texture.Height) * 0.5f
                    ),
                    new Rectangle(0, (texture.Height) * sigil.frame, texture.Width, texture.Height),
                    Color.White * (1 - (projectile.alpha / 255f)),
                    sigil.rotation,
                    new Vector2(texture.Width, texture.Width) * 0.5f,
                    sigil.scale,
                    SpriteEffects.None,
                    1f
                );
            }

            return preDraw;
        }
    }
}
