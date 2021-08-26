using Microsoft.Xna.Framework;
using TerraLeague.Items.Ammo;
using TerraLeague.Items.Weapons.Abilities;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class MagicalPlumage : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magical Plumage");
            Tooltip.SetDefault("");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        string GetWeaponTooltip()
        {
            return "Uses Razor Feathers as ammo";
        }

        public override void SetDefaults()
        {
            Item.damage = 15;
            Item.width = 32;
            Item.height = 32;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 1;
            Item.value = 54000;
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = new LegacySoundStyle(2, 19, Terraria.Audio.SoundType.Sound);
            Item.shootSpeed = 20f;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.useAmmo = ItemType<RazorFeather>();
            Item.noMelee = true;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.noUseGraphic = true;

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.W, new DeadlyPlumage(this));
            abilityItem.ChampQuote = "Feathers fly!";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<ManaBar>(), 16)
            .AddTile(TileID.Anvils)
            .Register();
            
        }
    }
}
