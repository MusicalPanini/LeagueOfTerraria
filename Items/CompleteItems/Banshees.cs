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
    public class Banshees : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Banshee's Veil");
            Tooltip.SetDefault("6% increased magic and summon damage" +
                "\nIncreases resist by 4" +
                "\nIncreases ability haste by 10" +
                "\nImmunity to Silence and Curse");
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
                new MagicVeil(40)
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Magic) += 0.06f;
            player.GetDamage(DamageClass.Summon) += 0.06f;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 10;
            player.GetModPlayer<PLAYERGLOBAL>().resist += 4;
            
            player.buffImmune[BuffID.Silenced] = true;
            player.buffImmune[BuffID.Cursed] = true;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<Codex>(), 1)
            .AddIngredient(ItemType<NullMagic>(), 1)
            .AddIngredient(ItemType<BlastingWand>(), 1)
            .AddIngredient(ItemID.CountercurseMantra, 1)
            .AddIngredient(ItemType<SilversteelBar>(), 12)
            .AddIngredient(ItemID.SoulofSight, 8)
            .AddTile(TileID.MythrilAnvil)
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
