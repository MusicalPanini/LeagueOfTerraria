using TerraLeague.Items.BasicItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.AdvItems
{
    public class RaptorCloak : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Raptor Cloak");
            Tooltip.SetDefault("4% increased summon damage" +
                "\nIncreases armor by 3" +
                "\nIncreases your max number of sentries" +
                "\nIncreases life regeneration by 2");
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
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Summon) += 0.04f;
            player.lifeRegen += 2;
            player.GetModPlayer<PLAYERGLOBAL>().armor += 3;
            player.maxTurrets += 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<ClothArmor>(), 1)
            .AddIngredient(ItemType<RejuvBead>(), 1)
            .AddIngredient(ItemID.Feather, 10)
            .AddIngredient(ItemID.SunplateBlock, 3)
            .AddTile(TileID.Anvils)
            .Register();
            
        }
    }
}
