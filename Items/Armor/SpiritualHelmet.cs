using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class SpiritualHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Spiritual Head Piece");
            Tooltip.SetDefault("Increases ability and item haste by 10" +
                "\nMAG increased by 30");
            base.SetStaticDefaults();
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ArmorIDs.Head.Sets.DrawFullHair[Mod.GetEquipSlot(Name, EquipType.Head)] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 16;
            Item.value = 45000;
            Item.rare = ItemRarityID.Orange;
            Item.defense = 2;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 10;
            player.GetModPlayer<PLAYERGLOBAL>().itemHaste += 10;
            player.GetModPlayer<PLAYERGLOBAL>().BonusMAG += 30;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<ManaBar>(), 15)
            .AddIngredient(ItemID.Emerald, 1)
            .AddTile(TileID.Anvils)
            .Register();
            
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            if(body.type == ItemType<SpiritualBreastplate>() && legs.type == ItemType<SpiritualLeggings>())
                return true;
            else
                return false;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Gain a bunch of stats while below 50% life";
            player.armorEffectDrawShadowLokis = true;
        }

        //public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        //{
        //    drawHair = true;
        //}
    }
}
