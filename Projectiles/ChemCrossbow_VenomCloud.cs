using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using TerraLeague.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    class ChemCrossbow_VenomCloud : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Venom Cask");
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.alpha = 0;
            Projectile.timeLeft = 600;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = false;
            Projectile.scale = 1.5f;
            base.SetDefaults();
        }

        public override void AI()
        {
            Projectile.tileCollide = false;
            Projectile.ai[1] += 1f;
            if (Projectile.ai[1] > 60f)
            {
                Projectile.ai[0] += 10f;
            }
            if (Projectile.ai[0] > 255f)
            {
                Projectile.Kill();
                Projectile.ai[0] = 255f;
            }
            Projectile.alpha = (int)(100.0 + (double)Projectile.ai[0] * 0.7);
            Projectile.rotation += Projectile.velocity.X * 0.1f;
            Projectile.rotation += (float)Projectile.direction * 0.003f;
            Projectile.velocity *= 0.96f;
            Rectangle projectileHitBox = new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height);
            for (int i = 0; i < Main.projectile.Length; i++)
            {
                if (i != Projectile.whoAmI && Main.projectile[i].active && Main.projectile[i].type == Projectile.type)
                {
                    Rectangle targetHitBox = new Rectangle((int)Main.projectile[i].position.X, (int)Main.projectile[i].position.Y, Main.projectile[i].width, Main.projectile[i].height);
                    if (projectileHitBox.Intersects(targetHitBox))
                    {
                        Vector2 vector77 = Main.projectile[i].Center - Projectile.Center;
                        if (vector77.X == 0f && vector77.Y == 0f)
                        {
                            if (i < Projectile.whoAmI)
                            {
                                vector77.X = -1f;
                                vector77.Y = 1f;
                            }
                            else
                            {
                                vector77.X = 1f;
                                vector77.Y = -1f;
                            }
                        }
                        vector77.Normalize();
                        vector77 *= 0.005f;
                        Projectile.velocity -= vector77;
                        Projectile projectile2 = Main.projectile[i];
                        projectile2.velocity += vector77;
                    }
                }
            }

            AnimateProjectile();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Prime();
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            int stacks = target.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().DeadlyVenomStacks;

            if (stacks < 4)
            {
                target.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().DeadlyVenomStacks++;

                if (Main.netMode == NetmodeID.MultiplayerClient)
                    TerraLeagueNPCsGLOBAL.PacketHandler.SendSyncStats(Main.LocalPlayer.whoAmI, -1, 3, target.whoAmI, target.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().DeadlyVenomStacks);
            }

            target.AddBuff(BuffType<DeadlyVenom>(), 300);

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            crit = false;

            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            return false;
        }

        public void Prime()
        {
            Projectile.velocity = Vector2.Zero;
            Projectile.tileCollide = false;
            Projectile.alpha = 255;
            Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y + (float)(Projectile.height / 2);
            Projectile.width = 150;
            Projectile.height = 150;
            Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
            Projectile.timeLeft = 3;
        }

        public void AnimateProjectile() 
        {
            Projectile.friendly = false;
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 20)
            {
                Projectile.friendly = true;
                Projectile.frameCounter = 0;
            }
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}
