using Microsoft.Xna.Framework;
using System;
using TerraLeague.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace TerraLeague.Projectiles
{
    public class Item_LightningBolt : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            
            DisplayName.SetDefault("Lightning Bolt");
        }

        public override void SetDefaults()
        {
            Projectile.width = 6;
            Projectile.height = 6;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.alpha = 255;
            Projectile.scale = 1.2f;
            Projectile.timeLeft = 301;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            Projectile.Center = Main.npc[(int)Projectile.ai[0]].Center;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffType<Slowed>(), 180);

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            TerraLeague.DustLine(Projectile.Center, Main.player[Projectile.owner].MountedCenter, DustID.AncientLight, 1, 1, new Color(0, 255, 255, 150), false);

            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.AncientLight, 0, 0, 0, new Color(0, 255, 255, 150), 1.5f);
                dust.velocity *= 3;
                dust.noGravity = true;
            }
            Terraria.Audio.SoundEngine.PlaySound(new LegacySoundStyle(3, 53), Projectile.position);

            base.Kill(timeLeft);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            crit = false;
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override bool? CanHitNPC(NPC target)
        {
            if ((int)Projectile.ai[0] == target.whoAmI)
                return true;
            else
                return false;
        }
    }
}
