using TerraLeague.Items.AdvItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class Warmogs : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Warmog's Armor");
            Tooltip.SetDefault("Increases maximum life by 40" +
                "\nIncreases life regeneration by 50%" +
                "\nIncreases ability haste by 10");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 60, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.accessory = true;

            Passives = new Passive[] 
            {
                new WarmogsHeart()
            };
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statLifeMax2 += 50;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 10;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<GiantsBelt>(), 1)
            .AddIngredient(ItemType<Kindlegem>(), 1)
            .AddIngredient(ItemType<CrystallineBracer>(), 1)
            .AddIngredient(ItemID.BeetleScaleMail, 1)
            .AddIngredient(ItemID.LifeFruit, 3)
            .AddIngredient(ItemID.Vine, 2)
            .AddIngredient(ItemID.JungleSpores, 5)
            .AddTile(TileID.MythrilAnvil)
            .Register();
            
        }
    }
}
