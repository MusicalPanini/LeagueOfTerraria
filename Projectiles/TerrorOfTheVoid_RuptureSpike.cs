using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
//using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class TerrorOfTheVoid_RuptureSpike : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rupture");
        }

        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 64;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.alpha = 255;
            Projectile.scale = 1f;
            Projectile.timeLeft = 180;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = true;
            //Projectile.scale = 4;
            //DrawOriginOffsetX = -128;
            //DrawOriginOffsetY = 192;
            Projectile.hide = true;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            Projectile.spriteDirection = (int)Projectile.ai[0];

            if (Projectile.timeLeft > 90)
            {
                if (Main.rand.Next(0, 3) == 0)
                {
                    Dust dustIndex = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y + Projectile.height), Projectile.width, 32, DustID.PurpleMoss, 0f, 0f, 0, default, 1.5f);
                }
            }

            if (Projectile.timeLeft == 90)
            {
                TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 89, -1f);

                Projectile.position.Y += Projectile.height;
                Projectile.velocity.Y = -Projectile.height/8;
                Projectile.alpha = 0;
                Projectile.extraUpdates = 0;
                Projectile.friendly = true;
                Projectile.tileCollide = false;
            }
            if (Projectile.timeLeft == 84)
            {
                Projectile.velocity *= 0;
                Projectile.extraUpdates = 0;
                for (int i = 0; i < 3; i++)
                {
                    Dust dustIndex = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.PurpleMoss, 0f, -4f, Projectile.alpha, default, 2);
                }

                if ((int)Projectile.ai[1] != 0)
                {
                    int numberProjectiles = Main.rand.Next(0, 4);
                    for (int i = 0; i < numberProjectiles; i++)
                    {
                        Vector2 perturbedSpeed = new Vector2(0, -16).RotatedByRandom(MathHelper.ToRadians(16));
                        Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, ModContent.ProjectileType<TerrorOfTheVoid_VorpalSpike>(), Projectile.damage, 1, Projectile.owner);
                    }
                }
            }
            else if (Projectile.timeLeft < 52)
            {
                Projectile.alpha += 15;

                if (Projectile.alpha <= 0)
                    Projectile.Kill();
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            //target.velocity = new Vector2(0, -16);

            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindNPCsAndTiles.Add(index);

            base.DrawBehind(index, behindNPCsAndTiles, behindNPCs, behindProjectiles, overPlayers, overWiresUI);
        }

    }
}
