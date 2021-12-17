using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using System;
using Terraria.ID;
using TerraLeague.Items.SummonerSpells;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.Boots;
using static Terraria.ModLoader.ModContent;
using TerraLeague.Items.Weapons;
using Terraria.GameContent;
using TerraLeague.Common.ModSystems;
using System.Collections.Generic;

namespace TerraLeague.UI
{
    internal class ItemUI : UIState
    {
        public UIElement MainPanel;
        public UISummonerPanel SummonerPanel;
        public UIItemPanel ItemPanel;
        public UIStatPanel StatPanel;
        public static bool visible = true;
        public static bool extraStats = false;
        public static bool panelLocked = false;

        public override void OnInitialize()
        {
            ItemPanel = new UIItemPanel();
            SummonerPanel = new UISummonerPanel(700, 700, 99, 54);
            StatPanel = new UIStatPanel(150, 54);

            Append(SummonerPanel);
            Append(ItemPanel);
            Append(StatPanel);
        }

        public override void Update(GameTime gameTime)
        {
            StatPanel.extraStats = extraStats;

            if (!ItemPanel.GetDimensions().ToRectangle().Intersects(GetDimensions().ToRectangle()))
            {
                var parentSpace = GetDimensions().ToRectangle();
                ItemPanel.Left.Pixels = Utils.Clamp(ItemPanel.Left.Pixels, 0, parentSpace.Right - ItemPanel.Width.Pixels);
                ItemPanel.Top.Pixels = Utils.Clamp(ItemPanel.Top.Pixels, 0, parentSpace.Bottom - ItemPanel.Height.Pixels);
            }
            SummonerPanel.Width.Pixels = 108;
            ItemPanel.Recalculate();
            SummonerPanel.Recalculate();
            Recalculate();
            base.Update(gameTime);
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            Player player = Main.LocalPlayer;
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            if (player.GetModPlayer<PLAYERGLOBAL>().currentGun != 0 && !Main.playerInventory)
            {
                int left = (int)(Main.screenWidth - 92);
                int top = (int)(Main.screenHeight - 386);
                Rectangle destRec = new Rectangle(left, top, 48, 232);

                Texture2D texture = Request<Texture2D>("TerraLeague/UI/LunariAmmoBar").Value;
                Rectangle sourRec = new Rectangle(0, 0, 48, 232);
                Main.spriteBatch.Draw(texture, destRec, sourRec, Color.White);

                Texture2D texture2 = Request<Texture2D>("TerraLeague/UI/SmallBlank_V").Value;
                Rectangle sourRec2 = new Rectangle(0, 0, 8, 8);
                Color color;
                Rectangle ammoBarPos;
                int ammoBarHeight;

                // Calibrum
                color = new Color(141, 252, 245);
                ammoBarHeight = (int)(216 * (modPlayer.calibrumAmmo / 100f));
                ammoBarPos = new Rectangle(left + 4, top + 8 + (216 - ammoBarHeight), 8, ammoBarHeight);
                Main.spriteBatch.Draw(texture2, ammoBarPos, sourRec2, color);

                color = new Color(216, 0, 32);
                ammoBarHeight = (int)(216 * (modPlayer.severumAmmo / 100f));
                ammoBarPos = new Rectangle(left + 12, top + 8 + (216 - ammoBarHeight), 8, ammoBarHeight);
                Main.spriteBatch.Draw(texture2, ammoBarPos, sourRec2, color);

                color = new Color(200, 37, 255);
                ammoBarHeight = (int)(216 * (modPlayer.gravitumAmmo / 100f));
                ammoBarPos = new Rectangle(left + 20, top + 8 + (216 - ammoBarHeight), 8, ammoBarHeight);
                Main.spriteBatch.Draw(texture2, ammoBarPos, sourRec2, color);

                color = new Color(0, 148, 255);
                ammoBarHeight = (int)(216 * (modPlayer.infernumAmmo / 100f));
                ammoBarPos = new Rectangle(left + 28, top + 8 + (216 - ammoBarHeight), 8, ammoBarHeight);
                Main.spriteBatch.Draw(texture2, ammoBarPos, sourRec2, color);

                color = new Color(255, 255, 255);
                ammoBarHeight = (int)(216 * (modPlayer.crescendumAmmo / 100f));
                ammoBarPos = new Rectangle(left + 36, top + 8 + (216 - ammoBarHeight), 8, ammoBarHeight);
                Main.spriteBatch.Draw(texture2, ammoBarPos, sourRec2, color);
            }
        }
    }

    class UISummonerPanel : UIMoveable
    {
        public override UIAnchor Anchor => Config.sumUIAnchor;
        public override ref int GetXOffset => ref Config.sumUIXOffset;
        public override ref int GetYOffset => ref Config.sumUIYOffset;

        readonly UISummonerSlot Slot1;
        readonly UISummonerSlot Slot2;
        readonly Texture2D _backgroundTexture = null;

