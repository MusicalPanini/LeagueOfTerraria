using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items.BasicItems
{
    public class BlastingWand : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blasting Wand");
            Tooltip.SetDefault("3% increased magic and summon damage");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 3, 75, 0);
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
            Item.material = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Magic) += 0.03f;
            player.GetDamage(DamageClass.Summon) += 0.03f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.WandofSparking, 1)
            .AddIngredient(ItemID.Bomb, 1)
            .AddIngredient(ItemID.FallenStar, 1)
            .AddTile(TileID.Anvils)
            .Register();
            

            
        }
    }
}
