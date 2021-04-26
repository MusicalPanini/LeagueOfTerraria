using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items.Accessories
{
    public class FlashofBrilliance : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flash of Brilliance");
            Tooltip.SetDefault("Periodically when you deal damage, cast 1 - 3 random magic spells that deal 50 magic damage.");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.rare = ItemRarityID.Orange;
            item.value = Item.buyPrice(0, 18, 0, 0);
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            modPlayer.flashofBrilliance = true;
        }
    }
}
