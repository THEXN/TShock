## AllowAllowedGroupsToSpawnBannedItems  
Permite que los grupos que tengan permiso de usar un objeto baneado lo generen (por ejemplo, a través del comando /item) incluso aunque el `PreventBannedItemSpawn` esté definido en true.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `False`

## AllowCorruptionCreep  
Permite que la corrupción se extienda cuando el mundo está en modo difícil.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `True`

## AllowCrimsonCreep  
Permite que el carmesí se extienda cuando el mundo está en modo difícil.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `True`

## AllowCutTilesAndBreakables  
Permite que los jugadores rompan bloques temporales (césped, jarrones, etc) donde no tienen permiso de construir.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `False`

## AllowHallowCreep  
Permite que la bendición se extienda cuando el mundo está en modo difícil.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `True`

## AllowIce  
Permite que el jugador coloque hielo donde no tiene permisos de construir.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `False`

## AllowLoginAnyUsername  
Permite a los usuarios autenticarse con cualquier cuenta incluso si su nombre de usuario no coincide con el de su personaje.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `True`

## AllowRegisterAnyUsername  
Permite a los usuarios registrar un nombre de usuario aunque no coincida con el nombre de su personaje.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `False`

## AnnounceSave  
Activa o desactiva los anuncios de guardado del mundo.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `True`

## AnonymousBossInvasions  
Muestra o no un mensaje cuando se invoca o se inicia una invasión.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `True`

## ApplicationRestTokens  
Un diccionario de los Tokens REST que aplicaciones externas podrían usar para realizar solicitudes a tu servidor.
* **Tipo de campo**: `Dictionary`2`
* **Predeterminado**: `System.Collections.Generic.Dictionary`2[System.String,Rests.SecureRest+TokenData]`

## AutoSave  
Activa o desactiva el guardado automático que que viene incluido con Terraria.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `True`

## BackupInterval  
Intervalo entre cada copia de seguridad del mundo, en minutos. Las copias de seguridad se guardan en la carpeta tshock/backups.
* **Tipo de campo**: `Int32`
* **Predeterminado**: `10`

## BackupKeepFor  
Establece cuanto tiempo se mantendrán las copias de seguridad en minutos.
Ej. 2880 = 2 dias.
* **Tipo de campo**: `Int32`
* **Predeterminado**: `240`

## BanOnHardcoreDeath  
Banear o no al jugador si mueren en hardcore (extremo).
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `False`

## BanOnMediumcoreDeath  
Banear o no al jugador si mueren en mediumcore (núcleo medio).
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `False`

## BCryptWorkFactor  
Determina el factor de trabajo que BCrypt usará. Si se sube, todas las contraseñas se actualizarán al nuevo factor de trabajo cuando se verifiquen. La cantidad de rondas computacionales es 2^n. Sube este valor con precaución. El rango es de 5 a 31.
* **Tipo de campo**: `Int32`
* **Predeterminado**: `7`

## BombExplosionRadius  
Determina el rango en bloques que una bomba puede destruir desde el punto de detonación.
* **Tipo de campo**: `Int32`
* **Predeterminado**: `5`

## BroadcastRGB  
Los valores RGB que se usarán para el color de los mensajes del comando /broadcast.
#.#.# = Red/Blue/Green
Valor máximo: 255
* **Tipo de campo**: `Int32[]`
* **Predeterminado**: `System.Int32[]`

## ChatAboveHeadsFormat  
Cambia el nombre del jugador cuando se usa el chat encima de la cabeza. Empieza con el nombre del jugador entre corchetes, según el formato de Terraria.
Mismo formato que ChatFormat sin el mensaje.
* **Tipo de campo**: `String`
* **Predeterminado**: `{2}`

## ChatFormat  
Cambia el formato del chat en el juego: {0} = Nombre del grupo, {1} = Prefix del grupo, {2} = Nombre del jugador, {3} = Suffix del grupo, {4} = Mensaje de chat.
* **Tipo de campo**: `String`
* **Predeterminado**: `{1}{2}{3}: {4}`

