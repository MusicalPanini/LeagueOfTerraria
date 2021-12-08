using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using TerraLeague.Items.Weapons.Abilities;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class Gravitum : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gravitum");
            Tooltip.SetDefault("");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        string GetWeaponTooltip()
        {
            return "Uses 5% Gravitum Ammo" +
                "\nEach Lunari gun has its own special ammo that rechages when the gun is not in use." +
                "\nYour attacks mark and slow your target";
        }

        public override void SetDefaults()
        {
            Item.damage = 100;
            Item.DamageType = DamageClass.Magic;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.width = 66;
            Item.height = 38;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.shootSpeed = 12f;
            Item.noMelee = true;
            Item.knockBack = 2;
            Item.value = 310000 * 5;
            Item.rare = ItemRarityID.Purple;
            Item.shoot = ProjectileType<Gravitum_Orb>();
            Item.UseSound = new Terraria.Audio.LegacySoundStyle(2, 111);
            Item.autoReuse = true;

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.Q, new BindingEclipse(this));
            abilityItem.SetAbility(AbilityType.W, new Phase(this, LunariGunType.Grv));
            abilityItem.ChampQuote = "Darkness will weigh upon them";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.GetModPlayer<PLAYERGLOBAL>().gravitumAmmo < 5)
            {
                if (Main.mouseLeftRelease)
                {
                    TerraLeague.PlaySoundWithPitch(player.MountedCenter, 12, 0, -0.5f);
                    CombatText.NewText(player.Hitbox, new Color(200, 37, 255), "NO AMMO");
                }
                return false;
            }
            return base.CanUseItem(player);
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            player.GetModPlayer<PLAYERGLOBAL>().gravitumAmmo -= 5;
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X - 20, velocity.Y - 20)) * 20;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            //Projectile.NewProjectileDirect(position, new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI, -1);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.LunarBar, 16)
            .AddTile(TileID.Anvils)
            .Register();
        }

        public override void OnCraft(Recipe recipe)
        {
            Item item = new Item();
            Item.SetDefaults(ItemType<Calibrum>());
            Item.Prefix(-1);
            Item.GetGlobalItem<LunariGun>().CalPrefix = Item.prefix;

            Item.SetDefaults(ItemType<Severum>());
            Item.Prefix(-1);
            Item.GetGlobalItem<LunariGun>().SevPrefix = Item.prefix;

            Item.SetDefaults(ItemType<Infernum>());
            Item.Prefix(-1);
            Item.GetGlobalItem<LunariGun>().InfPrefix = Item.prefix;

            Item.SetDefaults(ItemType<Crescendum>());
            Item.Prefix(-1);
            Item.GetGlobalItem<LunariGun>().CrePrefix = Item.prefix;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-20, -10);
        }
    }
}
