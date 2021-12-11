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
    public class HexCoreStaff_ChaosStorm : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chaos Storm");
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 24;
            Projectile.alpha = 0;
            Projectile.penetrate = -1;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;

            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;

            Projectile.scale = 1;
            Projectile.timeLeft = 3600;
        }

        public override void AI()
        {
            if (Projectile.soundDelay == 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Electric, 0, 0, 0, default, 2);
                    dust.noGravity = true;
                }
            }
            Projectile.soundDelay = 100;

            if (Main.rand.Next(3) == 0)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, 8, DustID.Electric, 0, 0, 0, default, 0.5f);
            }

            if (!Main.player[Projectile.owner].channel && Projectile.timeLeft < 3600 && Projectile.owner == Main.LocalPlayer.whoAmI || Projectile.alpha != 0 || Main.player[Projectile.owner].dead || !Main.player[Projectile.owner].active)
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
            if (Projectile.Distance(player.MountedCenter) > 1000)
                Projectile.Kill();

            Lighting.AddLight(Projectile.Center, 0, 1, 1);

            if (Main.LocalPlayer.whoAmI == player.whoAmI)
            {
                Projectile.localAI[0] ++;
                if ((int)Projectile.localAI[0]  == 60)
                {
                    //Projectile.NewProjectileDirect(Projectile.Center, Vector2.Zero, ProjectileType<HexCoreStaff_Storm>(), Projectile.damage, Projectile.knockBack, Projectile.owner);\
                    TerraLeague.DustBorderRing(224, Projectile.Center, 226, default, 0.75f);
                    List<int> npcs = Targeting.GetAllNPCsInRange(Projectile.Center, 224, true);
                    for (int i = 0; i < npcs.Count; i++)
                    {
                        NPC npc = Main.npc[npcs[i]];
                        if (!npc.dontTakeDamage)
                        {
                            Projectile.NewProjectileDirect(Projectile.GetProjectileSource_FromThis(), npc.Center, Vector2.Zero, ProjectileType<HexCoreStaff_ChaosStormZap>(), Projectile.damage, Projectile.knockBack, Projectile.owner, npc.whoAmI, Projectile.identity);
                            
                        }
                    }

                    if (npcs.Count > 0)
                        TerraLeague.PlaySoundWithPitch(Projectile.Center, 3, 53, 0);

                    Projectile.localAI[0]  = 0;
                }
            }

            

            if (Projectile.ai[0] == 0 && Projectile.owner == Main.LocalPlayer.whoAmI)
            {
                float num114 = 6;

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

                    // movement
                    Projectile.velocity.X = num115 * Vector2.Distance(Projectile.position, Main.MouseWorld) / 1000;
                    Projectile.velocity.Y = num116 * Vector2.Distance(Projectile.position, Main.MouseWorld) / 1000;

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

                    Projectile.velocity.X = num115 * Vector2.Distance(Projectile.position, Main.MouseWorld) / 1000;
                    Projectile.velocity.Y = num116 * Vector2.Distance(Projectile.position, Main.MouseWorld) / 1000;
                }
            }
            Projectile.timeLeft = 3000;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
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

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
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

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}
