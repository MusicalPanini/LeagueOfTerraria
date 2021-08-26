using Microsoft.Xna.Framework;
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
    public class DarksteelBattleaxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Darksteel Battleaxe");
            Tooltip.SetDefault("");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        string GetWeaponTooltip()
        {
            return "Struck enemies will start to 'Hemorrhage'" +
                "\nHemorrage stacks up to 5 times";
        }

        public override void SetDefaults()
        {
            Item.damage = 28;
            Item.width = 54;
            Item.height = 44;
            Item.DamageType = DamageClass.Melee;
            Item.useTime = 34;
            Item.useAnimation = 34;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5;
            Item.value = 6000;
            Item.rare = ItemRarityID.Orange;
            Item.axe = 30;
            Item.UseSound = SoundID.Item1;
            Item.scale = 1.2f;
            Item.autoReuse = true;

            AbilityItemGLOBAL abilityItem = Item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.Q, new Decimate(this));
            abilityItem.ChampQuote = "Death by my hand";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            int stacks = target.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().HemorrhageStacks;

            if (stacks < 4)
            {
                target.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().HemorrhageStacks++;

                if (Main.netMode == NetmodeID.MultiplayerClient)
                    TerraLeagueNPCsGLOBAL.PacketHandler.SendSyncStats(Main.LocalPlayer.whoAmI, -1, 4, target.whoAmI, target.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().HemorrhageStacks);
            }

            target.AddBuff(BuffType<Hemorrhage>(), 300);
            base.OnHitNPC(player, target, damage, knockBack, crit);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<DarksteelBar>(), 16)
            .AddTile(TileID.Anvils)
            .Register();
            
        }
    }
}
