using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.AdvItems
{
    public class Bamis : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bami's Cinder");
            Tooltip.SetDefault("Increases maximum life by 20");
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
                new Immolate(300, true)
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statLifeMax2 += 20;
            if (!hideVisual)
                player.GetModPlayer<PLAYERGLOBAL>().immolate = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<RubyCrystal>(), 1)
            .AddIngredient(ItemID.Hellstone, 10)
            .AddIngredient(ItemType<Sunstone>(), 5)
            .AddIngredient(ItemID.Fireblossom, 3)
            .AddTile(TileID.Furnaces)
            .Register();
            
        }
    }
}
