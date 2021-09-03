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
    public class Endenier : MasterworkItem
    {
        public override string MasterworkName => "Chestless Cavity";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Locket of the Iron Solari");
            Tooltip.SetDefault("Increases resist by 6" +
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

            Active = new UndyingHeart(1, 60);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().armor += 4;
            player.GetModPlayer<PLAYERGLOBAL>().resist += 6;
            player.noKnockback = true;
            player.buffImmune[BuffID.Burning] = true;
            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<Cowl>(), 1)
            .AddIngredient(ItemType<GiantsBelt>(), 1)
            .AddIngredient(ItemID.HallowedChest, 1)
            .AddIngredient(ItemType<DamnedSoul>(), 20)
            .AddIngredient(ItemID.SoulofSight, 10)
            .AddIngredient(ItemID.Spike, 6)
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

        public override string MasterworkTooltip()
        {
            return "Increases armor by " + LeagueTooltip.CreateColorString(MasterColor, "8") +
                "\nIncreases resist by " + LeagueTooltip.CreateColorString(MasterColor, "10") +
                "\nGrants immunity to knockback and fire blocks" +
                "\nIncreases length of invincibility after taking damage";
        }

        public override void UpdateMasterwork(Player player)
        {
            player.GetModPlayer<PLAYERGLOBAL>().armor += 4;
            player.GetModPlayer<PLAYERGLOBAL>().resist += 4;
        }
    }
}
