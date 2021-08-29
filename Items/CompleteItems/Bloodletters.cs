using TerraLeague.Items.AdvItems;
using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Actives;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class Bloodletters : MasterworkItem
    {
        public override string MasterworkName => "Hemomancer's Veil";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bloodletter's Veil");
            Tooltip.SetDefault("5% increased magic and summon damage" +
                "\nIncreases maximum life by 10");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override string MasterworkTooltip()
        {
            return LeagueTooltip.CreateColorString(MasterColor, "7%") + " increased magic and summon damage" +
                "\nIncreases maximum life by " + LeagueTooltip.CreateColorString(MasterColor, "30");
        }

        public override void UpdateMasterwork(Player player)
        {
            player.GetDamage(DamageClass.Magic) += 0.02f;
            player.statLifeMax2 += 20;
            player.GetDamage(DamageClass.Summon) += 0.02f;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 25, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.accessory = true;

            Active = new NightsVeil(7, 120, 75);
            Passives = new Passive[]
            {
                new TouchOfDeath(10)
            };
        }

        public override void UpdateVanity(Player player)
        {
            base.UpdateVanity(player);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Magic) += 0.05f;
            player.statLifeMax2 += 10;
            player.GetDamage(DamageClass.Summon) += 0.05f;
            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<Orb>(), 1)
            .AddIngredient(ItemType<BlastingWand>(), 1)
            .AddIngredient(ItemType<BrassBar>(), 12)
            .AddIngredient(ItemID.Bone, 10)
            .AddIngredient(ItemType<DamnedSoul>(), 10)
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
