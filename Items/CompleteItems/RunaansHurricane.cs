using TerraLeague.Items.AdvItems;
using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class RunaansHurricane : MasterworkItem
    {
        public override string MasterworkName => "Runaan's Tempest";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Runaan's Hurricane");
            Tooltip.SetDefault("15% increased ranged attack speed" +
                "\n8% increased ranged critical strike chance" +
                "\n7% increased movement speed");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 36;
            Item.value = Item.buyPrice(0, 45, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.accessory = true;

            Passives = new Passive[]
            {
                new WindsFury(false)
            };
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetCritChance(DamageClass.Ranged) += 8;
            player.GetModPlayer<PLAYERGLOBAL>().rangedAttackSpeed += 0.15;
            player.moveSpeed += 0.07f;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<Dagger>(), 2)
            .AddIngredient(ItemType<Zeal>(), 1)
            .AddIngredient(ItemID.MoltenFury, 1)
            .AddRecipeGroup("TerraLeague:Tier1Bar", 10)
            .AddIngredient(ItemID.SoulofSight, 5)
            .AddIngredient(ItemID.SoulofLight, 6)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }

        public override string MasterworkTooltip()
        {
            return LeagueTooltip.CreateColorString(MasterColor, "25%") + "15% increased ranged attack speed" +
                "\n" + LeagueTooltip.CreateColorString(MasterColor, "12%") + "8% increased ranged critical strike chance" +
                "\n" + LeagueTooltip.CreateColorString(MasterColor, "15%") + "7% increased movement speed";
        }

        public override void UpdateMasterwork(Player player)
        {
            player.GetCritChance(DamageClass.Ranged) += 4;
            player.GetModPlayer<PLAYERGLOBAL>().rangedAttackSpeed += 0.1;
            player.moveSpeed += 0.08f;
        }
    }
}
