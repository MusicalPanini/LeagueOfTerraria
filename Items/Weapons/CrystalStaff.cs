using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public class CrystalStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crystal Staff");
            Tooltip.SetDefault("");
            base.SetStaticDefaults();
            Item.staff[Item.type] = true;
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 9;
            Item.DamageType = DamageClass.Magic;
            Item.noMelee = true;
            Item.mana = 4;
            Item.width = 16;
            Item.height = 32;
            Item.useTime = 37;
            Item.useAnimation = 37;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 3.5f;
            Item.value = 1000;
            Item.rare = ItemRarityID.Blue;
            Item.shootSpeed = 6.5f;
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.SapphireBolt;

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.W, new DarkMatter(this));
            abilityItem.SetAbility(AbilityType.R, new PrimordialBurst(this));
            abilityItem.ChampQuote = "You deny the darkness in your soul. You deny your power!";
            abilityItem.IsAbilityItem = true;
        }
        public override void OnCraft(Recipe recipe)
        {
            Main.LocalPlayer.QuickSpawnItem(ItemID.ManaCrystal, 1);

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
