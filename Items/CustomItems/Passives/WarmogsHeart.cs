using TerraLeague.Items.CompleteItems;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class WarmogsHeart : Passive
    {
        public WarmogsHeart(LeagueItem item) : base(item)
        {
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            return TooltipName("WARMOG'S HEART") + LeagueTooltip.CreateColorString(PassiveSecondaryColor, "If you have over 600 maximum life and are standing still, gain a huge life regen boost");
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            modPlayer.warmogsHeart = true;
            base.UpdateAccessory(player, modItem);
        }
    }
}
