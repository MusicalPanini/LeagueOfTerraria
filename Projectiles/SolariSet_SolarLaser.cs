using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Enums;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class SolariSet_SolarLaser : ModProjectile
    {
        private const float MaxChargeValue = 0;
        private const float MoveDistance = 60f;
        private float Length = 500;
        public float Distance
        {
            get { return projectile.ai[1]; }
            set { projectile.ai[1] = value; }
        }

        public float Charge
        {
            get { return projectile.localAI[0]; }
            set { projectile.localAI[0] = value; }
        }

        public bool IsAtMaxCharge => Charge == MaxChargeValue;

        public bool AtMaxCharge { get { return Charge >= MaxChargeValue; } }

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
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
                DrawLaser(spriteBatch, Main.projectileTexture[projectile.type], Main.projectile[(int)projectile.ai[0]].Center,
                    projectile.velocity, 22, projectile.damage, -1.57f, 1f, 1000f, Color.White, (int)MoveDistance);
            return false;
        }

        public void DrawLaser(SpriteBatch spriteBatch, Texture2D texture, Vector2 start, Vector2 unit, float step, int damage, float rotation = 0f, float scale = 1f, float maxDist = 2000f, Color color = default(Color), int transDist = 50)
        {
            float r = unit.ToRotation() + rotation;
            color = Color.White;
            color *= 1 - (projectile.alpha/255f);

            #region Draw laser body
            for (float i = transDist; i <= Distance + 24; i += step)
            {
                Color c = color;
                Vector2 origin = start + (i - 24) * unit;
                spriteBatch.Draw(texture, origin - Main.screenPosition,
                    new Rectangle(0, 24, 88, 22), i < transDist ? Color.Transparent : c, r,
                    new Vector2(88 * .5f, 30 * .5f), scale, 0, 0);

                if (Main.rand.NextBool(5))
                {
                    Dust dust = Dust.NewDustDirect(origin - new Vector2(44, 0), 88, 10, Terraria.ID.DustID.AmberBolt, 0, -3, 0, default, 1.5f * (1 - (projectile.alpha / 255f)));
                    dust.noGravity = true;
                }
            }
            #endregion

            #region Draw laser tail
            spriteBatch.Draw(texture, start - Main.screenPosition + (unit * 10),
                new Rectangle(0, 0, 88, 22), color, r, new Vector2(88 * .5f, 22 * .5f), scale, 0, 0f);

            
            #endregion

            #region Draw laser head
            spriteBatch.Draw(texture, start + (Distance + step - 22) * unit - Main.screenPosition,
                new Rectangle(0, 44, 88, 34), color, r, new Vector2(88 * .5f, 34 * .5f), scale, 0, 0);
            for (int i = 0; i < 2; i++)
            {
                Dust dust = Dust.NewDustDirect(start + (Distance + step) * unit - new Vector2(44, 10), 88, 10, Terraria.ID.DustID.AmberBolt, 0, -3, 0, default, 4 * (1 - (projectile.alpha / 255f)));
                dust.noGravity = true;
            }

            #endregion

            Projectile sigil = Main.projectile[(int)projectile.ai[0]];
            texture = Main.projectileTexture[sigil.type];
            spriteBatch.Draw
            (
                texture,
                new Vector2
                (
                    sigil.position.X - Main.screenPosition.X + sigil.width * 0.5f,
                    sigil.position.Y - Main.screenPosition.Y + sigil.height - (texture.Height) * 0.5f
                ),
                new Rectangle(0, (texture.Height) * sigil.frame, texture.Width, texture.Height),
                color,
                sigil.rotation,
                new Vector2(texture.Width, texture.Width) * 0.5f,
                sigil.scale,
                SpriteEffects.None,
                1f
            );
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (AtMaxCharge)
            {
                Projectile sigil = Main.projectile[(int)projectile.ai[0]];
                Vector2 unit = projectile.velocity;
                float point = 0f;
                return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), sigil.Center,
                    sigil.Center + unit * Distance, 88, ref point);
            }
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Buffs.Stunned>(), 60);
            target.immune[projectile.owner] = 10;
        }

        public override void AI()
        {
            if (projectile.timeLeft <= 17)
                projectile.alpha += 15;

            if (projectile.soundDelay == 0)
            {
                projectile.soundDelay = 25;
                Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 15), projectile.Center);
            }

            Vector2 mousePos = Main.MouseWorld;
            Projectile sigil = Main.projectile[(int)projectile.ai[0]];

            projectile.velocity = new Vector2(0, 1);
            projectile.netUpdate = true;
            projectile.Center = sigil.Center;

            Vector2 start = projectile.Left + new Vector2(4, 0);
            Vector2 unit = projectile.velocity;
            unit *= -1;

            bool enableTileCollision = true;

            if (!Collision.CanHitLine(sigil.Center, 0, 0, sigil.Center + projectile.velocity * MoveDistance, 0, 0))
            {
                enableTileCollision = false;
            }

            for (Distance = MoveDistance; Distance <= 5000; Distance += 4f)
            {
                start = projectile.Left + projectile.velocity * Distance;
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

            DelegateMethods.v3_1 = new Vector3(0.8f, 0.8f, 1f);
            Utils.PlotTileLine(projectile.Center, projectile.Center + projectile.velocity * (Distance - MoveDistance), 26,
                DelegateMethods.CastLight);
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public override void CutTiles()
        {
            DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
            Vector2 unit = projectile.velocity;
            Utils.PlotTileLine(projectile.Center, projectile.Center + unit * Distance, (projectile.width + 16) * projectile.scale, DelegateMethods.CutTiles);
        }
    }
}
