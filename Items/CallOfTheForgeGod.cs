using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using TerraLeague.Items.CustomItems;
using TerraLeague.Projectiles;
using TerraLeague.Projectiles.Vanity;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items
{
    public class CallOfTheForgeGod : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Call of the Forge God");
            Tooltip.SetDefault("");
            base.SetStaticDefaults();
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips[0].overrideColor = MasterworkItem.MasterColor;
            TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Tooltip0" && x.mod == "Terraria");
            if (tt != null)
            {
                int pos = tooltips.IndexOf(tt);

                string text = "Upgrades first equipped accessory into a " + LeagueTooltip.CreateColorString(MasterworkItem.MasterColor, "Masterwork") + " Item.\n";

                if (!Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().HasMasterworkEquipped)
                {
                    if (Main.LocalPlayer.armor[3].ModItem is MasterworkItem masterItem)
                    {
                        if (!masterItem.IsMasterWorkItem)
                        {
                            text += "Current upgrade:" +
                                "\n" + LeagueTooltip.CreateColorString(Passive.PassiveMainColor, masterItem.Item.Name) + " -> " + LeagueTooltip.CreateColorString(MasterworkItem.MasterColor, masterItem.MasterworkName) + "\n";
                            text += masterItem.MasterworkTooltip();
                        }
                        else
                        {
                            text += "Item is already a Masterwork.";
                        }
                    }
                    else
                    {
                        text += "No item to upgrade in first accessory slot.";
                    }
                }
                else
                {
                    text += "Masterwork item already equipped.";
                }

                string[] itemTooltip = text.Split('\n');
                for (int i = 0; i < itemTooltip.Length; i++)
                {
                    tooltips.Insert(pos + i, new TooltipLine(TerraLeague.instance, "Tooltip" + (i), itemTooltip[i]));
                }
            }

            base.ModifyTooltips(tooltips);
        }

        public override bool CanUseItem(Player player)
        {
            if (player.whoAmI == Main.LocalPlayer.whoAmI)
            {
                PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

                if (!modPlayer.HasMasterworkEquipped)
                {
                    if (modPlayer.Player.armor[3].ModItem is MasterworkItem masterItem)
                    {
                        if (!masterItem.IsMasterWorkItem)
                        {
                            masterItem.ToggleMasterwork();
                            Projectile.NewProjectileDirect(null, player.MountedCenter + new Vector2(player.direction * 64, -56), Vector2.Zero, ProjectileType<ForgeGod>(), 0, 0, player.whoAmI, masterItem.Item.type);
                            Item.stack--;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.FallenStar);
            Item.rare = ItemRarityID.Yellow;
            Item.alpha = 0;
            Item.width = 32;
            Item.height = 20;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.maxStack = 1;
            Item.notAmmo = true;
            Item.ammo = AmmoID.None;
            Item.shoot = ItemID.None;
        }
    }
}
