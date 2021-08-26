using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using TerraLeague.NPCs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    class ArcaneEnergy_Artillery : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Right of the Arcane");
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.alpha = 255;
            Projectile.timeLeft = 600;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            if (Projectile.ai[1] == 0f && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
            {
                Projectile.ai[1] = 1f;
                Projectile.netUpdate = true;
            }
            if (Projectile.ai[1] != 0f)
            {
                Projectile.tileCollide = true;
            }

            if (Projectile.soundDelay == 0)
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item8, Projectile.position);
            Projectile.soundDelay = 10;

            //if (Projectile.timeLeft < 590)
            //{
            //    Projectile.friendly = true;
            //}

            if (Projectile.velocity.X > 12)
                Projectile.velocity.X = 12;
            else if (Projectile.velocity.X < -12)
                Projectile.velocity.X = -12;

            //for (int i = 0; i < 2; i++)
            //{
            //    Dust dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 113, 0, 0, 124, default(Color), 2.5f);
            //    dust2.noGravity = true;
            //    dust2.noLight = true;
            //    dust2.velocity *= 0.6f;
            //}

            for (int i = 0; i < 3; i++)
            {
                Vector2 dustBoxPosition = new Vector2(Projectile.position.X + 12, Projectile.position.Y + 12);
                int dustBoxWidth = Projectile.width - 24;
                int dustBoxHeight = Projectile.height - 24;
                Dust dust = Dust.NewDustDirect(dustBoxPosition, dustBoxWidth, dustBoxHeight, 113, 0f, 0f, 124, default, 2.5f);
                dust.noGravity = true;
                dust.velocity *= 0.1f;
                dust.velocity += Projectile.velocity * 0.1f;
                dust.position.X -= Projectile.velocity.X / 3f * (float)i;
                dust.position.Y -= Projectile.velocity.Y / 3f * (float)i;
            }

            Projectile.velocity.Y += 0.4f;


            base.AI();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Prime();
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            Terraria.Audio.SoundEngine.PlaySound(new LegacySoundStyle(2, 89), Projectile.position);

            TerraLeague.DustBorderRing(Projectile.width / 2, Projectile.Center, 113, default, 3f);

            Dust dust;
            for (int i = 0; i < 50; i++)
            {
                dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 113, 0, 0, 0, default, 2f);
                dust.noGravity = true;
                dust.velocity *= 2f;

                //dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 113, 0, -4, 0, default(Color), 3f);
                //dust.velocity *= 1f;
                //dust.noGravity = true;
                //dust.color = new Color(0, 220, 220);
            }

            Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y + (float)(Projectile.height / 2);
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Prime();
            return false;
        }

        public void Prime()
        {
            Projectile.friendly = true;

            Projectile.velocity = Vector2.Zero;
            Projectile.tileCollide = false;
            Projectile.alpha = 255;
            Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y + (float)(Projectile.height / 2);
            Projectile.width = 200;
            Projectile.height = 200;
            Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
            Projectile.timeLeft = 1;
        }
    }
}
