using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items.Accessories
{
    public class DarkinArtifact : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Darkin Artifact");
            Tooltip.SetDefault("Take on the form of a Darkin!");
            base.SetStaticDefaults();
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.rare = ItemRarityID.Pink;
            Item.value = 200000;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            modPlayer.darkinCostume = true;
            if (hideVisual)
            {
                modPlayer.darkinCostumeHideVanity = true;
            }
        }

        public override void AddRecipes()
        {
            //CreateRecipe()
            //.AddIngredient(GetInstance("TrueIceChunk"), 2);
            //.AddIngredient(ItemID.PurpleIceBlock, 32);
            //.AddIngredient(ItemID.SoulofNight, 8);
            //.AddTile(TileID.IceMachine);
            //.SetResult(this, 2);
            //
        }
    }

    public class DarkinHead : EquipTexture
    {
        public override bool DrawHead()
        {
            return false;
        }
    }

    public class DarkinBody : EquipTexture
    {
        public override bool DrawBody()
        {
            return false;
        }
    }

    public class DarkinLegs : EquipTexture
    {
        public override bool DrawLegs()
        {
            return false;
        }
    }
}
