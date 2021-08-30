using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using TerraLeague.Items.AdvItems;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class Ardents : MasterworkItem
    {
        public override string MasterworkName => "Fervent Censer";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ardent Censer");
            Tooltip.SetDefault("6% increased magic and summon damage" +
                "\n8% increased movement speed" +
                "\nIncreases mana regeneration by 20%" +
                "\n8% increased healing power" +
                "\nIncreases ability haste by 10");
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
                new ArdentsFrenzy(this)
            };
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().healPower += 0.08;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 10;
            player.GetModPlayer<PLAYERGLOBAL>().manaRegenModifer += 0.2;
            player.GetDamage(DamageClass.Magic) += 0.06f;
            player.GetDamage(DamageClass.Summon) += 0.06f;
            player.moveSpeed += 0.08f;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<ForbiddenIdol>(), 1)
            .AddIngredient(ItemType<AetherWisp>(), 1)
            .AddIngredient(ItemID.HeartLantern, 1)
            .AddRecipeGroup("TerraLeague:Tier3Bar", 8)
            .AddIngredient(ItemID.SoulofMight, 6)
            .AddIngredient(ItemID.SoulofSight, 6)
            .AddTile(TileID.MythrilAnvil)
            .Register();
            
        }

        public override string MasterworkTooltip()
        {
            return LeagueTooltip.CreateColorString(MasterColor, "8%") + " increased magic and summon damage" +
                "\n" + LeagueTooltip.CreateColorString(MasterColor, "12%") + " increased movement speed" +
                "\nIncreases mana regeneration by " + LeagueTooltip.CreateColorString(MasterColor, "50%") +
                "\n" + LeagueTooltip.CreateColorString(MasterColor, "15%") + " increased healing power" +
                "\nIncreases ability haste by " + LeagueTooltip.CreateColorString(MasterColor, "15");
        }

        public override void UpdateMasterwork(Player player)
        {
            player.GetDamage(DamageClass.Magic) += 0.02f;
            player.GetDamage(DamageClass.Summon) += 0.02f;
            player.moveSpeed += 0.04f;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 5;
            player.GetModPlayer<PLAYERGLOBAL>().manaRegenModifer += 0.3;
            player.GetModPlayer<PLAYERGLOBAL>().healPower += 0.7;
        }
    }
}
