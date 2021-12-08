using Microsoft.Xna.Framework;
using System;
using TerraLeague.Buffs;
using TerraLeague.Items.Weapons.Abilities;
using TerraLeague.NPCs;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class MercuryHammer : ModItem
    {
        public override bool OnlyShootOnSwing => true;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mercury Hammer");
            Tooltip.SetDefault("");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        string GetWeaponTooltip()
        {
            return "Gain " + LeagueTooltip.TooltipValue(2, false, "", new Tuple<int, ScaleType>(2, ScaleType.MaxMana)) + " mana on hit";
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 36;
            Item.width = 62;
            Item.height = 62;
            Item.DamageType = DamageClass.Melee;
            Item.useTime = 40;
            Item.useAnimation = 40;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5;
            Item.value = 10000;
            Item.rare = ItemRarityID.Orange;
            //Item.hammer = 150;
            Item.UseSound = SoundID.Item1;
            Item.scale = 1f;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.channel = true;
            Item.shoot = ProjectileType<MercuryHammer_LightningField>();

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.R, new MercuryTransform(this, MercuryType.Cannon));
            abilityItem.ChampQuote = "Strength through progress";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override bool? UseItem(Player player)
        {
            return base.UseItem(player);
        }

        public override bool CanShoot(Player player)
        {
            return (player.ownedProjectileCounts[ProjectileType<MercuryHammer_LightningField>()] == 0);
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            player.ManaEffect((int)(2 + player.statManaMax2 * 0.02));
            player.statMana += (int)(2 + player.statManaMax2 * 0.02);
            base.OnHitNPC(player, target, damage, knockBack, crit);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<PrototypeHexCore>(), 2)
            .AddIngredient(ItemID.IllegalGunParts, 1)
            .AddIngredient(ItemID.MeteoriteBar, 10)
            .AddIngredient(ItemID.HellstoneBar, 10)
            .AddTile(TileID.Anvils)
            .Register();
        }

        public override void OnCraft(Recipe recipe)
        {
            MercuryCannon item = new MercuryCannon();
            item.Item.SetDefaults(ItemType<MercuryCannon>());
            item.Item.Prefix(-1);
            Item.GetGlobalItem<MercuryWeapon>().CannonPrefix = item.Item.prefix;
            
            base.OnCraft(recipe);
        }
    }
}
