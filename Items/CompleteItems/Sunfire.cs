using TerraLeague.Items.AdvItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class Sunfire : MasterworkItem
    {
        public override string MasterworkName => "Forgefire Cape";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sunfire Cape");
            Tooltip.SetDefault("Increases maximum life by 20" +
                "\nIncreases armor by 6" +
                "\nImmunity to Bleeding and Poisoned");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
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
                new Immolate(500, false)
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statLifeMax2 += 20;
            player.GetModPlayer<PLAYERGLOBAL>().armor += 6;

            if (!hideVisual)
                player.GetModPlayer<PLAYERGLOBAL>().immolate = true;

            player.buffImmune[BuffID.Bleeding] = true;
            player.buffImmune[BuffID.Poisoned] = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<ChainVest>(), 1)
            .AddIngredient(ItemType<Bamis>(), 1)
            .AddIngredient(ItemID.MedicatedBandage, 1)
            .AddIngredient(ItemID.MoltenBreastplate, 1)
            .AddIngredient(ItemID.HellstoneBar, 10)
            .AddIngredient(ItemType<Sunstone>(), 10)
            .AddTile(TileID.MythrilAnvil)
            .Register();
            
        }

        public override string MasterworkTooltip()
        {
            return "Increases maximum life by " + LeagueTooltip.CreateColorString(MasterColor, "30") +
                "\nIncreases armor by " + LeagueTooltip.CreateColorString(MasterColor, "9") +
                "\nImmunity to Bleeding and Poisoned";
        }

        public override void UpdateMasterwork(Player player)
        {
            player.statLifeMax2 += 10;
            player.GetModPlayer<PLAYERGLOBAL>().armor += 3;
        }
    }
}
