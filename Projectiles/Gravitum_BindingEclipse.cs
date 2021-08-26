using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TerraLeague.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class Gravitum_BindingEclipse : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Binding Eclipse");
        }

        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 64;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.alpha = 255;
            Projectile.scale = 1.2f;
            Projectile.timeLeft = 300;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            Projectile.Center = Main.npc[(int)Projectile.ai[0]].Center;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Stunned>(), 60 * 6);
            target.DelBuff(target.FindBuffIndex(ModContent.BuffType<GravitumMark>()));
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            Terraria.Audio.SoundEngine.PlaySound(new LegacySoundStyle(2, 74), Projectile.Center);
            Terraria.Audio.SoundEngine.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 111), Projectile.Center);

            Dust dust;
            for (int i = 0; i < 20; i++)
            {
                dust = Dust.NewDustDirect(new Vector2(Projectile.position.X + Projectile.width / 4, Projectile.position.Y + Projectile.width / 4), Projectile.width / 2, Projectile.height / 2, 54, 0f, 0f, 100, new Color(0, 0, 0), 2f);
                dust.noGravity = true;
                dust.velocity = (dust.position - Projectile.Center) * -0.1f;

                dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 71, 0f, 0f, 100, default, 1f);
                dust.noGravity = true;
                dust.velocity *= 3f;
                dust.velocity = (dust.position - Projectile.Center) * -0.01f;

                dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 71, 0f, 0f, 100, default, 1f);
                dust.noGravity = true;
                dust.velocity = (dust.position - Projectile.Center) * -0.05f;
            }

            base.Kill(timeLeft);
        }

        public override bool? CanHitNPC(NPC target)
        {
            if ((int)Projectile.ai[0] == target.whoAmI)
                return true;
            else
                return false;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}
