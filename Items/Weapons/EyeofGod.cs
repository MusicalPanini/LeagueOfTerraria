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
    public class EyeofGod : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Eye of God");
            Tooltip.SetDefault("");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        string GetWeaponTooltip()
        {
            return "Summon a tentacle of Nagakabouros to fight for you";
        }

        public override void SetDefaults()
        {
            Item.damage = 15;
            Item.sentry = true;
            Item.DamageType = DamageClass.Summon;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.width = 32;
            Item.height = 32;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.mana = 10;
            Item.noMelee = true;
            Item.knockBack = 1;
            Item.value = 3500;
            Item.rare = ItemRarityID.Green;
            Item.scale = 1f;
            Item.shoot = ProjectileType<EyeofGod_Tentacle>();
            Item.UseSound = new Terraria.Audio.LegacySoundStyle(2, 8);
            Item.autoReuse = false;

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.E, new TestOfSpirit(this));
            abilityItem.ChampQuote = "There are kind and gentle gods. Mine isn't one of those";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.FindSentryRestingSpot(Item.shoot, out int xPos, out int yPos, out int yDis);
            Projectile proj = Projectile.NewProjectileDirect(source, new Vector2((float)xPos, (float)(yPos - yDis) - 24), Vector2.Zero, type, damage, knockback, player.whoAmI, 10, -1);
            proj.originalDamage = Item.damage;
            player.UpdateMaxTurrets();

            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<BrassBar>(), 14)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}
