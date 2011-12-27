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

## Automatically Generated < And old, will be updated when we release the next one. >

## InvasionMultiplier  
**Type:** Int32  
**Description:** The equation for calculating invasion size is 100 + (multiplier * (number of active players with greater than 200 health))  
**Default:** "1"  

## DefaultMaximumSpawns  
**Type:** Int32  
**Description:** The default maximum mobs that will spawn per wave. Higher means more mobs in that wave.  
**Default:** "5"  

## DefaultSpawnRate  
**Type:** Int32  
**Description:** The delay between waves. Shorter values lead to less mobs.  
**Default:** "600"  

## ServerPort  
**Type:** Int32  
**Description:** The port the server runs on.  
**Default:** "7777"  

## EnableWhitelist  
**Type:** Boolean  
**Description:** Enable or disable the whitelist based on IP addresses in whitelist.txt  
**Default:** "False"  

## InfiniteInvasion  
**Type:** Boolean  
**Description:** Enable the ability for invaison size to never decrease. Make sure to run /invade, and note that this adds 2 million+ goblins to the spawn que for the map.  
**Default:** "False"  

## AlwaysPvP  
**Type:** Boolean  
**Description:** Enable or disable perma pvp.  
**Default:** "False"  

## KickCheaters  
**Type:** Boolean  
**Description:** Kicks players who edit save files for cheating.  
**Default:** "True"  

## BanCheaters  
**Type:** Boolean  
**Description:** None  
**Default:** "True"  

## KickGriefers  
**Type:** Boolean  
**Description:** None  
**Default:** "True"  

## BanGriefers  
**Type:** Boolean  
**Description:** None  
**Default:** "True"  

## BanKillTileAbusers  
**Type:** Boolean  
**Description:** Bans players who attempt to kill tiles using TNT or exploits. Requires kick to be enabled.  
**Default:** "True"  

## KickKillTileAbusers  
**Type:** Boolean  
**Description:** Kicks players who attempt to kill tiles using TNT or exploits.  
**Default:** "True"  

## BanExplosives  
**Type:** Boolean  
**Description:** Bans players who attempt to throw a bomb or dynamite.  
**Default:** "True"  

## KickExplosives  
**Type:** Boolean  
**Description:** Kicks players who attempt to throw a bomb or dynamite.  
**Default:** "True"  

## DisableExplosives  
**Type:** Boolean  
**Description:** Explosives won't break blocks  
**Default:** "True"  

## SpawnProtection  
**Type:** Boolean  
**Description:** Prevents tiles from being placed within SpawnProtectionRadius of the default spawn.  
**Default:** "True"  

## SpawnProtectionRadius  
**Type:** Int32  
**Description:** Radius from spawn tile for SpawnProtection.  
**Default:** "5"  

## MaxSlots  
**Type:** Int32  
**Description:** Max slots for the server. If you want people to be kicked with "Server is full" set this to how many players you want max and then set Terraria max players to 2 higher.  
**Default:** "8"  

## RangeChecks  
**Type:** Boolean  
**Description:** Global protection agent for any block distance based anti-grief check.  
**Default:** "True"  

## SpamChecks  
**Type:** Boolean  
**Description:** Enable/Disable Terrarias built in spam checks.  
**Default:** "False"  

## DisableBuild  
**Type:** Boolean  
**Description:** Disables any building; placing of blocks  
**Default:** "False"  

## TileThreshold  
**Type:** Int32  
**Description:** Kick a player if they exceed this number of tile kills within 1 second.  
**Default:** "60"  

## SuperAdminChatRGB  
**Type:** Single[]  
**Description:** #.#.#. = Red/Blue/Green - RGB Colors for the Admin Chat Color. Max value: 255  
**Default:** "System.Single[]"  

## AdminChatPrefix  
**Type:** String  
**Description:** The Chat Prefix before an admin speaks. eg. *The prefix was set to "(Admin) ", so.. (Admin) : Hi! Note: If you put a space after the prefix, it will look like this: (Admin) <TerrariaDude): Hi!  
**Default:** "(Admin) "  

## AdminChatEnabled  
**Type:** Boolean  
**Description:** None  
**Default:** "True"  

## PvpThrottle  
**Type:** Int32  
**Description:** Seconds a player has to wait between pvp toggles.  
**Default:** "0"  

## BackupInterval  
**Type:** Int32  
**Description:** Backup frequency in minutes. So, a value of 60 = 60 minutes. Backups are stored in the \tshock\backups folder.  
**Default:** "0"  

## BackupKeepFor  
**Type:** Int32  
**Description:** How long backups are kept in minutes. 2880 = 2 days.  
**Default:** "60"  

## RememberLeavePos  
**Type:** Boolean  
**Description:** Remembers where a player left off. It works by remembering the IP, NOT the character.  
eg. When you try to disconnect, and reconnect to be automatically placed at spawn, you'll be at your last location. Note: Won't save after server restarts.  
**Default:** "False"  

## HardcoreOnly  
**Type:** Boolean  
**Description:** Hardcore players ONLY. This means softcore players cannot join.  
**Default:** "False"  

## MediumcoreOnly  
**Type:** Boolean  
**Description:** Mediumcore players ONLY. This means softcore players cannot join.  
**Default:** "False"  

## KickOnMediumcoreDeath  
**Type:** Boolean  
**Description:** Kicks a Hardcore player on death.  
**Default:** "False"  

## BanOnMediumcoreDeath  
**Type:** Boolean  
**Description:** Bans a Hardcore player on death.  
**Default:** "False"  

