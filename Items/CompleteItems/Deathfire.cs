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
    public class Deathfire : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Deathfire Grasp");
            Tooltip.SetDefault("12% increased magic damage" +
               "\nIncreases ability haste by 15");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 60, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.accessory = true;

            Active = new Doom(25, 1000, 120);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Magic) += 0.12f;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 15;
            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            // Add late game shadow isles material

            CreateRecipe()
            .AddIngredient(ItemType<LargeRod>(), 1)
            .AddIngredient(ItemType<Codex>(), 1)
            .AddIngredient(ItemID.Ectoplasm, 12)
            .AddIngredient(ItemType<DamnedSoul>(), 50)
            .AddTile(TileID.LihzahrdAltar)
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

