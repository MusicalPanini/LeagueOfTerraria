using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class VoidSharpened : Passive
    {
        readonly int baseDamage;
        readonly int minionScaling;

        public VoidSharpened(int BaseDamage, int MinionScaling, LeagueItem item) : base(item)
        {
            baseDamage = BaseDamage;
            minionScaling = MinionScaling;
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            return TooltipName("Icathian Bite") + LeagueTooltip.CreateColorString(PassiveSecondaryColor, "Melee attacks deal ") + LeagueTooltip.TooltipValue(baseDamage, false, "", new System.Tuple<int, ScaleType>(minionScaling, ScaleType.Minions)) + LeagueTooltip.CreateColorString(PassiveSecondaryColor, " On Hit damage");
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            modPlayer.meleeOnHit += baseDamage + (int)(modPlayer.SUM * minionScaling / 100d);

            base.UpdateAccessory(player, modItem);
        }

        public override void NPCHit(Item item, NPC target, ref int damage, ref float knockback, ref bool crit, ref int OnHitDamage, Player player)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            //OnHitDamage += baseDamage + (int)(modPlayer.SUM * minionScaling / 100d);

            base.NPCHit(item, target, ref damage, ref knockback, ref crit, ref OnHitDamage, player);
        }

        public override void NPCHitWithProjectile(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection, ref int OnHitDamage, Player player)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            base.NPCHitWithProjectile(proj, target, ref damage, ref knockback, ref crit, ref hitDirection, ref OnHitDamage, player);
        }
    }
}
