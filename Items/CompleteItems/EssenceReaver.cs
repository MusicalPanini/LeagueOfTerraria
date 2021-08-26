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
    public class EssenceReaver : LeagueItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Essence Reaver");
            Tooltip.SetDefault("4% increased melee and ranged damage" +
                "\n6% increased melee and ranged critical strike chance" +
                "\nIncreases ability haste by 20");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 25, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.accessory = true;

            Passives = new Passive[]
            {
                new SoulReave(7)
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetCritChance(DamageClass.Melee) += 6;
            player.GetCritChance(DamageClass.Ranged) += 6;
            player.GetDamage(DamageClass.Melee) += 0.04f;
            player.GetDamage(DamageClass.Ranged) += 0.04f;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 20;
            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<Pickaxe>(), 1)
            .AddIngredient(ItemType<Warhammer>(), 1)
            .AddIngredient(ItemType<CloakofAgility>(), 1)
            .AddIngredient(ItemID.Sickle, 1)
            .AddIngredient(ItemType<DamnedSoul>(), 20)
            .AddIngredient(ItemID.Bone, 10)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}
