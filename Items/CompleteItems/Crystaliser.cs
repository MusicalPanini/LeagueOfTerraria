using Microsoft.Xna.Framework;
using TerraLeague.Items.AdvItems;
using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class Crystaliser : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crystaliser");
            Tooltip.SetDefault("8% increased magic and summon damage" +
                "\n10% increased critical strike chance" +
                "\nIncreases mana regeneration by 50%" +
                "\n15% increased healing power");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 80, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.accessory = true;

            Passives = new Passive[]
            {
                new Crystallization(10, 50, 2, Color.Lavender, this)
            };
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().healPower += 0.15;
            player.GetModPlayer<PLAYERGLOBAL>().manaRegenModifer += 0.5;
            player.GetDamage(DamageClass.Magic) += 0.08f;
            player.GetDamage(DamageClass.Summon) += 0.08f;
            player.GetCritChance(DamageClass.Generic) += 10;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<ForbiddenIdol>(), 1)
            .AddIngredient(ItemType<Pickaxe>(), 1)
            .AddIngredient(ItemType<BrawlersGlove>(), 1)
            .AddIngredient(ItemType<HexCrystal>(), 1)
            .AddIngredient(ItemID.Sapphire, 1)
            .AddIngredient(ItemID.FragmentNebula, 10)
            .AddTile(TileID.LunarCraftingStation)
            .Register();
        }

        public override bool CanEquipAccessory(Player player, int slot)
        {
            return base.CanEquipAccessory(player, slot);
        }
    }
}
