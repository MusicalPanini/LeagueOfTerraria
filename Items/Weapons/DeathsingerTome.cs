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
using Terraria.Audio;
using TerraLeague.Items.Weapons.Abilities;
using Terraria.DataStructures;

namespace TerraLeague.Items.Weapons
{
    public class DeathsingerTome : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Death Singer's Tome");
            Tooltip.SetDefault("");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        string GetWeaponTooltip()
        {
            return "Detonate the targeted area after 0.5 seconds" +
                "\nIf you only hit 1 enemy, the damage is doubled";
        }

        public override void SetDefaults()
        {
            Item.damage = 28;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 10;
            Item.rare = ItemRarityID.Orange;
            Item.value = 10000;
            Item.width = 28;
            Item.height = 32;
            Item.useTime = 35;
            Item.useAnimation = 35;
            Item.UseSound = new LegacySoundStyle(2,8);
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shoot = ProjectileType<DeathsingerTome_LayWaste>();

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.E, new Defile(this));
            abilityItem.SetAbility(AbilityType.R, new Requiem(this));
            abilityItem.ChampQuote = "I am the Nightbringer~";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectileDirect(source, Main.MouseWorld, Vector2.Zero, type, damage, 0, player.whoAmI, Main.MouseWorld.X, Main.MouseWorld.Y);

            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.Book, 1)
            .AddIngredient(ItemType<DamnedSoul>(), 50)
            .AddIngredient(ItemID.HellstoneBar, 20)
            .AddIngredient(ItemID.Bone, 50)
            .AddTile(TileID.DemonAltar)
            .Register();
        }
    }
}
