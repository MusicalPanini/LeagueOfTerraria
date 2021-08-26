using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.AdvItems
{
    public class HauntingGuise : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Haunting Guise");
            Tooltip.SetDefault("5% increased summon damage" +
                "\nIncreases health by 20" +
                "\nIncreases your max number of minions by 1");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 25, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.accessory = true;

            Passives = new Passive[]
            {
                new Madness(1)
            };
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Summon) += 0.05f;
            player.statLifeMax2 += 20;
            player.maxMinions += 1;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<RubyCrystal>(), 1)
            .AddIngredient(ItemType<AmpTome>(), 1)
            .AddIngredient(ItemID.NecromanticScroll, 1)
            .AddIngredient(ItemID.MimeMask, 1)
            .AddIngredient(ItemID.SoulofFright, 5)
            .AddTile(TileID.MythrilAnvil)
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
