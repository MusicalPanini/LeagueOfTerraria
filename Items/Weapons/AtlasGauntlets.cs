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
    public class AtlasGauntlets : ModItem
    {
        public override bool OnlyShootOnSwing => false;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Atlas Gauntlets");
            Tooltip.SetDefault("");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        string GetWeaponTooltip()
        {
            return "Punching enemies grants " + LeagueTooltip.TooltipValue(0, true, "", new System.Tuple<int, ScaleType>( 1, ScaleType.MaxLife)) + " shield for 3 seconds";
        }

        public override void SetDefaults()
        {
            Item.damage = 70;
            Item.width = 52;
            Item.height = 30;
            Item.DamageType = DamageClass.Melee;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Thrust;
            Item.knockBack = 6;    
            Item.value = 100000;
            Item.rare = ItemRarityID.Pink; 
            Item.shoot = ProjectileType<AtlasGauntlets_Right>();
            Item.shootSpeed = 10;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.autoReuse = true;

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.E, new ExcessiveForce(this));
            abilityItem.ChampQuote = "Here comes the punch line!";
            abilityItem.IsAbilityItem = true;
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.ownedProjectileCounts[ProjectileType<AtlasGauntlets_Left>()] > 0 && player.ownedProjectileCounts[ProjectileType<AtlasGauntlets_Right>()] > 0)
                return false;
            return base.CanUseItem(player);
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Item.knockBack = 6;
            if (player.ownedProjectileCounts[ProjectileType<AtlasGauntlets_Right>()] == 0)
            {
                type = ProjectileType<AtlasGauntlets_Right>();
            }
            else if (player.ownedProjectileCounts[ProjectileType<AtlasGauntlets_Left>()] == 0)
            {
                type = ProjectileType<AtlasGauntlets_Left>();
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<PerfectHexCore>())
            .AddRecipeGroup("TerraLeague:Tier3Bar", 14)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
    }
}
