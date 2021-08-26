using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.Common.ModSystems;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace TerraLeague.UI
{
    internal class CustomResourceUI : UIState
    {
        public static bool visible = true;
        Texture2D _backgroundTexture;
        BuffPanelUI BuffUI;
        ResourcePanel ResourceUI;

        public static Color RedHealthColor = new Color(164, 55, 65);
        public static Color OrangeHealthColor = new Color(211, 113, 48);
        public static Color BonusHealthColor = new Color(100, 35, 40);

        public static Color BasicShieldColor = new Color(230, 230, 230);
        public static Color MagicShieldColor = new Color(172, 122, 219);
        public static Color PhysicalShieldColor = new Color(219, 190, 118);

        public static Color ManaColor = new Color(46, 67, 114);

        // Textures
        Texture2D texture_breathBar;
        Texture2D texture_lavaBar;
        Texture2D texture_voidBar;
        Texture2D texture_voidBreathBar;
        Texture2D texture_voidLavaBar;

        Texture2D texture_innerbar;
        Texture2D texture_smallBar;

        public override void OnInitialize()
        {
            //if (_backgroundTexture == null || _backgroundTexture.Width <= 1)
            //    _backgroundTexture = ModContent.Request<Texture2D>("TerraLeague/UI/BarBackground").Value;
            TerraLeague.GetTextureIfNull(ref _backgroundTexture, "TerraLeague/UI/BarBackground");

            TerraLeague.GetTextureIfNull(ref texture_breathBar, "TerraLeague/UI/BreathBar");
            TerraLeague.GetTextureIfNull(ref texture_lavaBar, "TerraLeague/UI/BarBackground");
            TerraLeague.GetTextureIfNull(ref texture_voidBar, "TerraLeague/UI/LavaCharmBar");
            TerraLeague.GetTextureIfNull(ref texture_voidBreathBar, "TerraLeague/UI/VoidAirBar");
            TerraLeague.GetTextureIfNull(ref texture_voidLavaBar, "TerraLeague/UI/VoidLavaBar");

            TerraLeague.GetTextureIfNull(ref texture_innerbar, "TerraLeague/UI/Blank");
            TerraLeague.GetTextureIfNull(ref texture_smallBar, "TerraLeague/UI/SmallBlank_H");

            BuffUI = new BuffPanelUI();
            Append(BuffUI);

            ResourceUI = new ResourcePanel();
            Append(ResourceUI);
        }

        public override void Update(GameTime gameTime)
        {
            Recalculate();
            base.Update(gameTime);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            //CalculatedStyle dimensions = MainPanel.GetDimensions();
            //Point point1 = new Point((int)dimensions.X, (int)dimensions.Y);
            //int width = (int)Math.Ceiling(dimensions.Width);
            //int height = (int)Math.Ceiling(dimensions.Height);
            //Main.spriteBatch.Draw(_backgroundTexture, new Rectangle(point1.X, point1.Y, width, height), Color.White);
            //base.Width.Pixels = Main.screenWidth;
            //base.Height.Pixels = Main.screenHeight;

            Player drawPlayer = Main.LocalPlayer;
            PLAYERGLOBAL modPlayer = drawPlayer.GetModPlayer<PLAYERGLOBAL>();

            bool breathActive = drawPlayer.breath != drawPlayer.breathMax;
            bool lavaActive = drawPlayer.lavaTime != drawPlayer.lavaMax;
            bool voidActive = modPlayer.VoidInflu != PLAYERGLOBAL.VoidInfluMax;
            bool duelMeter = false;
            Texture2D meter = texture_breathBar;
            
            Rectangle destRec = new Rectangle((int)((base.Width.Pixels/2) - 58), (int)((base.Height.Pixels/2) - 48), 116, 20);
            Rectangle barTop = new Rectangle();
            Rectangle barBot = new Rectangle();
            Color topColor = Color.White;
            Color botColor = Color.White;

            if (voidActive)
            {
                if (breathActive)
                {
                    barTop = new Rectangle((int)((base.Width.Pixels / 2) - 50), (int)((base.Height.Pixels / 2) - 46), (int)(100 * (drawPlayer.breath / (double)drawPlayer.breathMax)), 8);
                    topColor = Color.DarkCyan;
                    meter = texture_voidBreathBar;
                }
                else if (lavaActive)
                {
                    barTop = new Rectangle((int)((base.Width.Pixels / 2) - 50), (int)((base.Height.Pixels / 2) - 46), (int)(100 - 100 * (drawPlayer.lavaTime / (double)drawPlayer.lavaMax)), 8);
                    topColor = new Color((int)(255 - 255 * (drawPlayer.lavaTime / (float)drawPlayer.lavaMax)), 0, (int)(255 * (drawPlayer.lavaTime / (float)drawPlayer.lavaMax)));
                    meter = texture_voidLavaBar;
                }

                if (breathActive || lavaActive)
                {
                    duelMeter = true;
                    barBot = new Rectangle((int)((base.Width.Pixels / 2) - 50), (int)((base.Height.Pixels / 2) - 38), (int)(100 - 100 * (modPlayer.VoidInflu / PLAYERGLOBAL.VoidInfluMax)), 8);
                    botColor = Color.DarkMagenta;
                }
                else
                {
                    barTop = new Rectangle((int)((base.Width.Pixels / 2) - 50), (int)((base.Height.Pixels / 2) - 46), (int)(100 - 100 * (modPlayer.VoidInflu / PLAYERGLOBAL.VoidInfluMax)), 16);
                    topColor = Color.DarkMagenta;
                    meter = texture_voidBar;
                }

            }
            else
            {
                if (breathActive)
                {
                    barTop = new Rectangle((int)((base.Width.Pixels / 2) - 50), (int)((base.Height.Pixels / 2) - 46), (int)(100 * (drawPlayer.breath / (double)drawPlayer.breathMax)), 16);
                    topColor = Color.DarkCyan;
                    meter = texture_breathBar;
                }
                else if (lavaActive)
                {
                    barTop = new Rectangle((int)((base.Width.Pixels / 2) - 50), (int)((base.Height.Pixels / 2) - 46), (int)(100 - 100 * (drawPlayer.lavaTime / (double)drawPlayer.lavaMax)), 16);
                    topColor = new Color((int)(255 - 255 * (drawPlayer.lavaTime / (float)drawPlayer.lavaMax)), 0, (int)(255 * (drawPlayer.lavaTime / (float)drawPlayer.lavaMax)));
                    meter = texture_voidLavaBar;
                }
            }

            if (voidActive || breathActive || lavaActive)
            {
                Texture2D texture = meter;
                Rectangle sourRec = new Rectangle(0, 0, 116, 20);
                Main.spriteBatch.Draw(texture, destRec, sourRec, Color.White);

                if (duelMeter)
                {
                    Texture2D textureBar = texture_smallBar;
                    Rectangle sourRecTop = new Rectangle(0, 0, 8, 8);
                    Main.spriteBatch.Draw(textureBar, barTop, sourRecTop, topColor);

                    Rectangle sourRecBot = new Rectangle(0, 0, 8, 8);
                    Main.spriteBatch.Draw(textureBar, barBot, sourRecBot, botColor);
                }
                else
                {
                    Texture2D textureTop = texture_innerbar;
                    Rectangle sourRecTop = new Rectangle(0, 0, 16, 16);
                    Main.spriteBatch.Draw(textureTop, barTop, sourRecTop, topColor);
                }
            }
        }
    }

    public class ResourcePanel : UIMoveable
    {
        public override UIAnchor Anchor => Config.resourceUIAnchor;
        public override ref int GetXOffset => ref Config.resourceUIXOffset;
        public override ref int GetYOffset => ref Config.resourceUIYOffset;

        ResourceBar hp;
        ResourceBar mana;

        public override void OnInitialize()
        {
            Width.Set(500, 0);
            Height.Set(52, 0f);

            hp = new ResourceBar(ResourceBarMode.HP, 20, 480);
            // hp.Left.Set(10, 0f);
           // hp.Top.Set(4f, 0f);
            Append(hp);

            mana = new ResourceBar(ResourceBarMode.MANA, 20, 480);
            //mana.Left.Set(10, 0f);
            mana.Top.Set(20f, 0f);
            Append(mana);

            base.OnInitialize();
        }

        public override void Update(GameTime gameTime)
        {
            Top.Set(0, 0);

            base.Update(gameTime);
        }
    }

    class UIBar : UIElement
    {

        public Color backgroundColor = Color.Gray;
        private Texture2D _backgroundTexture;

        public UIBar()
        {
            
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            TerraLeague.GetTextureIfNull(ref _backgroundTexture, "TerraLeague/UI/Bar");

            CalculatedStyle dimensions = GetDimensions();
            Point point1 = new Point((int)dimensions.X, (int)dimensions.Y);
            int width = (int)Math.Ceiling(dimensions.Width);
            int height = (int)Math.Ceiling(dimensions.Height);
            Main.spriteBatch.Draw(_backgroundTexture, new Rectangle(point1.X, point1.Y, width, height), backgroundColor);
        }
    }

    class UIInnerBar : UIElement
    {
        public Color backgroundColor = Color.Gray;
        private Texture2D texture_innerbar;

        public UIInnerBar()
        {
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            TerraLeague.GetTextureIfNull(ref texture_innerbar, "TerraLeague/UI/Blank");

            CalculatedStyle dimensions = GetDimensions();
            Point point1 = new Point((int)dimensions.X, (int)dimensions.Y);
            int width = (int)Math.Ceiling(dimensions.Width);
            int height = (int)Math.Ceiling(dimensions.Height);
            Main.spriteBatch.Draw(texture_innerbar, new Rectangle(point1.X, point1.Y, width, height), backgroundColor);
        }
    }

    class ResourceBar : UIElement
    {
        public static int healthBarDividerDistance = 50;
        public static int manaBarDividerDistance = 50;
        private UIBar barBackground;
        private UIInnerBar currentBar;
        private UIInnerBar lifeFruitBar;
        private UIInnerBar bonusHealthBar;
        private UIInnerBar shieldBar;
        private UIInnerBar magicShieldBar;
        private UIInnerBar physShieldBar;
        private UIText text;
        private UIText regen;
        private readonly ResourceBarMode stat;
        private readonly float width;
        private readonly float height;

        private UIImage[] markers;
        

        public override void OnInitialize()
        {
            Height.Set(height, 0f);
            Width.Set(width, 0f);

            barBackground = new UIBar();
            barBackground.Left.Set(0f, 0f);
            barBackground.Top.Set(0f, 0f);
            barBackground.backgroundColor = Color.White;
            barBackground.Width.Set(width, 0f);
            barBackground.Height.Set(height, 0f);

            currentBar = new UIInnerBar();
            currentBar.SetPadding(0);
            currentBar.Left.Set(8f, 0f);
            currentBar.Top.Set(2, 0f);
            currentBar.Width.Set(width, 0f);
            currentBar.Height.Set(height - 4, 0f);

            switch (stat)
            {
                case ResourceBarMode.HP:
                    currentBar.backgroundColor = CustomResourceUI.RedHealthColor;

                    lifeFruitBar = new UIInnerBar();
                    lifeFruitBar.SetPadding(0);
                    lifeFruitBar.Left.Set(8f, 0f);
                    lifeFruitBar.Top.Set(2f, 0f);
                    lifeFruitBar.Width.Set(width, 0f);
                    lifeFruitBar.Height.Set(height - 4, 0f);
                    lifeFruitBar.backgroundColor = CustomResourceUI.OrangeHealthColor;

                    bonusHealthBar = new UIInnerBar();
                    bonusHealthBar.SetPadding(0);
                    bonusHealthBar.Left.Set(8f, 0f);
                    bonusHealthBar.Top.Set(2f, 0f);
                    bonusHealthBar.Width.Set(width, 0f);
                    bonusHealthBar.Height.Set(height - 4, 0f);
                    bonusHealthBar.backgroundColor = CustomResourceUI.BonusHealthColor;

                    shieldBar = new UIInnerBar(); 
                    shieldBar.SetPadding(0);
                    shieldBar.Left.Set(8f, 0f);
                    shieldBar.Top.Set(2f, 0f);
                    shieldBar.Width.Set(width, 0f);
                    shieldBar.Height.Set(height - 4, 0f);
                    shieldBar.backgroundColor = CustomResourceUI.BasicShieldColor;

                    magicShieldBar = new UIInnerBar();
                    magicShieldBar.SetPadding(0);
                    magicShieldBar.Left.Set(8f, 0f);
                    magicShieldBar.Top.Set(2f, 0f);
                    magicShieldBar.Width.Set(width, 0f);
                    magicShieldBar.Height.Set(height -4, 0f);
                    magicShieldBar.backgroundColor = CustomResourceUI.MagicShieldColor;

                    physShieldBar = new UIInnerBar();
                    physShieldBar.SetPadding(0);
                    physShieldBar.Left.Set(8f, 0f);
                    physShieldBar.Top.Set(2f, 0f);
                    physShieldBar.Width.Set(width, 0f);
                    physShieldBar.Height.Set(height -4, 0f);
                    physShieldBar.backgroundColor = CustomResourceUI.PhysicalShieldColor;

                    barBackground.Append(lifeFruitBar);
                    barBackground.Append(bonusHealthBar);
                    barBackground.Append(shieldBar);
                    barBackground.Append(magicShieldBar);
                    barBackground.Append(physShieldBar);
                    barBackground.Append(currentBar);

                    markers = InitaliseMarkers(2000/healthBarDividerDistance);

                    break;

                case ResourceBarMode.MANA:
                    currentBar.backgroundColor = CustomResourceUI.ManaColor;
                    barBackground.Append(currentBar);
                    markers = InitaliseMarkers(1000/manaBarDividerDistance);
                    break;
                default:
                    break;
            }

            text = new UIText("0|0"); 
            text.Width.Set(width, 0f);
            text.Height.Set(height, 0f);
            text.Top.Set((height / 2 - text.MinHeight.Pixels / 2), 0f);

            regen = new UIText("0/s", 0.75f);
            regen.Width.Set(width, 0f);
            regen.Height.Set(height, 0f);
            regen.Top.Set((height / 2 - text.MinHeight.Pixels / 2 + 3), 0f);
            regen.Left.Set(width / 2 - 26, 0);


            barBackground.Append(text);
            barBackground.Append(regen);
            base.Append(barBackground);
        }

        public ResourceBar(ResourceBarMode stat, int height, int width)
        {
            this.stat = stat;
            this.width = width;
            this.height = height;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Player player = Main.player[Main.myPlayer];
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();
            regen.Top.Set((Height.Pixels / 2 - text.MinHeight.Pixels / 2 + 3), 0f);
            regen.Left.Set(Width.Pixels / 2 - 26, 0);
            float quotient = 1;
            switch (stat)
            {
                case ResourceBarMode.HP:
                    int markerCount = player.statLifeMax2 / healthBarDividerDistance;
                    int markerEffectiveHealth = healthBarDividerDistance * markerCount;
                    float markerQuotent = markerEffectiveHealth / (player.statLifeMax2 * 1f);
                    for (int i = 0; i < markers.Length; i++)
                    {
                        float gap = (464 * markerQuotent) / markerCount;

                        markers[i].Left.Set(7 + gap + (gap * i), 0);
                        markers[i].ImageScale = (i >= markerCount ? 0 : 1);
                    }

                    int bonusLife = modPlayer.GetRealHeathWithoutShield(true) - (int)(player.statLifeMax);  // Max Bonus Life
                    int orangeLife = player.statLifeMax > 400 ? 5 * (player.statLifeMax - 400) : 0;         // Max Life Fruit Life
                    int redLife = player.statLifeMax - orangeLife;                                          // Max life Crystal Life

                    int currentBonusLife = bonusLife; 
                    int currentOrangeLife = orangeLife; 
                    int currentRedLife = redLife;

                    int missingLife = modPlayer.GetRealHeathWithoutShield(true) - modPlayer.GetRealHeathWithoutShield();
                    if (bonusLife < missingLife)
                    {
                        missingLife -= bonusLife;
                        currentBonusLife = 0;

                        if (redLife < missingLife)
                        {
                            missingLife -= redLife;
                            currentRedLife = 0;

                            if (orangeLife < missingLife)
                                currentOrangeLife = 0;
                            else
                                currentOrangeLife -= missingLife;
                        }
                        else
                        {
                            currentRedLife -= missingLife;
                        }
                    }
                    else
                    {
                        currentBonusLife -= missingLife;
                    }

                    float bonusQuotient = currentBonusLife / (float)player.statLifeMax2;
                    float orangeQuotient = currentOrangeLife / (float)player.statLifeMax2;
                    float redQuotient = currentRedLife / (float)player.statLifeMax2;
                    float normalQuotient = modPlayer.NormalShield / (float)player.statLifeMax2;
                    float physicalQuotient = modPlayer.PhysicalShield / (float)player.statLifeMax2;
                    float magicQuotient = modPlayer.MagicShield / (float)player.statLifeMax2;

                    lifeFruitBar.Left.Set(8, 0);
                    lifeFruitBar.Width.Set((width - 16) * orangeQuotient, 0f);

                    currentBar.Left.Set(lifeFruitBar.Left.Pixels + lifeFruitBar.Width.Pixels, 0);
                    currentBar.Width.Set((width - 16) * redQuotient, 0f);

                    bonusHealthBar.Left.Set(currentBar.Left.Pixels + currentBar.Width.Pixels, 0);
                    bonusHealthBar.Width.Set((width - 16) * bonusQuotient, 0f);

                    shieldBar.Left.Set(bonusHealthBar.Left.Pixels + bonusHealthBar.Width.Pixels,0);
                    shieldBar.Width.Set((width - 16) * normalQuotient, 0);

                    physShieldBar.Left.Set(shieldBar.Left.Pixels + shieldBar.Width.Pixels, 0);
                    physShieldBar.Width.Set((width - 16) * physicalQuotient, 0);

                    magicShieldBar.Left.Set(physShieldBar.Left.Pixels + physShieldBar.Width.Pixels, 0);
                    magicShieldBar.Width.Set((width - 16) * magicQuotient, 0);
                    break;

                case ResourceBarMode.MANA:

                    int manaMarkerCount = player.statManaMax2 / manaBarDividerDistance;
                    int markerEffectiveMana = manaBarDividerDistance * manaMarkerCount;
                    float manaMarkerQuotent = markerEffectiveMana / (player.statManaMax2 * 1f);
                    for (int i = 0; i < markers.Length; i++)
                    {
                        float gap = (464 * manaMarkerQuotent) / manaMarkerCount;

                        markers[i].Left.Set(7 + gap + (gap * i), 0);
                        markers[i].ImageScale = (i >= manaMarkerCount ? 0 : 1);
                    }

                    if (player.statMana >= player.statManaMax2)
                        quotient = 1;
                    else
                        quotient = (float)player.statMana / (float)player.statManaMax2;
                    currentBar.Width.Set(quotient * width - 16, 0f);
                    break;

                default:
                    break;
            }

            
            Recalculate(); 

            base.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            Player player = Main.LocalPlayer; 
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();

            if (modPlayer.GetTotalShield() > 0 && modPlayer.PureHealthLastStep > modPlayer.GetRealHeathWithoutShield())
            {
                int hpDiff = modPlayer.PureHealthLastStep - modPlayer.GetRealHeathWithoutShield();
                int damageToDeal = hpDiff;

                while (damageToDeal > 0 && modPlayer.Shields.Count != 0)
                {
                    if (modPlayer.wasHitByProjOrNPCLastStep == "Proj")
                    {
                        if (modPlayer.MagicShield > 0)
                        {
                            int ShieldNum = -1;

                            for (int i = 0; i < modPlayer.Shields.Count; i++)
                            {
                                if (modPlayer.Shields[i].ShieldType == ShieldType.Magic)
                                {
                                    ShieldNum = i;
                                    break;
                                }
                            }

                            if (ShieldNum != -1)
                            {
                                Shield currentShield = modPlayer.Shields[ShieldNum];

                                if (currentShield.ShieldAmount <= damageToDeal)
                                {
                                    damageToDeal -= currentShield.ShieldAmount;
                                    modPlayer.Shields.RemoveAt(ShieldNum);
                                }
                                else
                                {
                                    modPlayer.Shields[ShieldNum] = new Shield(currentShield.ShieldAmount - damageToDeal, currentShield.ShieldColor, currentShield.AssociatedBuff, currentShield.ShieldType, currentShield.ShieldTimeLeft);
                                    player.statLife += damageToDeal;
                                    break;
                                }
                            }
                            else
                            {
                                modPlayer.wasHitByProjOrNPCLastStep = "None";
                            }
                        }
                        else
                        {
                            modPlayer.wasHitByProjOrNPCLastStep = "None";
                        }
                    }
                    if (modPlayer.wasHitByProjOrNPCLastStep == "NPC")
                    {
                        if (modPlayer.PhysicalShield > 0)
                        {
                            int ShieldNum = -1;

                            for (int i = 0; i < modPlayer.Shields.Count; i++)
                            {
                                if (modPlayer.Shields[i].ShieldType == ShieldType.Physical)
                                {
                                    ShieldNum = i;
                                    break;
                                }
                            }

                            if (ShieldNum != -1)
                            {
                                Shield currentShield = modPlayer.Shields[ShieldNum];

                                if (modPlayer.Shields[0].ShieldAmount <= damageToDeal)
                                {
                                    damageToDeal -= modPlayer.Shields[ShieldNum].ShieldAmount;
                                    modPlayer.Shields.RemoveAt(ShieldNum);
                                }
                                else
                                {
                                    modPlayer.Shields[ShieldNum] = new Shield(currentShield.ShieldAmount - damageToDeal, currentShield.ShieldColor, currentShield.AssociatedBuff, currentShield.ShieldType, currentShield.ShieldTimeLeft);
                                    player.statLife += damageToDeal;
                                    break;
                                }
                            }
                            else
                            {
                                modPlayer.wasHitByProjOrNPCLastStep = "None";
                            }
                        }
                        else
                        {
                            modPlayer.wasHitByProjOrNPCLastStep = "None";
                        }
                    }
                    if (modPlayer.wasHitByProjOrNPCLastStep == "None")
                    {
                        int ShieldNum = -1;


                        if (player.lifeRegen < 0)
                        {
                            for (int i = 0; i < modPlayer.Shields.Count; i++)
                            {
                                if (modPlayer.Shields[i].ShieldType != ShieldType.Magic)
                                {
                                    ShieldNum = i;
                                    break;
                                }
                            }
                        }
                        else
                        {

                            for (int i = 0; i < modPlayer.Shields.Count; i++)
                            {
                                if (modPlayer.Shields[i].ShieldType != ShieldType.Physical && modPlayer.Shields[i].ShieldType != ShieldType.Magic)
                                {
                                    ShieldNum = i;
                                    break;
                                }
                            }
                        }

                        if (ShieldNum != -1)
                        {
                            Shield currentShield = modPlayer.Shields[ShieldNum];

                            if (modPlayer.Shields[0].ShieldAmount <= damageToDeal)
                            {
                                damageToDeal -= modPlayer.Shields[ShieldNum].ShieldAmount;
                                modPlayer.Shields.RemoveAt(ShieldNum);
                            }
                            else
                            {
                                modPlayer.Shields[ShieldNum] = new Shield(currentShield.ShieldAmount - damageToDeal, currentShield.ShieldColor, currentShield.AssociatedBuff, currentShield.ShieldType, currentShield.ShieldTimeLeft);
                                player.statLife += damageToDeal;
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                player.statLife += hpDiff - damageToDeal;
                modPlayer.wasHitByProjOrNPCLastStep = "None";
            }

            switch (stat)
            {
                case ResourceBarMode.HP:
                    if (modPlayer.GetTotalShield() > 0)
                        text.SetText("" + modPlayer.GetRealHeathWithoutShield() + "+(" + modPlayer.GetTotalShield() + ") / " + (modPlayer.GetRealHeathWithoutShield(true))); //Set Life
                    else
                        text.SetText("" + player.statLife + " / " + player.statLifeMax2);

                    regen.SetText((double)player.lifeRegen/2 + "/s", 0.6f, false);
                    break;

                case ResourceBarMode.MANA:
                    text.SetText("" + player.statMana + " / " + player.statManaMax2); //Set Mana
                    if (TerraLeague.UseCustomManaRegen)
                        regen.SetText(modPlayer.manaRegen + "/s", 0.6f, false);
                    else
                        regen.SetText("", 0.6f, false);
                    break;

                default:
                    break;
            }

            if (IsMouseHovering)
            {
                string tooltip = "";
                if (stat == ResourceBarMode.HP)
                {
                    int orangeLife = player.statLifeMax > 400 ? 5 * (player.statLifeMax - 400) : 0;
                    int redLife = player.statLifeMax - orangeLife;

                    tooltip += LeagueTooltip.CreateColorString(TerraLeague.TooltipHeadingColor, "Health Bar");
                    tooltip += "\nMax life consists of:";
                    if (redLife > 0)
                        tooltip += "\n" + LeagueTooltip.CreateColorString(CustomResourceUI.RedHealthColor, "Heart Crystals - " + redLife);
                    if (orangeLife > 0)
                        tooltip += "\n" + LeagueTooltip.CreateColorString(CustomResourceUI.OrangeHealthColor, "Life Fruit - " + orangeLife);
                    if (player.statLifeMax2 > 0)
                        tooltip += "\n" + LeagueTooltip.CreateColorString(CustomResourceUI.BonusHealthColor, "Bonus Health - " + (modPlayer.GetRealHeathWithoutShield(true) - player.statLifeMax));

                    ToolTipUI.SetText(tooltip.Split('\n'));
                }
                else if (stat == ResourceBarMode.MANA)
                {
                    tooltip += LeagueTooltip.CreateColorString(TerraLeague.TooltipHeadingColor, "Mana Bar");
                    tooltip += "\nMax mana consists of:";
                        tooltip += "\n" + LeagueTooltip.CreateColorString(CustomResourceUI.ManaColor, "Mana Crystals - " + player.statManaMax);
                    if (player.statManaMax2 > 0)
                        tooltip += "\n" + LeagueTooltip.CreateColorString(CustomResourceUI.ManaColor, "Bonus Mana - " + (player.statManaMax2 - player.statManaMax));

                    ToolTipUI.SetText(tooltip.Split('\n'));
                }
            }

            base.Update(gameTime);
        }

        UIImage[] InitaliseMarkers(int length)
        {
            markers = new UIImage[length];
            for (int i = 0; i < markers.Length; i++)
            {
                markers[i] = new UIImage(ModContent.Request<Texture2D>("TerraLeague/UI/BarMarker"));
                barBackground.Append(markers[i]);
                markers[i].Top.Set(2, 0);
            }
            return markers;
        }
    }

    internal enum ResourceBarMode
    {
        HP,
        MANA
    }

    
}
