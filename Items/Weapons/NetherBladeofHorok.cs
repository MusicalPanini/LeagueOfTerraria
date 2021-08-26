using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
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
    public class NetherBladeofHorok : ModItem
    {
        public override bool OnlyShootOnSwing => true;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Nether Blade of Horok");
            Tooltip.SetDefault("");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.damage = 74;
            Item.width = 40;
            Item.height = 40;
            Item.DamageType = DamageClass.Melee;
            Item.useTime = 32;
            Item.useAnimation = 32;
            Item.scale = 1.3f;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5;
            Item.value = 120000;
            Item.rare = ItemRarityID.Lime;
            Item.shoot = ProjectileType<NetherBladeofHorok_NullSphere>();
            Item.autoReuse = true;
            Item.UseSound = new LegacySoundStyle(2, 15);
            Item.shootSpeed = 7;
            //item.GetGlobalItem<TerraLeagueITEMGLOBAL>().meleeProjCooldown = true;

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.R, new Riftwalk(this));
            abilityItem.ChampQuote = "You are null and void";
            abilityItem.IsAbilityItem = true;
        }

        

        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectileDirect(source, player.MountedCenter, velocity, type, damage, 0, player.whoAmI, -2);
            return false;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            Dust dust = Dust.NewDustDirect(hitbox.TopLeft(), hitbox.Width, hitbox.Height, 112, 0,0, 255, new Color(59, 0, 255), 1f);
            dust.noGravity = true;
            dust.noLight = true;

            base.MeleeEffects(player, hitbox);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<VoidBar>(), 14)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}
