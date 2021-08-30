using TerraLeague.Items.AdvItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class FrozenMallet : MasterworkItem
    {
        public override string MasterworkName => "True Ice Warhammer";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Frozen Mallet");
            Tooltip.SetDefault("6% increased melee and ranged damage" +
                "\nIncreases maximum life by 25");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 30, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.accessory = true;

            Passives = new Passive[]
            {
                new Icy(2, this)
            };
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Melee) += 0.06f;
            player.GetDamage(DamageClass.Ranged) += 0.06f;
            player.statLifeMax2 += 25;
            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<Jaurim>(), 1)
            .AddIngredient(ItemType<GiantsBelt>(), 1)
            .AddIngredient(ItemType<TrueIceChunk>(), 10)
            .AddIngredient(ItemID.Pwnhammer, 1)
            .AddTile(TileID.Anvils)
            .Register();
            
        }

        public override string MasterworkTooltip()
        {
            return LeagueTooltip.CreateColorString(MasterColor, "10%") + " increased melee and ranged damage" +
                "\nIncreases maximum life by " + LeagueTooltip.CreateColorString(MasterColor, "40");
        }

        public override void UpdateMasterwork(Player player)
        {
            player.GetDamage(DamageClass.Melee) += 0.04f;
            player.GetDamage(DamageClass.Ranged) += 0.04f;
            player.statLifeMax2 += 15;
        }
    }
}
