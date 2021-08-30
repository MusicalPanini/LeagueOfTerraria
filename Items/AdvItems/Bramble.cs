using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.AdvItems
{
    public class Bramble : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bramble Vest");
            Tooltip.SetDefault("Increases armor by 3");
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
                new Thorns(this)
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().armor += 3;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<ClothArmor>(), 2)
            .AddIngredient(ItemType<DarksteelBar>(), 6)
            .AddIngredient(ItemID.Spike, 10)
            .AddTile(TileID.Anvils)
            .Register();
            
        }
    }
}
