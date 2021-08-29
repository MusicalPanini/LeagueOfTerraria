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
    public class Ludens : MasterworkItem
    {
        public override string MasterworkName => "Eye of Luden";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Luden's Tempest");
            Tooltip.SetDefault("6% increased magic damage" +
                "\nIncreases maximum mana by 20" +
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
                new TempestEcho(40, 10, 16),
                //new Echo(40, 10),
                new Haste()
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Magic) += 0.06f;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 10;
            player.statManaMax2 += 20;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<LostChapter>(), 1)
            .AddIngredient(ItemType<BlastingWand>(), 1)
            .AddIngredient(ItemID.RainbowRod, 1)
            .AddIngredient(ItemID.CrystalShard, 10)
            .AddIngredient(ItemType<VoidFragment>(), 100)
            .AddIngredient(ItemID.SoulofNight, 15)
            .AddTile(TileID.MythrilAnvil)
            .Register();
            
        }

        public override string GetStatText()
        {
            //if (Passives[0].currentlyActive)
            //    return ((int)Passives[0].passiveStat).ToString() + "%";
            //else
            //    return "";

            if (Passives[0].currentlyActive)
            {
                if (Passives[0].cooldownCount > 0)
                    return (Passives[0].cooldownCount / 60).ToString();
            }
            return "";
        }

        public override bool OnCooldown(Player player)
        {
            return (Passives[0].cooldownCount > 0);
        }

        public override string MasterworkTooltip()
        {
            return LeagueTooltip.CreateColorString(MasterColor, "10") + " increased magic damage" +
                "\nIncreases maximum mana by " + LeagueTooltip.CreateColorString(MasterColor, "30") +
                "\nIncreases ability haste by " + LeagueTooltip.CreateColorString(MasterColor, "25");
        }

        public override void UpdateMasterwork(Player player)
        {
            player.GetDamage(DamageClass.Magic) += 0.04f;
            player.statManaMax2 += 10;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 15;
        }
    }
}
