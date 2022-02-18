using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using TerraLeague.Buffs;
using TerraLeague.Items.Weapons.Abilities;
using TerraLeague.NPCs;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class BoneSkewer : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bone Skewer");
            Tooltip.SetDefault("");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        string GetWeaponTooltip()
        {
            return "Right Click to stab for double damage";
        }

        public override void SetDefaults()
        {
            Item.damage = 13;
            Item.width = 48;
            Item.height = 48;
            Item.DamageType = DamageClass.Melee;
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.knockBack = 2;
            Item.value = 3500;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item1;

            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.shoot = ProjectileType<BoneSkewer_Thrown>();
            Item.shootSpeed = 10;

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.R, new DeathFromBelow(this));
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.ChampQuote = "There’s plenty of room for everyone at the bottom of the sea...";
            abilityItem.IsAbilityItem = true;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                type = ProjectileType<BoneSkewer_Stab>();
                damage *= 2;
            }

            base.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);
        }

        public override float UseSpeedMultiplier(Player player)
        {
            float multi = base.UseSpeedMultiplier(player);
            if (player.altFunctionUse == 2)
                multi += 1;
            return multi;
        }

        public override bool CanUseItem(Player player)
        {
            return base.CanUseItem(player);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<BrassBar>(), 14)
            .AddTile(TileID.Anvils)
            .Register();
            
        }
    }
}
