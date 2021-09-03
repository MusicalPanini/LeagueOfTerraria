using TerraLeague.Items.AdvItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Actives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class TwinShadows : MasterworkItem
    {
        public override string MasterworkName => "Frost Queen's Claim";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Twin Shadows");
            Tooltip.SetDefault("8% increased summon damage" +
                "\n5% increased movement speed" +
                "\nIncreases ability haste by 10");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 36;
            Item.value = Item.buyPrice(0, 45, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.accessory = true;

            Active = new SpectralPursuit(40, 15, 90);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Summon) += 0.08f;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 10;
            player.moveSpeed += 0.05f;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<Codex>(), 1)
            .AddIngredient(ItemType<AetherWisp>(), 1)
            .AddIngredient(ItemType<BlackIceChunk>(), 6)
            .AddIngredient(ItemID.SoulofFright, 10)
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

        public override string MasterworkTooltip()
        {
            return LeagueTooltip.CreateColorString(MasterColor, "10%") + " increased summon damage" +
                "\n" + LeagueTooltip.CreateColorString(MasterColor, "15%") + " increased movement speed" +
                "\nIncreases ability haste by " + LeagueTooltip.CreateColorString(MasterColor, "20");
        }

        public override void UpdateMasterwork(Player player)
        {
            player.GetDamage(DamageClass.Summon) += 0.02f;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 10;
            player.moveSpeed += 0.1f;
        }
    }
}
