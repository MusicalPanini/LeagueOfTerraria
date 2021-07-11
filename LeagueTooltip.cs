using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.UI;
using Terraria;
using Terraria.ModLoader;

namespace TerraLeague
{
    public static class LeagueTooltip
    {
        public static string MeleeColor{ get { return scalingColor[ScaleType.Melee]; } }
        public static string RangedColor{ get { return scalingColor[ScaleType.Ranged]; } }
        public static string MagicColor{ get { return scalingColor[ScaleType.Magic]; } }
        public static string SummonColor{ get { return scalingColor[ScaleType.Summon]; } }
        public static string MinionsColor{ get { return scalingColor[ScaleType.Minions]; } }
        public static string SentriesColor{ get { return scalingColor[ScaleType.Sentries]; } }
        public static string ArmorColor{ get { return scalingColor[ScaleType.Armor]; } }
        public static string ResistColor{ get { return scalingColor[ScaleType.Resist]; } }
        public static string DefenceColor{ get { return scalingColor[ScaleType.Defence]; } }
        public static string MaxLifeColor{ get { return scalingColor[ScaleType.MaxLife]; } }
        public static string CurLifeColor{ get { return scalingColor[ScaleType.CurLife]; } }
        public static string MaxManaColor{ get { return scalingColor[ScaleType.MaxMana]; } }
        public static string CurManaColor{ get { return scalingColor[ScaleType.CurMana]; } }
        public static string HasteColor{ get { return scalingColor[ScaleType.Haste]; } }
        public static string HealPowerColor{ get { return scalingColor[ScaleType.HealPower]; } }

        private static readonly Dictionary<string, string> keys = new Dictionary<string, string>()
            {
                {"Escape", "Esc"},
                {"PrintScreen", "PrtSc"},
                {"OemTilde", "`"},
                {"D1", "1"},
                {"D2", "2"},
                {"D3", "3"},
                {"D4", "4"},
                {"D5", "5"},
                {"D6", "6"},
                {"D7", "7"},
                {"D8", "8"},
                {"D9", "9"},
                {"D0", "0"},
                {"OemPlus", "="},
                {"OemMinus", "_"},
                {"Insert", "Ins"},
                {"PageUp", "PgUp"},
                {"NumLock", "NumLk"},
                {"Divide", "/"},
                {"Multiply", "*"},
                {"Minus", "-"},

                {"OemOpenBracket", "["},
                {"OemCloseBracket", "]"},
                {"OemPipe", "\\"},
                {"Delete", "Dlt"},
                {"PageDown", "PgDw"},
                {"Plus", "+"},

                {"CapsLock", "Caps"},
                {"OemSemicolon", ";"},
                {"OemQuotes", "'"},

                {"RightShift", "ShiftR"},
                {"OemComma", ","},
                {"OemPeriod", "."},
                {"OemQuestion", "?"},

                {"LeftControl", "CtrlL"},
                {"LeftAlt", "AltL"},
                {"Space", "Spc"},

                {"Mouse1", "M1"},
                {"Mouse2", "M2"},
                {"Mouse3", "M3"},
                {"Mouse4", "M4"},
                {"Mouse5", "M5"},
                {"RightAlt", "AltR"},
                {"RightControl", "CtrlR"},
                {"RightWindows", "WinR"},
                {"LeftWindows", "WinL"},
                {"LeftShift", "ShiftL"},
                {"NumPad1", "NP1"},
                {"NumPad2", "NP2"},
                {"NumPad3", "NP3"},
                {"NumPad4", "NP4"},
                {"NumPad5", "NP5"},
                {"NumPad6", "NP6"},
                {"NumPad7", "NP7"},
                {"NumPad8", "NP8"},
                {"NumPad9", "NP9"},
                {"NumPad0", "NP0"},
                {"Decimal", "NP."},
                {"Scroll", "ScrLk"},
                {"Pause", "PsBrk"},
            };

