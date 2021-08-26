using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Items
{
    public class TerraLeaguePrefixGLOBAL : GlobalItem
    {
        public bool Transedent;
        public byte Armor;
        public byte Resist;
        public byte HealPower;
        public byte MEL;
        public byte RNG;
        public byte MAG;
        public byte SUM;

        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }

        public TerraLeaguePrefixGLOBAL()
        {
            Transedent = false;
            Armor = 0;
            Resist = 0;
            HealPower = 0;
            MEL = 0;
            RNG = 0;
            MAG = 0;
            SUM = 0;
        }

        public override bool PreReforge(Item item)
        {
            Transedent = false;
            Armor = 0;
            Resist = 0;
            HealPower = 0;
            MEL = 0;
            RNG = 0;
            MAG = 0;
            SUM = 0;

            return base.PreReforge(item);
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (!item.social && item.prefix > 0)
            {
                if (Transedent)
                {
                    TooltipLine line = new TooltipLine(Mod, "PrefixCDR", "+5 ability, item, and summoner spell haste")
                    {
                        isModifier = true
                    };
                    tooltips.Add(line);
                }
                if (Armor > 0)
                {
                    TooltipLine line = new TooltipLine(Mod, "PrefixArmor", "+" + (Armor) + " armor")
                    {
                        isModifier = true
                    };
                    tooltips.Add(line);
                }
                if (Resist > 0)
                {
                    TooltipLine line = new TooltipLine(Mod, "PrefixResist", "+" + (Resist) + " resist")
                    {
                        isModifier = true
                    };
                    tooltips.Add(line);
                }
                if (HealPower > 0)
                {
                    TooltipLine line = new TooltipLine(Mod, "PrefixHealPower", "+" + (HealPower) + "% healing power")
                    {
                        isModifier = true
                    };
                    tooltips.Add(line);
                }
                if (MEL > 0)
                {
                    TooltipLine line = new TooltipLine(Mod, "PrefixMEL", "+" + (MEL) + " MEL")
                    {
                        isModifier = true
                    };
                    tooltips.Add(line);
                }
                if (RNG > 0)
                {
                    TooltipLine line = new TooltipLine(Mod, "PrefixRNG", "+" + (RNG) + " RNG")
                    {
                        isModifier = true
                    };
                    tooltips.Add(line);
                }
                if (MAG > 0)
                {
                    TooltipLine line = new TooltipLine(Mod, "PrefixMAG", "+" + (MAG) + " MAG")
                    {
                        isModifier = true
                    };
                    tooltips.Add(line);
                }
                if (SUM > 0)
                {
                    TooltipLine line = new TooltipLine(Mod, "PrefixSUM", "+" + (SUM) + " SUM")
                    {
                        isModifier = true
                    };
                    tooltips.Add(line);
                }
            }

            base.ModifyTooltips(item, tooltips);
        }

        public override void UpdateEquip(Item item, Player player)
        {
            // Prefixes
            if (Transedent)
            {
                player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 5;
                player.GetModPlayer<PLAYERGLOBAL>().itemHaste += 5;
                player.GetModPlayer<PLAYERGLOBAL>().summonerHaste += 5;
            }
            else if (Armor > 0)
            {
                player.GetModPlayer<PLAYERGLOBAL>().armor += Armor;
            }
            else if (Resist > 0)
            {
                player.GetModPlayer<PLAYERGLOBAL>().resist += Resist;
            }
            else if (HealPower > 0)
            {
                player.GetModPlayer<PLAYERGLOBAL>().healPower += 0.01 * HealPower;
            }
            else if (MEL > 0)
            {
                player.GetModPlayer<PLAYERGLOBAL>().BonusMEL += MEL;
            }
            else if (RNG > 0)
            {
                player.GetModPlayer<PLAYERGLOBAL>().BonusRNG += RNG;
            }
            else if (MAG > 0)
            {
                player.GetModPlayer<PLAYERGLOBAL>().BonusMAG += MAG;
            }
            else if (SUM > 0)
            {
                player.GetModPlayer<PLAYERGLOBAL>().BonusSUM += SUM;
            }

            base.UpdateEquip(item, player);
        }

        public override GlobalItem Clone(Item item, Item itemClone)
        {
            TerraLeaguePrefixGLOBAL myClone = (TerraLeaguePrefixGLOBAL)base.Clone(item, itemClone);
            myClone.Transedent = Transedent;
            myClone.Armor = Armor;
            myClone.Resist = Resist;
            myClone.HealPower = HealPower;
            myClone.MEL = MEL;
            myClone.RNG = RNG;
            myClone.MAG = MAG;
            myClone.SUM = SUM;
            return myClone;
        }

        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write(Transedent);
            writer.Write(Armor);
            writer.Write(Resist);
            writer.Write(HealPower);
            writer.Write(MEL);
            writer.Write(RNG);
            writer.Write(MAG);
            writer.Write(SUM);
        }

        public override void NetReceive(Item item, BinaryReader reader)
        {
            Transedent = reader.ReadBoolean();
            Armor = reader.ReadByte();
            Resist = reader.ReadByte();
            HealPower = reader.ReadByte();
            MEL = reader.ReadByte();
            RNG = reader.ReadByte();
            MAG = reader.ReadByte();
            SUM = reader.ReadByte();
        }
    }
}
