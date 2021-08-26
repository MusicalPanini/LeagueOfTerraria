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
    public class EmperoroftheSands : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Emperor of the Sands");
            Tooltip.SetDefault("");
            base.SetStaticDefaults();
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        string GetWeaponTooltip()
        {
            return "Create a solder of sand to fight for you!";
        }

        public override void SetDefaults()
        {
            Item.damage = 11;
            Item.DamageType = DamageClass.Summon;
            Item.mana = 20;
            Item.width = 48;
            Item.height = 48;
            Item.useTime = 36;
            Item.useAnimation = 36;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 1;
            Item.value = 10000;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = new LegacySoundStyle(2, 113);
            Item.shoot = ProjectileType<EmperoroftheSands_SandSolder>();

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.R, new EmperorsDivide(this));
            abilityItem.ChampQuote = "Your emperor shall return";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            position = Main.MouseWorld;

            if (player.altFunctionUse != 2)
            {
                bool pathBlocked = false;
                for (int x = (int)((Main.mouseX + Main.screenPosition.X) / 16) - 1; x < (int)((Main.mouseX + Main.screenPosition.X) / 16) + 1; x++)
                {
                    for (int y = (int)((Main.mouseY + Main.screenPosition.Y) / 16) - 1; y <= (int)((Main.mouseY + Main.screenPosition.Y) / 16) + 1; y++)
                    {
                        if (Main.tile[x, y].CollisionType > 0)
                        {
                            pathBlocked = true;
                            break;
                        }
                    }
                }

                if (!pathBlocked)
                {
                    Projectile proj = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
                    proj.originalDamage = Item.damage;
                }
            }

            return false;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                player.MinionNPCTargetAim(true);
            }
            else
            {
                player.AddBuff(BuffType<SandSolder>(), 10);
            }
            return base.CanUseItem(player);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.Sapphire, 4)
            .AddIngredient(ItemID.SandBlock, 100)
            .AddRecipeGroup("TerraLeague:GoldGroup", 10)
            .AddIngredient(ItemType<Sunstone>(), 10)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}