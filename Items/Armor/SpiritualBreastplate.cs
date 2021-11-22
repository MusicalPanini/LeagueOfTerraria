using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Armor
{
    [AutoloadEquip(EquipType.Body, EquipType.Back)]
    public class SpiritualBreastplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Spiritual Gown");
            Tooltip.SetDefault("10% increased heal power");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ArmorIDs.Body.Sets.HidesArms[Mod.GetEquipSlot(Name, EquipType.Body)] = true;
            ArmorIDs.Body.Sets.HidesHands[Mod.GetEquipSlot(Name, EquipType.Body)] = false;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 20;
            Item.value = 40000;
            Item.rare = ItemRarityID.Orange;
            Item.defense = 3;
            Item.backSlot = (sbyte)Mod.GetEquipSlot("SpiritualBreastplate", EquipType.Back);
        }

        public override void UpdateEquip(Player player)
        {
            player.back = Item.backSlot;
            player.GetModPlayer<PLAYERGLOBAL>().healPower += 0.1;
        }

        public override void UpdateVanity(Player player)
        {
            if (player.wings <= 0)
                player.back = Item.backSlot;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(GetInstance<ManaBar>(), 18)
            .AddTile(TileID.Anvils)
            .Register();
        }

        public override void UpdateArmorSet(Player player)
        {
        }


        //public override void DrawHands(ref bool drawHands, ref bool drawArms)
        //{
        //    drawHands = true;
        //    drawArms = false;
        //}
    }
}
