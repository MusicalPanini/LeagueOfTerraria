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
    public class LightCannon : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Light Cannon");
            Tooltip.SetDefault("");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        string GetWeaponTooltip()
        {
            return "Deals an additional " + LeagueTooltip.TooltipValue(0, false, "",
              new System.Tuple<int, ScaleType>(100, ScaleType.Ranged)
              ) + " damage";
        }

        public override void SetDefaults()
        {
            Item.damage = 100;
            Item.DamageType = DamageClass.Ranged;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.width = 108;
            Item.height = 28;
            Item.channel = true;
            Item.useAnimation = 60;
            Item.useTime = 60;
            Item.shootSpeed = 10f;
            Item.noMelee = true;
            Item.knockBack = 7;
            Item.value = 160000;
            Item.rare = ItemRarityID.Pink;
            Item.shoot = ProjectileType<LightCannon_BeamControl>();
            Item.UseSound = new Terraria.Audio.LegacySoundStyle(2, 13);

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.Q, new PiercingDarkness(this));
            abilityItem.ChampQuote = "I don't carry this to compromise";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile proj = Projectile.NewProjectileDirect(source, player.Center, Vector2.Zero, type, damage + player.GetModPlayer<PLAYERGLOBAL>().RNG, knockback, player.whoAmI);
            proj.rotation = velocity.ToRotation();

            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<DamnedSoul>(), 100)
            .AddIngredient(ItemID.HallowedBar, 16)
            .AddIngredient(ItemID.Marble, 100)
            .AddIngredient(ItemID.IllegalGunParts, 1)
            .AddIngredient(ItemID.SoulofMight, 10)
            .AddIngredient(ItemID.SoulofLight, 5)
            .AddIngredient(ItemID.SoulofNight, 5)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-16, 4);
        }
    }
}