## CommandSilentSpecifier  
Indica que carácter ejecutará un comando de forma silenciosa.
Nota: Esto no funcionará correctamente si hay más de 1 carácter.
* **Tipo de campo**: `String`
* **Predeterminado**: `.`

## CommandSpecifier  
Indica que carácter se usará para ejecutar comandos.
Nota: Esto no funcionará correctamente si hay más de 1 carácter.
* **Tipo de campo**: `String`
* **Predeterminado**: `/`

## DebugLogs  
Establece si el servidor debería mostrar o no mensajes debug acerca de operaciones del sistema.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `False`

## DefaultGuestGroupName  
El nombre del grupo predeterminado para los jugadores sin registrar o sin autenticar.
* **Tipo de campo**: `String`
* **Predeterminado**: `guest`

## DefaultMaximumSpawns  
El número predeterminado de mobs que se generarán por cada oleada. Cuanto más alto sea el número mas mobs habrán por oleada.
* **Tipo de campo**: `Int32`
* **Predeterminado**: `5`

## DefaultRegistrationGroupName  
El nombre del grupo predeterminado para darle a los usuarios recién registrados.
* **Tipo de campo**: `String`
* **Predeterminado**: `default`

## DefaultSpawnRate  
El delay entre cada oleada. Valores mas bajos harán que se generen mas mobs.
* **Tipo de campo**: `Int32`
* **Predeterminado**: `600`

## DisableBuild  
Desactiva la colocación o eliminación de cualquier tipo de bloque.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `False`

## DisableClownBombs  
Desactiva las bombas de los payasos, que dejarán de spawnear.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `False`

## DisableCustomDeathMessages  
Te permite desactivar o activar la protección contra la creación de mensajes de muertes personalizados al morir. Creado para desarrolladores que tuvieron una idea más original para solucionar este problema.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `True`

## DisableDefaultIPBan  
Desactiva los bans por IP de forma predeterminada, en caso de que no se den argumentos al comando para banear.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `False`

## DisableDungeonGuardian  
Previene que el guardián de la mazmorra aparezca enviando a los jugadores a su punto de aparición en su lugar.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `False`

## DisableHardmode  
Si se activa, el modo difícil no se activará por el Muro de Carne o el comando /hardmode.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `False`

## DisableInvisPvP  
Desactiva el efecto de las pociones de invisibilidad cuando el PvP está activado haciendo que el jugador sea visible para el resto de clientes.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `False`

## DisableLoginBeforeJoin  
Previene que los usuarios puedan autenticarse antes de que hayan completado la conexión.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `False`

## DisableModifiedZenith  
Prohíbe el uso del proyectil de la Cénit con diferentes objetos en lugar de armas.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `False`

## DisablePrimeBombs  
Impide que las bombas del Skeletron Prime aparezcan, útil para prevenir destrucción del mundo no deseada en en la semilla for the worthy.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `False`

## DisableSecondUpdateLogs  
Previene que las comprobaciones en OnSecondUpdate sean escritas al archivo de log.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `False`

## DisableSnowBalls  
Impide que los proyectiles de bolas de nieve se generen.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `False`

## DisableSpewLogs  
Desactiva el envío de logs como mensajes a jugadores que tengan permiso para ver logs.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `True`

## DisableTombstones  
Desactiva la caída de tumbas cuando cualquier jugador muera.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `True`

## DisableUUIDLogin  
Previene que los usuarios puedan autenticarse usando el UUID de su cliente.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `False`

## DisplayIPToAdmins  
Muestra la IP de un jugador al unirse a los usuarios que tengan permiso para ver logs.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `False`

## EnableChatAboveHeads  
Mostrar o no mensajes de chat encima de la cabeza de los jugadores.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `False`

## EnableGeoIP  
Anunciar o no la ubicación geográfica de un jugador al unirse, basado en su IP.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `False`

