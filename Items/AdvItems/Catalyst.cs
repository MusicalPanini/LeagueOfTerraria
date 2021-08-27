using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.AdvItems
{
    public class Catalyst : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Catalyst of Aeons");
            Tooltip.SetDefault("\nIncreases maximum life by 10" +
                "\nIncreases maximum mana by 20");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 15, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.accessory = true;
            Item.material = true;

            Passives = new Passive[]
            {
                new Eternity()
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statLifeMax2 += 10;
            player.statManaMax2 += 20;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<RubyCrystal>(), 1)
            .AddIngredient(ItemType<SapphireCrystal>(), 1)
            .AddIngredient(ItemID.MagicCuffs, 1)
            .AddIngredient(ItemType<VoidFragment>(), 50)
            .AddIngredient(ItemID.Amethyst, 5)
            .AddTile(TileID.Furnaces)
            .Register();
            
        }
    }
}
