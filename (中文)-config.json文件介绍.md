## AllowAllowedGroupsToSpawnBannedItems  
即使设置了 PreventBannedItemSpawn 为 true，也允许“禁止物品允许列表中的组”生成禁止物品。
* **字段类型**: `Boolean`
* **默认值**: `False`

## AllowCorruptionCreep  
允许腐化蔓延。*
* **字段类型**: `Boolean`
* **默认值**: `True`

## AllowCrimsonCreep  
允许时猩红蔓延。*
* **字段类型**: `Boolean`
* **默认值**: `True`

## AllowCutTilesAndBreakables  
允许玩家在通常无法建造的地方破坏临时方块（草、罐子等）。
* **字段类型**: `Boolean`
* **默认值**: `False`

## AllowHallowCreep  
允许神圣蔓延。*
* **字段类型**: `Boolean`
* **默认值**: `True`

## AllowIce  
允许在通常无法建造的地方放置冰。
* **字段类型**: `Boolean`
* **默认值**: `False`

## AllowLoginAnyUsername  
允许用户登录任何账户，即使用户名与角色名称不匹配。
* **字段类型**: `Boolean`
* **默认值**: `True`

## AllowRegisterAnyUsername  
允许用户注册不一定与角色名称匹配的用户名。
* **字段类型**: `Boolean`
* **默认值**: `False`

## AnnounceSave  
启用或禁用世界保存广播。
* **字段类型**: `Boolean`
* **默认值**: `True`

## AnonymousBossInvasions  
是否在Boss生成或入侵开始时进行公告。
* **字段类型**: `Boolean`
* **默认值**: `True`

## ApplicationRestTokens  
一个包含REST令牌的字典，外部应用程序可以使用这些令牌向您的服务器发起查询。
* **字段类型**: `Dictionary`2`
* **默认值**: `System.Collections.Generic.Dictionary`2[System.String,Rests.SecureRest+TokenData]`

## AutoSave  
启用或禁用泰拉瑞亚内置的世界自动保存功能。
* **字段类型**: `Boolean`
* **默认值**: `True`

## BackupInterval  
备份间隔时间，以分钟为单位。备份文件存储在tshock/backups文件夹中。
* **字段类型**: `Int32`
* **默认值**: `10`

## BackupKeepFor  
备份保留多长时间，以分钟计。例如：2880 = 2天
* **字段类型**: `Int32`
* **默认值**: `240`

## BanOnHardcoreDeath  
是否在硬核模式玩家死亡时将其封禁。
* **字段类型**: `Boolean`
* **默认值**: `False`

## BanOnMediumcoreDeath  
是否在中核模式玩家死亡时将其封禁。
* **字段类型**: `Boolean`
* **默认值**: `False`

## BCryptWorkFactor  
确定使用的BCrypt工作因子。如果增加，所有密码将在验证时升级到新的工作因子。计算轮数为2^n。请谨慎增加。范围：5-31。
* **字段类型**: `Int32`
* **默认值**: `7`

## BombExplosionRadius  
确定炸弹从爆炸点开始影响方块的范围内，以方块为单位。
* **字段类型**: `Int32`
* **默认值**: `5`

## BroadcastRGB  
用于广播消息颜色的RGB值。
#.#.# = 红/蓝/绿
最大值：255
* **字段类型**: `Int32[]`
* **默认值**: `System.Int32[]`

## ChatAboveHeadsFormat  
在使用头顶聊天时更改玩家名称。以玩家名称开头，并用方括号括起来，遵循泰拉瑞亚的格式。
与ChatFormat格式相同，但不包含消息内容。
* **字段类型**: `String`
* **默认值**: `{2}`

## ChatFormat  
更改游戏内聊天格式：{0} = 组名，{1} = 组前缀，{2} = 玩家名，{3} = 组后缀，{4} = 聊天消息。
* **字段类型**: `String`
* **默认值**: `{1}{2}{3}: {4}`

## CommandSilentSpecifier  
指定以哪个字符串开始静默执行命令。
注意：如果字符串长度大于1，则无法正常工作。
* **字段类型**: `String`
* **默认值**: `.`

## CommandSpecifier  
指定以哪个字符串开始执行命令。
注意：如果字符串长度大于1，则无法正常工作。
* **字段类型**: `String`
* **默认值**: `/`

## DebugLogs  
服务器是否应输出与系统操作相关的调试级别消息。
* **字段类型**: `Boolean`
* **默认值**: `False`

## DefaultGuestGroupName  
未注册玩家默认的组名
* **字段类型**: `String`
* **默认值**: `guest`

