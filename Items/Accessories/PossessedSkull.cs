using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TerraLeague.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Accessories
{
    public class PossessedSkull : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Possessed Skull");
            Tooltip.SetDefault("Increase max number of minions by 1");
            base.SetStaticDefaults();
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 30;
            Item.rare = ItemRarityID.Green;
            Item.value = 5400 * 5;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            player.maxMinions += 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.Bone, 100)
            .AddIngredient(ItemType<DamnedSoul>(), 20)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
    }
}
