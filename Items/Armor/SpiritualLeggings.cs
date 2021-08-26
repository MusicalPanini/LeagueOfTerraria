using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class SpiritualLeggings : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Spiritual Leggings");
            Tooltip.SetDefault("Increases mana regeneration by 100%" +
                "\n5% increased movement speed");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.value = 40000;
            Item.rare = ItemRarityID.Orange;
            Item.defense = 3;
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.05f;
            player.GetModPlayer<PLAYERGLOBAL>().manaRegenModifer += 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(GetInstance<ManaBar>(), 12)
            .AddTile(TileID.Anvils)
            .Register();
            
        }

        public override void UpdateArmorSet(Player player)
        {
        }

        
    }
}
