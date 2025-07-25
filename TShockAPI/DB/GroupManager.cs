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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using MySql.Data.MySqlClient;
using TShockAPI.DB.Queries;

namespace TShockAPI.DB
{
	/// <summary>
	/// Represents the GroupManager, which is in charge of group management.
	/// </summary>
	public class GroupManager : IEnumerable<Group>
	{
		private IDbConnection database;
		public readonly List<Group> groups = new List<Group>();

		/// <summary>
		/// Initializes a new instance of the <see cref="GroupManager"/> class with the specified database connection.
		/// </summary>
		/// <param name="db">The connection.</param>
		public GroupManager(IDbConnection db)
		{
			database = db;

			var table = new SqlTable("GroupList",
				new SqlColumn("GroupName", MySqlDbType.VarChar, 32) { Primary = true },
				new SqlColumn("Parent", MySqlDbType.VarChar, 32),
				new SqlColumn("Commands", MySqlDbType.Text),
				new SqlColumn("ChatColor", MySqlDbType.Text),
				new SqlColumn("Prefix", MySqlDbType.Text),
				new SqlColumn("Suffix", MySqlDbType.Text)
			);

			SqlTableCreator creator = new(db, db.GetSqlQueryBuilder());

			if (creator.EnsureTableStructure(table))
			{
				// Add default groups if they don't exist
				AddDefaultGroup(TShock.Config.Settings.DefaultGuestGroupName, "",
					string.Join(",",
						Permissions.canbuild,
						Permissions.canregister,
						Permissions.canlogin,
						Permissions.canpartychat,
						Permissions.cantalkinthird,
						Permissions.canchat,
						Permissions.synclocalarea,
						Permissions.sendemoji));

				AddDefaultGroup(TShock.Config.Settings.DefaultRegistrationGroupName, TShock.Config.Settings.DefaultGuestGroupName,
					string.Join(",",
						Permissions.warp,
						Permissions.canchangepassword,
						Permissions.canlogout,
						Permissions.summonboss,
						Permissions.spawnpets,
						Permissions.worldupgrades,
						Permissions.whisper,
						Permissions.wormhole,
						Permissions.canpaint,
						Permissions.pylon,
						Permissions.tppotion,
						Permissions.magicconch,
						Permissions.demonconch));

				AddDefaultGroup("vip", TShock.Config.Settings.DefaultRegistrationGroupName,
					string.Join(",",
						Permissions.reservedslot,
						Permissions.renamenpc,
						Permissions.startinvasion,
						Permissions.summonboss,
						Permissions.whisper,
						Permissions.wormhole));

				AddDefaultGroup("insecure-guest", "",
					string.Join(",",
						Permissions.canbuild,
						Permissions.canregister,
						Permissions.canlogin,
						Permissions.canpartychat,
						Permissions.cantalkinthird,
						Permissions.canchat,
						Permissions.synclocalarea,
						Permissions.sendemoji,
						Permissions.warp,
						Permissions.summonboss,
						Permissions.spawnpets,
						Permissions.worldupgrades,
						Permissions.startinvasion,
						Permissions.whisper,
						Permissions.wormhole,
						Permissions.canpaint,
						Permissions.pylon,
						Permissions.whisper,
						Permissions.wormhole,
						Permissions.tppotion,
						Permissions.magicconch,
						Permissions.demonconch,
						Permissions.movenpc,
						Permissions.worldupgrades,
						Permissions.rod,
						Permissions.hurttownnpc,
						Permissions.startdd2,
						Permissions.spawnpets));

				AddDefaultGroup("newadmin", "vip",
					string.Join(",",
						Permissions.kick,
						Permissions.editspawn,
						Permissions.reservedslot,
						Permissions.annoy,
						Permissions.checkaccountinfo,
						Permissions.getpos,
						Permissions.mute,
						Permissions.rod,
						Permissions.savessc,
						Permissions.seeids,
						"tshock.world.time.*"));

				AddDefaultGroup("admin", "newadmin",
					string.Join(",",
						Permissions.ban,
						Permissions.whitelist,
						Permissions.spawnboss,
						Permissions.spawnmob,
						Permissions.managewarp,
						Permissions.time,
						Permissions.tp,
						Permissions.slap,
						Permissions.kill,
						Permissions.logs,
						Permissions.immunetokick,
						Permissions.tpothers,
						Permissions.advaccountinfo,
						Permissions.broadcast,
						Permissions.home,
						Permissions.tpallothers,
						Permissions.tpallow,
						Permissions.tpnpc,
						Permissions.tppos,
						Permissions.tpsilent,
						Permissions.userinfo,
						Permissions.spawn));

				AddDefaultGroup("trustedadmin", "admin",
					string.Join(",",
						Permissions.maintenance,
						"tshock.cfg.*",
						"tshock.world.*",
						Permissions.butcher,
						Permissions.item,
						Permissions.give,
						Permissions.heal,
						Permissions.immunetoban,
						Permissions.usebanneditem,
						Permissions.allowclientsideworldedit,
						Permissions.buff,
						Permissions.buffplayer,
						Permissions.clear,
						Permissions.clearangler,
						Permissions.godmode,
						Permissions.godmodeother,
						Permissions.ignoredamagecap,
						Permissions.ignorehp,
						Permissions.ignorekilltiledetection,
						Permissions.ignoreliquidsetdetection,
						Permissions.ignoremp,
						Permissions.ignorepaintdetection,
						Permissions.ignoreplacetiledetection,
						Permissions.ignoreprojectiledetection,
						Permissions.ignorestackhackdetection,
						Permissions.invade,
						Permissions.startdd2,
						Permissions.uploaddata,
						Permissions.uploadothersdata,
						Permissions.spawnpets,
						Permissions.journey_timefreeze,
						Permissions.journey_timeset,
						Permissions.journey_timespeed,
						Permissions.journey_godmode,
						Permissions.journey_windstrength,
						Permissions.journey_windfreeze,
						Permissions.journey_rainstrength,
						Permissions.journey_rainfreeze,
						Permissions.journey_placementrange,
						Permissions.journey_setdifficulty,
						Permissions.journey_biomespreadfreeze,
						Permissions.journey_setspawnrate,
						Permissions.journey_contributeresearch));

				AddDefaultGroup("owner", "trustedadmin",
					string.Join(",",
						Permissions.su,
						Permissions.allowdroppingbanneditems,
						Permissions.antibuild,
						Permissions.canusebannedprojectiles,
						Permissions.canusebannedtiles,
						Permissions.managegroup,
						Permissions.manageitem,
						Permissions.manageprojectile,
						Permissions.manageregion,
						Permissions.managetile,
						Permissions.maxspawns,
						Permissions.serverinfo,
						Permissions.settempgroup,
						Permissions.spawnrate,
						Permissions.tpoverride,
						Permissions.createdumps));
			}

			// Load Permissions from the DB
			LoadPermisions();

			Group.DefaultGroup = GetGroupByName(TShock.Config.Settings.DefaultGuestGroupName);

			AssertCoreGroupsPresent();
		}

