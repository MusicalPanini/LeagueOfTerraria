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
    public class VoidReaverScythe : ModItem
    {
        public override bool OnlyShootOnSwing => true;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Voidreaver Scythe");
            Tooltip.SetDefault("");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 32;
            Item.width = 46;          
            Item.height = 44;         
            Item.DamageType = DamageClass.Melee;        
            Item.useTime = 52;        
            Item.useAnimation = 26;
            Item.useStyle = ItemUseStyleID.Swing;          
            Item.knockBack = 3;    
            Item.value = 5400;
            Item.rare = ItemRarityID.Orange; 
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<VoidReaverScythe_VoidSpike>();
            Item.shootSpeed = 18;
            Item.scale = 1.3f;

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.R, new EvolvedWings(this));
            abilityItem.ChampQuote = "Fear the Void";
            abilityItem.IsAbilityItem = true;
        }

        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            Projectile.NewProjectile(source, position, velocity.RotatedBy(0.3f), type, damage, knockback, player.whoAmI);
            Projectile.NewProjectile(source, position, velocity.RotatedBy(-0.3f), type, damage, knockback, player.whoAmI);

            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<VoidFragment>(), 120)
            .AddIngredient(ItemType<VoidbornFlesh>(), 20)
            .AddTile(TileID.DemonAltar)
            .Register();
        }
    }
}
