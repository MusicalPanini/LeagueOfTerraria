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
    public class ToxicBlowgun : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Toxic Blowgun");
            Tooltip.SetDefault("");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.Blowgun);
            Item.damage = 24;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 24;
            Item.height = 54;
            Item.useAnimation = 35;
            Item.useTime = 35;
            Item.shootSpeed = 10f;
            Item.noMelee = true;
            Item.knockBack = 1;
            Item.value = 200000;
            Item.rare = ItemRarityID.Lime;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.PurificationPowder;

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.E, new ToxicShot(this));
            abilityItem.SetAbility(AbilityType.R, new NoxiousTrap(this));
            abilityItem.ChampQuote = "Never underestimate the power of the Scout's code";
            abilityItem.IsAbilityItem = true;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position = new Vector2(position.X, position.Y - 6);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.Blowgun, 1)
            .AddIngredient(ItemID.VialofVenom, 10)
            .AddIngredient(ItemID.Mushroom, 1)
            .AddTile(TileID.Anvils)
            .Register();
            
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(6, -10);
        }
    }
}
