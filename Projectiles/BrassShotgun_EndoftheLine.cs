using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    class BrassShotgun_EndoftheLine : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("End of the Line");
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.alpha = 0;
            Projectile.timeLeft = 600;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            if ((int)Projectile.ai[1] == 0)
            {
                Projectile.rotation += Projectile.velocity.X * 0.05f;
                Lighting.AddLight(Projectile.position, 0.5f, 0.45f, 0.30f);
                Projectile.velocity.Y += 0.4f;

                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke);
                dust.alpha = 100;
                dust.velocity /= 3;
                dust.noGravity = true;
            }
            else
            {
                Player player = Main.player[Projectile.owner];

                if (Projectile.timeLeft == 1000)
                {
                    //Reset();
                }
                else if (Projectile.timeLeft == 1000 - 7)
                {
                    Prime(20);
                }

                if (Projectile.localAI[0]  == 0f)
                {
                    AdjustMagnitude(ref Projectile.velocity);
                    Projectile.localAI[0]  = 1f;
                }

                
                Vector2 move = player.MountedCenter - Projectile.Center;
                AdjustMagnitude(ref move);
                Projectile.velocity = (10 * Projectile.velocity + move) / 11.5f;
                AdjustMagnitude(ref Projectile.velocity);


                    if (Projectile.Hitbox.Intersects(player.Hitbox) || (int)Projectile.ai[0] > 10)
                    {
                        Projectile.Kill();
                    }

                for (int i = 0; i < 2; i++)
                {
                    Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6, 0, 0, 0, default, 3f);
                    dust.noGravity = true;
                    dust.noLight = true;

                    dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6, 0, 3, 0, default, 2f);
                    dust.noLight = true;

                }
            }

            base.AI();
        }
        
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if ((int)Projectile.ai[1] == 0)
            {
                Projectile.ai[1] = 1;
                Projectile.tileCollide = false;
                Projectile.timeLeft = 1000;
                //Projectile.damage *= 2;
                Projectile.alpha = 255;
                Projectile.width = 10;
                Projectile.height = 10;
                Projectile.velocity = Vector2.Zero;
                Projectile.extraUpdates = 1; ;
                Prime(200);
            }

            return false;
        }

       
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override void Kill(int timeLeft)
        {

        }

        public void Reset()
        {
            Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y + (float)(Projectile.height / 2);
        }

        public void Prime(int size)
        {
            Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y + (float)(Projectile.height / 2);
            Projectile.width = size;
            Projectile.height = size;
            Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
            Projectile.timeLeft = 1001;

            Terraria.Audio.SoundEngine.PlaySound(new LegacySoundStyle(2, 14), Projectile.position);

            Dust dust;
            for (int i = 0; i < (size == 200 ? 20f : 2f); i++)
            {
                dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default, size == 200 ? 2f : 1f);
                dust.velocity *= 0.5f;

                dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 100, default, size == 200 ? 4f : 2f);
                dust.noGravity = true;
                dust.velocity *= 3f;
                dust.color = new Color(255, 0, 220);
            }
            for (int i = 0; i < 50; i++)
            {
                dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 100, default, size == 200 ? 3f : 1.5f);
                dust.color = new Color(255, 0, 220);
                dust.noGravity = true;
            }

            Projectile.ai[0]++;
        }

        private void AdjustMagnitude(ref Vector2 vector)
        {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 15f)
            {
                vector *= 15f / magnitude;
            }
        }
    }
}
