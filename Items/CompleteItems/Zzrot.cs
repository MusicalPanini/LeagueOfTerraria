using TerraLeague.Items.AdvItems;
using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Actives;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class Zzrot : MasterworkItem
    {
        public override string MasterworkName => "Zz'Rot Beckoning";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Zz'Rot Portal");
            Tooltip.SetDefault("Increases armor by 3" +
                "\nIncreases resist by 4" +
                "\nIncreases your max number of sentries" +
                "\nIncreases life regeneration by 2");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 45, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.accessory = true;
            Active = new VoidCaller(20, 3, 15, 60);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.maxTurrets += 1;
            player.GetModPlayer<PLAYERGLOBAL>().resist += 6;
            player.GetModPlayer<PLAYERGLOBAL>().armor += 4;
            player.lifeRegen += 2;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<RaptorCloak>(), 1)
            .AddIngredient(ItemType<NegatronCloak>(), 1)
            .AddIngredient(ItemType<VoidFragment>(), 100)
            .AddIngredient(ItemID.SoulofSight, 10)
            .AddTile(TileID.MythrilAnvil)
            .Register();
            
        }

        public override string GetStatText()
        {
            if (Active.currentlyActive)
            {
                if (Active.cooldownCount > 0)
                    return (Active.cooldownCount / 60).ToString();
            }
            return "";
        }

        public override bool OnCooldown(Player player)
        {
            if (Active.cooldownCount > 0 || !Active.currentlyActive)
                return true;
            else
                return false;
        }

        public override string MasterworkTooltip()
        {
            return "Increases armor by " + LeagueTooltip.CreateColorString(MasterColor, "4") +
                "\nIncreases resist by " + LeagueTooltip.CreateColorString(MasterColor, "6") +
                "\nIncreases your max number of sentries by " + LeagueTooltip.CreateColorString(MasterColor, "2") +
                "\nIncreases life regeneration by " + LeagueTooltip.CreateColorString(MasterColor, "3");
        }

        public override void UpdateMasterwork(Player player)
        {
            player.maxTurrets += 1;
            player.GetModPlayer<PLAYERGLOBAL>().armor += 1;
            player.GetModPlayer<PLAYERGLOBAL>().resist += 2;
            player.lifeRegen += 1;
        }
    }
}
