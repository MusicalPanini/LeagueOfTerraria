using TerraLeague.Items.AdvItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class Maw : MasterworkItem
    {
        public override string MasterworkName => "Malmortius' Manabane";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Maw of Malmortius");
            Tooltip.SetDefault("6% increased ranged damage" +
                "\nIncreases resist by 5");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 45, 0, 0);
            Item.rare = ItemRarityID.LightPurple;
            Item.accessory = true;

            Passives = new Passive[]
            {
                new Lifeline(90)
            };
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Ranged) += 0.06f;
            player.GetModPlayer<PLAYERGLOBAL>().resist += 5;
            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<Hexdrinker>(), 1)
            .AddIngredient(ItemType<Warhammer>(), 1)
            .AddIngredient(ItemType<SilversteelBar>(), 10)
            .AddIngredient(ItemID.SoulofFright, 10)
            .AddIngredient(ItemID.CrystalShard, 20)
            .AddTile(TileID.MythrilAnvil)
            .Register();
            
        }

        public override string GetStatText()
        {
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();

            if (Passives[0].currentlyActive)
            {
                if (modPlayer.lifeLineCooldown > 0)
                    return (modPlayer.lifeLineCooldown / 60).ToString();
            }
            return "";
        }

        public override bool OnCooldown(Player player)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (modPlayer.lifeLineCooldown > 0 || !Passives[0].currentlyActive)
                return true;
            else
                return false;
        }

        public override string MasterworkTooltip()
        {
            return LeagueTooltip.CreateColorString(MasterColor, "10%") + " increased ranged damage" +
                 "\nIncreases resist by " + LeagueTooltip.CreateColorString(MasterColor, "10");
        }

        public override void UpdateMasterwork(Player player)
        {
            player.GetDamage(DamageClass.Ranged) += 0.04f;
            player.GetModPlayer<PLAYERGLOBAL>().resist += 5;
        }
    }
}
