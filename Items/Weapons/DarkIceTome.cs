using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using TerraLeague.Projectiles;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using TerraLeague.Items.Weapons.Abilities;

namespace TerraLeague.Items.Weapons
{
    public class DarkIceTome : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dark Ice Tome");
            Tooltip.SetDefault("");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        string GetWeaponTooltip()
        {
            return "Attacks slow hit enemies." +
                "\nIf the enemy dies while slowed, they will shatter into icy shrapenel." +
                "\nThe shrapenel deals 10% of the dead npcs max health as magic damage.";
        }

        public override void SetDefaults()
        {
            Item.damage = 50;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 14;
            Item.value = 160000;
            Item.rare = ItemRarityID.Pink;
            Item.width = 28;
            Item.height = 32;
            Item.useTime = 45;
            Item.useAnimation = 45;
            Item.knockBack = 2;
            Item.UseSound = new Terraria.Audio.LegacySoundStyle(2,8);
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shootSpeed = 16;
            Item.shoot = ProjectileType<DarkIceTome_IceShard>();

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.W, new RingOfFrost(this));
            abilityItem.ChampQuote = "I will bury the world in ice";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<TrueIceChunk>(), 4)
            .AddIngredient(ItemID.DemonScythe, 1)
            .AddIngredient(ItemID.FrostCore, 1)
            .AddTile(TileID.Anvils)
            .Register();
            
        }
    }
}
