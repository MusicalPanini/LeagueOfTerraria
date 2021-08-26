using TerraLeague.Buffs;
using TerraLeague.Items.Weapons.Abilities;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class DarkinBlade : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Darkin Blade");
            Tooltip.SetDefault("");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 60;
            Item.width = 58;
            Item.height = 58;
            Item.DamageType = DamageClass.Melee;
            Item.useTime = 32;
            Item.useAnimation = 32;
            Item.scale = 1.3f;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6;
            Item.value = 100000;
            Item.rare = ItemRarityID.LightRed;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item1;

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.R, new WorldEnder(this));
            abilityItem.ChampQuote = "I am oblivion, I am destruction... I am doom";
            abilityItem.IsAbilityItem = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.BreakerBlade, 1)
            .AddRecipeGroup("TerraLeague:DemonGroup", 20)
            .AddIngredient(ItemID.SoulofNight, 10)
            .AddTile(TileID.DemonAltar)
            .Register();
            
        }
    }
}
