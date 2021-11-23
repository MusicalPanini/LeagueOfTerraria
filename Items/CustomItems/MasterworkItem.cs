using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace TerraLeague.Items.CustomItems
{
    abstract public class MasterworkItem : LeagueItem
    {
        public bool IsMasterWorkItem { get; private set; } = false;
        public const string MasterworkIconPath = "TerraLeague/Textures/UI/MasterWorkIcon";
        Texture2D MasterWorkIcon = ModContent.Request<Texture2D>(MasterworkIconPath).Value;
        public static Color MasterColor { get { return LeagueTooltip.PulseText(Color.OrangeRed); } }

        public override void SaveData(TagCompound tag)
        {
            tag.Add("Masterwork", IsMasterWorkItem );
        }

        public override void LoadData(TagCompound tag)
        {
            IsMasterWorkItem = tag.GetBool("Masterwork");
            ToggleMasterwork(IsMasterWorkItem);
        }


        //public abstract string NonMasterworkName { get; }
        public abstract string MasterworkName { get; }

        public void ToggleMasterwork(bool toggleOn = true)
        {
            if (toggleOn)
            {
                Item.SetNameOverride(MasterworkName);
            }
            else
            {
                Item.ClearNameOverride();

            }

            IsMasterWorkItem = toggleOn;
        }

        /// <summary>
        /// Replaces the base tooltip on the item if the item is a Masterwork Item
        /// </summary>
        /// <returns></returns>
        public abstract string MasterworkTooltip();

        /// <summary>
        /// Runs during Update Accessory if the item is a Masterwork item
        /// </summary>
        public abstract void UpdateMasterwork(Player player);

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (IsMasterWorkItem)
                UpdateMasterwork(player);
            player.GetModPlayer<PLAYERGLOBAL>().HasMasterworkEquipped |= IsMasterWorkItem;
            base.UpdateAccessory(player, hideVisual);
        }

        public override bool CanEquipAccessory(Player player, int slot, bool modded)
        {
            if (!player.GetModPlayer<PLAYERGLOBAL>().HasMasterworkEquipped || !IsMasterWorkItem)
            {
                return base.CanEquipAccessory(player, slot, modded);
            }
            else
            {
                if (player.armor[slot].ModItem is MasterworkItem masterItem)
                {
                    if (masterItem.IsMasterWorkItem)
                    {
                        return base.CanEquipAccessory(player, slot, modded);
                    }
                }
            }
            return false;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            base.ModifyTooltips(tooltips);

            if (IsMasterWorkItem)
                tooltips[0].overrideColor = MasterColor;

            TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Tooltip0" && x.mod == "Terraria");
            if (tt != null)
            {
                if (IsMasterWorkItem)
                {
                    //tooltips.Insert(1, new TooltipLine(TerraLeague.instance, "IsMasterwork", LeagueTooltip.CreateColorString(Color.OrangeRed, "MASTERWORK")));
                
                    int startPos = tooltips.FindIndex(x => x.Name == "Tooltip0" && x.mod == "Terraria");
                    string[] masterworkLines = MasterworkTooltip().Split('\n');

                    for (int i = 0; i < masterworkLines.Length; i++)
                    {
                        tooltips.Insert(startPos + i, new TooltipLine(TerraLeague.instance, "MasterworkTooltip" + i, masterworkLines[i])); 
                    }
                        
                    for (int i = 0; i < tooltips.Count; i++)
                    {
                        if (tooltips[i].Name.Contains("Tooltip") && tooltips[i].mod == "Terraria")
                        {
                            tooltips.RemoveAt(i);
                            i--;
                        }
                    }
                }
            }
        }

        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            if (IsMasterWorkItem)
            {
                TerraLeague.GetTextureIfNull(ref MasterWorkIcon, "TerraLeague/Textures/UI/MasterWorkIcon");

                Vector2 center = new Vector2(position.X + (frame.Width * scale / 2), position.Y + (frame.Height * scale / 2));

                spriteBatch.Draw(MasterWorkIcon, center - new Vector2(16, 16), new Rectangle(0, 0, 32, 32), drawColor, 0, origin, 1, SpriteEffects.None, 0);
            }

            base.PostDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
        }
    }
}
