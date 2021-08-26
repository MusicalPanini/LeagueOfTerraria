using Microsoft.Xna.Framework;
using TerraLeague.Items.Weapons.Abilities;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class Severum : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Severum");
            Tooltip.SetDefault("");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        string GetWeaponTooltip()
        {
            return "Uses 2% Severum Ammo" +
                "\nEach Lunari gun has its own special ammo that rechages when the gun is not in use." +
                "\n+2 melee life steal while attacking";
        }

        public override void SetDefaults()
        {
            Item.damage = 115;
            Item.DamageType = DamageClass.Melee;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.width = 32;
            Item.height = 32;
            Item.useAnimation = 14;
            Item.useTime = 14;
            Item.shootSpeed = 80;
            Item.knockBack = 2;
            Item.value = 310000 * 5;
            Item.rare = ItemRarityID.Purple;
            Item.shoot = ProjectileType<Severum_Slash>();
            Item.UseSound = null;
            Item.autoReuse = true;
            Item.noMelee = true;

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.Q, new Onslaught(this));
            abilityItem.SetAbility(AbilityType.W, new Phase(this, LunariGunType.Sev));
            abilityItem.ChampQuote = "Harvest death for life";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.GetModPlayer<PLAYERGLOBAL>().severumAmmo < 2)
            {
                if (Main.mouseLeftRelease)
                {
                    TerraLeague.PlaySoundWithPitch(player.MountedCenter, 12, 0, -0.5f);
                    CombatText.NewText(player.Hitbox, new Color(216, 0, 32), "NO AMMO");
                }
                return false;
            }
            return base.CanUseItem(player);
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
            return new Vector2(4, 6);
        }
    }
}