        private static readonly Dictionary<ScaleType, string> scaledTipName = new Dictionary<ScaleType, string>()
        {
            {ScaleType.Melee, "MEL"},
            {ScaleType.Ranged, "RNG"},
            {ScaleType.Magic, "MAG"},
            {ScaleType.Summon, "SUM"},
            {ScaleType.Minions, "MINs"},
            {ScaleType.Sentries, "SNTs"},
            {ScaleType.Armor, "ARM"},
            {ScaleType.Resist, "RST"},
            {ScaleType.Defence, "DEF"},
            {ScaleType.MaxLife, "MAXLIFE"},
            {ScaleType.CurLife, "LIFE"},
            {ScaleType.MaxMana, "MAXMANA"},
            {ScaleType.CurMana, "MANA"},
            {ScaleType.Haste, "HASTE"},
            {ScaleType.HealPower, "HEAL"},
        };

        private static readonly Dictionary<ScaleType, string> scalingColor = new Dictionary<ScaleType, string>()
        {
            {ScaleType.Melee, "FFA500"},
            {ScaleType.Ranged, "20B2AA"},
            {ScaleType.Magic, "8E70DB"},
            {ScaleType.Summon, "6495ed"},
            {ScaleType.Minions, "4682b4"},
            {ScaleType.Sentries, "4682b4"},
            {ScaleType.Armor, "FFFF00"},
            {ScaleType.Resist, "B0C4DE"},
            {ScaleType.Defence, "A0A0A0"},
            {ScaleType.MaxLife, "A43741"},
            {ScaleType.CurLife, "A43741"},
            {ScaleType.MaxMana, "2E4372"},
            {ScaleType.CurMana, "2E4372"},
            {ScaleType.Haste, "FFDD8F"},
            {ScaleType.HealPower, "008000"},
        };

        /// <summary>
        /// Converts Terrarias Hot Key strings to shorter, cleaner version for interfaces and tooltips
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string ConvertKeyString(ModHotKey key)
        {
            string keyConvertedString;
            string keyString = "N/A";
            if (key.GetAssignedKeys().Count > 0)
                keyString = key.GetAssignedKeys().First();


            if (keys.ContainsKey(keyString))
                keyConvertedString = keys[keyString];
            else
                keyConvertedString = keyString;

            return keyConvertedString;

        }

        public static string CreateColorString(string hexValue, string text)
        {
            var splitText = text.Split('\n');
            string rejoinedText = "";
            for (int i = 0; i < splitText.Length; i++)
            {
                if (i != 0)
                    rejoinedText += "\n";
                rejoinedText += "[c/" + PulseText(ConvertHexToColor(hexValue)).Hex3() + ":" + splitText[i] + "]";
            }

            return rejoinedText;
        }

        public static string CreateColorString(Color color, string text)
        {
            var splitText = text.Split('\n');
            string rejoinedText = "";
            for (int i = 0; i < splitText.Length; i++)
            {
                if (i != 0)
                    rejoinedText += "\n";
                rejoinedText += "[c/" + PulseText(color).Hex3() + ":" + splitText[i] + "]";
            }

            return rejoinedText;
        }

        public static string TooltipValue(int baseValue, bool useHealPower, string additionalText, params Tuple<int, ScaleType>[] scalings)
        {
            string text = "";

            if (ItemUI.extraStats)
            {
                if (baseValue != 0)
                    text += baseValue;

                for (int i = 0; i < scalings.Length; i++)
                {
                    if ((i == 0 && baseValue != 0) || i != 0)
                        text += " + ";
                    text += GetScaledTooltip(scalings[i].Item1, scalings[i].Item2, additionalText);
                }

                if (useHealPower)
                {
                    int heal = baseValue;
                    for (int i = 0; i < scalings.Length; i++)
                    {
                        heal += (int)(GetStat(scalings[i].Item2) * scalings[i].Item1 * 0.01);
                    }

                    text += " + [c/" + PulseText(scalingColor[ScaleType.HealPower]) + ":" + scaledTipName[ScaleType.HealPower] + "(" + (Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().ScaleValueWithHealPower(heal, true) - heal) + additionalText + ")]";
                }
            }
            else
            {
                int statTotal = baseValue;

                for (int i = 0; i < scalings.Length; i++)
                {
                    statTotal += (int)(GetStat(scalings[i].Item2) * scalings[i].Item1 * 0.01);
                }

                if (useHealPower)
                {
                    statTotal = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().ScaleValueWithHealPower(statTotal, true);
                }

                text = statTotal + additionalText;
            }

            return text;
        }