        public UISummonerPanel(int left, int top, int width, int height)
        {
            SetPadding(0);
            Left.Set(left, 0f);
            Top.Set(top, 0f);
            Width.Set(width, 0f);
            Height.Set(height, 0f);
            if (_backgroundTexture == null)
                _backgroundTexture = Request<Texture2D>("TerraLeague/UI/SummonerBackground").Value;

            Slot1 = new UISummonerSlot(1,5,5,44);
            Append(Slot1);

            Slot2 = new UISummonerSlot(2,52,5,44);
            Append(Slot2);
        }

        public override void Update(GameTime gameTime)
        {
            Width.Set(96, 0f);
            MoveMode(Anchor, ref Config.sumUIXOffset, ref Config.sumUIYOffset);
            base.Update(gameTime);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle dimensions = this.GetDimensions();
            Point point1 = new Point((int)dimensions.X, (int)dimensions.Y - 2);
            int width = (int)Math.Ceiling(dimensions.Width);
            int height = (int)Math.Ceiling(dimensions.Height);
            Main.spriteBatch.Draw(_backgroundTexture, new Rectangle(point1.X, point1.Y, width, height), Color.White);
            base.DrawSelf(spriteBatch);
        }
    }

    class UISummonerSlot : UIElement
    {
        Texture2D _backgroundTexture = null;
        readonly Texture2D placeholderArt = TextureAssets.Buff[BuffID.Oiled].Value;
        public UIImage sumImage;
        public UIText sumCD;
        readonly UIText itemKey;
        readonly UIText toolTip;
        readonly int slotNum;

        public UISummonerSlot(int SlotNum, int left, int top, int dimentions)
        {
            slotNum = SlotNum;
            Left.Set(left, 0f);
            Top.Set(top, 0f);
            Width.Set(dimentions, 0f);
            Height.Set(dimentions, 0f);
            

            sumImage = new UIImage(placeholderArt);
            sumImage.Width.Pixels = Width.Pixels;
            sumImage.Height.Pixels = Height.Pixels;
            sumImage.Left.Pixels = 6;
            sumImage.Top.Pixels = 4;
            Append(sumImage);

            sumCD = new UIText("", 1);
            sumCD.Left.Pixels = 8;
            sumCD.Top.Pixels = 12;
            Append(sumCD);

            itemKey = new UIText(slotNum.ToString(), 0.75f);
            itemKey.Left.Pixels = 2;
            itemKey.Top.Pixels = -2;
            Append(itemKey);

            toolTip = new UIText("",1);
            toolTip.Width.Set(500, 0f);
            Append(toolTip);
        }

        public override void Update(GameTime gameTime)
        {
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();
            SummonerSpell spell = modPlayer.sumSpells[slotNum - 1];
            TerraLeague.GetTextureIfNull(ref _backgroundTexture, "TerraLeague/UI/AbilityBorder");

            string itemSlotText = "N/A";
            switch (slotNum)
            {
                case 1:
                    itemSlotText = LeagueTooltip.ConvertKeyString(TerraLeague.Sum1);
                    break;
                case 2:
                    itemSlotText = LeagueTooltip.ConvertKeyString(TerraLeague.Sum2);
                    break;
                default:
                    break;
            }
            itemKey.SetText(itemSlotText);

            if (modPlayer.sumSpells[slotNum - 1] != null)
            {
                sumImage.SetImage(Request<Texture2D>(spell.GetIconTexturePath()));
                sumImage.ImageScale = 1;

                sumCD.SetText(GetCooldown(slotNum));

                sumCD.Left.Pixels = 0;
                sumCD.Width.Pixels = this.Width.Pixels;
                sumCD.HAlign = 0.5f;
            }
            else
            {
                sumImage.SetImage(placeholderArt);
                sumImage.ImageScale = 0;
                sumCD.SetText("");
            }

            if (IsMouseHovering)
            {
                string text = LeagueTooltip.CreateColorString(TerraLeague.TooltipHeadingColor, spell.GetSpellName());
                text += "\n" + spell.GetTooltip();
                if (spell.GetRawCooldown() > 0)
                    text += "\n" + spell.GetCooldown() + " second cooldown";

                ToolTipUI.SetText(text.Split('\n'));
            }

            sumImage.Recalculate();
            sumCD.Recalculate();
            Recalculate();
            base.Update(gameTime);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle dimensions = this.GetDimensions();
            Point point1 = new Point((int)dimensions.X, (int)dimensions.Y - 2);
            int width = (int)Math.Ceiling(dimensions.Width);
            int height = (int)Math.Ceiling(dimensions.Height);
            Main.spriteBatch.Draw(_backgroundTexture, new Rectangle(point1.X, point1.Y, width, height), Color.White);
            base.DrawSelf(spriteBatch);
        }

        string GetCooldown(int slot)
        {
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();
            float cooldown = modPlayer.sumCooldowns[slot - 1];

            if (cooldown > 0)
            {
                if (cooldown > 10 * 60)
                {
                    return (cooldown / 60).ToString().Split('.')[0];
                }
                else
                {
                    string text = (Math.Round(cooldown / 60, 1)).ToString();
                    if (text.Length == 1)
                        text += ".0";
                    return text;
                }
            }
            else
            {
                return "";
            }
        }
    }

