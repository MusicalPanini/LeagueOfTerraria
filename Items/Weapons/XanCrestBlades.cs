using Microsoft.Xna.Framework;
using System.Collections.Generic;
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
    public class XanCrestBlades : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Xan Crest Blades");

            Tooltip.SetDefault("");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        string GetWeaponTooltip()
        {
            return "Manipulate " +
                LeagueTooltip.TooltipValue(0, false, "",
                new System.Tuple<int, ScaleType>(150, ScaleType.Minions)
                ) +
                " flowing blades that deal damage based on their speed";
        }

        public override void SetDefaults()
        {
            Item.damage = 34;
            Item.width = 62;
            Item.height = 26;
            Item.DamageType = DamageClass.Summon;
            Item.useTime = 25;
            Item.mana = 20;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 0;
            Item.value = 350000;
            Item.rare = ItemRarityID.Lime;
            Item.UseSound = new LegacySoundStyle(2, 82, Terraria.Audio.SoundType.Sound);
            Item.shootSpeed = 15f;
            Item.shoot = ProjectileType<XanCrestBlades_DancingBlade>();
            Item.noMelee = true;
            Item.useTurn = true;
            Item.autoReuse = false;
            Item.noUseGraphic = true;
            Item.channel = true;

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.Q, new BladeSurge(this));
            abilityItem.ChampQuote = "Cut them down!";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[ProjectileType<XanCrestBlades_DancingBlade>()] < 1;
        }

        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = (int)(player.maxMinions * 1.5) - 1; i >= 0; i--)
            {
                Projectile proj = Projectile.NewProjectileDirect(source, player.Center, Vector2.Zero, ProjectileType<XanCrestBlades_DancingBlade>(), damage, knockback, player.whoAmI, i);
                proj.originalDamage = Item.damage;
            }

            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.HallowedBar, 16)
            .AddIngredient(ItemID.SoulofMight, 10)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
    }
}
