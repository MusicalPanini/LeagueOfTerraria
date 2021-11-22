using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items.SummonerSpells
{
    abstract public class SummonerSpell : ModItem
    {
        public static Dictionary<string, int> SummonerID = new Dictionary<string, int>()
        {
            { new BarrierRune().GetType().Name, ModContent.ItemType<BarrierRune>()},
            { new ClairvoyanceRune().GetType().Name, ModContent.ItemType<ClairvoyanceRune>()},
            { new ClarityRune().GetType().Name, ModContent.ItemType<ClarityRune>()},
            { new CleanseRune().GetType().Name, ModContent.ItemType<CleanseRune>()},
            { new ExhaustRune().GetType().Name, ModContent.ItemType<ExhaustRune>()},
            { new FlashRune().GetType().Name, ModContent.ItemType<FlashRune>()},
            { new GarrisonRune().GetType().Name, ModContent.ItemType<GarrisonRune>()},
            { new GhostRune().GetType().Name, ModContent.ItemType<GhostRune>()},
            { new HealRune().GetType().Name, ModContent.ItemType<HealRune>()},
            { new IgniteRune().GetType().Name, ModContent.ItemType<IgniteRune>()},
            { new LiftRune().GetType().Name, ModContent.ItemType<LiftRune>()},
            { new ReviveRune().GetType().Name, ModContent.ItemType<ReviveRune>()},
            { new SmiteRune().GetType().Name, ModContent.ItemType<SmiteRune>()},
            { new SurgeRune().GetType().Name, ModContent.ItemType<SurgeRune>()},
            { new SyphonRune().GetType().Name, ModContent.ItemType<SyphonRune>()},
            { new VanishRune().GetType().Name, ModContent.ItemType<VanishRune>()},
            { new TeleportRune().GetType().Name, ModContent.ItemType<TeleportRune>()}
        };

        static internal SummonerSpellsPacketHandler PacketHandler = new SummonerSpellsPacketHandler(6);

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.FallenStar);
            Item.rare = ItemRarityID.Orange;
            Item.width = 20;
            Item.height = 26;
            Item.maxStack = 1;
            Item.notAmmo = true;
            Item.ammo = AmmoID.None;
            Item.shoot = ItemID.None;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Tooltip0" && x.mod == "Terraria");
            if (tt != null)
            {
                int pos = tooltips.IndexOf(tt);

                string text = LeagueTooltip.CreateColorString(LeagueTooltip.ManaReductionColor, "Left or Right click to replace your Left or Right Summoner Spell") +
                    "\nEffect: " + GetTooltip();
                if (GetRawCooldown() > 0)
                    text += "\n" + GetCooldown() + " second cooldown";
                TooltipLine tip = new TooltipLine(TerraLeague.instance, "Tooltip0", text);
                tooltips[pos] = tip;
            }

            base.ModifyTooltips(tooltips);
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.whoAmI == Main.LocalPlayer.whoAmI)
            {
                PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

                if (modPlayer.sumSpells.Where(x => x.GetType() == this.GetType()).Count() > 0)
                    return false;
                else
                {
                    if (player.altFunctionUse == 2)
                    {
                        modPlayer.sumSpells[1] = (SummonerSpell)ModContent.GetModItem(SummonerSpell.SummonerID[GetType().Name]);
                    }
                    else
                    {
                        modPlayer.sumSpells[0] = (SummonerSpell)ModContent.GetModItem(SummonerSpell.SummonerID[GetType().Name]);
                    }
                    Item.stack = 0;
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        PacketHandler.SendSyncSpells(-1, player.whoAmI, modPlayer.sumSpells[0].Item.type, modPlayer.sumSpells[1].Item.type, player.whoAmI);
                    return base.CanUseItem(player);
                }
            }
            return false;
        }

        /// <summary>
        /// Gets the tooltip
        /// </summary>
        /// <returns></returns>
        virtual public string GetTooltip()
        {
            return "This Summoner Spell has not tooltip";
        }

        /// <summary>
        /// Gets the Cooldown of the spell adjusted with cooldown reduction
        /// </summary>
        /// <returns></returns>
        virtual public float GetCooldown()
        {
            return (float)Math.Round(GetRawCooldown() * (Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().SummonerCdrLastStep), 1);
        }

        /// <summary>
        /// Performs the summoner spells action
        /// </summary>
        /// <param name="player">Player casting the spell</param>
        /// <param name="spellSlot">Which slot the spell is in</param>
        abstract public void DoEffect(Player player, int spellSlot);

        /// <summary>
        /// Sets the cooldown of the specified slot
        /// </summary>
        /// <param name="player"></param>
        /// <param name="spellSlot"></param>
        protected void SetCooldowns(Player player, int spellSlot)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            modPlayer.sumCooldowns[spellSlot - 1] = (int)(GetCooldown() * 60);
        }

        /// <summary>
        /// The path for the spells icon
        /// </summary>
        abstract public string GetIconTexturePath();

        /// <summary>
        /// Name of the summoner spell
        /// </summary>
        abstract public string GetSpellName();

        /// <summary>
        /// The spells cooldown in seconds
        /// </summary>
        abstract public int GetRawCooldown();
    }
}
