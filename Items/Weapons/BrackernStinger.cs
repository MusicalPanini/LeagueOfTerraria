using Microsoft.Xna.Framework;
using TerraLeague.Items.Weapons.Abilities;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class BrackernStinger : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Brackern Stinger");
            Tooltip.SetDefault("");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 9;
            Item.width = 32;
            Item.height = 32;
            Item.DamageType = DamageClass.Melee;
            Item.useTime = 45;
            Item.useAnimation = 45;
            Item.useStyle = ItemUseStyleID.Thrust;
            Item.knockBack = 2;
            Item.value = 10000;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = new LegacySoundStyle(2, 101);
            Item.shootSpeed = 1f;
            Item.shoot = ProjectileType<BrackernStinger_Whip>();
            Item.noMelee = true;
            Item.noUseGraphic = true;

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.W, new CrystallineExoskeleton(this));
            abilityItem.ChampQuote = "Feel my sting!";
            abilityItem.IsAbilityItem = true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.ownedProjectileCounts[ProjectileType<BrackernStinger_Whip>()] < 1)
                return base.CanUseItem(player);
            return false;
        }

        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 0,
                Main.rand.Next(-100, 100) * 0.001f * player.gravDir);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.AntlionMandible, 6)
                .AddRecipeGroup("TerraLeague:GoldGroup", 10)
                .AddIngredient(ItemID.Amethyst, 1)
                .AddIngredient(ItemType<Sunstone>(), 10)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
