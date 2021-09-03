using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.AdvItems
{
    public class Chalice : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chalice of Harmony");
            Tooltip.SetDefault("Increases resist by 4" +
                "\nIncreases mana regeneration by 30%");
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
                new Harmony(1, 40, this)
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().resist += 4;
            player.GetModPlayer<PLAYERGLOBAL>().manaRegenModifer += 0.3;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<FaerieCharm>(), 2)
            .AddIngredient(ItemType<NullMagic>(), 1)
            .AddRecipeGroup("TerraLeague:GoldGroup", 8)
            .AddIngredient(ItemType<ManaBar>(), 4)
            .AddIngredient(ItemType<Petricite>(), 4)
            .AddTile(TileID.Anvils)
            .Register();
            
        }
    }
}
