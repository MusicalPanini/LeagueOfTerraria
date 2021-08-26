using System.Collections.Generic;
using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.AdvItems
{
    public class Seekers : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Seeker's Armguard");
            Tooltip.SetDefault("2% increased magic and summon damage" +
                "\nIncreases armor by 1");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 15, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.accessory = true;
            Item.material = true;

            Passives = new Passive[]
            {
                new Attunement(20, 0.2m, 0.2m)
            };
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            base.ModifyTooltips(tooltips);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Magic) += 0.02f;
            player.GetDamage(DamageClass.Summon) += 0.02f;
            player.GetModPlayer<PLAYERGLOBAL>().armor += 1;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<ClothArmor>(), 2)
            .AddIngredient(ItemType<AmpTome>(), 1)
            .AddIngredient(ItemType<TrueIceChunk>(), 2)
            .AddIngredient(ItemID.Obsidian, 20)
            .AddTile(TileID.Anvils)
            .Register();
            
        }

        public override string GetStatText()
        {
            if (Passives[0].currentlyActive)
                return ((int)Passives[0].passiveStat).ToString();
            else
                return "";
        }
    }
}
