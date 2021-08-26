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
    public class DarksteelThrowingAxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Darksteel Throwing Axe");
            Tooltip.SetDefault("");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 42;
            Item.width = 24;
            Item.height = 24;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 45;
            Item.useAnimation = 45;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 3.5f;
            Item.value = 6000;
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = new Terraria.Audio.LegacySoundStyle(2, 19, Terraria.Audio.SoundType.Sound);
            Item.shootSpeed = 15f;
            Item.shoot = ProjectileType<DarksteelThrowingAxe_ThrowingAxe>();
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.useTurn = true;
            Item.noUseGraphic = true;

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.Q, new SpinningAxe(this));
            abilityItem.ChampQuote = "Welcome to the League of Draaaaven";
            abilityItem.IsAbilityItem = true;
        }

        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            position.Y -= 5;

            if (player.GetModPlayer<PLAYERGLOBAL>().spinningAxe)
            {
                type = ProjectileType<DarksteelThrowingAxe_SpinningAxe>();
                AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
                if (abilityItem.GetAbility(AbilityType.Q) != null)
                {
                    damage += abilityItem.GetAbility(AbilityType.Q).GetAbilityBaseDamage(player);
                    damage += abilityItem.GetAbility(AbilityType.Q).GetAbilityScaledDamage(player, DamageType.RNG);
                }
                Projectile.NewProjectileDirect(source, position, velocity * 1.2f, type, damage, knockback + 1.5f, player.whoAmI, 1, player.velocity.X);
                player.ClearBuff(BuffType<Buffs.SpinningAxe>());
                return false;
            }
            else
            {
                return true;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<DarksteelBar>(), 14)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}
