using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Accessories
{
    public class NightbloomSkull : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Nightbloom Skull");
            Tooltip.SetDefault("Increase max number of minions by 1" +
                "\nIncreases mana regeneration by 1" +
                "\nWhile below 50% life increase mana regeneration by 3");
            base.SetStaticDefaults();
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 30;
            Item.rare = ItemRarityID.LightRed;
            Item.value = 100000;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            modPlayer.manaRegen += 1;
            player.maxMinions += 1;
            if (modPlayer.GetRealHeathWithoutShield(true)/2 > modPlayer.GetRealHeathWithoutShield())
            {
                modPlayer.manaRegen += 3;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<Nightbloom>(), 1)
            .AddIngredient(ItemType<PossessedSkull>(), 1)
            .AddTile(TileID.TinkerersWorkbench)
            .Register();
        }
    }
}
