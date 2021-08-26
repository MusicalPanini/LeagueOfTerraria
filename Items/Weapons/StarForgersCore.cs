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
    public class StarForgersCore : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Star Forger");
            Tooltip.SetDefault("");
            base.SetStaticDefaults();
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        string GetWeaponTooltip()
        {
            return "Become the center of the Universe!";
        }

        public override void SetDefaults()
        {
            Item.damage = 60;
            Item.DamageType = DamageClass.Summon;
            Item.mana = 20;
            Item.width = 40;
            Item.height = 42;
            Item.useTime = 36;
            Item.useAnimation = 36;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 1;
            Item.value = 200000;
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = new LegacySoundStyle(2, 113);
            Item.shoot = ProjectileType<StarForgersCore_ForgedStar>();

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.W, new CelestialExpansion(this));
            abilityItem.ChampQuote = "Now we're playing with star fire!";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.AddBuff(BuffType<Buffs.CenterOfTheUniverse>(), 3);
            position = player.MountedCenter;
            Projectile proj = Projectile.NewProjectileDirect(source, position, Vector2.Zero, type, damage, knockback, player.whoAmI, player.ownedProjectileCounts[type] + 1);
            proj.originalDamage = Item.damage;

            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<CelestialBar>(), 20)
            .AddIngredient(ItemType<FragmentOfTheAspect>(), 1)
            .AddIngredient(ItemID.FallenStar, 20)
            .AddIngredient(ItemID.SoulofLight, 10)
            .AddIngredient(ItemID.SoulofNight, 10)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
    }
}

