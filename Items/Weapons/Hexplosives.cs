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
    public class Hexplosives : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hexplosives");
            Tooltip.SetDefault("");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 55;
            Item.width = 32;
            Item.height = 32;
            Item.DamageType = DamageClass.Magic;
            Item.useAnimation = 32;
            Item.useTime = 32;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5;
            Item.value = 40000;
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = new LegacySoundStyle(2, 19, Terraria.Audio.SoundType.Sound);
            Item.shootSpeed = 12f;
            Item.shoot = ProjectileType<Hexplosives_Bomb>();
            Item.mana = 8;
            Item.noMelee = true;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.noUseGraphic = true;

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.Q, new BouncingBomb(this));
            abilityItem.SetAbility(AbilityType.E, new HexsplosiveMineField(this));
            abilityItem.ChampQuote = "This'll be a blast!";
            abilityItem.IsAbilityItem = true;
        }

        public override bool CanUseItem(Player player)
        {
            return true;
        }

        public override float UseTimeMultiplier(Player player)
        {
            return 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<HextechCore>(), 2)
            .AddIngredient(ItemID.Bomb, 20)
            .AddRecipeGroup("TerraLeague:Tier3Bar", 6)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
    }
}
