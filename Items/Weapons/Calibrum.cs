using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using TerraLeague.Items.Weapons.Abilities;
using TerraLeague.NPCs;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class Calibrum : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Calibrum");
            Tooltip.SetDefault("");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        string GetWeaponTooltip()
        {
            return "Uses 5% Calibrum Ammo" +
                "\nEach Lunari gun has its own special ammo that rechages when the gun is not in use." +
                "\nDeals more damage to far away enemies";
        }

        public override void SetDefaults()
        {
            Item.damage = 200;
            Item.DamageType = DamageClass.Ranged;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.width = 100;
            Item.height = 26;
            Item.useAnimation = 35;
            Item.useTime = 35;
            Item.shootSpeed = 2f;
            Item.noMelee = true;
            Item.knockBack = 1;
            Item.value = 310000 * 5;
            Item.rare = ItemRarityID.Purple;
            //Item.scale = 0.8f;
            Item.shoot = ProjectileType<Calibrum_Shot>();
            Item.UseSound = new Terraria.Audio.LegacySoundStyle(2, 75);
            Item.autoReuse = true;

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.Q, new Moonshot(this));
            abilityItem.SetAbility(AbilityType.W, new Phase(this, LunariGunType.Cal));
            abilityItem.ChampQuote = "Moonlight will guide your aim";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.GetModPlayer<PLAYERGLOBAL>().calibrumAmmo < 5)
            {
                if (Main.mouseLeftRelease)
                {
                    TerraLeague.PlaySoundWithPitch(player.MountedCenter, 12, 0, -0.5f);
                    CombatText.NewText(player.Hitbox, new Color(141, 252, 245), "NO AMMO");
                }
                return false;
            }
            return base.CanUseItem(player);
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            player.GetModPlayer<PLAYERGLOBAL>().calibrumAmmo -= 5;

            Vector2 muzzleOffset = Vector2.Normalize(velocity) * 46f;
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
            return new Vector2(-20, 0);
        }
    }
}
