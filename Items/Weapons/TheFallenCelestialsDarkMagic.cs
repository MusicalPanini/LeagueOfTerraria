using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.Collections.Generic;
using System.Linq;
using TerraLeague.NPCs;
using TerraLeague.Projectiles;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Audio;
using Terraria.Audio;
using TerraLeague.Items.Weapons.Abilities;
using Terraria.DataStructures;

namespace TerraLeague.Items.Weapons
{
    public class TheFallenCelestialsDarkMagic : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Fallen Celestials Dark Magic");
            Tooltip.SetDefault("");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        string GetWeaponTooltip()
        {
            return "Deals increased damage the lower the enemies life";
        }

        public override void SetDefaults()
        {
            Item.damage = 24;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Magic;
            Item.useTime = 80;
            Item.useAnimation = 80;
            Item.mana = 40;
            Item.rare = ItemRarityID.Yellow;
            Item.value = 300000;
            Item.width = 28;
            Item.height = 32;
            Item.knockBack = 0;
            Item.UseSound = SoundID.Item20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shoot = ProjectileType<TheFallenCelestialsDarkMagic_TormentedShadow>();

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.R, new SoulShackles(this));
            abilityItem.ChampQuote = "I am bound, but I will not break";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectileDirect(source, Main.MouseWorld, Vector2.Zero, type, damage, knockback, player.whoAmI);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.SpellTome, 1)
            .AddIngredient(ItemID.SoulofNight, 20)
            .AddIngredient(ItemID.Chain, 10)
            .AddIngredient(ItemType<FragmentOfTheAspect>(), 1)
            .AddIngredient(ItemType<CelestialBar>(), 20)
            .AddTile(TileID.Bookcases)
            .Register();
            
        }
    }
}
