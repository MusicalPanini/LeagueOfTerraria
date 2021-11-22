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
    public class MouthoftheAbyss : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mouth of the Abyss");
            Tooltip.SetDefault("");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        string GetWeaponTooltip()
        {
            return "33% chance to not consume ammo";
        }

        public override void SetDefaults()
        {
            Item.damage = 26;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 44;
            Item.height = 52;
            Item.useAnimation = 8;
            Item.useTime = 8;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 1;
            Item.value = 120000;
            Item.rare = ItemRarityID.Lime;
            Item.UseSound = new Terraria.Audio.LegacySoundStyle(2, 11);
            Item.shoot = ProjectileID.PurificationPowder;
            Item.autoReuse = true;
            Item.shootSpeed = 13f;
            Item.useAmmo = AmmoID.Bullet;

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.W, new BioArcaneBarrage(this));
            abilityItem.ChampQuote = "Hunger never sleep";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            position.Y += 8;
            
            int numberProjectiles = Main.rand.Next(0,4) != 0 ? 0 : Main.rand.Next(1, 4);
            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed = new Vector2(velocity.X + Main.rand.NextFloat(-2, 2), velocity.Y + Main.rand.NextFloat(-1, 1)).RotatedByRandom(MathHelper.ToRadians(8));
                Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, ProjectileType<MouthoftheAbyss_AcidBlob>(), damage, knockback, player.whoAmI);
            }

            return true;
        }

        public override bool CanConsumeAmmo(Player player)
        {
            return Main.rand.Next(0, 100) < 33;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, 0);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<VoidBar>(), 14)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
    }
}
