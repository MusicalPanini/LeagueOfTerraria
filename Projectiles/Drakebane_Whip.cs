using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class Drakebane_Whip : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Drakebane");
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.alpha = 0;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.extraUpdates = 3;
            Projectile.hide = true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (Projectile.ai[0] == 0f)
            {
                Projectile.localAI[1] = player.itemAnimationMax;
                Projectile.restrikeDelay = 10;
            }
            float speedIncrease = (float)player.HeldItem.useAnimation / Projectile.localAI[1];

            AI_075(12, (int)Projectile.localAI[1], true, 2, 39);
        }

        private Vector2 AI_075(float swingLength, int swingTime, bool ignoreTiles, int sndgroup, int sound)
        {
            Player player = Main.player[Projectile.owner];
            float halfPi = 1.57079637f;
            Vector2 vector = player.RotatedRelativePoint(player.MountedCenter, true);

            if (Projectile.localAI[0]  == 0f)
            {
                Projectile.localAI[0]  = Projectile.velocity.ToRotation();
            }
            float num33 = (float)((Projectile.localAI[0] .ToRotationVector2().X >= 0f) ? 1 : -1);
            if (Projectile.ai[1] <= 0f)
            {
                num33 *= -1f;
            }
            Vector2 vector25 = (num33 * (Projectile.ai[0] / swingTime * 6.28318548f - 1.57079637f)).ToRotationVector2();
            vector25.Y *= (float)Math.Sin((double)Projectile.ai[1]);
            if (Projectile.ai[1] <= 0f)
            {
                vector25.Y *= -1f;
            }
            vector25 = vector25.RotatedBy((double)Projectile.localAI[0] , default);
            Projectile.ai[0] += 1f / Projectile.MaxUpdates;
            if (Projectile.ai[0] < swingTime)
            {
                Projectile.velocity += swingLength * vector25;
            }
            else
            {
                Projectile.Kill();
            }

            Projectile.position = player.RotatedRelativePoint(player.MountedCenter, true) - Projectile.Size / 2f;
            Projectile.rotation = Projectile.velocity.ToRotation() + halfPi;
            Projectile.spriteDirection = Projectile.direction;
            player.ChangeDir(Projectile.direction);
            player.heldProj = Projectile.whoAmI;
            player.itemTime = Math.Max(player.itemTime, Projectile.restrikeDelay);
            player.itemAnimation = Math.Max(3, Projectile.restrikeDelay);
            player.itemRotation = (float)Math.Atan2((double)(Projectile.velocity.Y * (float)Projectile.direction), (double)(Projectile.velocity.X * (float)Projectile.direction));

            Vector2 vector34 = Main.OffsetsPlayerOnhand[player.bodyFrame.Y / 56] * 2f;
            if (player.direction != 1)
            {
                vector34.X = (float)player.bodyFrame.Width - vector34.X;
            }
            if (player.gravDir != 1f)
            {
                vector34.Y = (float)player.bodyFrame.Height - vector34.Y;
            }
            vector34 -= new Vector2((float)(player.bodyFrame.Width - player.width), (float)(player.bodyFrame.Height - 42)) / 2f;
            Projectile.Center = player.RotatedRelativePoint(player.position + vector34, true) - Projectile.velocity;

            Vector2 endPoint = Projectile.position + Projectile.velocity * 2f;

            if (Projectile.ai[0] > 1 && !ignoreTiles)
            {
                Vector2 prevPoint = Projectile.oldPosition + Projectile.oldVelocity * 2f;
                if (!Collision.CanHit(endPoint, Projectile.width, Projectile.height, prevPoint, Projectile.width, Projectile.height))
                {
                    if (Projectile.ai[0] * 2 < Projectile.localAI[1])
                    {
                        Projectile.restrikeDelay = player.itemAnimationMax - (int)Projectile.ai[0] * 2;
                        Projectile.ai[0] = Math.Max(1f, Projectile.localAI[1] - Projectile.ai[0] + 1);
                        Projectile.ai[1] *= -0.9f; 
                        Terraria.Audio.SoundEngine.PlaySound(sndgroup, endPoint, sound);
                        Collision.HitTiles(endPoint, endPoint - prevPoint, 8, 8);
                    }
                }
            }

            return endPoint;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float collisionPoint = 0f;
            if (Collision.CheckAABBvLineCollision(
                targetHitbox.TopLeft(), targetHitbox.Size(),
                Projectile.Center, Projectile.Center + Projectile.velocity,
                Projectile.width * Projectile.scale, ref collisionPoint))
            {
                return true;
            }
            return false;
        }

        public override bool? CanCutTiles()
        {
            DelegateMethods.tilecut_0 = Terraria.Enums.TileCuttingContext.AttackProjectile;
            Utils.PlotTileLine(Projectile.Center, Projectile.Center + Projectile.velocity, (float)Projectile.width * Projectile.scale, new Utils.TileActionAttempt(DelegateMethods.CutTiles));
            return true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            int handleHeight = 76;
            int chainHeight = 14;
            int partHeight = 14;
            int tipHeight = 54;
            int partCount = 8;


            Vector2 vector38 = Projectile.position + new Vector2((float)Projectile.width, (float)Projectile.height) / 2f + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition;
            Texture2D projectileTexture = TextureAssets.Projectile[Projectile.type].Value;
            Color alpha3 = Projectile.GetAlpha(Lighting.GetColor((int)Projectile.Center.X / 16, (int)Projectile.Center.Y / 16));

            alpha3 *= Projectile.Opacity;

            Rectangle handle = new Rectangle(0, 0, projectileTexture.Width, handleHeight);
            Rectangle chain = new Rectangle(0, handleHeight, projectileTexture.Width, chainHeight);
            Rectangle part = new Rectangle(0, handleHeight + chainHeight, projectileTexture.Width, partHeight);
            Rectangle tip = new Rectangle(0, handleHeight + chainHeight + partHeight, projectileTexture.Width, tipHeight);


            if (Projectile.velocity == Vector2.Zero)
            {
                return false;
            }
            SpriteEffects se = SpriteEffects.None;
            if (Projectile.spriteDirection < 0)
            {
                se = SpriteEffects.FlipHorizontally;
            }
            float chainCount = Projectile.velocity.Length() + 16f - tipHeight / 2;
            bool halfSize = chainCount < partHeight * 4.5f;
            Vector2 normalVel = Vector2.Normalize(Projectile.velocity);
            Rectangle currentRect = handle;
            Vector2 gfxOffY = new Vector2(0f, Main.player[Projectile.owner].gfxOffY);
            float rotation24 = Projectile.rotation + 3.14159274f;
            Main.spriteBatch.Draw(projectileTexture, Projectile.Center.Floor() - Main.screenPosition + gfxOffY, new Microsoft.Xna.Framework.Rectangle?(currentRect), alpha3, rotation24, currentRect.Size() / 2f - Vector2.UnitY * 4f, Projectile.scale, se, 0f);
            chainCount -= 40f * Projectile.scale;
            Vector2 centre = Projectile.Center.Floor();
            centre += normalVel * Projectile.scale * handle.Height / 2;
            Vector2 centreOffY;
            currentRect = chain;
            if (chainCount > 0f)
            {
                float i = 0f;
                while (i + 1f < chainCount)
                {
                    if (chainCount - i < (float)currentRect.Height)
                    {
                        currentRect.Height = (int)(chainCount - i);
                    }
                    centreOffY = centre + gfxOffY;
                    alpha3 = Projectile.GetAlpha(Lighting.GetColor((int)centreOffY.X / 16, (int)centreOffY.Y / 16));
                    Main.spriteBatch.Draw(projectileTexture, centreOffY - Main.screenPosition, new Microsoft.Xna.Framework.Rectangle?(currentRect), alpha3, rotation24, new Vector2((float)(currentRect.Width / 2), 0f), Projectile.scale, se, 0f);
                    i += (float)currentRect.Height * Projectile.scale;
                    centre += normalVel * (float)currentRect.Height * Projectile.scale;
                }
            }
            Vector2 centre2 = centre;
            centre = Projectile.Center.Floor();
            centre += normalVel * Projectile.scale * chain.Height / 2;
            currentRect = part;
            if (halfSize)
            {
                partCount /= 2;
            }
            float num200 = chainCount;
            if (chainCount > 0f)
            {
                float num201 = 0f;
                float num202 = num200 / (float)partCount;
                num201 += num202 * 0.25f;
                //centre += normalVel * num202 * 0.25f;
                centre += normalVel * Projectile.scale * handle.Height / 2;
                for (int i = 0; i < partCount; i++)
                {
                    float num204 = num202;
                    if (i == 0)
                    {
                        num204 *= 0.75f;
                    }
                    centreOffY = centre + gfxOffY;
                    alpha3 = Projectile.GetAlpha(Lighting.GetColor((int)centreOffY.X / 16, (int)centreOffY.Y / 16));
                    Main.spriteBatch.Draw(projectileTexture, centreOffY - Main.screenPosition, new Microsoft.Xna.Framework.Rectangle?(currentRect), alpha3, rotation24, new Vector2((float)(currentRect.Width / 2), 0f), Projectile.scale, se, 0f);
                    num201 += num204;
                    centre += normalVel * num204;
                }
            }
            currentRect = tip;
            Vector2 centreOffY2 = centre2 + gfxOffY;
            alpha3 = Projectile.GetAlpha(Lighting.GetColor((int)centreOffY2.X / 16, (int)centreOffY2.Y / 16));

            Main.spriteBatch.Draw(projectileTexture, centreOffY2 - Main.screenPosition, new Microsoft.Xna.Framework.Rectangle?(currentRect), alpha3, rotation24, projectileTexture.Frame(1, 1, 0, 0).Top(), Projectile.scale, se, 0f);

            return false;
        }

        public override void Kill(int timeLeft)
        {
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 10;
            return true;
        }
    }
}



