using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;

namespace TerraLeague
{
    public static class Targeting
    {
        public static void ForceNPCStoRetarget(Player player)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                player.GetModPlayer<PLAYERGLOBAL>().PacketHandler.SendRetarget(-1, -1, player.whoAmI);
            }
            else
            {
                for (int i = 0; i < Main.npc.Length; i++)
                {
                    NPC npc = Main.npc[i];

                    if (npc.active)
                    {
                        if (npc.target == player.whoAmI)
                        {
                            npc.TargetClosest();
                            npc.netUpdate = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns if 2 points are within a specified range of eachother
        /// </summary>
        /// <param name="startingPoint"></param>
        /// <param name="targetPoint"></param>
        /// <param name="Range"></param>
        /// <returns></returns>
        public static bool IsPointWithinRange(Vector2 startingPoint, Vector2 targetPoint, float range)
        {
            return Vector2.Distance(startingPoint, targetPoint) <= range;
        }

        /// <summary>
        /// Returns if a hitbox intersects with the range
        /// </summary>
        /// <param name="startingPoint"></param>
        /// <param name="hitbox"></param>
        /// <param name="Range"></param>
        /// <returns></returns>
        public static bool IsHitboxWithinRange(Vector2 startingPoint, Rectangle hitbox, float range)
        {
            if (IsPointWithinRange(hitbox.TopLeft(), startingPoint, range))
            {
                return true;
            }
            else if (IsPointWithinRange(hitbox.Right(), startingPoint, range))
            {
                return true;
            }
            else if (IsPointWithinRange(hitbox.TopRight(), startingPoint, range))
            {
                return true;
            }
            else if (IsPointWithinRange(hitbox.Left(), startingPoint, range))
            {
                return true;
            }
            else if (IsPointWithinRange(hitbox.BottomLeft(), startingPoint, range))
            {
                return true;
            }
            else if (IsPointWithinRange(hitbox.Top(), startingPoint, range))
            {
                return true;
            }
            else if (IsPointWithinRange(hitbox.BottomRight(), startingPoint, range))
            {
                return true;
            }
            else if (IsPointWithinRange(hitbox.Bottom(), startingPoint, range))
            {
                return true;
            }
            else if (hitbox.Intersects(new Rectangle((int)startingPoint.X, (int)startingPoint.Y, 1, 1)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// <para>Checks if the mouse is hovering over an NPC</para>
        /// Returns -1 if mouse was not hovering
        /// </summary>
        /// <param name="mouseLength">Width and height of the mouse hitbox</param>
        /// <param name="includeCritters">Include small animals in the check</param>
        /// <param name="includeTownNPCS">Include Town NPCs in the check</param>
        /// <returns></returns>
        public static int NPCMouseIsHovering(int mouseLength = 30, bool includeCritters = false, bool includeTownNPCS = false)
        {
            for (int i = 0; i < Main.npc.Length; i++)
            {
                if (!Main.npc[i].dontTakeDamage)
                {
                    if (!includeCritters && Main.npc[i].lifeMax != 5 && !Main.npc[i].friendly || !includeTownNPCS && !Main.npc[i].townNPC)
                    {
                        if (Main.npc[i].Hitbox.Intersects(new Rectangle((int)Main.MouseWorld.X - mouseLength / 2, (int)Main.MouseWorld.Y - mouseLength / 2, mouseLength, mouseLength)) && !Main.npc[i].immortal && Main.npc[i].active)
                        {
                            return i;
                        }
                    }
                }
            }

            return -1;
        }

        /// <summary>
        /// <para>Checks if the mouse is hovering over a Player</para>
        /// Returns -1 if mouse was not hovering
        /// </summary>
        /// <param name="mouseLength">Width and height of the mouse hitbox</param>
        /// <returns></returns>
        public static int PlayerMouseIsHovering(int mouseLength = 30, int doNotInclude = -1, int isOnTeam = -1, bool canBeDead = false)
        {
            int target = GetClosestPlayer(Main.MouseWorld, mouseLength / 2, doNotInclude, isOnTeam, -1, canBeDead);

            return target;
        }

        /// <summary>
        /// Returns a list of all NPCs array positions within a circular area
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <param name="includeSmallCreatures"></param>
        /// <param name="includeTownNPCs"></param>
        /// <param name="includeImmortal"></param>
        /// <returns></returns>
        public static List<int> GetAllNPCsInRange(Vector2 center, float radius, bool includeSmallCreatures = false, bool includeTargetDummy = false, bool includeTownNPCs = false, bool includeImmortal = false)
        {
            List<int> npcsInRange = new List<int>();
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active)
                {
                    if (npc.type == NPCID.TargetDummy && includeTargetDummy)
                    {
                        if (IsHitboxWithinRange(center, npc.Hitbox, radius))
                        {
                            npcsInRange.Add(i);
                        }
                    }
                    else if (!includeTownNPCs && !npc.townNPC || includeTownNPCs)
                    {
                        if (!includeSmallCreatures && npc.lifeMax > 5 || includeSmallCreatures)
                        {
                            if (!includeImmortal && !npc.immortal || includeImmortal)
                            {
                                if (IsHitboxWithinRange(center, npc.Hitbox, radius))
                                {
                                    npcsInRange.Add(i);
                                }
                            }
                        }
                    }
                }
            }

            return npcsInRange;
        }

        /// <summary>
        /// Get a list of all players within a set range
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <param name="doNotInclude"></param>
        /// <param name="isOnTeam"></param>
        /// <param name="canBeDead"></param>
        /// <returns></returns>
        public static List<int> GetAllPlayersInRange(Vector2 center, float radius, int doNotInclude = -1, int isOnTeam = -1, bool canBeDead = false)
        {
            List<int> playersInRange = new List<int>();
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                if (i != doNotInclude)
                {
                    if (player.active)
                    {
                        if (!player.dead && !canBeDead || canBeDead)
                        {
                            if (player.team == isOnTeam && isOnTeam != -1 || isOnTeam == -1)
                            {
                                if (IsHitboxWithinRange(center, player.Hitbox, radius))
                                {
                                    playersInRange.Add(i);
                                }
                            }
                        }
                    }
                }
            }

            return playersInRange;
        }

        /// <summary>
        /// Get the closest Player within a set range
        /// </summary>
        /// <param name="position"></param>
        /// <param name="maxDistance"></param>
        /// <param name="doNotInclude"></param>
        /// <param name="isOnTeam"></param>
        /// <param name="canBeDead"></param>
        /// <returns></returns>
        public static int GetClosestPlayer(Vector2 position, float maxDistance, int doNotInclude = -1, int isOnTeam = -1, int prioritisePlayer = -1, bool canBeDead = false)
        {
            int currentChoice = -1;
            float range = maxDistance;

            if (prioritisePlayer != -1)
            {
                Player player = Main.player[prioritisePlayer];
                if (IsHitboxWithinRange(position, player.Hitbox, range))
                {
                    return currentChoice;
                }
            }

            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                if (i != doNotInclude)
                {
                    if (player.active)
                    {
                        if (!player.dead && !canBeDead || canBeDead)
                        {
                            if (player.team == isOnTeam && isOnTeam != -1 || isOnTeam == -1)
                            {
                                if (IsHitboxWithinRange(position, player.Hitbox, range))
                                {
                                    currentChoice = i;
                                    range = Vector2.Distance(position, player.Center);
                                }
                            }
                        }
                    }
                }
            }

            return currentChoice;
        }

        /// <summary>
        /// Gives all NPCs withing a circular area a buff
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <param name="buff"></param>
        /// <param name="buffDuration"></param>
        /// <param name="includeSmallCreatures"></param>
        /// <param name="includeTownNPCs"></param>
        /// <param name="includeImmortal"></param>
        public static void GiveNPCsInRangeABuff(Vector2 center, float radius, int buff, int buffDuration, bool includeSmallCreatures = false, bool includeTargetDummy = false, bool includeTownNPCs = false, bool includeImmortal = false)
        {
            List<int> npcs = GetAllNPCsInRange(center, radius, includeSmallCreatures, includeTargetDummy, includeTownNPCs, includeImmortal);

            for (int i = 0; i < npcs.Count; i++)
            {
                Main.npc[npcs[i]].AddBuff(buff, buffDuration);
            }
        }

        /// <summary>
        /// Returns if there is at least 1 NPC in range (Can check if it has a buff too)
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <param name="hasBuffType"></param>
        /// <returns></returns>
        public static bool IsThereAnNPCInRange(Vector2 center, float radius, int hasBuffType = -1)
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && !npc.immortal && !npc.townNPC)
                {
                    if (IsHitboxWithinRange(center, npc.Hitbox, radius))
                    {
                        if (hasBuffType != -1)
                        {
                            if (npc.HasBuff(hasBuffType))
                            {
                                return true;
                            }
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Gets the closests NPC to a point
        /// </summary>
        /// <param name="position"></param>
        /// <param name="doNotInclude"></param>
        /// <param name="includeSmallCreatures"></param>
        /// <param name="includeTownNPCs"></param>
        /// <param name="includeImmortal"></param>
        /// <returns></returns>
        public static int GetClosestNPC(Vector2 position, float maxDistance, int doNotInclude = -1, int prioritiseNPC = -1, bool includeSmallCreatures = false, bool includeTargetDummy = false, bool includeTownNPCs = false, bool includeImmortal = false)
        {
            int currentChoice = -1;
            float range = maxDistance;

            if (prioritiseNPC != -1)
            {
                NPC npc = Main.npc[prioritiseNPC];
                if (IsHitboxWithinRange(position, npc.Hitbox, range))
                {
                    return prioritiseNPC;
                }
            }

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];

                if (npc.active && npc.whoAmI != doNotInclude)
                {
                    if (npc.type == NPCID.TargetDummy && includeTargetDummy)
                    {
                        if (IsHitboxWithinRange(position, npc.Hitbox, range))
                        {
                            currentChoice = i;
                            range = Vector2.Distance(position, npc.Center);
                        }
                    }
                    else if (!includeTownNPCs && !npc.townNPC || includeTownNPCs)
                    {
                        if (!includeSmallCreatures && npc.lifeMax > 5 || includeSmallCreatures)
                        {
                            if (!includeImmortal && !npc.immortal || includeImmortal)
                            {
                                if (IsHitboxWithinRange(position, npc.Hitbox, range))
                                {
                                    currentChoice = i;
                                    range = Vector2.Distance(position, npc.Center);
                                }
                            }
                        }
                    }
                }
            }

            return currentChoice;
        }

        /// <summary>
        /// Gets the closests NPC to a point not including the given array
        /// </summary>
        /// <param name="position"></param>
        /// <param name="doNotInclude"></param>
        /// <param name="includeSmallCreatures"></param>
        /// <param name="includeTownNPCs"></param>
        /// <param name="includeImmortal"></param>
        /// <returns></returns>
        public static int GetClosestNPC(Vector2 position, float maxDistance, int[] doNotInclude, bool includeSmallCreatures = false, bool includeTargetDummy = false, bool includeTownNPCs = false, bool includeImmortal = false)
        {
            int currentChoice = -1;
            float range = maxDistance;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && !doNotInclude.Contains(npc.whoAmI))
                {
                    if (npc.type == NPCID.TargetDummy && includeTargetDummy)
                    {
                        if (IsHitboxWithinRange(position, npc.Hitbox, range))
                        {
                            currentChoice = i;
                            range = Vector2.Distance(position, npc.Center);
                        }
                    }
                    else if (!includeTownNPCs && !npc.townNPC || includeTownNPCs)
                    {
                        if (!includeSmallCreatures && npc.lifeMax > 5 || includeSmallCreatures)
                        {
                            if (!includeImmortal && !npc.immortal || includeImmortal)
                            {
                                if (IsHitboxWithinRange(position, npc.Hitbox, range))
                                {
                                    currentChoice = i;
                                    range = Vector2.Distance(position, npc.Center);
                                }
                            }
                        }
                    }
                }
            }

            return currentChoice;
        }
        public static int GetClosestNPC(Vector2 position, float maxDistance, Vector2 collisionPosition, int collitionWidth, int collitionHeight, int doNotInclude = -1, int prioritiseNPC = -1, bool includeSmallCreatures = false, bool includeTargetDummy = false, bool includeTownNPCs = false, bool includeImmortal = false)
        {
            int currentChoice = -1;
            float range = maxDistance;

            if (prioritiseNPC != -1)
            {
                NPC npc = Main.npc[prioritiseNPC];
                if (IsHitboxWithinRange(position, npc.Hitbox, range))
                {
                    if (Collision.CanHitLine(collisionPosition, collitionWidth, collitionHeight, npc.position, npc.width, npc.height))
                    {
                        return prioritiseNPC;
                    }
                }
            }

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && i != doNotInclude)
                {
                    if (npc.type == NPCID.TargetDummy && includeTargetDummy)
                    {
                        if (IsHitboxWithinRange(position, npc.Hitbox, range))
                        {
                            if (Collision.CanHitLine(collisionPosition, collitionWidth, collitionHeight, npc.position, npc.width, npc.height))
                            {
                                currentChoice = i;
                                range = Vector2.Distance(position, npc.Center);
                            }
                        }
                    }
                    else if (!includeTownNPCs && !npc.townNPC || includeTownNPCs)
                    {
                        if (!includeSmallCreatures && npc.lifeMax > 5 || includeSmallCreatures)
                        {
                            if (!includeImmortal && !npc.immortal || includeImmortal)
                            {
                                if (IsHitboxWithinRange(position, npc.Hitbox, range))
                                {
                                    if (Collision.CanHitLine(collisionPosition, collitionWidth, collitionHeight, npc.position, npc.width, npc.height))
                                    {
                                        currentChoice = i;
                                        range = Vector2.Distance(position, npc.Center);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return currentChoice;
        }
        public static int GetClosestNPCWithBuff(Vector2 position, float maxDistance, int mustHaveBuff, int doNotInclude = -1, bool includeSmallCreatures = false, bool includeTargetDummy = false, bool includeTownNPCs = false, bool includeImmortal = false)
        {
            int currentChoice = -1;
            float range = maxDistance;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];

                if (npc.active && npc.whoAmI != doNotInclude)
                {
                    if (npc.type == NPCID.TargetDummy && includeTargetDummy)
                    {
                        if (IsHitboxWithinRange(position, npc.Hitbox, range))
                        {
                            currentChoice = i;
                            range = Vector2.Distance(position, npc.Center);
                        }
                    }
                    else if (!includeTownNPCs && !npc.townNPC || includeTownNPCs)
                    {
                        if (!includeSmallCreatures && npc.lifeMax > 5 || includeSmallCreatures)
                        {
                            if (!includeImmortal && !npc.immortal || includeImmortal)
                            {
                                if (npc.HasBuff(mustHaveBuff))
                                {
                                    if (IsHitboxWithinRange(position, npc.Hitbox, range))
                                    {
                                        currentChoice = i;
                                        range = Vector2.Distance(position, npc.Center);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return currentChoice;
        }
        public static int GetClosestNPCInLOS(Entity entity, float maxDistance, int doNotInclude = -1, int prioritiseNPC = -1, bool includeSmallCreatures = false, bool includeTargetDummy = false, bool includeTownNPCs = false, bool includeImmortal = false)
        {
            int currentChoice = -1;
            float range = maxDistance;

            if (prioritiseNPC != -1)
            {
                NPC npc = Main.npc[prioritiseNPC];
                if (IsHitboxWithinRange(entity.Center, npc.Hitbox, range))
                {
                    return prioritiseNPC;
                }
            }

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];

                if (npc.active && npc.whoAmI != doNotInclude)
                {
                    if (npc.type == NPCID.TargetDummy && includeTargetDummy)
                    {
                        if (Collision.CanHit(entity, npc))
                        {
                            if (IsHitboxWithinRange(entity.Center, npc.Hitbox, range))
                            {
                                currentChoice = i;
                                range = Vector2.Distance(entity.Center, npc.Center);
                            }
                        }
                    }
                    else if (!includeTownNPCs && !npc.townNPC || includeTownNPCs)
                    {
                        if (!includeSmallCreatures && npc.lifeMax > 5 || includeSmallCreatures)
                        {
                            if (!includeImmortal && !npc.immortal || includeImmortal)
                            {
                                if (Collision.CanHit(entity, npc))
                                {
                                    if (IsHitboxWithinRange(entity.Center, npc.Hitbox, range))
                                    {
                                        currentChoice = i;
                                        range = Vector2.Distance(entity.Center, npc.Center);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return currentChoice;
        }

        public static List<T> SortListByDistance<T> (this List<T> listToSort, Vector2 Center) where T : Entity
        {
            var clonedList = new List<T>(listToSort.Count);

            for (int i = 0; i < listToSort.Count; i++)
            {
                var item = listToSort[i];
                var currentIndex = i;

                while (currentIndex > 0 && clonedList[currentIndex - 1].Distance(Center) > item.Distance(Center))
                {
                    currentIndex--;
                }

                clonedList.Insert(currentIndex, item);
            }

            return clonedList;
        }

        public static List<int> SortPlayersByDistance(this List<int> listToSort, Vector2 Center)
        {
            var clonedList = new List<int>(listToSort.Count);

            for (int i = 0; i < listToSort.Count; i++)
            {
                var item = listToSort[i];
                var currentIndex = i;

                while (currentIndex > 0 && Main.player[clonedList[currentIndex - 1]].Distance(Center) > Main.player[item].Distance(Center))
                {
                    currentIndex--;
                }

                clonedList.Insert(currentIndex, item);
            }

            return clonedList;
        }

        public static List<int> SortNPCsByDistance(this List<int> listToSort, Vector2 Center)
        {
            var clonedList = new List<int>(listToSort.Count);

            for (int i = 0; i < listToSort.Count; i++)
            {
                var item = listToSort[i];
                var currentIndex = i;

                while (currentIndex > 0 && Main.npc[clonedList[currentIndex - 1]].Distance(Center) > Main.npc[item].Distance(Center))
                {
                    currentIndex--;
                }

                clonedList.Insert(currentIndex, item);
            }

            return clonedList;
        }
    }
}
