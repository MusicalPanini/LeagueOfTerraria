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
    public class SerpentsEmbrace : ModItem
    {
        public static int MAGScaling = 30;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Serpent's Embrace");
            Tooltip.SetDefault("");
        }

        string GetWeaponTooltip()
        {
            return "Fire a serpent that applies 'Venom'." +
                "\nIf a nearby enemy has 'Venom' launch Twin Fangs instead at nearby 'Venom' affected enemies instead." +
                "\nTwin Fangs deal " + LeagueTooltip.TooltipValue(item.damage, false, "",
              new System.Tuple<int, ScaleType>(MAGScaling, ScaleType.Magic)
              ) + " magic damage";
        }

        public override void SetDefaults()
        {
            item.damage = 31;
            item.ranged = true;
            item.width = 28;
            item.height = 58;
            item.useAnimation = 30;
            item.useTime = 30;
            item.shootSpeed = 10f;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noMelee = true;
            item.knockBack = 1;
            item.value = 160000;
            item.rare = ItemRarityID.Pink;
            item.UseSound = SoundID.Item5;
            item.autoReuse = true;
            item.shoot = ProjectileID.PurificationPowder;
            item.useAmmo = AmmoID.Arrow;

            AbilityItemGLOBAL abilityItem = item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.Q, new NoxiousBlast(this));
            abilityItem.ChampQuote = "There is no antidote for me";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (Targeting.IsThereAnNPCInRange(player.MountedCenter, 500, BuffID.Venom))
            {
                int tfDamage = item.damage + (MAGScaling * player.GetModPlayer<PLAYERGLOBAL>().MAG / 100);
                Vector2 center = player.MountedCenter;

                var targets = Targeting.GetAllNPCsInRange(center, 500, true);
                for (int i = 0; i < targets.Count; i++)
                {
                    if (Main.npc[targets[i]].HasBuff(BuffID.Venom))
                    {
                        Projectile.NewProjectileDirect(center, TerraLeague.CalcVelocityToPoint(center, Main.npc[targets[i]].Center, 16), ProjectileType<SerpentsEmbrace_TwinFangs>(), tfDamage, 1, player.whoAmI, targets[i]);
                    }
                }
                return false;
            }
            else
            {
                if (type == ProjectileID.WoodenArrowFriendly)
                    type = ProjectileType<SerpentsEmbrace_Serpent>();
                return true;
            }

        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<TrueIceChunk>(), 4);
            recipe.AddIngredient(ItemID.HellwingBow, 1);
            recipe.AddIngredient(ItemID.FrostCore, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
