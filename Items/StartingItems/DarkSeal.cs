using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.StartingItems
{
    public class DarkSeal : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Dark Seal");
            Tooltip.SetDefault("Increases maximum mana by 20");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = 50000;
            Item.rare = ItemRarityID.Orange;
            Item.accessory = true;
            Item.material = true;

            Passives = new Passive[]
            {
                new Dread(10, 4, 1)
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statManaMax2 += 20;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.BandofStarpower, 1)
            .AddIngredient(ItemType<DarksteelBar>(), 5)
            .AddIngredient(ItemID.MeteoriteBar, 1)
            .AddIngredient(ItemID.FallenStar, 3)
            .AddTile(TileID.Anvils)
            .Register();
        }

        public override string GetStatText()
        {
            if (Passives[0].currentlyActive)
                return ((int)Passives[0].passiveStat).ToString();
            else
                return "";
        }
    }
}
