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
    public class TimekeepersFury : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Timekeeper's Wrath");
            Tooltip.SetDefault("3% increased magic and summon damage" +
                "\nIncreases maximum life by 10" +
                "\nIncreases maximum mana by 10" +
                "\n[c/007399:TOUCH OF DEATH's magic pen is based on IMPENDULUM]");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 45, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.accessory = true;
            Item.material = true;

            Passives = new Passive[]
            {
                new Impendulum(2, 1.5),
                new TouchOfDeath(2)
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statLifeMax2 += 10;
            player.statManaMax2 += 10;
            player.GetDamage(DamageClass.Magic) += 0.03f;
            player.GetDamage(DamageClass.Summon) += 0.03f;

            Passives[1].passiveStat = Impendulum.GetStat;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<Orb>(), 1)
            .AddIngredient(ItemType<BlastingWand>(), 1)
            .AddIngredient(ItemType<SapphireCrystal>(), 1)
            .AddIngredient(ItemID.SoulofLight, 6)
            .AddIngredient(ItemID.SoulofNight, 6)
            .AddIngredient(ItemID.SoulofMight, 4)
            .AddTile(TileID.Anvils)
            .Register();
            
        }

        public override string GetStatText()
        {
            if (Active.currentlyActive)
            {
                return Impendulum.GetStat.ToString();
            }
            return "";
        }
    }
}