        public static Color ConvertHexToColor(string hexValue)
        {
            if (hexValue != null)
            {
                if (hexValue.Length >= 6)
                {
                    int red = Int32.Parse(hexValue[0] + "" + hexValue[1], System.Globalization.NumberStyles.HexNumber);
                    int green = Int32.Parse(hexValue[2] + "" + hexValue[3], System.Globalization.NumberStyles.HexNumber);
                    int blue = Int32.Parse(hexValue[4] + "" + hexValue[5], System.Globalization.NumberStyles.HexNumber);
                    return new Color(red, green, blue);
                }
            }
            return Color.White;
        }

        public static Color PulseText(Color color)
        {
            float pulse = (float)(int)Main.mouseTextColor / 255f;
            return new Color((byte)(color.R * pulse), (byte)(color.G * pulse), (byte)(color.B * pulse), Main.mouseTextColor);
        }

        public static string PulseText(string hex)
        {
            Color color = ConvertHexToColor(hex);
            float pulse = (float)(int)Main.mouseTextColor / 255f;
            return new Color((byte)(color.R * pulse), (byte)(color.G * pulse), (byte)(color.B * pulse), Main.mouseTextColor).Hex3();
        }

        public static string GetScaledTooltip(int percentScaling, ScaleType scaleType, string additionalText)
        {
            int value = GetStat(scaleType);

            string text = "[c/" + PulseText(scalingColor[scaleType]) + ":" + percentScaling + "% " + scaledTipName[scaleType] + "(" + (int)(value * percentScaling * 0.01) + additionalText + ")]";

            return text;
        }

        private static int GetStat(ScaleType scaleType)
        {
            int value = 0;
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();
            switch (scaleType)
            {
                case ScaleType.Melee:
                    value = modPlayer.MEL;
                    break;
                case ScaleType.Ranged:
                    value = modPlayer.RNG;
                    break;
                case ScaleType.Magic:
                    value = modPlayer.MAG;
                    break;
                case ScaleType.Summon:
                    value = modPlayer.SUM;
                    break;
                case ScaleType.Minions:
                    value = (int)modPlayer.maxMinionsLastStep;
                    break;
                case ScaleType.Sentries:
                    value = (int)Main.LocalPlayer.maxTurrets;
                    break;
                case ScaleType.Armor:
                    value = modPlayer.armorLastStep;
                    break;
                case ScaleType.Resist:
                    value = modPlayer.resistLastStep;
                    break;
                case ScaleType.Defence:
                    value = modPlayer.defenceLastStep;
                    break;
                case ScaleType.MaxLife:
                    value = modPlayer.maxLifeLastStep;
                    break;
                case ScaleType.CurLife:
                    value = modPlayer.GetRealHeathWithoutShield();
                    break;
                case ScaleType.MaxMana:
                    value = Main.LocalPlayer.statManaMax2;
                    break;
                case ScaleType.CurMana:
                    value = Main.LocalPlayer.statMana;
                    break;
                case ScaleType.Haste:
                    value = modPlayer.abilityHasteLastStep;
                    break;
                default:
                    break;
            }

            return value;
        }
    }

    public enum ScaleType
    {
        Melee,
        Ranged,
        Magic,
        Summon,
        Minions,
        Sentries,
        Armor,
        Resist,
        Defence,
        MaxLife,
        CurLife,
        MaxMana,
        CurMana,
        Haste,
        HealPower
    }
}