## AutoSave  
**Type:** Boolean  
**Description:** Enable/Disable Terrarias built in auto save  
**Default:** "True"  

## MaximumLoginAttempts  
**Type:** Int32  
**Description:** Number of failed login attempts before kicking the player.  
**Default:** "3"  

## RconPassword  
**Type:** String  
**Description:** Not implemented  
**Default:** ""  

## RconPort  
**Type:** Int32  
**Description:** Not implemented  
**Default:** "7777"  

## ServerName  
**Type:** String  
**Description:** Not implemented  
**Default:** ""  

## MasterServer  
**Type:** String  
**Description:** Not implemented  
**Default:** "127.0.0.1"  

## StorageType  
**Type:** String  
**Description:** Valid types are "sqlite" and "mysql"  
**Default:** "sqlite"  

## MySqlHost  
**Type:** String  
**Description:** None  
**Default:** "localhost:3306"  

## MySqlDbName  
**Type:** String  
**Description:** None  
**Default:** ""  

## MySqlUsername  
**Type:** String  
**Description:** None  
**Default:** ""  

## MySqlPassword  
**Type:** String  
**Description:** None  
**Default:** ""  

## RangeCheckBanReason  
**Type:** String  
**Description:** None  
**Default:** "Placing impossible to place blocks."  

## SendSectionAbuseReason  
**Type:** String  
**Description:** None  
**Default:** "SendSection abuse."  

## NPCSpawnAbuseReason  
**Type:** String  
**Description:** None  
**Default:** "Spawn NPC abuse"  

## UpdatePlayerAbuseReason  
**Type:** String  
**Description:** None  
**Default:** "Update Player abuse"  

## ExplosiveAbuseReason  
**Type:** String  
**Description:** None  
**Default:** "Throwing an explosive device."  

## KillMeAbuseReason  
**Type:** String  
**Description:** None  
**Default:** "Trying to execute KillMe on someone else."  

## IllogicalLiquidUseReason  
**Type:** String  
**Description:** None  
**Default:** "Manipulating liquid without bucket."  

## LiquidAbuseReason  
**Type:** String  
**Description:** None  
**Default:** "Placing impossible to place liquid."  

## TileKillAbuseReason  
**Type:** String  
**Description:** None  
**Default:** "Tile Kill abuse ({0})"  

## MediumcoreBanReason  
**Type:** String  
**Description:** None  
**Default:** "Death results in a ban"  

## MediumcoreKickReason  
**Type:** String  
**Description:** None  
**Default:** "Death results in a kick"  

## ProjectileAbuseReason  
**Type:** String  
**Description:** None  
**Default:** "Projectile abuse"  

## TileAbuseReason  
**Type:** String  
**Description:** None  
**Default:** "Tile abuse ({0})"  

## GriefClientReason  
**Type:** String  
**Description:** None  
**Default:** "Grief client detected ({0})"  

## EnableDNSHostResolution  
**Type:** Boolean  
**Description:** None  
**Default:** "False"  

## EnableBanOnUsernames  
**Type:** Boolean  
**Description:** None  
**Default:** "False"  

## EnableAntiLag  
**Type:** Boolean  
**Description:** None  
**Default:** "True"  

## DefaultRegistrationGroupName  
**Type:** String  
**Description:** None  
**Default:** "default"  

## DisableSpewLogs  
**Type:** Boolean  
**Description:** None  
**Default:** "True"  

## HashAlgorithm  
**Type:** String  
**Description:** Valid types are "sha512", "sha256", "md5", append with "-xp" for the xp supported algorithms  
**Default:** "sha512"  

## BufferPackets  
**Type:** Boolean  
**Description:** Buffers up the packets and sends them out at the end of each frame  
**Default:** "True"  

## ChatDisplayGroup  
**Type:** Boolean  
**Description:** Display the users group when they chat.  
**Default:** "False"  

## ServerFullReason  
**Type:** String  
**Description:** String that is used when kicking people when the server is full.  
**Default:** "Server is full"  

## SaveWorldOnCrash  
**Type:** Boolean  
**Description:** This will save the world if Terraria crashes from an unhandled exception.  
**Default:** "True"  

## KickCustomItems  
**Type:** Boolean  
**Description:** This is kick players who have custom items in their inventory (via a mod)  
**Default:** "False"  

## EnableGeoIP  
**Type:** Boolean  
**Description:** This will announce a player's location on join  
**Default:** "False"  

## EnableTokenEndpointAuthentication  
**Type:** Boolean  
**Description:** This will turn on a token requirement for the /status API endpoint.  
**Default:** "False"  

## ServerNickname  
**Type:** String  
**Description:** This is used when the API endpoint /status is queried.  
**Default:** "TShock Server"  

## RestApiEnabled  
**Type:** Boolean  
**Description:** Enable/Disable the rest api.  
**Default:** "False"  

## RestApiPort  
**Type:** Int32  
**Description:** This is the port which the rest api will listen on.  
**Default:** "7878"  

## DisableTombstones  
**Type:** Boolean  
**Description:** Disable tombstones for all players.  
**Default:** "True"  

## DisplayIPToAdmins  
**Type:** Boolean  
**Description:** Displays a player's IP on join to everyone who has the log permission  
**Default:** "False"  



If you do not see some of the values here, please delete config.json and run TShock again. It should regenerate.

The [[Cheating and Griefing Checks]], and what is classified as cheating and griefing is located on it's respective page.
