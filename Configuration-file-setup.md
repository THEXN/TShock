**Warning: The wiki will be moving to confluence soon. While we transition pages over, editing will be restricted to those given accounts by Shank (@nicatronTg). This wiki will be turned off eventually, however, editing will be opened on Confluence when this happens. (See http://ci.tshock.co:8080/)**

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

## Automatically Generated (Do not edit here)

## AllowLoginAnyUsername  
**Type:** Boolean  
**Description:** Allows users to login with any username with /login  
**Default:** "True"  

## AllowRegisterAnyUsername  
**Type:** Boolean  
**Description:** Allows users to register any username with /register  
**Default:** "False"  

## AutoSave  
**Type:** Boolean  
**Description:** Enable/Disable Terrarias built in auto save  
**Default:** "True"  

## BackupInterval  
**Type:** Int32  
**Description:** Backup frequency in minutes. So, a value of 60 = 60 minutes. Backups are stored in the \tshock\backups folder.  
**Default:** "0"  

## BackupKeepFor  
**Type:** Int32  
**Description:** How long backups are kept in minutes. 2880 = 2 days.  
**Default:** "60"  

## BanOnMediumcoreDeath  
**Type:** Boolean  
**Description:** Bans a Hardcore player on death.  
**Default:** "False"  

## BufferPackets  
**Type:** Boolean  
**Description:** Buffers up the packets and sends them out at the end of each frame  
**Default:** "True"  

## ChatFormat  
**Type:** String  
**Description:** Change ingame chat format, {0} = Group Name, {1} = Group Prefix, {2} = Player Name, {3} = Group Suffix, {4} = Chat Message  
**Default:** "{1}{2}{3}: {4}"  

## DefaultGuestGroupName  
**Type:** String  
**Description:** Selects the default group name to place non registered users under  
**Default:** "guest"  

## DefaultMaximumSpawns  
**Type:** Int32  
**Description:** The default maximum mobs that will spawn per wave. Higher means more mobs in that wave.  
**Default:** "5"  

## DefaultRegistrationGroupName  
**Type:** String  
**Description:** Selects the default group name to place new registrants under  
**Default:** "default"  

## DefaultSpawnRate  
**Type:** Int32  
**Description:** The delay between waves. Shorter values lead to less mobs.  
**Default:** "600"  

## DisableBuild  
**Type:** Boolean  
**Description:** Disables any building; placing of blocks  
**Default:** "False"  

## DisableClownBombs  
**Type:** Boolean  
**Description:** Disables clown bomb projectiles from spawning  
**Default:** "False"  

## DisableDungeonGuardian  
**Type:** Boolean  
**Description:** Disables Dungeon Guardian from being spawned by player packets, this will instead force a respawn  
**Default:** "False"  

## DisableHardmode  
**Type:** Boolean  
**Description:** Disables hardmode, can't never be activated. Overrides /starthardmode  
**Default:** "False"  

## DisableInvisPvP  
**Type:** Boolean  
**Description:** Disables Invisibility potions from being used in PvP (Note, they can use them on the client, but the effect isn't sent to the rest of the server)  
**Default:** "False"  

## DisableLoginBeforeJoin  
**Type:** Boolean  
**Description:** Disable users from being able to login with account password when joining  
**Default:** "False"  

## DisablePlayerCountReporting  
**Type:** Boolean  
**Description:** Disables reporting of playercount to the stat system.  
**Default:** "False"  

## DisableSnowBalls  
**Type:** Boolean  
**Description:** Disables snow ball projectiles from spawning  
**Default:** "False"  

## DisableSpewLogs  
**Type:** Boolean  
**Description:** Force-Disable printing logs to players with the log permission  
**Default:** "True"  

## DisableTombstones  
**Type:** Boolean  
**Description:** Disable tombstones for all players.  
**Default:** "True"  

## DisplayIPToAdmins  
**Type:** Boolean  
**Description:** Displays a player's IP on join to everyone who has the log permission  
**Default:** "False"  

## EnableBanOnUsernames  
**Type:** Boolean  
**Description:** Enables kicking of banned users by matching their Character Name  
**Default:** "False"  

## EnableDNSHostResolution  
**Type:** Boolean  
**Description:** Enables DNS resolution of incoming connections with GetGroupForIPExpensive.  
**Default:** "False"  

## EnableGeoIP  
**Type:** Boolean  
**Description:** This will announce a player's location on join  
**Default:** "False"  

## EnableInsecureTileFixes  
**Type:** Boolean  
**Description:** Some tiles are 'fixed' by not letting TShock handle them. Disabling this may break certain asthetic tiles.  
**Default:** "True"  

## EnableIPBans  
**Type:** Boolean  
**Description:** Enables kicking of banned users by matching their IP Address  
**Default:** "True"  

## EnableTokenEndpointAuthentication  
**Type:** Boolean  
**Description:** This will turn on a token requirement for the /status API endpoint.  
**Default:** "False"  

## EnableWhitelist  
**Type:** Boolean  
**Description:** Enable or disable the whitelist based on IP addresses in whitelist.txt  
**Default:** "False"  

## ForceTime  
**Type:** String  
**Description:** Force the world time to be normal, day, or night  
**Default:** "normal"  

## HardcoreOnly  
**Type:** Boolean  
**Description:** Hardcore players ONLY. This means softcore players cannot join.  
**Default:** "False"  

## HashAlgorithm  
**Type:** String  
**Description:** Valid types are "sha512", "sha256", "md5", append with "-xp" for the xp supported algorithms  
**Default:** "sha512"  

## InfiniteInvasion  
**Type:** Boolean  
**Description:** Enable the ability for invaison size to never decrease. Make sure to run /invade, and note that this adds 2 million+ goblins to the spawn que for the map.  
**Default:** "False"  

## InvasionMultiplier  
**Type:** Int32  
**Description:** The equation for calculating invasion size is 100 + (multiplier * (number of active players with greater than 200 health))  
**Default:** "1"  

## KickOnMediumcoreDeath  
**Type:** Boolean  
**Description:** Kicks a Hardcore player on death.  
**Default:** "False"  

## KickProxyUsers  
**Type:** Boolean  
**Description:** Kicks users using a proxy as identified with the GeoIP database  
**Default:** "True"  

## MasterServer  
**Type:** String  
**Description:** Not implemented  
**Default:** "127.0.0.1"  

## MaximumLoginAttempts  
**Type:** Int32  
**Description:** Number of failed login attempts before kicking the player.  
**Default:** "3"  

## MaxRangeForDisabled  
**Type:** Int32  
**Description:** The maximum distance players disabled for various reasons can move from  
**Default:** "10"  

## MaxSlots  
**Type:** Int32  
**Description:** Max slots for the server. If you want people to be kicked with "Server is full" set this to how many players you want max and then set Terraria max players to 2 higher.  
**Default:** "8"  

## MediumcoreBanReason  
**Type:** String  
**Description:** Bans a Mediumcore player on death.  
**Default:** "Death results in a ban"  

## MediumcoreKickReason  
**Type:** String  
**Description:** Kicks a Mediumcore player on death.  
**Default:** "Death results in a kick"  

## MediumcoreOnly  
**Type:** Boolean  
**Description:** Mediumcore players ONLY. This means softcore players cannot join.  
**Default:** "False"  

## MySqlDbName  
**Type:** String  
**Description:** Database name to connect to  
**Default:** ""  

## MySqlHost  
**Type:** String  
**Description:** The MySQL Hostname and port to direct connections to  
**Default:** "localhost:3306"  

## MySqlPassword  
**Type:** String  
**Description:** Database password to connect with  
**Default:** ""  

## MySqlUsername  
**Type:** String  
**Description:** Database username to connect with  
**Default:** ""  

## ProjectileThreshold  
**Type:** Int32  
**Description:** Disable a player if they exceed this number of projectile new within 1 second.  
**Default:** "50"  

## PvPMode  
**Type:** String  
**Description:** Set the server pvp mode. Vaild types are, "normal", "always", "disabled"  
**Default:** "normal"  

## RangeChecks  
**Type:** Boolean  
**Description:** Global protection agent for any block distance based anti-grief check.  
**Default:** "True"  

## RconPassword  
**Type:** String  
**Description:** Not implemented  
**Default:** ""  

## RconPort  
**Type:** Int32  
**Description:** Not implemented  
**Default:** "7777"  

## RegionProtectChests  
**Type:** Boolean  
**Description:** Protect chests with region and build permissions  
**Default:** "False"  

## RememberLeavePos  
**Type:** Boolean  
**Description:** Remembers where a player left off. It works by remembering the IP, NOT the character.  
eg. When you try to disconnect, and reconnect to be automatically placed at spawn, you'll be at your last location. Note: Won't save after server restarts.  
**Default:** "False"  

## RequireLogin  
**Type:** Boolean  
**Description:** Require all players to register or login before being allowed to play.  
**Default:** "False"  

## RestApiEnabled  
**Type:** Boolean  
**Description:** Enable/Disable the rest api.  
**Default:** "False"  

## RestApiPort  
**Type:** Int32  
**Description:** This is the port which the rest api will listen on.  
**Default:** "7878"  

## SaveWorldOnCrash  
**Type:** Boolean  
**Description:** This will save the world if Terraria crashes from an unhandled exception.  
**Default:** "True"  

## ServerFullNoReservedReason  
**Type:** String  
**Description:** String that is used when kicking people when the server is full with no reserved slots.  
**Default:** "Server is full. No reserved slots open."  

## ServerFullReason  
**Type:** String  
**Description:** String that is used when kicking people when the server is full.  
**Default:** "Server is full"  

## ServerName  
**Type:** String  
**Description:** Not implemented  
**Default:** ""  

## ServerNickname  
**Type:** String  
**Description:** This is used when the API endpoint /status is queried.  
**Default:** "TShock Server"  

## ServerPassword  
**Type:** String  
**Description:** Server password required to join server  
**Default:** ""  

## ServerPort  
**Type:** Int32  
**Description:** The port the server runs on.  
**Default:** "7777"  

## ServerSideInventory  
**Type:** Boolean  
**Description:** Enable Server Side Inventory checks, EXPERIMENTAL  
**Default:** "False"  

## SpawnProtection  
**Type:** Boolean  
**Description:** Prevents tiles from being placed within SpawnProtectionRadius of the default spawn.  
**Default:** "True"  

## SpawnProtectionRadius  
**Type:** Int32  
**Description:** Radius from spawn tile for SpawnProtection.  
**Default:** "10"  

## StorageType  
**Type:** String  
**Description:** Valid types are "sqlite" and "mysql"  
**Default:** "sqlite"  

## SuperAdminChatPrefix  
**Type:** String  
**Description:** Super admin group chat prefix  
**Default:** "(Admin) "  

## SuperAdminChatRGB  
**Type:** Single[]  
**Description:** #.#.#. = Red/Blue/Green - RGB Colors for the Admin Chat Color. Max value: 255  
**Default:** "System.Single[]"  

## SuperAdminChatSuffix  
**Type:** String  
**Description:** Super admin group chat suffix  
**Default:** ""  

## TileKillThreshold  
**Type:** Int32  
**Description:** Disable/Revert a player if they exceed this number of tile kills within 1 second.  
**Default:** "60"  

## TileLiquidThreshold  
**Type:** Int32  
**Description:** Disable a player if they exceed this number of liquid sets within 1 second.  
**Default:** "15"  

## TilePlaceThreshold  
**Type:** Int32  
**Description:** Disable/Revert a player if they exceed this number of tile places within 1 second.  
**Default:** "20"  


If you do not see some of the values here, please delete config.json and run TShock again. It should regenerate.

The [[Cheating and Griefing Checks]], and what is classified as cheating and griefing is located on it's respective page.