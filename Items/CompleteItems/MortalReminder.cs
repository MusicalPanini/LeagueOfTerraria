using TerraLeague.Items.AdvItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class MortalReminder : MasterworkItem
    {
        public override string MasterworkName => "King Slayer";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mortal Reminder");
            Tooltip.SetDefault("5% increased ranged damage");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 45, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.accessory = true;

            Passives = new Passive[]
            {
                new CustomItems.Passives.LastWhisper(20, false),
                new Executioner(3)
            };
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Ranged) += 0.05f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<Executioners>(), 1)
            .AddIngredient(ItemType<AdvItems.LastWhisper>(), 1)
            .AddIngredient(ItemID.BlackLens, 1)
            .AddIngredient(ItemType<DarksteelBar>(), 8)
            .AddIngredient(ItemID.SoulofSight, 5)
            .AddTile(TileID.MythrilAnvil)
            .Register();
            
        }

        public override string MasterworkTooltip()
        {
            return LeagueTooltip.CreateColorString(MasterColor, "10%") + " increased ranged damage";
        }

        public override void UpdateMasterwork(Player player)
        {
            player.GetDamage(DamageClass.Ranged) += 0.05f;
        }
    }
}
