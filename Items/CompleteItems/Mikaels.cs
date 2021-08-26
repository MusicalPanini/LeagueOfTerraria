using TerraLeague.Items.AdvItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Actives;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class Mikaels : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mikael's Crucible");
            Tooltip.SetDefault("Increases resist by 4" +
                "\nIncreases mana regeneration by 30%" +
                "\n10% increased healing power" +
                "\nIncreases ability haste by 10" +
                "\nImmunity to Curse");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 25, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.accessory = true;

            Active = new Purify(60);
            Passives = new Passive[]
            {
                new Harmony(1, 30)
            };
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().resist += 4;
            player.GetModPlayer<PLAYERGLOBAL>().manaRegenModifer += 0.3;
            player.GetModPlayer<PLAYERGLOBAL>().healPower += 0.1;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 10;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<Chalice>(), 1)
            .AddIngredient(ItemType<ForbiddenIdol>(), 1)
            .AddRecipeGroup("TerraLeague:GoldGroup", 10)
            .AddIngredient(ItemID.HellstoneBar, 4)
            .AddIngredient(ItemID.Nazar, 1)
            .AddTile(TileID.Anvils)
            .Register();
            
        }

        public override string GetStatText()
        {
            if (Active.currentlyActive)
            {
                if (Active.cooldownCount > 0)
                    return (Active.cooldownCount / 60).ToString();
            }
            return "";
        }

        public override bool OnCooldown(Player player)
        {
            if (Active.cooldownCount > 0 || !Active.currentlyActive)
                return true;
            else
                return false;
        }
    }
}
