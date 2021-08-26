using Microsoft.Xna.Framework;
using TerraLeague.Items.Weapons.Abilities;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class Crescendum : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crescendum");
            Tooltip.SetDefault("");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        string GetWeaponTooltip()
        {
            return "Uses 1% Crescendum Ammo" +
                "\nEach Lunari gun has its own special ammo that rechages when the gun is not in use." +
                "\nThrow up to " + 
                LeagueTooltip.TooltipValue(5, false, "",
              new System.Tuple<int, ScaleType>(100, ScaleType.Minions)
              ) + " returning chakrams";
        }

        public override void SetDefaults()
        {
            Item.damage = 100;
            Item.DamageType = DamageClass.Summon;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.width = 48;
            Item.height = 48;
            Item.useAnimation = 7;
            Item.useTime = 7;
            Item.shootSpeed = 16f;
            Item.noMelee = true;
            Item.knockBack = 2;
            Item.value = 310000 * 5;
            Item.rare = ItemRarityID.Purple;
            Item.shoot = ProjectileType<Crescendum_Proj>();
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.noUseGraphic = true;

            
            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.Q, new Sentry(this));
            abilityItem.SetAbility(AbilityType.W, new Phase(this, LunariGunType.Cre));
            abilityItem.ChampQuote = "An orbit of blades";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.ownedProjectileCounts[ProjectileType<Crescendum_Proj>()] < player.maxMinions + 5 )
            {
                if (player.GetModPlayer<PLAYERGLOBAL>().crescendumAmmo < 1)
                {
                    if (Main.mouseLeftRelease)
                    {
                        TerraLeague.PlaySoundWithPitch(player.MountedCenter, 12, 0, -0.5f);
                        CombatText.NewText(player.Hitbox, Color.White, "NO AMMO");
                    }
                    return false;
                }
                return base.CanUseItem(player);
            }

            return false;
        }

        public override bool AltFunctionUse(Player player)
        {
            return false;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            player.GetModPlayer<PLAYERGLOBAL>().crescendumAmmo -= 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.LunarBar, 16)
            .AddTile(TileID.Anvils)
            .Register();
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, 0);
        }
    }
}
