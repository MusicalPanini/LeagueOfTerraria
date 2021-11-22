using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
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
    public class Whisper : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Whisper");
            Tooltip.SetDefault("");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        string GetWeaponTooltip()
        {
            return "Fire 4 shots before having to reload" +
                "\nThe 4th shot will deal 2x damage and crit" +
                "\nIs unaffected by attack speed" +
                "\nInstead, will convert 1% " + LeagueTooltip.CreateColorString(LeagueTooltip.RngAtkSpdColor, "ATS") + " into 1 damage";
        }

        public override void SetDefaults()
        {
            Item.damage = 140;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 44;
            Item.height = 20;
            Item.useAnimation = 80;
            Item.useTime = 80;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true; 
            Item.knockBack = 4;
            Item.value = 100000;
            Item.rare = ItemRarityID.Pink;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 1f;
            Item.useAmmo = AmmoID.Bullet;
            Item.autoReuse = true;

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.Q, new DancingGrenade(this));
            abilityItem.ChampQuote = "Prepare... for your finale";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override bool CanUseItem(Player player)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (modPlayer.WhisperShotsLeft == 0)
                return false;
            return base.CanUseItem(player);
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage, ref float flat)
        {
            flat += (int)(player.GetModPlayer<PLAYERGLOBAL>().rangedAttackSpeed * 100f) - 100;
        }

        public override float UseSpeedMultiplier(Player player)
        {
            return 1;
        }
        


        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            if (modPlayer.WhisperShotsLeft == 1)
            {
                type = ProjectileType<Whisper_ForthShot>();
                damage *= 2;
            }
            else
            {
                type = ProjectileType<Whisper_Shot>();
            }
            SetStatsPostShoot(player);
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-7, 0);
        }

        public void SetStatsPostShoot(Player player)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            modPlayer.WhisperShotsLeft--;
            if (modPlayer.WhisperShotsLeft == 0)
                modPlayer.ReloadTimer = 160;
            else
                modPlayer.ReloadTimer = 320;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<HarmonicBar>(), 16)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
    }
}
