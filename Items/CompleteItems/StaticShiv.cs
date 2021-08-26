using TerraLeague.Items.AdvItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class StaticShiv : LeagueItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Statikk Shiv");
            Tooltip.SetDefault("12% increased melee and ranged attack speed" +
                "\n6% increased melee and ranged critical strike chance" +
                "\n5% increased movement speed");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 30, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.accessory = true;

            Passives = new Passive[]
            {
                new Energized(35, 25),
                new Discharge()
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetCritChance(DamageClass.Melee) += 6;
            player.GetCritChance(DamageClass.Ranged) += 6;
            player.meleeSpeed += 0.15f;
            player.GetModPlayer<PLAYERGLOBAL>().rangedAttackSpeed += 0.12;
            player.moveSpeed += 0.05f;
            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<Zeal>(), 1)
            .AddIngredient(ItemType<KircheisShard>(), 1)
            .AddIngredient(ItemID.IronBroadsword, 1)
            .AddIngredient(ItemID.NimbusRod, 1)
            .AddTile(TileID.Anvils)
            .Register();

            CreateRecipe()
            .AddIngredient(ItemType<Zeal>(), 1)
            .AddIngredient(ItemType<KircheisShard>(), 1)
            .AddIngredient(ItemID.LeadBroadsword, 1)
            .AddIngredient(ItemID.NimbusRod, 1)
            .AddTile(TileID.Anvils)
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
