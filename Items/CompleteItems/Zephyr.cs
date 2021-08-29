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
    public class Zephyr : MasterworkItem
    {
        public override string MasterworkName => "Typhoon";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Zephyr");
            Tooltip.SetDefault("4% increased melee and ranged damage" +
                "\n5% increased melee and ranged attack speed" +
                "\n10% increased movement speed" +
                "\nIncreases ability haste by 10" +
                "\nImmunity to Slow and Chilled");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 20, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.accessory = true;

            Passives = new Passive[]
            {
                new WindPower()
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Melee) += 0.04f;
            player.GetDamage(DamageClass.Ranged) += 0.04f;
            player.moveSpeed += 0.1f;
            player.meleeSpeed += 0.05f;
            player.GetModPlayer<PLAYERGLOBAL>().rangedAttackSpeed += 0.05;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 10;

            player.buffImmune[BuffID.Slow] = true;
            player.buffImmune[BuffID.Chilled] = true;
            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<BFSword>(), 1)
            .AddIngredient(ItemType<Stinger>(), 1)
            .AddIngredient(ItemType<Dagger>(), 1)
            .AddIngredient(ItemType<ManaBar>(), 12)
            .AddIngredient(ItemID.Cloud, 10)
            .AddTile(TileID.Anvils)
            .Register();
            
        }

        public override string GetStatText()
        {
            return "";
        }

        public override string MasterworkTooltip()
        {
            return LeagueTooltip.CreateColorString(MasterColor, "6%") + " increased melee and ranged damage" +
                "\n" + LeagueTooltip.CreateColorString(MasterColor, "12%") + " increased melee and ranged attack speed" +
                "\n" + LeagueTooltip.CreateColorString(MasterColor, "25%") + " increased movement speed" +
                "\nIncreases ability haste by 10" + LeagueTooltip.CreateColorString(MasterColor, "15") +
                "\nImmunity to Slow and Chilled";
        }

        public override void UpdateMasterwork(Player player)
        {
            player.GetDamage(DamageClass.Melee) += 0.02f;
            player.GetDamage(DamageClass.Ranged) += 0.02f;
            player.moveSpeed += 0.15f;
            player.meleeSpeed += 0.07f;
            player.GetModPlayer<PLAYERGLOBAL>().rangedAttackSpeed += 0.07;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 5;
        }
    }
}
