using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TerraLeague.Items.SummonerSpells;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items
{
    public class Sunstone : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sunstone");
            Tooltip.SetDefault("'Feels warm'");
            base.SetStaticDefaults();
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
        }

        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.width = 20;
            Item.height = 24;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.buyPrice(0, 1, 0, 0);
        }
    }
}