## EnableTokenEndpointAuthentication  
Requerir o no autenticación del token para usar los endpoints públicos de la REST API.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `False`

## EnableWhitelist  
Activa o desactiva la whitelist basado en la lista de direcciones IP en el archivo whitelist.txt.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `False`

## ForceHalloween  
Fuerza que los eventos que solo ocurren en Halloween ocurran todo el año.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `False`

## ForceTime  
Fuerza el tiempo del mundo a ser normal, dia o noche.  
Nota: usar `normal` para que el tiempo corra como lo haría normalmente, `day` para que sea siempre de día y `night` para que sea siempre de noche
* **Tipo de campo**: `String`
* **Predeterminado**: `normal`

## ForceXmas  
Fuerza que los eventos que solo ocurren en Navidad ocurran todo el año.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `False`

## GiveItemsDirectly  
Si esta establecido en true, los objetos datos a los jugadores se insertarán directamente en su inventario, Requiere SSC. En caso contrario, los objetos dados a los jugadores aparecerán como objetos dropeados. Esto es experimental. Podría no funcionar correctamente o resultar en pérdidas de objetos. 
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `False`

## HardcoreBanReason  
La razón que se dará cuando un jugador sea baneado por morir en extremo.
* **Tipo de campo**: `String`
* **Predeterminado**: `Death results in a ban`

## HardcoreKickReason  
La razón que se dará cuando un jugador sea expulsado por morir en  extremo.
* **Tipo de campo**: `String`
* **Predeterminado**: `Death results in a kick`

## HardcoreOnly  
Impide que los jugadores que no estén en extremo se conecten al servidor.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `False`

## HealOtherThreshold  
Deshabilita al jugador si envía esta cantidad de paquetes de HealOtherPlayer en 1 segundo.
* **Tipo de campo**: `Int32`
* **Predeterminado**: `50`

## IgnoreChestStacksOnLoad  
Permite que el stack de objetos en cofres sobrepase el límite durante la carga del mundo.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `False`

## IgnoreProjKill  
Ignora las comprobaciones para ver cuando un jugador 'puede' matar un proyectil.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `False`

## IgnoreProjUpdate  
Ignora las comprobaciones para ver si el jugador 'puede' actualizar un proyectil.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `False`

## InfiniteInvasion  
Si se activa las invasiones no se acabarán nunca. Aun tienes que iniciar el evento, como por ejemplo usando el comando /worldevent invasion.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `False`

## InvasionMultiplier  
Determina el tamaño de las invasiones.
La ecuación para calcular el tamaño de la invasión es 100 + (multiplicador * (número de jugadores activos con mas de 200 de vida)).
* **Tipo de campo**: `Int32`
* **Predeterminado**: `1`

## KickEmptyUUID  
Expulsa a los clientes que no envían su UUID al servidor.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `False`

## KickOnDamageThresholdBroken  
Expulsar o no a los jugadores cuando sobrepasan el límite de daño establecido en `MaxDamage`.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `False`

## KickOnHardcoreDeath  
Expulsar o no a los jugadores al morir en extremo.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `False`

## KickOnHealOtherThresholdBroken  
Expulsar o no a los usuarios cuando sobrepasan el límite de curación a otros jugadores.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `False`

## KickOnMediumcoreDeath  
Expulsar o no a los jugadores al morir en mediumcore (núcleo medio).
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `False`

## KickOnProjectileThresholdBroken  
Expulsar o no a los usuarios cuando sobrepasan el límite de creación de proyectiles.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `False`

## KickOnTileKillThresholdBroken  
Expulsar o no a los usuarios cuando sobrepasen el límite de eliminación de bloques.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `False`

## KickOnTileLiquidThresholdBroken  
Expulsar o no a los usuarios cuando sobrepasen el límite de manipulación de líquidos.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `False`

## KickOnTilePaintThresholdBroken  
Expulsar o no a los usuarios cuando sobrepasen el límite de pintar bloques.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `False`

