using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

public abstract class WormClass : ModNPC
{
    /* ai[0] = follower
     * ai[1] = following
     * ai[2] = distanceFromTail
     * ai[3] = head
     */
    public bool head;
    public bool tail;
    public int minLength;
    public int maxLength;
    public int headType;
    public int bodyType;
    public int tailType;
    public bool flies = false;
    public bool directional = false;
    public float speed;
    public float turnSpeed;

    public override void AI()
    {
        if (NPC.localAI[1] == 0f)
        {
            NPC.localAI[1] = 1f;
            Init();
        }
        if (NPC.ai[3] > 0f)
        {
            NPC.realLife = (int)NPC.ai[3];
        }
        if (!head && NPC.timeLeft < 300)
        {
            NPC.timeLeft = 300;
        }
        if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead)
        {
            NPC.TargetClosest(true);
        }
        if (Main.player[NPC.target].dead && NPC.timeLeft > 300)
        {
            NPC.timeLeft = 300;
        }
        if (Main.netMode != NetmodeID.MultiplayerClient)
        {
            if (!tail && NPC.ai[0] == 0f)
            {
                if (head)
                {
                    NPC.ai[3] = (float)NPC.whoAmI;
                    NPC.realLife = NPC.whoAmI;
                    NPC.ai[2] = (float)Main.rand.Next(minLength, maxLength + 1);
                    NPC.ai[0] = (float)NPC.NewNPC((int)(NPC.position.X + (float)(NPC.width / 2)), (int)(NPC.position.Y + (float)NPC.height), bodyType, NPC.whoAmI);
                }
                else if (NPC.ai[2] > 0f)
                {
                    NPC.ai[0] = (float)NPC.NewNPC((int)(NPC.position.X + (float)(NPC.width / 2)), (int)(NPC.position.Y + (float)NPC.height), NPC.type, NPC.whoAmI);
                }
                else
                {
                    NPC.ai[0] = (float)NPC.NewNPC((int)(NPC.position.X + (float)(NPC.width / 2)), (int)(NPC.position.Y + (float)NPC.height), tailType, NPC.whoAmI);
                }
                Main.npc[(int)NPC.ai[0]].ai[3] = NPC.ai[3];
                Main.npc[(int)NPC.ai[0]].realLife = NPC.realLife;
                Main.npc[(int)NPC.ai[0]].ai[1] = (float)NPC.whoAmI;
                Main.npc[(int)NPC.ai[0]].ai[2] = NPC.ai[2] - 1f;
                NPC.netUpdate = true;
            }
            if (!head && (!Main.npc[(int)NPC.ai[1]].active || Main.npc[(int)NPC.ai[1]].type != headType && Main.npc[(int)NPC.ai[1]].type != bodyType))
            {
                NPC.life = 0;
                NPC.HitEffect(0, 10.0);
                NPC.active = false;
            }
            if (!tail && (!Main.npc[(int)NPC.ai[0]].active || Main.npc[(int)NPC.ai[0]].type != bodyType && Main.npc[(int)NPC.ai[0]].type != tailType))
            {
                NPC.life = 0;
                NPC.HitEffect(0, 10.0);
                NPC.active = false;
            }
            if (!NPC.active && Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.DamageNPC, -1, -1, null, NPC.whoAmI, -1f, 0f, 0f, 0, 0, 0);
            }
        }
        int NPCXTile_Left = (int)(NPC.position.X / 16f) - 1;
        int NPCXTile_Right = (int)((NPC.position.X + (float)NPC.width) / 16f) + 2;
        int NPCYTile_Up = (int)(NPC.position.Y / 16f) - 1;
        int NPCTile_Down = (int)((NPC.position.Y + (float)NPC.height) / 16f) + 2;
        if (NPCXTile_Left < 0)
        {
            NPCXTile_Left = 0;
        }
        if (NPCXTile_Right > Main.maxTilesX)
        {
            NPCXTile_Right = Main.maxTilesX;
        }
        if (NPCYTile_Up < 0)
        {
            NPCYTile_Up = 0;
        }
        if (NPCTile_Down > Main.maxTilesY)
        {
            NPCTile_Down = Main.maxTilesY;
        }
        bool canCurrentlyDig = flies;
        if (!canCurrentlyDig)
        {
            for (int x = NPCXTile_Left; x < NPCXTile_Right; x++)
            {
                for (int y = NPCYTile_Up; y < NPCTile_Down; y++)
                {
                    if (Main.tile[x, y] != null && (Main.tile[x, y].IsActiveUnactuated && (Main.tileSolid[(int)Main.tile[x, y].type] || Main.tileSolidTop[(int)Main.tile[x, y].type] && Main.tile[x, y].frameY == 0) || Main.tile[x, y].LiquidAmount > 64))
                    {
                        //if (NPC.position.X + (float)NPC.width > tilePosition.X && NPC.position.X < tilePosition.X + 16f && NPC.position.Y + (float)NPC.height > tilePosition.Y && NPC.position.Y < tilePosition.Y + 16f)
                        if (NPC.Hitbox.Intersects(new Rectangle(x * 16, y * 16, 16, 16)))
                        {
                            canCurrentlyDig = true;
                            //if (Main.rand.NextBool(100) && NPC.behindTiles && Main.tile[x, y].IsActuated)
                            //{
                            //    WorldGen.KillTile(x, y, true, true, false);
                            //}
                        }
                    }
                }
            }
        }
        if (!canCurrentlyDig && head)
        {
            Rectangle rectangle = new Rectangle((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height);
            int radius = 1000;
            bool wormNotInPlayerArea = true;
            for (int i = 0; i < 255; i++)
            {
                if (Main.player[i].active)
                {
                    Rectangle playerArea = new Rectangle((int)Main.player[i].position.X - radius, (int)Main.player[i].position.Y - radius, radius * 2, radius * 2);
                    if (rectangle.Intersects(playerArea))
                    {
                        wormNotInPlayerArea = false;
                        break;
                    }
                }
            }
            if (wormNotInPlayerArea)
            {
                canCurrentlyDig = true;
            }
        }
        if (directional)
        {
            if (NPC.velocity.X < 0f)
            {
                NPC.spriteDirection = 1;
            }
            else if (NPC.velocity.X > 0f)
            {
                NPC.spriteDirection = -1;
            }
        }
        Vector2 NPCCenter = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
        float TargetCenterX = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2);
        float TargetCenterY = Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2);
        TargetCenterX = (float)((int)(TargetCenterX / 16f) * 16);
        TargetCenterY = (float)((int)(TargetCenterY / 16f) * 16);
        NPCCenter.X = (float)((int)(NPCCenter.X / 16f) * 16);
        NPCCenter.Y = (float)((int)(NPCCenter.Y / 16f) * 16);
        TargetCenterX -= NPCCenter.X;
        TargetCenterY -= NPCCenter.Y;
        float MagnitudeToTarget = (float)System.Math.Sqrt((double)(TargetCenterX * TargetCenterX + TargetCenterY * TargetCenterY));
        if (NPC.ai[1] > 0f && NPC.ai[1] < (float)Main.npc.Length)
        {
            try
            {
                NPCCenter = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
                TargetCenterX = Main.npc[(int)NPC.ai[1]].position.X + (float)(Main.npc[(int)NPC.ai[1]].width / 2) - NPCCenter.X;
                TargetCenterY = Main.npc[(int)NPC.ai[1]].position.Y + (float)(Main.npc[(int)NPC.ai[1]].height / 2) - NPCCenter.Y;
            }
            catch
            {
            }
            NPC.rotation = (float)System.Math.Atan2((double)TargetCenterY, (double)TargetCenterX) + 1.57f;
            MagnitudeToTarget = (float)System.Math.Sqrt((double)(TargetCenterX * TargetCenterX + TargetCenterY * TargetCenterY));
            int NPCWidth = NPC.width;
            MagnitudeToTarget = (MagnitudeToTarget - (float)NPCWidth) / MagnitudeToTarget;
            TargetCenterX *= MagnitudeToTarget;
            TargetCenterY *= MagnitudeToTarget;
            NPC.velocity = Vector2.Zero;
            NPC.position.X = NPC.position.X + TargetCenterX;
            NPC.position.Y = NPC.position.Y + TargetCenterY;
            if (directional)
            {
                if (TargetCenterX < 0f)
                {
                    NPC.spriteDirection = 1;
                }
                if (TargetCenterX > 0f)
                {
                    NPC.spriteDirection = -1;
                }
            }
        }
        else
        {
            if (!canCurrentlyDig)
            {
                NPC.TargetClosest(true);
                NPC.velocity.Y = NPC.velocity.Y + 0.11f;
                if (NPC.velocity.Y > speed)
                {
                    NPC.velocity.Y = speed;
                }
                if ((double)(System.Math.Abs(NPC.velocity.X) + System.Math.Abs(NPC.velocity.Y)) < (double)speed * 0.4)
                {
                    if (NPC.velocity.X < 0f)
                    {
                        NPC.velocity.X = NPC.velocity.X - turnSpeed * 1.1f;
                    }
                    else
                    {
                        NPC.velocity.X = NPC.velocity.X + turnSpeed * 1.1f;
                    }
                }
                else if (NPC.velocity.Y == speed)
                {
                    if (NPC.velocity.X < TargetCenterX)
                    {
                        NPC.velocity.X = NPC.velocity.X + turnSpeed;
                    }
                    else if (NPC.velocity.X > TargetCenterX)
                    {
                        NPC.velocity.X = NPC.velocity.X - turnSpeed;
                    }
                }
                else if (NPC.velocity.Y > 4f)
                {
                    if (NPC.velocity.X < 0f)
                    {
                        NPC.velocity.X = NPC.velocity.X + turnSpeed * 0.9f;
                    }
                    else
                    {
                        NPC.velocity.X = NPC.velocity.X - turnSpeed * 0.9f;
                    }
                }
            }
            else
            {
                if (!flies && NPC.behindTiles && NPC.soundDelay == 0)
                {
                    float num195 = MagnitudeToTarget / 40f;
                    if (num195 < 10f)
                    {
                        num195 = 10f;
                    }
                    if (num195 > 20f)
                    {
                        num195 = 20f;
                    }
                    NPC.soundDelay = (int)num195;
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Roar, NPC.position, 1);
                }
                MagnitudeToTarget = (float)System.Math.Sqrt((double)(TargetCenterX * TargetCenterX + TargetCenterY * TargetCenterY));
                float num196 = System.Math.Abs(TargetCenterX);
                float num197 = System.Math.Abs(TargetCenterY);
                float num198 = speed / MagnitudeToTarget;
                TargetCenterX *= num198;
                TargetCenterY *= num198;
                if (ShouldRun())
                {
                    bool flag20 = true;
                    for (int num199 = 0; num199 < 255; num199++)
                    {
                        if (Main.player[num199].active && !Main.player[num199].dead && Main.player[num199].ZoneCorrupt)
                        {
                            flag20 = false;
                        }
                    }
                    if (flag20)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient && (double)(NPC.position.Y / 16f) > (Main.rockLayer + (double)Main.maxTilesY) / 2.0)
                        {
                            NPC.active = false;
                            int num200 = (int)NPC.ai[0];
                            while (num200 > 0 && num200 < 200 && Main.npc[num200].active && Main.npc[num200].aiStyle == NPC.aiStyle)
                            {
                                int num201 = (int)Main.npc[num200].ai[0];
                                Main.npc[num200].active = false;
                                NPC.life = 0;
                                if (Main.netMode == NetmodeID.Server)
                                {
                                    NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, num200, 0f, 0f, 0f, 0, 0, 0);
                                }
                                num200 = num201;
                            }
                            if (Main.netMode == NetmodeID.Server)
                            {
                                NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, NPC.whoAmI, 0f, 0f, 0f, 0, 0, 0);
                            }
                        }
                        TargetCenterX = 0f;
                        TargetCenterY = speed;
                    }
                }
                bool flag21 = false;
                
                if (!flag21)
                {
                    if (NPC.velocity.X > 0f && TargetCenterX > 0f || NPC.velocity.X < 0f && TargetCenterX < 0f || NPC.velocity.Y > 0f && TargetCenterY > 0f || NPC.velocity.Y < 0f && TargetCenterY < 0f)
                    {
                        if (NPC.velocity.X < TargetCenterX)
                        {
                            NPC.velocity.X = NPC.velocity.X + turnSpeed;
                        }
                        else
                        {
                            if (NPC.velocity.X > TargetCenterX)
                            {
                                NPC.velocity.X = NPC.velocity.X - turnSpeed;
                            }
                        }
                        if (NPC.velocity.Y < TargetCenterY)
                        {
                            NPC.velocity.Y = NPC.velocity.Y + turnSpeed;
                        }
                        else
                        {
                            if (NPC.velocity.Y > TargetCenterY)
                            {
                                NPC.velocity.Y = NPC.velocity.Y - turnSpeed;
                            }
                        }
                        if ((double)System.Math.Abs(TargetCenterY) < (double)speed * 0.2 && (NPC.velocity.X > 0f && TargetCenterX < 0f || NPC.velocity.X < 0f && TargetCenterX > 0f))
                        {
                            if (NPC.velocity.Y > 0f)
                            {
                                NPC.velocity.Y = NPC.velocity.Y + turnSpeed * 2f;
                            }
                            else
                            {
                                NPC.velocity.Y = NPC.velocity.Y - turnSpeed * 2f;
                            }
                        }
                        if ((double)System.Math.Abs(TargetCenterX) < (double)speed * 0.2 && (NPC.velocity.Y > 0f && TargetCenterY < 0f || NPC.velocity.Y < 0f && TargetCenterY > 0f))
                        {
                            if (NPC.velocity.X > 0f)
                            {
                                NPC.velocity.X = NPC.velocity.X + turnSpeed * 2f;
                            }
                            else
                            {
                                NPC.velocity.X = NPC.velocity.X - turnSpeed * 2f;
                            }
                        }
                    }
                    else
                    {
                        if (num196 > num197)
                        {
                            if (NPC.velocity.X < TargetCenterX)
                            {
                                NPC.velocity.X = NPC.velocity.X + turnSpeed * 1.1f;
                            }
                            else if (NPC.velocity.X > TargetCenterX)
                            {
                                NPC.velocity.X = NPC.velocity.X - turnSpeed * 1.1f;
                            }
                            if ((double)(System.Math.Abs(NPC.velocity.X) + System.Math.Abs(NPC.velocity.Y)) < (double)speed * 0.5)
                            {
                                if (NPC.velocity.Y > 0f)
                                {
                                    NPC.velocity.Y = NPC.velocity.Y + turnSpeed;
                                }
                                else
                                {
                                    NPC.velocity.Y = NPC.velocity.Y - turnSpeed;
                                }
                            }
                        }
                        else
                        {
                            if (NPC.velocity.Y < TargetCenterY)
                            {
                                NPC.velocity.Y = NPC.velocity.Y + turnSpeed * 1.1f;
                            }
                            else if (NPC.velocity.Y > TargetCenterY)
                            {
                                NPC.velocity.Y = NPC.velocity.Y - turnSpeed * 1.1f;
                            }
                            if ((double)(System.Math.Abs(NPC.velocity.X) + System.Math.Abs(NPC.velocity.Y)) < (double)speed * 0.5)
                            {
                                if (NPC.velocity.X > 0f)
                                {
                                    NPC.velocity.X = NPC.velocity.X + turnSpeed;
                                }
                                else
                                {
                                    NPC.velocity.X = NPC.velocity.X - turnSpeed;
                                }
                            }
                        }
                    }
                }
            }
            NPC.rotation = (float)System.Math.Atan2((double)NPC.velocity.Y, (double)NPC.velocity.X) + 1.57f;
            if (head)
            {
                if (canCurrentlyDig)
                {
                    if (NPC.localAI[0] != 1f)
                    {
                        NPC.netUpdate = true;
                    }
                    NPC.localAI[0] = 1f;
                }
                else
                {
                    if (NPC.localAI[0] != 0f)
                    {
                        NPC.netUpdate = true;
                    }
                    NPC.localAI[0] = 0f;
                }
                if ((NPC.velocity.X > 0f && NPC.oldVelocity.X < 0f || NPC.velocity.X < 0f && NPC.oldVelocity.X > 0f || NPC.velocity.Y > 0f && NPC.oldVelocity.Y < 0f || NPC.velocity.Y < 0f && NPC.oldVelocity.Y > 0f) && !NPC.justHit)
                {
                    NPC.netUpdate = true;
                    return;
                }
            }
        }
        CustomBehavior();
    }

    public virtual void Init()
    {
    }

    public virtual bool ShouldRun()
    {
        return false;
    }

    public virtual void CustomBehavior()
    {
        
    }

    public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
    {
        return head ? (bool?)null : false;
    }
}