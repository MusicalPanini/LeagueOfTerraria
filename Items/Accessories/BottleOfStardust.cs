using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Accessories
{
    public class BottleOfStardust : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bottle of Stardust");
            Tooltip.SetDefault("Enhances the effects of Targon's Blessings");
            base.SetStaticDefaults();
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 30;
            Item.rare = ItemRarityID.Expert;
            Item.value = 100000;
            Item.expertOnly = true;
            Item.expert = true;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            modPlayer.bottleOfStarDustBuffer = true;
        }
    }
}
