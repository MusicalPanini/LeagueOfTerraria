using Microsoft.Xna.Framework;
using TerraLeague.Items.Weapons.Abilities;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class TrueIceFlail : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("True Ice Flail");
            Tooltip.SetDefault("");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        string GetWeaponTooltip()
        {
            return "Chance to slow enemies on hit." +
                "\nIf enemy is already slow, freeze them instead." +
                "\nMelee attacks shatter frozen enemies, dealing 10% of their max life as extra damage (Max 200)" +
                "\nShattered enemies cannot be frozen again for 5 seconds";
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = 160000;
            Item.rare = ItemRarityID.Pink;
            Item.noMelee = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 40;
            Item.useTime = 40;
            Item.knockBack = 6F;
            Item.damage = 54;
            Item.scale = 1;
            Item.noUseGraphic = true;
            Item.UseSound = SoundID.Item1;
            Item.shootSpeed = 13F;
            Item.DamageType = DamageClass.Melee;
            Item.channel = true;
            Item.shoot = ProjectileType<TrueIceFlail_Ball>();

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.R, new GlacialPrison(this));
            abilityItem.ChampQuote = "The cold does not forgive";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<TrueIceChunk>(), 4)
            .AddIngredient(ItemID.Sunfury, 1)
            .AddIngredient(ItemID.FrostCore, 1)
            .AddTile(TileID.Anvils)
            .Register();
            
        }
    }
}
