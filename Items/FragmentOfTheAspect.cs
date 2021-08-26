using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TerraLeague.Items.SummonerSpells;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items
{
    public class FragmentOfTheAspect : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fragment of the Aspect");
            Tooltip.SetDefault("Disappears after the sunset" +
                "\n'Gift from the gods'");
            base.SetStaticDefaults();

            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }

        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.width = 14;
            Item.height = 18;
            Item.rare = ItemRarityID.Cyan;
            Item.value = Item.buyPrice(0, 50, 0, 0);
        }

        public override void PostUpdate()
        {
            Lighting.AddLight(Item.Center, Color.Blue.ToVector3());

            if (!Main.dayTime)
            {
                Item.SetDefaults();

                for (int i = 0; i < 20; i++)
                {
                    Dust.NewDustDirect(Item.position, Item.width, Item.height, DustID.BlueTorch);
                }
            }
        }
    }
}
