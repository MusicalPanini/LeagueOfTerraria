using TerraLeague.Items.BasicItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.AdvItems
{
    public class RecurveBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Recurve Bow");
            Tooltip.SetDefault("5 melee and ranged On Hit Damage" +
                "\n10% increased melee and ranged attack speed");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().meleeOnHit += 5;
            player.GetModPlayer<PLAYERGLOBAL>().rangedOnHit += 5;
            player.meleeSpeed += 0.10f;
            player.GetModPlayer<PLAYERGLOBAL>().rangedAttackSpeed *= 1.10;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<Dagger>(), 2)
            .AddIngredient(ItemID.PalmWoodBow, 1)
            .AddIngredient(ItemID.WhiteString, 1)
            .AddTile(TileID.Anvils)
            .Register();
            
        }
    }
}
