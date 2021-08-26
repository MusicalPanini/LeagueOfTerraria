using Microsoft.Xna.Framework;
using TerraLeague.Dusts;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace TerraLeague.Projectiles
{
    class Whisper_DancingGrenade : ModProjectile
    {
        int bounces = 4;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dancing Grenade");
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 14;
            Projectile.alpha = 0;
            Projectile.timeLeft = 600;
            Projectile.penetrate = 4;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
            Projectile.aiStyle = 0;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
            Projectile.netImportant = true;
        }

        public override void AI()
        {
            if (Projectile.soundDelay == 0)
                Projectile.rotation = Main.rand.NextFloat(0, 6.282f);
            Projectile.soundDelay = 100;

            Lighting.AddLight(Projectile.position, 1f * Projectile.ai[0], 0.5f * Projectile.ai[0], 0.9f * Projectile.ai[0]);
            Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustType<Smoke>(),0,0,(int)(255 - (255 * Projectile.ai[0])), new Color(255,50,255));

            Projectile.rotation += Projectile.velocity.X * 0.05f;

            Projectile.velocity.Y += 0.4f;

            if(Projectile.velocity.X > 8)
                Projectile.velocity.X = 8;
            else if(Projectile.velocity.X < -8)
                Projectile.velocity.X = -8;

            base.AI();
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (target.statLife <= 0)
            {
                Projectile.ai[0] += 0.3f;
                Projectile.damage = (int)(Projectile.damage * 1.44f);
                Projectile.netUpdate = true;
            }
            base.OnHitPlayer(target, damage, crit);
        }
        
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            //if (target.life <= 0)
            {
                Projectile.ai[0] += 0.3f;
                Projectile.damage = (int)(Projectile.damage * 1.44f);
                Projectile.netUpdate = true;
            }
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Rebound();

            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 10);
            return false;
        }

        public void Rebound()
        {
            if (Projectile.velocity.X != Projectile.oldVelocity.X)
            {
                Projectile.velocity.X = -Projectile.oldVelocity.X;
            }
            else if (Projectile.velocity.Y != Projectile.oldVelocity.Y)
            {
                if (Projectile.oldVelocity.Y > 0)
                {
                    Projectile.velocity.Y = -8;
                    bounces--;
                }
                else
                {
                    Projectile.velocity.Y = 8;
                }
            }

            if (bounces == 0)
            {
                Projectile.Kill();
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    Dust dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustType<Smoke>(), 0f, 0f, 150, new Color(255, 50, 255));
                    dust.velocity *= 1f;
                }
            }
        }

        public override void Kill(int timeLeft)
        {
            Dust dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustType<Smoke>(), 0f, 0f, 100, new Color(255, 50, 255));
            dust.velocity *= 1f;
            base.Kill(timeLeft);
        }

    }
}
