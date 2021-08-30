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
    public class Morellos : MasterworkItem
    {
        public override string MasterworkName => "Morello's Forbidden Writings";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Morellonomicon");
            Tooltip.SetDefault("5% increased magic damage" +
                "\nIncreases health by 20");
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
                new CursedStrike(this),
                new TouchOfDeath(15, this)
            };
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Magic) += 0.05f;
            player.statLifeMax2 += 20;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<Orb>(), 1)
            .AddIngredient(ItemType<BlastingWand>(), 1)
            .AddIngredient(ItemID.SpellTome, 1)
            .AddIngredient(ItemType<DamnedSoul>(), 50)
            .AddIngredient(ItemID.SoulofFright, 10)
            .AddTile(TileID.CrystalBall)
            .Register();
            
        }

        public override string MasterworkTooltip()
        {
            return LeagueTooltip.CreateColorString(MasterColor, "10%") + " increased magic damage" +
                "\nIncreases health by " + LeagueTooltip.CreateColorString(MasterColor, "30");
        }

        public override void UpdateMasterwork(Player player)
        {
            player.GetDamage(DamageClass.Magic) += 0.05f;
            player.statLifeMax2 += 10;
        }
    }
}