		internal void AssertCoreGroupsPresent()
		{
			if (!GroupExists(TShock.Config.Settings.DefaultGuestGroupName))
			{
				TShock.Log.ConsoleError(GetString("The guest group could not be found. This may indicate a typo in the configuration file, or that the group was renamed or deleted."));
				throw new Exception(GetString("The guest group could not be found."));
			}

			if (!GroupExists(TShock.Config.Settings.DefaultRegistrationGroupName))
			{
				TShock.Log.ConsoleError(GetString("The default usergroup could not be found. This may indicate a typo in the configuration file, or that the group was renamed or deleted."));
				throw new Exception(GetString("The default usergroup could not be found."));
			}
		}

		/// <summary>
		/// Asserts that the group reference can be safely assigned to the player object.
		/// <para>If this assertion fails, and <paramref name="kick"/> is true, the player is disconnected. If <paramref name="kick"/> is false, the player will receive an error message.</para>
		/// </summary>
		/// <param name="player">The player in question</param>
		/// <param name="group">The group we want to assign them</param>
		/// <param name="kick">Whether or not failing this check disconnects the player.</param>
		/// <returns></returns>
		public bool AssertGroupValid(TSPlayer player, Group group, bool kick)
		{
			if (group == null)
			{
				if (kick)
					player.Disconnect(GetString("Your account's group could not be loaded. Please contact server administrators about this."));
				else
					player.SendErrorMessage(GetString("Your account's group could not be loaded. Please contact server administrators about this."));
				return false;
			}

			return true;
		}

