using TerraLeague.Items.PetrifiedWood;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class PetLeggings : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Petrified Greaves");
            Tooltip.SetDefault("Decreases maximum mana by 20");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.value = 0;
            Item.rare = ItemRarityID.Blue;
            Item.defense = 1;
        }

        public override void UpdateEquip(Player player)
        {
            player.statManaMax2 -= 20;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<PetWood>(), 25)
            .AddTile(TileID.WorkBenches)
            .Register();
            
        }

        public override void UpdateArmorSet(Player player)
        {
        }
    }
}
