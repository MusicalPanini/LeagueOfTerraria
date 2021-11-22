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
    public class TrueIceBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("True Ice Bow");
            Tooltip.SetDefault("");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        string GetWeaponTooltip()
        {
            return "Fires a flurry of slowing arrows after every shot that deal 30% of the original arrow's damage";
        }

        public override void SetDefaults()
        {
            Item.damage = 30;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 24;
            Item.height = 54;
            Item.useAnimation = 25;
            Item.useTime = 5;
            Item.reuseDelay = 20;
            Item.shootSpeed = 10f;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 1;
            Item.value = 160000;
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.useAmmo = AmmoID.Arrow;

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.W, new Volley(this));
            abilityItem.ChampQuote = "Right between the eyes";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (player.itemAnimation != player.itemAnimationMax)
            {
                type = ProjectileType<TrueIceBow_Flurry>();
                damage = (int)(damage * 0.3);
                knockback = 0;
                //int numberProjectiles = 5;
                //    int distance = 24;
                //    for (int i = 0; i < numberProjectiles; i++)
                //    {
                //        Vector2 relPosition = new Vector2(0 - (distance * 2) + (i * distance), 0).RotatedBy(TerraLeague.CalcAngle(player.Center, Main.MouseWorld) + MathHelper.PiOver2);
                //        Vector2 position = new Vector2(player.Center.X + relPosition.X, player.Center.Y + relPosition.Y);
                //        Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 15f);

                //        Projectile.NewProjectile(position, velocity, projType, damage, knockback, player.whoAmI);
                //    }
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<TrueIceChunk>(), 4)
            .AddIngredient(ItemID.HellwingBow, 1)
            .AddIngredient(ItemID.FrostCore, 1)
            .AddTile(TileID.Anvils)
            .Register();
        }

        public override bool CanConsumeAmmo(Player player)
        {
            return !(player.itemAnimation < (player.itemAnimationMax) - 2);
        }
    }
}
