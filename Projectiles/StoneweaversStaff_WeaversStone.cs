using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Achievements;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class StoneweaversStaff_WeaversStone : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Weaver's Stone");
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.timeLeft = 180;
            Projectile.penetrate = 1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
        }

        public override void AI()
        {
            if ((int)Projectile.ai[0] == 0)
            {
                if (Projectile.timeLeft < 150)
                {
                    Projectile.velocity.Y += 0.4f;
                    Projectile.velocity.X *= 0.97f;
                }
            }
            
            if ((int)Projectile.ai[0] == 1)
            {
                if (Projectile.timeLeft == 119)
                    Projectile.velocity = new Vector2(Projectile.velocity.X * -0.2f, -5);

                Projectile.velocity.Y += 0.4f;
                Projectile.velocity.X *= 0.97f;

                if (Projectile.timeLeft == 1)
                    Prime();
            }

            if (Projectile.velocity.Y > 16)
                Projectile.velocity.Y = 16;

            Projectile.rotation += (Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y)) * 0.03f * (float)Projectile.direction;

            if (Projectile.velocity.Length() > 2)
                Dust.NewDustDirect(Projectile.position, 16, 16, DustID.t_Slime, 0f, 0f, 100, new Color(255, 125, 0), 0.7f);
            
            base.AI();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if ((int)Projectile.ai[0] == 0)
            {
                Projectile.penetrate = 1000;
                Projectile.netUpdate = true;
                Projectile.ai[0] = 1;
                Projectile.friendly = false;
                Projectile.timeLeft = 120;
            }
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            if ((int)Projectile.ai[0] == 1)
            {
                Terraria.Audio.SoundEngine.PlaySound(new LegacySoundStyle(2, 14), Projectile.position);
                for (int g = 0; g < 4; g++)
                {
                    Gore.NewGore(new Vector2(Projectile.position.X + (float)(Projectile.width / 2) - 24f, Projectile.position.Y + (float)(Projectile.height / 2) - 24f), default, Main.rand.Next(61, 64), 1f);
                }

                Dust dust;
                for (int i = 0; i < 20; i++)
                {
                    dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.t_Slime, 0f, 0f, 100, new Color(255, 125, 0), 1f);
                    dust.velocity *= 2f;

                    dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.t_Slime, 0f, 0f, 100, new Color(255, 125, 0), 1.5f);
                    dust.velocity *= 1.5f;
                }
            }
            else if ((int)Projectile.ai[0] == 0)
            {
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);

                for (int i = 0; i < 12; i++)
                {
                    Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.t_Slime, Projectile.oldVelocity.X * 0.25f, Projectile.oldVelocity.Y * 0.25f, 0, new Color(255, 125, 0));
                }
            }
            base.Kill(timeLeft);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            if ((int)Projectile.ai[0] == 0)
                fallThrough = true;
            else
                fallThrough = false;

            width = height = 10;
            return true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if ((int)Projectile.ai[0] == 0)
                return true;
            return false;
        }

        public void Prime()
        {
            Projectile.damage = (int)(Projectile.damage * 1.5);
            Projectile.knockBack = 6;
            Projectile.friendly = true;
            Projectile.velocity = Vector2.Zero;
            Projectile.tileCollide = false;
            Projectile.alpha = 255;
            Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y + (float)(Projectile.height / 2);
            Projectile.width = 150;
            Projectile.height = 150;
            Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
        }
    }
}
