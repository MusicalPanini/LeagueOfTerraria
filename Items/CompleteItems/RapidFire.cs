using TerraLeague.Items.AdvItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class RapidFire : LeagueItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rapidfire Cannon");
            Tooltip.SetDefault("20% increased ranged attack speed" +
                "\n10% increased ranged critical strike chance" +
                "\n10% increased movement speed");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 80, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.accessory = true;

            Passives = new Passive[]
            {
                new Energized(70, 40),
                new Detonate()
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetCritChance(DamageClass.Ranged) += 10;
            player.GetModPlayer<PLAYERGLOBAL>().rangedAttackSpeed += 0.2;
            player.moveSpeed += 0.10f;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<KircheisShard>(), 1)
            .AddIngredient(ItemType<Zeal>(), 1)
            .AddIngredient(ItemID.Cannon, 1)
            .AddIngredient(ItemID.RocketLauncher, 1)
            .AddIngredient(ItemID.FragmentVortex, 10)
            .AddTile(TileID.LunarCraftingStation)
            .Register();
        }

        public override string GetStatText()
        {
            if (Passives[0].currentlyActive)
                return ((int)Passives[0].passiveStat).ToString() + "%";
            else
                return "";
        }

        public override bool OnCooldown(Player player)
        {
            return !Passives[0].currentlyActive;
        }
    }
}
