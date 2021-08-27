using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using TerraLeague.Items.Weapons.Abilities;
using TerraLeague.NPCs;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class TerrorOfTheVoid : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Terror of the Void");
            Tooltip.SetDefault("");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 75;
            Item.width = 48;
            Item.height = 48;
            Item.DamageType = DamageClass.Magic;
            Item.useTime = 35;
            Item.useAnimation = 35;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 10;
            Item.mana = 40;
            Item.value = 35000;
            Item.rare = ItemRarityID.Lime;
            Item.shoot = ProjectileType<TerrorOfTheVoid_RuptureControl>();
            Item.shootSpeed = 1f;
            Item.UseSound = SoundID.Item8;
            Item.noMelee = true;

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.R, new Feast(this));
            abilityItem.ChampQuote = "Your souls will feed the void";
            abilityItem.IsAbilityItem = true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.ownedProjectileCounts[ProjectileType<TerrorOfTheVoid_RuptureControl>()] > 0)
                return false;
            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (velocity.X > 0)
                velocity.X = 3;
            else
                velocity.X = -3;

            velocity.Y = 0;

            Projectile.NewProjectile(source, player.Top, velocity, type, damage, knockback, player.whoAmI, player.GetModPlayer<PLAYERGLOBAL>().feast3 ? 1 : 0);

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