		private void AddDefaultGroup(string name, string parent, string permissions)
		{
			if (!GroupExists(name))
				AddGroup(name, parent, permissions, Group.defaultChatColor);
		}

		/// <summary>
		/// Determines whether the given group exists.
		/// </summary>
		/// <param name="group">The group.</param>
		/// <returns><c>true</c> if it does; otherwise, <c>false</c>.</returns>
		public bool GroupExists(string group)
		{
			if (group == "superadmin")
				return true;

			return groups.Any(g => g.Name.Equals(group));
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		/// <summary>
		/// Gets the enumerator.
		/// </summary>
		/// <returns>The enumerator.</returns>
		public IEnumerator<Group> GetEnumerator()
		{
			return groups.GetEnumerator();
		}

		/// <summary>
		/// Gets the group matching the specified name.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns>The group.</returns>
		public Group GetGroupByName(string name)
		{
			var ret = groups.Where(g => g.Name == name);
			return 1 == ret.Count() ? ret.ElementAt(0) : null;
		}

		/// <summary>
		/// Adds group with name and permissions if it does not exist.
		/// </summary>
		/// <param name="name">name of group</param>
		/// <param name="parentname">parent of group</param>
		/// <param name="permissions">permissions</param>
		/// <param name="chatcolor">chatcolor</param>
		public void AddGroup(string name, string parentname, string permissions, string chatcolor)
		{
			if (GroupExists(name))
			{
				throw new GroupExistsException(name);
			}

			var group = new Group(name, null, chatcolor);
			group.Permissions = permissions;
			if (!string.IsNullOrWhiteSpace(parentname))
			{
				var parent = groups.FirstOrDefault(gp => gp.Name == parentname);
				if (parent == null || name == parentname)
				{
					var error = GetString($"Invalid parent group {parentname} for group {group.Name}");
					TShock.Log.ConsoleError(error);
					throw new GroupManagerException(error);
				}
				group.Parent = parent;
			}

			string query = database.GetSqlType() switch
			{
				SqlType.Sqlite => "INSERT OR IGNORE INTO GroupList (GroupName, Parent, Commands, ChatColor) VALUES (@0, @1, @2, @3);",
				SqlType.Mysql => "INSERT IGNORE INTO GroupList SET GroupName=@0, Parent=@1, Commands=@2, ChatColor=@3",
				SqlType.Postgres => "INSERT INTO GroupList (GroupName, Parent, Commands, ChatColor) VALUES (@0, @1, @2, @3) ON CONFLICT (GroupName) DO NOTHING",
				_ => throw new NotSupportedException(GetString("Unsupported database type."))
			};

			if (database.Query(query, name, parentname, permissions, chatcolor) is 1)
			{
				groups.Add(group);
			}
			else
			{
				throw new GroupManagerException(GetString($"Failed to add group {name}."));
			}
		}

		/// <summary>
		/// Updates a group including permissions
		/// </summary>
		/// <param name="name">name of the group to update</param>
		/// <param name="parentname">parent of group</param>
		/// <param name="permissions">permissions</param>
		/// <param name="chatcolor">chatcolor</param>
		/// <param name="suffix">suffix</param>
		/// <param name="prefix">prefix</param> //why is suffix before prefix?!
		public void UpdateGroup(string name, string parentname, string permissions, string chatcolor, string suffix, string prefix)
		{
			Group group = GetGroupByName(name);
			if (group == null)
				throw new GroupNotExistException(name);

			Group parent = null;
			if (!string.IsNullOrWhiteSpace(parentname))
			{
				parent = GetGroupByName(parentname);
				if (parent == null || parent == group)
					throw new GroupManagerException(GetString($"Invalid parent group {parentname} for group {name}."));

				// Check if the new parent would cause loops.
				List<Group> groupChain = new List<Group> { group, parent };
				Group checkingGroup = parent.Parent;
				while (checkingGroup != null)
				{
					if (groupChain.Contains(checkingGroup))
						throw new GroupManagerException(
							GetString($"Parenting group {group} to {parentname} would cause loops in the parent chain."));

					groupChain.Add(checkingGroup);
					checkingGroup = checkingGroup.Parent;
				}
			}

			// Ensure any group validation is also persisted to the DB.
			var newGroup = new Group(name, parent, chatcolor, permissions);
			newGroup.Prefix = prefix;
			newGroup.Suffix = suffix;
			string query = "UPDATE GroupList SET Parent=@0, Commands=@1, ChatColor=@2, Suffix=@3, Prefix=@4 WHERE GroupName=@5";
			if (database.Query(query, parentname, newGroup.Permissions, newGroup.ChatColor, suffix, prefix, name) != 1)
				throw new GroupManagerException(GetString($"Failed to update group \"{name}\"."));

			group.ChatColor = chatcolor;
			group.Permissions = permissions;
			group.Parent = parent;
			group.Prefix = prefix;
			group.Suffix = suffix;
		}

		/// <summary>
		/// Renames the specified group.
		/// </summary>
		/// <param name="name">The group's name.</param>
		/// <param name="newName">The new name.</param>
		/// <returns>The result from the operation to be sent back to the user.</returns>
		public string RenameGroup(string name, string newName)
		{
			if (!GroupExists(name))
			{
				throw new GroupNotExistException(name);
			}

			if (GroupExists(newName))
			{
				throw new GroupExistsException(newName);
			}

			using var db = database.CloneEx();
			db.Open();
			using var transaction = db.BeginTransaction();
			try
			{
				using (var command = db.CreateCommand())
				{
					command.CommandText = "UPDATE GroupList SET GroupName = @0 WHERE GroupName = @1";
					command.AddParameter("@0", newName);
					command.AddParameter("@1", name);
					command.ExecuteNonQuery();
				}

				var oldGroup = GetGroupByName(name);
				var newGroup = new Group(newName, oldGroup.Parent, oldGroup.ChatColor, oldGroup.Permissions)
				{
					Prefix = oldGroup.Prefix,
					Suffix = oldGroup.Suffix
				};
				groups.Remove(oldGroup);
				groups.Add(newGroup);

				// We need to check if the old group has been referenced as a parent and update those references accordingly
				using (var command = db.CreateCommand())
				{
					command.CommandText = "UPDATE GroupList SET Parent = @0 WHERE Parent = @1";
					command.AddParameter("@0", newName);
					command.AddParameter("@1", name);
					command.ExecuteNonQuery();
				}
				foreach (var group in groups.Where(g => g.Parent != null && g.Parent == oldGroup))
				{
					group.Parent = newGroup;
				}

				// Read the config file to prevent the possible loss of any unsaved changes
				TShock.Config.Read(FileTools.ConfigPath, out bool writeConfig);
				if (TShock.Config.Settings.DefaultGuestGroupName == oldGroup.Name)
				{
					TShock.Config.Settings.DefaultGuestGroupName = newGroup.Name;
					Group.DefaultGroup = newGroup;
				}
				if (TShock.Config.Settings.DefaultRegistrationGroupName == oldGroup.Name)
				{
					TShock.Config.Settings.DefaultRegistrationGroupName = newGroup.Name;
				}
				if (writeConfig)
				{
					TShock.Config.Write(FileTools.ConfigPath);
				}

				// We also need to check if any users belong to the old group and automatically apply changes
				using (var command = db.CreateCommand())
				{
					command.CommandText = "UPDATE Users SET Usergroup = @0 WHERE Usergroup = @1";
					command.AddParameter("@0", newName);
					command.AddParameter("@1", name);
					command.ExecuteNonQuery();
				}
				foreach (var player in TShock.Players.Where(p => p?.Group == oldGroup))
				{
					player.Group = newGroup;
				}

				transaction.Commit();
				return GetString($"Group {name} has been renamed to {newName}.");
			}
			catch (Exception ex)
			{
				TShock.Log.Error(GetString($"An exception has occurred during database transaction: {ex.Message}"));
				try
				{
					transaction.Rollback();
				}
				catch (Exception rollbackEx)
				{
					TShock.Log.Error(GetString($"An exception has occurred during database rollback: {rollbackEx.Message}"));
				}
			}

			throw new GroupManagerException(GetString($"Failed to rename group {name}."));
		}

		/// <summary>
		/// Deletes the specified group.
		/// </summary>
		/// <param name="name">The group's name.</param>
		/// <param name="exceptions">Whether exceptions will be thrown in case something goes wrong.</param>
		/// <returns>The result from the operation to be sent back to the user.</returns>
		public string DeleteGroup(string name, bool exceptions = false)
		{
			if (!GroupExists(name))
			{
				if (exceptions)
					throw new GroupNotExistException(name);
				return GetString($"Group {name} doesn't exist.");
			}

			if (name == Group.DefaultGroup.Name)
			{
				if (exceptions)
					throw new GroupManagerException(GetString("You can't remove the default guest group."));
				return GetString("You can't remove the default guest group.");
			}

			if (database.Query("DELETE FROM GroupList WHERE GroupName=@0", name) == 1)
			{
				groups.Remove(TShock.Groups.GetGroupByName(name));
				return GetString($"Group {name} has been deleted successfully.");
			}

			if (exceptions)
				throw new GroupManagerException(GetString($"Failed to delete group {name}."));
			return GetString($"Failed to delete group {name}.");
		}

		/// <summary>
		/// Enumerates the given permission list and adds permissions for the specified group accordingly.
		/// </summary>
		/// <param name="name">The group name.</param>
		/// <param name="permissions">The permission list.</param>
		/// <returns>The result from the operation to be sent back to the user.</returns>
		public string AddPermissions(string name, List<string> permissions)
		{
			if (!GroupExists(name))
				return GetString($"Group {name} doesn't exist.");

			var group = TShock.Groups.GetGroupByName(name);
			var oldperms = group.Permissions; // Store old permissions in case of error
			permissions.ForEach(p => group.AddPermission(p));

			if (database.Query("UPDATE GroupList SET Commands=@0 WHERE GroupName=@1", group.Permissions, name) == 1)
				return "Group " + name + " has been modified successfully.";

			// Restore old permissions so DB and internal object are in a consistent state
			group.Permissions = oldperms;
			return "";
		}

		/// <summary>
		/// Enumerates the given permission list and removes valid permissions for the specified group accordingly.
		/// </summary>
		/// <param name="name">The group name.</param>
		/// <param name="permissions">The permission list.</param>
		/// <returns>The result from the operation to be sent back to the user.</returns>
		public string DeletePermissions(string name, List<string> permissions)
		{
			if (!GroupExists(name))
				return GetString($"Group {name} doesn't exist.");

			var group = TShock.Groups.GetGroupByName(name);
			var oldperms = group.Permissions; // Store old permissions in case of error
			permissions.ForEach(p => group.RemovePermission(p));

			if (database.Query("UPDATE GroupList SET Commands=@0 WHERE GroupName=@1", group.Permissions, name) == 1)
				return "Group " + name + " has been modified successfully.";

			// Restore old permissions so DB and internal object are in a consistent state
			group.Permissions = oldperms;
			return "";
		}

		/// <summary>
		/// Enumerates the group list and loads permissions for each group appropriately.
		/// </summary>
		public void LoadPermisions()
		{
			try
			{
				List<Group> newGroups = new List<Group>(groups.Count);
				Dictionary<string, string> newGroupParents = new Dictionary<string, string>(groups.Count);
				using (var reader = database.QueryReader("SELECT * FROM GroupList"))
				{
					while (reader.Read())
					{
						string groupName = reader.Get<string>("GroupName");
						if (groupName == "superadmin")
						{
							TShock.Log.ConsoleWarn(GetString("Group \"superadmin\" is defined in the database even though it's a reserved group name."));
							continue;
						}

						newGroups.Add(new Group(groupName, null, reader.Get<string>("ChatColor"), reader.Get<string>("Commands"))
						{
							Prefix = reader.Get<string>("Prefix"),
							Suffix = reader.Get<string>("Suffix"),
						});

						try
						{
							newGroupParents.Add(groupName, reader.Get<string>("Parent"));
						}
						catch (ArgumentException)
						{
							// Just in case somebody messed with the unique primary key.
							TShock.Log.ConsoleError(GetString($"The group {groupName} appeared more than once. Keeping current group settings."));
							return;
						}
					}
				}

				try
				{
					// Get rid of deleted groups.
					for (int i = 0; i < groups.Count; i++)
						if (newGroups.All(g => g.Name != groups[i].Name))
							groups.RemoveAt(i--);

					// Apply changed group settings while keeping the current instances and add new groups.
					foreach (Group newGroup in newGroups)
					{
						Group currentGroup = groups.FirstOrDefault(g => g.Name == newGroup.Name);
						if (currentGroup != null)
							newGroup.AssignTo(currentGroup);
						else
							groups.Add(newGroup);
					}

					// Resolve parent groups.
					Debug.Assert(newGroups.Count == newGroupParents.Count);
					for (int i = 0; i < groups.Count; i++)
					{
						Group group = groups[i];
						string parentGroupName;
						if (!newGroupParents.TryGetValue(group.Name, out parentGroupName) || string.IsNullOrEmpty(parentGroupName))
							continue;

						group.Parent = groups.FirstOrDefault(g => g.Name == parentGroupName);
						if (group.Parent == null)
						{
							TShock.Log.ConsoleError(
								GetString($"Group {group.Name} is referencing a non existent parent group {parentGroupName}, parent reference was removed."));
						}
						else
						{
							if (group.Parent == group)
								TShock.Log.ConsoleWarn(
									GetString($"Group {group.Name} is referencing itself as parent group; parent reference was removed."));

							List<Group> groupChain = new List<Group> { group };
							Group checkingGroup = group;
							while (checkingGroup.Parent != null)
							{
								if (groupChain.Contains(checkingGroup.Parent))
								{
									TShock.Log.ConsoleError(
										GetString($"Group \"{checkingGroup.Name}\" is referencing parent group {checkingGroup.Parent.Name} which is already part of the parent chain. Parent reference removed."));

									checkingGroup.Parent = null;
									break;
								}
								groupChain.Add(checkingGroup);
								checkingGroup = checkingGroup.Parent;
							}
						}
					}
				}
				finally
				{
					if (!groups.Any(g => g is SuperAdminGroup))
						groups.Add(new SuperAdminGroup());
				}
			}
			catch (Exception ex)
			{
				TShock.Log.ConsoleError(GetString($"Error on reloading groups: {ex}"));
			}
		}
	}

	/// <summary>
	/// Represents the base GroupManager exception.
	/// </summary>
	[Serializable]
	public class GroupManagerException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="GroupManagerException"/> with the specified message.
		/// </summary>
		/// <param name="message">The message.</param>
		public GroupManagerException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="GroupManagerException"/> with the specified message and inner exception.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="inner">The inner exception.</param>
		public GroupManagerException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}

	/// <summary>
	/// Represents the GroupExists exception.
	/// This exception is thrown whenever an attempt to add an existing group into the database is made.
	/// </summary>
	[Serializable]
	public class GroupExistsException : GroupManagerException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="GroupExistsException"/> with the specified group name.
		/// </summary>
		/// <param name="name">The group name.</param>
		public GroupExistsException(string name)
			: base(GetString($"Group {name} already exists"))
		{
		}
	}

	/// <summary>
	/// Represents the GroupNotExist exception.
	/// This exception is thrown whenever we try to access a group that does not exist.
	/// </summary>
	[Serializable]
	public class GroupNotExistException : GroupManagerException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="GroupNotExistException"/> with the specified group name.
		/// </summary>
		/// <param name="name">The group name.</param>
		public GroupNotExistException(string name)
			: base(GetString($"Group {name} does not exist"))
		{
		}
	}
}
