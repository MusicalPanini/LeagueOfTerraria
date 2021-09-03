using TerraLeague.Items.AdvItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Actives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class Youmuus : MasterworkItem
    {
        public override string MasterworkName => "Yomuus's Wraithblade";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Youmuu's Ghostblade");
            Tooltip.SetDefault("8% increased melee and ranged damage" +
                "\n8% increased movement speed" +
                "\nIncreases ability haste by 10" +
                "\nIncreases melee armor penetration by 7");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 30, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.accessory = true;

            Active = new PoltergeistsAscension(10, 60);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Melee) += 0.08f;
            player.GetDamage(DamageClass.Ranged) += 0.08f;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 10;
            player.GetModPlayer<PLAYERGLOBAL>().meleeArmorPen += 7;
            player.moveSpeed += 0.08f;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<SerratedDirk>(), 1)
            .AddIngredient(ItemType<Warhammer>(), 1)
            .AddIngredient(ItemID.SoulofLight, 8)
            .AddIngredient(ItemID.SoulofNight, 8)
            .AddRecipeGroup("TerraLeague:DemonGroup", 5)
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
            return LeagueTooltip.CreateColorString(MasterColor, "10%") + " increased melee and ranged damage" +
                "\n" + LeagueTooltip.CreateColorString(MasterColor, "15%") + " increased movement speed" +
                "\nIncreases ability haste by " + LeagueTooltip.CreateColorString(MasterColor, "20") +
                "\nIncreases melee armor penetration by " + LeagueTooltip.CreateColorString(MasterColor, "12");
        }

        public override void UpdateMasterwork(Player player)
        {
            player.GetDamage(DamageClass.Melee) += 0.02f;
            player.GetDamage(DamageClass.Ranged) += 0.02f;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 10;
            player.GetModPlayer<PLAYERGLOBAL>().meleeArmorPen += 5;
            player.moveSpeed += 0.07f;
        }
    }
}
