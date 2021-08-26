using Microsoft.Xna.Framework;
using System;
using TerraLeague.Items.Weapons.Abilities;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class StrangleThornsTome : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Strangle Thorn Tome");
            Tooltip.SetDefault("");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        string GetWeaponTooltip()
        {
            return "Enemies hit with Strangle Thorns have a chance to drop a seed on death." +
                "\nIf Strangle Thorns passes near a bulb it will grow into a Thorn Spitter, attacking near by enemies" +
                "\n(WIP)";
        }

        public override void SetDefaults()
        {
            Item.damage = 30;
            Item.width = 56;
            Item.height = 56;       
            Item.DamageType = DamageClass.Summon;
            Item.noMelee = true;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 1;
            Item.value = 140000;
            Item.rare = ItemRarityID.Lime;
            Item.UseSound = SoundID.Item8;
            Item.mana = 16;
            Item.shootSpeed = 32;
            Item.shoot = ProjectileType<StrangleThornsTome_StrangleThorns>();

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.W, new RampantGrowth(this));
            abilityItem.ChampQuote = "Feel the thorns embrace";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.NettleBurst, 1)
            .AddIngredient(ItemID.Stinger, 12)
            .AddIngredient(ItemID.JungleSpores, 20)
            .AddIngredient(ItemID.JungleRose, 1)
            .AddIngredient(ItemID.SoulofNight, 12)
            .AddTile(TileID.DemonAltar)
            .Register();
            
        }
    }
}
