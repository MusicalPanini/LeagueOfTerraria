using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using TerraLeague.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    class ChemCrossbow_VenomCask : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Venom Cask");
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 26;
            Projectile.alpha = 0;
            Projectile.timeLeft = 600;
            Projectile.penetrate = 1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.aiStyle = 2;
            base.SetDefaults();
        }
        public override void AI()
        {
            Lighting.AddLight(Projectile.position, 0f, 0f, 0.5f);
            Dust dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Ice, 0f, 0f, 100, new Color(0, 255, 0));
            dust.noLight = true;
            dust.alpha = 0;
            dust.noLight = false;
            dust.noGravity = true;
            dust.scale = 1.4f;

            base.AI();
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

            Prime();
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            crit = false;

            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override void Kill(int timeLeft)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Shatter, Projectile.position);

            Dust dust;
            for (int i = 0; i < 50; i++)
            {
                dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, new Color(0, 255, 0), 1f);
                dust.velocity *= 1.4f;
                dust.noLight = true;
            }
            for (int i = 0; i < 80; i++)
            {
                dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 167, 0f, 0f, 125, new Color(0, 192, 255), 2f);
                dust.noGravity = true;
                dust.noLight = true;

                dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 167, 0f, 0f, 125, new Color(0, 192, 255), 1f);
                dust.noLight = true;
            }

            if (Projectile.owner == Main.myPlayer)
            {
                int spawnAmount = Main.rand.Next(12, 21);
                for (int i = 0; i < spawnAmount; i++)
                {
                    Vector2 vector14 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
                    vector14.Normalize();
                    vector14 *= (float)Main.rand.Next(10, 201) * 0.01f;
                    Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, vector14.X, vector14.Y, ProjectileType<ChemCrossbow_VenomCloud>(), 1, 1f, Projectile.owner, 0f, (float)Main.rand.Next(-45, 1));
                }
            }

            Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y + (float)(Projectile.height / 2);
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
            base.Kill(timeLeft);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 10; 
            return true;
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
    }
}
