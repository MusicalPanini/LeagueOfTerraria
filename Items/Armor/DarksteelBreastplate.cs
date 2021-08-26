using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Armor
{
    [AutoloadEquip(EquipType.Back, EquipType.Body)]
    public class DarksteelBreastplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Darksteel Breastplate");
            Tooltip.SetDefault("4 armor" +
                "\n10% increased melee damage");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 20;
            Item.value = 45000;
            Item.rare = ItemRarityID.Orange;
            Item.defense = 7;
        }
        
        public override void UpdateEquip(Player player)
        {
            player.back = (sbyte)Mod.GetEquipSlot("DarksteelBreastplate", EquipType.Back);
            player.GetModPlayer<PLAYERGLOBAL>().armor += 4;
            player.GetDamage(DamageClass.Melee) += 0.22f;
        }

        public override void UpdateVanity(Player player)
        {
            if (player.wings <= 0)
                player.back = (sbyte)Mod.GetEquipSlot("DarksteelBreastplate", EquipType.Back); ;
            base.UpdateVanity(player);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<DarksteelBar>(), 18)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}
