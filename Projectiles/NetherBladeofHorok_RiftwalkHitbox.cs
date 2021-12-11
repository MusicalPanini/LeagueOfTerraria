using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    class NetherBladeofHorok_RiftwalkHitbox : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Riftwalk");
        }

        public override void SetDefaults()
        {
            Projectile.width = 256;
            Projectile.height = 256;
            Projectile.timeLeft = 1;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.alpha = 255;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void AI()
        {
            Projectile.Center = Main.player[Projectile.owner].MountedCenter;
        }

        public override void Kill(int timeLeft)
        {
            TerraLeague.DustRing(112, Projectile, default);
            TerraLeague.DustBorderRing(Projectile.width/2, Projectile.Center, 112, default, 2);
            TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 82, -0.7f);

            base.Kill(timeLeft);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return Targeting.IsHitboxWithinRange(Projectile.Center, targetHitbox, Projectile.width / 2);
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}
