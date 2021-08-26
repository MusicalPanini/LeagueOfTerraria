using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items.StartingItems
{
    public class DoransRing : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Doran's Ring");
            Tooltip.SetDefault("+2 magic and summon damage" +
                "\nIncreases health by 5" +
                "\nIncreases mana regeneration by 1" +
                "\nRestore 5 mana on kill");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = 10000;
            Item.rare = ItemRarityID.Blue;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().dring = true;
            player.GetModPlayer<PLAYERGLOBAL>().manaRegen += 1;
            player.statLifeMax2 += 5;
            player.GetModPlayer<PLAYERGLOBAL>().magicFlatDamage += 2;
            player.GetModPlayer<PLAYERGLOBAL>().minionFlatDamage += 2;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<DoransBag>(), 1)
            .Register();
        }
    }
}
