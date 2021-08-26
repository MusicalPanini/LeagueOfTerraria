using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Accessories
{
    public class BloomPendant : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bloom Pendant");
            Tooltip.SetDefault("Increases mana regeneration by 1" +
                "\nWhile below 50% life increase mana regeneration by 3" +
                "\nIncreases length of invincibility after taking damage");
            base.SetStaticDefaults();
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 32;
            Item.rare = ItemRarityID.Pink;
            Item.value = 100000;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            modPlayer.manaRegen += 1;
            player.longInvince = true;
            if (modPlayer.GetRealHeathWithoutShield(true)/2 > modPlayer.GetRealHeathWithoutShield())
            {
                modPlayer.manaRegen += 3;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.CrossNecklace, 1)
            .AddIngredient(ItemType<Nightbloom>(), 1)
            .AddTile(TileID.TinkerersWorkbench)
            .Register();
        }
    }
}
