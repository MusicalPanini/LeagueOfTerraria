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
    public class ColossusFist : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Colossus Fist");
            Tooltip.SetDefault("");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 10;
            Item.value = 2400;
            Item.rare = ItemRarityID.Green;
            Item.noMelee = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 40;
            Item.useTime = 40;
            Item.knockBack = 7F;
            Item.damage = 17;
            Item.scale = 1;
            Item.noUseGraphic = false;
            Item.UseSound = new LegacySoundStyle(4, 3);
            Item.shootSpeed = 8f;
            Item.DamageType = DamageClass.Melee;
            Item.autoReuse = true;
            Item.noUseGraphic = true;
            Item.shoot = ProjectileType<ColossusFist_Fist>();

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.Q, new WindsOfWar(this));
            abilityItem.ChampQuote = "Time to make an impact!";
            abilityItem.IsAbilityItem = true;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position.Y += 4;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-6, 4);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<Petricite>(), 16)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}
