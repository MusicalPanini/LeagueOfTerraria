using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
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
	public class CelestialStaff : ModItem
	{
        static readonly float baseRejuvChance = 0.1f;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Celestial Staff");
            Tooltip.SetDefault("");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 26;
            Item.mana = 18;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.DamageType = DamageClass.Magic;
            Item.useTime = 35;
            Item.useAnimation = 35;
            Item.noMelee = true;
            Item.knockBack = 0;
            Item.value = 4000;
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item8;
            Item.shoot = ProjectileType<CelestialStaff_Starcall>();
            Item.shootSpeed = 12f;
            Item.autoReuse = true;

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.W, new AstralInfusion(this));
            abilityItem.SetAbility(AbilityType.R, new Wish(this));
            abilityItem.ChampQuote = "Stars, hear me";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        string GetWeaponTooltip()
        {
            return "Call down stars that have a chance to drop Rejuvenation Hearts on hit" +
                "\nDrop Chance: " +
                LeagueTooltip.TooltipValue((int)(baseRejuvChance * 100), true, "%",
                new System.Tuple<int, ScaleType>(15, ScaleType.Magic)
                );
        }

        public static float RejuvDropChance(Player player)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            return modPlayer.ScaleValueWithHealPower((baseRejuvChance + (modPlayer.MAG * 0.15f * 0.01f)) * 100, true) * 0.01f; //(100 - baseRejuvChance) / (100 + modPlayer.ScaleValueWithHealPower(modPlayer.MAG, true));
        }

        public override bool CanUseItem(Player player)
        {
            return true;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            base.ModifyTooltips(tooltips);
        }

        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            position = player.Center;
            position.X += Main.rand.NextFloat(-300, 300);
            position.Y -= 600;
            velocity = TerraLeague.CalcVelocityToMouse(position, 14f);
            Item.damage = 26;
            Projectile.NewProjectile(source, position, velocity, type, damage, 0, player.whoAmI);
            return false;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-9, 0);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<CelestialBar>(), 10)
            .AddIngredient(ItemID.PurificationPowder, 5)
            .AddIngredient(ItemID.FallenStar, 5)
            .AddIngredient(ItemID.Topaz, 1)
            .Register();
        }
    }
}
