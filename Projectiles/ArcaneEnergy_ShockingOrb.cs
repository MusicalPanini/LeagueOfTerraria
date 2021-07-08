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
            projectile.width = 16;
            projectile.height = 16;
            projectile.alpha = 255;
            projectile.timeLeft = 90;
            projectile.penetrate = -1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.magic = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.aiStyle = 0;
            projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            if (projectile.timeLeft == 2)
            {
                Prime();
            }
            else if (projectile.timeLeft > 2)
            {
                projectile.localAI[0] = 1 - projectile.timeLeft / 90f;

                for (int i = 0; i < 3; i++)
                {
                    Vector2 dustBoxPosition = new Vector2(projectile.position.X + 0, projectile.position.Y + 0);
                    int dustBoxWidth = projectile.width - 8;
                    int dustBoxHeight = projectile.height - 8;
                    Dust dust = Dust.NewDustDirect(dustBoxPosition, dustBoxWidth, dustBoxHeight, DustID.Clentaminator_Blue, 0f, 0f, 100, default, 1.5f + (1.5f * projectile.localAI[0]));
                    dust.noGravity = true;
                    dust.velocity *= 0.1f;
                    dust.velocity += projectile.velocity * 1.4f;
                    dust.position.X -= projectile.velocity.X / 3f * (float)i;
                    dust.position.Y -= projectile.velocity.Y / 3f * (float)i;
                }

            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Prime();
            target.AddBuff(BuffType<Stunned>(), 60 + (int)(180 * projectile.localAI[0]));
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(new Terraria.Audio.LegacySoundStyle(3, 53), projectile.position);

            TerraLeague.DustBorderRing(projectile.width / 2, projectile.Center, DustID.Clentaminator_Blue, default, 2f, true, true, 1);

            Dust dust;
            for (int i = 0; i < 80 * projectile.localAI[0]; i++)
            {
                Vector2 dustBoxPosition = new Vector2(projectile.position.X + (projectile.width/6), projectile.position.Y + (projectile.height / 6));
                int dustBoxWidth = projectile.width - (projectile.width / 3);
                int dustBoxHeight = projectile.height - (projectile.height / 3);

                dust = Dust.NewDustDirect(dustBoxPosition, dustBoxWidth, dustBoxHeight, DustID.Clentaminator_Blue, 0, 0, 0, default, 2f);
                dust.noGravity = true;
                dust.velocity = TerraLeague.CalcVelocityToPoint(projectile.Center, dust.position,  10 * Vector2.Distance(dust.position, projectile.Center)/(projectile.width / 2));

                dust = Dust.NewDustDirect(dustBoxPosition, dustBoxWidth, dustBoxHeight, DustID.Clentaminator_Blue, 0, 0, 0, default, 3f);
                dust.velocity *= 1f;
                dust.noGravity = true;
                dust.color = new Color(0, 220, 220);
            }

            projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
            projectile.width = 10;
            projectile.height = 10;
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Prime();
            return false;
        }

        public void Prime()
        {
            projectile.friendly = true;

            projectile.velocity = Vector2.Zero;
            projectile.tileCollide = false;
            projectile.alpha = 255;
            projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
            projectile.width = 50 + (int)(250 * projectile.localAI[0]);
            projectile.height = 50 + (int)(250 * projectile.localAI[0]);
            
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
            projectile.timeLeft = 2;
        }
    }
}