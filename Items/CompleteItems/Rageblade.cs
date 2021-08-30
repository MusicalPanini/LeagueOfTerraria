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
    public class Rageblade : MasterworkItem
    {
        public override string MasterworkName => "Guinsoo's Reckoning";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Guinsoo's Rageblade");
            Tooltip.SetDefault("5% increased damage" +
                "\n6% increased melee and ranged attack speed");
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
                new GuinsoosRage(2, this),
                new Afterburn(15, 5, 5, 5, this)
            };
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Generic) += 0.05f;
            player.meleeSpeed += 0.06f;
            player.GetModPlayer<PLAYERGLOBAL>().rangedAttackSpeed += 0.06;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<AmpTome>(), 1)
            .AddIngredient(ItemType<RecurveBow>(), 1)
            .AddIngredient(ItemType<Pickaxe>(), 1)
            .AddIngredient(ItemID.FieryGreatsword, 1)
            .AddIngredient(ItemID.SoulofMight, 10)
            .AddTile(TileID.MythrilAnvil)
            .Register();
            
        }

        public override string GetStatText()
        {
            if (Passives[0].currentlyActive)
                return ((int)Passives[0].passiveStat).ToString();
            else
                return "";
        }

        public override string MasterworkTooltip()
        {
            return LeagueTooltip.CreateColorString(MasterColor, "8%") + " increased damage" +
                "\n" + LeagueTooltip.CreateColorString(MasterColor, "15%") + " increased melee and ranged attack speed";
        }

        public override void UpdateMasterwork(Player player)
        {
            player.GetDamage(DamageClass.Generic) += 0.03f;
            player.meleeSpeed += 0.09f;
            player.GetModPlayer<PLAYERGLOBAL>().rangedAttackSpeed += 0.09;
        }
    }
}
