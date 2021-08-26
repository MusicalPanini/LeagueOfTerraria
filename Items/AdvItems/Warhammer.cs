using TerraLeague.Items.BasicItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.AdvItems
{
    public class Warhammer : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Caulfield's Warhammer");
            Tooltip.SetDefault("4% increased melee and ranged damage" +
                "\nIncreases ability haste by 10");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Melee) += 0.04f;
            player.GetDamage(DamageClass.Ranged) += 0.04f;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 10;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<LongSword>(), 2)
            .AddIngredient(ItemID.MoltenHamaxe, 1)
            .AddIngredient(ItemType<DarksteelBar>(), 10)
            .AddRecipeGroup("TerraLeague:IronGroup", 5)
            .AddTile(TileID.Anvils)
            .Register();
            
        }
    }
}
