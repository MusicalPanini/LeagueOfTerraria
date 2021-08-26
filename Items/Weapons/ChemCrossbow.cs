using System.Linq;
using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using TerraLeague.Items.Weapons.Abilities;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class ChemCrossbow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chem Crossbow");
            Tooltip.SetDefault("");
            base.SetStaticDefaults();
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        string GetWeaponTooltip()
        {
            return "Fires toxic arrows that apply 'Deadly Venom'" +
                "\n'Deadly Venom' stacks 5 times dealing more damage over time per stack";
        }

        public override void SetDefaults()
        {
            Item.damage = 5;
            Item.DamageType = DamageClass.Ranged;
            Item.noMelee = true;
            Item.width = 58;
            Item.height = 34;
            Item.useTime = 26;
            Item.useAnimation = 26;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 0f;
            Item.value = 1000;
            Item.rare = ItemRarityID.Blue;
            Item.shootSpeed = 10f;
            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.UseSound = SoundID.Item5;
            Item.useAmmo = AmmoID.Arrow;

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.W, new ToxicCask(this));
            abilityItem.SetAbility(AbilityType.E, new Contaminate(this));
            abilityItem.ChampQuote = "I dealt it! It was meeee!";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override bool CanUseItem(Player player)
        {
            return base.CanUseItem(player);
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (type == ProjectileID.WoodenArrowFriendly)
                type = ProjectileType<ChemCrossbow_ToxicArrow>();
        }

        public override void OnCraft(Recipe recipe)
        {
            Main.LocalPlayer.QuickSpawnItem(ItemID.WoodenArrow, 150);

            base.OnCraft(recipe);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<StartingItems.WeaponKit>(), 1)
            .Register();
        }
    }
}