## KickOnTilePlaceThresholdBroken  
Expulsar o no a los usuarios cuando sobrepasen el límite de colocación de bloques.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `False`

## KickProxyUsers  
Si GeoIP está activado, esto expulsará a los usuarios que hayan sido identificados de estar usando un proxy.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `True`

## LogPath  
La ruta al directorio donde se guardarán los logs.
* **Tipo de campo**: `String`
* **Predeterminado**: `tshock/logs`

## LogRest  
Guardar o no logs sobre las conexiones de la REST API.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `False`

## MaxDamage  
El daño máximo que un jugador puede provocar a NPCs o otros jugadores.
* **Tipo de campo**: `Int32`
* **Predeterminado**: `1175`

## MaxHP  
La vida máxima que un jugador puede tener, sin contar los buffs.
* **Tipo de campo**: `Int32`
* **Predeterminado**: `500`

## MaximumLoginAttempts  
El número máximo de intentos que tiene un jugador para autenticarse antes de ser expulsado.
* **Tipo de campo**: `Int32`
* **Predeterminado**: `3`

## MaxMP  
El maná máximo que un jugador puede tener, sin contar los buffs.
* **Tipo de campo**: `Int32`
* **Predeterminado**: `200`

## MaxProjDamage  
El daño máximo que un proyectil puede provocar.
* **Tipo de campo**: `Int32`
* **Predeterminado**: `1175`

## MaxRangeForDisabled  
La distancia máxima, en bloques, desde donde un jugador deshabilitado se puede mover.
* **Tipo de campo**: `Int32`
* **Predeterminado**: `10`

## MaxSlots  
El número máximo de clientes conectados a la vez.
Si quieres que las personas sean expulsadas con el mensaje "El servidor está lleno", establece esto en cuantos jugadores quieres como máximo y establece la cantidad máxima de jugadores en Terraria en 2 o mayor.
* **Tipo de campo**: `Int32`
* **Predeterminado**: `8`

## MediumcoreBanReason  
La razón que se dará a los jugadores baneados por morir en mediumcore (núcleo medio).
* **Tipo de campo**: `String`
* **Predeterminado**: `Death results in a ban`

## MediumcoreKickReason  
La razón que se dará a los jugadores expulsados por morir en mediumcore (núcleo medio).
* **Tipo de campo**: `String`
* **Predeterminado**: `Death results in a kick`

## MediumcoreOnly  
Impide que los jugadores en clásico se conecten al servidor.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `False`

## MinimumPasswordLength  
El tamaño mínimo que puede tener una contraseña para cuentas de usuario nuevas. Este valor nunca podrá ser menor a 4.
* **Tipo de campo**: `Int32`
* **Predeterminado**: `4`

## MySqlDbName  
El nombre de la base de datos para conectarse en caso de que se este usando una base de datos MySQL.
* **Tipo de campo**: `String`
* **Predeterminado**: ``

## MySqlHost  
El nombre de host y puerto para conectarse a la base de datos MySQL.
* **Tipo de campo**: `String`
* **Predeterminado**: `localhost:3306`

## MySqlPassword  
La contraseña usada para conectarse a la base de datos MySQL.
* **Tipo de campo**: `String`
* **Predeterminado**: ``

## MySqlUsername  
El nombre de usuario usado para conectarse a la base de datos MySQL.
* **Tipo de campo**: `String`
* **Predeterminado**: ``

## PreventBannedItemSpawn  
Previene que se generen o se den objetos baneados con comandos.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `False`

## PreventDeadModification  
Previene que los jugadores interactúen con el mundo mientras están muertos.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `True`

## PreventInvalidPlaceStyle  
Previene que los jugadores coloquen bloques con un PlaceStyle inválido.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `True`

## ProjectileThreshold  
Deshabilita al jugador si crea este número de proyectiles en 1 segundo.
* **Tipo de campo**: `Int32`
* **Predeterminado**: `50`

## ProjIgnoreShrapnel  
Ignorar o no los fragmentos de las balas de cristal para el límite de proyectiles por segundo establecido en `ProjectileThreshold`.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `True`

