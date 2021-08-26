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
    public class AssassinsKunai : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Assassin's Kunai");
            Tooltip.SetDefault("");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 36;
            Item.width = 18;
            Item.height = 36;
            Item.DamageType = DamageClass.Magic;
            Item.useTime = 23;
            Item.useAnimation = 23;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 2;
            Item.mana = 8;
            Item.value = 350000;
            Item.rare = ItemRarityID.Lime;
            Item.UseSound = new LegacySoundStyle(2, 19, Terraria.Audio.SoundType.Sound);
            Item.shoot = ProjectileType<AssassinsKunai_Kunai>();
            Item.shootSpeed = 16f;
            Item.noMelee = true;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.noUseGraphic = true;

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.W, new TwilightShroud(this));
            abilityItem.ChampQuote = "Fear the assassin with no master";
            abilityItem.IsAbilityItem = true;
        }

        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int numberProjectiles = 5;
            float startingAngle = 20;
            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.ToRadians(startingAngle));
                Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, knockback, player.whoAmI);
                startingAngle -= 10f;
            }

            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemType<HarmonicBar>(), 16)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
