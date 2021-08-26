using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Armor
{
    [AutoloadEquip(EquipType.Body, EquipType.Legs)]
    public class NecromancersRobe : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Necromancer's Robe");
            Tooltip.SetDefault("Increases your max number of sentries by 1");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 30;
            Item.value = 4000 * 5;
            Item.rare = ItemRarityID.Blue;
            Item.defense = 4;
        }
        
        public override void UpdateEquip(Player player)
        {
            player.maxTurrets += 1;
            player.legs = (sbyte)Mod.GetEquipSlot("NecromancersRobe", EquipType.Legs);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<DamnedSoul>(), 32)
            .AddIngredient(ItemID.Silk, 12)
            .AddTile(TileID.Loom)
            .Register();
            
        }

        public override void DrawHands(ref bool drawHands, ref bool drawArms)
        {
            drawHands = true;
            drawArms = false;
        }
    }
}
