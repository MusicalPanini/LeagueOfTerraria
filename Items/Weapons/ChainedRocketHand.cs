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
    public class ChainedRocketHand : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chained Rocket Hand");
            Tooltip.SetDefault("");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = 30000;
            Item.rare = ItemRarityID.Orange;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 40;
            Item.useTime = 40;
            Item.knockBack = 6F;
            Item.damage = 24;
            Item.scale = 1;
            Item.noUseGraphic = true;
            Item.UseSound = SoundID.Item10;
            Item.shootSpeed = 8f;
            Item.DamageType = DamageClass.Melee;
            Item.shoot = ProjectileType<ChainedRocketHand_RobotFist>();

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.R, new StaticField(this));
            abilityItem.ChampQuote = "Metal is harder than flesh";
            abilityItem.IsAbilityItem = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddRecipeGroup("TerraLeague:CopperGroup", 16)
            .AddIngredient(ItemID.HellstoneBar, 4)
            .AddIngredient(ItemID.Harpoon)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}
