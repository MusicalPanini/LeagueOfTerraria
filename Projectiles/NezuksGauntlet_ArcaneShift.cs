using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using TerraLeague.Items.Weapons.Abilities;
using TerraLeague.Projectiles.Homing;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    class NezuksGauntlet_ArcaneShift : HomingProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Arcane Shift");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.alpha = 255;
            Projectile.timeLeft = 90;
            Projectile.penetrate = 1;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
            Projectile.extraUpdates = 1;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;

            CanOnlyHitTarget = true;
            CanRetarget = true;
            TurningFactor = 0.93f;
            MaxVelocity = 16;
        }

        public override void AI()
        {
            HomingAI();

            for (int i = 0; i < 2; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.FireworksRGB, Projectile.velocity.X, Projectile.velocity.Y, 50, default, 0.5f);
                dust.noGravity = true;
                dust.velocity *= 0.3f;
            }

            Lighting.AddLight(Projectile.position, 0f, 0f, 0.5f);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 12; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.FireworksRGB, Projectile.velocity.X, Projectile.velocity.Y, 50, default, 1.2f);
                dust.noGravity = true;
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Player player = Main.player[Projectile.owner];
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            if (target.HasBuff(BuffType<Buffs.EssenceFluxDebuff>()))
            {
                modPlayer.magicFlatDamage += EssenceFlux.GetFluxDamage(modPlayer);

                TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 12, 0.5f);

                Projectile.DamageType = DamageClass.Magic;
                player.ManaEffect(100);
                player.statMana += 100;
            }

            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }
    }
    public class ArcaneShiftGlobalNPC : GlobalNPC
    {
        public override void OnHitByProjectile(NPC npc, Projectile projectile, int damage, float knockback, bool crit)
        {
            if (npc.HasBuff(BuffType<Buffs.EssenceFluxDebuff>()))
            {
                if (projectile.type == ProjectileType<NezuksGauntlet_ArcaneShift>())
                    npc.DelBuff(npc.FindBuffIndex(BuffType<Buffs.EssenceFluxDebuff>()));
            }
        }
    }
}
