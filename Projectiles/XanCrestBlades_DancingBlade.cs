using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class XanCrestBlades_DancingBlade : ModProjectile
    {
        readonly List<int> iFrames = new List<int>();
        readonly List<Vector2> positions = new List<Vector2>();
        readonly List<float> angles = new List<float>();
        readonly List<int> damage = new List<int>();
        int baseDamage = 0;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            DisplayName.SetDefault("Dancing Blade");
        }

        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 62;
            Projectile.alpha = 0;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.minion = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.scale = 1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 3600;
        }

        public override void AI()
        {
            if (Projectile.timeLeft == 3600)
            {
                baseDamage = Projectile.originalDamage;
                Projectile.damage = 0;
            }

            if (/*Main.mouseLeftRelease */ !Main.player[Projectile.owner].channel && Projectile.timeLeft < 3600 && Projectile.owner == Main.LocalPlayer.whoAmI || Projectile.alpha != 0 || Main.player[Projectile.owner].dead || !Main.player[Projectile.owner].active)
            {
                Projectile.alpha += 20;
                if (Projectile.alpha > 250)
                {
                    DeadMode();
                }
            }

            Player player = Main.player[Projectile.owner];
            Projectile.netUpdate = true;
            //player.itemTime = 5;

            if (Projectile.ai[0] == 0 && Projectile.owner == Main.LocalPlayer.whoAmI)
            {
                float num114 = 14;

                Vector2 vector10 = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
                float num115 = (float)Main.mouseX + Main.screenPosition.X - vector10.X;
                float num116 = (float)Main.mouseY + Main.screenPosition.Y - vector10.Y;
                if (Main.player[Projectile.owner].gravDir == -1f)
                {
                    num116 = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY - vector10.Y;
                }
                float num117 = (float)Math.Sqrt((double)(num115 * num115 + num116 * num116));
                if (num117 > num114)
                {
                    num117 = num114 / num117;
                    num115 *= num117;
                    num116 *= num117;
                    int num118 = (int)(num115 * 1000f);
                    int num119 = (int)(Projectile.velocity.X * 1000f);
                    int num120 = (int)(num116 * 1000f);
                    int num121 = (int)(Projectile.velocity.Y * 1000f);
                    if (num118 != num119 || num120 != num121)
                    {
                        Projectile.netUpdate = true;
                    }


                    if (Vector2.Distance(Projectile.position, Main.MouseWorld) < 600)
                    {
                        Projectile.velocity.X = num115 * Vector2.Distance(Projectile.position, Main.MouseWorld) / 600;
                        Projectile.velocity.Y = num116 * Vector2.Distance(Projectile.position, Main.MouseWorld) / 600;
                    }
                    else
                    {
                        Projectile.velocity.X = num115;
                        Projectile.velocity.Y = num116;
                    }
                }
                else
                {
                    int num122 = (int)(num115 * 1000f);
                    int num123 = (int)(Projectile.velocity.X * 1000f);
                    int num124 = (int)(num116 * 1000f);
                    int num125 = (int)(Projectile.velocity.Y * 1000f);
                    if (num122 != num123 || num124 != num125)
                    {
                        Projectile.netUpdate = true;
                    }

                    if (Vector2.Distance(Projectile.position, Main.MouseWorld) < 600)
                    {
                        Projectile.velocity.X = num115 * Vector2.Distance(Projectile.position, Main.MouseWorld) / 600;
                        Projectile.velocity.Y = num116 * Vector2.Distance(Projectile.position, Main.MouseWorld) / 600;
                    }
                    else
                    {
                        Projectile.velocity.X = num115;
                        Projectile.velocity.Y = num116;
                    }
                }

                
            }

            if ((int)Projectile.ai[0] == 0)
            {
                Projectile.originalDamage = (int)((Projectile.velocity.Length() / 14) * baseDamage);
                Projectile.rotation = (Projectile.velocity.X / 8);
                Projectile.ai[1] = 12 - (int)Projectile.velocity.Length() / 2;
                positions.Add(Projectile.Center);
                angles.Add(Projectile.rotation);
                damage.Add(Projectile.originalDamage);
                iFrames.Add((int)Projectile.ai[1]);

                Projectile proj;
                int projOwned = player.ownedProjectileCounts[ProjectileType<XanCrestBlades_DancingBlade>()];

                for (int i = 1; i <= projOwned; i++)
                {
                    if (positions.Count >= (6 * i))
                    {
                        proj = Main.projectile.FirstOrDefault(x => (int)x.ai[0] == i && x.owner == Projectile.owner && Projectile.type == x.type);
                        if (proj != null)
                        {
                            proj.Center = positions[positions.Count - (6 * i)];
                            proj.rotation = angles[angles.Count - (6 * i)];
                            proj.originalDamage = damage[damage.Count - (6 * i)];
                            proj.ai[1] = iFrames[iFrames.Count - (6 * i)];
                        }
                    }


                    if (positions.Count >= 6 * projOwned)
                    {
                        positions.RemoveAt(0);
                        angles.RemoveAt(0);
                        damage.RemoveAt(0);
                        iFrames.RemoveAt(0);
                    }
                }
            }

            if (Projectile.rotation < 0)
                Projectile.spriteDirection = 1;
            else if (Projectile.rotation > 0)
                Projectile.spriteDirection = -1;

            Projectile.timeLeft = 3000;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {

            target.immune[Projectile.owner] = (int)Projectile.ai[1]; 
            
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            //if (Projectile.ai[0] == 1f)
            //{
            //    crit = true;
            //}

            //damage *= (int)((Projectile.velocity.Length() / 14));

            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override void Kill(int timeLeft)
        {
        }

        
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = height = 10;
            return true;
        }

        public void DeadMode()
        {
            Projectile.Kill();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, drawOrigin, 1, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            }
            return true;
        }
    }
}
