using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items
{
    public class DamnedSoul : ModItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(8, 4));
            DisplayName.SetDefault("Damned Soul");
            base.SetStaticDefaults();

            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;

            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.width = 16;
            Item.height = 16;
            Item.uniqueStack = false;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.buyPrice(0, 0, 0, 20);
        }

        public override void PostUpdate()
        {
            Lighting.AddLight(Item.Center, Color.DarkSeaGreen.ToVector3());
        }
    }
}
