using Microsoft.Xna.Framework;
using TerraLeague.Items.Weapons.Abilities;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class LightPistol : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Light Pistol");
            Tooltip.SetDefault("");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 12;
            Item.DamageType = DamageClass.Ranged;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.width = 52;
            Item.height = 26;
            Item.useAnimation = 16;
            Item.reuseDelay = 20;
            Item.useTime = 8;
            Item.shootSpeed = 8f;
            Item.noMelee = true;
            Item.knockBack = 1;
            Item.value = 6000;
            Item.rare = ItemRarityID.Orange;
            Item.scale = 0.9f;
            Item.shoot = ProjectileType<LightPistol_Bullet>();
            Item.UseSound = new Terraria.Audio.LegacySoundStyle(2, 12);
            Item.autoReuse = true;

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.Q, new PiercingLight(this));
            abilityItem.ChampQuote = "Everybody deserves a second shot";
            abilityItem.IsAbilityItem = true;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(velocity) * 46f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<SilversteelBar>(), 16)
            .AddTile(TileID.Anvils)
            .Register();
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, 0);
        }
    }
}
