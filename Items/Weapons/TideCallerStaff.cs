using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using TerraLeague.Items.Weapons.Abilities;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
	public class TideCallerStaff : ModItem
	{
        int healing = 8;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tidecaller Staff");
            Tooltip.SetDefault("");
            Item.staff[Item.type] = false;
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            base.ModifyTooltips(tooltips);

            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();

            int tt2 = tooltips.FindIndex(x => x.Name == "Damage" && x.mod == "Terraria");
            if (tt2 != -1)
            {
                tooltips.Insert(tt2 + 1, new TooltipLine(TerraLeague.instance, "Healing", LeagueTooltip.TooltipValue((int)(healing * (float)modPlayer.magicDamageLastStep), true, "") + " magic healing"));
            }
        }

        string GetWeaponTooltip()
        {
            return "Hititng a stunned or bubbled enemy will heal a nearby ally" +
                "\nAfter healing, it will then strike another enemy";
        }

        public override void SetDefaults()
        {
            Item.damage = 26;
            Item.mana = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.DamageType = DamageClass.Magic;
            Item.useTime = 36;
            Item.useAnimation = 36;
            Item.noMelee = true;
            Item.knockBack = 3;
            Item.value = 4000;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = new Terraria.Audio.LegacySoundStyle(2, 21, Terraria.Audio.SoundType.Sound);
            Item.shoot = ProjectileType<TideCallerStaff_EbbandFlow>();
            //Item.shoot = ProjectileType<TideCallerStaff_WaterShot>();
            Item.shootSpeed = 11f;
            healing = 8;

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.Q, new AquaPrison(this));
            abilityItem.SetAbility(AbilityType.E, new TidecallersBlessing(this));
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.ChampQuote = "I decide what the tide will bring";
            abilityItem.IsAbilityItem = true;

            base.SetDefaults();
        }

        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            int scaledHealing = modPlayer.ScaleValueWithHealPower(healing * player.GetDamage(DamageClass.Magic));

            Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI, damage, scaledHealing);
            return false;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-9, 0);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddRecipeGroup("TerraLeague:GoldGroup", 10)
            .AddIngredient(ItemID.Seashell, 5)
            .AddIngredient(ItemID.FallenStar, 5)
            .AddIngredient(ItemID.Sapphire, 1)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}
