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
    public class FishBones : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fish Bones");
            Tooltip.SetDefault("");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        string GetWeaponTooltip()
        {
            return "Kills grant 'EXCITED!'" +
                "\nEXCITED increase firerate";
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.damage = 35;
            Item.DamageType = DamageClass.Ranged;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3;
            Item.value = 120000;
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item11;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.RocketI;
            Item.shootSpeed = 6;
            Item.useAmmo = AmmoID.Rocket;

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.R, new SuperMegaDeathRocket(this));
            abilityItem.ChampQuote = "BYE BYE";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            int ammoItem = -1;
            for (int i = 0; i < 4; i++)
            {
                if (player.inventory[54 + i].ammo == AmmoID.Rocket)
                {
                    ammoItem = player.inventory[54 + i].type;
                    break;
                }
            }
            try
            {
                if (ammoItem == -1)
                {
                    ammoItem = player.inventory.Where(x => x.ammo == AmmoID.Rocket).First().type;
                    type = AmmoID.Sets.SpecificLauncherAmmoProjectileMatches[ItemID.RocketLauncher][ammoItem];
                }
                else
                {
                    type = AmmoID.Sets.SpecificLauncherAmmoProjectileMatches[ItemID.RocketLauncher][ammoItem];
                }
            }
            catch (System.Exception)
            {
                type = 10;
            }

            if (ammoItem == -1)
            {
                type = 10;
            }

            position.Y -= 8;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-65, -15);
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage, ref float flat)
        {
            if (player.GetModPlayer<PLAYERGLOBAL>().excited)
                damage *= 1.5f;

        }

        public override float UseSpeedMultiplier(Player player)
        {
            if (player.GetModPlayer<PLAYERGLOBAL>().excited)
            {
                return base.UseSpeedMultiplier(player) * 1.5f;
            }
            else
            {
                return base.UseSpeedMultiplier(player);
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.IllegalGunParts, 1)
            .AddIngredient(ItemID.Megashark, 1)
            .AddIngredient(ItemID.RocketLauncher, 1)
            .AddIngredient(ItemID.SharkFin, 5)
            .AddIngredient(ItemID.SoulofMight, 20)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
    }
}
