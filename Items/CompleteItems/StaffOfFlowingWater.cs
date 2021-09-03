using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using TerraLeague.Items.AdvItems;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class StaffOfFlowingWater : MasterworkItem
    {
        public override string MasterworkName => "Staff of Rapids";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Staff of Flowing Water");
            Tooltip.SetDefault("8% increased magic and summon damage" +
                "\nIncreases mana regeneration by 20%" +
                "\n8% increased healing power");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 45, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.accessory = true;

            Passives = new Passive[]
            {
                new Rapids(this)
            };
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().healPower += 0.08;
            player.GetModPlayer<PLAYERGLOBAL>().manaRegenModifer += 0.2;
            player.GetDamage(DamageClass.Magic) += 0.08f;
            player.GetDamage(DamageClass.Summon) += 0.08f;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<ForbiddenIdol>(), 1)
            .AddIngredient(ItemType<BasicItems.BlastingWand>(), 1)
            .AddIngredient(ItemType<ManaBar>(), 8)
            .AddIngredient(ItemID.SoulofMight, 6)
            .AddIngredient(ItemID.AquaScepter, 1)
            .AddTile(TileID.MythrilAnvil)
            .Register();
            
        }

        public override string MasterworkTooltip()
        {
            return LeagueTooltip.CreateColorString(MasterColor, "10%") + " increased magic and summon damage" +
                "\nIncreases mana regeneration by " + LeagueTooltip.CreateColorString(MasterColor, "50%") +
                "\n" + LeagueTooltip.CreateColorString(MasterColor, "15%") + " increased healing power";
        }

        public override void UpdateMasterwork(Player player)
        {
            player.GetModPlayer<PLAYERGLOBAL>().healPower += 0.07;
            player.GetModPlayer<PLAYERGLOBAL>().manaRegenModifer += 0.3;
            player.GetDamage(DamageClass.Magic) += 0.02f;
            player.GetDamage(DamageClass.Summon) += 0.02f;
        }
    }
}
