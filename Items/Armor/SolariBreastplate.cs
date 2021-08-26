using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Armor
{
    [AutoloadEquip(EquipType.Back, EquipType.Body)]
    public class SolariBreastplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Solari Breastplate");
            Tooltip.SetDefault("10 armor" +
                "\nIncreases your max life by 20");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 20;
            Item.value = 145000 * 5;
            Item.rare = ItemRarityID.Yellow;
            Item.defense = 30;
        }


        
        public override void UpdateEquip(Player player)
        {
            player.back = (sbyte)Mod.GetEquipSlot("SolariBreastplate", EquipType.Back);
            player.GetModPlayer<PLAYERGLOBAL>().armor += 10;
            player.statLifeMax2 += 20;
        }

        public override void UpdateVanity(Player player)
        {
            if (player.wings <= 0)
                player.back = (sbyte)Mod.GetEquipSlot("SolariBreastplate", EquipType.Back); ;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<CelestialBar>(), 14)
            .AddIngredient(ItemType<FragmentOfTheAspect>(), 1)
            .AddIngredient(ItemID.LunarTabletFragment, 8)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}
