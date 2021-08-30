using TerraLeague.Items.AdvItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class AbyssalMask : MasterworkItem
    {
        public override string MasterworkName => "Infernal Mask";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Abyssal Mask");
            Tooltip.SetDefault("Increases maximum life by 20" +
                "\nIncreases maximum mana by 30" +
                "\nIncreases resist by 5" +
                "\nIncreases ability haste by 10");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 45, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.accessory = true;
            Item.material = true;

            Passives = new Passive[]
            {
                new AbyssalCurse(this),
                new Eternity(this)
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().resist += 5;
            player.statLifeMax2 += 20;
            player.statManaMax2 += 30;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 10;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<Catalyst>(), 1)
            .AddIngredient(ItemType<NegatronCloak>(), 1)
            .AddIngredient(ItemID.MimeMask, 1)
            .AddIngredient(ItemType<VoidBar>(), 10)
            .AddRecipeGroup("TerraLeague:EvilPartGroup", 10)
            .AddIngredient(ItemID.SoulofNight, 10)
            .AddIngredient(ItemID.SoulofFright, 10)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }

        public override string MasterworkTooltip()
        {
            return "Increases maximum life by " + LeagueTooltip.CreateColorString(MasterColor, "30") +
                "\nIncreases maximum mana by " + LeagueTooltip.CreateColorString(MasterColor, "40") +
                "\nIncreases resist by " + LeagueTooltip.CreateColorString(MasterColor, "8") +
                "\nIncreases ability haste by " + LeagueTooltip.CreateColorString(MasterColor, "15");
        }

        public override void UpdateMasterwork(Player player)
        {
            player.GetModPlayer<PLAYERGLOBAL>().resist += 3;
            player.statLifeMax2 += 10;
            player.statManaMax2 += 10;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 5;
        }
    }
}
