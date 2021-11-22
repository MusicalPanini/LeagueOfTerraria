using Microsoft.Xna.Framework;
using TerraLeague.Items.Weapons.Abilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class Blaze : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blaze");
            Tooltip.SetDefault("");
            base.SetStaticDefaults();
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.scale = 0.75f;
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
            Item.shootSpeed = 14f;
            Item.autoReuse = true;
            Item.UseSound = new Terraria.Audio.LegacySoundStyle(2, 41);
            Item.shoot = ProjectileID.PurificationPowder;
            Item.useAmmo = AmmoID.Bullet;

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            //abilityItem.SetAbility(AbilityType.W, new HeightenedSenses(this));
            abilityItem.ChampQuote = "";
            abilityItem.IsAbilityItem = true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, 0);
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
