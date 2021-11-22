using Microsoft.Xna.Framework;
using TerraLeague.Items.Weapons.Abilities;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class PowPow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pow Pow");
            Tooltip.SetDefault("");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        string GetWeaponTooltip()
        {
            return "Kills grant 'EXCITED!'" +
                "\nEXCITED increases firerate and damage";
        }

        public override void SetDefaults()
        {
            Item.damage = 15;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 76;
            Item.height = 46;
            Item.useAnimation = 12;
            Item.useTime = 4;
            Item.reuseDelay = 14;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 0;
            Item.value = 60000;
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = new Terraria.Audio.LegacySoundStyle(2, 31);
            Item.shoot = ProjectileID.PurificationPowder;
            Item.autoReuse = true;
            Item.shootSpeed = 13f;
            Item.useAmmo = AmmoID.Bullet;

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.E, new Zap(this));
            abilityItem.ChampQuote = "SAY HELLO TO MY FRIENDS OF VARYING SIZES!";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y + 6)) * 25f;
            
            position += muzzleOffset;

            Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(3));

            velocity = perturbedSpeed;
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage, ref float flat)
        {
            if (player.GetModPlayer<PLAYERGLOBAL>().excited)
                damage *= 2;

            base.ModifyWeaponDamage(player, ref damage, ref flat);
        }

        public override float UseSpeedMultiplier(Player player)
        {
            if (player.GetModPlayer<PLAYERGLOBAL>().excited)
            {
                return base.UseSpeedMultiplier(player) * 1.3f;
            }
            else
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
            return new Vector2(-20, 8);
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
