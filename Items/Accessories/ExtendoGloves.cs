using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items.Accessories
{
    public class ExtendoGloves : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Extendo Gloves");
            Tooltip.SetDefault("Increases block placement & tool range by 3" +
                "\n50% increased mining speed");
            base.SetStaticDefaults();
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 28;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.buyPrice(0, 18, 0, 0);
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            player.blockRange += 3;
            player.pickSpeed += 0.5f;
        }
    }
}
