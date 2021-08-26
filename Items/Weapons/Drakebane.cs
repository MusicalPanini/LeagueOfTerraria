using Microsoft.Xna.Framework;
using TerraLeague.Items.Weapons.Abilities;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class Drakebane : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Drakebane");
            Tooltip.SetDefault("");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 26;
            Item.width = 64;
            Item.height = 64;
            Item.DamageType = DamageClass.Melee;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 2;
            Item.value = 6000;
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = new LegacySoundStyle(2, 101);
            Item.shootSpeed = 1f;
            Item.shoot = ProjectileType<Drakebane_Whip>();
            Item.noMelee = true;
            Item.noUseGraphic = true;

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.E, new DemacianStandard(this));
            abilityItem.ChampQuote = "Righteous retribution!";
            abilityItem.IsAbilityItem = true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.ownedProjectileCounts[ProjectileType<Drakebane_Whip>()] < 1)
                return base.CanUseItem(player);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<SilversteelBar>(), 12)
            .AddTile(TileID.Anvils)
            .Register();
            
        }
    }
}
