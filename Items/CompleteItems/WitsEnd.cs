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
    public class WitsEnd : MasterworkItem
    {
        public override string MasterworkName => "Wit's Deathwish";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wit's End");
            Tooltip.SetDefault("24 melee On hit Damage" +
                "\n15% increased melee speed" +
                "\nIncreases resist by 4" +
                "\nIncreases your max number of minions");
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
                new Absorption()
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.meleeSpeed += 0.15f;
            player.maxMinions += 1;
            player.GetModPlayer<PLAYERGLOBAL>().resist += 4;
            player.GetModPlayer<PLAYERGLOBAL>().meleeOnHit += 24;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<RecurveBow>(), 1)
            .AddIngredient(ItemType<NegatronCloak>(), 1)
            .AddIngredient(ItemType<Dagger>(), 1)
            .AddIngredient(ItemID.Cutlass, 1)
            .AddIngredient(ItemType<SilversteelBar>(), 8)
            .AddIngredient(ItemID.SoulofSight, 10)
            .AddTile(TileID.MythrilAnvil)
            .Register();
            
        }

        public override string GetStatText()
        {
            if (Passives[0].currentlyActive)
                return (Passives[0].passiveStat).ToString();
            else
                return "";
        }

        public override string MasterworkTooltip()
        {
            return LeagueTooltip.CreateColorString(MasterColor, "32") + " melee On hit Damage" +
                "\n" + LeagueTooltip.CreateColorString(MasterColor, "20%") + " increased melee speed" +
                "\nIncreases resist by " + LeagueTooltip.CreateColorString(MasterColor, "6") +
                "\nIncreases your max number of minions";
        }

        public override void UpdateMasterwork(Player player)
        {
            player.meleeSpeed += 0.5f;
            player.GetModPlayer<PLAYERGLOBAL>().resist += 2;
            player.GetModPlayer<PLAYERGLOBAL>().meleeOnHit += 8;
        }
    }
}
