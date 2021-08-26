using TerraLeague.Items.Weapons;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.StartingItems
{
    public class WeaponKit : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Basic Weapon Kit");
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
