using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items.Accessories
{
    public class HexOSkeleton : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hex-O-Skeleton");
            Tooltip.SetDefault("Gain vision of Enemies and Traps" +
                "\nIncreases block placement & tool range by 3" +
                "\n50% increased mining speed" +
                "\nYou can jump 6 extra times really high and sprint" +
                "\nYou are immune to fall damage" +
                "\nPeriodically when you deal damage, cast 1 - 3 random magic spells that deal 50 magic damage.");
            base.SetStaticDefaults();
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.buyPrice(0, 36, 0, 0);
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            player.dangerSense = true;
            player.detectCreature = true;

            modPlayer.flashofBrilliance = true;

            player.blockRange += 3;
            player.pickSpeed += 0.5f;

            modPlayer.T4Boots = true;
            player.noFallDmg = true;
            player.jumpSpeedBoost += 4;
            player.jumpBoost = true;

            player.hasJumpOption_Cloud = true;
            player.hasJumpOption_Blizzard = true;
            player.hasJumpOption_Sandstorm = true;
            player.hasJumpOption_Sail = true;
            player.hasJumpOption_Unicorn = true;
            player.hasJumpOption_Fart = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<XrayGoggles>())
            .AddIngredient(ModContent.ItemType<PulseBoots>())
            .AddIngredient(ModContent.ItemType<FlashofBrilliance>())
            .AddIngredient(ModContent.ItemType<ExtendoGloves>())
            .AddTile(TileID.TinkerersWorkbench)
            .Register();
        }
    }
}
