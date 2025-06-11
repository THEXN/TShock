## BanCreateV3  
创建一个新封禁记录。  
* **权限**：`tshock.rest.bans.manage`  

**参数**：  
* `identifier`（必填）`String` - 要封禁的标识符（如玩家名称/IP/UUID）。  
* `reason`（可选）`String` - 封禁原因。  
* `start`（可选）`String` - 封禁开始时间（日期时间格式）。  
* `end`（可选）`String` - 封禁结束时间（日期时间格式）。  
* `token`（必填）`String` - REST 身份验证令牌。  

**示例用法**：`/v3/bans/create?identifier=目标标识符&reason=封禁原因&start=开始时间&end=结束时间&token=令牌`  


## BanDestroyV3  
删除现有封禁记录。  
* **权限**：`tshock.rest.bans.manage`  

**参数**：  
* `ticketNumber`（必填）`String` - 要删除的封禁记录编号。  
* `fullDelete`（可选）`Boolean` - 是否从系统完全移除封禁记录。  
* `token`（必填）`String` - REST 身份验证令牌。  

**示例用法**：`/v3/bans/destroy?ticketNumber=记录编号&fullDelete=true&token=令牌`  


## BanInfoV3  
查看特定封禁记录详情。  
* **权限**：`tshock.rest.bans.view`  

**参数**：  
* `ticketNumber`（必填）`String` - 要查询的封禁记录编号。  
* `token`（必填）`String` - REST 身份验证令牌。  

**示例用法**：`/v3/bans/read?ticketNumber=记录编号&token=令牌`  


## BanListV3  
查看 TShock 数据库中的所有封禁记录。  
* **权限**：`tshock.rest.bans.view`  

**参数**：  
* `token`（必填）`String` - REST 身份验证令牌。  

**示例用法**：`/v3/bans/list?token=令牌`  


## GroupCreate  
创建新用户组。  
* **权限**：`tshock.rest.groups.manage`  

**参数**：  
* `group`（必填）`String` - 新用户组名称。  
* `parent`（可选）`String` - 父组名称（可选）。  
* `permissions`（可选）`String` - 权限列表（逗号分隔）。  
* `chatcolor`（可选）`String` - 聊天颜色（RGB 格式字符串）。  
* `token`（必填）`String` - REST 身份验证令牌。  

**示例用法**：`/v2/groups/create?group=新组名&parent=父组名&permissions=权限1,权限2&chatcolor=255,0,0&token=令牌`  


## GroupDestroy  
删除用户组。  
* **权限**：`tshock.rest.groups.manage`  

**参数**：  
* `group`（必填）`String` - 要删除的用户组名称。  
* `token`（必填）`String` - REST 身份验证令牌。  

**示例用法**：`/v2/groups/destroy?group=组名&token=令牌`  


## GroupInfo  
显示用户组信息。  
* **权限**：`tshock.rest.groups.view`  

**参数**：  
* `group`（必填）`String` - 要查询的用户组名称。  
* `token`（必填）`String` - REST 身份验证令牌。  

**示例用法**：`/v2/groups/read?group=组名&token=令牌`  


## GroupList  
查看 TShock 数据库中的所有用户组。  
* **权限**：`tshock.rest.groups.view`  

**参数**：  
* `token`（必填）`String` - REST 身份验证令牌。  

**示例用法**：`/v2/groups/list?token=令牌`  


## PlayerKickV2  
踢出玩家。  
* **权限**：`tshock.rest.kick`  

**参数**：  
* `player`（必填）`String` - 要踢出的玩家名称/ID。  
* `reason`（可选）`String` - 踢出原因。  
* `token`（必填）`String` - REST 身份验证令牌。  

**示例用法**：`/v2/players/kick?player=玩家名&reason=违规原因&token=令牌`  


## PlayerKill  
杀死玩家。  
* **权限**：`tshock.rest.kill`  

**参数**：  
* `player`（必填）`String` - 要杀死的玩家名称/ID。  
* `from`（可选）`String` - 击杀来源（如“管理员”）。  
* `token`（必填）`String` - REST 身份验证令牌。  

**示例用法**：`/v2/players/kill?player=玩家名&from=管理员&token=令牌`  


