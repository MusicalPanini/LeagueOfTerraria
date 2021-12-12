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
    public class EchoingFlameCannon : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Echoing Flame Cannon");
            Tooltip.SetDefault("");
            base.SetStaticDefaults();
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        string GetWeaponTooltip()
        {
            return "You are surrounded by 6 markers." +
                "\nShooting towards one will fire a shotgun of cursed flame." +
                "\nEach mark recharges every 7 seconds";
        }

        public override void SetDefaults()
        {
            Item.damage = 52;
            Item.DamageType = DamageClass.Ranged;
            Item.noMelee = true;
            Item.width = 40;
            Item.height = 32; 
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 5f;
            Item.value = 100000;
            Item.rare = ItemRarityID.Pink;
            Item.shootSpeed = 12f;
            Item.UseSound = new LegacySoundStyle(2, 36);
            Item.useAmmo = AmmoID.Bullet;
            Item.shoot = ProjectileID.Bullet;
            Item.autoReuse = true;

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.Q, new CorrosiveCharge(this));
            abilityItem.ChampQuote = "You cannot know strength... Until you are broken";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        
        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            EchoingFlamesEffect(player, damage, velocity, source);
            return true;
        }

        public void EchoingFlamesEffect(Player player, int damage, Vector2 velocity, ProjectileSource_Item_WithAmmo source)
        {
            float angle = velocity.ToRotation();
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            int cooldown = 7;
            float[] projectileAngles = null;
            if (angle > -MathHelper.PiOver2 && angle <= -MathHelper.PiOver2 + MathHelper.ToRadians(60))
            {
                if (modPlayer.echoingFlames_RT <= 0)
                {
                    modPlayer.echoingFlames_RT = cooldown * 60;
                    projectileAngles = new float[] { -85, -75, -65, -55, -45, -35 };
                }
            }
            else if (angle > -MathHelper.ToRadians(30) && angle <= MathHelper.ToRadians(30))
            {
                if (modPlayer.echoingFlames_RM <= 0)
                {
                    modPlayer.echoingFlames_RM = cooldown * 60;
                    projectileAngles = new float[] { -25, -15, -5, 5, 15, 25 };
                }
            }
            else if (angle > MathHelper.ToRadians(30) && angle <= MathHelper.PiOver2)
            {
                if (modPlayer.echoingFlames_RB <= 0)
                {
                    modPlayer.echoingFlames_RB = cooldown * 60;
                    projectileAngles = new float[] { 35, 45, 55, 65, 75, 85};
                }
            }
            else if (angle > MathHelper.PiOver2 && angle <= MathHelper.PiOver2 + MathHelper.ToRadians(60))
            {
                if (modPlayer.echoingFlames_LB <= 0)
                {
                    modPlayer.echoingFlames_LB = cooldown * 60;
                    projectileAngles = new float[] { 95, 105, 115, 125, 135, 145 };
                }
            }
            else if ((angle > MathHelper.Pi - MathHelper.ToRadians(30) && angle <= MathHelper.Pi) || angle >= -MathHelper.Pi && angle <= -MathHelper.Pi + MathHelper.ToRadians(30))
            {
                if (modPlayer.echoingFlames_LM <= 0)
                {
                    modPlayer.echoingFlames_LM = cooldown * 60;
                    projectileAngles = new float[] { 155, 165, 175, -175, -165, -155 };
                }
            }
            else if (angle > -MathHelper.Pi + MathHelper.ToRadians(30) && angle <= -MathHelper.PiOver2)
            {
                if (modPlayer.echoingFlames_LT <= 0)
                {
                    modPlayer.echoingFlames_LT = cooldown * 60;
                    projectileAngles = new float[] { -145, -135, -125, -115, -105, -95 };
                }
            }

            if (projectileAngles != null)
            {
                for (int i = 0; i < projectileAngles.Length; i++)
                {
                    Projectile.NewProjectileDirect(source, player.MountedCenter, new Vector2(12, 0).RotatedBy(MathHelper.ToRadians(projectileAngles[i])), ProjectileType<EchoingFlameCannon_EchoingFlames>(), damage/2, 5, player.whoAmI);
                }
            }
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-8, 0);
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