## PvPMode  
Establece el modo de pvp. Los modos válidos son: "normal", "always", "disabled" y "pvpwithnoteam".  
Si se establece "normal" los jugadores podrán cambiar su PvP libremente, en "always" tendrán el PvP siempre activado, en "disabled" lo tendrán siempre desactivado y en "pvpwithnoteam" tendrán el PvP siempre activo y no podrán cambiar de equipo.
* **Tipo de campo**: `String`
* **Predeterminado**: `normal`

## RangeChecks  
Activa o desactiva las comprobaciones de rango del anti-cheat basado en la distancia entre el jugador y sus colocaciones de bloques.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `True`

## RegionProtectChests  
Decide si la protección de las regiones debería aplicar o no a los cofres.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `False`

## RegionProtectGemLocks  
Decide si la protección de las regiones debería aplicar o no a los Cierres de gemas.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `True`

## RememberLeavePos  
Recuerda donde un jugador se desconectó, basado en su IP. No se mantiene entre los reinicios del servidor.
Ejemplo: Cuando te desconectes, al volver a conectarte estarás en el lugar donde saliste del servidor en lugar del spawn.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `False`

## RequireLogin  
Requiere que todos los jugadores se autentiquen antes de poder jugar.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `False`

## ReservedSlots  
El número de slots reservados mas allá de los slots máximos donde se pueden unir jugadores reservados.
* **Tipo de campo**: `Int32`
* **Predeterminado**: `20`

## RespawnBossSeconds  
El número de segundos que un jugador debe esperar antes de respawnear si hay un jefe cerca. Rango permitido: 0 (predeterminado) hasta 30 segundos. Úsalo bajo tu propio riesgo.
* **Tipo de campo**: `Int32`
* **Predeterminado**: `0`

## RespawnSeconds  
El número de segundos que un jugador debe esperar antes de respawnear. Rango permitido: 0 (predeterminado) hasta 15 segundos. Úsalo bajo tu propio riesgo.
* **Tipo de campo**: `Int32`
* **Predeterminado**: `0`

## RestApiEnabled  
Activa o desactiva la REST API.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `False`

## RestApiPort  
El puerto utilizado por la REST API.
* **Tipo de campo**: `Int32`
* **Predeterminado**: `7878`

## RESTMaximumRequestsPerInterval  
El máximo de solicitudes REST en el bucket antes de denegar solicitudes. El valor mínimo es 5.
* **Tipo de campo**: `Int32`
* **Predeterminado**: `5`

## RESTRequestBucketDecreaseIntervalMinutes  
Cada cuantos minutos el bucket de solicitudes REST se reduce en 1. El valor mínimo es 1 minuto.
* **Tipo de campo**: `Int32`
* **Predeterminado**: `1`

## RevertToTextLogsOnSqlFailures  
Número de veces que los logs SQL deben fallar antes de volver a usar el log del archivo de texto.
* **Tipo de campo**: `Int32`
* **Predeterminado**: `10`

## SaveWorldOnCrash  
Guardar o no el mundo en caso de que el server crashee debido a una excepción no controlada.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `True`

## SaveWorldOnLastPlayerExit  
Guardar o no el mundo cuando el último jugador se desconecta.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `True`

## ServerFullNoReservedReason  
La razón dada al expulsar jugadores que intenten entrar cuando el servidor está lleno y no quedan slots reservados disponibles.
* **Tipo de campo**: `String`
* **Predeterminado**: `Server is full. No reserved slots open.`

## ServerFullReason  
La razón dada al expulsar jugadores que intenten entrar cuando el servidor está lleno.
* **Tipo de campo**: `String`
* **Predeterminado**: `Server is full`

## ServerName  
Reemplaza el nombre del mundo durante la sesión si UserServerName está establecido en true.
* **Tipo de campo**: `String`
* **Predeterminado**: ``

