﻿using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class ArdentsFrenzy : Passive
    {
        public ArdentsFrenzy(LeagueItem item) : base(item)
        {
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            return TooltipName("FRENZY") + LeagueTooltip.CreateColorString(PassiveSecondaryColor, "Healing an ally will give you both 20% melee and " +
                "\nranged attack speed and deal 20 melee and ranged On Hit damage");
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            player.GetModPlayer<PLAYERGLOBAL>().ardentsFrenzy = true;

            base.UpdateAccessory(player, modItem);
        }
    }
}
