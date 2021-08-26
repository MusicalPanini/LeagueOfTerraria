using Microsoft.Xna.Framework;
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
    public class StarfireSpellbladesAscended : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starfire Spellblade");
            Tooltip.SetDefault("");
        }

        string GetWeaponTooltip()
        {
            return "You have ascended!" +
                "\nFire a wave of starfire that deals " +
                LeagueTooltip.TooltipValue((int)(Item.damage * 0.75), false, "",
                new System.Tuple<int, ScaleType>(30, ScaleType.Melee),
                new System.Tuple<int, ScaleType>(50, ScaleType.Summon)
                ) + " melee damage";
        }

        public override void SetDefaults()
        {
            Item.damage = 110;
            Item.width = 56;
            Item.height = 56;       
            Item.DamageType = DamageClass.Melee;
            Item.useTime = 36;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6;
            Item.value = 200000;
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<StarfireSpellblades_Firewave>();
            Item.shootSpeed = 8;

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.R, new DivineJudgement(this));
            abilityItem.ChampQuote = "Behold, the righteous flame!";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            //Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 16f);
            damage = (int)(Item.damage * 0.75) + (int)(player.GetModPlayer<PLAYERGLOBAL>().MEL * 0.3) + (int)(player.GetModPlayer<PLAYERGLOBAL>().SUM * 0.5);
            int numberProjectiles = 24;
            float startingAngle = 24;
            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.ToRadians(startingAngle));
                startingAngle -= 2f;
                Projectile proj = Projectile.NewProjectileDirect(source, position, perturbedSpeed, type, damage, knockback, player.whoAmI, i);
            }

            return false;
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Daybreak, 60);
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            Dust dust = Dust.NewDustDirect(hitbox.TopLeft(), hitbox.Width, hitbox.Height, DustID.GemTopaz, 0, 0, 100, default, 0.7f);
            dust.noGravity = true;
            base.MeleeEffects(player, hitbox);
        }

        public override void UpdateInventory(Player player)
        {
            if (player.GetModPlayer<PLAYERGLOBAL>().AscensionStacks != 6)
            {
                int prefix = Item.prefix;
                Item.SetDefaults(ModContent.ItemType<StarfireSpellblades>());
                Item.prefix = prefix;
            }

            base.UpdateInventory(player);
        }

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            int prefix = Item.prefix;
            Item.SetDefaults(ModContent.ItemType<StarfireSpellblades>());
            Item.prefix = prefix;

            base.Update(ref gravity, ref maxFallSpeed);
        }
    }
}
