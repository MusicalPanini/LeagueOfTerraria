using Microsoft.Xna.Framework;
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
    public class SerpentsEmbrace : ModItem
    {
        public static int MAGScaling = 30;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Serpent's Embrace");
            Tooltip.SetDefault("");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        string GetWeaponTooltip()
        {
            return "Fire a serpent that applies 'Venom'." +
                "\nRight Click to fire Twin Fangs that home in on 'Venom' affected enemies." +
                "\nTwin Fangs deal " + LeagueTooltip.TooltipValue(Item.damage, false, "",
              new System.Tuple<int, ScaleType>(MAGScaling, ScaleType.Magic)
              ) + " magic damage" +
              "\nUses " + (int)(10 * Main.LocalPlayer.manaCost) + " mana";
        }

        public override void SetDefaults()
        {
            Item.damage = 31;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 28;
            Item.height = 58;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.shootSpeed = 10f;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 1;
            Item.value = 60000 * 5;
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.useAmmo = AmmoID.Arrow;

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.Q, new NoxiousBlast(this));
            abilityItem.ChampQuote = "There is no antidote for me";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override bool AltFunctionUse(Player player)
        {
            return player.CheckMana(10, true);
        }

        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                int tfDamage = Item.damage + (MAGScaling * player.GetModPlayer<PLAYERGLOBAL>().MAG / 100);
                Projectile.NewProjectileDirect(source, position, velocity, ProjectileType<SerpentsEmbrace_TwinFangs>(), tfDamage, 1, player.whoAmI, -2);
                return false;
            }
            //if (Targeting.IsThereAnNPCInRange(player.MountedCenter, 500, BuffID.Venom))
            //{
                
            //    Vector2 center = player.MountedCenter;

            //    var targets = Targeting.GetAllNPCsInRange(center, 500, true);
            //    for (int i = 0; i < targets.Count; i++)
            //    {
            //        if (Main.npc[targets[i]].HasBuff(BuffID.Venom))
            //        {
            //            Projectile.NewProjectileDirect(source, center, TerraLeague.CalcVelocityToPoint(center, Main.npc[targets[i]].Center, 16), ProjectileType<SerpentsEmbrace_TwinFangs>(), tfDamage, 1, player.whoAmI, targets[i]);
            //        }
            //    }
            //    return false;
            //}
            else
            {
                if (type == ProjectileID.WoodenArrowFriendly)
                    type = ProjectileType<SerpentsEmbrace_Serpent>();

                Projectile.NewProjectileDirect(source, position, velocity, type, damage, 1, player.whoAmI);
                return false;
            }

        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<Sunstone>(), 10)
            .AddIngredient(ItemID.Sapphire, 1)
            .AddIngredient(ItemID.SoulofSight, 12)
            .AddIngredient(ItemID.AncientBattleArmorMaterial, 1)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}
