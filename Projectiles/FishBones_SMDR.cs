using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    class FishBones_SMDR : ModProjectile
    {
        int baseDamage;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Super Mega Death Rocket");
        }

        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 26;
            Projectile.timeLeft = 1200;
            Projectile.penetrate = 1000;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.scale = 1.5f;
            AIType = 0;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            if (Projectile.soundDelay == 0)
                baseDamage = Projectile.damage;
            Projectile.soundDelay = 100;

            Lighting.AddLight(Projectile.Center, 1f, 0.34f, 0.9f);
            Projectile.damage = (int)(baseDamage * (Projectile.velocity.Length() / 25));
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Projectile.velocity.X < 0)
            {
                Projectile.rotation = Projectile.velocity.ToRotation();
                Projectile.scale = -1.5f;
                Projectile.spriteDirection = -1;
            }

            if (Projectile.velocity.Length() < 25)
            {
                Projectile.velocity.X *= 1.05f;
                Projectile.velocity.Y *= 1.05f;

            }

            for (int i = 0; i < 3; i++)
            {
                Dust dust1 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height/2, 6);
                Dust dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height/2, 6);
                dust1.scale = 2 * (Projectile.velocity.Length() / 25);
                dust1.noGravity = true;
                dust2.scale = 2 * (Projectile.velocity.Length() / 50);
                dust2.noGravity = true;
            }
            
            if (Projectile.timeLeft < 30)
            {
                Projectile.alpha += 9;
            }
        }

        public override void Kill(int timeLeft)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode.WithVolume(1f), Projectile.position);

            for (int i = 0; i < 80; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default, 2f);
                dust.velocity *= 3f;
                if (Main.rand.Next(2) == 0)
                {
                    dust.scale = 0.5f;
                    dust.fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                }
            }
            for (int i = 0; i < 120; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6, 0f, 0f, 100, default, 4.5f);
                dust.noGravity = true;
                dust.velocity *= 5f;

                dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6, 0f, 0f, 100, default, 3f);
                dust.velocity *= 3f;
            }
            for (int i = 0; i < 3; i++)
            {
                float velScale = (i+1) * 1f;

                Gore gore = Gore.NewGoreDirect(Projectile.Center, default, Main.rand.Next(61, 64), 2f);
                gore.velocity.X += 1.5f;
                gore.velocity.Y += 1.5f;

                gore = Gore.NewGoreDirect(Projectile.Center, default, Main.rand.Next(61, 64), 2f);
                gore.velocity.X -= 1.5f;
                gore.velocity.Y -= 1.5f;

                gore = Gore.NewGoreDirect(Projectile.Center, default, Main.rand.Next(61, 64), 2f);
                gore.velocity.X += 1.5f;
                gore.velocity.Y -= 1.5f;

                gore = Gore.NewGoreDirect(Projectile.Center, default, Main.rand.Next(61, 64), 2f);
                gore.velocity.X -= 1.5f;
                gore.velocity.Y += 1.5f;
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            hitDirection = Projectile.Center.X > target.Center.X ? -1 : 1;

            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (NPCID.Sets.ShouldBeCountedAsBoss[target.type] || Projectile.penetrate == 997)
                Prime();

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Prime();
            return false;
        }

        public void Prime()
        {
            Projectile.tileCollide = false;
            Projectile.velocity = Vector2.Zero;
            Projectile.alpha = 255;
            Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y + (float)(Projectile.height / 2);
            Projectile.width = 350;
            Projectile.height = 350;
            Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
            Projectile.timeLeft = 3;
        }
    }
}
