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
    public class Lichbane : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lich Bane");
            Tooltip.SetDefault("15% increased summon damage" +
                "\n7% increased movement speed" +
                "\nIncreases maximum mana by 30" +
                "\nIncreases ability haste by 10" +
                "\nIncreases your max number of minions");
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
                new Spellblade(1.5),
                new SummonedBlade(120)
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Summon) += 0.15f;
            player.moveSpeed += 0.07f;
            player.statManaMax2 += 30;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 10;
            player.maxMinions += 1;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<Sheen>(), 1)
            .AddIngredient(ItemType<AetherWisp>(), 1)
            .AddIngredient(ItemType<BlastingWand>(), 1)
            .AddIngredient(ItemID.BrokenHeroSword, 1)
            .AddIngredient(ItemID.FragmentStardust, 10)
            .AddTile(TileID.LunarCraftingStation)
            .Register();
            
        }
    }
}
