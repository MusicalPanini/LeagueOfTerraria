using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
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
    public class RadiantStaff : ModItem
    {
        readonly int shielding = 20;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Radiant Staff");
            Tooltip.SetDefault("");
            Item.staff[Item.type] = false;
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            base.ModifyTooltips(tooltips);

            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();

            int tt2 = tooltips.FindIndex(x => x.Name == "Damage" && x.mod == "Terraria");
            if (tt2 != -1)
            {
                tooltips.Insert(tt2 + 1, new TooltipLine(TerraLeague.instance, "Shielding", 
                    LeagueTooltip.TooltipValue((int)(shielding * (float)modPlayer.magicDamageLastStep), true, "") + " magic shielding"));
            }
        }

        string GetWeaponTooltip()
        {
            return "Send out a returning refraction of your staff, shielding allies and damaging enemies" +
                "\nHas a chance to apply 'Illuminated' to enemies" +
                "\n'Illuminated' enemies take " + LeagueTooltip.TooltipValue(40, false, "",
              new System.Tuple<int, ScaleType>(20, ScaleType.Magic)
              ) + " magic On Hit damage from Radiant Staff";
        }

        public override void SetDefaults()
        {
            Item.damage = 35;
            Item.mana = 18;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.DamageType = DamageClass.Magic;
            Item.useTime = 34;
            Item.useAnimation = 34;
            Item.noMelee = true;
            Item.knockBack = 3;
            Item.value = 55000 * 5;
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = new LegacySoundStyle(2, 8, Terraria.Audio.SoundType.Sound);
            Item.shoot = ProjectileType<RadiantStaff_PrismaticBarrier>();
            Item.shootSpeed = 12f;
            Item.autoReuse = true;

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.E, new LucentSingularity(this));
            abilityItem.SetAbility(AbilityType.R, new FinalSpark(this));
            abilityItem.ChampQuote = "Shine with me";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] == 0;
        }

        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI, player.GetModPlayer<PLAYERGLOBAL>().ScaleValueWithHealPower(shielding * (float)player.GetModPlayer<PLAYERGLOBAL>().magicDamageLastStep, true));
            return false;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-9, 0);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.HallowedBar, 10)
            .AddIngredient(ItemID.SoulofLight, 20)
            .AddIngredient(ItemID.DiamondGemsparkBlock, 10)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
    }
}
