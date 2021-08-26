using TerraLeague.Items.AdvItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class DeadMans : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dead Man's Plate");
            Tooltip.SetDefault("Increases maximum life by 30" +
                "\nIncreases armor by 6" +
                "\nImmunity to Weakness and Broken Armor");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 45, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.accessory = true;

            Passives = new Passive[]
            {
                new Dreadnought(0.05f)
            };
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statLifeMax2 += 30;
            player.GetModPlayer<PLAYERGLOBAL>().armor += 6;

            player.buffImmune[BuffID.Weak] = true;
            player.buffImmune[BuffID.BrokenArmor] = true;

        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<ChainVest>(), 1)
            .AddIngredient(ItemType<GiantsBelt>(), 1)
            .AddIngredient(ItemID.ArmorBracing, 1)
            .AddIngredient(ItemID.NecroBreastplate, 1)
            .AddIngredient(ItemType<BrassBar>(), 10)
            .AddIngredient(ItemID.SoulofMight, 8)
            .AddTile(TileID.MythrilAnvil)
            .Register();
            
        }

        public override string GetStatText()
        {
            if (Passives[0].currentlyActive)
                return ((int)Passives[0].passiveStat).ToString() + "%";
            else
                return "";
        }
    }
}
