using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items.BasicItems
{
    public class CloakofAgility : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cloak of Agility");
            Tooltip.SetDefault("5% increased ranged and melee critical strike chance");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 36;
            Item.height = 34;
            Item.value = Item.buyPrice(0, 3, 75, 0);
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
            Item.material = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetCritChance(DamageClass.Ranged) += 5;
            player.GetCritChance(DamageClass.Melee) += 5;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.Silk, 25)
            .AddTile(TileID.Loom)
            .Register();
        }
    }
}
