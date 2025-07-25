﻿/*
TShock, a server mod for Terraria
Copyright (C) 2011-2019 Pryaxis & TShock Contributors

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using TShockAPI.DB;
using TerrariaApi.Server;
using TShockAPI.Hooks;
using Terraria.GameContent.Events;
using Microsoft.Xna.Framework;
using TShockAPI.Localization;
using System.Text.RegularExpressions;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;

namespace TShockAPI
{
	public delegate void CommandDelegate(CommandArgs args);

	public class CommandArgs : EventArgs
	{
		public string Message { get; private set; }
		public TSPlayer Player { get; private set; }
		public bool Silent { get; private set; }

		/// <summary>
		/// Parameters passed to the argument. Does not include the command name.
		/// IE '/kick "jerk face"' will only have 1 argument
		/// </summary>
		public List<string> Parameters { get; private set; }

		public Player TPlayer
		{
			get { return Player.TPlayer; }
		}

		public CommandArgs(string message, TSPlayer ply, List<string> args)
		{
			Message = message;
			Player = ply;
			Parameters = args;
			Silent = false;
		}

		public CommandArgs(string message, bool silent, TSPlayer ply, List<string> args)
		{
			Message = message;
			Player = ply;
			Parameters = args;
			Silent = silent;
		}
	}

	public class Command
	{
		/// <summary>
		/// Gets or sets whether to allow non-players to use this command.
		/// </summary>
		public bool AllowServer { get; set; }
		/// <summary>
		/// Gets or sets whether to do logging of this command.
		/// </summary>
		public bool DoLog { get; set; }
		/// <summary>
		/// Gets or sets the help text of this command.
		/// </summary>
		public string HelpText { get; set; }
		/// <summary>
		/// Gets or sets an extended description of this command.
		/// </summary>
		public string[] HelpDesc { get; set; }
		/// <summary>
		/// Gets the name of the command.
		/// </summary>
		public string Name { get { return Names[0]; } }
		/// <summary>
		/// Gets the names of the command.
		/// </summary>
		public List<string> Names { get; protected set; }
		/// <summary>
		/// Gets the permissions of the command.
		/// </summary>
		public List<string> Permissions { get; protected set; }

		private CommandDelegate commandDelegate;
		public CommandDelegate CommandDelegate
		{
			get { return commandDelegate; }
			set
			{
				if (value == null)
					throw new ArgumentNullException();

				commandDelegate = value;
			}
		}

		public Command(List<string> permissions, CommandDelegate cmd, params string[] names)
			: this(cmd, names)
		{
			Permissions = permissions;
		}

		public Command(string permissions, CommandDelegate cmd, params string[] names)
			: this(cmd, names)
		{
			Permissions = new List<string> { permissions };
		}

		public Command(CommandDelegate cmd, params string[] names)
		{
			if (cmd == null)
				throw new ArgumentNullException("cmd");
			if (names == null || names.Length < 1)
				throw new ArgumentException("names");

			AllowServer = true;
			CommandDelegate = cmd;
			DoLog = true;
			HelpText = GetString("No help available.");
			HelpDesc = null;
			Names = new List<string>(names);
			Permissions = new List<string>();
		}

		public bool Run(CommandArgs args)
		{
			if (!CanRun(args.Player))
				return false;

			try
			{
				CommandDelegate(args);
			}
			catch (Exception e)
			{
				args.Player.SendErrorMessage(GetString("Command failed, check logs for more details."));
				TShock.Log.Error(e.ToString());
			}

			return true;
		}

		public bool Run(string msg, bool silent, TSPlayer ply, List<string> parms)
		{
			return Run(new CommandArgs(msg, silent, ply, parms));
		}

		public bool Run(string msg, TSPlayer ply, List<string> parms)
		{
			return Run(msg, false, ply, parms);
		}

		public bool HasAlias(string name)
		{
			return Names.Contains(name);
		}

		public bool CanRun(TSPlayer ply)
		{
			if (Permissions == null || Permissions.Count < 1)
				return true;
			foreach (var Permission in Permissions)
			{
				if (ply.HasPermission(Permission))
					return true;
			}
			return false;
		}
	}

	public static class Commands
	{
		public static List<Command> ChatCommands = new List<Command>();
		public static ReadOnlyCollection<Command> TShockCommands = new ReadOnlyCollection<Command>(new List<Command>());

		/// <summary>
		/// The command specifier, defaults to "/"
		/// </summary>
		public static string Specifier
		{
			get { return string.IsNullOrWhiteSpace(TShock.Config.Settings.CommandSpecifier) ? "/" : TShock.Config.Settings.CommandSpecifier; }
		}

		/// <summary>
		/// The silent command specifier, defaults to "."
		/// </summary>
		public static string SilentSpecifier
		{
			get { return string.IsNullOrWhiteSpace(TShock.Config.Settings.CommandSilentSpecifier) ? "." : TShock.Config.Settings.CommandSilentSpecifier; }
		}

		private delegate void AddChatCommand(string permission, CommandDelegate command, params string[] names);

		public static void InitCommands()
		{
			List<Command> tshockCommands = new List<Command>(100);
			Action<Command> add = (cmd) =>
			{
				tshockCommands.Add(cmd);
				ChatCommands.Add(cmd);
			};

			add(new Command(SetupToken, "setup")
			{
				AllowServer = false,
				HelpText = GetString("Used to authenticate as superadmin when first setting up TShock.")
			});
			add(new Command(Permissions.user, ManageUsers, "user")
			{
				DoLog = false,
				HelpText = GetString("Manages user accounts.")
			});

			#region Account Commands
			add(new Command(Permissions.canlogin, AttemptLogin, "login")
			{
				AllowServer = false,
				DoLog = false,
				HelpText = GetString("Logs you into an account.")
			});
			add(new Command(Permissions.canlogout, Logout, "logout")
			{
				AllowServer = false,
				DoLog = false,
				HelpText = GetString("Logs you out of your current account.")
			});
			add(new Command(Permissions.canchangepassword, PasswordUser, "password")
			{
				AllowServer = false,
				DoLog = false,
				HelpText = GetString("Changes your account's password.")
			});
			add(new Command(Permissions.canregister, RegisterUser, "register")
			{
				AllowServer = false,
				DoLog = false,
				HelpText = GetString("Registers you an account.")
			});
			add(new Command(Permissions.checkaccountinfo, ViewAccountInfo, "accountinfo", "ai")
			{
				HelpText = GetString("Shows information about a user.")
			});
			#endregion
			#region Admin Commands
			add(new Command(Permissions.ban, Ban, "ban")
			{
				HelpText = GetString("Manages player bans.")
			});
			add(new Command(Permissions.broadcast, Broadcast, "broadcast", "bc", "say")
			{
				HelpText = GetString("Broadcasts a message to everyone on the server.")
			});
			add(new Command(Permissions.logs, DisplayLogs, "displaylogs")
			{
				HelpText = GetString("Toggles whether you receive server logs.")
			});
			add(new Command(Permissions.managegroup, Group, "group")
			{
				HelpText = GetString("Manages groups.")
			});
			add(new Command(Permissions.manageitem, ItemBan, "itemban")
			{
				HelpText = GetString("Manages item bans.")
			});
			add(new Command(Permissions.manageprojectile, ProjectileBan, "projban")
			{
				HelpText = GetString("Manages projectile bans.")
			});
			add(new Command(Permissions.managetile, TileBan, "tileban")
			{
				HelpText = GetString("Manages tile bans.")
			});
			add(new Command(Permissions.manageregion, Region, "region")
			{
				HelpText = GetString("Manages regions.")
			});
			add(new Command(Permissions.kick, Kick, "kick")
			{
				HelpText = GetString("Removes a player from the server.")
			});
			add(new Command(Permissions.mute, Mute, "mute", "unmute")
			{
				HelpText = GetString("Prevents a player from talking.")
			});
			add(new Command(Permissions.savessc, OverrideSSC, "overridessc", "ossc")
			{
				HelpText = GetString("Overrides serverside characters for a player, temporarily.")
			});
			add(new Command(Permissions.savessc, SaveSSC, "savessc")
			{
				HelpText = GetString("Saves all serverside characters.")
			});
			add(new Command(Permissions.uploaddata, UploadJoinData, "uploadssc")
			{
				HelpText = GetString("Upload the account information when you joined the server as your Server Side Character data.")
			});
			add(new Command(Permissions.settempgroup, TempGroup, "tempgroup")
			{
				HelpText = GetString("Temporarily sets another player's group.")
			});
			add(new Command(Permissions.su, SubstituteUser, "su")
			{
				HelpText = GetString("Temporarily elevates you to Super Admin.")
			});
			add(new Command(Permissions.su, SubstituteUserDo, "sudo")
			{
				HelpText = GetString("Executes a command as the super admin.")
			});
			add(new Command(Permissions.userinfo, GrabUserUserInfo, "userinfo", "ui")
			{
				HelpText = GetString("Shows information about a player.")
			});
			#endregion
			#region Annoy Commands
			add(new Command(Permissions.annoy, Annoy, "annoy")
			{
				HelpText = GetString("Annoys a player for an amount of time.")
			});
			add(new Command(Permissions.annoy, Rocket, "rocket")
			{
				HelpText = GetString("Rockets a player upwards. Requires SSC.")
			});
			add(new Command(Permissions.annoy, FireWork, "firework")
			{
				HelpText = GetString("Spawns fireworks at a player.")
			});
			#endregion
			#region Configuration Commands
			add(new Command(Permissions.maintenance, CheckUpdates, "checkupdates")
			{
				HelpText = GetString("Checks for TShock updates.")
			});
			add(new Command(Permissions.maintenance, Off, "off", "exit", "stop")
			{
				HelpText = GetString("Shuts down the server while saving.")
			});
			add(new Command(Permissions.maintenance, OffNoSave, "off-nosave", "exit-nosave", "stop-nosave")
			{
				HelpText = GetString("Shuts down the server without saving.")
			});
			add(new Command(Permissions.cfgreload, Reload, "reload")
			{
				HelpText = GetString("Reloads the server configuration file.")
			});
			add(new Command(Permissions.cfgpassword, ServerPassword, "serverpassword")
			{
				HelpText = GetString("Changes the server password.")
			});
			add(new Command(Permissions.maintenance, GetVersion, "version")
			{
				HelpText = GetString("Shows the TShock version.")
			});
			add(new Command(Permissions.whitelist, Whitelist, "whitelist")
			{
				HelpText = GetString("Manages the server whitelist.")
			});
			#endregion
			#region Item Commands
			add(new Command(Permissions.give, Give, "give", "g")
			{
				HelpText = GetString("Gives another player an item.")
			});
			add(new Command(Permissions.item, Item, "item", "i")
			{
				AllowServer = false,
				HelpText = GetString("Gives yourself an item.")
			});
			#endregion
			#region NPC Commands
			add(new Command(Permissions.butcher, Butcher, "butcher")
			{
				HelpText = GetString("Kills hostile NPCs or NPCs of a certain type.")
			});
			add(new Command(Permissions.renamenpc, RenameNPC, "renamenpc")
			{
				HelpText = GetString("Renames an NPC.")
			});
			add(new Command(Permissions.maxspawns, MaxSpawns, "maxspawns")
			{
				HelpText = GetString("Sets the maximum number of NPCs.")
			});
			add(new Command(Permissions.spawnboss, SpawnBoss, "spawnboss", "sb")
			{
				AllowServer = false,
				HelpText = GetString("Spawns a number of bosses around you.")
			});
			add(new Command(Permissions.spawnmob, SpawnMob, "spawnmob", "sm")
			{
				AllowServer = false,
				HelpText = GetString("Spawns a number of mobs around you.")
			});
			add(new Command(Permissions.spawnrate, SpawnRate, "spawnrate")
			{
				HelpText = GetString("Sets the spawn rate of NPCs.")
			});
			add(new Command(Permissions.clearangler, ClearAnglerQuests, "clearangler")
			{
				HelpText = GetString("Resets the list of users who have completed an angler quest that day.")
			});
			#endregion
			#region TP Commands
			add(new Command(Permissions.home, Home, "home")
			{
				AllowServer = false,
				HelpText = GetString("Sends you to your spawn point.")
			});
			add(new Command(Permissions.spawn, Spawn, "spawn")
			{
				AllowServer = false,
				HelpText = GetString("Sends you to the world's spawn point.")
			});
			add(new Command(Permissions.tp, TP, "tp")
			{
				AllowServer = false,
				HelpText = GetString("Teleports a player to another player.")
			});
			add(new Command(Permissions.tpothers, TPHere, "tphere")
			{
				AllowServer = false,
				HelpText = GetString("Teleports a player to yourself.")
			});
			add(new Command(Permissions.tpnpc, TPNpc, "tpnpc")
			{
				AllowServer = false,
				HelpText = GetString("Teleports you to an npc.")
			});
			add(new Command(Permissions.tppos, TPPos, "tppos")
			{
				AllowServer = false,
				HelpText = GetString("Teleports you to tile coordinates.")
			});
			add(new Command(Permissions.getpos, GetPos, "pos")
			{
				AllowServer = false,
				HelpText = GetString("Returns the user's or specified user's current position.")
			});
			add(new Command(Permissions.tpallow, TPAllow, "tpallow")
			{
				AllowServer = false,
				HelpText = GetString("Toggles whether other people can teleport you.")
			});
			#endregion
			#region World Commands
			add(new Command(Permissions.toggleexpert, ChangeWorldMode, "worldmode", "gamemode")
			{
				HelpText = GetString("Changes the world mode.")
			});
			add(new Command(Permissions.antibuild, ToggleAntiBuild, "antibuild")
			{
				HelpText = GetString("Toggles build protection.")
			});
			add(new Command(Permissions.grow, Grow, "grow")
			{
				AllowServer = false,
				HelpText = GetString("Grows plants at your location.")
			});
			add(new Command(Permissions.halloween, ForceHalloween, "forcehalloween")
			{
				HelpText = GetString("Toggles halloween mode (goodie bags, pumpkins, etc).")
			});
			add(new Command(Permissions.xmas, ForceXmas, "forcexmas")
			{
				HelpText = GetString("Toggles christmas mode (present spawning, santa, etc).")
			});
			add(new Command(Permissions.manageevents, ManageWorldEvent, "worldevent")
			{
				HelpText = GetString("Enables starting and stopping various world events.")
			});
			add(new Command(Permissions.hardmode, Hardmode, "hardmode")
			{
				HelpText = GetString("Toggles the world's hardmode status.")
			});
			add(new Command(Permissions.editspawn, ProtectSpawn, "protectspawn")
			{
				HelpText = GetString("Toggles spawn protection.")
			});
			add(new Command(Permissions.worldsave, Save, "save")
			{
				HelpText = GetString("Saves the world file.")
			});
			add(new Command(Permissions.worldspawn, SetSpawn, "setspawn")
			{
				AllowServer = false,
				HelpText = GetString("Sets the world's spawn point to your location.")
			});
			add(new Command(Permissions.dungeonposition, SetDungeon, "setdungeon")
			{
				AllowServer = false,
				HelpText = GetString("Sets the dungeon's position to your location.")
			});
			add(new Command(Permissions.worldsettle, Settle, "settle")
			{
				HelpText = GetString("Forces all liquids to update immediately.")
			});
			add(new Command(Permissions.time, Time, "time")
			{
				HelpText = GetString("Sets the world time.")
			});
			add(new Command(Permissions.wind, Wind, "wind")
			{
				HelpText = GetString("Changes the wind speed.")
			});
			add(new Command(Permissions.worldinfo, WorldInfo, "worldinfo")
			{
				HelpText = GetString("Shows information about the current world.")
			});
			#endregion
			#region Other Commands
			add(new Command(Permissions.buff, Buff, "buff")
			{
				AllowServer = false,
				HelpText = GetString("Gives yourself a buff or debuff for an amount of time. Putting -1 for time will set it to 415 days.")
			});
			add(new Command(Permissions.clear, Clear, "clear")
			{
				HelpText = GetString("Clears item drops or projectiles.")
			});
			add(new Command(Permissions.buffplayer, GBuff, "gbuff", "buffplayer")
			{
				HelpText = GetString("Gives another player a buff or debuff for an amount of time. Putting -1 for time will set it to 415 days.")
			});
			add(new Command(Permissions.godmode, ToggleGodMode, "godmode", "god")
			{
				HelpText = GetString("Toggles godmode on a player.")
			});
			add(new Command(Permissions.heal, Heal, "heal")
			{
				HelpText = GetString("Heals a player in HP and MP.")
			});
			add(new Command(Permissions.kill, Kill, "kill", "slay")
			{
				HelpText = GetString("Kills another player.")
			});
			add(new Command(Permissions.cantalkinthird, ThirdPerson, "me")
			{
				HelpText = GetString("Sends an action message to everyone.")
			});
			add(new Command(Permissions.canpartychat, PartyChat, "party", "p")
			{
				AllowServer = false,
				HelpText = GetString("Sends a message to everyone on your team.")
			});
			add(new Command(Permissions.whisper, Reply, "reply", "r")
			{
				HelpText = GetString("Replies to a PM sent to you.")
			});
			add(new Command(Rests.RestPermissions.restmanage, ManageRest, "rest")
			{
				HelpText = GetString("Manages the REST API.")
			});
			add(new Command(Permissions.slap, Slap, "slap")
			{
				HelpText = GetString("Slaps a player, dealing damage.")
			});
			add(new Command(Permissions.serverinfo, ServerInfo, "serverinfo")
			{
				HelpText = GetString("Shows the server information.")
			});
			add(new Command(Permissions.warp, Warp, "warp")
			{
				HelpText = GetString("Teleports you to a warp point or manages warps.")
			});
			add(new Command(Permissions.whisper, Whisper, "whisper", "w", "tell", "pm", "dm")
			{
				HelpText = GetString("Sends a PM to a player.")
			});
			add(new Command(Permissions.whisper, Wallow, "wallow", "wa")
			{
				AllowServer = false,
				HelpText = GetString("Toggles to either ignore or recieve whispers from other players.")
			});
			add(new Command(Permissions.createdumps, CreateDumps, "dump-reference-data")
			{
				HelpText = GetString("Creates a reference tables for Terraria data types and the TShock permission system in the server folder.")
			});
			add(new Command(Permissions.synclocalarea, SyncLocalArea, "sync")
			{
				HelpText = GetString("Sends all tiles from the server to the player to resync the client with the actual world state.")
			});
			add(new Command(Permissions.respawn, Respawn, "respawn")
			{
				HelpText = GetString("Respawn yourself or another player.")
			});
			#endregion

			add(new Command(Aliases, "aliases")
			{
				HelpText = GetString("Shows a command's aliases.")
			});
			add(new Command(Help, "help")
			{
				HelpText = GetString("Lists commands or gives help on them.")
			});
			add(new Command(Motd, "motd")
			{
				HelpText = GetString("Shows the message of the day.")
			});
			add(new Command(ListConnectedPlayers, "playing", "online", "who")
			{
				HelpText = GetString("Shows the currently connected players.")
			});
			add(new Command(Rules, "rules")
			{
				HelpText = GetString("Shows the server's rules.")
			});

			TShockCommands = new ReadOnlyCollection<Command>(tshockCommands);
		}

		public static bool HandleCommand(TSPlayer player, string text)
		{
			string cmdText = text.Remove(0, 1);
			string cmdPrefix = text[0].ToString();
			bool silent = false;

			if (cmdPrefix == SilentSpecifier)
				silent = true;

			int index = -1;
			for (int i = 0; i < cmdText.Length; i++)
			{
				if (IsWhiteSpace(cmdText[i]))
				{
					index = i;
					break;
				}
			}
			string cmdName;
			if (index == 0) // Space after the command specifier should not be supported
			{
				player.SendErrorMessage(GetString("You entered a space after {0} instead of a command. Type {0}help for a list of valid commands.", Specifier));
				return true;
			}
			else if (index < 0)
				cmdName = cmdText.ToLower();
			else
				cmdName = cmdText.Substring(0, index).ToLower();

			List<string> args;
			if (index < 0)
				args = new List<string>();
			else
				args = ParseParameters(cmdText.Substring(index));

			IEnumerable<Command> cmds = ChatCommands.FindAll(c => c.HasAlias(cmdName));

			if (Hooks.PlayerHooks.OnPlayerCommand(player, cmdName, cmdText, args, ref cmds, cmdPrefix))
				return true;

			if (cmds.Count() == 0)
			{
				if (player.AwaitingResponse.ContainsKey(cmdName))
				{
					Action<CommandArgs> call = player.AwaitingResponse[cmdName];
					player.AwaitingResponse.Remove(cmdName);
					call(new CommandArgs(cmdText, player, args));
					return true;
				}
				player.SendErrorMessage(GetString("Invalid command entered. Type {0}help for a list of valid commands.", Specifier));
				return true;
			}
			foreach (Command cmd in cmds)
			{
				if (!cmd.CanRun(player))
				{
					if (cmd.DoLog)
						TShock.Utils.SendLogs(GetString("{0} tried to execute {1}{2}.", player.Name, Specifier, cmdText), Color.PaleVioletRed, player);
					else
						TShock.Utils.SendLogs(GetString("{0} tried to execute (args omitted) {1}{2}.", player.Name, Specifier, cmdName), Color.PaleVioletRed, player);
					player.SendErrorMessage(GetString("You do not have access to this command."));
					if (player.HasPermission(Permissions.su))
					{
						player.SendInfoMessage(GetString("You can use '{0}sudo {0}{1}' to override this check.", Specifier, cmdText));
					}
				}
				else if (!cmd.AllowServer && !player.RealPlayer)
				{
					player.SendErrorMessage(GetString("You must use this command in-game."));
				}
				else
				{
					if (cmd.DoLog)
						TShock.Utils.SendLogs(GetString("{0} executed: {1}{2}.", player.Name, silent ? SilentSpecifier : Specifier, cmdText), Color.PaleVioletRed, player);
					else
						TShock.Utils.SendLogs(GetString("{0} executed (args omitted): {1}{2}.", player.Name, silent ? SilentSpecifier : Specifier, cmdName), Color.PaleVioletRed, player);

					CommandArgs arguments = new CommandArgs(cmdText, silent, player, args);
					bool handled = PlayerHooks.OnPrePlayerCommand(cmd, ref arguments);
					if (!handled)
						cmd.Run(arguments);
					PlayerHooks.OnPostPlayerCommand(cmd, arguments, handled);
				}
			}
			return true;
		}

		/// <summary>
		/// Parses a string of parameters into a list. Handles quotes.
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		private static List<String> ParseParameters(string str)
		{
			var ret = new List<string>();
			var sb = new StringBuilder();
			bool instr = false;
			for (int i = 0; i < str.Length; i++)
			{
				char c = str[i];

				if (c == '\\' && ++i < str.Length)
				{
					if (str[i] != '"' && str[i] != ' ' && str[i] != '\\')
						sb.Append('\\');
					sb.Append(str[i]);
				}
				else if (c == '"')
				{
					instr = !instr;
					if (!instr)
					{
						ret.Add(sb.ToString());
						sb.Clear();
					}
					else if (sb.Length > 0)
					{
						ret.Add(sb.ToString());
						sb.Clear();
					}
				}
				else if (IsWhiteSpace(c) && !instr)
				{
					if (sb.Length > 0)
					{
						ret.Add(sb.ToString());
						sb.Clear();
					}
				}
				else
					sb.Append(c);
			}
			if (sb.Length > 0)
				ret.Add(sb.ToString());

			return ret;
		}

		private static bool IsWhiteSpace(char c)
		{
			return c == ' ' || c == '\t' || c == '\n';
		}

		#region Account commands

		private static void AttemptLogin(CommandArgs args)
		{
			if (args.Player.LoginAttempts > TShock.Config.Settings.MaximumLoginAttempts && (TShock.Config.Settings.MaximumLoginAttempts != -1))
			{
				TShock.Log.Warn(GetString("{0} ({1}) had {2} or more invalid login attempts and was kicked automatically.",
					args.Player.IP, args.Player.Name, TShock.Config.Settings.MaximumLoginAttempts));
				args.Player.Kick(GetString("Too many invalid login attempts."));
				return;
			}

			if (args.Player.IsLoggedIn)
			{
				args.Player.SendErrorMessage(GetString("You are already logged in, and cannot login again."));
				return;
			}

			// We need to emulate the checks done in Player.TrySwitchingLoadout, because otherwise the server is not allowed to sync the
			// loadout index to the player, causing catastrophic desync.
			// The player must not be dead, using an item, or CC'd to switch loadouts.

			// Note that we only check for CC'd players when SSC is enabled, as that is only where it can cause issues,
			// and the RequireLogin config option (without SSC) will disable player's until they login, creating a vicious cycle.

			// FIXME: There is always the chance that in-between the time we check these requirements on the server, and the loadout sync
			//		  packet reaches the client, that the client state has changed, causing the loadout sync to be rejected, even though
			//		  we expected it to succeed.

			if (args.TPlayer.dead)
			{
				args.Player.SendErrorMessage(GetString("You cannot login whilst dead."));
				return;
			}

			// FIXME: This check is not correct -- even though we reject PlayerAnimation whilst disabled, we don't re-sync it to the client,
			//		  meaning these will still be set on the client, and they WILL reject the loadout sync.
			if (args.TPlayer.itemTime > 0 || args.TPlayer.itemAnimation > 0)
			{
				args.Player.SendErrorMessage(GetString("You cannot login whilst using an item."));
				return;
			}

			if (args.TPlayer.CCed && Main.ServerSideCharacter)
			{
				args.Player.SendErrorMessage(GetString("You cannot login whilst crowd controlled."));
				return;
			}

			UserAccount account = TShock.UserAccounts.GetUserAccountByName(args.Player.Name);
			string password = "";
			bool usingUUID = false;
			if (args.Parameters.Count == 0 && !TShock.Config.Settings.DisableUUIDLogin)
			{
				if (PlayerHooks.OnPlayerPreLogin(args.Player, args.Player.Name, ""))
					return;
				usingUUID = true;
			}
			else if (args.Parameters.Count == 1)
			{
				if (PlayerHooks.OnPlayerPreLogin(args.Player, args.Player.Name, args.Parameters[0]))
					return;
				password = args.Parameters[0];
			}
			else if (args.Parameters.Count == 2 && TShock.Config.Settings.AllowLoginAnyUsername)
			{
				if (String.IsNullOrEmpty(args.Parameters[0]))
				{
					args.Player.SendErrorMessage(GetString("Bad login attempt."));
					return;
				}

				if (PlayerHooks.OnPlayerPreLogin(args.Player, args.Parameters[0], args.Parameters[1]))
					return;

				account = TShock.UserAccounts.GetUserAccountByName(args.Parameters[0]);
				password = args.Parameters[1];
			}
			else
			{
				if (!TShock.Config.Settings.DisableUUIDLogin)
					args.Player.SendMessage(GetString($"{Specifier}login - Authenticates you using your UUID and character name."), Color.White);

				if (TShock.Config.Settings.AllowLoginAnyUsername)
					args.Player.SendMessage(GetString($"{Specifier}login <username> <password> - Authenticates you using your username and password."), Color.White);
				else
					args.Player.SendMessage(GetString($"{Specifier}login <password> - Authenticates you using your password and character name."), Color.White);

				args.Player.SendWarningMessage(GetString("If you forgot your password, contact the administrator for help."));
				return;
			}
			try
			{
				if (account == null)
				{
					args.Player.SendErrorMessage(GetString("A user account by that name does not exist."));
				}
				else if (account.VerifyPassword(password) ||
						(usingUUID && account.UUID == args.Player.UUID && !TShock.Config.Settings.DisableUUIDLogin &&
						!String.IsNullOrWhiteSpace(args.Player.UUID)))
				{
					var group = TShock.Groups.GetGroupByName(account.Group);

					if (!TShock.Groups.AssertGroupValid(args.Player, group, false))
					{
						args.Player.SendErrorMessage(GetString("Login attempt failed - see the message above."));
						return;
					}

					args.Player.PlayerData = TShock.CharacterDB.GetPlayerData(args.Player, account.ID);

					args.Player.Group = group;
					args.Player.tempGroup = null;
					args.Player.Account = account;
					args.Player.IsLoggedIn = true;
					args.Player.IsDisabledForSSC = false;

					if (Main.ServerSideCharacter)
					{
						if (args.Player.HasPermission(Permissions.bypassssc))
						{
							args.Player.PlayerData.CopyCharacter(args.Player);
							TShock.CharacterDB.InsertPlayerData(args.Player);
						}
						args.Player.PlayerData.RestoreCharacter(args.Player);
					}
					args.Player.LoginFailsBySsi = false;

					if (args.Player.HasPermission(Permissions.ignorestackhackdetection))
						args.Player.IsDisabledForStackDetection = false;

					if (args.Player.HasPermission(Permissions.usebanneditem))
						args.Player.IsDisabledForBannedWearable = false;

					args.Player.SendSuccessMessage(GetString("Authenticated as {0} successfully.", account.Name));

					TShock.Log.ConsoleInfo(GetString("{0} authenticated successfully as user: {1}.", args.Player.Name, account.Name));
					if ((args.Player.LoginHarassed) && (TShock.Config.Settings.RememberLeavePos))
					{
						if (TShock.RememberedPos.GetLeavePos(args.Player.Name, args.Player.IP) != Vector2.Zero)
						{
							Vector2 pos = TShock.RememberedPos.GetLeavePos(args.Player.Name, args.Player.IP);
							args.Player.Teleport((int)pos.X * 16, (int)pos.Y * 16);
						}
						args.Player.LoginHarassed = false;

					}
					TShock.UserAccounts.SetUserAccountUUID(account, args.Player.UUID);

					Hooks.PlayerHooks.OnPlayerPostLogin(args.Player);
				}
				else
				{
					if (usingUUID && !TShock.Config.Settings.DisableUUIDLogin)
					{
						args.Player.SendErrorMessage(GetString("UUID does not match this character."));
					}
					else
					{
						args.Player.SendErrorMessage(GetString("Invalid password."));
					}
					TShock.Log.Warn(GetString("{0} failed to authenticate as user: {1}.", args.Player.IP, account.Name));
					args.Player.LoginAttempts++;
				}
			}
			catch (Exception ex)
			{
				args.Player.SendErrorMessage(GetString("There was an error processing your login or authentication related request."));
				TShock.Log.Error(ex.ToString());
			}
		}

		private static void Logout(CommandArgs args)
		{
			if (!args.Player.IsLoggedIn)
			{
				args.Player.SendErrorMessage(GetString("You are not logged-in. Therefore, you cannot logout."));
				return;
			}

			if (args.Player.TPlayer.talkNPC != -1)
			{
				args.Player.SendErrorMessage(GetString("Please close NPC windows before logging out."));
				return;
			}

			args.Player.Logout();
			args.Player.SendSuccessMessage(GetString("You have been successfully logged out of your account."));
			if (Main.ServerSideCharacter)
			{
				args.Player.SendWarningMessage(GetString("Server side characters are enabled. You need to be logged-in to play."));
			}
		}

		private static void PasswordUser(CommandArgs args)
		{
			try
			{
				if (args.Player.IsLoggedIn && args.Parameters.Count == 2)
				{
					string password = args.Parameters[0];
					if (args.Player.Account.VerifyPassword(password))
					{
						try
						{
							args.Player.SendSuccessMessage(GetString("You have successfully changed your password."));
							TShock.UserAccounts.SetUserAccountPassword(args.Player.Account, args.Parameters[1]); // SetUserPassword will hash it for you.
							TShock.Log.ConsoleInfo(GetString("{0} ({1}) changed the password for account {2}.", args.Player.IP, args.Player.Name, args.Player.Account.Name));
						}
						catch (ArgumentOutOfRangeException)
						{
							args.Player.SendErrorMessage(GetString("Password must be greater than or equal to {0} characters.", TShock.Config.Settings.MinimumPasswordLength));
						}
					}
					else
					{
						args.Player.SendErrorMessage(GetString("You failed to change your password."));
						TShock.Log.ConsoleInfo(GetString("{0} ({1}) failed to change the password for account {2}.", args.Player.IP, args.Player.Name, args.Player.Account.Name));
					}
				}
				else
				{
					args.Player.SendErrorMessage(GetString("Not logged in or Invalid syntax. Proper syntax: {0}password <oldpassword> <newpassword>.", Specifier));
				}
			}
			catch (UserAccountManagerException ex)
			{
				args.Player.SendErrorMessage(GetString("Sorry, an error occurred: {0}.", ex.Message));
				TShock.Log.ConsoleError(GetString("PasswordUser returned an error: {0}.", ex));
			}
		}

		private static void RegisterUser(CommandArgs args)
		{
			try
			{
				var account = new UserAccount();
				string echoPassword = "";
				if (args.Parameters.Count == 1)
				{
					account.Name = args.Player.Name;
					echoPassword = args.Parameters[0];
					try
					{
						account.CreateBCryptHash(args.Parameters[0]);
					}
					catch (ArgumentOutOfRangeException)
					{
						args.Player.SendErrorMessage(GetString("Password must be greater than or equal to {0} characters.", TShock.Config.Settings.MinimumPasswordLength));
						return;
					}
				}
				else if (args.Parameters.Count == 2 && TShock.Config.Settings.AllowRegisterAnyUsername)
				{
					account.Name = args.Parameters[0];
					echoPassword = args.Parameters[1];
					try
					{
						account.CreateBCryptHash(args.Parameters[1]);
					}
					catch (ArgumentOutOfRangeException)
					{
						args.Player.SendErrorMessage(GetString("Password must be greater than or equal to {0} characters.", TShock.Config.Settings.MinimumPasswordLength));
						return;
					}
				}
				else
				{
					args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}register <password>.", Specifier));
					return;
				}

				account.Group = TShock.Config.Settings.DefaultRegistrationGroupName; // FIXME -- we should get this from the DB. --Why?
				account.UUID = args.Player.UUID;

				if (TShock.UserAccounts.GetUserAccountByName(account.Name) == null && account.Name != TSServerPlayer.AccountName) // Cheap way of checking for existance of a user
				{
					args.Player.SendSuccessMessage(GetString("Your account, \"{0}\", has been registered.", account.Name));
					args.Player.SendSuccessMessage(GetString("Your password is {0}.", echoPassword));

					if (!TShock.Config.Settings.DisableUUIDLogin)
						args.Player.SendMessage(GetString($"Type {Specifier}login to log-in to your account using your UUID."), Color.White);

					if (TShock.Config.Settings.AllowLoginAnyUsername)
						args.Player.SendMessage(GetString($"Type {Specifier}login \"{account.Name.Color(Utils.GreenHighlight)}\" {echoPassword.Color(Utils.BoldHighlight)} to log-in to your account."), Color.White);
					else
						args.Player.SendMessage(GetString($"Type {Specifier}login {echoPassword.Color(Utils.BoldHighlight)} to log-in to your account."), Color.White);

					TShock.UserAccounts.AddUserAccount(account);
					TShock.Log.ConsoleInfo(GetString("{0} registered an account: \"{1}\".", args.Player.Name, account.Name));
				}
				else
				{
					args.Player.SendErrorMessage(GetString("Sorry, {0} was already taken by another person.", account.Name));
					args.Player.SendErrorMessage(GetString("Please try a different username."));
					TShock.Log.ConsoleInfo(GetString("{0} attempted to register for the account {1} but it was already taken.", args.Player.Name, account.Name));
				}
			}
			catch (UserAccountManagerException ex)
			{
				args.Player.SendErrorMessage(GetString("Sorry, an error occurred: {0}.", ex.Message));
				TShock.Log.ConsoleError(GetString("RegisterUser returned an error: {0}.", ex));
			}
		}

		private static void ManageUsers(CommandArgs args)
		{
			// This guy needs to be here so that people don't get exceptions when they type /user
			if (args.Parameters.Count < 1)
			{
				args.Player.SendErrorMessage(GetString("Invalid user syntax. Try {0}user help.", Specifier));
				return;
			}

			string subcmd = args.Parameters[0];

			// Add requires a username, password, and a group specified.
			if (subcmd == "add" && args.Parameters.Count == 4)
			{
				var account = new UserAccount();

				account.Name = args.Parameters[1];
				try
				{
					account.CreateBCryptHash(args.Parameters[2]);
				}
				catch (ArgumentOutOfRangeException)
				{
					args.Player.SendErrorMessage(GetString("Password must be greater than or equal to {0} characters.", TShock.Config.Settings.MinimumPasswordLength));
					return;
				}
				account.Group = args.Parameters[3];

				try
				{
					TShock.UserAccounts.AddUserAccount(account);
					args.Player.SendSuccessMessage(GetString("Account {0} has been added to group {1}.", account.Name, account.Group));
					TShock.Log.ConsoleInfo(GetString("{0} added account {1} to group {2}.", args.Player.Name, account.Name, account.Group));
				}
				catch (GroupNotExistsException)
				{
					args.Player.SendErrorMessage(GetString("Group {0} does not exist.", account.Group));
				}
				catch (UserAccountExistsException)
				{
					args.Player.SendErrorMessage(GetString("User {0} already exists.", account.Name));
				}
				catch (UserAccountManagerException e)
				{
					args.Player.SendErrorMessage(GetString("User {0} could not be added, check console for details.", account.Name));
					TShock.Log.ConsoleError(e.ToString());
				}
			}
			// User deletion requires a username
			else if (subcmd == "del" && args.Parameters.Count == 2)
			{
				var account = new UserAccount();
				account.Name = args.Parameters[1];

				try
				{
					TShock.UserAccounts.RemoveUserAccount(account);
					args.Player.SendSuccessMessage(GetString("Account removed successfully."));
					TShock.Log.ConsoleInfo(GetString("{0} successfully deleted account: {1}.", args.Player.Name, args.Parameters[1]));
				}
				catch (UserAccountNotExistException)
				{
					args.Player.SendErrorMessage(GetString("The user {0} does not exist! Therefore, the account was not deleted.", account.Name));
				}
				catch (UserAccountManagerException ex)
				{
					args.Player.SendErrorMessage(ex.Message);
					TShock.Log.ConsoleError(ex.ToString());
				}
			}

			// Password changing requires a username, and a new password to set
			else if (subcmd == "password" && args.Parameters.Count == 3)
			{
				var account = new UserAccount();
				account.Name = args.Parameters[1];

				try
				{
					TShock.UserAccounts.SetUserAccountPassword(account, args.Parameters[2]);
					TShock.Log.ConsoleInfo(GetString("{0} changed the password for account {1}", args.Player.Name, account.Name));
					args.Player.SendSuccessMessage(GetString("Password change succeeded for {0}.", account.Name));
				}
				catch (UserAccountNotExistException)
				{
					args.Player.SendErrorMessage(GetString("Account {0} does not exist! Therefore, the password cannot be changed.", account.Name));
				}
				catch (UserAccountManagerException e)
				{
					args.Player.SendErrorMessage(GetString("Password change attempt for {0} failed for an unknown reason. Check the server console for more details.", account.Name));
					TShock.Log.ConsoleError(e.ToString());
				}
				catch (ArgumentOutOfRangeException)
				{
					args.Player.SendErrorMessage(GetString("Password must be greater than or equal to {0} characters.", TShock.Config.Settings.MinimumPasswordLength));
				}
			}
			// Group changing requires a username or IP address, and a new group to set
			else if (subcmd == "group" && args.Parameters.Count == 3)
			{
				var account = new UserAccount();
				account.Name = args.Parameters[1];

				try
				{
					TShock.UserAccounts.SetUserGroup(args.Player, account, args.Parameters[2]);
					TShock.Log.ConsoleInfo(GetString("{0} changed account {1} to group {2}.", args.Player.Name, account.Name, args.Parameters[2]));
					args.Player.SendSuccessMessage(GetString("Account {0} has been changed to group {1}.", account.Name, args.Parameters[2]));

					//send message to player with matching account name
					var player = TShock.Players.FirstOrDefault(p => p != null && p.Account?.Name == account.Name);
					if (player != null && !args.Silent)
						player.SendSuccessMessage(GetString($"{args.Player.Name} has changed your group to {args.Parameters[2]}."));
				}
				catch (GroupNotExistsException)
				{
					args.Player.SendErrorMessage(GetString("That group does not exist."));
				}
				catch (UserAccountNotExistException)
				{
					args.Player.SendErrorMessage(GetString($"User {account.Name} does not exist."));
				}
				catch (UserGroupUpdateLockedException)
				{
					args.Player.SendErrorMessage(GetString("Hook blocked the attempt to change the user group."));
				}
				catch (UserAccountManagerException e)
				{
					args.Player.SendErrorMessage(GetString($"User {account.Name} could not be added. Check console for details."));
					TShock.Log.ConsoleError(e.ToString());
				}
			}
			else if (subcmd == "help")
			{
				args.Player.SendInfoMessage(GetString("User management command help:"));
				args.Player.SendInfoMessage(GetString("{0}user add username password group   -- Adds a specified user", Specifier));
				args.Player.SendInfoMessage(GetString("{0}user del username                  -- Removes a specified user", Specifier));
				args.Player.SendInfoMessage(GetString("{0}user password username newpassword -- Changes a user's password", Specifier));
				args.Player.SendInfoMessage(GetString("{0}user group username newgroup       -- Changes a user's group", Specifier));
			}
			else
			{
				args.Player.SendErrorMessage(GetString("Invalid user syntax. Try {0}user help.", Specifier));
			}
		}

		#endregion

		#region Stupid commands

		private static void ServerInfo(CommandArgs args)
		{
			args.Player.SendInfoMessage(GetString($"Memory usage: {Process.GetCurrentProcess().WorkingSet64}"));
			args.Player.SendInfoMessage(GetString($"Allocated memory: {Process.GetCurrentProcess().VirtualMemorySize64}"));
			args.Player.SendInfoMessage(GetString($"Total processor time: {Process.GetCurrentProcess().TotalProcessorTime}"));
			args.Player.SendInfoMessage(GetString($"Operating system: {Environment.OSVersion}"));
			args.Player.SendInfoMessage(GetString($"Proc count: {Environment.ProcessorCount}"));
			args.Player.SendInfoMessage(GetString($"Machine name: {Environment.MachineName}"));
		}

		private static void WorldInfo(CommandArgs args)
		{
			args.Player.SendInfoMessage(GetString("Information about the currently running world"));
			args.Player.SendInfoMessage(GetString($"Name: {(TShock.Config.Settings.UseServerName ? TShock.Config.Settings.ServerName : Main.worldName)}"));
			args.Player.SendInfoMessage(GetString("Size: {0}x{1}", Main.maxTilesX, Main.maxTilesY));
			args.Player.SendInfoMessage(GetString($"ID: {Main.worldID}"));
			args.Player.SendInfoMessage(GetString($"Seed: {WorldGen.currentWorldSeed}"));
			args.Player.SendInfoMessage(GetString($"Mode: {Main.GameMode}"));
			args.Player.SendInfoMessage(GetString($"Path: {Main.worldPathName}"));
		}

		#endregion

		#region Player Management Commands

		private static void GrabUserUserInfo(CommandArgs args)
		{
			if (args.Parameters.Count < 1)
			{
				args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}userinfo <player>.", Specifier));
				return;
			}

			var players = TSPlayer.FindByNameOrID(args.Parameters[0]);
			if (players.Count < 1)
				args.Player.SendErrorMessage(GetString("Invalid player."));
			else if (players.Count > 1)
				args.Player.SendMultipleMatchError(players.Select(p => p.Name));
			else
			{
				args.Player.SendSuccessMessage(GetString($"IP Address: {players[0].IP}."));
				if (players[0].Account != null && players[0].IsLoggedIn)
					args.Player.SendSuccessMessage(GetString($" -> Logged-in as: {players[0].Account.Name}; in group {players[0].Group.Name}."));
			}
		}

		private static void ViewAccountInfo(CommandArgs args)
		{
			if (args.Parameters.Count < 1)
			{
				args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}accountinfo <username>.", Specifier));
				return;
			}

			string username = String.Join(" ", args.Parameters);
			if (!string.IsNullOrWhiteSpace(username))
			{
				var account = TShock.UserAccounts.GetUserAccountByName(username);
				if (account != null)
				{
					DateTime LastSeen;
					string Timezone = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Hours.ToString("+#;-#");

					if (DateTime.TryParse(account.LastAccessed, out LastSeen))
					{
						LastSeen = DateTime.Parse(account.LastAccessed).ToLocalTime();
						args.Player.SendSuccessMessage(GetString("{0}'s last login occurred {1} {2} UTC{3}.", account.Name, LastSeen.ToShortDateString(),
							LastSeen.ToShortTimeString(), Timezone));
					}

					if (args.Player.Group.HasPermission(Permissions.advaccountinfo))
					{
						List<string> KnownIps = JsonConvert.DeserializeObject<List<string>>(account.KnownIps?.ToString() ?? string.Empty);
						string ip = KnownIps?[KnownIps.Count - 1] ?? GetString("N/A");
						DateTime Registered = DateTime.Parse(account.Registered).ToLocalTime();

						args.Player.SendSuccessMessage(GetString("{0}'s group is {1}.", account.Name, account.Group));
						args.Player.SendSuccessMessage(GetString("{0}'s last known IP is {1}.", account.Name, ip));
						args.Player.SendSuccessMessage(GetString("{0}'s register date is {1} {2} UTC{3}.", account.Name, Registered.ToShortDateString(), Registered.ToShortTimeString(), Timezone));
					}
				}
				else
					args.Player.SendErrorMessage(GetString("User {0} does not exist.", username));
			}
			else args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}accountinfo <username>.", Specifier));
		}

		private static void Kick(CommandArgs args)
		{
			if (args.Parameters.Count < 1)
			{
				args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}kick <player> [reason].", Specifier));
				return;
			}
			if (args.Parameters[0].Length == 0)
			{
				args.Player.SendErrorMessage(GetString("A player name must be provided to kick a player. Please provide one."));
				return;
			}

			string plStr = args.Parameters[0];
			var players = TSPlayer.FindByNameOrID(plStr);
			if (players.Count == 0)
			{
				args.Player.SendErrorMessage(GetString("Player not found. Unable to kick the player."));
			}
			else if (players.Count > 1)
			{
				args.Player.SendMultipleMatchError(players.Select(p => p.Name));
			}
			else
			{
				string reason = args.Parameters.Count > 1
									? String.Join(" ", args.Parameters.GetRange(1, args.Parameters.Count - 1))
									: GetString("Misbehaviour.");
				if (!players[0].Kick(reason, !args.Player.RealPlayer, false, args.Player.Name))
				{
					args.Player.SendErrorMessage(GetString("You can't kick another admin."));
				}
			}
		}

		private static void Ban(CommandArgs args)
		{
			//Ban syntax:
			// ban add <target> [reason] [duration] [flags (default: -a -u -ip)]
			//						Duration is in the format 0d0h0m0s. Any part can be ignored. E.g., 1s is a valid ban time, as is 1d1s, etc. If no duration is specified, ban is permanent
			//						Valid flags: -a (ban account name), -u (ban UUID), -n (ban character name), -ip (ban IP address), -e (exact, ban the identifier provided as 'target')
			//						Unless -e is passed to the command, <target> is assumed to be a player or player index.
			// ban del <ban ID>
			//						Target is expected to be a ban Unique ID
			// ban list [page]
			//						Displays a paginated list of bans
			// ban details <ban ID>
			//						Target is expected to be a ban Unique ID
			//ban help [command]
			//						Provides extended help on specific ban commands

			void Help()
			{
				if (args.Parameters.Count > 1)
				{
					MoreHelp(args.Parameters[1].ToLower());
					return;
				}

				//TODO: Translate. The string interpolation here will break the text extractor.
				args.Player.SendMessage(GetString("TShock Ban Help"), Color.White);
				args.Player.SendMessage(GetString("Available Ban commands:"), Color.White);
				args.Player.SendMessage(GetString($"ban {"add".Color(Utils.RedHighlight)} <Target> [Flags]"), Color.White);
				args.Player.SendMessage(GetString($"ban {"del".Color(Utils.RedHighlight)} <Ban ID>"), Color.White);
				args.Player.SendMessage(GetString($"ban {"list".Color(Utils.RedHighlight)}"), Color.White);
				args.Player.SendMessage(GetString($"ban {"details".Color(Utils.RedHighlight)} <Ban ID>"), Color.White);
				args.Player.SendMessage(GetString($"Quick usage: {"ban add".Color(Utils.BoldHighlight)} {args.Player.Name.Color(Utils.RedHighlight)} \"Griefing\""), Color.White);
				args.Player.SendMessage(GetString($"For more info, use {"ban help".Color(Utils.BoldHighlight)} {"command".Color(Utils.RedHighlight)} or {"ban help".Color(Utils.BoldHighlight)} {"examples".Color(Utils.RedHighlight)}"), Color.White);
			}

			void MoreHelp(string cmd)
			{
				switch (cmd)
				{
					case "add":
						args.Player.SendMessage(GetString(""), Color.White);
						args.Player.SendMessage(GetString("Ban Add Syntax"), Color.White);
						args.Player.SendMessage(GetString($"{"ban add".Color(Utils.BoldHighlight)} <{"Target".Color(Utils.RedHighlight)}> [{"Reason".Color(Utils.BoldHighlight)}] [{"Duration".Color(Utils.PinkHighlight)}] [{"Flags".Color(Utils.GreenHighlight)}]"), Color.White);
						args.Player.SendMessage(GetString($"- {"Duration".Color(Utils.PinkHighlight)}: uses the format {"0d0m0s".Color(Utils.PinkHighlight)} to determine the length of the ban."), Color.White);
						args.Player.SendMessage(GetString($"   Eg a value of {"10d30m0s".Color(Utils.PinkHighlight)} would represent 10 days, 30 minutes, 0 seconds."), Color.White);
						args.Player.SendMessage(GetString($"   If no duration is provided, the ban will be permanent."), Color.White);
						args.Player.SendMessage(GetString($"- {"Flags".Color(Utils.GreenHighlight)}: -a (account name), -u (UUID), -n (character name), -ip (IP address), -e (exact, {"Target".Color(Utils.RedHighlight)} will be treated as identifier)"), Color.White);
						args.Player.SendMessage(GetString($"   Unless {"-e".Color(Utils.GreenHighlight)} is passed to the command, {"Target".Color(Utils.RedHighlight)} is assumed to be a player or player index"), Color.White);
						args.Player.SendMessage(GetString($"   If no {"Flags".Color(Utils.GreenHighlight)} are specified, the command uses {"-a -u -ip".Color(Utils.GreenHighlight)} by default."), Color.White);
						args.Player.SendMessage(GetString($"Example usage: {"ban add".Color(Utils.BoldHighlight)} {args.Player.Name.Color(Utils.RedHighlight)} {"\"Cheating\"".Color(Utils.BoldHighlight)} {"10d30m0s".Color(Utils.PinkHighlight)} {"-a -u -ip".Color(Utils.GreenHighlight)}"), Color.White);
						break;

					case "del":
						args.Player.SendMessage(GetString(""), Color.White);
						args.Player.SendMessage(GetString("Ban Del Syntax"), Color.White);
						args.Player.SendMessage(GetString($"{"ban del".Color(Utils.BoldHighlight)} <{"Ticket Number".Color(Utils.RedHighlight)}>"), Color.White);
						args.Player.SendMessage(GetString($"- {"Ticket Numbers".Color(Utils.RedHighlight)} are provided when you add a ban, and can also be viewed with the {"ban list".Color(Utils.BoldHighlight)} command."), Color.White);
						args.Player.SendMessage(GetString($"Example usage: {"ban del".Color(Utils.BoldHighlight)} {"12345".Color(Utils.RedHighlight)}"), Color.White);
						break;

					case "list":
						args.Player.SendMessage(GetString(""), Color.White);
						args.Player.SendMessage(GetString("Ban List Syntax"), Color.White);
						args.Player.SendMessage(GetString($"{"ban list".Color(Utils.BoldHighlight)} [{"Page".Color(Utils.PinkHighlight)}]"), Color.White);
						args.Player.SendMessage(GetString("- Lists active bans. Color trends towards green as the ban approaches expiration"), Color.White);
						args.Player.SendMessage(GetString($"Example usage: {"ban list".Color(Utils.BoldHighlight)}"), Color.White);
						break;

					case "details":
						args.Player.SendMessage(GetString(""), Color.White);
						args.Player.SendMessage(GetString("Ban Details Syntax"), Color.White);
						args.Player.SendMessage(GetString($"{"ban details".Color(Utils.BoldHighlight)} <{"Ticket Number".Color(Utils.RedHighlight)}>"), Color.White);
						args.Player.SendMessage(GetString($"- {"Ticket Numbers".Color(Utils.RedHighlight)} are provided when you add a ban, and can be found with the {"ban list".Color(Utils.BoldHighlight)} command."), Color.White);
						args.Player.SendMessage(GetString($"Example usage: {"ban details".Color(Utils.BoldHighlight)} {"12345".Color(Utils.RedHighlight)}"), Color.White);
						break;

					case "identifiers":
						if (!PaginationTools.TryParsePageNumber(args.Parameters, 2, args.Player, out int pageNumber))
						{
							args.Player.SendMessage(GetString($"Invalid page number. Page number must be numeric."), Color.White);
							return;
						}

						var idents = from ident in Identifier.Available
									 select $"{ident.Color(Utils.RedHighlight)} - {ident.Description}";

						args.Player.SendMessage(GetString(""), Color.White);
						PaginationTools.SendPage(args.Player, pageNumber, idents.ToList(),
							new PaginationTools.Settings
							{
								HeaderFormat = GetString("Available identifiers ({{0}}/{{1}}):"),
								FooterFormat = GetString("Type {0}ban help identifiers {{0}} for more.", Specifier),
								NothingToDisplayString = GetString("There are currently no available identifiers."),
								HeaderTextColor = Color.White,
								LineTextColor = Color.White
							});
						break;

					case "examples":
						args.Player.SendMessage(GetString(""), Color.White);
						args.Player.SendMessage(GetString("Ban Usage Examples"), Color.White);
						args.Player.SendMessage(GetString("- Ban an offline player by account name"), Color.White);
						args.Player.SendMessage(GetString($"   {Specifier}{"ban add".Color(Utils.BoldHighlight)} \"{"acc:".Color(Utils.RedHighlight)}{args.Player.Account.Color(Utils.RedHighlight)}\" {"\"Multiple accounts are not allowed\"".Color(Utils.BoldHighlight)} {"-e".Color(Utils.GreenHighlight)} (Permanently bans this account name)"), Color.White);
						args.Player.SendMessage(GetString("- Ban an offline player by IP address"), Color.White);
						args.Player.SendMessage(GetString($"   {Specifier}{"ai".Color(Utils.BoldHighlight)} \"{args.Player.Account.Color(Utils.RedHighlight)}\" (Find the IP associated with the offline target's account)"), Color.White);
						args.Player.SendMessage(GetString($"   {Specifier}{"ban add".Color(Utils.BoldHighlight)} {"ip:".Color(Utils.RedHighlight)}{args.Player.IP.Color(Utils.RedHighlight)} {"\"Griefing\"".Color(Utils.BoldHighlight)} {"-e".Color(Utils.GreenHighlight)} (Permanently bans this IP address)"), Color.White);
						args.Player.SendMessage(GetString($"- Ban an online player by index (Useful for hard to type names)"), Color.White);
						args.Player.SendMessage(GetString($"   {Specifier}{"who".Color(Utils.BoldHighlight)} {"-i".Color(Utils.GreenHighlight)} (Find the player index for the target)"), Color.White);
						args.Player.SendMessage(GetString($"   {Specifier}{"ban add".Color(Utils.BoldHighlight)} {"tsi:".Color(Utils.RedHighlight)}{args.Player.Index.Color(Utils.RedHighlight)} {"\"Trolling\"".Color(Utils.BoldHighlight)} {"-a -u -ip".Color(Utils.GreenHighlight)} (Permanently bans the online player by Account, UUID, and IP)"), Color.White);
						// Ban by account ID when?
						break;

					default:
						args.Player.SendMessage(GetString($"Unknown ban command. Try {"ban help".Color(Utils.BoldHighlight)} {"add".Color(Utils.RedHighlight)}, {"del".Color(Utils.RedHighlight)}, {"list".Color(Utils.RedHighlight)}, {"details".Color(Utils.RedHighlight)}, {"identifiers".Color(Utils.RedHighlight)}, or {"examples".Color(Utils.RedHighlight)}."), Color.White); break;
				}
			}

			void DisplayBanDetails(Ban ban)
			{
				args.Player.SendMessage(GetString($"{"Ban Details".Color(Utils.BoldHighlight)} - Ticket Number: {ban.TicketNumber.Color(Utils.GreenHighlight)}"), Color.White);
				args.Player.SendMessage(GetString($"{"Identifier:".Color(Utils.BoldHighlight)} {ban.Identifier}"), Color.White);
				args.Player.SendMessage(GetString($"{"Reason:".Color(Utils.BoldHighlight)} {ban.Reason}"), Color.White);
				args.Player.SendMessage(GetString($"{"Banned by:".Color(Utils.BoldHighlight)} {ban.BanningUser.Color(Utils.GreenHighlight)} on {ban.BanDateTime.ToString("yyyy/MM/dd").Color(Utils.RedHighlight)} ({ban.GetPrettyTimeSinceBanString().Color(Utils.YellowHighlight)} ago)"), Color.White);
				if (ban.ExpirationDateTime < DateTime.UtcNow)
				{
					args.Player.SendMessage(GetString($"{"Ban expired:".Color(Utils.BoldHighlight)} {ban.ExpirationDateTime.ToString("yyyy/MM/dd").Color(Utils.RedHighlight)} ({ban.GetPrettyExpirationString().Color(Utils.YellowHighlight)} ago)"), Color.White);
				}
				else
				{
					string remaining;
					if (ban.ExpirationDateTime == DateTime.MaxValue)
					{
						remaining = GetString("Never.").Color(Utils.YellowHighlight);
					}
					else
					{
						remaining = GetString($"{ban.GetPrettyExpirationString().Color(Utils.YellowHighlight)} remaining.");
					}

					args.Player.SendMessage(GetString($"{"Ban expires:".Color(Utils.BoldHighlight)} {ban.ExpirationDateTime.ToString("yyyy/MM/dd").Color(Utils.RedHighlight)} ({remaining})"), Color.White);
				}
			}

			AddBanResult DoBan(string ident, string reason, DateTime expiration)
			{
				AddBanResult banResult = TShock.Bans.InsertBan(ident, reason, args.Player.Account.Name, DateTime.UtcNow, expiration);
				if (banResult.Ban != null)
				{
					args.Player.SendSuccessMessage(GetString($"Ban added. Ticket Number {banResult.Ban.TicketNumber.Color(Utils.GreenHighlight)} was created for identifier {ident.Color(Utils.WhiteHighlight)}."));
				}
				else
				{
					args.Player.SendWarningMessage(GetString($"Failed to add ban for identifier: {ident.Color(Utils.WhiteHighlight)}."));
					args.Player.SendWarningMessage(GetString($"Reason: {banResult.Message}."));
				}

				return banResult;
			}

			void AddBan()
			{
				if (!args.Parameters.TryGetValue(1, out string target))
				{
					args.Player.SendMessage(GetString($"Invalid Ban Add syntax. Refer to {"ban help add".Color(Utils.BoldHighlight)} for details on how to use the {"ban add".Color(Utils.BoldHighlight)} command"), Color.White);
					return;
				}

				bool exactTarget = args.Parameters.Any(p => p == "-e");
				bool banAccount = args.Parameters.Any(p => p == "-a");
				bool banUuid = args.Parameters.Any(p => p == "-u");
				bool banName = args.Parameters.Any(p => p == "-n");
				bool banIp = args.Parameters.Any(p => p == "-ip");

				List<string> flags = new List<string>() { "-e", "-a", "-u", "-n", "-ip" };

				string reason = GetString("Banned.");
				string duration = null;
				DateTime expiration = DateTime.MaxValue;

				//This is hacky. We want flag values to be independent of order so we must force the consecutive ordering of the 'reason' and 'duration' parameters,
				//while still allowing them to be placed arbitrarily in the parameter list.
				//As an example, the following parameter lists (and more) should all be acceptable:
				//-u "reason" -a duration -ip
				//"reason" duration -u -a -ip
				//-u -a -ip "reason" duration
				//-u -a -ip
				for (int i = 2; i < args.Parameters.Count; i++)
				{
					var param = args.Parameters[i];
					if (!flags.Contains(param))
					{
						reason = param;
						break;
					}
				}
				for (int i = 3; i < args.Parameters.Count; i++)
				{
					var param = args.Parameters[i];
					if (!flags.Contains(param))
					{
						duration = param;
						break;
					}
				}

				if (TShock.Utils.TryParseTime(duration, out ulong seconds))
				{
					expiration = DateTime.UtcNow.AddSeconds(seconds);
				}

				//If no flags were specified, default to account, uuid, and IP
				if (!exactTarget && !banAccount && !banUuid && !banName && !banIp)
				{
					banAccount = banUuid = banIp = true;

					if (TShock.Config.Settings.DisableDefaultIPBan)
					{
						banIp = false;
					}
				}

				reason = reason ?? "Banned";

				if (exactTarget)
				{
					DoBan(target, reason, expiration);
					return;
				}

				var players = TSPlayer.FindByNameOrID(target);

				if (players.Count > 1)
				{
					args.Player.SendMultipleMatchError(players.Select(p => p.Name));
					return;
				}

				if (players.Count < 1)
				{
					args.Player.SendErrorMessage(GetString("Could not find the target specified. Check that you have the correct spelling."));
					return;
				}

				var player = players[0];
				AddBanResult banResult = null;

				if (banAccount)
				{
					if (player.Account != null)
					{
						banResult = DoBan($"{Identifier.Account}{player.Account.Name}", reason, expiration);
					}
				}

				if (banUuid && player.UUID.Length > 0)
				{
					banResult = DoBan($"{Identifier.UUID}{player.UUID}", reason, expiration);
				}

				if (banName)
				{
					banResult = DoBan($"{Identifier.Name}{player.Name}", reason, expiration);
				}

				if (banIp)
				{
					banResult = DoBan($"{Identifier.IP}{player.IP}", reason, expiration);
				}

				if (banResult?.Ban != null)
				{
					player.Disconnect(GetString($"#{banResult.Ban.TicketNumber} - You have been banned: {banResult.Ban.Reason}."));
				}
			}

			void DelBan()
			{
				if (!args.Parameters.TryGetValue(1, out string target))
				{
					args.Player.SendMessage(GetString($"Invalid Ban Del syntax. Refer to {"ban help del".Color(Utils.BoldHighlight)} for details on how to use the {"ban del".Color(Utils.BoldHighlight)} command"), Color.White);
					return;
				}

				if (!int.TryParse(target, out int banId))
				{
					args.Player.SendMessage(GetString($"Invalid Ticket Number. Refer to {"ban help del".Color(Utils.BoldHighlight)} for details on how to use the {"ban del".Color(Utils.BoldHighlight)} command"), Color.White);
					return;
				}

				if (TShock.Bans.RemoveBan(banId))
				{
					TShock.Log.ConsoleInfo(GetString($"Ban {banId} has been revoked by {args.Player.Account.Name}."));
					args.Player.SendSuccessMessage(GetString($"Ban {banId.Color(Utils.GreenHighlight)} has now been marked as expired."));
				}
				else
				{
					args.Player.SendErrorMessage(GetString("Failed to remove ban."));
				}
			}

			void ListBans()
			{
				string PickColorForBan(Ban ban)
				{
					double hoursRemaining = (ban.ExpirationDateTime - DateTime.UtcNow).TotalHours;
					double hoursTotal = (ban.ExpirationDateTime - ban.BanDateTime).TotalHours;
					double percentRemaining = TShock.Utils.Clamp(hoursRemaining / hoursTotal, 100, 0);

					int red = TShock.Utils.Clamp((int)(255 * 2.0f * percentRemaining), 255, 0);
					int green = TShock.Utils.Clamp((int)(255 * (2.0f * (1 - percentRemaining))), 255, 0);

					return $"{red:X2}{green:X2}{0:X2}";
				}

				if (!PaginationTools.TryParsePageNumber(args.Parameters, 1, args.Player, out int pageNumber))
				{
					args.Player.SendMessage(GetString($"Invalid Ban List syntax. Refer to {"ban help list".Color(Utils.BoldHighlight)} for details on how to use the {"ban list".Color(Utils.BoldHighlight)} command"), Color.White);
					return;
				}

				var bans = from ban in TShock.Bans.Bans
						   where ban.Value.ExpirationDateTime > DateTime.UtcNow
						   orderby ban.Value.ExpirationDateTime ascending
						   select $"[{ban.Key.Color(Utils.GreenHighlight)}] {ban.Value.Identifier.Color(PickColorForBan(ban.Value))}";

				PaginationTools.SendPage(args.Player, pageNumber, bans.ToList(),
					new PaginationTools.Settings
					{
						HeaderFormat = GetString("Bans ({{0}}/{{1}}):"),
						FooterFormat = GetString("Type {0}ban list {{0}} for more.", Specifier),
						NothingToDisplayString = GetString("There are currently no active bans.")
					});
			}

			void BanDetails()
			{
				if (!args.Parameters.TryGetValue(1, out string target))
				{
					args.Player.SendMessage(GetString($"Invalid Ban Details syntax. Refer to {"ban help details".Color(Utils.BoldHighlight)} for details on how to use the {"ban details".Color(Utils.BoldHighlight)} command"), Color.White);
					return;
				}

				if (!int.TryParse(target, out int banId))
				{
					args.Player.SendMessage(GetString($"Invalid Ticket Number. Refer to {"ban help details".Color(Utils.BoldHighlight)} for details on how to use the {"ban details".Color(Utils.BoldHighlight)} command"), Color.White);
					return;
				}

				Ban ban = TShock.Bans.GetBanById(banId);

				if (ban == null)
				{
					args.Player.SendErrorMessage(GetString("No bans found matching the provided ticket number."));
					return;
				}

				DisplayBanDetails(ban);
			}

			string subcmd = args.Parameters.Count == 0 ? "help" : args.Parameters[0].ToLower();
			switch (subcmd)
			{
				case "help":
					Help();
					break;

				case "add":
					AddBan();
					break;

				case "del":
					DelBan();
					break;

				case "list":
					ListBans();
					break;

				case "details":
					BanDetails();
					break;

				default:
					break;
			}
		}

		private static void Whitelist(CommandArgs args)
		{
			if (args.Parameters.Count == 1)
			{
				using (var tw = new StreamWriter(FileTools.WhitelistPath, true))
				{
					tw.WriteLine(args.Parameters[0]);
				}
				args.Player.SendSuccessMessage(GetString($"Added {args.Parameters[0]} to the whitelist."));
			}
		}

		private static void DisplayLogs(CommandArgs args)
		{
			args.Player.DisplayLogs = (!args.Player.DisplayLogs);
			if (args.Player.DisplayLogs)
			{
				args.Player.SendSuccessMessage(GetString("Log display enabled."));
			}
			else
			{
				args.Player.SendSuccessMessage(GetString("Log display disabled."));
			}
		}

		private static void SaveSSC(CommandArgs args)
		{
			if (Main.ServerSideCharacter)
			{
				args.Player.SendSuccessMessage(GetString("Your server-side character data has been saved."));
				foreach (TSPlayer player in TShock.Players)
				{
					if (player != null && player.IsLoggedIn && !player.IsDisabledPendingTrashRemoval)
					{
						TShock.CharacterDB.InsertPlayerData(player, true);
					}
				}
			}
		}

		private static void OverrideSSC(CommandArgs args)
		{
			if (!Main.ServerSideCharacter)
			{
				args.Player.SendErrorMessage(GetString("Server-side characters is disabled."));
				return;
			}
			if (args.Parameters.Count < 1)
			{
				args.Player.SendErrorMessage(GetString("Correct usage: {0}overridessc|{0}ossc <player name>", Specifier));
				return;
			}

			string playerNameToMatch = string.Join(" ", args.Parameters);
			var matchedPlayers = TSPlayer.FindByNameOrID(playerNameToMatch);
			if (matchedPlayers.Count < 1)
			{
				args.Player.SendErrorMessage(GetString("No players matched \"{0}\".", playerNameToMatch));
				return;
			}
			else if (matchedPlayers.Count > 1)
			{
				args.Player.SendMultipleMatchError(matchedPlayers.Select(p => p.Name));
				return;
			}

			TSPlayer matchedPlayer = matchedPlayers[0];
			if (matchedPlayer.IsLoggedIn)
			{
				args.Player.SendErrorMessage(GetString("Player \"{0}\" is already logged in.", matchedPlayer.Name));
				return;
			}
			if (!matchedPlayer.LoginFailsBySsi)
			{
				args.Player.SendErrorMessage(GetString("Player \"{0}\" has to perform a /login attempt first.", matchedPlayer.Name));
				return;
			}
			if (matchedPlayer.IsDisabledPendingTrashRemoval)
			{
				args.Player.SendErrorMessage(GetString("Player \"{0}\" has to reconnect first, because they need to delete their trash.", matchedPlayer.Name));
				return;
			}

			TShock.CharacterDB.InsertPlayerData(matchedPlayer);
			args.Player.SendSuccessMessage(GetString("Server-side character data from \"{0}\" has been replaced by their current local data.", matchedPlayer.Name));
		}

		private static void UploadJoinData(CommandArgs args)
		{
			TSPlayer targetPlayer = args.Player;
			if (args.Parameters.Count == 1 && args.Player.HasPermission(Permissions.uploadothersdata))
			{
				List<TSPlayer> players = TSPlayer.FindByNameOrID(args.Parameters[0]);
				if (players.Count > 1)
				{
					args.Player.SendMultipleMatchError(players.Select(p => p.Name));
					return;
				}
				else if (players.Count == 0)
				{
					args.Player.SendErrorMessage(GetString("No player was found matching '{0}'.", args.Parameters[0]));
					return;
				}
				else
				{
					targetPlayer = players[0];
				}
			}
			else if (args.Parameters.Count == 1)
			{
				args.Player.SendErrorMessage(GetString("You do not have permission to upload another player's character join-state server-side-character data."));
				return;
			}
			else if (args.Parameters.Count > 0)
			{
				args.Player.SendErrorMessage(GetString("Usage: /uploadssc [playername]."));
				return;
			}
			else if (args.Parameters.Count == 0 && args.Player is TSServerPlayer)
			{
				args.Player.SendErrorMessage(GetString("The targeted user cannot have their data uploaded, because they are not a player."));
				args.Player.SendErrorMessage(GetString("Usage: /uploadssc [playername]."));
				return;
			}

			if (targetPlayer.IsLoggedIn)
			{
				if (TShock.CharacterDB.InsertSpecificPlayerData(targetPlayer, targetPlayer.DataWhenJoined))
				{
					targetPlayer.DataWhenJoined.RestoreCharacter(targetPlayer);
					targetPlayer.SendSuccessMessage(GetString("Your local character data, from your initial connection, has been uploaded to the server."));
					args.Player.SendSuccessMessage(GetString("The player's character data was successfully uploaded from their initial connection."));
				}
				else
				{
					args.Player.SendErrorMessage(GetString("Failed to upload your character data to the server. Are you logged-in to an account?"));
				}
			}
			else
			{
				args.Player.SendErrorMessage(GetString("The target player has not logged in yet."));
			}
		}

		private static void ForceHalloween(CommandArgs args)
		{
			TShock.Config.Settings.ForceHalloween = !TShock.Config.Settings.ForceHalloween;
			Main.checkHalloween();
			if (args.Silent)
				if (TShock.Config.Settings.ForceHalloween)
					args.Player.SendInfoMessage(GetString("Enabled halloween mode."));
				else
					args.Player.SendInfoMessage(GetString("Disabled halloween mode."));
			else
			{
				if (TShock.Config.Settings.ForceHalloween)
					TSPlayer.All.SendInfoMessage(GetString("{0} enabled halloween mode.", args.Player.Name));
				else
					TSPlayer.All.SendInfoMessage(GetString("{0} disabled halloween mode.", args.Player.Name));
			}
		}

		private static void ForceXmas(CommandArgs args)
		{
			TShock.Config.Settings.ForceXmas = !TShock.Config.Settings.ForceXmas;
			Main.checkXMas();
			if (args.Silent)
			{
				if (TShock.Config.Settings.ForceXmas)
					args.Player.SendInfoMessage(GetString("Enabled xmas mode."));
				else
					args.Player.SendInfoMessage(GetString("Disabled xmas mode."));
			}
			else
			{
				if (TShock.Config.Settings.ForceXmas)
					TSPlayer.All.SendInfoMessage(GetString("{0} enabled xmas mode.", args.Player.Name));
				else
					TSPlayer.All.SendInfoMessage(GetString("{0} disabled xmas mode.", args.Player.Name));
			}
		}

		private static void TempGroup(CommandArgs args)
		{
			if (args.Parameters.Count < 2)
			{
				args.Player.SendInfoMessage(GetString("Invalid syntax."));
				args.Player.SendInfoMessage(GetString("Usage: {0}tempgroup <username> <new group> [time]", Specifier));
				return;
			}

			List<TSPlayer> ply = TSPlayer.FindByNameOrID(args.Parameters[0]);
			if (ply.Count < 1)
			{
				args.Player.SendErrorMessage(GetString("Could not find player {0}.", args.Parameters[0]));
				return;
			}

			if (ply.Count > 1)
			{
				args.Player.SendMultipleMatchError(ply.Select(p => p.Account.Name));
			}

			if (!TShock.Groups.GroupExists(args.Parameters[1]))
			{
				args.Player.SendErrorMessage(GetString("Could not find group {0}", args.Parameters[1]));
				return;
			}

			if (args.Parameters.Count > 2)
			{
				ulong time;
				if (!TShock.Utils.TryParseTime(args.Parameters[2], out time))
				{
					args.Player.SendErrorMessage(GetString("Invalid time string! Proper format: _d_h_m_s, with at least one time specifier."));
					args.Player.SendErrorMessage(GetString("For example, 1d and 10h-30m+2m are both valid time strings, but 2 is not."));
					return;
				}

				ply[0].tempGroupTimer = new System.Timers.Timer(time * 1000d);
				ply[0].tempGroupTimer.Elapsed += ply[0].TempGroupTimerElapsed;
				ply[0].tempGroupTimer.Start();
			}

			Group g = TShock.Groups.GetGroupByName(args.Parameters[1]);

			ply[0].tempGroup = g;

			if (args.Parameters.Count < 3)
			{
				args.Player.SendSuccessMessage(GetString("You have changed {0}'s group to {1}", ply[0].Name, g.Name));
				ply[0].SendSuccessMessage(GetString("Your group has temporarily been changed to {0}", g.Name));
			}
			else
			{
				args.Player.SendSuccessMessage(GetString("You have changed {0}'s group to {1} for {2}",
					ply[0].Name, g.Name, args.Parameters[2]));
				ply[0].SendSuccessMessage(GetString("Your group has been changed to {0} for {1}",
					g.Name, args.Parameters[2]));
			}
		}

		private static void SubstituteUser(CommandArgs args)
		{

			if (args.Player.tempGroup != null)
			{
				args.Player.tempGroup = null;
				args.Player.tempGroupTimer.Stop();
				args.Player.SendSuccessMessage(GetString("Your previous permission set has been restored."));
				return;
			}
			else
			{
				args.Player.tempGroup = new SuperAdminGroup();
				args.Player.tempGroupTimer = new System.Timers.Timer(600 * 1000);
				args.Player.tempGroupTimer.Elapsed += args.Player.TempGroupTimerElapsed;
				args.Player.tempGroupTimer.Start();
				args.Player.SendSuccessMessage(GetString("Your account has been elevated to superadmin for 10 minutes."));
				return;
			}
		}

		#endregion Player Management Commands

		#region Server Maintenence Commands

		// Executes a command as a superuser if you have sudo rights.
		private static void SubstituteUserDo(CommandArgs args)
		{
			if (args.Parameters.Count == 0)
			{
				args.Player.SendErrorMessage(GetString("Usage: /sudo [command]."));
				args.Player.SendErrorMessage(GetString("Example: /sudo /ban add particles 2d Hacking."));
				return;
			}

			string replacementCommand = String.Join(" ", args.Parameters.Select(p => p.Contains(" ") ? $"\"{p}\"" : p));
			args.Player.tempGroup = new SuperAdminGroup();
			HandleCommand(args.Player, replacementCommand);
			args.Player.tempGroup = null;
			return;
		}

		private static void Broadcast(CommandArgs args)
		{
			if (args.Parameters.Count > 0)
			{
				string message = string.Join(" ", args.Parameters);

				TShock.Utils.Broadcast(
					GetString("(Server Broadcast) ") + message,
					Convert.ToByte(TShock.Config.Settings.BroadcastRGB[0]), Convert.ToByte(TShock.Config.Settings.BroadcastRGB[1]),
					Convert.ToByte(TShock.Config.Settings.BroadcastRGB[2]));
			}
		}

		private static void Off(CommandArgs args)
		{

			if (Main.ServerSideCharacter)
			{
				foreach (TSPlayer player in TShock.Players)
				{
					if (player != null && player.IsLoggedIn && !player.IsDisabledPendingTrashRemoval)
					{
						player.SaveServerCharacter();
					}
				}
			}

			string reason = ((args.Parameters.Count > 0) ? GetString("Server shutting down: ") + String.Join(" ", args.Parameters) : GetString("Server shutting down!"));
			TShock.Utils.StopServer(true, reason);
		}

		private static void OffNoSave(CommandArgs args)
		{
			string reason = ((args.Parameters.Count > 0) ? GetString("Server shutting down: ") + String.Join(" ", args.Parameters) : GetString("Server shutting down."));
			Netplay.SaveOnServerExit = false;
			TShock.Utils.StopServer(false, reason);
		}

		private static void CheckUpdates(CommandArgs args)
		{
			args.Player.SendInfoMessage(GetString("An update check has been queued. If an update is available, you will be notified shortly."));
			try
			{
				TShock.UpdateManager.UpdateCheckAsync(null);
			}
			catch (Exception)
			{
				//swallow the exception
				return;
			}
		}

		private static void ManageRest(CommandArgs args)
		{
			string subCommand = "help";
			if (args.Parameters.Count > 0)
				subCommand = args.Parameters[0];

			switch (subCommand.ToLower())
			{
				case "listusers":
					{
						int pageNumber;
						if (!PaginationTools.TryParsePageNumber(args.Parameters, 1, args.Player, out pageNumber))
							return;

						Dictionary<string, int> restUsersTokens = new Dictionary<string, int>();
						foreach (Rests.SecureRest.TokenData tokenData in TShock.RestApi.Tokens.Values)
						{
							if (restUsersTokens.ContainsKey(tokenData.Username))
								restUsersTokens[tokenData.Username]++;
							else
								restUsersTokens.Add(tokenData.Username, 1);
						}

						List<string> restUsers = new List<string>(
							restUsersTokens.Select(ut => GetString("{0} ({1} tokens)", ut.Key, ut.Value)));

						PaginationTools.SendPage(
							args.Player, pageNumber, PaginationTools.BuildLinesFromTerms(restUsers), new PaginationTools.Settings
							{
								NothingToDisplayString = GetString("There are currently no active REST users."),
								HeaderFormat = GetString("Active REST Users ({{0}}/{{1}}):"),
								FooterFormat = GetString("Type {0}rest listusers {{0}} for more.", Specifier)
							}
						);

						break;
					}
				case "destroytokens":
					{
						TShock.RestApi.Tokens.Clear();
						args.Player.SendSuccessMessage(GetString("All REST tokens have been destroyed."));
						break;
					}
				default:
					{
						args.Player.SendInfoMessage(GetString("Available REST Sub-Commands:"));
						args.Player.SendMessage(GetString("listusers - Lists all REST users and their current active tokens."), Color.White);
						args.Player.SendMessage(GetString("destroytokens - Destroys all current REST tokens."), Color.White);
						break;
					}
			}
		}

		#endregion Server Maintenence Commands

		#region Cause Events and Spawn Monsters Commands

		static readonly List<string> _validEvents = new List<string>()
		{
			"meteor",
			"fullmoon",
			"bloodmoon",
			"eclipse",
			"invasion",
			"sandstorm",
			"rain",
			"lanternsnight"
		};
		static readonly List<string> _validInvasions = new List<string>()
		{
			"goblins",
			"snowmen",
			"pirates",
			"pumpkinmoon",
			"frostmoon",
			"martians"
		};

		private static void ManageWorldEvent(CommandArgs args)
		{
			if (args.Parameters.Count < 1)
			{
				args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}worldevent <event type>.", Specifier));
				args.Player.SendErrorMessage(GetString("Valid event types: {0}.", String.Join(", ", _validEvents)));
				args.Player.SendErrorMessage(GetString("Valid invasion types if spawning an invasion: {0}.", String.Join(", ", _validInvasions)));
				return;
			}

			var eventType = args.Parameters[0].ToLowerInvariant();

			void FailedPermissionCheck()
			{
				args.Player.SendErrorMessage(GetString("You do not have permission to start the {0} event.", eventType));
				return;
			}

			switch (eventType)
			{
				case "meteor":
					if (!args.Player.HasPermission(Permissions.dropmeteor) && !args.Player.HasPermission(Permissions.managemeteorevent))
					{
						FailedPermissionCheck();
						return;
					}

					DropMeteor(args);
					return;

				case "fullmoon":
				case "full moon":
					if (!args.Player.HasPermission(Permissions.fullmoon) && !args.Player.HasPermission(Permissions.managefullmoonevent))
					{
						FailedPermissionCheck();
						return;
					}
					Fullmoon(args);
					return;

				case "bloodmoon":
				case "blood moon":
					if (!args.Player.HasPermission(Permissions.bloodmoon) && !args.Player.HasPermission(Permissions.managebloodmoonevent))
					{
						FailedPermissionCheck();
						return;
					}
					Bloodmoon(args);
					return;

				case "eclipse":
					if (!args.Player.HasPermission(Permissions.eclipse) && !args.Player.HasPermission(Permissions.manageeclipseevent))
					{
						FailedPermissionCheck();
						return;
					}
					Eclipse(args);
					return;

				case "invade":
				case "invasion":
					if (!args.Player.HasPermission(Permissions.invade) && !args.Player.HasPermission(Permissions.manageinvasionevent))
					{
						FailedPermissionCheck();
						return;
					}
					Invade(args);
					return;

				case "sandstorm":
					if (!args.Player.HasPermission(Permissions.sandstorm) && !args.Player.HasPermission(Permissions.managesandstormevent))
					{
						FailedPermissionCheck();
						return;
					}
					Sandstorm(args);
					return;

				case "rain":
					if (!args.Player.HasPermission(Permissions.rain) && !args.Player.HasPermission(Permissions.managerainevent))
					{
						FailedPermissionCheck();
						return;
					}
					Rain(args);
					return;

				case "lanternsnight":
				case "lanterns":
					if (!args.Player.HasPermission(Permissions.managelanternsnightevent))
					{
						FailedPermissionCheck();
						return;
					}
					LanternsNight(args);
					return;

				default:
					args.Player.SendErrorMessage(GetString("Invalid event type. Valid event types: {0}.", String.Join(", ", _validEvents)));
					return;
			}
		}

		private static void DropMeteor(CommandArgs args)
		{
			WorldGen.spawnMeteor = false;
			WorldGen.dropMeteor();
			if (args.Silent)
			{
				args.Player.SendInfoMessage(GetString("A meteor has been triggered."));
			}
			else
			{
				TSPlayer.All.SendInfoMessage(GetString("{0} triggered a meteor.", args.Player.Name));
			}
		}

		private static void Fullmoon(CommandArgs args)
		{
			TSPlayer.Server.SetFullMoon();
			if (args.Silent)
			{
				args.Player.SendInfoMessage(GetString("Started a full moon event."));
			}
			else
			{
				TSPlayer.All.SendInfoMessage(GetString("{0} started a full moon event.", args.Player.Name));
			}
		}

		private static void Bloodmoon(CommandArgs args)
		{
			TSPlayer.Server.SetBloodMoon(!Main.bloodMoon);
			if (args.Silent)
			{
				if (Main.bloodMoon)
				{
					args.Player.SendInfoMessage(GetString("Started a blood moon event."));
				}
				else
				{
					args.Player.SendInfoMessage(GetString("Stopped the current blood moon event."));
				}
			}
			else
			{
				if (Main.bloodMoon)
				{
					TSPlayer.All.SendInfoMessage(GetString("{0} started a blood moon event.", args.Player.Name));
				}
				else
				{
					TSPlayer.All.SendInfoMessage(GetString("{0} stopped the current blood moon.", args.Player.Name));
				}
			}
		}

		private static void Eclipse(CommandArgs args)
		{
			TSPlayer.Server.SetEclipse(!Main.eclipse);
			if (args.Silent)
			{
				if (Main.eclipse)
				{
					args.Player.SendInfoMessage(GetString("Started an eclipse."));
				}
				else
				{
					args.Player.SendInfoMessage(GetString("Stopped an eclipse."));
				}
			}
			else
			{
				if (Main.eclipse)
				{
					TSPlayer.All.SendInfoMessage(GetString("{0} started an eclipse.", args.Player.Name));
				}
				else
				{
					TSPlayer.All.SendInfoMessage(GetString("{0} stopped an eclipse.", args.Player.Name));
				}
			}
		}

		private static void Invade(CommandArgs args)
		{
			if (Main.invasionSize <= 0)
			{
				if (args.Parameters.Count < 2)
				{
					args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax:  {0}worldevent invasion [invasion type] [invasion wave].", Specifier));
					args.Player.SendErrorMessage(GetString("Valid invasion types: {0}.", String.Join(", ", _validInvasions)));
					return;
				}

				int wave = 1;
				switch (args.Parameters[1].ToLowerInvariant())
				{
					case "goblin":
					case "goblins":
						TSPlayer.All.SendInfoMessage(GetString("{0} has started a goblin army invasion.", args.Player.Name));
						TShock.Utils.StartInvasion(1);
						break;

					case "snowman":
					case "snowmen":
						TSPlayer.All.SendInfoMessage(GetString("{0} has started a snow legion invasion.", args.Player.Name));
						TShock.Utils.StartInvasion(2);
						break;

					case "pirate":
					case "pirates":
						TSPlayer.All.SendInfoMessage(GetString("{0} has started a pirate invasion.", args.Player.Name));
						TShock.Utils.StartInvasion(3);
						break;

					case "pumpkin":
					case "pumpkinmoon":
						if (args.Parameters.Count > 2)
						{
							if (!int.TryParse(args.Parameters[2], out wave) || wave <= 0)
							{
								args.Player.SendErrorMessage(GetString("Invalid pumpkin moon event wave."));
								break;
							}
						}

						TSPlayer.Server.SetPumpkinMoon(true);
						Main.bloodMoon = false;
						NPC.waveKills = 0f;
						NPC.waveNumber = wave;
						TSPlayer.All.SendInfoMessage(GetString("{0} started the pumpkin moon at wave {1}!", args.Player.Name, wave));
						break;

					case "frost":
					case "frostmoon":
						if (args.Parameters.Count > 2)
						{
							if (!int.TryParse(args.Parameters[2], out wave) || wave <= 0)
							{
								args.Player.SendErrorMessage(GetString("Invalid frost moon event wave."));
								return;
							}
						}

						TSPlayer.Server.SetFrostMoon(true);
						Main.bloodMoon = false;
						NPC.waveKills = 0f;
						NPC.waveNumber = wave;
						TSPlayer.All.SendInfoMessage(GetString("{0} started the frost moon at wave {1}!", args.Player.Name, wave));
						break;

					case "martian":
					case "martians":
						TSPlayer.All.SendInfoMessage(GetString("{0} has started a martian invasion.", args.Player.Name));
						TShock.Utils.StartInvasion(4);
						break;

					default:
						args.Player.SendErrorMessage(GetString("Invalid invasion type. Valid invasion types: {0}.", String.Join(", ", _validInvasions)));
						break;
				}
			}
			else if (DD2Event.Ongoing)
			{
				DD2Event.StopInvasion();
				TSPlayer.All.SendInfoMessage(GetString("{0} has ended the Old One's Army event.", args.Player.Name));
			}
			else
			{
				TSPlayer.All.SendInfoMessage(GetString("{0} has ended the current invasion event.", args.Player.Name));
				Main.invasionSize = 0;
			}
		}

		private static void Sandstorm(CommandArgs args)
		{
			if (Terraria.GameContent.Events.Sandstorm.Happening)
			{
				Terraria.GameContent.Events.Sandstorm.StopSandstorm();
				TSPlayer.All.SendInfoMessage(GetString("{0} stopped the current sandstorm event.", args.Player.Name));
			}
			else
			{
				Terraria.GameContent.Events.Sandstorm.StartSandstorm();
				TSPlayer.All.SendInfoMessage(GetString("{0} started a sandstorm event.", args.Player.Name));
			}
		}

		private static void Rain(CommandArgs args)
		{
			bool slime = false;
			if (args.Parameters.Count > 1 && args.Parameters[1].ToLowerInvariant() == "slime")
			{
				slime = true;
			}

			if (!slime)
			{
				args.Player.SendInfoMessage(GetString("Use \"{0}worldevent rain slime\" to start slime rain!", Specifier));
			}

			if (slime && Main.raining) //Slime rain cannot be activated during normal rain
			{
				args.Player.SendErrorMessage(GetString("Slime rain cannot be activated during normal rain. Stop the normal rainstorm and try again."));
				return;
			}

			if (slime && Main.slimeRain) //Toggle slime rain off
			{
				Main.StopSlimeRain(false);
				TSPlayer.All.SendData(PacketTypes.WorldInfo);
				TSPlayer.All.SendInfoMessage(GetString("{0} ended the slime rain.", args.Player.Name));
				return;
			}

			if (slime && !Main.slimeRain) //Toggle slime rain on
			{
				Main.StartSlimeRain(false);
				TSPlayer.All.SendData(PacketTypes.WorldInfo);
				TSPlayer.All.SendInfoMessage(GetString("{0} caused it to rain slime.", args.Player.Name));
			}

			if (Main.raining && !slime) //Toggle rain off
			{
				Main.StopRain();
				TSPlayer.All.SendData(PacketTypes.WorldInfo);
				TSPlayer.All.SendInfoMessage(GetString("{0} ended the rain.", args.Player.Name));
				return;
			}

			if (!Main.raining && !slime) //Toggle rain on
			{
				Main.StartRain();
				TSPlayer.All.SendData(PacketTypes.WorldInfo);
				TSPlayer.All.SendInfoMessage(GetString("{0} caused it to rain.", args.Player.Name));
				return;
			}
		}

		private static void LanternsNight(CommandArgs args)
		{
			LanternNight.ToggleManualLanterns();
			if (args.Silent)
			{
				if (LanternNight.LanternsUp)
				{
					args.Player.SendSuccessMessage(GetString("Lanterns are now up."));
				}
				else
				{
					args.Player.SendSuccessMessage(GetString("Lanterns are now down."));
				}
			}
			else
			{
				if (LanternNight.LanternsUp)
				{
					TSPlayer.All.SendInfoMessage(GetString("{0} started a lantern night.", args.Player.Name));
				}
				else
				{
					TSPlayer.All.SendInfoMessage(GetString("{0} stopped the lantern night.", args.Player.Name));
				}
			}
		}

		private static void ClearAnglerQuests(CommandArgs args)
		{
			if (args.Parameters.Count > 0)
			{
				var result = Main.anglerWhoFinishedToday.RemoveAll(s => s.ToLower().Equals(args.Parameters[0].ToLower()));
				if (result > 0)
				{
					foreach (TSPlayer ply in TShock.Players.Where(p => p != null && p.Active && p.TPlayer.name.ToLower().Equals(args.Parameters[0].ToLower())))
					{
						//this will always tell the client that they have not done the quest today.
						ply.SendData(PacketTypes.AnglerQuest, "");
					}
					args.Player.SendSuccessMessage(GetString("Removed {0} players from the angler quest completion list for today.", result));
				}
				else
					args.Player.SendErrorMessage(GetString("Failed to find any users by that name on the list."));

			}
			else
			{
				Main.anglerWhoFinishedToday.Clear();
				NetMessage.SendAnglerQuest(-1);
				args.Player.SendSuccessMessage(GetString("Cleared all users from the angler quest completion list for today."));
			}
		}

		static Dictionary<string, int> _worldModes = new Dictionary<string, int>
		{
			{ "normal",    0 },
			{ "expert",    1 },
			{ "master",    2 },
			{ "journey",   3 },
			{ "creative",  3 }
		};

		private static void ChangeWorldMode(CommandArgs args)
		{
			if (args.Parameters.Count < 1)
			{
				args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}worldmode <mode>.", Specifier));
				args.Player.SendErrorMessage(GetString("Valid world modes: {0}", String.Join(", ", _worldModes.Keys)));
				return;
			}

			int mode;

			if (int.TryParse(args.Parameters[0], out mode))
			{
				if (mode < 0 || mode > 3)
				{
					args.Player.SendErrorMessage(GetString("Invalid world mode. Valid world modes: {0}", String.Join(", ", _worldModes.Keys)));
					return;
				}
			}
			else if (_worldModes.ContainsKey(args.Parameters[0].ToLowerInvariant()))
			{
				mode = _worldModes[args.Parameters[0].ToLowerInvariant()];
			}
			else
			{
				args.Player.SendErrorMessage(GetString("Invalid mode world mode. Valid modes: {0}", String.Join(", ", _worldModes.Keys)));
				return;
			}

			Main.GameMode = mode;
			args.Player.SendSuccessMessage(GetString("World mode set to {0}.", _worldModes.Keys.ElementAt(mode)));
			TSPlayer.All.SendData(PacketTypes.WorldInfo);
		}

		private static void Hardmode(CommandArgs args)
		{
			if (Main.hardMode)
			{
				Main.hardMode = false;
				TSPlayer.All.SendData(PacketTypes.WorldInfo);
				args.Player.SendSuccessMessage(GetString("Hardmode is now off."));
			}
			else if (!TShock.Config.Settings.DisableHardmode)
			{
				WorldGen.StartHardmode();
				args.Player.SendSuccessMessage(GetString("Hardmode is now on."));
			}
			else
			{
				args.Player.SendErrorMessage(GetString("Hardmode is disabled in the server configuration file."));
			}
		}

		private static void SpawnBoss(CommandArgs args)
		{
			if (args.Parameters.Count < 1 || args.Parameters.Count > 2)
			{
				args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}spawnboss <boss type> [amount].", Specifier));
				return;
			}

			int amount = 1;
			if (args.Parameters.Count == 2 && (!int.TryParse(args.Parameters[1], out amount) || amount <= 0))
			{
				args.Player.SendErrorMessage(GetString("Invalid boss amount."));
				return;
			}


			string spawnName;
			NPC npc = new NPC();
			switch (args.Parameters[0].ToLower())
			{
				case "*":
				case "all":
					int[] npcIds = { 4, 13, 35, 50, 125, 126, 127, 134, 222, 245, 262, 266, 370, 398, 439, 636, 657 };
					TSPlayer.Server.SetTime(false, 0.0);
					foreach (int i in npcIds)
					{
						npc.SetDefaults(i);
						TSPlayer.Server.SpawnNPC(npc.type, npc.FullName, amount, args.Player.TileX, args.Player.TileY);
					}
					spawnName = GetString("all bosses");
					break;

				case "brain":
				case "brain of cthulhu":
				case "boc":
					npc.SetDefaults(266);
					TSPlayer.Server.SpawnNPC(npc.type, npc.FullName, amount, args.Player.TileX, args.Player.TileY);
					spawnName = GetString("the Brain of Cthulhu");
					break;

				case "destroyer":
					npc.SetDefaults(134);
					TSPlayer.Server.SetTime(false, 0.0);
					TSPlayer.Server.SpawnNPC(npc.type, npc.FullName, amount, args.Player.TileX, args.Player.TileY);
					spawnName = GetString("the Destroyer");
					break;
				case "duke":
				case "duke fishron":
				case "fishron":
					npc.SetDefaults(370);
					TSPlayer.Server.SpawnNPC(npc.type, npc.FullName, amount, args.Player.TileX, args.Player.TileY);
					spawnName = GetString("Duke Fishron");
					break;
				case "eater":
				case "eater of worlds":
				case "eow":
					npc.SetDefaults(13);
					TSPlayer.Server.SpawnNPC(npc.type, npc.FullName, amount, args.Player.TileX, args.Player.TileY);
					spawnName = GetString("the Eater of Worlds");
					break;
				case "eye":
				case "eye of cthulhu":
				case "eoc":
					npc.SetDefaults(4);
					TSPlayer.Server.SetTime(false, 0.0);
					TSPlayer.Server.SpawnNPC(npc.type, npc.FullName, amount, args.Player.TileX, args.Player.TileY);
					spawnName = GetString("the Eye of Cthulhu");
					break;
				case "golem":
					npc.SetDefaults(245);
					TSPlayer.Server.SpawnNPC(npc.type, npc.FullName, amount, args.Player.TileX, args.Player.TileY);
					spawnName = GetString("the Golem");
					break;
				case "king":
				case "king slime":
				case "ks":
					npc.SetDefaults(50);
					TSPlayer.Server.SpawnNPC(npc.type, npc.FullName, amount, args.Player.TileX, args.Player.TileY);
					spawnName = GetString("the King Slime");
					break;
				case "plantera":
					npc.SetDefaults(262);
					TSPlayer.Server.SpawnNPC(npc.type, npc.FullName, amount, args.Player.TileX, args.Player.TileY);
					spawnName = GetString("Plantera");
					break;
				case "prime":
				case "skeletron prime":
					npc.SetDefaults(127);
					TSPlayer.Server.SetTime(false, 0.0);
					TSPlayer.Server.SpawnNPC(npc.type, npc.FullName, amount, args.Player.TileX, args.Player.TileY);
					spawnName = GetString("Skeletron Prime");
					break;
				case "queen bee":
				case "qb":
					npc.SetDefaults(222);
					TSPlayer.Server.SpawnNPC(npc.type, npc.FullName, amount, args.Player.TileX, args.Player.TileY);
					spawnName = GetString("the Queen Bee");
					break;
				case "skeletron":
					npc.SetDefaults(35);
					TSPlayer.Server.SetTime(false, 0.0);
					TSPlayer.Server.SpawnNPC(npc.type, npc.FullName, amount, args.Player.TileX, args.Player.TileY);
					spawnName = GetString("Skeletron");
					break;
				case "twins":
					TSPlayer.Server.SetTime(false, 0.0);
					npc.SetDefaults(125);
					TSPlayer.Server.SpawnNPC(npc.type, npc.FullName, amount, args.Player.TileX, args.Player.TileY);
					npc.SetDefaults(126);
					TSPlayer.Server.SpawnNPC(npc.type, npc.FullName, amount, args.Player.TileX, args.Player.TileY);
					spawnName = GetString("the Twins");
					break;
				case "wof":
				case "wall of flesh":
					if (Main.wofNPCIndex != -1)
					{
						args.Player.SendErrorMessage(GetString("There is already a Wall of Flesh."));
						return;
					}
					if (args.Player.Y / 16f < Main.maxTilesY - 205)
					{
						args.Player.SendErrorMessage(GetString("You must spawn the Wall of Flesh in hell."));
						return;
					}
					NPC.SpawnWOF(new Vector2(args.Player.X, args.Player.Y));
					spawnName = GetString("the Wall of Flesh");
					break;
				case "moon":
				case "moon lord":
				case "ml":
					npc.SetDefaults(398);
					TSPlayer.Server.SpawnNPC(npc.type, npc.FullName, amount, args.Player.TileX, args.Player.TileY);
					spawnName = GetString("the Moon Lord");
					break;
				case "empress":
				case "empress of light":
				case "eol":
					npc.SetDefaults(636);
					TSPlayer.Server.SpawnNPC(npc.type, npc.FullName, amount, args.Player.TileX, args.Player.TileY);
					spawnName = GetString("the Empress of Light");
					break;
				case "queen slime":
				case "qs":
					npc.SetDefaults(657);
					TSPlayer.Server.SpawnNPC(npc.type, npc.FullName, amount, args.Player.TileX, args.Player.TileY);
					spawnName = GetString("the Queen Slime");
					break;
				case "lunatic":
				case "lunatic cultist":
				case "cultist":
				case "lc":
					npc.SetDefaults(439);
					TSPlayer.Server.SpawnNPC(npc.type, npc.FullName, amount, args.Player.TileX, args.Player.TileY);
					spawnName = GetString("the Lunatic Cultist");
					break;
				case "betsy":
					npc.SetDefaults(551);
					TSPlayer.Server.SpawnNPC(npc.type, npc.FullName, amount, args.Player.TileX, args.Player.TileY);
					spawnName = GetString("Betsy");
					break;
				case "flying dutchman":
				case "flying":
				case "dutchman":
					npc.SetDefaults(491);
					TSPlayer.Server.SpawnNPC(npc.type, npc.FullName, amount, args.Player.TileX, args.Player.TileY);
					spawnName = GetString("the Flying Dutchman");
					break;
				case "mourning wood":
					npc.SetDefaults(325);
					TSPlayer.Server.SpawnNPC(npc.type, npc.FullName, amount, args.Player.TileX, args.Player.TileY);
					spawnName = GetString("Mourning Wood");
					break;
				case "pumpking":
					npc.SetDefaults(327);
					TSPlayer.Server.SpawnNPC(npc.type, npc.FullName, amount, args.Player.TileX, args.Player.TileY);
					spawnName = GetString("the Pumpking");
					break;
				case "everscream":
					npc.SetDefaults(344);
					TSPlayer.Server.SpawnNPC(npc.type, npc.FullName, amount, args.Player.TileX, args.Player.TileY);
					spawnName = GetString("Everscream");
					break;
				case "santa-nk1":
				case "santa":
					npc.SetDefaults(346);
					TSPlayer.Server.SpawnNPC(npc.type, npc.FullName, amount, args.Player.TileX, args.Player.TileY);
					spawnName = GetString("Santa-NK1");
					break;
				case "ice queen":
					npc.SetDefaults(345);
					TSPlayer.Server.SpawnNPC(npc.type, npc.FullName, amount, args.Player.TileX, args.Player.TileY);
					spawnName = GetString("the Ice Queen");
					break;
				case "martian saucer":
					npc.SetDefaults(392);
					TSPlayer.Server.SpawnNPC(npc.type, npc.FullName, amount, args.Player.TileX, args.Player.TileY);
					spawnName = GetString("a Martian Saucer");
					break;
				case "solar pillar":
					npc.SetDefaults(517);
					TSPlayer.Server.SpawnNPC(npc.type, npc.FullName, amount, args.Player.TileX, args.Player.TileY);
					spawnName = GetString("a Solar Pillar");
					break;
				case "nebula pillar":
					npc.SetDefaults(507);
					TSPlayer.Server.SpawnNPC(npc.type, npc.FullName, amount, args.Player.TileX, args.Player.TileY);
					spawnName = GetString("a Nebula Pillar");
					break;
				case "vortex pillar":
					npc.SetDefaults(422);
					TSPlayer.Server.SpawnNPC(npc.type, npc.FullName, amount, args.Player.TileX, args.Player.TileY);
					spawnName = GetString("a Vortex Pillar");
					break;
				case "stardust pillar":
					npc.SetDefaults(493);
					TSPlayer.Server.SpawnNPC(npc.type, npc.FullName, amount, args.Player.TileX, args.Player.TileY);
					spawnName = GetString("a Stardust Pillar");
					break;
				case "deerclops":
					npc.SetDefaults(668);
					TSPlayer.Server.SpawnNPC(npc.type, npc.FullName, amount, args.Player.TileX, args.Player.TileY);
					spawnName = GetString("a Deerclops");
					break;
				default:
					args.Player.SendErrorMessage(GetString("Invalid boss type!"));
					return;
			}

			if (args.Silent)
			{
				args.Player.SendSuccessMessage(GetPluralString("You spawned {0} {1} time.", "You spawned {0} {1} times.", amount, spawnName, amount));
			}
			else
			{
				TSPlayer.All.SendSuccessMessage(GetPluralString("{0} spawned {1} {2} time.", "{0} spawned {1} {2} times.", amount, args.Player.Name, spawnName, amount));
			}
		}

		private static void SpawnMob(CommandArgs args)
		{
			if (args.Parameters.Count < 1 || args.Parameters.Count > 2)
			{
				args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}spawnmob <mob type> [amount].", Specifier));
				return;
			}
			if (args.Parameters[0].Length == 0)
			{
				args.Player.SendErrorMessage(GetString("Invalid mob type."));
				return;
			}

			int amount = 1;
			if (args.Parameters.Count == 2 && !int.TryParse(args.Parameters[1], out amount))
			{
				args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}spawnmob <mob type> [amount].", Specifier));
				return;
			}

			amount = Math.Min(amount, Main.maxNPCs);

			var npcs = TShock.Utils.GetNPCByIdOrName(args.Parameters[0]);
			if (npcs.Count == 0)
			{
				args.Player.SendErrorMessage(GetString("Invalid mob type!"));
			}
			else if (npcs.Count > 1)
			{
				args.Player.SendMultipleMatchError(npcs.Select(n => $"{n.FullName}({n.type})"));
			}
			else
			{
				var npc = npcs[0];
				if (npc.type >= 1 && npc.type < Terraria.ID.NPCID.Count && npc.type != 113)
				{
					TSPlayer.Server.SpawnNPC(npc.netID, npc.FullName, amount, args.Player.TileX, args.Player.TileY, 50, 20);
					if (args.Silent)
					{
						args.Player.SendSuccessMessage(GetPluralString("Spawned {0} {1} time.", "Spawned {0} {1} times.", amount, npc.FullName, amount));
					}
					else
					{
						TSPlayer.All.SendSuccessMessage(GetPluralString("{0} has spawned {1} {2} time.", "{0} has spawned {1} {2} times.", amount, args.Player.Name, npc.FullName, amount));
					}
				}
				else if (npc.type == 113)
				{
					if (Main.wofNPCIndex != -1 || (args.Player.Y / 16f < (Main.maxTilesY - 205)))
					{
						args.Player.SendErrorMessage(GetString("Unable to spawn a Wall of Flesh based on its current state or your current location."));
						return;
					}
					NPC.SpawnWOF(new Vector2(args.Player.X, args.Player.Y));
					if (args.Silent)
					{
						args.Player.SendSuccessMessage(GetString("Spawned a Wall of Flesh."));
					}
					else
					{
						TSPlayer.All.SendSuccessMessage(GetString("{0} has spawned a Wall of Flesh.", args.Player.Name));
					}
				}
				else
				{
					args.Player.SendErrorMessage(GetString("Invalid mob type."));
				}
			}
		}

		#endregion Cause Events and Spawn Monsters Commands

		#region Teleport Commands

		private static void Home(CommandArgs args)
		{
			if (args.Player.Dead)
			{
				args.Player.SendErrorMessage(GetString("You are dead. Dead players can't go home."));
				return;
			}
			args.Player.Spawn(PlayerSpawnContext.RecallFromItem);
			args.Player.SendSuccessMessage(GetString("Teleported to your spawn point (home)."));
		}

		private static void Spawn(CommandArgs args)
		{
			if (args.Player.Teleport(Main.spawnTileX * 16, (Main.spawnTileY * 16) - 48))
				args.Player.SendSuccessMessage(GetString("Teleported to the map's spawn point."));
		}

		private static void TP(CommandArgs args)
		{
			if (args.Parameters.Count != 1 && args.Parameters.Count != 2)
			{
				if (args.Player.HasPermission(Permissions.tpothers))
					args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}tp <player> [player 2].", Specifier));
				else
					args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}tp <player>.", Specifier));
				return;
			}

			if (args.Parameters.Count == 1)
			{
				var players = TSPlayer.FindByNameOrID(args.Parameters[0]);
				if (players.Count == 0)
					args.Player.SendErrorMessage(GetString("Invalid destination player."));
				else if (players.Count > 1)
					args.Player.SendMultipleMatchError(players.Select(p => p.Name));
				else
				{
					var target = players[0];
					if (!target.TPAllow && !args.Player.HasPermission(Permissions.tpoverride))
					{
						args.Player.SendErrorMessage(GetString("{0} has disabled incoming teleports.", target.Name));
						return;
					}
					if (args.Player.Teleport(target.TPlayer.position.X, target.TPlayer.position.Y))
					{
						args.Player.SendSuccessMessage(GetString("Teleported to {0}.", target.Name));
						if (!args.Player.HasPermission(Permissions.tpsilent))
							target.SendInfoMessage(GetString("{0} teleported to you.", args.Player.Name));
					}
				}
			}
			else
			{
				if (!args.Player.HasPermission(Permissions.tpothers))
				{
					args.Player.SendErrorMessage(GetString("You do not have permission to teleport other players."));
					return;
				}

				var players1 = TSPlayer.FindByNameOrID(args.Parameters[0]);
				var players2 = TSPlayer.FindByNameOrID(args.Parameters[1]);

				if (players2.Count == 0)
					args.Player.SendErrorMessage(GetString("Invalid destination player."));
				else if (players2.Count > 1)
					args.Player.SendMultipleMatchError(players2.Select(p => p.Name));
				else if (players1.Count == 0)
				{
					if (args.Parameters[0] == "*")
					{
						if (!args.Player.HasPermission(Permissions.tpallothers))
						{
							args.Player.SendErrorMessage(GetString("You do not have permission to teleport all players."));
							return;
						}

						var target = players2[0];
						foreach (var source in TShock.Players.Where(p => p != null && p != args.Player))
						{
							if (!target.TPAllow && !args.Player.HasPermission(Permissions.tpoverride))
								continue;
							if (source.Teleport(target.TPlayer.position.X, target.TPlayer.position.Y))
							{
								if (args.Player != source)
								{
									if (args.Player.HasPermission(Permissions.tpsilent))
										source.SendSuccessMessage(GetString("You were teleported to {0}.", target.Name));
									else
										source.SendSuccessMessage(GetString("{0} teleported you to {1}.", args.Player.Name, target.Name));
								}
								if (args.Player != target)
								{
									if (args.Player.HasPermission(Permissions.tpsilent))
										target.SendInfoMessage(GetString("{0} was teleported to you.", source.Name));
									if (!args.Player.HasPermission(Permissions.tpsilent))
										target.SendInfoMessage(GetString("{0} teleported {1} to you.", args.Player.Name, source.Name));
								}
							}
						}
						args.Player.SendSuccessMessage(GetString("Teleported everyone to {0}.", target.Name));
					}
					else
						args.Player.SendErrorMessage(GetString("Invalid destination player."));
				}
				else if (players1.Count > 1)
					args.Player.SendMultipleMatchError(players1.Select(p => p.Name));
				else
				{
					var source = players1[0];
					if (!source.TPAllow && !args.Player.HasPermission(Permissions.tpoverride))
					{
						args.Player.SendErrorMessage(GetString("{0} has disabled incoming teleports.", source.Name));
						return;
					}
					var target = players2[0];
					if (!target.TPAllow && !args.Player.HasPermission(Permissions.tpoverride))
					{
						args.Player.SendErrorMessage(GetString("{0} has disabled incoming teleports.", target.Name));
						return;
					}
					args.Player.SendSuccessMessage(GetString("Teleported {0} to {1}.", source.Name, target.Name));
					if (source.Teleport(target.TPlayer.position.X, target.TPlayer.position.Y))
					{
						if (args.Player != source)
						{
							if (args.Player.HasPermission(Permissions.tpsilent))
								source.SendSuccessMessage(GetString("You were teleported to {0}.", target.Name));
							else
								source.SendSuccessMessage(GetString("{0} teleported you to {1}.", args.Player.Name, target.Name));
						}
						if (args.Player != target)
						{
							if (args.Player.HasPermission(Permissions.tpsilent))
								target.SendInfoMessage(GetString("{0} was teleported to you.", source.Name));
							if (!args.Player.HasPermission(Permissions.tpsilent))
								target.SendInfoMessage(GetString("{0} teleported {1} to you.", args.Player.Name, source.Name));
						}
					}
				}
			}
		}

		private static void TPHere(CommandArgs args)
		{
			if (args.Parameters.Count < 1)
			{
				if (args.Player.HasPermission(Permissions.tpallothers))
					args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}tphere <player|*>.", Specifier));
				else
					args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}tphere <player>.", Specifier));
				return;
			}

			string playerName = String.Join(" ", args.Parameters);
			var players = TSPlayer.FindByNameOrID(playerName);
			if (players.Count == 0)
			{
				if (playerName == "*")
				{
					if (!args.Player.HasPermission(Permissions.tpallothers))
					{
						args.Player.SendErrorMessage(GetString("You do not have permission to teleport all other players."));
						return;
					}
					foreach (var player in TShock.Players)
					{
						if (player != null && player.Active && player.Index != args.Player.Index)
						{
							if (player.Teleport(args.TPlayer.position.X, args.TPlayer.position.Y))
								player.SendSuccessMessage(GetString("You were teleported to {0}.", args.Player.Name));
						}
					}
					args.Player.SendSuccessMessage(GetString("Teleported everyone to yourself."));
				}
				else
					args.Player.SendErrorMessage(GetString("Invalid destination player."));
			}
			else if (players.Count > 1)
				args.Player.SendMultipleMatchError(players.Select(p => p.Name));
			else
			{
				var plr = players[0];
				if (plr.Teleport(args.TPlayer.position.X, args.TPlayer.position.Y))
				{
					plr.SendInfoMessage(GetString("You were teleported to {0}.", args.Player.Name));
					args.Player.SendSuccessMessage(GetString("Teleported {0} to yourself.", plr.Name));
				}
			}
		}

		private static void TPNpc(CommandArgs args)
		{
			if (args.Parameters.Count < 1)
			{
				args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}tpnpc <NPC>.", Specifier));
				return;
			}

			var npcStr = string.Join(" ", args.Parameters);
			var matches = new List<NPC>();
			foreach (var npc in Main.npc.Where(npc => npc.active))
			{
				var englishName = EnglishLanguage.GetNpcNameById(npc.netID);

				if (string.Equals(npc.FullName, npcStr, StringComparison.InvariantCultureIgnoreCase) ||
					string.Equals(englishName, npcStr, StringComparison.InvariantCultureIgnoreCase))
				{
					matches = new List<NPC> { npc };
					break;
				}
				if (npc.FullName.ToLowerInvariant().StartsWith(npcStr.ToLowerInvariant()) ||
					englishName?.StartsWith(npcStr, StringComparison.InvariantCultureIgnoreCase) == true)
					matches.Add(npc);
			}

			if (matches.Count > 1)
			{
				args.Player.SendMultipleMatchError(matches.Select(n => $"{n.FullName}({n.whoAmI})"));
				return;
			}
			if (matches.Count == 0)
			{
				args.Player.SendErrorMessage(GetString("Invalid destination NPC."));
				return;
			}

			var target = matches[0];
			args.Player.Teleport(target.position.X, target.position.Y);
			args.Player.SendSuccessMessage(GetString("Teleported to the '{0}'.", target.FullName));
		}

		private static void GetPos(CommandArgs args)
		{
			var player = args.Player.Name;
			if (args.Parameters.Count > 0)
			{
				player = String.Join(" ", args.Parameters);
			}

			var players = TSPlayer.FindByNameOrID(player);
			if (players.Count == 0)
			{
				args.Player.SendErrorMessage(GetString("Invalid target player."));
			}
			else if (players.Count > 1)
			{
				args.Player.SendMultipleMatchError(players.Select(p => p.Name));
			}
			else
			{
				args.Player.SendSuccessMessage(GetString("Location of {0} is ({1}, {2}).", players[0].Name, players[0].TileX, players[0].TileY));
			}
		}

		private static void TPPos(CommandArgs args)
		{
			if (args.Parameters.Count != 2)
			{
				args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}tppos <tile x> <tile y>.", Specifier));
				return;
			}

			int x, y;
			if (!int.TryParse(args.Parameters[0], out x) || !int.TryParse(args.Parameters[1], out y))
			{
				args.Player.SendErrorMessage(GetString("The destination coordinates provided don't look like valid numbers."));
				return;
			}
			x = Math.Max(0, x);
			y = Math.Max(0, y);
			x = Math.Min(x, Main.maxTilesX - 1);
			y = Math.Min(y, Main.maxTilesY - 1);

			args.Player.Teleport(16 * x, 16 * y);
			args.Player.SendSuccessMessage(GetString("Teleported to {0}, {1}.", x, y));
		}

		private static void TPAllow(CommandArgs args)
		{
			if (!args.Player.TPAllow)
				args.Player.SendSuccessMessage(GetString("Incoming teleports are now allowed."));
			if (args.Player.TPAllow)
				args.Player.SendSuccessMessage(GetString("Incoming teleports are now disabled."));
			args.Player.TPAllow = !args.Player.TPAllow;
		}

		private static void Warp(CommandArgs args)
		{
			bool hasManageWarpPermission = args.Player.HasPermission(Permissions.managewarp);
			if (args.Parameters.Count < 1)
			{
				if (hasManageWarpPermission)
				{
					args.Player.SendInfoMessage(GetString("Invalid syntax. Proper syntax: {0}warp [command] [arguments].", Specifier));
					args.Player.SendInfoMessage(GetString("Commands: add, del, hide, list, send, [warpname]."));
					args.Player.SendInfoMessage(GetString("Arguments: add [warp name], del [warp name], list [page]."));
					args.Player.SendInfoMessage(GetString("Arguments: send [player] [warp name], hide [warp name] [Enable(true/false)]."));
					args.Player.SendInfoMessage(GetString("Examples: {0}warp add foobar, {0}warp hide foobar true, {0}warp foobar.", Specifier));
					return;
				}
				else
				{
					args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}warp [name] or {0}warp list <page>.", Specifier));
					return;
				}
			}

			if (args.Parameters[0].Equals("list"))
			{
				#region List warps
				int pageNumber;
				if (!PaginationTools.TryParsePageNumber(args.Parameters, 1, args.Player, out pageNumber))
					return;
				IEnumerable<string> warpNames = from warp in TShock.Warps.Warps
												where !warp.IsPrivate
												select warp.Name;
				PaginationTools.SendPage(args.Player, pageNumber, PaginationTools.BuildLinesFromTerms(warpNames),
					new PaginationTools.Settings
					{
						HeaderFormat = GetString("Warps ({{0}}/{{1}}):"),
						FooterFormat = GetString("Type {0}warp list {{0}} for more.", Specifier),
						NothingToDisplayString = GetString("There are currently no warps defined.")
					});
				#endregion
			}
			else if (args.Parameters[0].ToLower() == "add" && hasManageWarpPermission)
			{
				#region Add warp
				if (args.Parameters.Count == 2)
				{
					string warpName = args.Parameters[1];
					if (warpName == "list" || warpName == "hide" || warpName == "del" || warpName == "add")
					{
						args.Player.SendErrorMessage(GetString("Invalid warp name. The names 'list', 'hide', 'del' and 'add' are reserved for commands."));
					}
					else if (TShock.Warps.Add(args.Player.TileX, args.Player.TileY, warpName))
					{
						args.Player.SendSuccessMessage(GetString($"Warp added: {warpName}."));
					}
					else
					{
						args.Player.SendErrorMessage(GetString($"Warp {warpName} already exists."));
					}
				}
				else
					args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}warp add [name].", Specifier));
				#endregion
			}
			else if (args.Parameters[0].ToLower() == "del" && hasManageWarpPermission)
			{
				#region Del warp
				if (args.Parameters.Count == 2)
				{
					string warpName = args.Parameters[1];
					if (TShock.Warps.Remove(warpName))
					{
						args.Player.SendSuccessMessage(GetString($"Warp deleted: {warpName}"));
					}
					else
						args.Player.SendErrorMessage(GetString($"Could not find a warp named {warpName} to remove."));
				}
				else
					args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}warp del [name].", Specifier));
				#endregion
			}
			else if (args.Parameters[0].ToLower() == "hide" && hasManageWarpPermission)
			{
				#region Hide warp
				if (args.Parameters.Count == 3)
				{
					string warpName = args.Parameters[1];
					bool state = false;
					if (Boolean.TryParse(args.Parameters[2], out state))
					{
						if (TShock.Warps.Hide(args.Parameters[1], state))
						{
							if (state)
								args.Player.SendSuccessMessage(GetString("Warp {0} is now private.", warpName));
							else
								args.Player.SendSuccessMessage(GetString("Warp {0} is now public.", warpName));
						}
						else
							args.Player.SendErrorMessage(GetString("Could not find specified warp."));
					}
					else
						args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}warp hide [name] <true/false>.", Specifier));
				}
				else
					args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}warp hide [name] <true/false>.", Specifier));
				#endregion
			}
			else if (args.Parameters[0].ToLower() == "send" && args.Player.HasPermission(Permissions.tpothers))
			{
				#region Warp send
				if (args.Parameters.Count < 3)
				{
					args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}warp send [player] [warpname].", Specifier));
					return;
				}

				var foundplr = TSPlayer.FindByNameOrID(args.Parameters[1]);
				if (foundplr.Count == 0)
				{
					args.Player.SendErrorMessage(GetString("Invalid target player."));
					return;
				}
				else if (foundplr.Count > 1)
				{
					args.Player.SendMultipleMatchError(foundplr.Select(p => p.Name));
					return;
				}

				string warpName = args.Parameters[2];
				var warp = TShock.Warps.Find(warpName);
				var plr = foundplr[0];
				if (warp != null)
				{
					if (plr.Teleport(warp.Position.X * 16, warp.Position.Y * 16))
					{
						plr.SendSuccessMessage(GetString("{0} warped you to {1}.", args.Player.Name, warpName));
						args.Player.SendSuccessMessage(GetString("You warped {0} to {1}.", plr.Name, warpName));
					}
				}
				else
				{
					args.Player.SendErrorMessage(GetString($"The destination warp, {warpName}, was not found."));
				}
				#endregion
			}
			else
			{
				string warpName = String.Join(" ", args.Parameters);
				var warp = TShock.Warps.Find(warpName);
				if (warp != null)
				{
					if (args.Player.Teleport(warp.Position.X * 16, warp.Position.Y * 16))
						args.Player.SendSuccessMessage(GetString($"Warped to {warpName}."));
				}
				else
				{
					args.Player.SendErrorMessage(GetString($"The destination warp, {warpName}, was not found."));
				}
			}
		}

		#endregion Teleport Commands

		#region Group Management

		private static void Group(CommandArgs args)
		{
			string subCmd = args.Parameters.Count == 0 ? "help" : args.Parameters[0].ToLower();

			switch (subCmd)
			{
				case "add":
					#region Add group
					{
						if (args.Parameters.Count < 2)
						{
							args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}group add <group name> [permissions].", Specifier));
							return;
						}

						string groupName = args.Parameters[1];
						args.Parameters.RemoveRange(0, 2);
						string permissions = String.Join(",", args.Parameters);

						try
						{
							TShock.Groups.AddGroup(groupName, null, permissions, TShockAPI.Group.defaultChatColor);
							args.Player.SendSuccessMessage(GetString($"Group {groupName} was added successfully."));
						}
						catch (GroupExistsException)
						{
							args.Player.SendErrorMessage(GetString("A group with the same name already exists."));
						}
						catch (GroupManagerException ex)
						{
							args.Player.SendErrorMessage(ex.ToString());
						}
					}
					#endregion
					return;
				case "addperm":
					#region Add permissions
					{
						if (args.Parameters.Count < 3)
						{
							args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}group addperm <group name> <permissions...>.", Specifier));
							return;
						}

						string groupName = args.Parameters[1];
						args.Parameters.RemoveRange(0, 2);
						if (groupName == "*")
						{
							foreach (Group g in TShock.Groups)
							{
								TShock.Groups.AddPermissions(g.Name, args.Parameters);
							}
							args.Player.SendSuccessMessage(GetString("The permissions have been added to all of the groups in the system."));
							return;
						}
						try
						{
							string response = TShock.Groups.AddPermissions(groupName, args.Parameters);
							if (response.Length > 0)
							{
								args.Player.SendSuccessMessage(response);
							}
							return;
						}
						catch (GroupManagerException ex)
						{
							args.Player.SendErrorMessage(ex.ToString());
						}
					}
					#endregion
					return;
				case "help":
					#region Help
					{
						int pageNumber;
						if (!PaginationTools.TryParsePageNumber(args.Parameters, 1, args.Player, out pageNumber))
							return;

						var lines = new List<string>
						{
							GetString("add <name> <permissions...> - Adds a new group."),
							GetString("addperm <group> <permissions...> - Adds permissions to a group."),
							GetString("color <group> <rrr,ggg,bbb> - Changes a group's chat color."),
							GetString("rename <group> <new name> - Changes a group's name."),
							GetString("del <group> - Deletes a group."),
							GetString("delperm <group> <permissions...> - Removes permissions from a group."),
							GetString("list [page] - Lists groups."),
							GetString("listperm <group> [page] - Lists a group's permissions."),
							GetString("parent <group> <parent group> - Changes a group's parent group."),
							GetString("prefix <group> <prefix> - Changes a group's prefix."),
							GetString("suffix <group> <suffix> - Changes a group's suffix.")
						};

						PaginationTools.SendPage(args.Player, pageNumber, lines,
							new PaginationTools.Settings
							{
								HeaderFormat = GetString("Group Sub-Commands ({{0}}/{{1}}):"),
								FooterFormat = GetString("Type {0}group help {{0}} for more sub-commands.", Specifier)
							}
						);
					}
					#endregion
					return;
				case "parent":
					#region Parent
					{
						if (args.Parameters.Count < 2)
						{
							args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}group parent <group name> [new parent group name].", Specifier));
							return;
						}

						string groupName = args.Parameters[1];
						Group group = TShock.Groups.GetGroupByName(groupName);
						if (group == null)
						{
							args.Player.SendErrorMessage(GetString("No such group \"{0}\".", groupName));
							return;
						}

						if (args.Parameters.Count > 2)
						{
							string newParentGroupName = string.Join(" ", args.Parameters.Skip(2));
							if (!string.IsNullOrWhiteSpace(newParentGroupName) && !TShock.Groups.GroupExists(newParentGroupName))
							{
								args.Player.SendErrorMessage(GetString("No such group \"{0}\".", newParentGroupName));
								return;
							}

							try
							{
								TShock.Groups.UpdateGroup(groupName, newParentGroupName, group.Permissions, group.ChatColor, group.Suffix, group.Prefix);

								if (!string.IsNullOrWhiteSpace(newParentGroupName))
									args.Player.SendSuccessMessage(GetString("Parent of group \"{0}\" set to \"{1}\".", groupName, newParentGroupName));
								else
									args.Player.SendSuccessMessage(GetString("Removed parent of group \"{0}\".", groupName));
							}
							catch (GroupManagerException ex)
							{
								args.Player.SendErrorMessage(ex.Message);
							}
						}
						else
						{
							if (group.Parent != null)
								args.Player.SendSuccessMessage(GetString("Parent of \"{0}\" is \"{1}\".", group.Name, group.Parent.Name));
							else
								args.Player.SendSuccessMessage(GetString("Group \"{0}\" has no parent.", group.Name));
						}
					}
					#endregion
					return;
				case "suffix":
					#region Suffix
					{
						if (args.Parameters.Count < 2)
						{
							args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}group suffix <group name> [new suffix].", Specifier));
							return;
						}

						string groupName = args.Parameters[1];
						Group group = TShock.Groups.GetGroupByName(groupName);
						if (group == null)
						{
							args.Player.SendErrorMessage(GetString("No such group \"{0}\".", groupName));
							return;
						}

						if (args.Parameters.Count > 2)
						{
							string newSuffix = string.Join(" ", args.Parameters.Skip(2));

							try
							{
								TShock.Groups.UpdateGroup(groupName, group.ParentName, group.Permissions, group.ChatColor, newSuffix, group.Prefix);

								if (!string.IsNullOrWhiteSpace(newSuffix))
									args.Player.SendSuccessMessage(GetString("Suffix of group \"{0}\" set to \"{1}\".", groupName, newSuffix));
								else
									args.Player.SendSuccessMessage(GetString("Removed suffix of group \"{0}\".", groupName));
							}
							catch (GroupManagerException ex)
							{
								args.Player.SendErrorMessage(ex.Message);
							}
						}
						else
						{
							if (!string.IsNullOrWhiteSpace(group.Suffix))
								args.Player.SendSuccessMessage(GetString("Suffix of \"{0}\" is \"{1}\".", group.Name, group.Suffix));
							else
								args.Player.SendSuccessMessage(GetString("Group \"{0}\" has no suffix.", group.Name));
						}
					}
					#endregion
					return;
				case "prefix":
					#region Prefix
					{
						if (args.Parameters.Count < 2)
						{
							args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}group prefix <group name> [new prefix].", Specifier));
							return;
						}

						string groupName = args.Parameters[1];
						Group group = TShock.Groups.GetGroupByName(groupName);
						if (group == null)
						{
							args.Player.SendErrorMessage(GetString("No such group \"{0}\".", groupName));
							return;
						}

						if (args.Parameters.Count > 2)
						{
							string newPrefix = string.Join(" ", args.Parameters.Skip(2));

							try
							{
								TShock.Groups.UpdateGroup(groupName, group.ParentName, group.Permissions, group.ChatColor, group.Suffix, newPrefix);

								if (!string.IsNullOrWhiteSpace(newPrefix))
									args.Player.SendSuccessMessage(GetString("Prefix of group \"{0}\" set to \"{1}\".", groupName, newPrefix));
								else
									args.Player.SendSuccessMessage(GetString("Removed prefix of group \"{0}\".", groupName));
							}
							catch (GroupManagerException ex)
							{
								args.Player.SendErrorMessage(ex.Message);
							}
						}
						else
						{
							if (!string.IsNullOrWhiteSpace(group.Prefix))
								args.Player.SendSuccessMessage(GetString("Prefix of \"{0}\" is \"{1}\".", group.Name, group.Prefix));
							else
								args.Player.SendSuccessMessage(GetString("Group \"{0}\" has no prefix.", group.Name));
						}
					}
					#endregion
					return;
				case "color":
					#region Color
					{
						if (args.Parameters.Count < 2 || args.Parameters.Count > 3)
						{
							args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}group color <group name> [new color(000,000,000)].", Specifier));
							return;
						}

						string groupName = args.Parameters[1];
						Group group = TShock.Groups.GetGroupByName(groupName);
						if (group == null)
						{
							args.Player.SendErrorMessage(GetString("No such group \"{0}\".", groupName));
							return;
						}

						if (args.Parameters.Count == 3)
						{
							string newColor = args.Parameters[2];

							String[] parts = newColor.Split(',');
							byte r;
							byte g;
							byte b;
							if (parts.Length == 3 && byte.TryParse(parts[0], out r) && byte.TryParse(parts[1], out g) && byte.TryParse(parts[2], out b))
							{
								try
								{
									TShock.Groups.UpdateGroup(groupName, group.ParentName, group.Permissions, newColor, group.Suffix, group.Prefix);

									args.Player.SendSuccessMessage(GetString("Chat color for group \"{0}\" set to \"{1}\".", groupName, newColor));
								}
								catch (GroupManagerException ex)
								{
									args.Player.SendErrorMessage(ex.Message);
								}
							}
							else
							{
								args.Player.SendErrorMessage(GetString("Invalid syntax for color, expected \"rrr,ggg,bbb\"."));
							}
						}
						else
						{
							args.Player.SendSuccessMessage(GetString("Chat color for \"{0}\" is \"{1}\".", group.Name, group.ChatColor));
						}
					}
					#endregion
					return;
				case "rename":
					#region Rename group
					{
						if (args.Parameters.Count != 3)
						{
							args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}group rename <group> <new name>.", Specifier));
							return;
						}

						string group = args.Parameters[1];
						string newName = args.Parameters[2];
						try
						{
							string response = TShock.Groups.RenameGroup(group, newName);
							args.Player.SendSuccessMessage(response);
						}
						catch (GroupManagerException ex)
						{
							args.Player.SendErrorMessage(ex.Message);
						}
					}
					#endregion
					return;
				case "del":
					#region Delete group
					{
						if (args.Parameters.Count != 2)
						{
							args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}group del <group name>.", Specifier));
							return;
						}

						try
						{
							string response = TShock.Groups.DeleteGroup(args.Parameters[1], true);
							if (response.Length > 0)
							{
								args.Player.SendSuccessMessage(response);
							}
						}
						catch (GroupManagerException ex)
						{
							args.Player.SendErrorMessage(ex.Message);
						}
					}
					#endregion
					return;
				case "delperm":
					#region Delete permissions
					{
						if (args.Parameters.Count < 3)
						{
							args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}group delperm <group name> <permissions...>.", Specifier));
							return;
						}

						string groupName = args.Parameters[1];
						args.Parameters.RemoveRange(0, 2);
						if (groupName == "*")
						{
							foreach (Group g in TShock.Groups)
							{
								TShock.Groups.DeletePermissions(g.Name, args.Parameters);
							}
							args.Player.SendSuccessMessage(GetString("The permissions have been removed from all of the groups in the system."));
							return;
						}
						try
						{
							string response = TShock.Groups.DeletePermissions(groupName, args.Parameters);
							if (response.Length > 0)
							{
								args.Player.SendSuccessMessage(response);
							}
							return;
						}
						catch (GroupManagerException ex)
						{
							args.Player.SendErrorMessage(ex.Message);
						}
					}
					#endregion
					return;
				case "list":
					#region List groups
					{
						int pageNumber;
						if (!PaginationTools.TryParsePageNumber(args.Parameters, 1, args.Player, out pageNumber))
							return;
						var groupNames = from grp in TShock.Groups.groups
										 select grp.Name;
						PaginationTools.SendPage(args.Player, pageNumber, PaginationTools.BuildLinesFromTerms(groupNames),
							new PaginationTools.Settings
							{
								HeaderFormat = GetString("Groups ({{0}}/{{1}}):"),
								FooterFormat = GetString("Type {0}group list {{0}} for more.", Specifier)
							});
					}
					#endregion
					return;
				case "listperm":
					#region List permissions
					{
						if (args.Parameters.Count == 1)
						{
							args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}group listperm <group name> [page].", Specifier));
							return;
						}
						int pageNumber;
						if (!PaginationTools.TryParsePageNumber(args.Parameters, 2, args.Player, out pageNumber))
							return;

						if (!TShock.Groups.GroupExists(args.Parameters[1]))
						{
							args.Player.SendErrorMessage(GetString("Invalid group."));
							return;
						}
						Group grp = TShock.Groups.GetGroupByName(args.Parameters[1]);
						List<string> permissions = grp.TotalPermissions;

						PaginationTools.SendPage(args.Player, pageNumber, PaginationTools.BuildLinesFromTerms(permissions),
							new PaginationTools.Settings
							{
								HeaderFormat = GetString("Permissions for {0} ({{0}}/{{1}}):", grp.Name),
								FooterFormat = GetString("Type {0}group listperm {1} {{0}} for more.", Specifier, grp.Name),
								NothingToDisplayString = GetString($"There are currently no permissions for {grp.Name}.")
							});
					}
					#endregion
					return;
				default:
					args.Player.SendErrorMessage(GetString("Invalid subcommand! Type {0}group help for more information on valid commands.", Specifier));
					return;
			}
		}
		#endregion Group Management

		#region Item Management

		private static void ItemBan(CommandArgs args)
		{
			string subCmd = args.Parameters.Count == 0 ? "help" : args.Parameters[0].ToLower();
			switch (subCmd)
			{
				case "add":
					#region Add item
					{
						if (args.Parameters.Count != 2)
						{
							args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}itemban add <item name>.", Specifier));
							return;
						}

						List<Item> items = TShock.Utils.GetItemByIdOrName(args.Parameters[1]);
						if (items.Count == 0)
						{
							args.Player.SendErrorMessage(GetString("Invalid item."));
						}
						else if (items.Count > 1)
						{
							args.Player.SendMultipleMatchError(items.Select(i => $"{i.Name}({i.netID})"));
						}
						else
						{
							// Yes this is required because of localization
							// User may have passed in localized name but itembans works on English names
							string englishNameForStorage = EnglishLanguage.GetItemNameById(items[0].type);
							TShock.ItemBans.DataModel.AddNewBan(englishNameForStorage);

							// It was decided in Telegram that we would continue to ban
							// projectiles based on whether or not their associated item was
							// banned. However, it was also decided that we'd change the way
							// this worked: in particular, we'd make it so that the item ban
							// system just adds things to the projectile ban system at the
							// command layer instead of inferring the state of projectile
							// bans based on the state of the item ban system.

							if (items[0].type == ItemID.DirtRod)
							{
								TShock.ProjectileBans.AddNewBan(ProjectileID.DirtBall);
							}

							if (items[0].type == ItemID.Sandgun)
							{
								TShock.ProjectileBans.AddNewBan(ProjectileID.SandBallGun);
								TShock.ProjectileBans.AddNewBan(ProjectileID.EbonsandBallGun);
								TShock.ProjectileBans.AddNewBan(ProjectileID.PearlSandBallGun);
							}

							// This returns the localized name to the player, not the item as it was stored.
							args.Player.SendSuccessMessage(GetString($"Banned {items[0].Name}."));
						}
					}
					#endregion
					return;
				case "allow":
					#region Allow group to item
					{
						if (args.Parameters.Count != 3)
						{
							args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}itemban allow <item name> <group name>.", Specifier));
							return;
						}

						List<Item> items = TShock.Utils.GetItemByIdOrName(args.Parameters[1]);
						if (items.Count == 0)
						{
							args.Player.SendErrorMessage(GetString("Invalid item."));
						}
						else if (items.Count > 1)
						{
							args.Player.SendMultipleMatchError(items.Select(i => $"{i.Name}({i.netID})"));
						}
						else
						{
							if (!TShock.Groups.GroupExists(args.Parameters[2]))
							{
								args.Player.SendErrorMessage(GetString("Invalid group."));
								return;
							}

							ItemBan ban = TShock.ItemBans.DataModel.GetItemBanByName(EnglishLanguage.GetItemNameById(items[0].type));
							if (ban == null)
							{
								args.Player.SendErrorMessage(GetString("{0} is not banned.", items[0].Name));
								return;
							}
							if (!ban.AllowedGroups.Contains(args.Parameters[2]))
							{
								TShock.ItemBans.DataModel.AllowGroup(EnglishLanguage.GetItemNameById(items[0].type), args.Parameters[2]);
								args.Player.SendSuccessMessage(GetString("{0} has been allowed to use {1}.", args.Parameters[2], items[0].Name));
							}
							else
							{
								args.Player.SendWarningMessage(GetString("{0} is already allowed to use {1}.", args.Parameters[2], items[0].Name));
							}
						}
					}
					#endregion
					return;
				case "del":
					#region Delete item
					{
						if (args.Parameters.Count != 2)
						{
							args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}itemban del <item name>.", Specifier));
							return;
						}

						List<Item> items = TShock.Utils.GetItemByIdOrName(args.Parameters[1]);
						if (items.Count == 0)
						{
							args.Player.SendErrorMessage(GetString("Invalid item."));
						}
						else if (items.Count > 1)
						{
							args.Player.SendMultipleMatchError(items.Select(i => $"{i.Name}({i.netID})"));
						}
						else
						{
							TShock.ItemBans.DataModel.RemoveBan(EnglishLanguage.GetItemNameById(items[0].type));
							args.Player.SendSuccessMessage(GetString($"Unbanned {items[0].Name}."));
						}
					}
					#endregion
					return;
				case "disallow":
					#region Disllow group from item
					{
						if (args.Parameters.Count != 3)
						{
							args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}itemban disallow <item name> <group name>.", Specifier));
							return;
						}

						List<Item> items = TShock.Utils.GetItemByIdOrName(args.Parameters[1]);
						if (items.Count == 0)
						{
							args.Player.SendErrorMessage(GetString("Invalid item."));
						}
						else if (items.Count > 1)
						{
							args.Player.SendMultipleMatchError(items.Select(i => $"{i.Name}({i.netID})"));
						}
						else
						{
							if (!TShock.Groups.GroupExists(args.Parameters[2]))
							{
								args.Player.SendErrorMessage(GetString("Invalid group."));
								return;
							}

							ItemBan ban = TShock.ItemBans.DataModel.GetItemBanByName(EnglishLanguage.GetItemNameById(items[0].type));
							if (ban == null)
							{
								args.Player.SendErrorMessage(GetString("{0} is not banned.", items[0].Name));
								return;
							}
							if (ban.AllowedGroups.Contains(args.Parameters[2]))
							{
								TShock.ItemBans.DataModel.RemoveGroup(EnglishLanguage.GetItemNameById(items[0].type), args.Parameters[2]);
								args.Player.SendSuccessMessage(GetString("{0} has been disallowed to use {1}.", args.Parameters[2], items[0].Name));
							}
							else
							{
								args.Player.SendWarningMessage(GetString("{0} is already disallowed to use {1}.", args.Parameters[2], items[0].Name));
							}
						}
					}
					#endregion
					return;
				case "help":
					#region Help
					{
						int pageNumber;
						if (!PaginationTools.TryParsePageNumber(args.Parameters, 1, args.Player, out pageNumber))
							return;

						var lines = new List<string>
						{
							GetString("add <item> - Adds an item ban."),
							GetString("allow <item> <group> - Allows a group to use an item."),
							GetString("del <item> - Deletes an item ban."),
							GetString("disallow <item> <group> - Disallows a group from using an item."),
							GetString("list [page] - Lists all item bans.")
						};

						PaginationTools.SendPage(args.Player, pageNumber, lines,
							new PaginationTools.Settings
							{
								HeaderFormat = GetString("Item Ban Sub-Commands ({{0}}/{{1}}):"),
								FooterFormat = GetString("Type {0}itemban help {{0}} for more sub-commands.", Specifier)
							}
						);
					}
					#endregion
					return;
				case "list":
					#region List items
					{
						int pageNumber;
						if (!PaginationTools.TryParsePageNumber(args.Parameters, 1, args.Player, out pageNumber))
							return;
						IEnumerable<string> itemNames = from itemBan in TShock.ItemBans.DataModel.ItemBans
														select itemBan.Name;
						PaginationTools.SendPage(args.Player, pageNumber, PaginationTools.BuildLinesFromTerms(itemNames),
							new PaginationTools.Settings
							{
								HeaderFormat = GetString("Item bans ({{0}}/{{1}}):"),
								FooterFormat = GetString("Type {0}itemban list {{0}} for more.", Specifier),
								NothingToDisplayString = GetString("There are currently no banned items.")
							});
					}
					#endregion
					return;
				default:
					#region Default
					{
						args.Player.SendErrorMessage(GetString("Invalid subcommand. Type {0}itemban help for more information on valid subcommands.", Specifier));
					}
					#endregion
					return;

			}
		}
		#endregion Item Management

		#region Projectile Management

		private static void ProjectileBan(CommandArgs args)
		{
			string subCmd = args.Parameters.Count == 0 ? "help" : args.Parameters[0].ToLower();
			switch (subCmd)
			{
				case "add":
					#region Add projectile
					{
						if (args.Parameters.Count != 2)
						{
							args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}projban add <proj id>", Specifier));
							return;
						}
						short id;
						if (Int16.TryParse(args.Parameters[1], out id) && id > 0 && id < Terraria.ID.ProjectileID.Count)
						{
							TShock.ProjectileBans.AddNewBan(id);
							args.Player.SendSuccessMessage(GetString("Banned projectile {0}.", id));
						}
						else
							args.Player.SendErrorMessage(GetString("Invalid projectile ID!"));
					}
					#endregion
					return;
				case "allow":
					#region Allow group to projectile
					{
						if (args.Parameters.Count != 3)
						{
							args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}projban allow <id> <group>.", Specifier));
							return;
						}

						short id;
						if (Int16.TryParse(args.Parameters[1], out id) && id > 0 && id < Terraria.ID.ProjectileID.Count)
						{
							if (!TShock.Groups.GroupExists(args.Parameters[2]))
							{
								args.Player.SendErrorMessage(GetString("Invalid group."));
								return;
							}

							ProjectileBan ban = TShock.ProjectileBans.GetBanById(id);
							if (ban == null)
							{
								args.Player.SendErrorMessage(GetString("Projectile {0} is not banned.", id));
								return;
							}
							if (!ban.AllowedGroups.Contains(args.Parameters[2]))
							{
								TShock.ProjectileBans.AllowGroup(id, args.Parameters[2]);
								args.Player.SendSuccessMessage(GetString("{0} has been allowed to use projectile {1}.", args.Parameters[2], id));
							}
							else
								args.Player.SendWarningMessage(GetString("{0} is already allowed to use projectile {1}.", args.Parameters[2], id));
						}
						else
							args.Player.SendErrorMessage(GetString("Invalid projectile ID."));
					}
					#endregion
					return;
				case "del":
					#region Delete projectile
					{
						if (args.Parameters.Count != 2)
						{
							args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}projban del <id>.", Specifier));
							return;
						}

						short id;
						if (Int16.TryParse(args.Parameters[1], out id) && id > 0 && id < Terraria.ID.ProjectileID.Count)
						{
							TShock.ProjectileBans.RemoveBan(id);
							args.Player.SendSuccessMessage(GetString("Unbanned projectile {0}.", id));
							return;
						}
						else
							args.Player.SendErrorMessage(GetString("Invalid projectile ID."));
					}
					#endregion
					return;
				case "disallow":
					#region Disallow group from projectile
					{
						if (args.Parameters.Count != 3)
						{
							args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}projban disallow <id> <group name>.", Specifier));
							return;
						}

						short id;
						if (Int16.TryParse(args.Parameters[1], out id) && id > 0 && id < Terraria.ID.ProjectileID.Count)
						{
							if (!TShock.Groups.GroupExists(args.Parameters[2]))
							{
								args.Player.SendErrorMessage(GetString("Invalid group."));
								return;
							}

							ProjectileBan ban = TShock.ProjectileBans.GetBanById(id);
							if (ban == null)
							{
								args.Player.SendErrorMessage(GetString("Projectile {0} is not banned.", id));
								return;
							}
							if (ban.AllowedGroups.Contains(args.Parameters[2]))
							{
								TShock.ProjectileBans.RemoveGroup(id, args.Parameters[2]);
								args.Player.SendSuccessMessage(GetString("{0} has been disallowed from using projectile {1}.", args.Parameters[2], id));
								return;
							}
							else
								args.Player.SendWarningMessage(GetString("{0} is already prevented from using projectile {1}.", args.Parameters[2], id));
						}
						else
							args.Player.SendErrorMessage(GetString("Invalid projectile ID."));
					}
					#endregion
					return;
				case "help":
					#region Help
					{
						int pageNumber;
						if (!PaginationTools.TryParsePageNumber(args.Parameters, 1, args.Player, out pageNumber))
							return;

						var lines = new List<string>
						{
							GetString("add <projectile ID> - Adds a projectile ban."),
							GetString("allow <projectile ID> <group> - Allows a group to use a projectile."),
							GetString("del <projectile ID> - Deletes an projectile ban."),
							GetString("disallow <projectile ID> <group> - Disallows a group from using a projectile."),
							GetString("list [page] - Lists all projectile bans.")
						};

						PaginationTools.SendPage(args.Player, pageNumber, lines,
							new PaginationTools.Settings
							{
								HeaderFormat = GetString("Projectile Ban Sub-Commands ({{0}}/{{1}}):"),
								FooterFormat = GetString("Type {0}projban help {{0}} for more sub-commands.", Specifier)
							}
						);
					}
					#endregion
					return;
				case "list":
					#region List projectiles
					{
						int pageNumber;
						if (!PaginationTools.TryParsePageNumber(args.Parameters, 1, args.Player, out pageNumber))
							return;
						IEnumerable<Int16> projectileIds = from projectileBan in TShock.ProjectileBans.ProjectileBans
														   select projectileBan.ID;
						PaginationTools.SendPage(args.Player, pageNumber, PaginationTools.BuildLinesFromTerms(projectileIds),
							new PaginationTools.Settings
							{
								HeaderFormat = GetString("Projectile bans ({{0}}/{{1}}):"),
								FooterFormat = GetString("Type {0}projban list {{0}} for more.", Specifier),
								NothingToDisplayString = GetString("There are currently no banned projectiles.")
							});
					}
					#endregion
					return;
				default:
					#region Default
					{
						args.Player.SendErrorMessage(GetString("Invalid subcommand. Type {0}projban help for more information on valid subcommands.", Specifier));
					}
					#endregion
					return;
			}
		}
		#endregion Projectile Management

		#region Tile Management
		private static void TileBan(CommandArgs args)
		{
			string subCmd = args.Parameters.Count == 0 ? "help" : args.Parameters[0].ToLower();
			switch (subCmd)
			{
				case "add":
					#region Add tile
					{
						if (args.Parameters.Count != 2)
						{
							args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}tileban add <tile id>.", Specifier));
							return;
						}
						short id;
						if (Int16.TryParse(args.Parameters[1], out id) && id >= 0 && id < Terraria.ID.TileID.Count)
						{
							TShock.TileBans.AddNewBan(id);
							args.Player.SendSuccessMessage(GetString("Banned tile {0}.", id));
						}
						else
							args.Player.SendErrorMessage(GetString("Invalid tile ID."));
					}
					#endregion
					return;
				case "allow":
					#region Allow group to place tile
					{
						if (args.Parameters.Count != 3)
						{
							args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}tileban allow <id> <group>.", Specifier));
							return;
						}

						short id;
						if (Int16.TryParse(args.Parameters[1], out id) && id >= 0 && id < Terraria.ID.TileID.Count)
						{
							if (!TShock.Groups.GroupExists(args.Parameters[2]))
							{
								args.Player.SendErrorMessage(GetString("Invalid group."));
								return;
							}

							TileBan ban = TShock.TileBans.GetBanById(id);
							if (ban == null)
							{
								args.Player.SendErrorMessage(GetString("Tile {0} is not banned.", id));
								return;
							}
							if (!ban.AllowedGroups.Contains(args.Parameters[2]))
							{
								TShock.TileBans.AllowGroup(id, args.Parameters[2]);
								args.Player.SendSuccessMessage(GetString("{0} has been allowed to place tile {1}.", args.Parameters[2], id));
							}
							else
								args.Player.SendWarningMessage(GetString("{0} is already allowed to place tile {1}.", args.Parameters[2], id));
						}
						else
							args.Player.SendErrorMessage(GetString("Invalid tile ID."));
					}
					#endregion
					return;
				case "del":
					#region Delete tile ban
					{
						if (args.Parameters.Count != 2)
						{
							args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}tileban del <id>.", Specifier));
							return;
						}

						short id;
						if (Int16.TryParse(args.Parameters[1], out id) && id >= 0 && id < Terraria.ID.TileID.Count)
						{
							TShock.TileBans.RemoveBan(id);
							args.Player.SendSuccessMessage(GetString("Unbanned tile {0}.", id));
							return;
						}
						else
							args.Player.SendErrorMessage(GetString("Invalid tile ID."));
					}
					#endregion
					return;
				case "disallow":
					#region Disallow group from placing tile
					{
						if (args.Parameters.Count != 3)
						{
							args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}tileban disallow <id> <group name>.", Specifier));
							return;
						}

						short id;
						if (Int16.TryParse(args.Parameters[1], out id) && id >= 0 && id < Terraria.ID.TileID.Count)
						{
							if (!TShock.Groups.GroupExists(args.Parameters[2]))
							{
								args.Player.SendErrorMessage(GetString("Invalid group."));
								return;
							}

							TileBan ban = TShock.TileBans.GetBanById(id);
							if (ban == null)
							{
								args.Player.SendErrorMessage(GetString("Tile {0} is not banned.", id));
								return;
							}
							if (ban.AllowedGroups.Contains(args.Parameters[2]))
							{
								TShock.TileBans.RemoveGroup(id, args.Parameters[2]);
								args.Player.SendSuccessMessage(GetString("{0} has been disallowed from placing tile {1}.", args.Parameters[2], id));
								return;
							}
							else
								args.Player.SendWarningMessage(GetString("{0} is already prevented from placing tile {1}.", args.Parameters[2], id));
						}
						else
							args.Player.SendErrorMessage(GetString("Invalid tile ID."));
					}
					#endregion
					return;
				case "help":
					#region Help
					{
						int pageNumber;
						if (!PaginationTools.TryParsePageNumber(args.Parameters, 1, args.Player, out pageNumber))
							return;

						var lines = new List<string>
						{
							GetString("add <tile ID> - Adds a tile ban."),
							GetString("allow <tile ID> <group> - Allows a group to place a tile."),
							GetString("del <tile ID> - Deletes a tile ban."),
							GetString("disallow <tile ID> <group> - Disallows a group from place a tile."),
							GetString("list [page] - Lists all tile bans.")
						};

						PaginationTools.SendPage(args.Player, pageNumber, lines,
							new PaginationTools.Settings
							{
								HeaderFormat = GetString("Tile Ban Sub-Commands ({{0}}/{{1}}):"),
								FooterFormat = GetString("Type {0}tileban help {{0}} for more sub-commands.", Specifier)
							}
						);
					}
					#endregion
					return;
				case "list":
					#region List tile bans
					{
						int pageNumber;
						if (!PaginationTools.TryParsePageNumber(args.Parameters, 1, args.Player, out pageNumber))
							return;
						IEnumerable<Int16> tileIds = from tileBan in TShock.TileBans.TileBans
													 select tileBan.ID;
						PaginationTools.SendPage(args.Player, pageNumber, PaginationTools.BuildLinesFromTerms(tileIds),
							new PaginationTools.Settings
							{
								HeaderFormat = GetString("Tile bans ({{0}}/{{1}}):"),
								FooterFormat = GetString("Type {0}tileban list {{0}} for more.", Specifier),
								NothingToDisplayString = GetString("There are currently no banned tiles.")
							});
					}
					#endregion
					return;
				default:
					#region Default
					{
						args.Player.SendErrorMessage(GetString("Invalid subcommand. Type {0}tileban help for more information on valid subcommands.", Specifier));
					}
					#endregion
					return;
			}
		}
		#endregion Tile Management

		#region Server Config Commands

		private static void SetSpawn(CommandArgs args)
		{
			Main.spawnTileX = args.Player.TileX + 1;
			Main.spawnTileY = args.Player.TileY + 3;
			SaveManager.Instance.SaveWorld(false);
			args.Player.SendSuccessMessage(GetString("Spawn has now been set at your location."));
		}

		private static void SetDungeon(CommandArgs args)
		{
			Main.dungeonX = args.Player.TileX + 1;
			Main.dungeonY = args.Player.TileY + 3;
			SaveManager.Instance.SaveWorld(false);
			args.Player.SendSuccessMessage(GetString("The dungeon's position has now been set at your location."));
		}

		private static void Reload(CommandArgs args)
		{
			TShock.Utils.Reload();
			Hooks.GeneralHooks.OnReloadEvent(args.Player);

			args.Player.SendSuccessMessage(
				GetString("Configuration, permissions, and regions reload complete. Some changes may require a server restart."));
		}

		private static void ServerPassword(CommandArgs args)
		{
			if (args.Parameters.Count != 1)
			{
				args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}serverpassword \"<new password>\".", Specifier));
				return;
			}
			string passwd = args.Parameters[0];
			TShock.Config.Settings.ServerPassword = passwd;
			args.Player.SendSuccessMessage(GetString("Server password has been changed to: {0}.", passwd));
		}

		private static void Save(CommandArgs args)
		{
			SaveManager.Instance.SaveWorld(false);
			foreach (TSPlayer tsply in TShock.Players.Where(tsply => tsply != null))
			{
				tsply.SaveServerCharacter();
			}
		}

		private static void Settle(CommandArgs args)
		{
			if (Liquid.panicMode)
			{
				args.Player.SendWarningMessage(GetString("Liquids are already settling."));
				return;
			}
			Liquid.StartPanic();
			args.Player.SendInfoMessage(GetString("Settling liquids."));
		}

		private static void MaxSpawns(CommandArgs args)
		{
			if (args.Parameters.Count == 0)
			{
				args.Player.SendInfoMessage(GetString("Current maximum spawns: {0}.", TShock.Config.Settings.DefaultMaximumSpawns));
				return;
			}

			if (String.Equals(args.Parameters[0], "default", StringComparison.CurrentCultureIgnoreCase))
			{
				TShock.Config.Settings.DefaultMaximumSpawns = NPC.defaultMaxSpawns = 5;
				if (args.Silent)
				{
					args.Player.SendInfoMessage(GetString("Changed the maximum spawns to 5."));
				}
				else
				{
					TSPlayer.All.SendInfoMessage(GetString("{0} changed the maximum spawns to 5.", args.Player.Name));
				}
				return;
			}

			int maxSpawns = -1;
			if (!int.TryParse(args.Parameters[0], out maxSpawns) || maxSpawns < 0 || maxSpawns > Main.maxNPCs)
			{
				args.Player.SendWarningMessage(GetString("Invalid maximum spawns.  Acceptable range is {0} to {1}.", 0, Main.maxNPCs));
				return;
			}

			TShock.Config.Settings.DefaultMaximumSpawns = NPC.defaultMaxSpawns = maxSpawns;
			if (args.Silent)
			{
				args.Player.SendInfoMessage(GetString("Changed the maximum spawns to {0}.", maxSpawns));
			}
			else
			{
				TSPlayer.All.SendInfoMessage(GetString("{0} changed the maximum spawns to {1}.", args.Player.Name, maxSpawns));
			}
		}

		private static void SpawnRate(CommandArgs args)
		{
			if (args.Parameters.Count == 0)
			{
				args.Player.SendInfoMessage(GetString("Current spawn rate: {0}.", TShock.Config.Settings.DefaultSpawnRate));
				return;
			}

			if (String.Equals(args.Parameters[0], "default", StringComparison.CurrentCultureIgnoreCase))
			{
				TShock.Config.Settings.DefaultSpawnRate = NPC.defaultSpawnRate = 600;
				if (args.Silent)
				{
					args.Player.SendInfoMessage(GetString("Changed the spawn rate to 600."));
				}
				else
				{
					TSPlayer.All.SendInfoMessage(GetString("{0} changed the spawn rate to 600.", args.Player.Name));
				}
				return;
			}

			int spawnRate = -1;
			if (!int.TryParse(args.Parameters[0], out spawnRate) || spawnRate < 0)
			{
				args.Player.SendWarningMessage(GetString("The spawn rate you provided is out-of-range or not a number."));
				return;
			}
			TShock.Config.Settings.DefaultSpawnRate = NPC.defaultSpawnRate = spawnRate;
			if (args.Silent)
			{
				args.Player.SendInfoMessage(GetString("Changed the spawn rate to {0}.", spawnRate));
			}
			else
			{
				TSPlayer.All.SendInfoMessage(GetString("{0} changed the spawn rate to {1}.", args.Player.Name, spawnRate));
			}
		}

		#endregion Server Config Commands

		#region Time/PvpFun Commands

		private static void Time(CommandArgs args)
		{
			if (args.Parameters.Count == 0)
			{
				double time = Main.time / 3600.0;
				time += 4.5;
				if (!Main.dayTime)
					time += 15.0;
				time = time % 24.0;
				args.Player.SendInfoMessage(GetString("The current time is {0}:{1:D2}.", (int)Math.Floor(time), (int)Math.Floor((time % 1.0) * 60.0)));
				return;
			}

			switch (args.Parameters[0].ToLower())
			{
				case "day":
					TSPlayer.Server.SetTime(true, 0.0);
					TSPlayer.All.SendInfoMessage(GetString("{0} set the time to 04:30.", args.Player.Name));
					break;
				case "night":
					TSPlayer.Server.SetTime(false, 0.0);
					TSPlayer.All.SendInfoMessage(GetString("{0} set the time to 19:30.", args.Player.Name));
					break;
				case "noon":
					TSPlayer.Server.SetTime(true, 27000.0);
					TSPlayer.All.SendInfoMessage(GetString("{0} set the time to 12:00.", args.Player.Name));
					break;
				case "midnight":
					TSPlayer.Server.SetTime(false, 16200.0);
					TSPlayer.All.SendInfoMessage(GetString("{0} set the time to 00:00.", args.Player.Name));
					break;
				default:
					string[] array = args.Parameters[0].Split(':');
					if (array.Length != 2)
					{
						args.Player.SendErrorMessage(GetString("Invalid time string. Proper format: hh:mm, in 24-hour time."));
						return;
					}

					int hours;
					int minutes;
					if (!int.TryParse(array[0], out hours) || hours < 0 || hours > 23
						|| !int.TryParse(array[1], out minutes) || minutes < 0 || minutes > 59)
					{
						args.Player.SendErrorMessage(GetString("Invalid time string. Proper format: hh:mm, in 24-hour time."));
						return;
					}

					decimal time = hours + (minutes / 60.0m);
					time -= 4.50m;
					if (time < 0.00m)
						time += 24.00m;

					if (time >= 15.00m)
					{
						TSPlayer.Server.SetTime(false, (double)((time - 15.00m) * 3600.0m));
					}
					else
					{
						TSPlayer.Server.SetTime(true, (double)(time * 3600.0m));
					}
					TSPlayer.All.SendInfoMessage(GetString("{0} set the time to {1}:{2:D2}.", args.Player.Name, hours, minutes));
					break;
			}
		}

		private static void Slap(CommandArgs args)
		{
			if (args.Parameters.Count < 1 || args.Parameters.Count > 2)
			{
				args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}slap <player> [damage].", Specifier));
				return;
			}

			if (args.Parameters[0].Length == 0)
			{
				args.Player.SendErrorMessage(GetString("Invalid target player."));
				return;
			}

			string plStr = args.Parameters[0];
			var players = TSPlayer.FindByNameOrID(plStr);
			if (players.Count == 0)
			{
				args.Player.SendErrorMessage(GetString("Invalid target player."));
			}
			else if (players.Count > 1)
			{
				args.Player.SendMultipleMatchError(players.Select(p => p.Name));
			}
			else
			{
				var plr = players[0];
				int damage = 5;
				if (args.Parameters.Count == 2)
				{
					int.TryParse(args.Parameters[1], out damage);
				}
				if (!args.Player.HasPermission(Permissions.kill))
				{
					damage = TShock.Utils.Clamp(damage, 15, 0);
				}
				plr.DamagePlayer(damage);
				TSPlayer.All.SendInfoMessage(GetString("{0} slapped {1} for {2} damage.", args.Player.Name, plr.Name, damage));
				TShock.Log.Info(GetString("{0} slapped {1} for {2} damage.", args.Player.Name, plr.Name, damage));
			}
		}

		private static void Wind(CommandArgs args)
		{
			if (args.Parameters.Count != 1)
			{
				args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}wind <speed in mph>.", Specifier));
				return;
			}

			float mph;
			if (!float.TryParse(args.Parameters[0], out mph) || mph is < -40f or > 40f)
			{
				args.Player.SendErrorMessage(GetString("Invalid wind speed (must be between -40 and 40)."));
				return;
			}

			float speed = mph / 50f; // -40 to 40 mph -> -0.8 to 0.8
			Main.windSpeedCurrent = speed;
			Main.windSpeedTarget = speed;
			TSPlayer.All.SendData(PacketTypes.WorldInfo);
			TSPlayer.All.SendInfoMessage(GetString("{0} changed the wind speed to {1}mph.", args.Player.Name, mph));
		}

		#endregion Time/PvpFun Commands

		#region Region Commands

		private static void Region(CommandArgs args)
		{
			string cmd = "help";
			if (args.Parameters.Count > 0)
			{
				cmd = args.Parameters[0].ToLower();
			}
			switch (cmd)
			{
				case "name":
					{
						{
							args.Player.SendInfoMessage(GetString("Hit a block to get the name of the region."));
							args.Player.AwaitingName = true;
							args.Player.AwaitingNameParameters = args.Parameters.Skip(1).ToArray();
						}
						break;
					}
				case "set":
					{
						int choice = 0;
						if (args.Parameters.Count == 2 &&
							int.TryParse(args.Parameters[1], out choice) &&
							choice >= 1 && choice <= 2)
						{
							args.Player.SendInfoMessage(GetString("Hit a block to set point {0}.", choice));
							args.Player.AwaitingTempPoint = choice;
						}
						else
						{
							args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: /region set <1/2>."));
						}
						break;
					}
				case "define":
					{
						if (args.Parameters.Count > 1)
						{
							if (!args.Player.TempPoints.Any(p => p == Point.Zero))
							{
								string regionName = String.Join(" ", args.Parameters.GetRange(1, args.Parameters.Count - 1));
								var x = Math.Min(args.Player.TempPoints[0].X, args.Player.TempPoints[1].X);
								var y = Math.Min(args.Player.TempPoints[0].Y, args.Player.TempPoints[1].Y);
								var width = Math.Abs(args.Player.TempPoints[0].X - args.Player.TempPoints[1].X);
								var height = Math.Abs(args.Player.TempPoints[0].Y - args.Player.TempPoints[1].Y);

								if (TShock.Regions.AddRegion(x, y, width, height, regionName, args.Player.Account.Name,
															 Main.worldID.ToString()))
								{
									args.Player.TempPoints[0] = Point.Zero;
									args.Player.TempPoints[1] = Point.Zero;
									args.Player.SendInfoMessage(GetString("Set region {0}.", regionName));
								}
								else
								{
									args.Player.SendErrorMessage(GetString($"Region {regionName} already exists."));
								}
							}
							else
							{
								args.Player.SendErrorMessage(GetString("Region points need to be defined first. Use /region set 1 and /region set 2."));
							}
						}
						else
							args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}region define <name>.", Specifier));
						break;
					}
				case "protect":
					{
						if (args.Parameters.Count == 3)
						{
							string regionName = args.Parameters[1];
							if (args.Parameters[2].ToLower() == "true")
							{
								if (TShock.Regions.SetRegionState(regionName, true))
									args.Player.SendInfoMessage(GetString("Marked region {0} as protected.", regionName));
								else
									args.Player.SendErrorMessage(GetString($"Could not find the region {regionName}."));
							}
							else if (args.Parameters[2].ToLower() == "false")
							{
								if (TShock.Regions.SetRegionState(regionName, false))
									args.Player.SendInfoMessage(GetString("Marked region {0} as unprotected.", regionName));
								else
									args.Player.SendErrorMessage(GetString($"Could not find the region {regionName}."));
							}
							else
								args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}region protect <name> <true/false>.", Specifier));
						}
						else
							args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: /region protect <name> <true/false>.", Specifier));
						break;
					}
				case "delete":
					{
						if (args.Parameters.Count > 1)
						{
							string regionName = String.Join(" ", args.Parameters.GetRange(1, args.Parameters.Count - 1));
							if (TShock.Regions.DeleteRegion(regionName))
							{
								args.Player.SendInfoMessage(GetString("Deleted region \"{0}\".", regionName));
							}
							else
								args.Player.SendErrorMessage(GetString($"Could not find the region {regionName}."));
						}
						else
							args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}region delete <name>.", Specifier));
						break;
					}
				case "clear":
					{
						args.Player.TempPoints[0] = Point.Zero;
						args.Player.TempPoints[1] = Point.Zero;
						args.Player.SendInfoMessage(GetString("Temporary region set points have been removed."));
						args.Player.AwaitingTempPoint = 0;
						break;
					}
				case "allow":
					{
						if (args.Parameters.Count > 2)
						{
							string playerName = args.Parameters[1];
							string regionName = "";

							for (int i = 2; i < args.Parameters.Count; i++)
							{
								if (regionName == "")
								{
									regionName = args.Parameters[2];
								}
								else
								{
									regionName = regionName + " " + args.Parameters[i];
								}
							}
							if (TShock.UserAccounts.GetUserAccountByName(playerName) != null)
							{
								if (TShock.Regions.AddNewUser(regionName, playerName))
								{
									args.Player.SendInfoMessage(GetString($"Added user {playerName} to {regionName}."));
								}
								else
									args.Player.SendErrorMessage(GetString($"Could not find the region {regionName}."));
							}
							else
							{
								args.Player.SendErrorMessage(GetString($"Player {playerName} not found."));
							}
						}
						else
							args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}region allow <name> <region>.", Specifier));
						break;
					}
				case "remove":
					if (args.Parameters.Count > 2)
					{
						string playerName = args.Parameters[1];
						string regionName = "";

						for (int i = 2; i < args.Parameters.Count; i++)
						{
							if (regionName == "")
							{
								regionName = args.Parameters[2];
							}
							else
							{
								regionName = regionName + " " + args.Parameters[i];
							}
						}
						if (TShock.UserAccounts.GetUserAccountByName(playerName) != null)
						{
							if (TShock.Regions.RemoveUser(regionName, playerName))
							{
								args.Player.SendInfoMessage(GetString($"Removed user {playerName} from {regionName}."));
							}
							else
								args.Player.SendErrorMessage(GetString($"Could not find the region {regionName}."));
						}
						else
						{
							args.Player.SendErrorMessage(GetString($"Player {playerName} not found."));
						}
					}
					else
						args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}region remove <name> <region>.", Specifier));
					break;
				case "allowg":
					{
						if (args.Parameters.Count > 2)
						{
							string group = args.Parameters[1];
							string regionName = "";

							for (int i = 2; i < args.Parameters.Count; i++)
							{
								if (regionName == "")
								{
									regionName = args.Parameters[2];
								}
								else
								{
									regionName = regionName + " " + args.Parameters[i];
								}
							}
							if (TShock.Groups.GroupExists(group))
							{
								if (TShock.Regions.AllowGroup(regionName, group))
								{
									args.Player.SendInfoMessage(GetString($"Added group {group} to {regionName}."));
								}
								else
									args.Player.SendErrorMessage(GetString($"Could not find the region {regionName}."));
							}
							else
							{
								args.Player.SendErrorMessage(GetString($"Group {group} not found."));
							}
						}
						else
							args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}region allowg <group> <region>.", Specifier));
						break;
					}
				case "removeg":
					if (args.Parameters.Count > 2)
					{
						string group = args.Parameters[1];
						string regionName = "";

						for (int i = 2; i < args.Parameters.Count; i++)
						{
							if (regionName == "")
							{
								regionName = args.Parameters[2];
							}
							else
							{
								regionName = regionName + " " + args.Parameters[i];
							}
						}
						if (TShock.Groups.GroupExists(group))
						{
							if (TShock.Regions.RemoveGroup(regionName, group))
							{
								args.Player.SendInfoMessage(GetString("Removed group {0} from {1}", group, regionName));
							}
							else
								args.Player.SendErrorMessage(GetString($"Could not find the region {regionName}."));
						}
						else
						{
							args.Player.SendErrorMessage(GetString($"Group {group} not found."));
						}
					}
					else
						args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}region removeg <group> <region>.", Specifier));
					break;
				case "list":
					{
						int pageNumber;
						if (!PaginationTools.TryParsePageNumber(args.Parameters, 1, args.Player, out pageNumber))
							return;

						IEnumerable<string> regionNames = from region in TShock.Regions.Regions
														  where region.WorldID == Main.worldID.ToString()
														  select region.Name;
						PaginationTools.SendPage(args.Player, pageNumber, PaginationTools.BuildLinesFromTerms(regionNames),
							new PaginationTools.Settings
							{
								HeaderFormat = GetString("Regions ({{0}}/{{1}}):"),
								FooterFormat = GetString("Type {0}region list {{0}} for more.", Specifier),
								NothingToDisplayString = GetString("There are currently no regions defined.")
							});
						break;
					}
				case "info":
					{
						if (args.Parameters.Count == 1 || args.Parameters.Count > 4)
						{
							args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}region info <region> [-d] [page].", Specifier));
							break;
						}

						string regionName = args.Parameters[1];
						bool displayBoundaries = args.Parameters.Skip(2).Any(
							p => p.Equals("-d", StringComparison.InvariantCultureIgnoreCase)
						);

						Region region = TShock.Regions.GetRegionByName(regionName);
						if (region == null)
						{
							args.Player.SendErrorMessage(GetString($"Could not find the region {regionName}."));
							break;
						}

						int pageNumberIndex = displayBoundaries ? 3 : 2;
						int pageNumber;
						if (!PaginationTools.TryParsePageNumber(args.Parameters, pageNumberIndex, args.Player, out pageNumber))
							break;

						List<string> lines = new List<string>
						{
							GetString("X: {0}; Y: {1}; W: {2}; H: {3}, Z: {4}", region.Area.X, region.Area.Y, region.Area.Width, region.Area.Height, region.Z),
							GetString($"Region owner: {region.Owner}."),
							GetString($"Protected: {region.DisableBuild.ToString()}."),
						};

						if (region.AllowedIDs.Count > 0)
						{
							IEnumerable<string> sharedUsersSelector = region.AllowedIDs.Select(userId =>
							{
								UserAccount account = TShock.UserAccounts.GetUserAccountByID(userId);
								if (account != null)
									return account.Name;

								return string.Concat("{ID: ", userId, "}");
							});
							List<string> extraLines = PaginationTools.BuildLinesFromTerms(sharedUsersSelector.Distinct());
							extraLines[0] = GetString("Shared with: ") + extraLines[0];
							lines.AddRange(extraLines);
						}
						else
						{
							lines.Add(GetString("Region is not shared with any users."));
						}

						if (region.AllowedGroups.Count > 0)
						{
							List<string> extraLines = PaginationTools.BuildLinesFromTerms(region.AllowedGroups.Distinct());
							extraLines[0] = GetString("Shared with groups: ") + extraLines[0];
							lines.AddRange(extraLines);
						}
						else
						{
							lines.Add(GetString("Region is not shared with any groups."));
						}

						PaginationTools.SendPage(
							args.Player, pageNumber, lines, new PaginationTools.Settings
							{
								HeaderFormat = GetString("Information About Region \"{0}\" ({{0}}/{{1}}):", region.Name),
								FooterFormat = GetString("Type {0}region info {1} {{0}} for more information.", Specifier, regionName)
							}
						);

						if (displayBoundaries)
						{
							Rectangle regionArea = region.Area;
							foreach (Point boundaryPoint in Utils.Instance.EnumerateRegionBoundaries(regionArea))
							{
								// Preferring dotted lines as those should easily be distinguishable from actual wires.
								if ((boundaryPoint.X + boundaryPoint.Y & 1) == 0)
								{
									// Could be improved by sending raw tile data to the client instead but not really
									// worth the effort as chances are very low that overwriting the wire for a few
									// nanoseconds will cause much trouble.
									ITile tile = Main.tile[boundaryPoint.X, boundaryPoint.Y];
									bool oldWireState = tile.wire();
									tile.wire(true);

									try
									{
										args.Player.SendTileSquareCentered(boundaryPoint.X, boundaryPoint.Y, 1);
									}
									finally
									{
										tile.wire(oldWireState);
									}
								}
							}

							Timer boundaryHideTimer = null;
							boundaryHideTimer = new Timer((state) =>
							{
								foreach (Point boundaryPoint in Utils.Instance.EnumerateRegionBoundaries(regionArea))
									if ((boundaryPoint.X + boundaryPoint.Y & 1) == 0)
										args.Player.SendTileSquareCentered(boundaryPoint.X, boundaryPoint.Y, 1);

								Debug.Assert(boundaryHideTimer != null);
								boundaryHideTimer.Dispose();
							},
								null, 5000, Timeout.Infinite
							);
						}

						break;
					}
				case "z":
					{
						if (args.Parameters.Count == 3)
						{
							string regionName = args.Parameters[1];
							int z = 0;
							if (int.TryParse(args.Parameters[2], out z))
							{
								if (TShock.Regions.SetZ(regionName, z))
									args.Player.SendInfoMessage(GetString("Region's z is now {0}", z));
								else
									args.Player.SendErrorMessage(GetString("Could not find specified region"));
							}
							else
								args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}region z <name> <#>", Specifier));
						}
						else
							args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}region z <name> <#>", Specifier));
						break;
					}
				case "resize":
				case "expand":
					{
						if (args.Parameters.Count == 4)
						{
							int direction;
							switch (args.Parameters[2])
							{
								case "u":
								case "up":
									{
										direction = 0;
										break;
									}
								case "r":
								case "right":
									{
										direction = 1;
										break;
									}
								case "d":
								case "down":
									{
										direction = 2;
										break;
									}
								case "l":
								case "left":
									{
										direction = 3;
										break;
									}
								default:
									{
										direction = -1;
										break;
									}
							}
							int addAmount;
							int.TryParse(args.Parameters[3], out addAmount);
							if (TShock.Regions.ResizeRegion(args.Parameters[1], addAmount, direction))
							{
								args.Player.SendInfoMessage(GetString("Region Resized Successfully!"));
								TShock.Regions.Reload();
							}
							else
								args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}region resize <region> <u/d/l/r> <amount>", Specifier));
						}
						else
							args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}region resize <region> <u/d/l/r> <amount>", Specifier));
						break;
					}
				case "rename":
					{
						if (args.Parameters.Count != 3)
						{
							args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}region rename <region> <new name>", Specifier));
							break;
						}
						else
						{
							string oldName = args.Parameters[1];
							string newName = args.Parameters[2];

							if (oldName == newName)
							{
								args.Player.SendErrorMessage(GetString("Error: both names are the same."));
								break;
							}

							Region oldRegion = TShock.Regions.GetRegionByName(oldName);

							if (oldRegion == null)
							{
								args.Player.SendErrorMessage(GetString("Invalid region \"{0}\".", oldName));
								break;
							}

							Region newRegion = TShock.Regions.GetRegionByName(newName);

							if (newRegion != null)
							{
								args.Player.SendErrorMessage(GetString("Region \"{0}\" already exists.", newName));
								break;
							}

							if (TShock.Regions.RenameRegion(oldName, newName))
							{
								args.Player.SendInfoMessage(GetString("Region renamed successfully!"));
							}
							else
							{
								args.Player.SendErrorMessage(GetString("Failed to rename the region."));
							}
						}
						break;
					}
				case "tp":
					{
						if (!args.Player.HasPermission(Permissions.tp))
						{
							args.Player.SendErrorMessage(GetString("You do not have permission to teleport."));
							break;
						}
						if (args.Parameters.Count <= 1)
						{
							args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}region tp <region>.", Specifier));
							break;
						}

						string regionName = string.Join(" ", args.Parameters.Skip(1));
						Region region = TShock.Regions.GetRegionByName(regionName);
						if (region == null)
						{
							args.Player.SendErrorMessage(GetString("Region \"{0}\" does not exist.", regionName));
							break;
						}

						args.Player.Teleport(region.Area.Center.X * 16, region.Area.Center.Y * 16);
						break;
					}
				case "help":
				default:
					{
						int pageNumber;
						int pageParamIndex = 0;
						if (args.Parameters.Count > 1)
							pageParamIndex = 1;
						if (!PaginationTools.TryParsePageNumber(args.Parameters, pageParamIndex, args.Player, out pageNumber))
							return;

						List<string> lines = new List<string> {
						  GetString("set <1/2> - Sets the temporary region points."),
						  GetString("clear - Clears the temporary region points."),
						  GetString("define <name> - Defines the region with the given name."),
						  GetString("delete <name> - Deletes the given region."),
						  GetString("name [-u][-z][-p] - Shows the name of the region at the given point."),
						  GetString("rename <region> <new name> - Renames the given region."),
						  GetString("list - Lists all regions."),
						  GetString("resize <region> <u/d/l/r> <amount> - Resizes a region."),
						  GetString("allow <user> <region> - Allows a user to a region."),
						  GetString("remove <user> <region> - Removes a user from a region."),
						  GetString("allowg <group> <region> - Allows a user group to a region."),
						  GetString("removeg <group> <region> - Removes a user group from a region."),
						  GetString("info <region> [-d] - Displays several information about the given region."),
						  GetString("protect <name> <true/false> - Sets whether the tiles inside the region are protected or not."),
						  GetString("z <name> <#> - Sets the z-order of the region."),
						};
						if (args.Player.HasPermission(Permissions.tp))
							lines.Add(GetString("tp <region> - Teleports you to the given region's center."));

						PaginationTools.SendPage(
						  args.Player, pageNumber, lines,
						  new PaginationTools.Settings
						  {
							  HeaderFormat = GetString("Available Region Sub-Commands ({{0}}/{{1}}):"),
							  FooterFormat = GetString("Type {0}region {{0}} for more sub-commands.", Specifier)
						  }
						);
						break;
					}
			}
		}

		#endregion Region Commands

		#region World Protection Commands

		private static void ToggleAntiBuild(CommandArgs args)
		{
			TShock.Config.Settings.DisableBuild = !TShock.Config.Settings.DisableBuild;
			TSPlayer.All.SendSuccessMessage(TShock.Config.Settings.DisableBuild ? GetString("Anti-build is now on.") : GetString("Anti-build is now off."));
		}

		private static void ProtectSpawn(CommandArgs args)
		{
			TShock.Config.Settings.SpawnProtection = !TShock.Config.Settings.SpawnProtection;
			TSPlayer.All.SendSuccessMessage(TShock.Config.Settings.SpawnProtection ? GetString("Spawn is now protected.") : GetString("Spawn is now open."));
		}

		#endregion World Protection Commands

		#region General Commands

		private static void Help(CommandArgs args)
		{
			if (args.Parameters.Count > 1)
			{
				args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}help <command/page>", Specifier));
				return;
			}

			int pageNumber;
			if (args.Parameters.Count == 0 || int.TryParse(args.Parameters[0], out pageNumber))
			{
				if (!PaginationTools.TryParsePageNumber(args.Parameters, 0, args.Player, out pageNumber))
				{
					return;
				}

				IEnumerable<string> cmdNames = from cmd in ChatCommands
											   where cmd.CanRun(args.Player) && (cmd.Name != "setup" || TShock.SetupToken != 0)
											   select Specifier + cmd.Name;

				PaginationTools.SendPage(args.Player, pageNumber, PaginationTools.BuildLinesFromTerms(cmdNames),
					new PaginationTools.Settings
					{
						HeaderFormat = GetString("Commands ({{0}}/{{1}}):"),
						FooterFormat = GetString("Type {0}help {{0}} for more.", Specifier)
					});
			}
			else
			{
				string commandName = args.Parameters[0].ToLower();
				if (commandName.StartsWith(Specifier))
				{
					commandName = commandName.Substring(1);
				}

				Command command = ChatCommands.Find(c => c.Names.Contains(commandName));
				if (command == null)
				{
					args.Player.SendErrorMessage(GetString("Invalid command."));
					return;
				}
				if (!command.CanRun(args.Player))
				{
					args.Player.SendErrorMessage(GetString("You do not have access to this command."));
					return;
				}

				args.Player.SendSuccessMessage(GetString("{0}{1} help: ", Specifier, command.Name));
				if (command.HelpDesc == null)
				{
					args.Player.SendInfoMessage(command.HelpText);
					return;
				}
				foreach (string line in command.HelpDesc)
				{
					args.Player.SendInfoMessage(line);
				}
			}
		}

		private static void GetVersion(CommandArgs args)
		{
			args.Player.SendMessage(GetString($"TShock: {TShock.VersionNum.Color(Utils.BoldHighlight)} {TShock.VersionCodename.Color(Utils.RedHighlight)}."), Color.White);
		}

		private static void ListConnectedPlayers(CommandArgs args)
		{
			bool invalidUsage = (args.Parameters.Count > 2);

			bool displayIdsRequested = false;
			int pageNumber = 1;
			if (!invalidUsage)
			{
				foreach (string parameter in args.Parameters)
				{
					if (parameter.Equals("-i", StringComparison.InvariantCultureIgnoreCase))
					{
						displayIdsRequested = true;
						continue;
					}

					if (!int.TryParse(parameter, out pageNumber))
					{
						invalidUsage = true;
						break;
					}
				}
			}
			if (invalidUsage)
			{
				args.Player.SendMessage(GetString($"List Online Players Syntax"), Color.White);
				args.Player.SendMessage(GetString($"{"playing".Color(Utils.BoldHighlight)} {"[-i]".Color(Utils.RedHighlight)} {"[page]".Color(Utils.GreenHighlight)}"), Color.White);
				args.Player.SendMessage(GetString($"Command aliases: {"playing".Color(Utils.GreenHighlight)}, {"online".Color(Utils.GreenHighlight)}, {"who".Color(Utils.GreenHighlight)}"), Color.White);
				args.Player.SendMessage(GetString($"Example usage: {"who".Color(Utils.BoldHighlight)} {"-i".Color(Utils.RedHighlight)}"), Color.White);
				return;
			}
			if (displayIdsRequested && !args.Player.HasPermission(Permissions.seeids))
			{
				args.Player.SendErrorMessage(GetString("You do not have permission to see player IDs."));
				return;
			}

			if (TShock.Utils.GetActivePlayerCount() == 0)
			{
				args.Player.SendMessage(GetString("There are currently no players online."), Color.White);
				return;
			}
			args.Player.SendMessage(GetString($"Online Players ({TShock.Utils.GetActivePlayerCount().Color(Utils.GreenHighlight)}/{TShock.Config.Settings.MaxSlots})"), Color.White);

			var players = new List<string>();

			foreach (TSPlayer ply in TShock.Players)
			{
				if (ply != null && ply.Active && ply.FinishedHandshake)
				{
					if (displayIdsRequested)
						if (ply.Account != null)
							players.Add(GetString($"{ply.Name} (Index: {ply.Index}, Account ID: {ply.Account.ID})"));
						else
							players.Add(GetString($"{ply.Name} (Index: {ply.Index})"));
					else
						players.Add(ply.Name);
				}
			}

			PaginationTools.SendPage(
				args.Player, pageNumber, PaginationTools.BuildLinesFromTerms(players),
				new PaginationTools.Settings
				{
					IncludeHeader = false,
					FooterFormat = GetString($"Type {Specifier}who {(displayIdsRequested ? "-i" : string.Empty)} for more.")
				}
			);
		}

		private static void SetupToken(CommandArgs args)
		{
			if (TShock.SetupToken == 0)
			{
				args.Player.SendWarningMessage(GetString("The initial setup system is disabled. This incident has been logged."));
				args.Player.SendWarningMessage(GetString("If you are locked out of all admin accounts, ask for help on https://tshock.co/"));
				TShock.Log.Warn("{0} attempted to use the initial setup system even though it's disabled.", args.Player.IP);
				return;
			}

			// If the user account is already logged in, turn off the setup system
			if (args.Player.IsLoggedIn && args.Player.tempGroup == null)
			{
				args.Player.SendSuccessMessage(GetString("Your new account has been verified, and the {0}setup system has been turned off.", Specifier));
				args.Player.SendSuccessMessage(GetString("Share your server, talk with admins, and chill on GitHub & Discord. -- https://tshock.co/"));
				args.Player.SendSuccessMessage(GetString("Thank you for using TShock for Terraria!"));
				FileTools.CreateFile(Path.Combine(TShock.SavePath, "setup.lock"));
				File.Delete(Path.Combine(TShock.SavePath, "setup-code.txt"));
				TShock.SetupToken = 0;
				return;
			}

			if (args.Parameters.Count == 0)
			{
				args.Player.SendErrorMessage(GetString("You must provide a setup code!"));
				return;
			}

			int givenCode;
			if (!Int32.TryParse(args.Parameters[0], out givenCode) || givenCode != TShock.SetupToken)
			{
				args.Player.SendErrorMessage(GetString("Incorrect setup code. This incident has been logged."));
				TShock.Log.Warn(args.Player.IP + " attempted to use an incorrect setup code.");
				return;
			}

			if (args.Player.Group.Name != "superadmin")
				args.Player.tempGroup = new SuperAdminGroup();

			args.Player.SendInfoMessage(GetString("Temporary system access has been given to you, so you can run one command."));
			args.Player.SendWarningMessage(GetString("Please use the following to create a permanent account for you."));
			args.Player.SendWarningMessage(GetString("{0}user add <username> <password> owner", Specifier));
			args.Player.SendInfoMessage(GetString("Creates: <username> with the password <password> as part of the owner group."));
			args.Player.SendInfoMessage(GetString("Please use {0}login <username> <password> after this process.", Specifier));
			args.Player.SendWarningMessage(GetString("If you understand, please {0}login <username> <password> now, and then type {0}setup.", Specifier));
			return;
		}

		private static void ThirdPerson(CommandArgs args)
		{
			if (args.Parameters.Count == 0)
			{
				args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}me <text>", Specifier));
				return;
			}
			if (args.Player.mute)
				args.Player.SendErrorMessage(GetString("You are muted."));
			else
				TSPlayer.All.SendMessage(GetString("*{0} {1}", args.Player.Name, String.Join(" ", args.Parameters)), 205, 133, 63);
		}

		private static void PartyChat(CommandArgs args)
		{
			if (args.Parameters.Count == 0)
			{
				args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}p <team chat text>", Specifier));
				return;
			}
			int playerTeam = args.Player.Team;

			if (args.Player.mute)
				args.Player.SendErrorMessage(GetString("You are muted."));
			else if (playerTeam != 0)
			{
				string msg = GetString("<{0}> {1}", args.Player.Name, String.Join(" ", args.Parameters));
				foreach (TSPlayer player in TShock.Players)
				{
					if (player != null && player.Active && player.Team == playerTeam)
						player.SendMessage(msg, Main.teamColor[playerTeam].R, Main.teamColor[playerTeam].G, Main.teamColor[playerTeam].B);
				}
			}
			else
				args.Player.SendErrorMessage(GetString("You are not in a party!"));
		}

		private static void Mute(CommandArgs args)
		{
			if (args.Parameters.Count < 1)
			{
				args.Player.SendMessage(GetString("Mute Syntax"), Color.White);
				args.Player.SendMessage(GetString($"{"mute".Color(Utils.BoldHighlight)} <{"player".Color(Utils.RedHighlight)}> [{"reason".Color(Utils.GreenHighlight)}]"), Color.White);
				args.Player.SendMessage(GetString($"Example usage: {"mute".Color(Utils.BoldHighlight)} \"{args.Player.Name.Color(Utils.RedHighlight)}\" \"{"No swearing on my Christian server".Color(Utils.GreenHighlight)}\""), Color.White);
				args.Player.SendMessage(GetString($"To mute a player without broadcasting to chat, use the command with {SilentSpecifier.Color(Utils.GreenHighlight)} instead of {Specifier.Color(Utils.RedHighlight)}"), Color.White);
				return;
			}

			var players = TSPlayer.FindByNameOrID(args.Parameters[0]);
			if (players.Count == 0)
			{
				args.Player.SendErrorMessage(GetString($"Could not find any players named \"{args.Parameters[0]}\""));
			}
			else if (players.Count > 1)
			{
				args.Player.SendMultipleMatchError(players.Select(p => p.Name));
			}
			else if (players[0].HasPermission(Permissions.mute))
			{
				args.Player.SendErrorMessage(GetString($"You do not have permission to mute {players[0].Name}"));
			}
			else if (players[0].mute)
			{
				var plr = players[0];
				plr.mute = false;
				if (args.Silent)
					args.Player.SendSuccessMessage(GetString($"You have unmuted {plr.Name}."));
				else
					TSPlayer.All.SendInfoMessage(GetString($"{args.Player.Name} has unmuted {plr.Name}."));
			}
			else
			{
				string reason = GetString("No reason specified.");
				if (args.Parameters.Count > 1)
					reason = String.Join(" ", args.Parameters.ToArray(), 1, args.Parameters.Count - 1);
				var plr = players[0];
				plr.mute = true;
				if (args.Silent)
					args.Player.SendSuccessMessage(GetString($"You have muted {plr.Name} for {reason}"));
				else
					TSPlayer.All.SendInfoMessage(GetString($"{args.Player.Name} has muted {plr.Name} for {reason}."));
			}
		}

		private static void Motd(CommandArgs args)
		{
			args.Player.SendFileTextAsMessage(FileTools.MotdPath);
		}

		private static void Rules(CommandArgs args)
		{
			args.Player.SendFileTextAsMessage(FileTools.RulesPath);
		}

		public static void Whisper(CommandArgs args)
		{
			if (args.Parameters.Count < 2)
			{
				args.Player.SendMessage(GetString("Whisper Syntax"), Color.White);
				args.Player.SendMessage(GetString($"{"whisper".Color(Utils.BoldHighlight)} <{"player".Color(Utils.RedHighlight)}> <{"message".Color(Utils.PinkHighlight)}>"), Color.White);
				args.Player.SendMessage(GetString($"Example usage: {"w".Color(Utils.BoldHighlight)} {args.Player.Name.Color(Utils.RedHighlight)} {"We're no strangers to love, you know the rules, and so do I.".Color(Utils.PinkHighlight)}"), Color.White);
				return;
			}
			var players = TSPlayer.FindByNameOrID(args.Parameters[0]);
			if (players.Count == 0)
			{
				args.Player.SendErrorMessage(GetString($"Could not find any player named \"{args.Parameters[0]}\""));
			}
			else if (players.Count > 1)
			{
				args.Player.SendMultipleMatchError(players.Select(p => p.Name));
			}
			else if (args.Player.mute)
			{
				args.Player.SendErrorMessage(GetString("You are muted."));
			}
			else
			{
				var plr = players[0];
				if (plr == args.Player)
				{
					args.Player.SendErrorMessage(GetString("You cannot whisper to yourself."));
					return;
				}
				if (!plr.AcceptingWhispers)
				{
					args.Player.SendErrorMessage(GetString($"{plr.Name} is not accepting whispers."));
					return;
				}
				var msg = string.Join(" ", args.Parameters.ToArray(), 1, args.Parameters.Count - 1);
				plr.SendMessage(GetString($"<From {args.Player.Name}> {msg}"), Color.MediumPurple);
				args.Player.SendMessage(GetString($"<To {plr.Name}> {msg}"), Color.MediumPurple);
				plr.LastWhisper = args.Player;
				args.Player.LastWhisper = plr;
			}
		}

		private static void Wallow(CommandArgs args)
		{
			args.Player.AcceptingWhispers = !args.Player.AcceptingWhispers;
			if (args.Player.AcceptingWhispers)
				args.Player.SendInfoMessage(GetString("You may now receive whispers from other players."));
			else
				args.Player.SendInfoMessage(GetString("You will no longer receive whispers from other players."));
			args.Player.SendMessage(GetString($"You can use {Specifier.Color(Utils.GreenHighlight)}{"wa".Color(Utils.GreenHighlight)} to toggle this setting."), Color.White);
		}

		private static void Reply(CommandArgs args)
		{
			if (args.Player.mute)
			{
				args.Player.SendErrorMessage(GetString("You are muted."));
			}
			else if (args.Player.LastWhisper != null && args.Player.LastWhisper.Active)
			{
				if (!args.Player.LastWhisper.AcceptingWhispers)
				{
					args.Player.SendErrorMessage(GetString($"{args.Player.LastWhisper.Name} is not accepting whispers."));
					return;
				}
				var msg = string.Join(" ", args.Parameters);
				args.Player.LastWhisper.SendMessage(GetString($"<From {args.Player.Name}> {msg}"), Color.MediumPurple);
				args.Player.SendMessage(GetString($"<To {args.Player.LastWhisper.Name}> {msg}"), Color.MediumPurple);
			}
			else if (args.Player.LastWhisper != null)
			{
				args.Player.SendErrorMessage(GetString($"{args.Player.LastWhisper.Name} is offline and cannot receive your reply."));
			}
			else
			{
				args.Player.SendErrorMessage(GetString("You haven't previously received any whispers."));
				args.Player.SendMessage(GetString($"You can use {Specifier.Color(Utils.GreenHighlight)}{"w".Color(Utils.GreenHighlight)} to whisper to other players."), Color.White);
			}
		}

		private static void Annoy(CommandArgs args)
		{
			if (args.Parameters.Count != 2)
			{
				args.Player.SendMessage(GetString("Annoy Syntax"), Color.White);
				args.Player.SendMessage(GetString($"{"annoy".Color(Utils.BoldHighlight)} <{"player".Color(Utils.RedHighlight)}> <{"seconds".Color(Utils.PinkHighlight)}>"), Color.White);
				args.Player.SendMessage(GetString($"Example usage: {"annoy".Color(Utils.BoldHighlight)} <{args.Player.Name.Color(Utils.RedHighlight)}> <{"10".Color(Utils.PinkHighlight)}>"), Color.White);
				args.Player.SendMessage(GetString($"You can use {SilentSpecifier.Color(Utils.GreenHighlight)} instead of {Specifier.Color(Utils.RedHighlight)} to annoy a player silently."), Color.White);
				return;
			}
			int annoy = 5;
			int.TryParse(args.Parameters[1], out annoy);

			var players = TSPlayer.FindByNameOrID(args.Parameters[0]);
			if (players.Count == 0)
				args.Player.SendErrorMessage(GetString($"Could not find any player named \"{args.Parameters[0]}\""));
			else if (players.Count > 1)
				args.Player.SendMultipleMatchError(players.Select(p => p.Name));
			else
			{
				var ply = players[0];
				args.Player.SendSuccessMessage(GetString($"Annoying {ply.Name} for {annoy} seconds."));
				if (!args.Silent)
					ply.SendMessage(GetString("You are now being annoyed."), Color.LightGoldenrodYellow);
				new Thread(ply.Whoopie).Start(annoy);
			}
		}

		private static void Rocket(CommandArgs args)
		{
			if (args.Parameters.Count != 1)
			{
				args.Player.SendMessage(GetString("Rocket Syntax"), Color.White);
				args.Player.SendMessage(GetString($"{"rocket".Color(Utils.BoldHighlight)} <{"player".Color(Utils.RedHighlight)}>"), Color.White);
				args.Player.SendMessage(GetString($"Example usage: {"rocket".Color(Utils.BoldHighlight)} {args.Player.Name.Color(Utils.RedHighlight)}"), Color.White);
				args.Player.SendMessage(GetString($"You can use {SilentSpecifier.Color(Utils.GreenHighlight)} instead of {Specifier.Color(Utils.RedHighlight)} to rocket a player silently."), Color.White);
				return;
			}
			var players = TSPlayer.FindByNameOrID(args.Parameters[0]);
			if (players.Count == 0)
				args.Player.SendErrorMessage(GetString($"Could not find any player named \"{args.Parameters[0]}\""));
			else if (players.Count > 1)
				args.Player.SendMultipleMatchError(players.Select(p => p.Name));
			else
			{
				var target = players[0];

				if (target.IsLoggedIn && Main.ServerSideCharacter)
				{
					target.TPlayer.velocity.Y = -50;
					TSPlayer.All.SendData(PacketTypes.PlayerUpdate, "", target.Index);

					if (!args.Silent)
					{
						if (target == args.Player)
							if (args.Player.TPlayer.Male)
								TSPlayer.All.SendInfoMessage(GetString($"{args.Player.Name} has launched himself into space."));
							else
								TSPlayer.All.SendInfoMessage(GetString($"{args.Player.Name} has launched herself into space."));
						else
							TSPlayer.All.SendInfoMessage(GetString($"{args.Player.Name} has launched {target.Name} into space."));
						return;
					}

					if (target == args.Player)
						args.Player.SendSuccessMessage(GetString("You have launched yourself into space."));
					else
						args.Player.SendSuccessMessage(GetString($"You have launched {target.Name} into space."));
				}
				else
				{
					if (!Main.ServerSideCharacter)
						args.Player.SendErrorMessage(GetString("SSC must be enabled to use this command."));
					else
						if (target.TPlayer.Male)
						args.Player.SendErrorMessage(GetString($"Unable to launch {target.Name} because he is not logged in."));
					else
						args.Player.SendErrorMessage(GetString($"Unable to launch {target.Name} because she is not logged in."));
				}
			}
		}

		private static void FireWork(CommandArgs args)
		{
			var user = args.Player;
			if (args.Parameters.Count < 1)
			{
				// firework <player> [R|G|B|Y]
				user.SendMessage(GetString("Firework Syntax"), Color.White);
				user.SendMessage(GetString($"{"firework".Color(Utils.CyanHighlight)} <{"player".Color(Utils.PinkHighlight)}> [{"R".Color(Utils.RedHighlight)}|{"G".Color(Utils.GreenHighlight)}|{"B".Color(Utils.BoldHighlight)}|{"Y".Color(Utils.YellowHighlight)}]"), Color.White);
				user.SendMessage(GetString($"Example usage: {"firework".Color(Utils.CyanHighlight)} {user.Name.Color(Utils.PinkHighlight)} {"R".Color(Utils.RedHighlight)}"), Color.White);
				user.SendMessage(GetString($"You can use {SilentSpecifier.Color(Utils.GreenHighlight)} instead of {Specifier.Color(Utils.RedHighlight)} to launch a firework silently."), Color.White);
				return;
			}
			var players = TSPlayer.FindByNameOrID(args.Parameters[0]);
			if (players.Count == 0)
				user.SendErrorMessage(GetString($"Could not find any player named \"{args.Parameters[0]}\""));
			else if (players.Count > 1)
				user.SendMultipleMatchError(players.Select(p => p.Name));
			else
			{
				int type = ProjectileID.RocketFireworkRed;
				if (args.Parameters.Count > 1)
				{
					switch (args.Parameters[1].ToLower())
					{
						case "red":
						case "r":
							type = ProjectileID.RocketFireworkRed;
							break;
						case "green":
						case "g":
							type = ProjectileID.RocketFireworkGreen;
							break;
						case "blue":
						case "b":
							type = ProjectileID.RocketFireworkBlue;
							break;
						case "yellow":
						case "y":
							type = ProjectileID.RocketFireworkYellow;
							break;
						case "r2":
						case "star":
							type = ProjectileID.RocketFireworksBoxRed;
							break;
						case "g2":
						case "spiral":
							type = ProjectileID.RocketFireworksBoxGreen;
							break;
						case "b2":
						case "rings":
							type = ProjectileID.RocketFireworksBoxBlue;
							break;
						case "y2":
						case "flower":
							type = ProjectileID.RocketFireworksBoxYellow;
							break;
						default:
							type = ProjectileID.RocketFireworkRed;
							break;
					}
				}
				var target = players[0];
				int p = Projectile.NewProjectile(Projectile.GetNoneSource(), target.TPlayer.position.X, target.TPlayer.position.Y - 64f, 0f, -8f, type, 0, 0);
				Main.projectile[p].Kill();
				if (target == user)
					args.Player.SendSuccessMessage(GetString("You launched fireworks on yourself."));
				else
					args.Player.SendSuccessMessage(GetString($"You launched fireworks on {target.Name}."));
				if (!args.Silent && target != user)
					target.SendSuccessMessage(GetString($"{user.Name} launched fireworks on you."));
			}
		}

		private static void Aliases(CommandArgs args)
		{
			if (args.Parameters.Count < 1)
			{
				args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}aliases <command or alias>", Specifier));
				return;
			}

			string givenCommandName = string.Join(" ", args.Parameters);
			if (string.IsNullOrWhiteSpace(givenCommandName))
			{
				args.Player.SendErrorMessage(GetString("Please enter a proper command name or alias."));
				return;
			}

			string commandName;
			if (givenCommandName[0] == Specifier[0])
				commandName = givenCommandName.Substring(1);
			else
				commandName = givenCommandName;

			bool didMatch = false;
			foreach (Command matchingCommand in ChatCommands.Where(cmd => cmd.Names.IndexOf(commandName) != -1))
			{
				if (matchingCommand.Names.Count > 1)
					args.Player.SendInfoMessage(
						GetString("Aliases of {0}{1}: {0}{2}", Specifier, matchingCommand.Name, string.Join($", {Specifier}", matchingCommand.Names.Skip(1))));
				else
					args.Player.SendInfoMessage(GetString("{0}{1} defines no aliases.", Specifier, matchingCommand.Name));

				didMatch = true;
			}

			if (!didMatch)
				args.Player.SendErrorMessage(GetString("No command or command alias matching \"{0}\" found.", givenCommandName));
		}

		private static void CreateDumps(CommandArgs args)
		{
			TShock.Utils.DumpPermissionMatrix("PermissionMatrix.txt");
			TShock.Utils.Dump(false);
			args.Player.SendSuccessMessage(GetString("Your reference dumps have been created in the server folder."));
			return;
		}

		private static void SyncLocalArea(CommandArgs args)
		{
			args.Player.SendTileSquareCentered(args.Player.TileX, args.Player.TileY, 32);
			args.Player.SendWarningMessage(GetString("Sync'd!"));
			return;
		}

		#endregion General Commands

		#region Game Commands

		private static void Clear(CommandArgs args)
		{
			var user = args.Player;
			var everyone = TSPlayer.All;
			int radius = 50;

			if (args.Parameters.Count != 1 && args.Parameters.Count != 2)
			{
				user.SendMessage(GetString("Clear Syntax"), Color.White);
				user.SendMessage(GetString($"{"clear".Color(Utils.BoldHighlight)} <{"item".Color(Utils.GreenHighlight)}|{"npc".Color(Utils.RedHighlight)}|{"projectile".Color(Utils.YellowHighlight)}> [{"radius".Color(Utils.PinkHighlight)}]"), Color.White);
				user.SendMessage(GetString($"Example usage: {"clear".Color(Utils.BoldHighlight)} {"i".Color(Utils.RedHighlight)} {"10000".Color(Utils.GreenHighlight)}"), Color.White); user.SendMessage(GetString($"Example usage: {"clear".Color(Utils.BoldHighlight)} {"item".Color(Utils.RedHighlight)} {"10000".Color(Utils.GreenHighlight)}"), Color.White);
				user.SendMessage(GetString($"If you do not specify a radius, it will use a default radius of {radius} around your character."), Color.White);
				user.SendMessage(GetString($"You can use {SilentSpecifier.Color(Utils.GreenHighlight)} instead of {Specifier.Color(Utils.RedHighlight)} to execute this command silently."), Color.White);
				return;
			}

			if (args.Parameters.Count == 2)
			{
				if (!int.TryParse(args.Parameters[1], out radius) || radius <= 0)
				{
					user.SendErrorMessage(GetString($"\"{args.Parameters[1]}\" is not a valid radius."));
					return;
				}
			}

			switch (args.Parameters[0].ToLower())
			{
				case "item":
				case "items":
				case "i":
					{
						int cleared = 0;
						for (int i = 0; i < Main.maxItems; i++)
						{
							float dX = Main.item[i].position.X - user.X;
							float dY = Main.item[i].position.Y - user.Y;

							if (Main.item[i].active && dX * dX + dY * dY <= radius * radius * 256f)
							{
								Main.item[i].active = false;
								everyone.SendData(PacketTypes.ItemDrop, "", i);
								cleared++;
							}
						}
						if (args.Silent)
							user.SendSuccessMessage(GetPluralString("You deleted {0} item within a radius of {1}.", "You deleted {0} items within a radius of {1}.", cleared, cleared, radius));
						else
							everyone.SendInfoMessage(GetPluralString("{0} deleted {1} item within a radius of {2}.", "{0} deleted {1} items within a radius of {2}.", cleared, user.Name, cleared, radius));
					}
					break;
				case "npc":
				case "npcs":
				case "n":
					{
						int cleared = 0;
						for (int i = 0; i < Main.maxNPCs; i++)
						{
							float dX = Main.npc[i].position.X - user.X;
							float dY = Main.npc[i].position.Y - user.Y;

							if (Main.npc[i].active && dX * dX + dY * dY <= radius * radius * 256f)
							{
								Main.npc[i].active = false;
								Main.npc[i].type = 0;
								everyone.SendData(PacketTypes.NpcUpdate, "", i);
								cleared++;
							}
						}
						if (args.Silent)
							user.SendSuccessMessage(GetPluralString("You deleted {0} NPC within a radius of {1}.", "You deleted {0} NPCs within a radius of {1}.", cleared, cleared, radius));
						else
							everyone.SendInfoMessage(GetPluralString("{0} deleted {1} NPC within a radius of {2}.", "{0} deleted {1} NPCs within a radius of {2}.", cleared, user.Name, cleared, radius));
					}
					break;
				case "proj":
				case "projectile":
				case "projectiles":
				case "p":
					{
						int cleared = 0;
						for (int i = 0; i < Main.maxProjectiles; i++)
						{
							float dX = Main.projectile[i].position.X - user.X;
							float dY = Main.projectile[i].position.Y - user.Y;

							if (Main.projectile[i].active && dX * dX + dY * dY <= radius * radius * 256f)
							{
								Main.projectile[i].active = false;
								Main.projectile[i].type = 0;
								everyone.SendData(PacketTypes.ProjectileNew, "", i);
								cleared++;
							}
						}
						if (args.Silent)
							user.SendSuccessMessage(GetPluralString("You deleted {0} projectile within a radius of {1}.", "You deleted {0} projectiles within a radius of {1}.", cleared, cleared, radius));
						else
							everyone.SendInfoMessage(GetPluralString("{0} deleted {1} projectile within a radius of {2}.", "{0} deleted {1} projectiles within a radius of {2}.", cleared, user.Name, cleared, radius));
					}
					break;
				default:
					user.SendErrorMessage(GetString($"\"{args.Parameters[0]}\" is not a valid clear option."));
					break;
			}
		}

		private static void Kill(CommandArgs args)
		{
			// To-Do: separate kill self and kill other player into two permissions
			var user = args.Player;
			if (args.Parameters.Count < 1)
			{
				user.SendMessage(GetString("Kill syntax and example"), Color.White);
				user.SendMessage(GetString($"{"kill".Color(Utils.BoldHighlight)} <{"player".Color(Utils.RedHighlight)}>"), Color.White);
				user.SendMessage(GetString($"Example usage: {"kill".Color(Utils.BoldHighlight)} {user.Name.Color(Utils.RedHighlight)}"), Color.White);
				user.SendMessage(GetString($"You can use {SilentSpecifier.Color(Utils.GreenHighlight)} instead of {Specifier.Color(Utils.RedHighlight)} to execute this command silently."), Color.White);
				return;
			}

			string targetName = String.Join(" ", args.Parameters);
			var players = TSPlayer.FindByNameOrID(targetName);

			if (players.Count == 0)
				user.SendErrorMessage(GetString($"Could not find any player named \"{targetName}\"."));
			else if (players.Count > 1)
				user.SendMultipleMatchError(players.Select(p => p.Name));
			else
			{
				var target = players[0];

				if (target.Dead)
				{
					if (target == user)
						user.SendErrorMessage(GetString("You are already dead!"));
					else
						user.SendErrorMessage(GetString($"{target.Name} is already dead!"));
					return;
				}
				target.KillPlayer();
				if (target == user)
					user.SendSuccessMessage(GetString("You just killed yourself!"));
				else
					user.SendSuccessMessage(GetString($"You just killed {target.Name}!"));
				if (!args.Silent && target != user)
					target.SendErrorMessage(GetString($"{user.Name} just killed you!"));
			}
		}

		private static void Respawn(CommandArgs args)
		{
			if (!args.Player.RealPlayer && args.Parameters.Count == 0)
			{
				args.Player.SendErrorMessage(GetString("You can't respawn the server console!"));
				return;
			}
			TSPlayer playerToRespawn;
			if (args.Parameters.Count > 0)
			{
				if (!args.Player.HasPermission(Permissions.respawnother))
				{
					args.Player.SendErrorMessage(GetString("You do not have permission to respawn another player."));
					return;
				}
				string plStr = String.Join(" ", args.Parameters);
				var players = TSPlayer.FindByNameOrID(plStr);
				if (players.Count == 0)
				{
					args.Player.SendErrorMessage(GetString($"Could not find any player named \"{plStr}\""));
					return;
				}
				if (players.Count > 1)
				{
					args.Player.SendMultipleMatchError(players.Select(p => p.Name));
					return;
				}
				playerToRespawn = players[0];
			}
			else
				playerToRespawn = args.Player;

			if (!playerToRespawn.Dead)
			{
				if (playerToRespawn == args.Player)
					args.Player.SendErrorMessage(GetString("You are not dead!"));
				else
					args.Player.SendErrorMessage(GetString($"{playerToRespawn.Name} is not dead!"));
				return;
			}
			playerToRespawn.Spawn(PlayerSpawnContext.ReviveFromDeath);

			if (playerToRespawn != args.Player)
			{
				args.Player.SendSuccessMessage(GetString($"You have respawned {playerToRespawn.Name}"));
				if (!args.Silent)
					playerToRespawn.SendSuccessMessage(GetString($"{args.Player.Name} has respawned you."));
			}
			else
				playerToRespawn.SendSuccessMessage(GetString("You have respawned yourself."));
		}

		private static void Butcher(CommandArgs args)
		{
			var user = args.Player;
			if (args.Parameters.Count > 1)
			{
				user.SendMessage(GetString("Butcher Syntax and Example"), Color.White);
				user.SendMessage(GetString($"{"butcher".Color(Utils.BoldHighlight)} [{"NPC name".Color(Utils.RedHighlight)}|{"ID".Color(Utils.RedHighlight)}]"), Color.White);
				user.SendMessage(GetString($"Example usage: {"butcher".Color(Utils.BoldHighlight)} {"pigron".Color(Utils.RedHighlight)}"), Color.White);
				user.SendMessage(GetString("All alive NPCs (excluding town NPCs) on the server will be killed if you do not input a name or ID."), Color.White);
				user.SendMessage(GetString($"To get rid of NPCs without making them drop items, use the {"clear".Color(Utils.BoldHighlight)} command instead."), Color.White);
				user.SendMessage(GetString($"To execute this command silently, use {SilentSpecifier.Color(Utils.GreenHighlight)} instead of {Specifier.Color(Utils.RedHighlight)}"), Color.White);
				return;
			}

			int npcId = 0;

			if (args.Parameters.Count == 1)
			{
				var npcs = TShock.Utils.GetNPCByIdOrName(args.Parameters[0]);
				if (npcs.Count == 0)
				{
					user.SendErrorMessage(GetString($"\"{args.Parameters[0]}\" is not a valid NPC."));
					return;
				}

				if (npcs.Count > 1)
				{
					user.SendMultipleMatchError(npcs.Select(n => $"{n.FullName}({n.type})"));
					return;
				}
				npcId = npcs[0].netID;
			}

			int kills = 0;
			for (int i = 0; i < Main.npc.Length; i++)
			{
				if (Main.npc[i].active && ((npcId == 0 && !Main.npc[i].townNPC && Main.npc[i].netID != NPCID.TargetDummy) || Main.npc[i].netID == npcId))
				{
					TSPlayer.Server.StrikeNPC(i, (int)(Main.npc[i].life + (Main.npc[i].defense * 0.6)), 0, 0);
					kills++;
				}
			}

			if (args.Silent)
				user.SendSuccessMessage(GetPluralString("You butchered {0} NPC.", "You butchered {0} NPCs.", kills, kills));
			else
				TSPlayer.All.SendInfoMessage(GetPluralString("{0} butchered {1} NPC.", "{0} butchered {1} NPCs.", kills, user.Name, kills));
		}

		private static void Item(CommandArgs args)
		{
			if (args.Parameters.Count < 1)
			{
				args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}item <item name/id> [item amount] [prefix id/name]", Specifier));
				return;
			}

			int amountParamIndex = -1;
			int itemAmount = 0;
			for (int i = 1; i < args.Parameters.Count; i++)
			{
				if (int.TryParse(args.Parameters[i], out itemAmount))
				{
					amountParamIndex = i;
					break;
				}
			}

			string itemNameOrId;
			if (amountParamIndex == -1)
				itemNameOrId = string.Join(" ", args.Parameters);
			else
				itemNameOrId = string.Join(" ", args.Parameters.Take(amountParamIndex));

			Item item;
			List<Item> matchedItems = TShock.Utils.GetItemByIdOrName(itemNameOrId);
			if (matchedItems.Count == 0)
			{
				args.Player.SendErrorMessage(GetString("Invalid item type!"));
				return;
			}
			else if (matchedItems.Count > 1)
			{
				args.Player.SendMultipleMatchError(matchedItems.Select(i => $"{i.Name}({i.netID})"));
				return;
			}
			else
			{
				item = matchedItems[0];
			}
			if (item.type < 1 && item.type >= Terraria.ID.ItemID.Count)
			{
				args.Player.SendErrorMessage(GetString("The item type {0} is invalid.", itemNameOrId));
				return;
			}

			int prefixId = 0;
			if (amountParamIndex != -1 && args.Parameters.Count > amountParamIndex + 1)
			{
				string prefixidOrName = args.Parameters[amountParamIndex + 1];
				var prefixIds = TShock.Utils.GetPrefixByIdOrName(prefixidOrName);

				if (item.accessory && prefixIds.Contains(PrefixID.Quick))
				{
					prefixIds.Remove(PrefixID.Quick);
					prefixIds.Remove(PrefixID.Quick2);
					prefixIds.Add(PrefixID.Quick2);
				}
				else if (!item.accessory && prefixIds.Contains(PrefixID.Quick))
					prefixIds.Remove(PrefixID.Quick2);

				if (prefixIds.Count > 1)
				{
					args.Player.SendMultipleMatchError(prefixIds.Select(p => p.ToString()));
					return;
				}
				else if (prefixIds.Count == 0)
				{
					args.Player.SendErrorMessage(GetString("No prefix matched \"{0}\".", prefixidOrName));
					return;
				}
				else
				{
					prefixId = prefixIds[0];
				}
			}

			if (args.Player.InventorySlotAvailable || (item.type > 70 && item.type < 75) || item.ammo > 0 || item.type == 58 || item.type == 184)
			{
				if (itemAmount == 0 || itemAmount > item.maxStack)
					itemAmount = item.maxStack;

				if (args.Player.GiveItemCheck(item.type, EnglishLanguage.GetItemNameById(item.type), itemAmount, prefixId))
				{
					item.prefix = (byte)prefixId;
					args.Player.SendSuccessMessage(GetPluralString("Gave {0} {1}.", "Gave {0} {1}s.", itemAmount, itemAmount, item.AffixName()));
				}
				else
				{
					args.Player.SendErrorMessage(GetString("You cannot spawn banned items."));
				}
			}
			else
			{
				args.Player.SendErrorMessage(GetString("Your inventory seems full."));
			}
		}

		private static void RenameNPC(CommandArgs args)
		{
			if (args.Parameters.Count != 2)
			{
				args.Player.SendErrorMessage(GetString("Invalid syntax. Proper syntax: {0}renameNPC <guide, nurse, etc.> <newname>", Specifier));
				return;
			}
			int npcId = 0;
			if (args.Parameters.Count == 2)
			{
				List<NPC> npcs = TShock.Utils.GetNPCByIdOrName(args.Parameters[0]);
				if (npcs.Count == 0)
				{
					args.Player.SendErrorMessage(GetString("Invalid mob type!"));
					return;
				}
				else if (npcs.Count > 1)
				{
					args.Player.SendMultipleMatchError(npcs.Select(n => $"{n.FullName}({n.type})"));
					return;
				}
				else if (args.Parameters[1].Length > 200)
				{
					args.Player.SendErrorMessage(GetString("New name is too large!"));
					return;
				}
				else
				{
					npcId = npcs[0].netID;
				}
			}
			int done = 0;
			for (int i = 0; i < Main.npc.Length; i++)
			{
				if (Main.npc[i].active && ((npcId == 0 && !Main.npc[i].townNPC) || (Main.npc[i].netID == npcId && Main.npc[i].townNPC)))
				{
					Main.npc[i].GivenName = args.Parameters[1];
					NetMessage.SendData(56, -1, -1, NetworkText.FromLiteral(args.Parameters[1]), i, 0f, 0f, 0f, 0);
					done++;
				}
			}
			if (done > 0)
			{
				TSPlayer.All.SendInfoMessage(GetString("{0} renamed the {1}.", args.Player.Name, args.Parameters[0]));
			}
			else
			{
				args.Player.SendErrorMessage(GetString("Could not rename {0}!", args.Parameters[0]));
			}
		}

		private static void Give(CommandArgs args)
		{
			if (args.Parameters.Count < 2)
			{
				args.Player.SendErrorMessage(
					"Invalid syntax. Proper syntax: {0}give <item type/id> <player> [item amount] [prefix id/name]", Specifier);
				return;
			}
			if (args.Parameters[0].Length == 0)
			{
				args.Player.SendErrorMessage(GetString("Missing item name/id."));
				return;
			}
			if (args.Parameters[1].Length == 0)
			{
				args.Player.SendErrorMessage(GetString("Missing player name."));
				return;
			}
			int itemAmount = 0;
			int prefix = 0;
			var items = TShock.Utils.GetItemByIdOrName(args.Parameters[0]);
			args.Parameters.RemoveAt(0);
			string plStr = args.Parameters[0];
			args.Parameters.RemoveAt(0);
			if (args.Parameters.Count == 1)
				int.TryParse(args.Parameters[0], out itemAmount);
			if (items.Count == 0)
			{
				args.Player.SendErrorMessage(GetString("Invalid item type!"));
			}
			else if (items.Count > 1)
			{
				args.Player.SendMultipleMatchError(items.Select(i => $"{i.Name}({i.netID})"));
			}
			else
			{
				var item = items[0];

				if (args.Parameters.Count == 2)
				{
					int.TryParse(args.Parameters[0], out itemAmount);
					var prefixIds = TShock.Utils.GetPrefixByIdOrName(args.Parameters[1]);
					if (item.accessory && prefixIds.Contains(PrefixID.Quick))
					{
						prefixIds.Remove(PrefixID.Quick);
						prefixIds.Remove(PrefixID.Quick2);
						prefixIds.Add(PrefixID.Quick2);
					}
					else if (!item.accessory && prefixIds.Contains(PrefixID.Quick))
						prefixIds.Remove(PrefixID.Quick2);
					if (prefixIds.Count == 1)
						prefix = prefixIds[0];
				}

				if (item.type >= 1 && item.type < Terraria.ID.ItemID.Count)
				{
					var players = TSPlayer.FindByNameOrID(plStr);
					if (players.Count == 0)
					{
						args.Player.SendErrorMessage(GetString("Invalid player!"));
					}
					else if (players.Count > 1)
					{
						args.Player.SendMultipleMatchError(players.Select(p => p.Name));
					}
					else
					{
						var plr = players[0];
						if (plr.InventorySlotAvailable || (item.type > 70 && item.type < 75) || item.ammo > 0 || item.type == 58 || item.type == 184)
						{
							if (itemAmount == 0 || itemAmount > item.maxStack)
								itemAmount = item.maxStack;
							if (plr.GiveItemCheck(item.type, EnglishLanguage.GetItemNameById(item.type), itemAmount, prefix))
							{
								args.Player.SendSuccessMessage(GetPluralString("Gave {0} {1} {2}.", "Gave {0} {1} {2}s.", itemAmount, plr.Name, itemAmount, item.Name));
								plr.SendSuccessMessage(GetPluralString("{0} gave you {1} {2}.", "{0} gave you {1} {2}s.", itemAmount, args.Player.Name, itemAmount, item.Name));
							}
							else
							{
								args.Player.SendErrorMessage(GetString("You cannot spawn banned items."));
							}

						}
						else
						{
							args.Player.SendErrorMessage(GetString("Player does not have free slots!"));
						}
					}
				}
				else
				{
					args.Player.SendErrorMessage(GetString("Invalid item type!"));
				}
			}
		}

		private static void Heal(CommandArgs args)
		{
			// heal <player> [amount]
			// To-Do: break up heal self and heal other into two separate permissions
			var user = args.Player;
			if (args.Parameters.Count < 1 || args.Parameters.Count > 2)
			{
				user.SendMessage(GetString("Heal Syntax and Example"), Color.White);
				user.SendMessage(GetString($"{"heal".Color(Utils.BoldHighlight)} <{"player".Color(Utils.RedHighlight)}> [{"amount".Color(Utils.GreenHighlight)}]"), Color.White);
				user.SendMessage(GetString($"Example usage: {"heal".Color(Utils.BoldHighlight)} {user.Name.Color(Utils.RedHighlight)} {"100".Color(Utils.GreenHighlight)}"), Color.White);
				user.SendMessage(GetString($"If no amount is specified, it will default to healing the target player by their max HP."), Color.White);
				user.SendMessage(GetString($"To execute this command silently, use {SilentSpecifier.Color(Utils.GreenHighlight)} instead of {Specifier.Color(Utils.RedHighlight)}"), Color.White);
				return;
			}
			if (args.Parameters[0].Length == 0)
			{
				user.SendErrorMessage(GetString($"You didn't put a player name."));
				return;
			}

			string targetName = args.Parameters[0];
			var players = TSPlayer.FindByNameOrID(targetName);
			if (players.Count == 0)
				user.SendErrorMessage(GetString($"Unable to find any players named \"{targetName}\""));
			else if (players.Count > 1)
				user.SendMultipleMatchError(players.Select(p => p.Name));
			else
			{
				var target = players[0];
				int amount = target.TPlayer.statLifeMax2;

				if (target.Dead)
				{
					user.SendErrorMessage(GetString("You can't heal a dead player!"));
					return;
				}

				if (args.Parameters.Count == 2)
				{
					int.TryParse(args.Parameters[1], out amount);
				}
				target.Heal(amount);

				if (args.Silent)
					if (target == user)
						user.SendSuccessMessage(GetString($"You healed yourself for {amount} HP."));
					else
						user.SendSuccessMessage(GetString($"You healed {target.Name} for {amount} HP."));
				else
				{
					if (target == user)
						if (target.TPlayer.Male)
							TSPlayer.All.SendInfoMessage(GetString($"{user.Name} healed himself for {amount} HP."));
						else
							TSPlayer.All.SendInfoMessage(GetString($"{user.Name} healed herself for {amount} HP."));
					else
						TSPlayer.All.SendInfoMessage(GetString($"{user.Name} healed {target.Name} for {amount} HP."));
				}
			}
		}

		private static void Buff(CommandArgs args)
		{
			// buff <"buff name|ID"> [duration]
			var user = args.Player;
			if (args.Parameters.Count < 1 || args.Parameters.Count > 2)
			{
				user.SendMessage(GetString("Buff Syntax and Example"), Color.White);
				user.SendMessage(GetString($"{"buff".Color(Utils.BoldHighlight)} <\"{"buff name".Color(Utils.RedHighlight)}|{"ID".Color(Utils.RedHighlight)}\"> [{"duration".Color(Utils.GreenHighlight)}]"), Color.White);
				user.SendMessage(GetString($"Example usage: {"buff".Color(Utils.BoldHighlight)} \"{"obsidian skin".Color(Utils.RedHighlight)}\" {"-1".Color(Utils.GreenHighlight)}"), Color.White);
				user.SendMessage(GetString($"If you don't specify the duration, it will default to {"60".Color(Utils.GreenHighlight)} seconds."), Color.White);
				user.SendMessage(GetString($"If you put {"-1".Color(Utils.GreenHighlight)} as the duration, it will use the max possible time of 415 days."), Color.White);
				return;
			}

			int id = 0;
			int time = 60;
			var timeLimit = (int.MaxValue / 60) - 1;

			if (!int.TryParse(args.Parameters[0], out id))
			{
				var found = TShock.Utils.GetBuffByName(args.Parameters[0]);

				if (found.Count == 0)
				{
					user.SendErrorMessage(GetString($"Unable to find any buffs named \"{args.Parameters[0]}\""));
					return;
				}

				if (found.Count > 1)
				{
					user.SendMultipleMatchError(found.Select(f => Lang.GetBuffName(f)));
					return;
				}
				id = found[0];
			}

			if (args.Parameters.Count == 2)
				int.TryParse(args.Parameters[1], out time);

			if (id > 0 && id < Terraria.ID.BuffID.Count)
			{
				// Max possible buff duration as of Terraria 1.4.2.3 is 35791393 seconds (415 days).
				if (time < 0 || time > timeLimit)
					time = timeLimit;
				user.SetBuff(id, time * 60);
				user.SendSuccessMessage(GetString($"You buffed yourself with {TShock.Utils.GetBuffName(id)} ({TShock.Utils.GetBuffDescription(id)}) for {time} seconds."));
			}
			else
				user.SendErrorMessage(GetString($"\"{id}\" is not a valid buff ID!"));
		}

		private static void GBuff(CommandArgs args)
		{
			var user = args.Player;
			if (args.Parameters.Count < 2 || args.Parameters.Count > 3)
			{
				user.SendMessage(GetString("Give Buff Syntax and Example"), Color.White);
				user.SendMessage(GetString($"{"gbuff".Color(Utils.BoldHighlight)} <{"player".Color(Utils.RedHighlight)}> <{"buff name".Color(Utils.PinkHighlight)}|{"ID".Color(Utils.PinkHighlight)}> [{"seconds".Color(Utils.GreenHighlight)}]"), Color.White);
				user.SendMessage(GetString($"Example usage: {"gbuff".Color(Utils.BoldHighlight)} {user.Name.Color(Utils.RedHighlight)} {"regen".Color(Utils.PinkHighlight)} {"-1".Color(Utils.GreenHighlight)}"), Color.White);
				user.SendMessage(GetString($"To buff a player without them knowing, use {SilentSpecifier.Color(Utils.RedHighlight)} instead of {Specifier.Color(Utils.GreenHighlight)}"), Color.White);
				return;
			}
			int id = 0;
			int time = 60;
			var timeLimit = (int.MaxValue / 60) - 1;
			var foundplr = TSPlayer.FindByNameOrID(args.Parameters[0]);
			if (foundplr.Count == 0)
			{
				user.SendErrorMessage(GetString($"Unable to find any player named \"{args.Parameters[0]}\""));
				return;
			}
			else if (foundplr.Count > 1)
			{
				user.SendMultipleMatchError(foundplr.Select(p => p.Name));
				return;
			}
			else
			{
				if (!int.TryParse(args.Parameters[1], out id))
				{
					var found = TShock.Utils.GetBuffByName(args.Parameters[1]);
					if (found.Count == 0)
					{
						user.SendErrorMessage(GetString($"Unable to find any buff named \"{args.Parameters[1]}\""));
						return;
					}
					else if (found.Count > 1)
					{
						user.SendMultipleMatchError(found.Select(b => Lang.GetBuffName(b)));
						return;
					}
					id = found[0];
				}
				if (args.Parameters.Count == 3)
					int.TryParse(args.Parameters[2], out time);
				if (id > 0 && id < Terraria.ID.BuffID.Count)
				{
					var target = foundplr[0];
					if (time < 0 || time > timeLimit)
						time = timeLimit;
					target.SetBuff(id, time * 60);
					if (target == user)
						user.SendSuccessMessage(GetString($"You buffed yourself with {TShock.Utils.GetBuffName(id)} ({TShock.Utils.GetBuffDescription(id)}) for {time} seconds."));
					else
						user.SendSuccessMessage(GetString($"You have buffed {target.Name} with {TShock.Utils.GetBuffName(id)} ({TShock.Utils.GetBuffDescription(id)}) for {time} seconds!"));
					if (!args.Silent && target != user)
						target.SendSuccessMessage(GetString($"{user.Name} has buffed you with {TShock.Utils.GetBuffName(id)} ({TShock.Utils.GetBuffDescription(id)}) for {time} seconds!"));
				}
				else
					user.SendErrorMessage(GetString("Invalid buff ID!"));
			}
		}

		public static void Grow(CommandArgs args)
		{
			bool canGrowEvil = args.Player.HasPermission(Permissions.growevil);
			string subcmd = args.Parameters.Count == 0 ? "help" : args.Parameters[0].ToLower();

			var name = "Fail"; // assigned value never used
			var x = args.Player.TileX;
			var y = args.Player.TileY + 3;

			if (!TShock.Regions.CanBuild(x, y, args.Player))
			{
				args.Player.SendErrorMessage(GetString("You're not allowed to change tiles here!"));
				return;
			}

			switch (subcmd)
			{
				case "help":
					{
						if (!PaginationTools.TryParsePageNumber(args.Parameters, 1, args.Player, out int pageNumber))
							return;

						var lines = new List<string>
						{
							GetString("- Default trees :"),
							GetString("     'basic', 'sakura', 'willow', 'boreal', 'mahogany', 'ebonwood', 'shadewood', 'pearlwood'."),
							GetString("- Palm trees :"),
							GetString("     'palm', 'corruptpalm', 'crimsonpalm', 'hallowpalm'."),
							GetString("- Gem trees :"),
							GetString("     'topaz', 'amethyst', 'sapphire', 'emerald', 'ruby', 'diamond', 'amber'."),
							GetString("- Misc :"),
							GetString("     'cactus', 'herb', 'mushroom'.")
						};

						PaginationTools.SendPage(args.Player, pageNumber, lines,
								new PaginationTools.Settings
								{
									HeaderFormat = GetString("Trees types & misc available to use. ({{0}}/{{1}}):"),
									FooterFormat = GetString("Type {0}grow help {{0}} for more sub-commands.", Commands.Specifier)
								}
							);
					}
					break;

					bool rejectCannotGrowEvil()
					{
						if (!canGrowEvil)
						{
							args.Player.SendErrorMessage(GetString("You do not have permission to grow this tree type"));
							return false;
						}

						return true;
					}

					bool prepareAreaForGrow(ushort groundType = TileID.Grass, bool evil = false)
					{
						if (evil && !rejectCannotGrowEvil())
							return false;

						for (var i = x - 2; i < x + 3; i++)
						{
							Main.tile[i, y].active(true);
							Main.tile[i, y].type = groundType;
							Main.tile[i, y].wall = WallID.None;
						}
						Main.tile[x, y - 1].wall = WallID.None;

						return true;
					}

					bool growTree(ushort groundType, string fancyName, bool evil = false)
					{
						if (!prepareAreaForGrow(groundType, evil))
							return false;
						WorldGen.GrowTree(x, y);
						name = fancyName;

						return true;
					}

					bool growTreeByType(ushort groundType, string fancyName, ushort typeToPrepare = 2, bool evil = false)
					{
						if (!prepareAreaForGrow(typeToPrepare, evil))
							return false;
						WorldGen.TryGrowingTreeByType(groundType, x, y);
						name = fancyName;

						return true;
					}

					bool growPalmTree(ushort sandType, ushort supportingType, string properName, bool evil = false)
					{
						if (evil && !rejectCannotGrowEvil())
							return false;

						for (int i = x - 2; i < x + 3; i++)
						{
							Main.tile[i, y].active(true);
							Main.tile[i, y].type = sandType;
							Main.tile[i, y].wall = WallID.None;
						}
						for (int i = x - 2; i < x + 3; i++)
						{
							Main.tile[i, y + 1].active(true);
							Main.tile[i, y + 1].type = supportingType;
							Main.tile[i, y + 1].wall = WallID.None;
						}

						Main.tile[x, y - 1].wall = WallID.None;
						WorldGen.GrowPalmTree(x, y);

						name = properName;

						return true;
					}

				case "basic":
					growTree(TileID.Grass, GetString("Basic Tree"));
					break;

				case "boreal":
					growTree(TileID.SnowBlock, GetString("Boreal Tree"));
					break;

				case "mahogany":
					growTree(TileID.JungleGrass, GetString("Rich Mahogany"));
					break;

				case "sakura":
					growTreeByType(TileID.VanityTreeSakura, GetString("Sakura Tree"));
					break;

				case "willow":
					growTreeByType(TileID.VanityTreeYellowWillow, GetString("Willow Tree"));
					break;

				case "shadewood":
					if (!growTree(TileID.CrimsonGrass, GetString("Shadewood Tree"), true))
						return;
					break;

				case "ebonwood":
					if (!growTree(TileID.CorruptGrass, GetString("Ebonwood Tree"), true))
						return;
					break;

				case "pearlwood":
					if (!growTree(TileID.HallowedGrass, GetString("Pearlwood Tree"), true))
						return;
					break;

				case "palm":
					growPalmTree(TileID.Sand, TileID.HardenedSand, GetString("Desert Palm"));
					break;

				case "hallowpalm":
					if (!growPalmTree(TileID.Pearlsand, TileID.HallowHardenedSand, GetString("Hallow Palm"), true))
						return;
					break;

				case "crimsonpalm":
					if (!growPalmTree(TileID.Crimsand, TileID.CrimsonHardenedSand, GetString("Crimson Palm"), true))
						return;
					break;

				case "corruptpalm":
					if (!growPalmTree(TileID.Ebonsand, TileID.CorruptHardenedSand, GetString("Corruption Palm"), true))
						return;
					break;

				case "topaz":
					growTreeByType(TileID.TreeTopaz, GetString("Topaz Gemtree"), 1);
					break;

				case "amethyst":
					growTreeByType(TileID.TreeAmethyst, GetString("Amethyst Gemtree"), 1);
					break;

				case "sapphire":
					growTreeByType(TileID.TreeSapphire, GetString("Sapphire Gemtree"), 1);
					break;

				case "emerald":
					growTreeByType(TileID.TreeEmerald, GetString("Emerald Gemtree"), 1);
					break;

				case "ruby":
					growTreeByType(TileID.TreeRuby, GetString("Ruby Gemtree"), 1);
					break;

				case "diamond":
					growTreeByType(TileID.TreeDiamond, GetString("Diamond Gemtree"), 1);
					break;

				case "amber":
					growTreeByType(TileID.TreeAmber, GetString("Amber Gemtree"), 1);
					break;

				case "cactus":
					Main.tile[x, y].type = TileID.Sand;
					WorldGen.GrowCactus(x, y);
					name = GetString("Cactus");
					break;

				case "herb":
					Main.tile[x, y].active(true);
					Main.tile[x, y].frameX = 36;
					Main.tile[x, y].type = TileID.MatureHerbs;
					WorldGen.GrowAlch(x, y);
					name = GetString("Herb");
					break;

				case "mushroom":
					prepareAreaForGrow(TileID.MushroomGrass);
					WorldGen.GrowShroom(x, y);
					name = GetString("Glowing Mushroom Tree");
					break;

				default:
					args.Player.SendErrorMessage(GetString("Unknown plant!"));
					return;
			}
			if (args.Parameters.Count == 1)
			{
				args.Player.SendTileSquareCentered(x - 2, y - 20, 25);
				args.Player.SendSuccessMessage(GetString($"Tried to grow a {name}."));
			}
		}

		private static void ToggleGodMode(CommandArgs args)
		{
			TSPlayer playerToGod;
			if (args.Parameters.Count > 0)
			{
				if (!args.Player.HasPermission(Permissions.godmodeother))
				{
					args.Player.SendErrorMessage(GetString("You do not have permission to god mode another player."));
					return;
				}
				string plStr = String.Join(" ", args.Parameters);
				var players = TSPlayer.FindByNameOrID(plStr);
				if (players.Count == 0)
				{
					args.Player.SendErrorMessage(GetString("Invalid player!"));
					return;
				}
				else if (players.Count > 1)
				{
					args.Player.SendMultipleMatchError(players.Select(p => p.Name));
					return;
				}
				else
				{
					playerToGod = players[0];
				}
			}
			else if (!args.Player.RealPlayer)
			{
				args.Player.SendErrorMessage(GetString("You can't god mode a non player!"));
				return;
			}
			else
			{
				playerToGod = args.Player;
			}

			playerToGod.GodMode = !playerToGod.GodMode;

			if (playerToGod != args.Player)
			{
				args.Player.SendSuccessMessage(playerToGod.GodMode
					? GetString("{0} is now in god mode.", playerToGod.Name)
					: GetString("{0} is no longer in god mode.", playerToGod.Name));
			}

			if (!args.Silent || (playerToGod == args.Player))
			{
				playerToGod.SendSuccessMessage(playerToGod.GodMode
					? GetString("You are now in god mode.", playerToGod.Name)
					: GetString("You are no longer in god mode.", playerToGod.Name));
			}
		}

		#endregion Game Commands
	}
}
