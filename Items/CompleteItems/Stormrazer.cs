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
    public class Stormrazer : LeagueItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stormrazer");
            Tooltip.SetDefault("5% increased melee and ranged damage" +
                "\n8% increased melee and ranged attack speed");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 25, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.accessory = true;

            Passives = new Passive[]
            {
                new Energized(15, 25),
                new Storm()
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Melee) += 0.05f;
            player.GetDamage(DamageClass.Ranged) += 0.05f;
            player.meleeSpeed += 0.08f;
            player.GetModPlayer<PLAYERGLOBAL>().rangedAttackSpeed += 0.08;
            player.moveSpeed += 0.05f;
            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<BFSword>(), 1)
            .AddIngredient(ItemType<LongSword>(), 1)
            .AddIngredient(ItemType<KircheisShard>(), 1)
            .AddIngredient(ItemID.Muramasa, 1)
            .AddTile(TileID.Anvils)
            .Register();
            

            
        }

        public override string GetStatText()
        {
            if (Passives[0].currentlyActive)
                return ((int)Passives[0].passiveStat).ToString() + "%";
            else
                return "";
        }

        public override bool OnCooldown(Player player)
        {
            return !Passives[0].currentlyActive;
        }
    }
}
