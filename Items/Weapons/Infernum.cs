using Microsoft.Xna.Framework;
using TerraLeague.Items.Weapons.Abilities;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class Infernum : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Infernum");
            Tooltip.SetDefault("");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        string GetWeaponTooltip()
        {
            return "Uses 5% Infernum Ammo" +
                "\nEach Lunari gun has its own special ammo that rechages when the gun is not in use." +
                "\nYour attacks will create a splash of flame";
        }

        public override void SetDefaults()
        {
            Item.damage = 100;
            Item.DamageType = DamageClass.Ranged;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.width = 76;
            Item.height = 28;
            Item.useAnimation = 26;
            Item.useTime = 26;
            Item.shootSpeed = 12f;
            Item.noMelee = true;
            Item.knockBack = 2;
            Item.value = 310000 * 5;
            Item.rare = ItemRarityID.Purple;
            Item.shoot = ProjectileType<Infernum_Flame>();
            Item.UseSound = new Terraria.Audio.LegacySoundStyle(2, 45);
            Item.autoReuse = true;

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.Q, new Duskwave(this));
            abilityItem.SetAbility(AbilityType.W, new Phase(this, LunariGunType.Inf));
            abilityItem.ChampQuote = "Cosmic flame will fill the night";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.GetModPlayer<PLAYERGLOBAL>().infernumAmmo < 5)
            {
                if (Main.mouseLeftRelease)
                {
                    CombatText.NewText(player.Hitbox, new Color(0, 148, 255), "NO AMMO");
                    TerraLeague.PlaySoundWithPitch(player.MountedCenter, 12, 0, -0.5f);
                }
                return false;
            }
            return base.CanUseItem(player);
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            player.GetModPlayer<PLAYERGLOBAL>().infernumAmmo -= 5;

            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y + 3)) * 25f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.LunarBar, 16)
            .AddTile(TileID.Anvils)
            .Register();
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-25, 10);
        }
    }
}
