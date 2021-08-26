using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items
{
    public class GreaterSoul : ModItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(8, 4));
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            DisplayName.SetDefault("Greater Soul");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.width = 32;
            Item.height = 32;
            Item.uniqueStack = false;
            Item.rare = ItemRarityID.Orange;
            Item.value = 100;
        }

        public override void PostUpdate()
        {
            ItemID.Sets.ItemIconPulse[Item.type] = false;

            Lighting.AddLight(Item.Center, Color.DarkSeaGreen.ToVector3() * 0.55f * Main.essScale);
        }
    }
}
