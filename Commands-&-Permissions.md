TShock requires you to be logged into an administrator-level user to run most commands. 
Most of these commands can alter gameplay significantly

**Type /help 1-5 in-game to view the rest of the commands.**

### Complete command list:

## Commands available to everyone
* /register [username] [password] - Register a user account
* /login [username] [password] - Log in to a user account
* /help [page] - Provides a list of commands.
* /playing or /who or /online - List of players on the server.
* /me - Third person chat.
* /p - Team chat.
* /rules - Prints server rules.

## Commands available to logged in users
* /password [oldpassword] [newpassword] - Change your password

## Commands available to administrative users

**Permissions Required: Logged in as an Account in 'superadmin' group**

* /user add [accountname:password] [group] - Adds a user account and assigns them a group.
* /user add [IP.add.re.ss] [group] - Adds a user IP address  and assigns them a group.
* /user del [accountname] - Removes a user account
* /user del [IP.add.re.ss] - Removes a user IP address
* /user group [accountname] [newgroup] - Assigns a new group to a user
* /user password [accountname] [password] - Assigns a new password to a user
* /auth-verify [CODE] - Verifies initial superadmin account(s). [CODE] is displayed in server console.

**Permissions Required: kick, ban, unban**

* /kick [player] [reason] - Use "" for player names with spacing.
* /ban [player] [reason] - Use "" for player names with spacing.
* /ban [ip] [reason]
* /unban [player] - Use "" for player names with spacing.
* /unbanip [ip]

**Permissions Required: maintenance**

* /off - Turns off the server after saving.
* /off-nosave - Kills the server without saving.
* /checkupdates - Checks for TShock updates.

**Permissions Required: causeevents**

* /dropmeteor - Drops a meteor on the map.
* /star - Attempts to bring a star to your location (required night time).
* /bloodmoon - Starts a blood moon event.
* /invade - Toggles a goblin invasion.

**Permissions Required: spawnboss**

* /eater - Summons the Eater of Worlds.
* /eye - Summons the Eye of Cthulhu.
* /skeletron - Summons Skeletron.
* /king - Summons the Slime King.
* /hardcore - Summons Eater of Worlds, Eye of Cthulhu , Slime King, and Skeletron.

**Permissions Required: spawnmob**

* /spawnmob [npc-id or name] [amount]- Spawns an NPC from the [[NPC list]] - Use "" for mob names with spacing.

**Permissions Required: tp**

* /tp [player] - Teleport to the requested player.
* /tp home - Teleport to home. (Bed, or spawn)
* /tp spawn - Teleport to current map's spawn.

**Permissions Required: tphere**

* /tphere [player] - Teleports the requested player to you.

**Permissions Required: warp**

* /warp list - Lists the available warp destinations.
* /warp [destination] - Warps to the named destination.

**Permissions Required: managewarp**

* /setwarp [destination] - Sets a named warp at your current position.
* /delwarp [destination] - Removes a named warp.

**Permissions Required: cfg**

* /setspawn - Sets the map's spawn point at your current position.
* /reload - Reloads the configuration file.
* /showconfig - Shows the current configuration settings.
* /serverpassword - Remotely changes the server's password.
* /save - Saves the server.
* /maxspawns [amount] - Changes the maximum spawns per wave. Higher numbers results in more mobs per spawn wave.
* /spawnrate [amount] - Changes the rate at which monsters spawn. Lower results in faster spawn rates.
* /broadcast [message] - Broadcasts a message from the <Server> playername.

**Permissions Required: time**

* /time [day|night|dusk|noon|midnight] - Sets a specific world time.

**Permissions Required: pvpfun**

* /slap [player] <damage> - Slaps [player], defaults to 5 damage if not provided - Use "" for item/player names with spacing.

**Permissions Required: editspawn**

* /antibuild - Toggles build protection.
* /protectspawn - Toggles spawn protection.
* /region set [1/2] - Sets temporary region points
* /region define [name] - Adds temporary region points into the region list
* /region delete [name] - Deletes the specified region
* /region allow [name] [regionname] - Allows a player to build in specified region
* /region help - Lists region help
* /debugreg - Lists known regions (Will scroll if you have many!)

**Permissions Required: logs**

* /displaylogs - Turns on and off Server Console messages.

**Permissions Required: whisper**

* /whisper [player] [message] - Sends a private whisper to another player
* /w [player] [message] - Sends a private whisper to another player
* /tell [player] [message] - Sends a private whisper to another player
* /reply [message] - Whisper replies to the last person to send you a whisper.
* /r [message] - Whisper replies to the last person to send you a whisper.

**Permissions Required: kill, butcher, item, heal**

* /kill [player] - Kills provided [player] - Use "" for item/player names with spacing.
* /butcher - Slays all non-town NPCs.
* /item [id or name] <amount> or /i [id or name] <amount>- Gives you [id or name] - ***IMPORTANT** Use " " for item names with spacing.
* /give [id or name] [player] <amount> - Gives [player] item [id or name] - ***IMPORTANT** Use "" for item/player names with spacing.
* /heal [player] - Heals you, or if defined, [player] - Use "" for player names with spacing.
