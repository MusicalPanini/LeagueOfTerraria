using Microsoft.Xna.Framework;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class SummonedBlade : Passive
    {
        readonly int minionScaling;

        public SummonedBlade(int MinionScaling)
        {
            minionScaling = MinionScaling;
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            return TooltipName("SUMMONED BLADE") + LeagueTooltip.CreateColorString(PassiveSecondaryColor, "SPELLBLADE summons ") + LeagueTooltip.TooltipValue(0, false, "", new System.Tuple<int, ScaleType>(200, ScaleType.Minions))
                + LeagueTooltip.CreateColorString(PassiveSecondaryColor," etheral blades around the struck enemy.\nThe blades deal ") + LeagueTooltip.TooltipValue(0, false, "", new System.Tuple<int, ScaleType>(minionScaling, ScaleType.Summon)) + LeagueTooltip.CreateColorString(PassiveSecondaryColor, " damage");
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
                    modPlayer.summonedBlade = true;
            base.UpdateAccessory(player, modItem);
        }

        public override void NPCHit(Item item, NPC target, ref int damage, ref float knockback, ref bool crit, ref int OnHitDamage, Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (modPlayer.spellBladeBuff)
            {
                if (modPlayer.summonedBlade)
                {
                    int totalSwords = player.maxMinions;
                    int baseDamage = (int)(modPlayer.SUM * minionScaling / 100d);
                    for (int i = 0; i < totalSwords; i++)
                    {
                        Projectile proj = Projectile.NewProjectileDirect(player.GetProjectileSource_Accessory(item), player.Center, Vector2.Zero, ProjectileType<Item_SummonedSwordA>(), baseDamage, 1, player.whoAmI, ((MathHelper.TwoPi * i) / totalSwords), target.whoAmI);
                        //proj.originalDamage = baseDamage;
                        proj = Projectile.NewProjectileDirect(player.GetProjectileSource_Accessory(item), player.Center, Vector2.Zero, ProjectileType<Item_SummonedSwordB>(), baseDamage, 1, player.whoAmI, ((MathHelper.TwoPi * i) / totalSwords), target.whoAmI);
                        //proj.originalDamage = baseDamage;
                    }
                    modPlayer.spellBladeBuff = false;
                }
            }
            base.NPCHit(item, target, ref damage, ref knockback, ref crit, ref OnHitDamage, player, modItem);
        }

        public override void PostPlayerUpdate(Player player, ModItem modItem)
        {
            base.PostPlayerUpdate(player, modItem);
        }
    }
}
