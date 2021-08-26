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
	public class StoneweaversStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stoneweaver's Staff");
            Tooltip.SetDefault("");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        string GetWeaponTooltip()
        {
            return "Uses Stone as ammo";
        }

        public override void SetDefaults()
        {
            Item.damage = 17;
            Item.mana = 4;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.DamageType = DamageClass.Magic;
            Item.useTime = 35;
            Item.useAnimation = 35;
            Item.noMelee = true;
            Item.knockBack = 2;
            Item.value = 10000;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item8;
            Item.shoot = ProjectileType<StoneweaversStaff_WeaversStone>();
            Item.shootSpeed = 12f;
            Item.autoReuse = true;
            Item.useAmmo = ItemType<BlackIceChunk>();

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.Q, new ThreadedVolley(this));
            abilityItem.ChampQuote = "Throw another rock!";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override bool CanUseItem(Player player)
        {
            return player.HasAmmo(Item, false);
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, -6);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddRecipeGroup("TerraLeague:GoldGroup", 10)
            .AddIngredient(ItemID.Sandstone, 50)
            .AddIngredient(ItemType<Sunstone>(), 10)
            .AddTile(TileID.Anvils)
            .Register();
            
        }
    }
}
