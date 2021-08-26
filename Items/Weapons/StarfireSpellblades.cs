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
    public class StarfireSpellblades : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starfire Spellblade");
            Tooltip.SetDefault("");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        string GetWeaponTooltip()
        {
            return "Gains attack speed and damage each half second in combat" +
                "\nAfter 6 seconds, the sword will ascend and fire waves of starfire" +
                "\nThe wave deals " + LeagueTooltip.TooltipValue((int)(Item.damage * 1.5), false, "",
                new System.Tuple<int, ScaleType>(30, ScaleType.Melee),
                new System.Tuple<int, ScaleType>(50, ScaleType.Summon)
                ) + " melee damage";
           // "\nThe wave deals " + (int)(Item.damage * 0.75) + " + " + TerraLeague.CreateScalingTooltip(DamageType.MEL, Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().MEL, 30) + " + " + TerraLeague.CreateScalingTooltip(DamageType.SUM, Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().SUM, 50) + " melee damage";
        }

        public override void SetDefaults()
        {
            Item.damage = 55;
            Item.width = 56;
            Item.height = 56;       
            Item.DamageType = DamageClass.Melee;
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6;
            Item.value = 200000;
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.R, new DivineJudgement(this));
            abilityItem.ChampQuote = "As evil grows, so shall I";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage, ref float flat)
        {
            //mult = 1 + (player.GetModPlayer<PLAYERGLOBAL>().AscensionStacks * 0.2f);
        }

        public override void UpdateInventory(Player player)
        {
            if (player.GetModPlayer<PLAYERGLOBAL>().AscensionStacks >= 6)
            {
                int prefix = Item.prefix;
                Item.SetDefaults(ItemType<StarfireSpellbladesAscended>());
                Item.prefix = prefix;
            }

            base.UpdateInventory(player);
        }

        public override float UseSpeedMultiplier(Player player)
        {
            return base.UseSpeedMultiplier(player) + (player.GetModPlayer<PLAYERGLOBAL>().AscensionStacks * 0.05f);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.BrokenHeroSword, 2)
            .AddIngredient(ItemID.SoulofLight, 20)
            .AddIngredient(ItemID.FallenStar, 10)
            .AddIngredient(ItemType<FragmentOfTheAspect>(), 1)
            .AddIngredient(ItemType<CelestialBar>(), 20)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
    }
}
