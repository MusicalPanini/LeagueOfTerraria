using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using TerraLeague.Buffs;
using TerraLeague.Items.Weapons.Abilities;
using Terraria.GameContent.Creative;

namespace TerraLeague.Items.Weapons
{
    public class ArcaneEnergy : ModItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 4));
            DisplayName.SetDefault("Arcane Energy");
            Tooltip.SetDefault("");
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        string GetWeaponTooltip()
        {
            return "Gains 100% extra damage and range for each second you charge it" +
                    "\nMax of 1000%";
        }

        public override void SetDefaults()
        {
            Item.damage = 75;
            Item.width = 32;
            Item.height = 32;
            Item.DamageType = DamageClass.Magic;
            Item.useAnimation = 32;
            Item.useTime = 32;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.knockBack = 5;
            Item.value = 40000;
            Item.rare = ItemRarityID.LightRed;
            Item.shootSpeed = 12f;
            Item.shoot = ProjectileType<ArcaneEnergy_PulseControl>();
            Item.useTurn = true;
            Item.noUseGraphic = true;
            Item.mana = 40;
            Item.autoReuse = false;
            Item.channel = true;
            Item.noMelee = true;
            Item.UseSound = new LegacySoundStyle(2, 82, Terraria.Audio.SoundType.Sound);

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.E, new ShockingOrb(this));
            abilityItem.SetAbility(AbilityType.R, new RiteOfTheArcane(this));
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.ChampQuote = "Behold my power";
            abilityItem.IsAbilityItem = true;
        }

        public override bool CanUseItem(Player player)
        {
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemType<Sunstone>(), 20)
                .AddIngredient(ItemID.Chain, 4)
                .AddIngredient(ItemID.SoulofMight, 10)
                .AddIngredient(ItemID.FallenStar, 10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
