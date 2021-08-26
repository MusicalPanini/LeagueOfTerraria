using TerraLeague.Items.AdvItems;
using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Actives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class Zhonyas : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Zhonya's Hourglass");
            Tooltip.SetDefault("8% increased magic damage" +
                "\nIncreases armor by 5" +
                "\nIncreases ability haste by 10");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 50, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.accessory = true;
            Active = new Stasis(2, 120);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Magic) += 0.08f;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste -= 10;
            player.GetModPlayer<PLAYERGLOBAL>().armor += 4;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<Seekers>(), 1)
            .AddIngredient(ItemType<Stopwatch>(), 1)
            .AddIngredient(ItemType<Codex>(), 1)
            .AddIngredient(ItemID.Glass, 20)
            .AddIngredient(ItemID.Ectoplasm, 8)
            .AddTile(TileID.MythrilAnvil)
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

