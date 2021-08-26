using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using TerraLeague.Items.Weapons.Abilities;
using TerraLeague.NPCs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class WujuBlade : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Highlander Ring Sword");
            Tooltip.SetDefault("");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 18;
            Item.width = 48;
            Item.height = 52;
            Item.DamageType = DamageClass.Melee;
            Item.useTime = 16;
            Item.useAnimation = 16;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 2;
            Item.value = 54000;
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.R, new Highlander(this));
            abilityItem.ChampQuote = "Keep up!";
            abilityItem.IsAbilityItem = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<ManaBar>(), 16)
            .AddTile(TileID.Anvils)
            .Register();
            
        }
    }
}
