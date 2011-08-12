TShock requires you to be logged into an administrator-level user to run most commands. 
Most of these commands can alter gameplay significantly

###Type /help 1-5 in-game to view the rest of the commands.###

### Complete command list:

## Commands available to everyone
* /register [username] [password] - Register a user account
* /login [username] [password] - Log in to a user account
* /help [page] - Provides a list of commands.
* /playing, /who, /online - List of players on the server.
* /me - Third person chat.
* /p - Party/Team chat.
* /rules - Prints server rules.

## Commands available to logged in users
* /password [oldpassword] [newpassword] - Change your password

## Commands available to groups ##

###Permissions Required: Logged in as 'superadmin' group###

* /user add [accountname:password] [group] - Adds a user account and assigns them a group.
* /user add [IP.add.re.ss] [group] - Adds a user IP address  and assigns them a group.
* /user del [accountname] - Removes a user account
* /user del [IP.add.re.ss] - Removes a user IP address
* /user group [accountname] [newgroup] - Assigns a new group to a user
* /user password [accountname] [password] - Assigns a new password to a user
* /auth-verify [CODE] - Verifies initial superadmin account(s). [CODE] is displayed in server console.
* /ip [player] - Privately print the IP address of a player

###Permissions Required: kick, ban, unban###

* /kick [player] [reason] - Use "quotes" for player names with spacing. (Example: /kick "sarah connor" abuse)
* /ban [player] [reason] - Use "quotes" for player names with spacing.
* /ban [ip] [reason]
* /unban [player] - Use "" for player names with spacing.
* /unbanip [ip]

###Permissions Required: maintenance###

* /off - Turns off the server after saving.
* /off-nosave - Kills the server without saving.
* /checkupdates - Checks for TShock updates.
* /clearbans -1 - Removes all bans

###Permissions Required: whitelist###

* /whitelist [player] - While "EnableWhitelist: true" in the config.json, only players who are added to the whitelist with this command are able to enter the server. These names are stored in \tshock\whitelist.txt.

###Permissions Required: causeevents###

* /dropmeteor - Drops a meteor on the map.
* /star - Attempts to bring a star to your location (required night time).
* /bloodmoon - Starts a blood moon event.
* /invade - Toggles a goblin invasion.

###Permissions Required: spawnboss###

* /eater - Summons the Eater of Worlds.
* /eye - Summons the Eye of Cthulhu.
* /skeletron - Summons Skeletron.
* /king - Summons the Slime King.
* /hardcore - Summons Eater of Worlds, Eye of Cthulhu , Slime King, and Skeletron.

###Permissions Required: spawnmob###

* /spawnmob [npc-id or name] [amount]- Spawns an NPC from the [[NPC list]] - Use "" for mob names with spacing.

###Permissions Required: tp###

* /tp [player] - Teleport to the requested player.
* /spawn, /home - Teleport to home. (Bed, or spawn)

###Permissions Required: tphere###

* /tphere [player] - Teleports the requested player to you.

###Permissions Required: warp###

* /warp list - Lists the available warp destinations.
* /warp [destination] - Warps to the named destination.

###Permissions Required: managewarp###

* /setwarp [destination] - Sets a named warp at your current position.
* /delwarp [destination] - Removes a named warp.

###Permissions Required: managegroup###

* /addGroup <group name> [optional permissions, space separated or comma separated, not both] - Allows adding of additional group types, other than the default groups (VIP, newadmin, trustedadmin, etc.).
 * Example: /addGroup tester ban kick heal 
 * OR: /addGroup tester ban,kick,heal 
 * NOT: /addGroup tester ban, kick, heal
* /delGroup <group name> - Delete an existing group.
* /modGroup add|del <group name> <permission to add or remove, only one at a time, sorry!> - <permission> is the permission required to use a set of commands.
 * Example: /modGroup add vip managewarp


###Permissions Required: manageitem###
* /addItem <item name or id> - Add an item to the ban list.
 * Example: /addItem "gold coin"
* /delItem <item name or id> - Delete an existing item from the ban list.
 * Example: /delItem "gold coin"


###Permissions Required: cfg###

* /setspawn - Sets the map's spawn point at your current position.
* /reload - Reloads the configuration file.
* /showconfig - Shows the current configuration settings.
* /serverpassword - Remotely changes the server's password.
* /save - Saves the server.
* /maxspawns [amount] - Changes the maximum spawns per wave. Higher numbers results in more mobs per spawn wave.
* /spawnrate [amount] - Changes the rate at which monsters spawn. Lower results in faster spawn rates.
* /broadcast [message] - Broadcasts a message from the <Server> playername.
* /convert - This command will dump all users from both Regions and Warps and also change all Worlds to reference this WorldID. You must manually fix multi-world configurations.

###Permissions Required: time###

* /time [day|night|dusk|noon|midnight] - Sets a specific world time.

###Permissions Required: pvpfun###

* /slap [player] <damage> - Slaps [player], defaults to 5 damage if not provided - Use "quotes" for player names with spacing.

###Permissions Required: editspawn###

* /antibuild - Toggles build protection.
* /protectspawn - Toggles spawn protection.
* /region set [1/2] - Sets temporary region points
* /region name - Shows the name of the region you are in
* /region define [name] - Adds temporary region points into the region list
* /region delete [name] - Deletes the specified region
* /region allow [name] [regionname] - Allows a player to build in specified region
* /region list - Shows a named list of all regions for that world
* /region help - Lists region help

###Permissions Required: logs###
_Note: By default, groups with this permission will automatically see all server console messages (slash "/" commands)._

* /displaylogs - Turns on and off Server Console messages.

###Permissions Required: whisper###
_Note: groups with the "logs" permission (above) will be able to see all /commands being used, and thus will see whispers._

* /whisper, /w, /tell [player] [message] - Sends a private whisper to another player
* /reply, /r [message] - Whisper replies to the last person to send you a whisper.

###Permissions Requred: annoy###

* /annoy [player] [time in seconds] - Plays the Whoopie Cushion sound on a player for [time in seconds]. Cannot be heard by other players.

###Permissions Required: kill, butcher, item, heal###

* /kill [player] - Kills provided [player] - Use "" for item/player names with spacing.
* /butcher - Slays all non-town NPCs.
* /item [id or name] <amount> or /i [id or name] <amount>- Gives you [id or name] (Use "qoutes" for names with spacing)
 * Example: /i "molten pick"
* /give [id or name] [player] <amount> - Gives [player] item [id or name] (Use "qoutes" for items/names with spacing)
 * Example: /give "molten pick" "sarah connor" 1
* /heal [player] - Heals you, or if defined, [player] (Use "quotes" for player names with spacing)

###Other Permissions###
_Note: These are not in-game commands, but can still be added to a group's permissions._

* canwater - allow this group to use water
* canlava - allow this group to use lava
* canspike - allow this group to place spikes
* canbuild - allow this group to build (does not bypass other checks, such as protected regions)
* adminchat - Colors and adds a prefix to the group's chat. This prefix and color is set in \tshock\config.json.
* immunetokick - group can't be kicked
* immunetoban - group can't be banned
* ignorecheatdetection - allow group to cheat (health/mana cheats) 
* ignoregriefdetection - allow group to grief (use explosives, water, lava even if they dont have premission to)
* usebanneditem - allows group the use of banned items
* reservedslot - allow this group to join the server, regardless of whether or not it is full