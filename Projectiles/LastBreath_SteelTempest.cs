using System;
using TerraLeague.Buffs;
using TerraLeague.Projectiles.Spear;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class LastBreath_SteelTempest : SpearProjectile
    {
        protected override float HoldoutRangeMin => 52;
        protected override float HoldoutRangeMax => 100;

        bool enemyHit = false;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("SteelTempest");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.scale = 1;
        }


        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.player[Projectile.owner];

            if (!enemyHit && Projectile.ai[0] == 0)
            {
                if (!player.GetModPlayer<PLAYERGLOBAL>().gathering2 && !player.GetModPlayer<PLAYERGLOBAL>().gathering3)
                {
                    player.AddBuff(BuffType<LastBreath2>(), 360);
                }
                else
                {
                    player.AddBuff(BuffType<LastBreath3>(), 360);
                    player.ClearBuff(BuffType<LastBreath2>());
                }
            }

            enemyHit = true;

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool PreAI()
        {
            base.PreAI();
            Player player = Main.player[Projectile.owner];
            player.ChangeDir(Projectile.Center.X < player.MountedCenter.X ? -1 : 1);

            return false;
        }
    }
}
