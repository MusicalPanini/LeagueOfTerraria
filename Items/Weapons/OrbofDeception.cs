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
    public class OrbofDeception : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Orb of Deception");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(8, 4));

            Tooltip.SetDefault("");
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        string GetWeaponTooltip()
        {
            return "Send out a returning orb of magic that crits on the way back";
        }

        public override void SetDefaults()
        {
            Item.damage = 28;
            Item.width = 30;
            Item.height = 30;
            Item.DamageType = DamageClass.Magic;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Thrust;
            Item.knockBack = 1;
            Item.mana = 20;
            Item.value = 54000;
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = new LegacySoundStyle(2, 8, Terraria.Audio.SoundType.Sound);
            Item.shootSpeed = 15f;
            Item.shoot = ProjectileType<OrbofDeception_Orb>();
            Item.noMelee = true;
            Item.useTurn = true;
            Item.autoReuse = false;
            Item.noUseGraphic = true;

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.W, new FoxFire(this));
            abilityItem.ChampQuote = "Let's have some real fun";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[ProjectileType<OrbofDeception_Orb>()] < 1;
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
