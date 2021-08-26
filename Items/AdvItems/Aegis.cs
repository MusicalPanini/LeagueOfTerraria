using TerraLeague.Items.BasicItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.AdvItems
{
    public class Aegis : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Aegis of the Legion");
            Tooltip.SetDefault("Increases armor by 3" +
                "\nIncreases resist by 3" +
                "\nGrants immunity to fire blocks");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.accessory = true;
            Item.material = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().armor += 3;
            player.GetModPlayer<PLAYERGLOBAL>().resist += 3;
            player.fireWalk = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<NullMagic>(), 1)
            .AddIngredient(ItemType<ClothArmor>(), 1)
            .AddIngredient(ItemID.ObsidianSkull, 1)
            .AddIngredient(ItemID.SunplateBlock, 20)
            .AddTile(TileID.Anvils)
            .Register();
            
        }
    }
}
