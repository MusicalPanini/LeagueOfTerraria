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
    public class Locket : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Locket of the Iron Solari");
            Tooltip.SetDefault("Increases armor by 4" +
                "\nIncreases resist by 6" +
                "\nGrants immunity to knockback and fire blocks" +
                "\nIncreases length of invincibility after taking damage");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 36;
            Item.value = Item.buyPrice(0, 30, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.accessory = true;

            Active = new SolarisProtection(20, 500, 10, 120);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().armor += 4;
            player.GetModPlayer<PLAYERGLOBAL>().resist += 6;
            player.noKnockback = true;
            player.buffImmune[BuffID.Burning] = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<Aegis>(), 1)
            .AddIngredient(ItemType<NullMagic>(), 1)
            .AddIngredient(ItemID.CrossNecklace, 1)
            .AddIngredient(ItemID.CobaltShield, 1)
            .AddIngredient(ItemID.SunplateBlock, 20)
            .AddIngredient(ItemID.LifeCrystal, 2)
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
