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
    public class VoidProphetsStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Void Prophets Staff");
            Tooltip.SetDefault("");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        string GetWeaponTooltip()
        {
            return "Summon a gate way from the void";
        }

        public override void SetDefaults()
        {
            Item.damage = 20;
            Item.sentry = true;
            Item.DamageType = DamageClass.Summon;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.width = 48;
            Item.height = 48;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.mana = 10;
            Item.noMelee = true;
            Item.knockBack = 1;
            Item.value = 35000;
            Item.rare = ItemRarityID.Lime;
            Item.scale = 1f;
            Item.shoot = ProjectileType<VoidProphetsStaff_ZzrotPortal>();
            Item.UseSound = new Terraria.Audio.LegacySoundStyle(2, 113);
            Item.autoReuse = false;

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.E, new MaleficVisions(this));
            abilityItem.ChampQuote = "Bow to the void! Or be consumed by it!";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.FindSentryRestingSpot(Item.shoot, out int xPos, out int yPos, out int yDis);
            Projectile proj = Projectile.NewProjectileDirect(source, new Vector2((float)xPos, (float)(yPos - yDis)), Vector2.Zero, type, damage, knockback, player.whoAmI, 10, -1);
            proj.originalDamage = Item.damage;
            player.UpdateMaxTurrets();

            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<VoidBar>(), 14)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}
