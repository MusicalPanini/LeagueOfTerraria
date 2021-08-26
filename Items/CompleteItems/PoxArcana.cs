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
    public class PoxArcana : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pox Arcana");
            Tooltip.SetDefault("4% increased magic and summon damage" +
                "\nIncreases mana regeneration by 60%" +
                "\nIncreases ability haste by 10");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 25, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.accessory = true;

            Active = new DiseaseHarvest(12, 5, 15, 60);
            Passives = new Passive[]
            {
                new Pox()
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Magic) += 0.04f;
            player.GetDamage(DamageClass.Summon) += 0.04f;
            player.GetModPlayer<PLAYERGLOBAL>().manaRegenModifer += 0.6;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 10;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<FaerieCharm>(), 2)
            .AddIngredient(ItemType<Codex>(), 1)
            .AddIngredient(ItemType<AmpTome>(), 1)
            .AddIngredient(ItemType<DamnedSoul>(), 20)
            .AddIngredient(ItemID.Stinger, 5)
            .AddIngredient(ItemID.Bone, 10)
            .AddTile(TileID.Bookcases)
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
