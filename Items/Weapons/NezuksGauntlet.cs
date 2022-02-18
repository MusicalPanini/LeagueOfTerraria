using Microsoft.Xna.Framework;
using TerraLeague.Items.Weapons.Abilities;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class NezuksGauntlet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ne'Zuk's Gauntlet");
            Tooltip.SetDefault("");
            base.SetStaticDefaults();
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 52;
            Item.mana = 7;
            Item.DamageType = DamageClass.Ranged;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.width = 32;
            Item.height = 34;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.knockBack = 4f;
            Item.value = 55000;
            Item.rare = ItemRarityID.LightRed;
            Item.shootSpeed = 16f;
            Item.UseSound = new Terraria.Audio.LegacySoundStyle(2, 73);
            Item.shoot = ProjectileType<NezuksGauntlet_MysticShot>();

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.W, new EssenceFlux(this));
            abilityItem.SetAbility(AbilityType.E, new ArcaneShift(this));
            abilityItem.ChampQuote = "The gauntlet's for show... the talent's all me";
            abilityItem.IsAbilityItem = true;
        }
    }

    public class GauntletGlobalNPC : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.netID == NPCID.Mimic)
                npcLoot.Add(ItemDropRule.Common(ItemType<NezuksGauntlet>(), 10));
        }
    }
}
