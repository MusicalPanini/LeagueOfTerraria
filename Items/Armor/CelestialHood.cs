using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class CelestialHood : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Celestial Hood");
            Tooltip.SetDefault("Increases maximum mana by 30" +
                "\nIncreases ability haste by 20" +
                "\nIncreases item and summoner spell haste by 15");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ArmorIDs.Head.Sets.DrawHatHair[Mod.GetEquipSlot(Name, EquipType.Head)] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 20;
            Item.value = 6000 * 5;
            Item.rare = ItemRarityID.Green;
            Item.defense = 3;
        }


        
        public override void UpdateEquip(Player player)
        {
            player.statManaMax2 += 30;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 20;
            player.GetModPlayer<PLAYERGLOBAL>().itemHaste += 15;
        }

        //public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        //{
        //    drawAltHair = true;
        //    //base.DrawHair(ref drawHair, ref drawAltHair);
        //}

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            if (body.type == ItemType<CelestialShirt>() && legs.type == ItemType<CelestialBoots>())
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
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            player.setBonus = "Gain " + LeagueTooltip.TooltipValue(0, false, "%", new System.Tuple<int, ScaleType>(25, ScaleType.Haste)) + " increased critical strike chance";
            player.GetCritChance(DamageClass.Generic) += modPlayer.abilityHasteLastStep / 4;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<CelestialBar>(), 10)
            .AddIngredient(ItemID.Silk, 10)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}