## DefaultMaximumSpawns  
每波将生成的怪物默认最大数量。数值越高，波次中的怪物就越多（最大刷怪量）
* **字段类型**: `Int32`
* **默认值**: `5`

## DefaultRegistrationGroupName  
新注册用户默认的组名
* **字段类型**: `String`
* **默认值**: `默认值`

## DefaultSpawnRate  
波次之间的延迟。值越低，怪物越多（刷怪速率）。
* **字段类型**: `Int32`
* **默认值**: `600`

## DisableBuild  
禁用任何放置或移除物块。
* **字段类型**: `Boolean`
* **默认值**: `False`

## DisableClownBombs  
禁用小丑炸弹射弹的生成。
* **字段类型**: `Boolean`
* **默认值**: `False`

## DisableCustomDeathMessages  
允许您禁用或启用防止创建自定义死亡消息的保护功能。专为那些为此问题提出了更具原创性解决方案的开发者而设计。
* **字段类型**: `Boolean`
* **默认值**: `True`

## DisableDefaultIPBan  
如果没有传递参数给封禁命令，默认情况下禁用IP封禁。
* **字段类型**: `Boolean`
* **默认值**: `False`

## DisableDungeonGuardian  
阻止地牢守卫生成，同时将玩家传送至其出生点。
* **字段类型**: `Boolean`
* **默认值**: `False`

## DisableHardmode  
如果启用，血肉墙 或 /starthardmode 命令将不会激活困难模式。
* **字段类型**: `Boolean`
* **默认值**: `False`

## DisableInvisPvP  
在启用PvP时，通过使玩家对其他客户端可见来禁用隐身药水的效果。
* **字段类型**: `Boolean`
* **默认值**: `False`

## DisableLoginBeforeJoin  
防止用户在完成连接之前登录。
* **字段类型**: `Boolean`
* **默认值**: `False`

## DisableModifiedZenith  
禁止使用非天顶剑武器或篡改AI参数的天顶剑射弹。
* **字段类型**: `Boolean`
* **默认值**: `False`

## DisablePrimeBombs  
禁用机械骷髅王炸弹生成，用于在"For The Worthy"种子世界中防止意外地形破坏
* **字段类型**: `Boolean`
* **默认值**: `False`

## DisableSecondUpdateLogs  
防止OnSecondUpdate检查写入日志文件。
* **字段类型**: `Boolean`
* **默认值**: `False`

## DisableSnowBalls  
禁用雪人的雪球物块射弹。
* **字段类型**: `Boolean`
* **默认值**: `False`

## DisableSpewLogs  
禁用向具有日志权限的玩家发送日志消息。
* **字段类型**: `Boolean`
* **默认值**: `True`

## DisableTombstones  
禁用所有玩家死亡时掉落墓碑。
* **字段类型**: `Boolean`
* **默认值**: `True`

## DisableUUIDLogin  
防止用户使用其客户端UUID登录。
* **字段类型**: `Boolean`
* **默认值**: `False`

## DisplayIPToAdmins  
向具有日志权限的用户显示玩家的IP地址。
* **字段类型**: `Boolean`
* **默认值**: `False`

## EnableChatAboveHeads  
是否在玩家头顶显示聊天消息。
* **字段类型**: `Boolean`
* **默认值**: `False`

## EnableGeoIP  
是否根据玩家的IP在加入时宣布玩家的地理位置。
* **字段类型**: `Boolean`
* **默认值**: `False`

## EnableTokenEndpointAuthentication  
是否要求使用公共REST API端点时进行令牌认证。
* **字段类型**: `Boolean`
* **默认值**: `False`

## EnableWhitelist  
根据whitelist.txt文件中的IP地址启用或禁用白名单。
* **字段类型**: `Boolean`
* **默认值**: `False`

## ForceHalloween  
强制全年开启万圣节专属活动。
* **字段类型**: `Boolean`
* **默认值**: `False`

## ForceTime  
强制世界时间设置为正常、白天或夜晚(normal, day, or night)。
* **字段类型**: `String`
* **默认值**: `normal`

## ForceXmas  
强制全年开启圣诞节专属活动。
* **字段类型**: `Boolean`
* **默认值**: `False`

## GiveItemsDirectly  
如果设置为true，将直接将给予玩家的物品插入到他们的背包中。需要SSC。否则，给予玩家的物品将作为掉落物品生成。实验性功能。可能无法正确工作或导致物品丢失
* **字段类型**: `Boolean`
* **默认值**: `False`