## ServerPassword  
La contraseña requerida para entrar al servidor.
* **Tipo de campo**: `String`
* **Predeterminado**: ``

## ServerPort  
El puerto que usará el servidor.
* **Tipo de campo**: `Int32`
* **Predeterminado**: `7777`

## ShowBackupAutosaveMessages  
Mostrar o no mensajes de cuando se guarda una copia de seguridad del mundo automáticamente.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `True`

## SoftcoreOnly  
Impide que jugadores que no estén en clásico se conecten.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `False`

## SpawnProtection  
Previene que se coloquen bloques en el área de protección del spawn establecida en SpawnProtectionRadius.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `True`

## SpawnProtectionRadius  
El radio de bloques alrededor del spawn que están protegidos por la opción de configuración SpawnProtection.
* **Tipo de campo**: `Int32`
* **Predeterminado**: `10`

## SqliteDBPath  
La ruta de la base de datos SQLite.
* **Tipo de campo**: `String`
* **Predeterminado**: `tshock.sqlite`

## StatueSpawn200  
Cuantos NPCs puede generar una estatua en un radio de 200 pixels(?) antes de que deje de generarlos.
Default = 3.
* **Tipo de campo**: `Int32`
* **Predeterminado**: `3`

## StatueSpawn600  
Cuantos NPCs puede generar una estatua en un radio de 600 pixels(?) antes de que deje de generarlos.
Default = 6.
* **Tipo de campo**: `Int32`
* **Predeterminado**: `6`

## StatueSpawnWorld  
Cuantos NPCs puede generar una estatua antes de que deje de generarlos.
Default = 10.
* **Tipo de campo**: `Int32`
* **Predeterminado**: `10`

## StorageType  
El tipo de base de datos que se usará para almacenar datos (puede ser "sqlite" o "mysql").
* **Tipo de campo**: `String`
* **Predeterminado**: `sqlite`

## SuperAdminChatPrefix  
El prefix para el grupo superadmin.
* **Tipo de campo**: `String`
* **Predeterminado**: `(Super Admin) `

## SuperAdminChatRGB  
El color de chat para el grupo superadmin.
#.#.# = Red/Blue/Green
Valor máximo: 255
* **Tipo de campo**: `Int32[]`
* **Predeterminado**: `System.Int32[]`

## SuperAdminChatSuffix  
El suffix del chat para el grupo superadmin.
* **Tipo de campo**: `String`
* **Predeterminado**: ``

## SuppressPermissionFailureNotices  
Decide si el servidor debería o no dejar de enviar avisos acerca de falta de permisos de regiones, punto de spawn, o edición del server.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `False`

## TileKillThreshold  
Deshabilita un jugador y revierte sus acciones si este número de bloques eliminados en 1 segundo se sobrepasa.
* **Tipo de campo**: `Int32`
* **Predeterminado**: `60`

## TileLiquidThreshold  
Deshabilita al jugador si este número de manipulación de líquidos en 1 segundo se sobrepasa.
* **Tipo de campo**: `Int32`
* **Predeterminado**: `50`

## TilePaintThreshold  
Deshabilita al jugador si este número de bloques pintados en 1 segundo se sobrepasa.
* **Tipo de campo**: `Int32`
* **Predeterminado**: `15`

## TilePlaceThreshold  
Deshabilita al jugador y revierte sus acciones si este número de bloques colocados en 1 segundo se sobrepasa.
* **Tipo de campo**: `Int32`
* **Predeterminado**: `32`

## UseServerName  
Usar o no el nombre del servidor establecido en `ServerName` en lugar del nombre del mundo.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `False`

## UseSqlLogs  
Guardar o no los logs en la base de datos SQL en lugar de un archivo de texto.
Predeterminado = false.
* **Tipo de campo**: `Boolean`
* **Predeterminado**: `False`

## WhitelistKickReason  
La razón data a los jugadores cuando sean expulsados por no estar en la whitelist.
* **Tipo de campo**: `String`
* **Predeterminado**: `You are not on the whitelist.`
