using TerraLeague.Items.BasicItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.AdvItems
{
    public class NegatronCloak : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Negatron Cloak");
            Tooltip.SetDefault("Resist increased by 4");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
            Item.material = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().resist += 4;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<NullMagic>(), 1)
            .AddIngredient(ItemType<Petricite>(), 16)
            .AddIngredient(ItemID.Silk, 10)
            .AddIngredient(ItemID.TatteredCloth, 1)
            .AddTile(TileID.Loom)
            .Register();
            
        }
    }
}
