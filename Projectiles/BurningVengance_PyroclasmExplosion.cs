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
    public class BurningVengance_PyroclasmExplosion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pyroclasm");
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.timeLeft = 300;
            Projectile.penetrate = 100;
            Projectile.friendly = true;
            Projectile.alpha = 255;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            if (Projectile.timeLeft == 300)
                Prime();
            base.AI();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (target.HasBuff(BuffID.OnFire))
            {
                target.AddBuff(BuffType<Ablaze>(), 600);
                target.DelBuff(target.FindBuffIndex(BuffID.OnFire));
            }
            else if (target.HasBuff(BuffType<Ablaze>()))
            {
                target.AddBuff(BuffType<Ablaze>(), 600);
            }
            else
            {
                target.AddBuff(BuffID.OnFire, 1200);
            }

            base.OnHitNPC(target, damage, knockback, crit);
        }


        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            hitDirection = Projectile.Center.X > target.Center.X ? -1 : 1;

            if (target.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().ablaze)
            {
                damage *= 2;
                Projectile.NewProjectileDirect(Projectile.GetProjectileSource_FromThis(), target.Center, Vector2.Zero, ProjectileType<BurningVengance_PyroclasmExplosion>(), Projectile.damage / 2, 5, Projectile.owner);
            }

            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override void Kill(int timeLeft)
        {
            Terraria.Audio.SoundEngine.PlaySound(new LegacySoundStyle(2, 14), Projectile.position);

            Dust dust;
            for (int i = 0; i < 20; i++)
            {
                dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default, 1f);
                dust.velocity *= 0.5f;
            }
            for (int i = 0; i < 100; i++)
            {
                dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 100, default, 3f);
                dust.noGravity = true;
                dust.velocity *= 3f;
                dust.color = new Color(255, 0, 220);

                dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 100, default, 2f);
                dust.color = new Color(255, 0, 220);
                dust.noGravity = true;
            }
            base.Kill(timeLeft);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public void Prime()
        {
            int size = 256;

            Projectile.tileCollide = false;
            Projectile.velocity = Vector2.Zero;
            Projectile.alpha = 255;
            Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y + (float)(Projectile.height / 2);
            Projectile.width = size;
            Projectile.height = size;
            Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
            Projectile.timeLeft = 1;
        }
    }
}
