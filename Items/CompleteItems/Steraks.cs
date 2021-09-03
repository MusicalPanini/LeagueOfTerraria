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
    public class Steraks : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sterak's Gage");
            Tooltip.SetDefault("Increases maximum life by 40" +
                "\nIncreases melee knockback");
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
                new Lifeline(90, this),
                new GiantStrength(25, this)
            };
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statLifeMax2 += 40;
            player.kbGlove = true;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<Jaurim>(), 1)
            .AddIngredient(ItemType<Pickaxe>(), 1)
            .AddIngredient(ItemType<RubyCrystal>(), 1)
            .AddIngredient(ItemID.FragmentSolar, 10)
            .AddIngredient(ItemType<BrassBar>(), 20)
            .AddIngredient(ItemID.MechanicalGlove, 1)
            .AddTile(TileID.LunarCraftingStation)
            .Register();
            
        }

        public override string GetStatText()
        {
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();

            if (Passives[0].currentlyActive)
            {
                if (modPlayer.lifeLineCooldown > 0)
                    return (modPlayer.lifeLineCooldown / 60).ToString();
            }
            return "";
        }

        public override bool OnCooldown(Player player)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (modPlayer.lifeLineCooldown > 0 || !Passives[0].currentlyActive)
                return true;
            else
                return false;
        }
    }
}
