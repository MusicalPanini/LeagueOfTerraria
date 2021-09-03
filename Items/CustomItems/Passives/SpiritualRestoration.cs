using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class SpiritualRestoration : Passive
    {
        public SpiritualRestoration(LeagueItem item) : base(item)
        {
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            return TooltipName("Boundless vitality") + LeagueTooltip.CreateColorString(PassiveSecondaryColor, "Increases all incoming healing by 30%");
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            modPlayer.spiritualRestur = true;

            base.UpdateAccessory(player, modItem);
        }
    }
}
