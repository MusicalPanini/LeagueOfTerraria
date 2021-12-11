using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.Collections.Generic;
using System.Linq;
using TerraLeague.NPCs;
using TerraLeague.Projectiles;
using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using Terraria.Audio;
using static Terraria.ModLoader.ModContent;
using TerraLeague.Items.Weapons.Abilities;

namespace TerraLeague.Items.Weapons
{
    public class BurningVengance : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Burning Vengance");
            Tooltip.SetDefault("");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 23;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Magic;
            Item.useTime = 5;
            Item.useAnimation = 10;
            Item.shootSpeed = 14;
            Item.mana = 6;
            Item.rare = ItemRarityID.Pink;
            Item.value = 300000;
            Item.width = 28;
            Item.height = 32;
            Item.knockBack = 1;
            Item.UseSound = new LegacySoundStyle(2, 34);
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shoot = ProjectileType<BurningVengance_Flame>();
            Item.autoReuse = true;

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.W, new PillarOfFlame(this));
            abilityItem.SetAbility(AbilityType.R, new Pyroclasm(this));
            abilityItem.ChampQuote = "The inferno begins";
            abilityItem.IsAbilityItem = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.SpellTome, 1)
            .AddIngredient(ItemID.LivingFireBlock, 20)
            .AddIngredient(ItemID.SoulofFright, 10)
            .AddTile(TileID.Bookcases)
            .Register();
        }
    }
}
