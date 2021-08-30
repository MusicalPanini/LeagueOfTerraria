using TerraLeague.Items.CustomItems.Passives;
using TerraLeague.Items.AdvItems;
using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class BloodThirster : MasterworkItem
    {
        public override string MasterworkName => "Blood Devourer";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bloodthirster");
            Tooltip.SetDefault("6% increased ranged damage" +
                "\n+1 ranged life steal"/* +
                "\n30% decreased maximum life" +
                "\n30% increased damage taken"*/);
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
                new BloodShield(this)
            };
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Ranged) += 0.06f;
            player.GetModPlayer<PLAYERGLOBAL>().lifeStealRange += 1;// 0.06;
            //player.GetModPlayer<PLAYERGLOBAL>().healthModifier -= 0.3;
            //player.GetModPlayer<PLAYERGLOBAL>().damageTakenModifier += 0.3;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<BFSword>(), 1)
            .AddIngredient(ItemType<VampiricScepter>(), 1)
            .AddIngredient(ItemType<LongSword>(), 1)
            .AddIngredient(ItemID.SoulofFright, 10)
            .AddIngredient(ItemID.Ruby, 3)
            .AddIngredient(ItemID.Bone, 50)
            .AddTile(TileID.MythrilAnvil)
            .Register();
            
        }

        public override string MasterworkTooltip()
        {
            return LeagueTooltip.CreateColorString(MasterColor, "8%") + " increased ranged damage" +
                "\n+" + LeagueTooltip.CreateColorString(MasterColor, "2") + " ranged life steal";
        }

        public override void UpdateMasterwork(Player player)
        {
            player.GetDamage(DamageClass.Ranged) += 0.02f;
            player.GetModPlayer<PLAYERGLOBAL>().lifeStealRange += 1;
        }
    }
}
