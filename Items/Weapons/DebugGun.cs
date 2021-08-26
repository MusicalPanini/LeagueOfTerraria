using Microsoft.Xna.Framework;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class DebugGun : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Debug Gun");
            Tooltip.SetDefault("");
        }

        string GetWeaponTooltip()
        {
            return "Does whatever I say it does";
        }

        public override void SetDefaults()
        {
            Item.damage = 100;
            Item.DamageType = DamageClass.Magic;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.width = 40;
            Item.height = 24;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.shootSpeed = 12f;
            Item.noMelee = true;
            Item.knockBack = 0;
            Item.value = 1;
            Item.rare = ItemRarityID.Expert;
            Item.scale = 0.9f;
            Item.shoot = ProjectileType<SolariSet_LargeSolarSigil>();
            //Item.UseSound = new Terraria.Audio.LegacySoundStyle(2, 12);
            Item.autoReuse = false;

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            //abilityItem.SetAbility(AbilityType.Q, new Defile(this));
            //abilityItem.SetAbility(AbilityType.W, new Defile(this));
            //abilityItem.SetAbility(AbilityType.E, new Defile(this));
            //abilityItem.SetAbility(AbilityType.R, new Requiem(this));
            abilityItem.ChampQuote = "You probably shouldn't have this";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position = Main.MouseWorld;
            velocity = Vector2.Zero;
            Item.channel = false;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, 0);
        }
    }
}
