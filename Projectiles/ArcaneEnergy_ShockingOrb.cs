using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace TerraLeague.Projectiles
{
    public class ArcaneEnergy_ShockingOrb : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shocking Orb");
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.alpha = 255;
            Projectile.timeLeft = 90;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.aiStyle = 0;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            if (Projectile.timeLeft == 2)
            {
                Prime();
            }
            else if (Projectile.timeLeft > 2)
            {
                Projectile.localAI[0]  = 1 - Projectile.timeLeft / 90f;

                for (int i = 0; i < 3; i++)
                {
                    Vector2 dustBoxPosition = new Vector2(Projectile.position.X + 0, Projectile.position.Y + 0);
                    int dustBoxWidth = Projectile.width - 8;
                    int dustBoxHeight = Projectile.height - 8;
                    Dust dust = Dust.NewDustDirect(dustBoxPosition, dustBoxWidth, dustBoxHeight, 113, 0f, 0f, 100, default, 1.5f + (1.5f * Projectile.localAI[0] ));
                    dust.noGravity = true;
                    dust.velocity *= 0.1f;
                    dust.velocity += Projectile.velocity * 1.4f;
                    dust.position.X -= Projectile.velocity.X / 3f * (float)i;
                    dust.position.Y -= Projectile.velocity.Y / 3f * (float)i;
                }

            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Prime();
            target.AddBuff(BuffType<Stunned>(), 60 + (int)(180 * Projectile.localAI[0] ));
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            Terraria.Audio.SoundEngine.PlaySound(new Terraria.Audio.LegacySoundStyle(3, 53), Projectile.position);

            TerraLeague.DustBorderRing(Projectile.width / 2, Projectile.Center, 113, default, 2f, true, true, 1);

            Dust dust;
            for (int i = 0; i < 80 * Projectile.localAI[0] ; i++)
            {
                Vector2 dustBoxPosition = new Vector2(Projectile.position.X + (Projectile.width/6), Projectile.position.Y + (Projectile.height / 6));
                int dustBoxWidth = Projectile.width - (Projectile.width / 3);
                int dustBoxHeight = Projectile.height - (Projectile.height / 3);

                dust = Dust.NewDustDirect(dustBoxPosition, dustBoxWidth, dustBoxHeight, 113, 0, 0, 0, default, 2f);
                dust.noGravity = true;
                dust.velocity = TerraLeague.CalcVelocityToPoint(Projectile.Center, dust.position,  10 * Vector2.Distance(dust.position, Projectile.Center)/(Projectile.width / 2));

                dust = Dust.NewDustDirect(dustBoxPosition, dustBoxWidth, dustBoxHeight, 113, 0, 0, 0, default, 3f);
                dust.velocity *= 1f;
                dust.noGravity = true;
                dust.color = new Color(0, 220, 220);
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
            Projectile.width = 50 + (int)(250 * Projectile.localAI[0] );
            Projectile.height = 50 + (int)(250 * Projectile.localAI[0] );
            
            Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
            Projectile.timeLeft = 2;
        }
    }
}