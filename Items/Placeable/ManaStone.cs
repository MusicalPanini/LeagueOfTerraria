using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Placeable
{
    class ManaStone : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mana Infused Earth");
            Tooltip.SetDefault("It's emitting wild magicks");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
        }

        public override void SetDefaults()
        {
            Item.value = Item.buyPrice(0, 0, 50, 0);
            Item.rare = ItemRarityID.Green;
            Item.width = 12;
            Item.height = 12;
            Item.maxStack = 999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = TileType<Tiles.ManaStone>();
        }
    }
}
