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
    public class Roa : MasterworkItem
    {
        public override string MasterworkName => "Rod of Millennia";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rod of Ages");
            Tooltip.SetDefault("3% increased magic and summon damage" +
                "\nIncreases maximum life by 10" +
                "\nIncreases maximum mana by 10");
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
                new Impendulum(4, 1),
                new Eternity()
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statLifeMax2 += 10;
            player.statManaMax2 += 10;
            player.GetDamage(DamageClass.Magic) += 0.03f;
            player.GetDamage(DamageClass.Summon) += 0.03f;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<Catalyst>(), 1)
            .AddIngredient(ItemType<BlastingWand>(), 1)
            .AddIngredient(ItemType<VoidFragment>(), 100)
            .AddRecipeGroup("TerraLeague:EvilPartGroup", 10)
            .AddIngredient(ItemID.SoulofLight, 6)
            .AddIngredient(ItemID.SoulofNight, 6)
            .AddIngredient(ItemID.SoulofMight, 4)
            .AddTile(TileID.Anvils)
            .Register();
            
        }

        public override string GetStatText()
        {
            if (Passives[0].currentlyActive)
                return (Impendulum.GetStat).ToString();
            else
                return "";
        }

        public override string MasterworkTooltip()
        {
            return LeagueTooltip.CreateColorString(MasterColor, "5%") + "3% increased magic and summon damage" +
                "\nIncreases maximum life by 10" + LeagueTooltip.CreateColorString(MasterColor, "20") +
                "\nIncreases maximum mana by 10" + LeagueTooltip.CreateColorString(MasterColor, "20");
        }

        public override void UpdateMasterwork(Player player)
        {
            player.statLifeMax2 += 10;
            player.statManaMax2 += 10;
            player.GetDamage(DamageClass.Magic) += 0.02f;
            player.GetDamage(DamageClass.Summon) += 0.02f;
        }
    }
}
