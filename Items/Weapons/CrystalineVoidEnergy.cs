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
    public class CrystalineVoidEnergy : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crystaline Void Energy");
            Tooltip.SetDefault("");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        string GetWeaponTooltip()
        {
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();

            return "Shots apply stacks of 'Caustic Wounds'" +
                "\nAt 5 stacks, the enemy will take 25% of their missing life as magic damage" +
                "\nMax damage: " + LeagueTooltip.TooltipValue(50, false, "",
              new System.Tuple<int, ScaleType>(100, ScaleType.Magic)
              ) + "";
        }

        public override void SetDefaults()
        {
            Item.damage = 16;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 24;
            Item.height = 54;
            Item.useAnimation = 35;
            Item.useTime = 35;
            Item.shootSpeed = 9f;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 1;
            Item.value = 5400;
            Item.rare = ItemRarityID.Orange;
            Item.UseSound =  new LegacySoundStyle(2, 75);
            Item.autoReuse = true;
            Item.shoot = ProjectileType<CrystalineVoidEnergy_VoidEnergy>();

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.W, new VoidSeeker(this));
            abilityItem.ChampQuote = "Exploit their weakness";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position.Y += 4;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<VoidFragment>(), 120)
            .AddIngredient(ItemType<VoidbornFlesh>(), 20)
            .AddTile(TileID.DemonAltar)
            .Register();
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-6, 0);
        }
    }
}