## HardcoreBanReason  
硬核模式玩家死亡被封禁时时显示的被封禁原因。
* **字段类型**: `String`
* **默认值**: `Death results in a ban`

## HardcoreKickReason  
硬核模式玩家死亡被踢出时显示的被踢出原因。
* **字段类型**: `String`
* **默认值**: `Death results in a kick`

## HardcoreOnly  
防止硬核模式玩家连接游戏。
* **字段类型**: `Boolean`
* **默认值**: `False`

## HealOtherThreshold  
如果在1秒内发送此数量的HealOtherPlayer数据包，则禁用该玩家。
* **字段类型**: `Int32`
* **默认值**: `50`

## IgnoreChestStacksOnLoad  
允许在世界加载期间，箱子中的物品堆叠数量超过堆叠上限。
* **字段类型**: `Boolean`
* **默认值**: `False`

## IgnoreProjKill  
忽略检查玩家是否“可以”摧毁射弹。
* **字段类型**: `Boolean`
* **默认值**: `False`

## IgnoreProjUpdate  
忽略检查玩家是否“可以”更新射弹。
* **字段类型**: `Boolean`
* **默认值**: `False`

## InfiniteInvasion  
启用永无止境的入侵事件。你仍然需要启动事件，例如使用 /invade 命令。
* **字段类型**: `Boolean`
* **默认值**: `False`

## InvasionMultiplier  
确定入侵事件的大小。计算入侵大小的公式是100 + (multiplier * (active players with greater than 200 health的数量))
* **字段类型**: `Int32`
* **默认值**: `1`

## KickEmptyUUID  
踢出没有向服务器发送其UUID的客户端。
* **字段类型**: `Boolean`
* **默认值**: `False`

## 伤害阈值超限时踢出玩家  
当用户超过最大伤害阈值时是否踢出。
* **字段类型**: `Boolean`
* **默认值**: `False`

## 硬核模式死亡时踢出  
硬核模式玩家死亡时是否踢出。
* **字段类型**: `Boolean`
* **默认值**: `False`

## 治疗他人阈值超限时踢出  
当用户超过治疗他人阈值时是否踢出。
* **字段类型**: `Boolean`
* **默认值**: `False`

## 中核模式死亡时踢出  
中核模式玩家死亡时是否踢出。
* **字段类型**: `Boolean`
* **默认值**: `False`

## 抛射物阈值超限时踢出  
当用户超过射弹阈值时是否踢出。
* **字段类型**: `Boolean`
* **默认值**: `False`

## 方块破坏阈值超限时踢出  
当用户超过方块破坏阈值时是否踢出。
* **字段类型**: `Boolean`
* **默认值**: `False`

## 方块液体操作阈值超限时踢出  
当用户超过方块液体操作阈值时是否踢出。
* **字段类型**: `Boolean`
* **默认值**: `False`

## 方块涂色阈值超限时踢出  
当用户超过方块涂色阈值时是否踢出。
* **字段类型**: `Boolean`
* **默认值**: `False`

## 方块放置阈值超限时踢出  
当用户超过方块放置阈值时是否踢出。
* **字段类型**: `Boolean`
* **默认值**: `False`

## KickProxyUsers  
如果启用了GeoIP，将踢出被检测到使用代理的用户。
* **字段类型**: `Boolean`
* **默认值**: `True`

## LogPath  
日志应写入的目录路径。
* **字段类型**: `String`
* **默认值**: `tshock/logs`

## LogRest  
是否记录REST API连接。
* **字段类型**: `Boolean`
* **默认值**: `False`

## MaxDamage  
玩家/NPC可造成的最大伤害值。
* **字段类型**: `Int32`
* **默认值**: `1175`

## MaxHP  
玩家在装备增益之前可以拥有的最大HP。
* **字段类型**: `Int32`
* **默认值**: `500`

## MaximumLoginAttempts  
踢出玩家之前的失败登录尝试次数。
* **字段类型**: `Int32`
* **默认值**: `3`

## MaxMP  
玩家在装备增益之前可以拥有的最大MP。
* **字段类型**: `Int32`
* **默认值**: `200`

## MaxProjDamage  
射弹可以造成的最大伤害
* **字段类型**: `Int32`
* **默认值**: `1175`

## MaxRangeForDisabled  
被禁用玩家可以移动的最大距离，以物块为单位。
* **字段类型**: `Int32`
* **默认值**: `10`

