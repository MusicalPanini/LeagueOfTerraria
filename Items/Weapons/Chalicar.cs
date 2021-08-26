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
    public class Chalicar : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chalicar of Setaka");
            Tooltip.SetDefault("");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 14;
            Item.width = 38;
            Item.height = 38;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 1.5f;
            Item.value = 10000;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = new LegacySoundStyle(2, 19, Terraria.Audio.SoundType.Sound);
            Item.shootSpeed = 12f;
            Item.shoot = ProjectileType<Projectiles.Chalicar_Disk>();
            Item.noMelee = true;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.noUseGraphic = true;

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.W, new Ricochet(this));
            abilityItem.ChampQuote = "Better duck!";
            abilityItem.IsAbilityItem = true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.ownedProjectileCounts[ProjectileType<Chalicar_Disk>()] < 1)
                return base.CanUseItem(player);
            else
                return false;
        }


        public override void AddRecipes()
        {
            CreateRecipe()
            .AddRecipeGroup("TerraLeague:IronGroup", 10)
            .AddRecipeGroup("TerraLeague:GoldGroup", 10)
            .AddIngredient(ItemType<Sunstone>(), 10)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}