    class UIItemPanel : UIMoveable
    {
        public override UIAnchor Anchor => Config.itemUIAnchor;
        public override ref int GetXOffset => ref Config.itemUIXOffset;
        public override ref int GetYOffset => ref Config.itemUIYOffset;

        int slotsActiveLastFrame = 0;
        readonly UIItemSlot[] ItemSlots = new UIItemSlot[10];
        readonly Texture2D _backgroundTexture = null;
        public UIItemPanel()
        {
            SetPadding(0);

            if (_backgroundTexture == null)
                _backgroundTexture = Request<Texture2D>("TerraLeague/UI/ItemBackground").Value;

            for (int i = 0; i < ItemSlots.Length; i++)
            {
                ItemSlots[i] = new UIItemSlot(i + 1, 5, 5, 44);
                Append(ItemSlots[i]);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            UpdateSlots();
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle dimensions = this.GetDimensions();
            Point point1 = new Point((int)dimensions.X, (int)dimensions.Y - 2);
            int width = (int)Math.Ceiling(dimensions.Width);
            int height = (int)Math.Ceiling(dimensions.Height);
            Main.spriteBatch.Draw(_backgroundTexture, new Rectangle(point1.X, point1.Y, width, height), Color.White);
            base.DrawSelf(spriteBatch);
        }

        void UpdateSlots()
        {
            int slotsActive = 0;
            for (int i = 0; i < ItemSlots.Length; i++)
            {
                if (ItemSlots[i].IsActive())
                {
                    slotsActive++;
                }
            }

            if (slotsActiveLastFrame != slotsActive)
            {
                bool isCountOdd = slotsActive % 2 != 0;
                int botRowCount = (slotsActive - (isCountOdd ? 1 : 0)) / 2;
                int topRowCount = slotsActive - botRowCount;

                List<Vector2> slotPositions = new List<Vector2>();
                for (int i = 0; i < topRowCount; i++)
                {
                    slotPositions.Add(new Vector2(5 + (47 * i), 5));
                }
                for (int i = 0; i < botRowCount; i++)
                {
                    slotPositions.Add(new Vector2(5 + (isCountOdd ? 24 : 0) + (47 * i), 52));
                }

                int slotNum = 1;
                for (int i = 0; i < ItemSlots.Length; i++)
                {
                    if (ItemSlots[i].IsActive())
                    {
                        ItemSlots[i].SetPosition(slotPositions[0]);
                        ItemSlots[i].slotNum = slotNum;
                        slotPositions.RemoveAt(0);
                        slotNum++;
                    }
                }

                Width.Set(5 + (topRowCount * 47) + 2, 0);
                Height.Set(5 + (topRowCount > 0 ? 47 : 0) + (botRowCount > 0 ? 47 : 0) + 2, 0);

                slotsActiveLastFrame = slotsActive;
            }
        }
    }

    class UIItemSlot : UIElement
    {
        Texture2D _backgroundTexture = null;
        Texture2D cooldown_texture;
        Texture2D active_texture;
        Texture2D normal_texture;
        Texture2D masterwork_texture;

        readonly Texture2D placeholderArt = TextureAssets.Buff[BuffID.Oiled].Value;
        readonly UIImage itemImage;
        readonly UIImage masterWorkImage;
        readonly UIText itemCooldown;
        readonly UIText itemStat;
        readonly UIText itemKey;
        readonly int accessorySlot;
        public int slotNum;

        public UIItemSlot(int SlotNum, int left, int top, int dimentions)
        {
            accessorySlot = SlotNum;
            Left.Set(left, 0f);
            Top.Set(top, 0f);
            Width.Set(dimentions, 0f);
            Height.Set(dimentions, 0f);

            itemImage = new UIImage(placeholderArt);
            itemImage.Width.Pixels = Width.Pixels;
            itemImage.Height.Pixels = Height.Pixels;
            itemImage.Left.Pixels = 0;//-6;
            itemImage.Top.Pixels = 0;//-6;
            Append(itemImage);

            TerraLeague.GetTextureIfNull(ref masterwork_texture, MasterworkItem.MasterworkIconPath);
            masterWorkImage = new UIImage(masterwork_texture);
            masterWorkImage.Width.Pixels = Width.Pixels;
            masterWorkImage.Height.Pixels = Height.Pixels;
            masterWorkImage.Left.Pixels = 12;//-6;
            masterWorkImage.Top.Pixels = 12;//-6;
            Append(masterWorkImage);

            itemStat = new UIText("", 0.75f);
            itemStat.Left.Pixels = 8;
            itemStat.Top.Pixels = 24;
            Append(itemStat);

            itemCooldown = new UIText("", 1);
            itemCooldown.Width.Pixels = dimentions;
            itemCooldown.Height.Pixels = dimentions;
            itemCooldown.HAlign = 0.5f;
            itemCooldown.VAlign = 0.5f;
            Append(itemCooldown);

            itemKey = new UIText(slotNum.ToString(), 0.75f);
            itemKey.Left.Pixels = 2;
            itemKey.Top.Pixels = -2;
            Append(itemKey);
        }

