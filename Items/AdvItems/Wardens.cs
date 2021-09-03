using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.AdvItems
{
    public class Wardens : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Warden's Mail");
            Tooltip.SetDefault("Armor increased by 4");
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

            Passives = new Passive[]
            {
                new ColdSteel(2, 250, this)
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().armor += 4;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<ClothArmor>(), 2)
            .AddIngredient(ItemType<TrueIceChunk>(), 6)
            .AddRecipeGroup("TerraLeague:IronGroup", 8)
            .AddIngredient(ItemType<SilversteelBar>(), 4)
            .AddIngredient(ItemID.Obsidian, 20)
            .AddTile(TileID.Anvils)
            .Register();
            
        }
    }
}