## MaxSlots  
最大同时连接的客户端数量。  
如果你希望玩家在服务器满员时被踢出并显示“服务器已满”，请将此值设置为你希望的最大玩家数量，然后将泰拉瑞亚的最大玩家数量设置为比此值高2。
* **字段类型**: `Int32`
* **默认值**: `8`

## MediumcoreBanReason  
中核模式玩家死亡被封禁时显示的被封禁原因。
* **字段类型**: `String`
* **默认值**: `Death results in a ban`

## MediumcoreKickReason  
中核模式玩家死亡被踢出时显示的被封禁原因。
* **字段类型**: `String`
* **默认值**: `Death results in a kick`

## MediumcoreOnly  
阻止软核玩家连接服务器。
* **字段类型**: `Boolean`
* **默认值**: `False`

## MinimumPasswordLength  
新用户帐户的最小密码长度。不能少于4。
* **字段类型**: `Int32`
* **默认值**: `4`

## MySqlDbName  
使用MySQL作为数据库类型时连接的数据库名称。
* **字段类型**: `String`
* **默认值**: ``

## MySqlHost  
用于将连接定向到MySQL主机名和端口。
* **字段类型**: `String`
* **默认值**: `localhost:3306`

## MySqlPassword  
连接到MySQL数据库时使用的密码。
* **字段类型**: `String`
* **默认值**: ``

## MySqlUsername  
连接到MySQL数据库时使用的用户名。
* **字段类型**: `String`
* **默认值**: ``

## PreventBannedItemSpawn  
防止禁止物品被生成或通过命令给予。
* **字段类型**: `Boolean`
* **默认值**: `False`

## PreventDeadModification  
禁止玩家在死亡状态下与游戏世界进行交互。
* **字段类型**: `Boolean`
* **默认值**: `True`

## PreventInvalidPlaceStyle  
防止玩家放置具有无效风格的物块。
* **字段类型**: `Boolean`
* **默认值**: `True`

## ProjectileThreshold  
若玩家在1秒内生成的射弹数量达到此值，则将其禁用。
* **字段类型**: `Int32`
* **默认值**: `50`

## ProjIgnoreShrapnel  
是否在射弹阈值计数中忽略水晶子弹的碎片。
* **字段类型**: `Boolean`
* **默认值**: `True`

## PvPMode  
设置PvP模式。有效类型包括："normal"（正常）、"always"（始终开启）和"disabled"（禁用）。
* **字段类型**: `String`
* **默认值**: `normal`

## RangeChecks  
根据玩家与其放置的方块之间的距离，启用或禁用反作弊距离检查。
* **字段类型**: `Boolean`
* **默认值**: `True`

## RegionProtectChests  
区域保护是否应用于箱子。
* **字段类型**: `Boolean`
* **默认值**: `False`

## RegionProtectGemLocks  
区域保护是否应用于宝石锁。
* **字段类型**: `Boolean`
* **默认值**: `True`

## RememberLeavePos  
根据玩家的IP地址记录其退出时的位置（服务器重启后不保留）。  
例如：当您尝试断开连接并重新连接时，通常会被自动回到到出生点，但此时您仍会停留在上次离开时的位置。
* **字段类型**: `Boolean`
* **默认值**: `False`

## RequireLogin  
要求所有玩家在被允许玩游戏之前注册或登录。
* **字段类型**: `Boolean`
* **默认值**: `False`

## ReservedSlots  
超出服务器最大玩家数的预留位数量（这些预留位仅允许预留玩家加入）。
* **字段类型**: `Int32`
* **默认值**: `20`

## RespawnBossSeconds  
当附近存在Boss时，玩家重生前必须等待的秒数。有效范围：0（默认值）至30秒。使用时风险自负。
* **字段类型**: `Int32`
* **默认值**: `0`

## RespawnSeconds  
玩家重生前必须等待的秒数。有效范围：0（默认值）至15秒。使用时风险自负。
* **字段类型**: `Int32`
* **默认值**: `0`

## RestApiEnabled  
启用或禁用REST API。
* **字段类型**: `Boolean`
* **默认值**: `False`

## RestApiPort  
REST API使用的端口。
* **字段类型**: `Int32`
* **默认值**: `7878`

## RESTMaximumRequestsPerInterval  
在拒绝请求前，桶中允许的最大REST请求数。最小值为5。
* **字段类型**: `Int32`
* **默认值**: `5`

## RESTRequestBucketDecreaseIntervalMinutes  
REST请求桶每分钟减少一次的频率。最小值是1分钟。
* **字段类型**: `Int32`
* **默认值**: `1`

