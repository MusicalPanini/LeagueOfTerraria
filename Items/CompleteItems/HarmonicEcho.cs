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
    public class HarmonicEcho : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Harmonic Echo");
            Tooltip.SetDefault("3% increased magic and summon damage" +
                "\nIncreases maximum mana by 20" +
                "\nIncreases mana regeneration by 20%" +
                "\n10% increased healing power" +
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
                new HealingEcho(20, 20, 8, 20, this),
                //new Echo(40, 10),
                new Haste(this)
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Magic) += 0.03f;
            player.GetDamage(DamageClass.Summon) += 0.03f;
            player.statManaMax2 += 20;
            player.GetModPlayer<PLAYERGLOBAL>().manaRegenModifer += 0.2;
            player.GetModPlayer<PLAYERGLOBAL>().healPower += 0.1;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 10;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<LostChapter>(), 1)
            .AddIngredient(ItemType<ForbiddenIdol>(), 1)
            .AddIngredient(ItemID.RainbowRod, 1)
            .AddIngredient(ItemID.LifeFruit, 2)
            .AddIngredient(ItemType<HarmonicBar>(), 6)
            .AddIngredient(ItemID.SpectreBar, 6)
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
    }
}
