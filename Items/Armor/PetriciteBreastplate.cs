using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Armor
{
    [AutoloadEquip(EquipType.Body, EquipType.Back)]
    public class PetriciteBreastplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Silver-Steel Breastplate");
            Tooltip.SetDefault("5 resist" +
            "\n10% increased melee damage" +
            "\nEnemies are more likely to target you");
            base.SetStaticDefaults();
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 26;
            Item.value = 45000;
            Item.rare = ItemRarityID.Orange;
            Item.defense = 6;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<PLAYERGLOBAL>().resist += 5;
            player.GetDamage(DamageClass.Melee) += 0.07f;
            player.aggro += 150;
            player.back = (sbyte)Mod.GetEquipSlot("PetriciteBreastplate", EquipType.Back);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(GetInstance<SilversteelBar>(), 18)
            .AddTile(TileID.Anvils)
            .Register();
        }

        public override void UpdateVanity(Player player)
        {
            if (player.wings <= 0)
                player.back = (sbyte)Mod.GetEquipSlot("PetriciteBreastplate", EquipType.Back);
        }
    }
}
