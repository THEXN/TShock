## Enabled  
启用服务器端角色功能，使客户端数据存储在服务器而非客户端。
* **字段类型**: `Boolean`
* **默认值**: `False`

## LogonDiscardThreshold  
当启用“服务器端角色”（ServerSideCharacters）功能时，登录后禁止丢弃物品的时间（以毫秒为单位）。
* **字段类型**: `Int32`
* **默认值**: `250`

## ServerSideCharacterSave  
服务器端角色（SSC）的自动保存间隔（以分钟为单位）。
* **字段类型**: `Int32`
* **默认值**: `5`

## StartingHealth  
启用SSC功能时，新玩家创建角色时的初始默认生命值。
* **字段类型**: `Int32`
* **默认值**: `100`

## StartingInventory  
启用SSC功能时，新玩家创建角色时的初始默认物品栏配置。
* **字段类型**: `List`1`
* **默认值**: `System.Collections.Generic.List`1[TShockAPI.NetItem]`

## StartingMana  
启用SSC功能时，新玩家创建角色时的初始默认魔力值。
* **字段类型**: `Int32`
* **默认值**: `20`

## WarnPlayersAboutBypassPermission  
当玩家拥有`tshock.ignore.ssc`权限（可绕过SSC系统）但SSC数据库中仍存在其角色数据时，向玩家和服务器控制台发送警告信息。
* **字段类型**: `Boolean`
* **默认值**: `True`

