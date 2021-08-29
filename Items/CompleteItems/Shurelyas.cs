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
    public class Shurelyas : MasterworkItem
    {
        public override string MasterworkName => "Shurelya's Battlesong";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shurelya's Reverie");
            Tooltip.SetDefault("7% increased magic and summon damage" +
                "\n5% increased movement speed" +
                "\nIncreases maximum life by 20" +
                "\nIncreases mana regeneration by 60%" +
                "\nIncreases ability haste by 15");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 30, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.accessory = true;

            Active = new FleetFoot(500, 4, 90);
        }

        public override bool CanEquipAccessory(Player player, int slot)
        {
            if (slot >= 3 && slot <= 8)
                player.GetModPlayer<PLAYERGLOBAL>().accessoryStat[slot - 3] = (int)(90 * player.GetModPlayer<PLAYERGLOBAL>().Cdr * 60);
            return true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Summon) += 0.07f;
            player.GetDamage(DamageClass.Magic) += 0.07f;
            player.moveSpeed += 0.05f;
            player.statLifeMax2 += 20;
            player.GetModPlayer<PLAYERGLOBAL>().manaRegenModifer += 0.6;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 15;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<Kindlegem>(), 1)
            .AddIngredient(ItemType<AetherWisp>(), 1)
            .AddIngredient(ItemType<FaerieCharm>(), 1)
            .AddIngredient(ItemID.AncientBattleArmorMaterial, 1) // Forbidden Fragment
            .AddIngredient(ItemType<Sunstone>(), 10)
            .AddIngredient(ItemID.SoulofLight, 5)
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
            return LeagueTooltip.CreateColorString(MasterColor, "10%") + " increased magic and summon damage" +
                "\n" + LeagueTooltip.CreateColorString(MasterColor, "15%") + " increased movement speed" +
                "\nIncreases maximum life by " + LeagueTooltip.CreateColorString(MasterColor, "30") +
                "\nIncreases mana regeneration by " + LeagueTooltip.CreateColorString(MasterColor, "100%") +
                "\nIncreases ability haste by " + LeagueTooltip.CreateColorString(MasterColor, "20");
        }

        public override void UpdateMasterwork(Player player)
        {
            player.GetDamage(DamageClass.Summon) += 0.03f;
            player.GetDamage(DamageClass.Magic) += 0.03f;
            player.moveSpeed += 0.1f;
            player.statLifeMax2 += 10;
            player.GetModPlayer<PLAYERGLOBAL>().manaRegenModifer += 0.4;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 5;
        }
    }
}
