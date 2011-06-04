The configuration is stored in config.json, and is pretty much self explanatory. Located inside the 'tshock' folder, you can edit config.json to your preferences.


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
	  "SpawnProtection": true,
	  "SpawnProtectionRadius": 5
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

_SpawnProtection_: Prevents tiles from being placed within _SpawnProtectionRadius_ of the default spawn.

_SpawnProtectionRadius_: Radius from spawn tile for _SpawnProtection_.

The [[Cheating and Griefing Checks]], and what is classified as cheating and griefing is located on it's respective page.
