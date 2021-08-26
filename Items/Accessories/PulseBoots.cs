using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items.Accessories
{
    public class PulseBoots : ModItem
    {

        /// Add cool extra jumps when they add the functionalliy
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pulse Boots");
            Tooltip.SetDefault("You can jump 6 extra times really high and sprint" +
                "\nYou are immune to fall damage");
            base.SetStaticDefaults();
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 30;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.buyPrice(0, 18, 0, 0);
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            //modPlayer.PulseJump = true;
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
    }
}
