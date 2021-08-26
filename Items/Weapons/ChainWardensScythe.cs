using Microsoft.Xna.Framework;
using TerraLeague.Items.Weapons.Abilities;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class ChainWardensScythe : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Chain Warden's Scythe");
            Tooltip.SetDefault("");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 46;
            Item.value = 54000;
            Item.rare = ItemRarityID.Green;
            Item.noMelee = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 40;
            Item.useTime = 40;
            Item.knockBack = 6F;
            Item.damage = 14;
            Item.scale = 1;
            Item.noUseGraphic = true;
            Item.UseSound = SoundID.Item1;
            Item.shootSpeed = 14F;
            Item.DamageType = DamageClass.Melee;
            Item.channel = true;
            Item.shoot = ProjectileType<ChainWardensScythe_Scythe>();

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.W, new DarkPassage(this));
            abilityItem.ChampQuote = "What delightful agony we shall inflict";
            abilityItem.IsAbilityItem = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<DamnedSoul>(), 50)
            .AddRecipeGroup("TerraLeague:DemonGroup", 16)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}
