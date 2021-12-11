using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using TerraLeague.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace TerraLeague.Projectiles
{
    public class DarkIceTome_FrostRing : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ring of Frost");
        }

        public override void SetDefaults()
        {
            Projectile.width = 700;
            Projectile.height = 700;
            Projectile.timeLeft = 2;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.alpha = 255;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
            Projectile.tileCollide = false;
            Projectile.velocity = Vector2.Zero;
        }

        public override void AI()
        {
            base.AI();
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            target.AddBuff(BuffType<Slowed>(), 300);
            target.GetGlobalNPC<NPCs.TerraLeagueNPCsGLOBAL>().slowed = true;
            target.GetGlobalNPC<NPCs.TerraLeagueNPCsGLOBAL>().icebornSubjugation = true;
            target.GetGlobalNPC<NPCs.TerraLeagueNPCsGLOBAL>().icebornSubjugationOwner = Projectile.owner;
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool PreKill(int timeLeft)
        {
            return base.PreKill(timeLeft);
        }

        public override void Kill(int timeLeft)
        {
            TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 82, -0.7f);

            for (int i = 0; i < 50; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Ice, 0, 0, 50, default, 1.5f);
                dust.velocity *= 2f;
                dust.noGravity = true;
                dust.noLight = true;
                dust.fadeIn = 2;
            }
            for (int i = 0; i < 50; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Ice, 0, 0, 50, default, 1);
                dust.velocity *= 2f;
                dust.noGravity = true;
            }

            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < 18; i++)
                {
                    Vector2 pos = new Vector2(350, 0).RotatedBy(MathHelper.ToRadians((20 * i) + (j * 6))) + Projectile.Center;

                    Dust dustR = Dust.NewDustPerfect(pos, 113, Vector2.Zero, 0, default, 1);
                    dustR.noGravity = true;
                    dustR.fadeIn = 1.5f;
                }
            }

            base.Kill(timeLeft);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return Targeting.IsHitboxWithinRange(Projectile.Center, targetHitbox, Projectile.width / 2);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}
