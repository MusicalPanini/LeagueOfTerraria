using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using TerraLeague.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class TrueIceFlail_GlacialPrison : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("GlacialPrison");
        }

        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 64;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.alpha = 255;
            Projectile.scale = 0.75f;
            Projectile.timeLeft = 45;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.aiStyle = 0;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 25;
            }
            if (Projectile.alpha < 0)
            {
                Projectile.alpha = 0;
            }
            for (int i = 0; i < 1; i++)
            {
                Dust dustIndex = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Ice, 0f, 0f, 100, default, 1.5f);
                dustIndex.noGravity = true;
                dustIndex.velocity *= 0.3f;
            }
            Projectile.rotation += 0.4f * (float)Projectile.direction;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            return true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.netUpdate = true;
            target.buffImmune[BuffType<Frozen>()] = false;
            TerraLeague.RemoveBuffFromNPC(BuffType<FrozenCooldown>(), target.whoAmI);
            target.AddBuff(BuffType<Frozen>(), 120);
            Projectile.Center = target.Center;
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            if (Main.netMode != NetmodeID.Server)
            {
                Terraria.Audio.SoundEngine.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 27), Projectile.position);
                TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 14, 1f);
            }
            if (Main.LocalPlayer.whoAmI == Projectile.owner)
            {
                Projectile proj = Projectile.NewProjectileDirect(Projectile.GetProjectileSource_FromThis(), Projectile.Center, Vector2.Zero, ProjectileType<TrueIceFlail_GlacialStorm>(), Projectile.damage, 0, Projectile.owner);
                //proj.Center = Projectile.Center;
            }
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = height = 10;

            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
    }
}
