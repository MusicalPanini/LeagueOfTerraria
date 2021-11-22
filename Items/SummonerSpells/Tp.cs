﻿using Microsoft.Xna.Framework;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.UI;
using Terraria;
using Terraria.Chat;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items.SummonerSpells
{
    public class TeleportRune : SummonerSpell
    {
        public override void Load()
        {
			IL.Terraria.Player.HasUnityPotion += HookHasUnityPotion;
			IL.Terraria.Player.TakeUnityPotion += HookTakeUnityPotion;
			IL.Terraria.Player.InInteractionRange += HookInteractionRange;
			IL.Terraria.GameContent.TeleportPylonsSystem.IsPlayerNearAPylon += HookIsNearPylon;
			IL.Terraria.GameContent.TeleportPylonsSystem.HowManyNPCsDoesPylonNeed += HookEnoughtNPCS;
		}

		private static void HookTakeUnityPotion(ILContext il)
        {
			ILCursor c = new ILCursor(il);

            //Define a lable
            var label = il.DefineLabel();

			// Push Player (this) onto the stack
            c.Emit(OpCodes.Ldarg_0);

			// Check if Player has teleport and push result onto stack
			c.EmitDelegate<Func<Player, bool>>(player => TeleportRune.CheckForTeleportSum(player));

			// If player does not have teleport, branch to label
			c.Emit(OpCodes.Brfalse, label);

			// Return
			c.Emit(OpCodes.Ret);

			// The defined label branching point
			c.MarkLabel(label);

			// The rest of the code
		}

		private static void HookHasUnityPotion(ILContext il)
        {
			ILCursor c = new ILCursor(il);

            if (!c.TryGotoNext(i => i.MatchLdcI4(58)))
            {
                return; // Patch unable to be applied
            }
			if (!c.TryGotoNext(i => i.MatchLdcI4(0)))
			{
				return; // Patch unable to be applied
			}

			c.Index++;

			// Push the Player instance onto the stack
			c.Emit(OpCodes.Ldarg_0);
			// Call a delegate using the int and Player from the stack.
			c.EmitDelegate<Func<bool, Player, bool>>((returnValue, player) => {
				// Regular c# code
				if (Main.netMode == NetmodeID.Server)
					Console.WriteLine("HookHasUnityPotion");
				return TeleportRune.CheckForTeleportSum(player);
			});

		}

		private static void HookIsNearPylon(ILContext il)
		{
			ILCursor c = new ILCursor(il);

			if (!c.TryGotoNext(i => i.MatchRet()))
			{
				return; // Patch unable to be applied
			}

			// Push the Player instance onto the stack
			c.Emit(OpCodes.Ldarg_0);
			// Call a delegate using the int and Player from the stack.
			c.EmitDelegate<Func<bool, Player, bool>>((returnValue, player) => {
				if (Main.netMode == NetmodeID.Server)
					Console.WriteLine("HookIsNearPylon");
				if (TeleportRune.CheckForTeleportSum(player))
				{
					return true;
				}
				return player.IsTileTypeInInteractionRange(597);
			});

		}

		private static void HookInteractionRange(ILContext il)
		{
            ILCursor c = new ILCursor(il);

            if (!c.TryGotoNext(i => i.MatchRet()))
            {
                return; // Patch unable to be applied
            }

            // returnvalue already pushed
            // Push the Player instance onto the stack
            c.Emit(OpCodes.Ldloc_2);
			// Call a delegate using the int and Player from the stack.
			c.Emit(OpCodes.Ldarg_0);
            c.EmitDelegate<Func<bool, Tile, Player, bool>>((returnValue, tile, player) =>
            {
				if (Main.netMode == NetmodeID.Server)
					Console.WriteLine("HookInteractionRange");
				if (tile.type == 597 && TeleportRune.CheckForTeleportSum(player))
                {
                    return true;
                }
                return false;
                //return num >= interactX - Player.tileRangeX && num <= interactX + Player.tileRangeX + 1 && num2 >= interactY - Player.tileRangeY && num2 <= interactY + Player.tileRangeY + 1;
            });

        }

		private static void HookEnoughtNPCS(ILContext il)
		{
			ILCursor c = new ILCursor(il);

            if (!c.TryGotoNext(i => i.MatchRet()))
            {
                return; // Patch unable to be applied
            }

            // returnvalue already pushed
            // Call a delegate using the int and Player from the stack.
            c.Emit(OpCodes.Ldarg_2);
            c.EmitDelegate<Func<int, Player, int>>((returnValue, player) =>
            {
				//ChatHelper.SendChatMessageToClient(Terraria.Localization.NetworkText.FromLiteral(), new Color(255, 240, 20), player.whoAmI);
				if (Main.netMode == NetmodeID.Server)
					Console.WriteLine(player.name + " HookEnoughtNPCS");
				if (TeleportRune.CheckForTeleportSum(player))
                {
                    return 0;
                }
                return returnValue;
            });

        }

		public static bool CheckForTeleportSum(Player player)
		{
			bool hasTP = false;
			PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
			
			for (int i = 0; i < modPlayer.sumSpells.Length; i++)
			{
				hasTP = modPlayer.sumSpells[i].Name == "TeleportRune";

				if (hasTP)
				{
					if (modPlayer.sumCooldowns[i] <= 0)
						break;
					else
						return false;
				}
			}
			if (Main.netMode == NetmodeID.Server)
				Console.WriteLine("Player " + player.name + " has TP: " + hasTP);
			return hasTP;
		}

		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Teleport Rune");
            Tooltip.SetDefault("");
            base.SetStaticDefaults();
			Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}
        public override string GetIconTexturePath()
        {
            return "TerraLeague/Items/SummonerSpells/Tp";
        }

        public override string GetSpellName()
        {
            return "Teleport";
        }

        public override int GetRawCooldown()
        {
            return 0;
        }

        public override string GetTooltip()
        {
            return "Allows free teleportation to Pylons and Allies";
        }

        public override void DoEffect(Player player, int spellSlot)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

			if (Main.mapFullscreen)
			{
                Terraria.Audio.SoundEngine.PlaySound(11, -1, -1, 1, 1f, 0f);
				Main.mapFullscreen = false;
			}
			else
			{
				player.TryOpeningFullscreenMap();
			}

			//player.TeleportationPotion();
			//TeleportUI.visible = !TeleportUI.visible;
			//SetCooldowns(player, spellSlot);
		}

        public static void Efx(Vector2 teleportPoint)
        {
            TerraLeague.PlaySoundWithPitch(teleportPoint, 2, 6, 0);
            for (int i = 0; i < 20; i++)
            {
                Dust dust = Dust.NewDustDirect(teleportPoint - (Vector2.One * 16), 32, 42, DustID.Shadowflame, 0, 0, 0, default, 4);
                dust.noGravity = true;
                dust.noLight = true;
                dust.velocity *= 2;
            }
        }

		public static void AttemptTP(Player player, TeleportType type)
        {
			if (type == TeleportType.Random)
            {
				player.TeleportationPotion();
				Efx(player.position);
				SetTPCooldown();
				PacketHandler.SendTeleport(-1, player.whoAmI, player.position);
            }
			else if (Main.netMode == NetmodeID.MultiplayerClient)
            {
				PacketHandler.SendTeleportRequest(-1, player.whoAmI, (int)type);
            }
            else if (Main.netMode == NetmodeID.SinglePlayer)
			{
                switch (type)
                {
                    case TeleportType.LeftBeach:
						DoTP(player, LeftBeach());
                        break;
                    case TeleportType.RightBeach:
						DoTP(player, RightBeach());
						break;
                    case TeleportType.Dungeon:
						DoTP(player, Dungeon());
						break;
      //              case TeleportType.Hell:
						//DoTP(player, Hell(player));
						//break;
      //              case TeleportType.Random:
						//DoTP(player, RandomTP());
						//break;
                    default:
                        break;
                }
            }
        }

		public static void AttemptTP(Player player, int playerTarget)
		{
			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				PacketHandler.SendTeleportRequestPlayer(-1, player.whoAmI, playerTarget);
			}
			else if (Main.netMode == NetmodeID.SinglePlayer)
			{
				DoTP(player, Main.player[playerTarget].position);
			}
		}

		public static Vector2 RandomTP()
        {
            return Vector2.Zero;
        }
        public static Vector2 LeftBeach()
        {
            int x = 300;
            for (int y = 0; y < Main.maxTilesY; y++)
            {
				var s= Main.tile[x, y];
                if (Collision.SolidTiles(x, x, y, y))
                {
                    return new Vector2(x * 16, (y - 3) * 16);
                }
            }

            return Main.LocalPlayer.position;
        }
        public static Vector2 RightBeach()
        {
            int x = Main.maxTilesX - 300;
            for (int y = 0; y < Main.maxTilesY; y++)
            {
                if (Collision.SolidTiles(x, x, y, y))
                {
                    return new Vector2(x * 16, (y - 3) * 16);
                }
            }

            return Main.LocalPlayer.position;
        }
        public static Vector2 Hell(Player player)
        {
			
			return Vector2.Zero;
			//bool flag = false;
			//int num = Main.maxTilesX / 2;
			//int num2 = 100;
			//int num3 = num2 / 2;
			//int teleportStartY = Main.maxTilesY - 200 + 20;
			//int teleportRangeY = 80;
			//RandomTeleportationAttemptSettings settings = new RandomTeleportationAttemptSettings
			//{
			//	mostlySolidFloor = true,
			//	avoidAnyLiquid = true,
			//	avoidLava = true,
			//	avoidHurtTiles = true,
			//	avoidWalls = true,
			//	attemptsBeforeGivingUp = 1000,
			//	maximumFallDistanceFromOrignalPoint = 30
			//};
			//Vector2 vector = CheckForGoodTeleportationSpot(player, ref flag, num - num3, num2, teleportStartY, teleportRangeY, settings);
			//if (!flag)
			//{
			//	vector = CheckForGoodTeleportationSpot(player, ref flag, num - num2, num3, teleportStartY, teleportRangeY, settings);
			//}
			//if (!flag)
			//{
			//	vector = CheckForGoodTeleportationSpot(player, ref flag, num + num3, num3, teleportStartY, teleportRangeY, settings);
			//}
			//if (flag)
			//{
			//	Vector2 vector2 = vector;
			//	return vector2;
			//}
			//else
			//{
			//	Vector2 position = player.position;
			//	return position;
			//}
        }
        public static Vector2 Dungeon()
        {
            return new Vector2(Main.dungeonX * 16, Main.dungeonY * 16 - 48);
        }

        public static void DoTP(Player player, Vector2 teleportPoint)
        {
			if (Main.LocalPlayer.whoAmI == player.whoAmI)
			{
				if (Vector2.Distance(teleportPoint, player.position) > 16 * 10)
					SetTPCooldown();
			}

			player.Teleport(teleportPoint, 10, 0);
            Efx(teleportPoint);
        }

		static void SetTPCooldown()
		{
			PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();

			for (int i = 0; i < modPlayer.sumSpells.Length; i++)
			{
				if (modPlayer.sumSpells[i].Name == "TeleportRune")
				{
					modPlayer.sumCooldowns[i] = (int)(modPlayer.sumSpells[i].GetCooldown() * 60);
					return;
				}
			}
		}

		static private Vector2 CheckForGoodTeleportationSpot(Player player, ref bool canSpawn, int teleportStartX, int teleportRangeX, int teleportStartY, int teleportRangeY, RandomTeleportationAttemptSettings settings)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int width = player.width;
			Vector2 vector = new Vector2((float)num2, (float)num3) * 16f + new Vector2((float)(-width / 2 + 8), (float)(-player.height));
			while (!canSpawn && num < settings.attemptsBeforeGivingUp)
			{
				num++;
				num2 = teleportStartX + Main.rand.Next(teleportRangeX);
				num3 = teleportStartY + Main.rand.Next(teleportRangeY);
				int num4 = 5;
				num2 = (int)MathHelper.Clamp((float)num2, (float)num4, (float)(Main.maxTilesX - num4));
				num3 = (int)MathHelper.Clamp((float)num3, (float)num4, (float)(Main.maxTilesY - num4));
				vector = new Vector2((float)num2, (float)num3) * 16f + new Vector2((float)(-width / 2 + 8), (float)(-player.height));
				int num7;
				Tile tileSafely;
				if (!Collision.SolidCollision(vector, width, player.height))
				{
					if (Main.tile[num2, num3] == null)
					{
						Tile[,] tile = Main.tile;
						int num5 = num2;
						int num6 = num3;
						Tile tile2 = new Tile();
						tile[num5, num6] = tile2;
					}
					if (settings.avoidWalls && Main.tile[num2, num3].wall > 0)
					{
						continue;
					}
					if (Main.tile[num2, num3].wall == 87 && (double)num3 > Main.worldSurface && !NPC.downedPlantBoss)
					{
						continue;
					}
					if (Main.wallDungeon[Main.tile[num2, num3].wall] && (double)num3 > Main.worldSurface && !NPC.downedBoss3)
					{
						continue;
					}
					num7 = 0;
					while (num7 < settings.maximumFallDistanceFromOrignalPoint)
					{
						if (Main.tile[num2, num3 + num7] == null)
						{
							Tile[,] tile3 = Main.tile;
							int num8 = num2;
							int num9 = num3 + num7;
							Tile tile4 = new Tile();
							tile3[num8, num9] = tile4;
						}
						Tile tile5 = Main.tile[num2, num3 + num7];
						vector = new Vector2((float)num2, (float)(num3 + num7)) * 16f + new Vector2((float)(-width / 2 + 8), (float)(-player.height));
						Collision.SlopeCollision(vector, player.velocity, width, player.height, player.gravDir, false);
						if (!Collision.SolidCollision(vector, width, player.height))
						{
							num7++;
						}
						else
						{
							if (tile5.IsActive && !tile5.IsActuated && Main.tileSolid[tile5.type])
							{
								break;
							}
							num7++;
						}
					}
					vector.Y -= 16f;
					int i = (int)vector.X / 16;
					int j = (int)vector.Y / 16;
					int num10 = (int)(vector.X + (float)width * 0.5f) / 16;
					int j2 = (int)(vector.Y + (float)player.height) / 16;
					tileSafely = Framing.GetTileSafely(i, j);
					Tile tileSafely2 = Framing.GetTileSafely(num10, j2);
					if (settings.avoidAnyLiquid && tileSafely2.LiquidAmount > 0)
					{
						continue;
					}
					if (settings.mostlySolidFloor)
					{
						Tile tileSafely3 = Framing.GetTileSafely(num10 - 1, j2);
						Tile tileSafely4 = Framing.GetTileSafely(num10 + 1, j2);
						if (tileSafely3.IsActive && !tileSafely3.IsActuated && Main.tileSolid[tileSafely3.type] && !Main.tileSolidTop[tileSafely3.type] && tileSafely4.IsActive && !tileSafely4.IsActuated && Main.tileSolid[tileSafely4.type] && !Main.tileSolidTop[tileSafely4.type])
						{
							//goto IL_034e;
						}
                        else
                        {
							continue;
                        }
                    }
					else
                    {
						//goto IL_034e;
                    }
				}
                else
                {
					continue;
                }
			//IL_034e:
				if ((!settings.avoidWalls || tileSafely.wall <= 0) && (!settings.avoidAnyLiquid || !Collision.WetCollision(vector, width, player.height)) && (!settings.avoidLava || !Collision.LavaCollision(vector, width, player.height)) && (!settings.avoidHurtTiles || !(Collision.HurtTiles(vector, player.velocity, width, player.height, false).Y > 0f)) && !Collision.SolidCollision(vector, width, player.height) && num7 < settings.maximumFallDistanceFromOrignalPoint - 1)
				{
					Vector2 vector2 = Vector2.UnitX * 16f;
					if (!(Collision.TileCollision(vector - vector2, vector2, player.width, player.height, false, false, (int)player.gravDir) != vector2))
					{
						vector2 = -Vector2.UnitX * 16f;
						if (!(Collision.TileCollision(vector - vector2, vector2, player.width, player.height, false, false, (int)player.gravDir) != vector2))
						{
							vector2 = Vector2.UnitY * 16f;
							if (!(Collision.TileCollision(vector - vector2, vector2, player.width, player.height, false, false, (int)player.gravDir) != vector2))
							{
								vector2 = -Vector2.UnitY * 16f;
								if (!(Collision.TileCollision(vector - vector2, vector2, player.width, player.height, false, false, (int)player.gravDir) != vector2))
								{
									canSpawn = true;
									num3 += num7;
									break;
								}
							}
						}
					}
				}
			}
			return vector;
		}

		private class RandomTeleportationAttemptSettings
		{
			public bool mostlySolidFloor;

			public bool avoidLava;

			public bool avoidAnyLiquid;

			public bool avoidHurtTiles;

			public bool avoidWalls;

			public int attemptsBeforeGivingUp;

			public int maximumFallDistanceFromOrignalPoint;
		}
	}
}


