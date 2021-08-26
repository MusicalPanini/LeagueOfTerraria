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
    public class Liandrys : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Liandry's Torment");
            Tooltip.SetDefault("8% increased summon damage" +
                "\nIncreases health by 40" +
                "\nIncreases your max number of minions by 2");
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
                new Madness(2),
                new Torment(3)
            };
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Summon) += 0.08f;
            player.statLifeMax2 += 40;
            player.maxMinions += 2;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<HauntingGuise>(), 1)
            .AddIngredient(ItemType<BlastingWand>(), 1)
            .AddIngredient(ItemID.PapyrusScarab, 1)
            .AddIngredient(ItemID.FragmentStardust, 10)
            .AddTile(TileID.LunarCraftingStation)
            .Register();
            
        }

        public override string GetStatText()
        {
            if (Passives[0].currentlyActive)
                return ((int)Passives[0].passiveStat).ToString();
            else
                return "";
        }
    }
}
