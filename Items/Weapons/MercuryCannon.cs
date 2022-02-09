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
    public class MercuryCannon : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mercury Cannon");
            Tooltip.SetDefault("");
            Item.staff[Item.type] = true;
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        string GetWeaponTooltip()
        {
            return "Firing through an Acceleration Gate will power up the Shock Blast";
        }

        public override void SetDefaults()
        {
            Item.damage = 26;
            Item.DamageType = DamageClass.Ranged;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.width = 66;
            Item.height = 38;
            Item.useAnimation = 34;
            Item.useTime = 34;
            Item.shootSpeed = 10f;
            Item.noMelee = true;
            Item.knockBack = 2;
            Item.value = 10000;
            Item.rare = ItemRarityID.Orange;
            Item.shoot = ProjectileType<MercuryCannon_Shot>();
            Item.UseSound = new Terraria.Audio.LegacySoundStyle(2, 109);
            Item.autoReuse = true;
            Item.mana = 8;

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.E, new AccelGate(this));
            abilityItem.SetAbility(AbilityType.R, new MercuryTransform(this, MercuryType.Hammer));
            abilityItem.ChampQuote = "Fully charged";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override bool CanUseItem(Player player)
        {
            return base.CanUseItem(player);
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 20;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            //Projectile.NewProjectileDirect(position, new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI, -1);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<PrototypeHexCore>(), 2)
            .AddIngredient(ItemID.IllegalGunParts, 1)
            .AddIngredient(ItemID.MeteoriteBar, 10)
            .AddIngredient(ItemID.HellstoneBar, 10)
            .AddTile(TileID.Anvils)
            .Register();
        }

        public override Vector2? HoldoutOrigin()
        {
            return new Vector2(16, 16);
        }

        public override void OnCraft(Recipe recipe)
        {
            MercuryHammer item = new MercuryHammer();
            item.Item.SetDefaults(ItemType<MercuryHammer>());
            item.Item.Prefix(-1);
            Item.GetGlobalItem<MercuryWeapon>().HammerPrefix = item.Item.prefix;

            base.OnCraft(recipe);
        }
    }
}
