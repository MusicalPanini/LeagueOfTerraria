using TerraLeague.Items.AdvItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Actives;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class Glp : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hextech GLP-800");
            Tooltip.SetDefault("7% increased magic and summon damage" +
                "\nIncreases maximum mana by 40" +
                "\nIncreases ability haste by 10");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 30, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.accessory = true;

            Active = new FrostBolt(45, 20, 30);
            Passives = new Passive[]
            {
                new Haste()
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statManaMax2 += 40;
            player.GetDamage(DamageClass.Magic) += 0.07f;
            player.GetDamage(DamageClass.Summon) += 0.07f;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 10;
            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<LostChapter>(), 1)
            .AddIngredient(ItemType<HextechAlternator>(), 1)
            .AddIngredient(ItemID.FrostStaff, 1)
            .AddIngredient(ItemType<HextechCore>(), 1)
            .AddIngredient(ItemType<TrueIceChunk>(), 2)
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
