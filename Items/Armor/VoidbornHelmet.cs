using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class VoidbornHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Voidborn Terror Mask");
            Tooltip.SetDefault("Increases your max number of minions" +
            "\nIncreases your max mana by 40");
            base.SetStaticDefaults();
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 26;
            Item.value = 40000;
            Item.rare = ItemRarityID.Orange;
            Item.defense = 4;
        }

        public override void UpdateEquip(Player player)
        {
            player.statManaMax2 += 40;
            player.maxMinions++;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.FossilHelm, 1)
            .AddIngredient(GetInstance<VoidFragment>(), 60)
            .AddTile(TileID.Anvils)
            .Register();
            
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {

            if(body.type == ItemType<VoidbornBreastplate>() && legs.type == ItemType<VoidbornLeggings>())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "When you deal summon damage, restore 2 mana";
            player.armorEffectDrawShadowBasilisk = true;
            player.GetModPlayer<PLAYERGLOBAL>().voidbornSet = true;
        }
    }
}
