using Microsoft.Xna.Framework;
using System.Collections.Generic;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.Weapons.Abilities;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class HexCoreStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hex Core Staff");
            Tooltip.SetDefault("");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        string GetWeaponTooltip()
        {
            return "Control a Chaos Storm";
        }

        public override void SetDefaults()
        {
            Item.damage = 48;
            Item.width = 48;
            Item.height = 48;
            Item.DamageType = DamageClass.Magic;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 0;
            Item.value = 100000;
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = new LegacySoundStyle(2, 82, Terraria.Audio.SoundType.Sound);
            Item.shootSpeed = 8f;
            Item.shoot = ProjectileType<HexCoreStaff_ChaosStorm>();
            Item.noMelee = true;
            Item.useTurn = true;
            Item.autoReuse = false;
            Item.mana = 40;
            Item.channel = true;

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.W, new GravityField(this));
            abilityItem.ChampQuote = "Join the glorious evolution";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[ProjectileType<HexCoreStaff_ChaosStorm>()] < 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<PerfectHexCore>())
            .AddRecipeGroup("TerraLeague:Tier3Bar", 14)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
    }
}
