using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerraLeague.Projectiles;
using TerraLeague.Buffs;
using static Terraria.ModLoader.ModContent;
using TerraLeague.Items.Weapons.Abilities;

namespace TerraLeague.Items.Weapons
{
    public class LastBreath : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Last Breath");
            Tooltip.SetDefault("");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.damage = 22;
            Item.DamageType = DamageClass.Melee;
            Item.width = 58;
            Item.height = 56;
            Item.useTime = 35;
            Item.useAnimation = 35;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 2;
            Item.value = 48000;
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item1;
            Item.crit = 25;
            Item.autoReuse = true;
            Item.shootSpeed = 8f;

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.Q, new SteelTempest(this));
            abilityItem.ChampQuote = "No cure for fools";
            abilityItem.IsAbilityItem = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.Katana, 1)
            .AddIngredient(ItemID.AnkletoftheWind, 1)
            .AddTile(TileID.Anvils)
            .Register();
            
        }
    }
}
