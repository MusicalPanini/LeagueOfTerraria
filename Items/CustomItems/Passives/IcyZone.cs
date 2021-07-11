using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class IcyZone : Passive
    {
        public override string Tooltip(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            return TooltipName("Snow Bind") + LeagueTooltip.CreateColorString(PassiveSecondaryColor, "Triggering SPELLBLADE will cause nearby enemies to take ") +
               LeagueTooltip.TooltipValue(0, false, "", new System.Tuple<int, ScaleType>(100, ScaleType.Armor)) +
                LeagueTooltip.CreateColorString(PassiveSecondaryColor, " damage and apply 'Slowed'");
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            modPlayer.icyZone = true;
            base.UpdateAccessory(player, modItem);
        }
    }
}
