using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using TerraLeague.Items.Weapons.Abilities;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class DarkinScythe : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Darkin Scythe");
            Tooltip.SetDefault("");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        string GetWeaponTooltip()
        {
            return "Attacks will mark enemies";
        }

        public override void SetDefaults()
        {
            Item.damage = 32;
            Item.width = 60;
            Item.height = 54;       
            Item.DamageType = DamageClass.Melee;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 3;
            Item.value = 100000;
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.useTurn = true;

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.Q, new ReapingSlash(this));
            abilityItem.SetAbility(AbilityType.R, new UmbralTrespass(this));
            abilityItem.ChampQuote = "From the shadow comes the slaughter";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffType<Buffs.UmbralTrespass>(), 300);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.Sickle, 1)
            .AddRecipeGroup("TerraLeague:DemonGroup", 20)
            .AddIngredient(ItemID.SoulofNight, 10)
            .AddTile(TileID.DemonAltar)
            .Register();
            
        }
    }
}
