using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace TerraLeague.Projectiles
{
    public class DarkIceTome_IceShard : ModProjectile
    {
        bool split = false;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ice Shard");
        }

        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.alpha = 255;
            Projectile.scale = 1.2f;
            Projectile.timeLeft = 50;
            Projectile.DamageType = DamageClass.Magic;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Left, 0.09f, 0.40f, 0.60f);


            if (Projectile.alpha > 50)
            {
                Projectile.alpha -= 15;
            }
            if (Projectile.alpha < 50)
            {
                Projectile.alpha = 50;
            }

            for (int i = 0; i < 1; i++)
            {
                Dust dustIndex = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Ice, 0f, 0f, 100, default, 1.5f);
                dustIndex.noGravity = true;
                dustIndex.velocity *= 0.3f;
            }

            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;

            if (Projectile.velocity.Y > 16f)
            {
                Projectile.velocity.Y = 16f;
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Split(target.whoAmI);
            target.AddBuff(BuffType<Buffs.Slowed>(), 240);
            target.GetGlobalNPC<NPCs.TerraLeagueNPCsGLOBAL>().slowed = true;
            target.GetGlobalNPC<NPCs.TerraLeagueNPCsGLOBAL>().icebornSubjugation = true;
            target.GetGlobalNPC<NPCs.TerraLeagueNPCsGLOBAL>().icebornSubjugationOwner = Projectile.owner;
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = height = 10;

            return true;
        }

        public override void Kill(int timeLeft)
        {
            Terraria.Audio.SoundEngine.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 27), Projectile.position);
            for (int i = 0; i < 10; i++)
            {
                Dust dustIndex = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Ice, Projectile.velocity.X / 1.5f, Projectile.velocity.Y / 1.5f, 100, default, 1.5f);
                dustIndex.noGravity = true;
            }
            base.Kill(timeLeft);
        }

        public void Split(int num = -1)
        {
            if (!split && num != -1)
            {
                for (int j = Main.rand.Next(0, 2); j < 2; j++)
                {
                    Projectile proj3 = Projectile.NewProjectileDirect(Projectile.GetProjectileSource_FromThis(), Projectile.Center, Projectile.velocity.RotatedBy(MathHelper.ToRadians(Main.rand.Next(10, 21))), ProjectileType<DarkIceTome_IceShardSmallA>(), Projectile.damage, 0, Projectile.owner, num == -1 ? 255 : num);
                }
                for (int j = Main.rand.Next(0, 2); j < 2; j++)
                {
                    Projectile proj2 = Projectile.NewProjectileDirect(Projectile.GetProjectileSource_FromThis(), Projectile.Center, Projectile.velocity.RotatedBy(MathHelper.ToRadians(Main.rand.Next(-5, 6))), ProjectileType<DarkIceTome_IceShardSmallB>(), Projectile.damage, 0, Projectile.owner, num == -1 ? 255 : num);
                }
                for (int j = Main.rand.Next(0, 2); j < 2; j++)
                {
                    Projectile proj = Projectile.NewProjectileDirect(Projectile.GetProjectileSource_FromThis(), Projectile.Center, Projectile.velocity.RotatedBy(MathHelper.ToRadians(Main.rand.Next(-20, -11))), ProjectileType<DarkIceTome_IceShardSmallC>(), Projectile.damage, 0, Projectile.owner, num == -1 ? 255 : num);
                }

                split = true;
            }
        }
    }
}
