using Microsoft.Xna.Framework;
using TerraLeague.Items.Weapons.Abilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class PetriciteCrossbow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Petricite Crossbow");
            Tooltip.SetDefault("");
            base.SetStaticDefaults();
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 14;
            Item.DamageType = DamageClass.Ranged;
            Item.noMelee = true;
            Item.width = 56;
            Item.height = 24;
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 1f;
            Item.value = 2400;
            Item.rare = ItemRarityID.Green;
            Item.shootSpeed = 10f;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item5;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.useAmmo = AmmoID.Arrow;

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.W, new HeightenedSenses(this));
            abilityItem.ChampQuote = "Valor, to me!";
            abilityItem.IsAbilityItem = true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
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
