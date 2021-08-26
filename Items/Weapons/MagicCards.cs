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
    public class MagicCards : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Card Masters Deck");
            Tooltip.SetDefault("");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        string GetWeaponTooltip()
        {
            return "Has a chance to throw a special card" +
                "\n" + LeagueTooltip.CreateColorString("0066ff", "Blue Card") + " - Restore 25 mana on hit" +
                "\n" + LeagueTooltip.CreateColorString("ffff4d", "Yellow Card") + " - Applies 'Stunned'" +
                "\n" + LeagueTooltip.CreateColorString("ff1a1a", "Red Card") + " - Explodes on contact";
        }

        public override void SetDefaults()
        {
            Item.damage = 12;
            Item.width = 24;
            Item.height = 24;
            Item.DamageType = DamageClass.Magic;
            Item.useTime = 28;
            Item.useAnimation = 28;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 2;
            Item.mana = 6;
            Item.value = 3500;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = new LegacySoundStyle(2, 19, Terraria.Audio.SoundType.Sound);
            Item.shootSpeed = 15f;
            Item.shoot = ProjectileType<MagicCards_GreenCard>();
            Item.noMelee = true;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.noUseGraphic = true;

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.Q, new WildCards(this));
            abilityItem.ChampQuote = "Lady luck is smilin'";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override bool CanUseItem(Player player)
        {
            return base.CanUseItem(player);
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (Main.rand.Next(0, 5) == 0)
            {
                switch (Main.rand.Next(0, 3))
                {
                    case 0:
                        type = ProjectileType<MagicCards_RedCard>();
                        knockback *= 2;
                        damage = (int)(damage * 1.25);
                        break;
                    case 1:
                        type = ProjectileType<MagicCards_BlueCard>();
                        damage = (int)(damage * 1.5);
                        break;
                    case 2:
                        type = ProjectileType<MagicCards_YellowCard>();
                        knockback = 0;
                        break;
                    default:
                        break;
                }
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<BrassBar>(), 14)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}
