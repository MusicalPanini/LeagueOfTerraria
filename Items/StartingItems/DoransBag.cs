using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.StartingItems
{
    public class DoransBag : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Doran's Crafting Kit");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.maxStack = 1;
            Item.material = true;
            Item.rare = ItemRarityID.LightRed;
            base.SetDefaults();
        }

        
    }
}