## PlayerList  
列出当前在线玩家名称。  
无特殊权限要求。  

**参数**：  
* `token`（必填）`String` - REST 身份验证令牌。  

**示例用法**：`/lists/players?token=令牌`  


## PlayerListV2  
获取所有在线玩家的详细信息，可通过键值对过滤（键为字段名，值为字段值）。  
无特殊权限要求。  

**参数**：  
* `token`（必填）`String` - REST 身份验证令牌。  

**示例用法**：`/v2/players/list?token=令牌`  


## PlayerMute  
禁言玩家。  
* **权限**：`tshock.rest.mute`  

**参数**：  
* `player`（必填）`String` - 要禁言的玩家名称/ID。  
* `token`（必填）`String` - REST 身份验证令牌。  

**示例用法**：`/v2/players/mute?player=玩家名&token=令牌`  


## PlayerReadV3  
获取玩家信息。  
* **权限**：`tshock.rest.users.info`  

**参数**：  
* `player`（必填）`String` - 要查询的玩家名称/ID。  
* `token`（必填）`String` - REST 身份验证令牌。  

**示例用法**：`/v3/players/read?player=玩家名&token=令牌`  


## PlayerReadV4  
获取玩家信息。  
* **权限**：`tshock.rest.users.info`  

**参数**：  
* `player`（必填）`String` - 要查询的玩家名称/ID。  
* `token`（必填）`String` - REST 身份验证令牌。  

**示例用法**：`/v4/players/read?player=玩家名&token=令牌`  


## PlayerUnMute  
解除禁言玩家。  
* **权限**：`tshock.rest.mute`  

**参数**：  
* `player`（必填）`String` - 要解除禁言的玩家名称/ID。  
* `token`（必填）`String` - REST 身份验证令牌。  

**示例用法**：`/v2/players/unmute?player=玩家名&token=令牌`  


## ServerBroadcast  
全局广播消息。  
无特殊权限要求。  

**参数**：  
* `msg`（必填）`String` - 广播内容。  
* `token`（必填）`String` - REST 身份验证令牌。  

**示例用法**：`/v2/server/broadcast?msg=消息内容&token=令牌`  


## ServerCommandV3  
执行服务器远程命令并返回结果。  
* **权限**：`tshock.rest.command`  

**参数**：  
* `cmd`（必填）`String` - 要执行的命令及参数。  
* `token`（必填）`String` - REST 身份验证令牌。  

**示例用法**：`/v3/server/rawcmd?cmd=命令参数&token=令牌`  


## ServerMotd  
获取服务器 MOTD。  
无特殊权限要求。  

**参数**：  
* `token`（必填）`String` - REST 身份验证令牌。  

**示例用法**：`/v3/server/motd?token=令牌`  


## ServerOff  
关闭服务器。  
* **权限**：`tshock.rest.maintenance`  

**参数**：  
* `confirm`（必填）`Boolean` - 确认关机（需设为 `true`）。  
* `message`（可选）`String` - 关机提示消息。  
* `nosave`（可选）`Boolean` - 不保存直接关机。  
* `token`（必填）`String` - REST 身份验证令牌。  

**示例用法**：`/v2/server/off?confirm=true&message=关机维护&nosave=false&token=令牌`  


## ServerReload  
重新加载服务器配置文件。  
* **权限**：`tshock.rest.cfg`  

**参数**：  
* `token`（必填）`String` - REST 身份验证令牌。  

**示例用法**：`/v3/server/reload?token=令牌`  


## ServerRules  
获取服务器规则。  
无特殊权限要求。  

**参数**：  
* `token`（必填）`String` - REST 身份验证令牌。  

**示例用法**：`/v3/server/rules?token=令牌`  


## ServerStatusV2  
获取 TShock 服务器状态信息。  
无特殊权限要求。  

**参数**：  
* `token`（必填）`String` - REST 身份验证令牌。  

**示例用法**：`/v2/server/status?token=令牌`  


## ServerTokenTest  
验证令牌有效性。  
无特殊权限要求。  

**参数**：  
* `token`（必填）`String` - REST 身份验证令牌。  

**示例用法**：`/tokentest?token=令牌`  


## UserActiveListV2  
获取当前服务器中活跃的用户账户列表。  
* **权限**：`tshock.rest.users.view`  

