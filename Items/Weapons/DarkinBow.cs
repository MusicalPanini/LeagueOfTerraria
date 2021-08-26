using Microsoft.Xna.Framework;
using TerraLeague.Items.Weapons.Abilities;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class DarkinBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Darkin Bow");
            Tooltip.SetDefault("");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        string GetWeaponTooltip()
        {
            return "Charge an arrow that gains damage and velocity the longer you charge" +
                "\nCharges faster with ranged attack speed";
        }


        public override void SetDefaults()
        {
            Item.damage = 34;
            Item.DamageType = DamageClass.Ranged;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.width = 24;
            Item.height = 64;
            Item.channel = true;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.shootSpeed = 10f;
            Item.noMelee = true;
            Item.knockBack = 1;
            Item.value = 100000;
            Item.rare = ItemRarityID.LightRed;
            Item.shoot = ProjectileType<DarkinBow_ArrowControl>();
            Item.useAmmo = AmmoID.Arrow;

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.E, new RainOfArrows(this));
            abilityItem.ChampQuote = "The guilty will know agony";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (type == ProjectileID.WoodenArrowFriendly)
                type = ProjectileType<DarkinBow_DarkinArrow>();

            Projectile proj = Projectile.NewProjectileDirect(source, player.Center, Vector2.Zero, ProjectileType<DarkinBow_ArrowControl>(), damage, knockback, player.whoAmI, type);
            proj.rotation = velocity.ToRotation();

            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.ShadowFlameBow, 1)
            .AddRecipeGroup("TerraLeague:DemonGroup", 20)
            .AddIngredient(ItemID.SoulofNight, 10)
            .AddTile(TileID.DemonAltar)
            .Register();
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, 0);
        }
    }
}