## RevertToTextLogsOnSqlFailures  
在回退到文本日志之前，SQL日志插入失败必须达到的次数。
* **字段类型**: `Int32`
* **默认值**: `10`

## SaveWorldOnCrash  
如果服务器因未处理的异常而崩溃，是否保存世界。
* **字段类型**: `Boolean`
* **默认值**: `True`

## SaveWorldOnLastPlayerExit  
当最后一个玩家断开连接时，是否保存世界。
* **字段类型**: `Boolean`
* **默认值**: `True`

## ServerFullNoReservedReason  
当服务器已满且没有预留玩家空位可用时，踢出尝试加入的玩家的理由。
* **字段类型**: `String`
* **默认值**: `Server is full. No reserved slots open.`

## ServerFullReason  
当服务器已满时，踢出尝试加入的玩家的理由。
* **字段类型**: `String`
* **默认值**: `Server is full`

## ServerName  
若UseServerName设置为true，则在会话期间替换世界名称。
* **字段类型**: `String`
* **默认值**: ``

## ServerPassword  
加入服务器所需的服务器密码。
* **字段类型**: `String`
* **默认值**: ``

## ServerPort  
服务器运行的端口。
* **字段类型**: `Int32`
* **默认值**: `7777`

## ShowBackupAutosaveMessages  
是否显示备份自动保存消息。
* **字段类型**: `Boolean`
* **默认值**: `True`

## SoftcoreOnly  
禁止非软核玩家连接。
* **字段类型**: `Boolean`
* **默认值**: `False`

## SpawnProtection  
防止在重生保护范围的默认出生点内放置物块。
* **字段类型**: `Boolean`
* **默认值**: `True`

## SpawnProtectionRadius  
受重生保护范围设置保护的出生点周围的物块半径。
* **字段类型**: `Int32`
* **默认值**: `10`

## SqliteDBPath  
sqlite数据库的路径。
* **字段类型**: `String`
* **默认值**: `tshock.sqlite`

## StatueSpawn200  
一个雕像在200像素(?)(12.5格)内可以生成的NPC数量，之后才会停止生成。默认 = 3。
* **字段类型**: `Int32`
* **默认值**: `3`

## StatueSpawn600  
一个雕像在600像素(?)(37.5格)内可以生成的NPC数量，之后才会停止生成。默认 = 6。
* **字段类型**: `Int32`
* **默认值**: `6`

## StatueSpawnWorld  
一个雕像可以生成的NPC总数，之后才会停止生成。默认 = 10。
* **字段类型**: `Int32`
* **默认值**: `10`

## StorageType  
存储数据时使用的数据库类型（”sqlite” 或 “mysql”）。
* **字段类型**: `String`
* **默认值**: `sqlite`

## SuperAdminChatPrefix  
超级管理员聊天前缀。
* **字段类型**: `String`
* **默认值**: `(Super Admin) `

## SuperAdminChatRGB  
超级管理员组的聊天颜色。#.#.# = 红/绿/蓝 最大值：255
* **字段类型**: `Int32[]`
* **默认值**: `System.Int32[]`

## SuperAdminChatSuffix  
The superadmin chat suffix.
* **字段类型**: `String`
* **默认值**: ``

## SuppressPermissionFailureNotices  
服务器是否应该抑制来自区域、出生点或服务器编辑失败的构建权限失败警告。
* **字段类型**: `Boolean`
* **默认值**: `False`

## TileKillThreshold  
如果在1秒内方块破坏次数超过此数值，则禁用玩家并回滚其操作。
* **字段类型**: `Int32`
* **默认值**: `60`

## TileLiquidThreshold  
如果在1秒内液体放置次数超过此数值，则禁用该玩家。
* **字段类型**: `Int32`
* **默认值**: `50`

## TilePaintThreshold  
如果在1秒内涂色方块次数超过此数值，则禁用该玩家。
* **字段类型**: `Int32`
* **默认值**: `15`

## TilePlaceThreshold  
如果在1秒内方块放置次数超过此数值，则禁用玩家并回滚其操作。
* **字段类型**: `Int32`
* **默认值**: `32`

## UseServerName  
是否使用服务器名称（ServerName）替代世界名称。
* **字段类型**: `Boolean`
* **默认值**: `False`

## UseSqlLogs  
是否将日志保存到SQL数据库而不是文本文件。
* **字段类型**: `Boolean`
* **默认值**: `False`

## WhitelistKickReason  
当玩家因未加入白名单而被踢出时显示的原因。
* **字段类型**: `String`
* **默认值**: `You are not on the whitelist.`

