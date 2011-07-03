The configuration is stored in config.json, and is pretty much self explanatory. Located inside the 'tshock' folder, you can edit config.json to your preferences.


**NOTE: ALL ASTERISKS "*" BEFORE A DESCRIPTION FOR THE SETTING IS A GUESS, OR A 'PRETTY SURE', THIS WAS EDITED BY A USER.**

	{
	  "InvasionMultiplier": 50,
	  "DefaultMaximumSpawns": 4,
	  "DefaultSpawnRate": 700,
	  "ServerPort": 7777,
	  "EnableWhitelist": false,
	  "InfiniteInvasion": false,
	  "AlwaysPvP": false,
	  "KickCheaters": true,
	  "BanCheaters": true,
	  "KickGriefers": true,
	  "BanGriefers": true,
	  "BanKillTileAbusers": true,
	  "KickKillTileAbusers": true,
	  "BanExplosives": true,
	  "KickExplosives": true,
	  "DisableExplosives": true,
	  "SpawnProtection": true,
	  "SpawnProtectionRadius": 5,
	  "DistributationAgent": "facepunch",
	  "MaxSlots": 8,
	  "RangeChecks": true,
	  "SpamChecks": false,
	  "DisableBuild": false,
	  "TileThreshold": 20,
	  "AdminChatRGB": [
             255.0,
             0.0,
             0.0
           ], 
	  "AdminChatPrefix": "(Admin) ",
	  "PvpThrottle": 0,
	  "BackupInterval": 0,
	  "BackupKeepFor": 60,
	  "RememberLeavePos": false,
	  "HardcoreOnly": false,
	  "KickOnHardcoreOnlyDeath": false,
	  "BanOnHardcoreOnlyDeath": false,
	  "AutoSave": true
	}


_InvasionMultiplier_: The equation for calculating invasion size is 100 + (multiplier * (number of active players with greater than 200 health))

_DefaultMaximumSpawns_: The default maximum mobs that will spawn per wave. Higher means more mobs in that wave.

_DefaultSpawnRate_: The delay between waves. Shorter values lead to less mobs.

_ServerPort_: The port the server runs on.

_EnableWhitelist_: Enable or disable the whitelist based on IP addresses in whitelist.txt

_InfiniteInvasion_: Enable the ability for invaison size to never decrease. Make sure to run /invade, and note that this adds 2 million+ goblins to the spawn que for the map.

_AlwaysPvP_: Enable or disable perma pvp.

_KickSaveEditors_: Kicks players who edit save files for cheating.

_BanSaveEditors_: Writes players who edit save files to the cheaters.txt file, and prevents people in that file from joining. You must have kick enabled for this to work as intended.

_BanKillTileAbusers_: Bans players who attempt to kill tiles using TNT or exploits. Requires kick to be enabled.

_KickKillTileAbuesrs_: Kicks players who attempt to kill tiles using TNT or exploits.

_BanExplosives_: Bans players who attempt to throw a bomb or dynamite.

_KickExplosives_: Kicks players who attempt to throw a bomb or dynamite.

_DisableExplosives_: Explosives won't break blocks

_SpawnProtection_: Prevents tiles from being placed within _SpawnProtectionRadius_ of the default spawn.

_SpawnProtectionRadius_: Radius from spawn tile for _SpawnProtection_.

_DistributationAgent_: *Not sure.

_MaxSlots_: Max slots for the server.  *Not sure if this overrides the servers -config. 

_RangeChecks_: Global protection agent for any block distance based anti-grief check.

_SpamChecks_: Global protection against spamming

_DisableBuild_: Disables any building; placing of blocks

_TileThreshold_: *Not sure

_AdminChatRGB_: #.#.#. = Red/Blue/Green - RGB Colors for the Admin Chat Color.  Max value: 255

_AdminChatPrefix_: The Chat Prefix before an admin speaks.  eg. *The prefix was set to "(Admin) ", so..
(Admin) <TerrariaDude>: Hi!
Note: Always put a space after the prefix, so it won't look like this: (Admin)<TerrariaDude): Hi!

_PvpThrottle_: *I believe this means if PVP should be toggle-able or not.  If 1, meaning it's always on.

_BackupInterval_: Backup Interval, in a certain amount of time.  *I believe it's in minutes.  So, value 60 = 60 mins.

_BackupKeepFor_: How long till backup's be kept.

_RememberLeavePos_: Remembers where a player left off.  It works by remembering the IP, NOT the character.  
eg. When you try to disconnect, and reconnect to be automatically placed at spawn, you'll be at your last location.
Note: Won't save after server restarts.

_HardcoreOnly_: Hardcore players ONLY.  This means softcore players cannot join.

_KickOnHardcoreOnlyDeath_: *Kicks a Hardcore player on death.

_BanOnHardcoreOnlyDeath_: *Bans a Hardcore player on death.

_AutoSave_: *Autosave like the default Terraria offers.

If you do not see some of the values here, please delete config.json and run TShock again. It should regenerate.

The [[Cheating and Griefing Checks]], and what is classified as cheating and griefing is located on it's respective page.
