using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.Linq;
using Microsoft.Xna.Framework;
using TerraLeague.Projectiles;
using Terraria.Audio;
using static Terraria.ModLoader.ModContent;
using TerraLeague.Items.Weapons.Abilities;

namespace TerraLeague.Items.Weapons
{
    public class EyeOfTheVoid : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Eye of the Void");
            Tooltip.SetDefault("");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        string GetWeaponTooltip()
        {
            return "Fire a lifeform disintegration ray!";
        }

        public override void SetDefaults()
        {
            Item.damage = 16;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Magic;
            Item.channel = true;
            Item.mana = 6;
            Item.rare = ItemRarityID.Orange;
            Item.value = 5400;
            Item.width = 28;
            Item.height = 32;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.UseSound = new LegacySoundStyle(2, 15);
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shootSpeed = 10;
            Item.shoot = ProjectileType<EyeOfTheVoid_Lazer>();

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.Q, new PlasmaFission(this));
            abilityItem.ChampQuote = "Knowledge through... disintegration";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<VoidFragment>(), 120)
            .AddIngredient(ItemType<VoidbornFlesh>(), 20)
            .AddTile(TileID.DemonAltar)
            .Register();
        }
    }
}
