using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using TerraLeague.Items.Weapons.Abilities;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class HextechWrench : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hextech Wrench");
            Tooltip.SetDefault("");
            base.SetStaticDefaults();
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        string GetWeaponTooltip()
        {
            return "Create a H-28G Evolution Turret to fight for you!";
        }

        public override void SetDefaults()
        {
            Item.damage = 8;
            Item.sentry = true;
            Item.DamageType = DamageClass.Summon;
            Item.mana = 20;
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 36;
            Item.useAnimation = 36;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 1;
            Item.value = 1000;
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = new LegacySoundStyle(2, 113);
            Item.shoot = ProjectileType<HextechWrench_EvolutionTurret>();

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.W, new MicroRockets(this));
            abilityItem.SetAbility(AbilityType.E, new StormGrenade(this));
            abilityItem.ChampQuote = "Stand back! I am about to do...science!";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.FindSentryRestingSpot(Item.shoot, out int xPos, out int yPos, out int yDis);
            Projectile proj = Projectile.NewProjectileDirect(source, new Vector2((float)xPos, (float)(yPos - yDis) + 3), Vector2.Zero, type, damage, knockback, player.whoAmI, 10, -1);
            proj.originalDamage = Item.damage;
            player.UpdateMaxTurrets();

            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<StartingItems.WeaponKit>(), 1)
            .Register();
        }
    }
}