        public override void Update(GameTime gameTime)
        {
            if (IsActive())
            {
                SetKeyString();

                TerraLeague.GetTextureIfNull(ref cooldown_texture, "TerraLeague/UI/ItemBorderCooldown");
                TerraLeague.GetTextureIfNull(ref active_texture, "TerraLeague/UI/ItemBorderActive");
                TerraLeague.GetTextureIfNull(ref normal_texture, "TerraLeague/UI/ItemBorder");
                TerraLeague.GetTextureIfNull(ref masterwork_texture, MasterworkItem.MasterworkIconPath);


                if (Main.LocalPlayer.armor[accessorySlot + 2].ModItem is LeagueItem legItem)
                {
                    if (legItem is MasterworkItem mastItem)
                    {
                        if (mastItem.IsMasterWorkItem)
                        {
                            masterWorkImage.SetImage(masterwork_texture);
                            masterWorkImage.Color = Color.White;
                            masterWorkImage.Left.Pixels = 9;//-6;
                            masterWorkImage.Top.Pixels = 7;//-6;
                        }
                        else
                        {
                            masterWorkImage.Color = Color.White * 0;
                        }
                    }
                    else
                    {
                        masterWorkImage.Color = Color.White * 0;
                    }

                    if (legItem.OnCooldown(Main.LocalPlayer))
                    {
                        itemCooldown.SetText(legItem.GetStatText());
                        itemStat.SetText("");
                    }
                    else
                    {
                        itemCooldown.SetText("");
                        itemStat.SetText(legItem.GetStatText());
                    }

                    if (legItem.OnCooldown(Main.LocalPlayer))
                    {
                        _backgroundTexture = cooldown_texture;
                    }
                    else if (legItem.Active != null)
                    {
                        _backgroundTexture = active_texture;
                    }
                    else
                    {
                        _backgroundTexture = normal_texture;
                    }
                }
                else
                {
                    itemCooldown.SetText("");
                    itemStat.SetText("");
                    _backgroundTexture = normal_texture;
                    masterWorkImage.Color = Color.White * 0;
                }


                if (Main.LocalPlayer.armor[accessorySlot + 2].active)
                {
                    Texture2D texture = GetTexture(Main.LocalPlayer.armor[accessorySlot + 2]);
                    itemImage.SetImage(texture);
                    itemImage.Left.Pixels = ((32 - texture.Width) / 2) + 6;
                    itemImage.Top.Pixels = ((32 - texture.Height) / 2) + 4;
                    itemStat.Left.Pixels = 16;
                    itemStat.Top.Pixels = 30;
                    itemImage.ImageScale = 1;
                    itemCooldown.HAlign = 0.5f;
                    itemCooldown.Top.Pixels = 12;

                    
                }
                else
                {
                    itemImage.SetImage(placeholderArt);
                    itemImage.ImageScale = 0;
                }

                if (IsMouseHovering)
                {
                    if (Lang.GetItemName(Main.LocalPlayer.armor[slotNum + 2].type).ToString() != "")
                    {
                        ModItem modItem = Main.LocalPlayer.armor[slotNum + 2].ModItem;

                        if (modItem != null)
                        {
                            string[] activeTip;
                            string[] primPassiveTip;
                            System.Collections.Generic.List<string> activePassiveTooltips = new System.Collections.Generic.List<string>();

                            for (int i = 3; i < 9; i++)
                            {
                                if (Main.LocalPlayer.armor[i].type == modItem.Item.type)
                                {
                                    modItem = Main.LocalPlayer.armor[i].ModItem;
                                    break;
                                }
                            }

                            legItem = modItem as LeagueItem;

                            if (legItem != null)
                            {
                                int slot = TerraLeague.FindAccessorySlotOnPlayer(Main.LocalPlayer, legItem);
                                if (slot != -1)
                                {
                                    if (legItem.Active != null)
                                    {
                                        if (legItem.Active.currentlyActive)
                                        {
                                            activeTip = legItem.Active.Tooltip(Main.LocalPlayer, legItem).Split('\n');
                                            for (int i = 0; i < activeTip.Length; i++)
                                            {
                                                activePassiveTooltips.Add(activeTip[i]);
                                            }
                                        }
                                    }
                                    if (legItem.Passives != null)
                                    {
                                        for (int j = 0; j < legItem.Passives.Length; j++)
                                        {
                                            if (legItem.Passives[j].currentlyActive)
                                            {
                                                primPassiveTip = legItem.Passives[j].Tooltip(Main.LocalPlayer, legItem).Split('\n');
                                                for (int i = 0; i < primPassiveTip.Length; i++)
                                                {
                                                    activePassiveTooltips.Add(primPassiveTip[i]);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (modItem is LeagueBoot bootItem)
                                {
                                    primPassiveTip = bootItem.BuildFullTooltip(false).Split('\n');
                                    for (int i = 0; i < primPassiveTip.Length; i++)
                                    {
                                        activePassiveTooltips.Add(primPassiveTip[i]);
                                    }
                                }
                            }

                            string ToolTip = "";
                            ToolTip = LeagueTooltip.CreateColorString(TerraLeague.TooltipHeadingColor, modItem.Item.Name);

                            //string heading = LeagueTooltip.CreateColorString(TerraLeague.TooltipHeadingColor, Lang.GetItemName(modItem.Item.type).Value);

                            int baseTooltipLines = 0;
                            if (modItem is MasterworkItem masterItem && masterItem.IsMasterWorkItem)
                            {
                                ToolTip += "\n" + masterItem.MasterworkTooltip();
                            }
                            else
                            {
                                ItemTooltip itemsBaseTooltip = Lang.GetTooltip(modItem.Item.type);
                                baseTooltipLines = itemsBaseTooltip.Lines;
                                if (baseTooltipLines == 1 && itemsBaseTooltip.GetLine(0) == "")
                                    baseTooltipLines = 0;
                                for (int i = 0; i < baseTooltipLines; i++)
                                {
                                    ToolTip += "\n" + itemsBaseTooltip.GetLine(i);
                                }
                            }

                            //string[] compiledTooltip = new string[];
                            //compiledTooltip[0] = heading;

                            for (int i = 0; i < activePassiveTooltips.Count; i++)
                            {
                                ToolTip += "\n" + activePassiveTooltips[i];
                            }

                            ToolTipUI.SetText(ToolTip);
                        }
                        else
                        {
                            string ToolTip = LeagueTooltip.CreateColorString(TerraLeague.TooltipHeadingColor, Lang.GetItemName(Main.LocalPlayer.armor[slotNum + 2].type).Value);

                            //string heading = LeagueTooltip.CreateColorString(TerraLeague.TooltipHeadingColor, Lang.GetItemName(Main.LocalPlayer.armor[slotNum + 2].type).Value);
                            var itemsBaseTooltip = Lang.GetTooltip(Main.LocalPlayer.armor[slotNum + 2].type);

                            //string[] compiledTooltip = new string[ToolTipUI.MaxLines];
                            //compiledTooltip[0] = heading;

                            for (int i = 0; i < itemsBaseTooltip.Lines; i++)
                            {
                                ToolTip += "\n" + itemsBaseTooltip.GetLine(i);
                            }
                            ToolTipUI.SetText(ToolTip);
                        }
                    }
                }
                Recalculate();
            }
            else
            {
                itemStat.SetText("");
                itemKey.SetText("");
                itemCooldown.SetText("");
                itemImage.ImageScale = 0;
                SetPosition(Vector2.One * 1000);
            }
            base.Update(gameTime);
        }

        private Texture2D GetTexture(Item item)
        {
            Texture2D texture = TextureAssets.Item[item.type].Value;

            if (texture.Width * 2 < texture.Height)
            {
                Rectangle newBounds = texture.Bounds;
                newBounds.X = 0;
                newBounds.Y = 0;
                newBounds.Width = texture.Width;
                newBounds.Height = texture.Width;

                Texture2D croppedTexture = new Texture2D(Main.graphics.GraphicsDevice, newBounds.Width, newBounds.Height);

                Color[] data = new Color[newBounds.Width * newBounds.Height];
                texture.GetData(0, newBounds, data, 0, newBounds.Width * newBounds.Height);
                croppedTexture.SetData(data);

                return croppedTexture;
            }

            return texture;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (IsActive())
            {
                CalculatedStyle dimensions = this.GetDimensions();
                Point point1 = new Point((int)dimensions.X, (int)dimensions.Y - 2);
                int width = (int)Math.Ceiling(dimensions.Width);
                int height = (int)Math.Ceiling(dimensions.Height);
                Main.spriteBatch.Draw(_backgroundTexture, new Rectangle(point1.X, point1.Y, width, height), Color.White);
                base.DrawSelf(spriteBatch);
            }
        }

        private void SetKeyString()
        {
            string itemSlotText = "N/A";
            if (slotNum >= 1 && slotNum <= 6)
            {
                switch (slotNum)
            {
                case 1:
                    itemSlotText = LeagueTooltip.ConvertKeyString(TerraLeague.Item1);
                    break;
                case 2:
                    itemSlotText = LeagueTooltip.ConvertKeyString(TerraLeague.Item2);
                    break;
                case 3:
                    itemSlotText = LeagueTooltip.ConvertKeyString(TerraLeague.Item3);
                    break;
                case 4:
                    itemSlotText = LeagueTooltip.ConvertKeyString(TerraLeague.Item4);
                    break;
                case 5:
                    itemSlotText = LeagueTooltip.ConvertKeyString(TerraLeague.Item5);
                    break;
                case 6:
                    itemSlotText = LeagueTooltip.ConvertKeyString(TerraLeague.Item6);
                    break;
                default:
                    break;
            }
            }
            else
            {
                itemSlotText = "";
            }
            itemKey.SetText(itemSlotText);
        }

        public bool IsActive()
        {
            if (accessorySlot >= 1 && accessorySlot <= 5)
            {
                return true;
            }
            else if (accessorySlot == 6)
            {
                return Main.LocalPlayer.extraAccessory && Main.expertMode;
            }
            else if (accessorySlot == 7)
            {
                return Main.masterMode;
            }
            else
            {
                int extraslots = Main.LocalPlayer.GetAmountOfExtraAccessorySlotsToShow();
                return extraslots + 7 - 2 >= accessorySlot;
            }
        }

        public void SetPosition(Vector2 newPosition)
        {
            Left.Set(newPosition.X, 0f);
            Top.Set(newPosition.Y, 0f);
        }
    }

    class UIStatPanel : UIMoveable
    {
        public override ref int GetXOffset => ref Config.statUIXOffset;
        public override ref int GetYOffset => ref Config.statUIYOffset;
        public override UIAnchor Anchor => Config.statUIAnchor;

        public bool extraStats = false;
        readonly UIText meleeStats;
        readonly UIText rangedStats;
        readonly UIText magicStats;
        readonly UIText summonStats;
        readonly UIText armorStats;
        readonly UIText resistStats;
        readonly UIText CDRStats;
        readonly UIText ammoStats;
        readonly UIText healStats;
        readonly UIText manaStats;
        //readonly UIText tooltip;

        Texture2D _backgroundTexture;

        public UIStatPanel(int width, int height)
        {
            SetPadding(0);
            Width.Set(width, 0f);
            Height.Set(height, 0f);
            if (_backgroundTexture == null)
                _backgroundTexture = Request<Texture2D>("TerraLeague/UI/StatsBackgroundSmall").Value;

            armorStats = new UIText("ARM: 000", 0.65f);
            armorStats.Left.Pixels = 8;
            armorStats.Top.Pixels = 6;
            armorStats.TextColor = Color.Yellow;

            resistStats = new UIText("RST: 000", 0.65f);
            resistStats.Left.Pixels = 80;
            resistStats.Top.Pixels = 6;
            resistStats.TextColor = Color.LightSteelBlue;

            meleeStats = new UIText("MEL: 000%", 0.65f);
            meleeStats.Left.Pixels = 8;
            meleeStats.Top.Pixels = 22;
            meleeStats.TextColor = Color.Orange;

            rangedStats = new UIText("RNG: 000%", 0.65f);
            rangedStats.Left.Pixels = 80;
            rangedStats.Top.Pixels = 22;
            rangedStats.TextColor = Color.LightSeaGreen;

            magicStats = new UIText("MAG: 000%", 0.65f);
            magicStats.Left.Pixels = 8;
            magicStats.Top.Pixels = 38;
            magicStats.TextColor = Color.MediumPurple;

            summonStats = new UIText("SUM: 000%", 0.65f);
            summonStats.Left.Pixels = 80;
            summonStats.Top.Pixels = 38;
            summonStats.TextColor = Color.SkyBlue;

            CDRStats = new UIText("CDR: 40%", 0.65f);
            CDRStats.Left.Pixels = 8;
            CDRStats.Top.Pixels = 54;
            CDRStats.TextColor = LeagueTooltip.ConvertHexToColor(LeagueTooltip.HasteColor);

            healStats = new UIText("HEAL: 000%", 0.65f);
            healStats.Left.Pixels = 80;
            healStats.Top.Pixels = 54;
            healStats.TextColor = Color.Green;

            ammoStats = new UIText("AMMO: 100%", 0.65f);
            ammoStats.Left.Pixels = 8;
            ammoStats.Top.Pixels = 70;
            ammoStats.TextColor = Color.Gray;

            manaStats = new UIText("MANA: 000%", 0.65f);
            manaStats.Left.Pixels = 80;
            manaStats.Top.Pixels = 70;
            manaStats.TextColor = Color.RoyalBlue;


            //tooltip = new UIText("");
            //tooltip.Left.Set(Main.screenWidth/2 - 250 - Left.Pixels, 0);
            //tooltip.Top.Set(Main.screenHeight - 171 - Top.Pixels, 0);

            Append(meleeStats);
            Append(rangedStats);
            Append(magicStats);
            Append(summonStats);
            Append(armorStats);
            Append(resistStats);
            Append(CDRStats);
            Append(healStats);
            Append(ammoStats);
            Append(manaStats);
            //Append(tooltip);
        }

        public override void Update(GameTime gameTime)
        {
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();

            armorStats.Width.Set(30,0);
            resistStats.Width.Set(30,0);
            meleeStats.Width.Set(30,0);
            rangedStats.Width.Set(30,0);
            magicStats.Width.Set(30,0);
            summonStats.Width.Set(30,0);
            CDRStats.Width.Set(30,0);
            healStats.Width.Set(30,0);
            ammoStats.Width.Set(30,0);

            manaStats.Width.Set(30,0);

            if (extraStats)
            {
                _backgroundTexture = Request<Texture2D>("TerraLeague/UI/StatsBackgroundLarge").Value;

                Height.Set(90, 0f);
                GetStats(true);
            }
            else
            {
                _backgroundTexture = Request<Texture2D>("TerraLeague/UI/StatsBackgroundSmall").Value;

                Height.Set(58, 0f);
                GetStats();
            }

            string text = "";


            if (!moveMode)
            {
                if (armorStats.IsMouseHovering)
                {
                    text = LeagueTooltip.CreateColorString(LeagueTooltip.ArmorColor, "Armor") +
                        "\nReduces damage from contact by " + Math.Round(100 - (modPlayer.ArmorDamageReduction * 100f), 2) + "%" +
                        "\nCurrent Armor consists of" +
                        "\n" + LeagueTooltip.CreateColorString(LeagueTooltip.ArmorColor, "From Armor increases: " + modPlayer.armorLastStep);
                    if (TerraLeague.UseCustomDefenceStat)
                        text += "\n" + LeagueTooltip.CreateColorString(LeagueTooltip.DefenceColor, "From Defence increases: " + modPlayer.defenceLastStep);
                }
                else if (resistStats.IsMouseHovering)
                {
                    text = LeagueTooltip.CreateColorString(LeagueTooltip.ResistColor, "Resist") +
                        "\nReduces damage from projectiles by " + Math.Round(100 - (modPlayer.ResistDamageReduction * 100f), 2) + "%" +
                        "\nCurrent Resist consists of" +
                        "\n" + LeagueTooltip.CreateColorString(LeagueTooltip.ResistColor, "From Resist increases: " + modPlayer.resistLastStep);
                    if (TerraLeague.UseCustomDefenceStat)
                        text += "\n" + LeagueTooltip.CreateColorString(LeagueTooltip.DefenceColor, "From Defence increases: " + modPlayer.defenceLastStep);
                }
                else if (meleeStats.IsMouseHovering)
                {
                    text = LeagueTooltip.CreateColorString(LeagueTooltip.MeleeColor, "Melee Damage") +
                        "\nUsed for Abilities and Items scaling damage." +
                        "\nGain a flat amount that increases throughout the game plus 1.5 per 1% melee damage" +
                        "\nMelee Weapons Deal " + (int)(modPlayer.meleeDamageLastStep * 100) + "% damage." +
                        "\nExtra Damage: +" + modPlayer.meleeFlatDamage +
                        "\nCrit Chance: +" + (modPlayer.Player.GetCritChance(DamageClass.Melee) + modPlayer.Player.GetCritChance(DamageClass.Generic) - 4) + "%" +
                        "\nLife Steal: " + (int)(modPlayer.lifeStealMelee) +
                        "\nFlat On Hit: " + modPlayer.meleeOnHit +
                        "\nArmor Penetration: " + (modPlayer.meleeArmorPen + modPlayer.Player.armorPenetration);
                }
                else if (rangedStats.IsMouseHovering)
                {
                    text = LeagueTooltip.CreateColorString(LeagueTooltip.RangedColor, "Ranged Damage") +
                        "\nUsed for Abilities and Items scaling damage. Gain 2 per 1% ranged damage" +
                        "\nRanged Weapons Deal " + (int)(modPlayer.rangedDamageLastStep * 100) + "% damage." +
                        "\nExtra Damage: +" + modPlayer.rangedFlatDamage +
                        "\nCrit Chance: +" + (modPlayer.Player.GetCritChance(DamageClass.Ranged) + modPlayer.Player.GetCritChance(DamageClass.Generic) - 4) + "%" +
                        "\nLife Steal: " + (int)(modPlayer.lifeStealRange) +
                        "\nFlat On Hit: " + modPlayer.rangedOnHit +
                        "\nArmor Penetration: " + (modPlayer.rangedArmorPen + modPlayer.Player.armorPenetration);
                }
                else if (magicStats.IsMouseHovering)
                {
                    text = LeagueTooltip.CreateColorString(LeagueTooltip.MagicColor, "Magic Damage") +
                        "\nUsed for Abilities and Items scaling damage. Gain 2.5 per 1% magic damage" +
                        "\nMagic Weapons Deal " + (int)(modPlayer.magicDamageLastStep * 100) + "% damage." +
                        "\nExtra Damage: +" + modPlayer.magicFlatDamage +
                        "\nCrit Chance: +" + (modPlayer.Player.GetCritChance(DamageClass.Magic) + modPlayer.Player.GetCritChance(DamageClass.Generic) - 4) + "%" +
                        "\nLife Steal: " + (int)(modPlayer.lifeStealMagic) +
                        "\nFlat On Hit: " + modPlayer.magicOnHit +
                        "\nArmor Penetration: " + (modPlayer.magicArmorPen + modPlayer.Player.armorPenetration);
                }
                else if (summonStats.IsMouseHovering)
                {
                    text = LeagueTooltip.CreateColorString(LeagueTooltip.SummonColor, "Summon Damage") +
                        "\nUsed for Abilities and Items scaling damage. Gain 1.75 per 1% summon damage" +
                        "\nSummoner Weapons Deal " + (int)(modPlayer.minionDamageLastStep * 100) + "% damage." +
                        "\nExtra Damage: +" + modPlayer.minionFlatDamage +
                        "\nLife Steal: " + (int)(modPlayer.lifeStealMinion) +
                        "\nFlat On Hit: " + modPlayer.meleeOnHit +
                        "\nArmor Penetration: " + (modPlayer.minionArmorPen + modPlayer.Player.armorPenetration) +
                        "\nMinions: " + (modPlayer.Player.maxMinions) +
                        " ~ Sentries: " + (modPlayer.Player.maxTurrets);
                }
                else if (CDRStats.IsMouseHovering)
                {
                    text = LeagueTooltip.CreateColorString(LeagueTooltip.HasteColor, "Haste") +
                        "\nThe percent increase in spell/item casts" +
                        "\nAbility Haste: " + modPlayer.abilityHaste + " (" + Math.Round(100 - (modPlayer.Cdr * 100), 2) + "% reduction)" +
                        "\nItem Haste: " + modPlayer.itemHaste + " (" + Math.Round(100 - (modPlayer.ItemCdr * 100), 2) + "% reduction)" +
                        "\nSummoner Spell Haste: " + modPlayer.summonerHaste + " (" + Math.Round(100 - (modPlayer.SummonerCdr * 100), 2) + "% reduction)";
                }
                else if (ammoStats.IsMouseHovering)
                {
                    text = LeagueTooltip.CreateColorString(LeagueTooltip.RngAtkSpdColor, "Ranged Attack Speed") +
                        "\nThe percent increase in ranged weapons attack speed";

                    Item item = Main.LocalPlayer.HeldItem;
                    if (item.DamageType == DamageClass.Ranged)
                    {
                        float scaledUsetime = Math.Max(item.useTime * (2 - item.GetGlobalItem<Items.TerraLeagueITEMGLOBAL>().UseSpeedMultiplier(item, Main.LocalPlayer)), 1);
                        text += "\n" + item.Name + " fires " + Math.Min(Math.Abs(Math.Round(60f / (scaledUsetime + (item.reuseDelay * (float)scaledUsetime / item.useAnimation)), 2)), 60) + " times per second";
                    }
                    else
                    {
                        text += "\nHold a Ranged Weapon to see its fire rate";
                    }
                }
                else if (healStats.IsMouseHovering)
                {
                    text = LeagueTooltip.CreateColorString(LeagueTooltip.HealPowerColor, "Heal Power") +
                        "\nThe percent increase in all your outgoing healing and shielding";
                }
                else if (manaStats.IsMouseHovering)
                {
                    text = LeagueTooltip.CreateColorString(LeagueTooltip.ManaReductionColor, "Mana Cost Reduction") +
                        "\nThe percent reduction of all mana costs";
                }

                SetPosition(Anchor, GetXOffset, GetYOffset, this);
            }

            base.Update(gameTime);

            if (text != "")
            {
                ToolTipUI.SetText(text.Split('\n'));
            }
        }

        public void GetStats(bool extra = false)
        {
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();

            armorStats.SetText("ARM: " + (modPlayer.armor + (TerraLeague.UseCustomDefenceStat ? modPlayer.defenceLastStep : 0)));
            resistStats.SetText("RST: " + (modPlayer.resist + (TerraLeague.UseCustomDefenceStat ? modPlayer.defenceLastStep : 0)));
            meleeStats.SetText("MEL: " + modPlayer.MEL);
            rangedStats.SetText("RNG: " + modPlayer.RNG);
            magicStats.SetText("MAG: " + modPlayer.MAG);
            summonStats.SetText("SUM: " + modPlayer.SUM);

            if (extra)
            {
                ammoStats.SetText("ATS: " + (Math.Round(modPlayer.rangedAttackSpeed * 100)).ToString() + "%");
                healStats.SetText("HEAL: " + ((int)(modPlayer.healPower * 100)).ToString() + "%");
                CDRStats.SetText("HST: " + modPlayer.abilityHaste);
                manaStats.SetText("MANA: " + ((int)((1 - modPlayer.Player.manaCost) * 100)).ToString() + "%");
            }
            else
            {
                ammoStats.SetText("");
                healStats.SetText("");
                CDRStats.SetText("");
                manaStats.SetText("");
            }
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle dimensions = this.GetDimensions();
            Point point1 = new Point((int)dimensions.X, (int)dimensions.Y - 2);
            int width = (int)Math.Ceiling(dimensions.Width);
            int height = (int)Math.Ceiling(dimensions.Height);
            Main.spriteBatch.Draw(_backgroundTexture, new Rectangle(point1.X, point1.Y, width, height), Color.White);
            base.DrawSelf(spriteBatch);
        }
    }
}