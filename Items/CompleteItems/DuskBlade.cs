using TerraLeague.Items.AdvItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class DuskBlade : MasterworkItem
    {
        public override string MasterworkName => "Draktharr's Shadowcarver";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Duskblade of Draktharr");
            Tooltip.SetDefault("10% increased melee damage" +
                "\nIncreases ability haste by 10" +
                "\nIncreases melee armor penetration by 7");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 30;
            Item.value = Item.buyPrice(0, 45, 0, 0);
            Item.rare = ItemRarityID.Lime;
            Item.accessory = true;

            Passives = new Passive[]
            {
                new Nightstalker(3, 50, this)
            };
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Melee) += 0.1f;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 10;
            player.GetModPlayer<PLAYERGLOBAL>().meleeArmorPen += 7;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<SerratedDirk>(), 1)
            .AddIngredient(ItemType<Warhammer>(), 1)
            .AddIngredient(ItemID.WarriorEmblem, 1)
            .AddIngredient(ItemType<HarmonicBar>(), 8)
            .AddIngredient(ItemID.SoulofSight, 6)
            .AddIngredient(ItemID.SoulofFright, 6)
            .AddTile(TileID.MythrilAnvil)
            .Register();
            
        }

        public override string GetStatText()
        {
            if (Passives[0].currentlyActive)
                return ((int)Passives[0].passiveStat).ToString() + "%";
            else
                return "";
        }

        public override string MasterworkTooltip()
        {
            return LeagueTooltip.CreateColorString(MasterColor, "12%") + " increased melee damage" +
                "\nIncreases ability haste by " + LeagueTooltip.CreateColorString(MasterColor, "20") +
                "\nIncreases melee armor penetration by " + LeagueTooltip.CreateColorString(MasterColor, "12");
        }

        public override void UpdateMasterwork(Player player)
        {
            player.GetDamage(DamageClass.Melee) += 0.02f;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 10;
            player.GetModPlayer<PLAYERGLOBAL>().meleeArmorPen += 5;
        }
    }
}
