using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.AdvItems
{
    public class Sheen : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sheen");
            Tooltip.SetDefault("Increases maximum mana by 30" +
                "\nIncreases ability haste by 10");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 15, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.accessory = true;
            Item.material = true;

            Passives = new Passive[]
            {
                new Spellblade(1.5)
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statManaMax2 += 30;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 10;
            player.GetModPlayer<PLAYERGLOBAL>().spellblade = true;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<SapphireCrystal>(), 1)
            .AddIngredient(ItemID.SilverBroadsword, 1)
            .AddIngredient(ItemType<ManaBar>(), 8)
            .AddIngredient(ItemID.Sapphire, 3)
            .AddIngredient(ItemID.FallenStar, 5)
            .AddTile(TileID.Anvils)
            .Register();


            CreateRecipe()
            .AddIngredient(ItemType<SapphireCrystal>(), 1)
            .AddIngredient(ItemID.TungstenBroadsword, 1)
            .AddIngredient(ItemType<ManaBar>(), 8)
            .AddIngredient(ItemID.Sapphire, 3)
            .AddIngredient(ItemID.FallenStar, 5)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}
