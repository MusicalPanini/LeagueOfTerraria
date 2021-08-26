using Microsoft.Xna.Framework;
using TerraLeague.Items.Weapons.Abilities;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class HeartoftheTempest : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Heart of the Tempest");

            Tooltip.SetDefault("");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        string GetWeaponTooltip()
        {
            return "Create a Slicing Maelstrom after hitting 3 times";
        }

        public override void SetDefaults()
        {
            Item.damage = 60;
            Item.width = 30;
            Item.height = 30;
            Item.DamageType = DamageClass.Melee;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 2.5f;
            Item.value = 350000;
            Item.rare = ItemRarityID.Lime;
            Item.UseSound = SoundID.Item1;
            Item.shootSpeed = 16f;
            Item.shoot = ProjectileType<HeartoftheTempest_Yoyo>();
            Item.noMelee = true;
            Item.channel = true;
            Item.noUseGraphic = true;

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.E, new LightningRush(this));
            abilityItem.ChampQuote = "I am the wind!";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<HarmonicBar>(), 16)
            .AddTile(TileID.MythrilAnvil)
            .Register();
            
        }
    }
}
