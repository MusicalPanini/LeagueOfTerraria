using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.AdvItems
{
    public class HextechAlternator : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hextech Alternator");
            Tooltip.SetDefault("4% increased magic and summon damage");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 52;
            Item.height = 20;
            Item.value = Item.buyPrice(0, 15, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.accessory = true;
            Item.material = true;

            Passives = new Passive[]
            {
                new MagicBolt(20, 20, 30, this)
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Magic) += 0.04f;
            player.GetDamage(DamageClass.Summon) += 0.04f;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<AmpTome>(), 2)
            //.AddIngredient(ItemID.Revolver, 1);
            //.AddIngredient(ItemID.SpaceGun, 1);
            .AddIngredient(ItemID.MeteoriteBar, 10)
            .AddIngredient(ItemID.Wire, 10)
            .AddIngredient(ItemID.FallenStar, 4)
            .AddIngredient(ItemType<PrototypeHexCore>(), 1)
            .AddTile(TileID.Anvils)
            .Register();
            
        }

        public override string GetStatText()
        {
            if (Passives[0].currentlyActive)
            {
                if (Passives[0].cooldownCount > 0)
                    return (Passives[0].cooldownCount / 60).ToString();
            }
            return "";
        }

        public override bool OnCooldown(Player player)
        {
            if (Passives[0].cooldownCount > 0 || !Passives[0].currentlyActive)
                return true;
            else
                return false;
        }
    }
}
