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
    public class ElectricRifle : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Electric Rifle");
            Tooltip.SetDefault("");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        string GetWeaponTooltip()
        {
            return "";
        }

        public override void SetDefaults()
        {
            Item.damage = 8;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 76;
            Item.height = 46;
            Item.useAnimation = 21;
            Item.useTime = 3;
            Item.reuseDelay = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 0;
            Item.value = 60000;
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = new Terraria.Audio.LegacySoundStyle(2, 31);
            Item.shoot = ProjectileType<ElectricRifle_ElectricShot>();
            Item.autoReuse = true;
            Item.shootSpeed = 14f;
            Item.scale = 0.85f;
            Item.mana = 6;

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            //abilityItem.SetAbility(AbilityType.E, new Zap(this));
            abilityItem.ChampQuote = "lightning go brrrrr";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 20f;
            muzzleOffset += new Vector2(0, -5);
            position += muzzleOffset;
            
            velocity *= Main.rand.NextFloat(0.9f, 1.1f);
        }

        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(8));
            velocity = perturbedSpeed;

            Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);

            return false;
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage, ref float flat)
        {
            base.ModifyWeaponDamage(player, ref damage, ref flat);
        }

        public override float UseSpeedMultiplier(Player player)
        {
            {
                return base.UseSpeedMultiplier(player);
            }
        }

        public override bool CanConsumeAmmo(Player player)
        {
            return !(player.itemAnimation < (player.itemAnimationMax) - 2);
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, 0);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.IllegalGunParts, 1)
            .AddIngredient(ItemID.Minishark, 1)
            .AddIngredient(ItemID.ClockworkAssaultRifle, 1)
            .AddIngredient(ItemID.SoulofSight, 10)
            .AddIngredient(ItemID.PinkPaint, 3)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
    }
}