**参数**：  
* `token`（必填）`String` - REST 身份验证令牌。  

**示例用法**：`/v2/users/activelist?token=令牌`  


## UserCreateV2  
创建 TShock 用户账户。  
* **权限**：`tshock.rest.users.manage`  

**参数**：  
* `user`（必填）`String` - 用户名。  
* `group`（可选）`String` - 所属用户组（可选）。  
* `password`（必填）`String` - 用户密码。  
* `token`（必填）`String` - REST 身份验证令牌。  

**示例用法**：`/v2/users/create?user=新用户&group=玩家组&password=密码&token=令牌`  


## UserDestroyV2  
删除 TShock 用户账户。  
* **权限**：`tshock.rest.users.manage`  

**参数**：  
* `user`（必填）`String` - 搜索条件（用户名或 ID）。  
* `type`（必填）`String` - 搜索类型（`name` 按名称，`id` 按 ID）。  
* `token`（必填）`String` - REST 身份验证令牌。  

**示例用法**：`/v2/users/destroy?user=目标用户&type=name&token=令牌`  


## UserInfoV2  
获取用户账户详细信息。  
* **权限**：`tshock.rest.users.view`  

**参数**：  
* `user`（必填）`String` - 搜索条件（用户名或 ID）。  
* `type`（必填）`String` - 搜索类型（`name` 按名称，`id` 按 ID）。  
* `token`（必填）`String` - REST 身份验证令牌。  

**示例用法**：`/v2/users/read?user=目标用户&type=id&token=令牌`  


## UserListV2  
列出 TShock 数据库中的所有用户账户。  
* **权限**：`tshock.rest.users.view`  

**参数**：  
* `token`（必填）`String` - REST 身份验证令牌。  

**示例用法**：`/v2/users/list?token=令牌`  


## UserUpdateV2  
更新用户账户信息。  
* **权限**：`tshock.rest.users.manage`  

**参数**：  
* `user`（必填）`String` - 搜索条件（用户名或 ID）。  
* `type`（必填）`String` - 搜索类型（`name` 按名称，`id` 按 ID）。  
* `password`（可选）`String` - 新密码（需至少提供密码或用户组之一）。  
* `group`（可选）`String` - 新用户组（需至少提供密码或用户组之一）。  
* `token`（必填）`String` - REST 身份验证令牌。  

**示例用法**：`/v2/users/update?user=目标用户&type=name&password=新密码&group=管理员组&token=令牌`  


## WorldBloodmoon  
切换血月状态。  
* **权限**：`tshock.rest.causeevents`  

**操作参数**：  
* `bloodmoon`（必填）`Boolean` - 血月状态（`true` 开启，`false` 关闭）。  

**参数**：  
* `token`（必填）`String` - REST 身份验证令牌。  

**示例用法**：`/world/bloodmoon/true?token=令牌`  


## WorldBloodmoonV3  
切换血月状态（V3 版本）。  
* **权限**：`tshock.rest.causeevents`  

**参数**：  
* `token`（必填）`String` - REST 身份验证令牌。  

**示例用法**：`/v3/world/bloodmoon?token=令牌`  


## WorldButcher  
击杀 NPC。  
* **权限**：`tshock.rest.butcher`  

**参数**：  
* `killfriendly`（可选）`Boolean` - 是否击杀友好 NPC。  
* `token`（必填）`String` - REST 身份验证令牌。  

**示例用法**：`/v2/world/butcher?killfriendly=true&token=令牌`  


## WorldMeteor  
在世界中生成陨石。  
* **权限**：`tshock.rest.causeevents`  

**参数**：  
* `token`（必填）`String` - REST 身份验证令牌。  

**示例用法**：`/world/meteor?token=令牌`  


## WorldRead  
获取世界信息。  
无特殊权限要求。  

**参数**：  
* `token`（必填）`String` - REST 身份验证令牌。  

**示例用法**：`/world/read?token=令牌`  


## WorldSave  
保存世界数据。  
* **权限**：`tshock.rest.cfg`  

**参数**：  
* `token`（必填）`String` - REST 身份验证令牌。  

**示例用法**：`/v2/world/save?token=令牌`  