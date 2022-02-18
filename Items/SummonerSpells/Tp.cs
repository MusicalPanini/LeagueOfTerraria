using Microsoft.Xna.Framework;
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
using Terraria.GameContent;
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

			IL.Terraria.GameContent.TeleportPylonsSystem.HandleTeleportRequest += HookHandleTeleportRequest;
			IL.Terraria.Map.TeleportPylonsMapLayer.Draw += HookDrawPylon;
		}

		private static void HookHandleTeleportRequest(ILContext il)
		{
			ILCursor c = new ILCursor(il);

			//Define a lable
			var label = il.DefineLabel();

			// Push Player (this) onto the stack
			c.Emit(OpCodes.Ldarg_2);

			// Check if Player has teleport and push result onto stack
			c.EmitDelegate<Func<int, bool>>(playerIndex => TeleportRune.CheckForTeleportSum(Main.player[playerIndex]));

			// If player does not have teleport, branch to label
			c.Emit(OpCodes.Brfalse, label);

			c.Emit(OpCodes.Ldarg_1);

			c.Emit(OpCodes.Ldarg_2);

			c.EmitDelegate<Action<TeleportPylonInfo, int>>((info, playerIndex) =>
			{
				Player player = Main.player[playerIndex];
				Vector2 newPos = info.PositionInTiles.ToWorldCoordinates(8f, 8f) - new Vector2(0f, (float)player.HeightOffsetBoost);
				int num2 = 9;
				int typeOfPylon = (int)info.TypeOfPylon;
				int number = 0;
				player.Teleport(newPos, num2, typeOfPylon);
				player.velocity = Vector2.Zero;
				if (Main.netMode == 2)
				{
					RemoteClient.CheckSection(player.whoAmI, player.position, 1);
					NetMessage.SendData(65, -1, -1, null, 0, (float)player.whoAmI, newPos.X, newPos.Y, num2, number, typeOfPylon);
					return;
				}
			});

			// Return
			c.Emit(OpCodes.Ret);

			// The defined label branching point
			c.MarkLabel(label);
		}

		private static void HookDrawPylon(ILContext il)
        {
			ILCursor c = new ILCursor(il);

			if (!c.TryGotoNext(i => i.MatchLdcI4(0)))
			{
				return; // Patch unable to be applied
			}

			//c.Emit(OpCodes.Ldloc_S, 4);

			c.EmitDelegate<Func</*Color,*/ Color>>((/*originalValue*/) => {
				if (TeleportRune.CheckForTeleportSum(Main.LocalPlayer))
				{
					return Color.White;
				}
				return Color.Gray * 0.5f;
			});

			c.Emit(OpCodes.Stloc, 4);
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
				Console.WriteLine("HookHasUnityPotion");
				return TeleportRune.CheckForTeleportSum(player);
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
						//DoTP(player, LeftBeach());
                        break;
                    case TeleportType.RightBeach:
						//DoTP(player, RightBeach());
						break;
                    case TeleportType.Dungeon:
						//DoTP(player, Dungeon());
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
	}
}


