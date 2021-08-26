using Microsoft.Xna.Framework;
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
    public class ClockworkStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Clockwork Staff");
            Tooltip.SetDefault("");
            base.SetStaticDefaults();
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        string GetWeaponTooltip()
        {
            return "Uses 3 minion slots" +
                "\nCan only have one";
        }

        public override void SetDefaults()
        {
            Item.damage = 70;
            Item.DamageType = DamageClass.Summon;
            Item.mana = 20;
            Item.width = 48;
            Item.height = 48;
            Item.useTime = 36;
            Item.useAnimation = 36;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 1;
            Item.value = 100000;
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = new LegacySoundStyle(2, 113);
            Item.shoot = ProjectileType<ClockworkStaff_TheBall>();

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.W, new CommandProtect(this));
            abilityItem.ChampQuote = "Time tick-ticks away";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.ownedProjectileCounts[type] > 0)
            {
                Projectile projectile = Main.projectile.FirstOrDefault(x => x.type == ProjectileType<ClockworkStaff_TheBall>() || x.owner == player.whoAmI);
                
                return false;
            }
            else
            {
                position = Main.MouseWorld;

                if (player.altFunctionUse != 2)
                {
                    Projectile proj = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
                    proj.originalDamage = Item.damage;
                    return false;
                }
                return false;
            }
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                player.MinionNPCTargetAim(true);
            }
            else
            {
                player.AddBuff(BuffType<Buffs.TheBall>(), 2);
            }
            return base.CanUseItem(player);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<PerfectHexCore>())
            .AddRecipeGroup("TerraLeague:Tier3Bar", 14)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
    }
}