using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.BasicItems
{
    public class LargeRod : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Needlessly Large Rod");
            Tooltip.SetDefault("4% increased magic and summon damage");
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
            player.GetDamage(DamageClass.Magic) += 0.04f;
            player.GetDamage(DamageClass.Summon) += 0.04f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddRecipeGroup("Wood", 10)
            .AddIngredient(ItemID.Bone, 5)
            .AddIngredient(ItemType<ManaBar>(), 2)
            .AddTile(TileID.WorkBenches)
            .Register();
            
        }
    }
}
