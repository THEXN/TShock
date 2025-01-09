Soporte a Español por: [FrankV22_GitHub](https://github.com/itsFrankV22) [Discord](https://discord.gg/qdBSvagzPz
)

## AllowIce  
**Tipo:** Booleano  
**Descripción:** Permite colocar hielo incluso si el usuario no tiene permisos de construcción  
**Predeterminado:** "False"  

## AllowLoginAnyUsername  
**Tipo:** Booleano  
**Descripción:** Permite a los usuarios iniciar sesión con cualquier nombre de usuario usando /login  
**Predeterminado:** "True"  

## AllowRegisterAnyUsername  
**Tipo:** Booleano  
**Descripción:** Permite a los usuarios registrarse con cualquier nombre de usuario usando /register  
**Predeterminado:** "False"  

## AutoSave  
**Tipo:** Booleano  
**Descripción:** Habilitar/Deshabilitar el guardado automático integrado de Terraria  
**Predeterminado:** "True"  

## BackupInterval  
**Tipo:** Int32  
**Descripción:** Frecuencia de respaldo en minutos. Por ejemplo, un valor de 60 = 60 minutos. Los respaldos se guardan en la carpeta \tshock\backups.  
**Predeterminado:** "0"  

## BackupKeepFor  
**Tipo:** Int32  
**Descripción:** Tiempo en minutos que se conservan los respaldos. 2880 = 2 días.  
**Predeterminado:** "60"  

## BanOnMediumcoreDeath  
**Tipo:** Booleano  
**Descripción:** Prohíbe a un jugador de Hardcore al morir.  
**Predeterminado:** "False"  

## BufferPackets  
**Tipo:** Booleano  
**Descripción:** Almacena temporalmente los paquetes y los envía al final de cada cuadro  
**Predeterminado:** "True"  

## ChatFormat  
**Tipo:** Cadena  
**Descripción:** Cambiar el formato del chat en el juego, {0} = Nombre del Grupo, {1} = Prefijo del Grupo, {2} = Nombre del Jugador, {3} = Sufijo del Grupo, {4} = Mensaje del Chat  
**Predeterminado:** "{1}{2}{3}: {4}"  

## DefaultGuestGroupName  
**Tipo:** Cadena  
**Descripción:** Selecciona el nombre del grupo predeterminado para asignar a los usuarios no registrados  
**Predeterminado:** "guest"  

## DefaultMaximumSpawns  
**Tipo:** Int32  
**Descripción:** El máximo predeterminado de mobs que aparecerán por ola. Un número mayor significa más mobs en esa ola.  
**Predeterminado:** "5"  

## DefaultRegistrationGroupName  
**Tipo:** Cadena  
**Descripción:** Selecciona el nombre del grupo predeterminado para asignar a nuevos registrados  
**Predeterminado:** "default"  

## DefaultSpawnRate  
**Tipo:** Int32  
**Descripción:** El retraso entre las olas. Valores más bajos generan menos mobs.  
**Predeterminado:** "600"  

## DisableBuild  
**Tipo:** Booleano  
**Descripción:** Desactiva cualquier construcción; colocación de bloques  
**Predeterminado:** "False"  

## DisableClownBombs  
**Tipo:** Booleano  
**Descripción:** Desactiva la aparición de proyectiles de bombas de payasos  
**Predeterminado:** "False"  

## DisableDungeonGuardian  
**Tipo:** Booleano  
**Descripción:** Desactiva la aparición del Guardián de la Mazmorra generado por paquetes del jugador, en su lugar, fuerza una reaparición  
**Predeterminado:** "False"  

## DisableHardmode  
**Tipo:** Booleano  
**Descripción:** Desactiva el modo difícil, nunca se puede activar. Sobrescribe /starthardmode  
**Predeterminado:** "False"  

## DisableInvisPvP  
**Tipo:** Booleano  
**Descripción:** Desactiva el uso de pociones de invisibilidad en PvP (Nota: pueden usarlas en el cliente, pero el efecto no se envía al resto del servidor)  
**Predeterminado:** "False"  

## DisableLoginBeforeJoin  
**Tipo:** Booleano  
**Descripción:** Deshabilita que los usuarios inicien sesión con contraseña de cuenta al unirse  
**Predeterminado:** "False"  

## DisablePlayerCountReporting  
**Tipo:** Booleano  
**Descripción:** Desactiva el reporte del número de jugadores al sistema de estadísticas.  
**Predeterminado:** "False"  

## DisableSnowBalls  
**Tipo:** Booleano  
**Descripción:** Desactiva la aparición de proyectiles de bolas de nieve  
**Predeterminado:** "False"  

## DisableSpewLogs  
**Tipo:** Booleano  
**Descripción:** Desactiva de forma forzada los registros enviados a jugadores con permiso de registro  
**Predeterminado:** "True"  

## DisableTombstones  
**Tipo:** Booleano  
**Descripción:** Desactiva las lápidas para todos los jugadores.  
**Predeterminado:** "True"

## DisplayIPToAdmins  
**Tipo:** Booleano  
**Descripción:** Muestra la IP de un jugador al unirse a todos los que tienen el permiso de registro  
**Por Defecto:** "False"  

## EnableBanOnUsernames  
**Tipo:** Booleano  
**Descripción:** Permite expulsar a los usuarios prohibidos al coincidir con su Nombre de Personaje  
**Por Defecto:** "False"  

## EnableDNSHostResolution  
**Tipo:** Booleano  
**Descripción:** Activa la resolución DNS de conexiones entrantes con GetGroupForIPExpensive.  
**Por Defecto:** "False"  

## EnableGeoIP  
**Tipo:** Booleano  
**Descripción:** Anuncia la ubicación de un jugador al unirse  
**Por Defecto:** "False"  

## EnableInsecureTileFixes  
**Tipo:** Booleano  
**Descripción:** Algunos bloques se "arreglan" al no permitir que TShock los maneje. Desactivarlo puede romper ciertos bloques estéticos.  
**Por Defecto:** "True"  

## EnableIPBans  
**Tipo:** Booleano  
**Descripción:** Permite expulsar a usuarios prohibidos al coincidir con su dirección IP  
**Por Defecto:** "True"  

## EnableTokenEndpointAuthentication  
**Tipo:** Booleano  
**Descripción:** Activa un requisito de token para el punto final de la API /status.  
**Por Defecto:** "False"  

## EnableWhitelist  
**Tipo:** Booleano  
**Descripción:** Activa o desactiva la lista blanca basada en direcciones IP en whitelist.txt  
**Por Defecto:** "False"  

## ForceTime  
**Tipo:** Cadena  
**Descripción:** Forzar el tiempo del mundo a normal, día o noche  
**Por Defecto:** "normal"  

## HardcoreOnly  
**Tipo:** Booleano  
**Descripción:** Solo jugadores de modo Hardcore. Los jugadores de modo Softcore no pueden unirse.  
**Por Defecto:** "False"  

## HashAlgorithm  
**Tipo:** Cadena  
**Descripción:** Tipos válidos son "sha512", "sha256", "md5", agregue "-xp" para los algoritmos compatibles con xp  
**Por Defecto:** "sha512"  

## IgnoreNoClip  
**Tipo:** Booleano  
**Descripción:** Ignora todas las verificaciones de no clip para jugadores  
**Por Defecto:** "False"  

## IgnoreProjKill  
**Tipo:** Booleano  
**Descripción:** Ignora la verificación para ver si un jugador puede matar un proyectil  
**Por Defecto:** "False"  

## IgnoreProjUpdate  
**Tipo:** Booleano  
**Descripción:** Ignora la verificación para ver si un jugador puede actualizar un proyectil  
**Por Defecto:** "False"  

## InfiniteInvasion  
**Tipo:** Booleano  
**Descripción:** Permite que el tamaño de la invasión nunca disminuya. Asegúrese de ejecutar /invade, y tenga en cuenta que esto agrega más de 2 millones de goblins a la cola de generación para el mapa.  
**Por Defecto:** "False"  

## InvasionMultiplier  
**Tipo:** Int32  
**Descripción:** La ecuación para calcular el tamaño de la invasión es 100 + (multiplicador * (número de jugadores activos con más de 200 de vida))  
**Por Defecto:** "1"  

## KickOnMediumcoreDeath  
**Tipo:** Booleano  
**Descripción:** Expulsa a un jugador de modo Hardcore al morir.  
**Por Defecto:** "False"  

## KickProxyUsers  
**Tipo:** Booleano  
**Descripción:** Expulsa a usuarios que usan un proxy identificado con la base de datos GeoIP  
**Por Defecto:** "True"  

## LogonDiscardThreshold  
**Tipo:** Int32  
**Descripción:** Tiempo, en milisegundos, para deshabilitar el descarte de objetos después de iniciar sesión cuando ServerSideInventory está ACTIVADO  
**Por Defecto:** "250"  

## MasterServer  
**Tipo:** Cadena  
**Descripción:** No implementado  
**Por Defecto:** "127.0.0.1"  

## MaxDamage  
**Tipo:** Int32  
**Descripción:** El daño máximo que un jugador/NPC puede infligir  
**Por Defecto:** "175"  

## MaximumLoginAttempts  
**Tipo:** Int32  
**Descripción:** Número de intentos fallidos de inicio de sesión antes de expulsar al jugador.  
**Por Defecto:** "3"  

## MaxProjDamage  
**Tipo:** Int32  
**Descripción:** El daño máximo que un proyectil puede infligir  
**Por Defecto:** "175"  

## MaxRangeForDisabled  
**Tipo:** Int32  
**Descripción:** La distancia máxima que los jugadores deshabilitados por diversas razones pueden moverse  
**Por Defecto:** "10"  

## MaxSlots  
**Tipo:** Int32  
**Descripción:** Máximo de espacios para el servidor. Si desea que las personas sean expulsadas con "Servidor lleno", configure esto al número máximo de jugadores deseados y luego configure el máximo de jugadores de Terraria a 2 más.  
**Por Defecto:** "8"  

## MediumcoreBanReason  
**Tipo:** Cadena  
**Descripción:** Prohíbe a un jugador de modo Mediumcore al morir.  
**Por Defecto:** "La muerte resulta en una prohibición"  

## MediumcoreKickReason  
**Tipo:** Cadena  
**Descripción:** Expulsa a un jugador de modo Mediumcore al morir.  
**Por Defecto:** "La muerte resulta en una expulsión"  

## MediumcoreOnly  
**Tipo:** Booleano  
**Descripción:** Solo jugadores de modo Mediumcore. Los jugadores de modo Softcore no pueden unirse.  
**Por Defecto:** "False"  

## MySqlDbName  
**Tipo:** Cadena  
**Descripción:** Nombre de la base de datos a conectar  
**Por Defecto:** ""  

## MySqlHost  
**Tipo:** Cadena  
**Descripción:** Nombre de host y puerto MySQL para conexiones directas  
**Por Defecto:** "localhost:3306"  

## MySqlPassword  
**Tipo:** Cadena  
**Descripción:** Contraseña de la base de datos para conectar  
**Por Defecto:** ""  

## MySqlUsername  
**Tipo:** Cadena  
**Descripción:** Nombre de usuario de la base de datos para conectar  
**Por Defecto:** ""  

## ProjectileThreshold  
**Tipo:** Int32  
**Descripción:** Deshabilita a un jugador si supera este número de proyectiles nuevos en 1 segundo.  
**Por Defecto:** "50"  

## ProjIgnoreShrapnel  
**Tipo:** Booleano  
**Descripción:** Ignorar metralla de balas de cristal para el umbral de proyectiles.  
**Por Defecto:** "True"  

## PvPMode  
**Tipo:** Cadena  
**Descripción:** Establece el modo PvP del servidor. Tipos válidos: "normal", "always", "disabled"  
**Por Defecto:** "normal"  

## RangeChecks  
**Tipo:** Booleano  
**Descripción:** Agente de protección global para cualquier verificación de distancia de bloque basada en anti-grief.  
**Por Defecto:** "True"  

## RconPassword  
**Tipo:** Cadena  
**Descripción:** No implementado  
**Por Defecto:** ""  

## RconPort  
**Tipo:** Int32  
**Descripción:** No implementado  
**Por Defecto:** "7777"  

## RegionProtectChests  
**Tipo:** Booleano  
**Descripción:** Protege cofres con permisos de región y construcción  
**Por Defecto:** "False"  

## RememberLeavePos  
**Tipo:** Booleano  
**Descripción:** Recuerda dónde dejó un jugador. Funciona recordando la IP, NO el personaje.  
Ejemplo: Cuando intenta desconectarse y reconectarse para ser colocado automáticamente en el punto de inicio, estará en su última ubicación. Nota: No se guarda después de reiniciar el servidor.  
**Por Defecto:** "False"  

## RequireLogin  
**Tipo:** Booleano  
**Descripción:** Requiere que todos los jugadores se registren o inicien sesión antes de poder jugar.  
**Por Defecto:** "False"  

## RestApiEnabled  
**Tipo:** Booleano  
**Descripción:** Activa/Desactiva la API REST.  
**Por Defecto:** "False"  

## RestApiPort  
**Tipo:** Int32  
**Descripción:** Este es el puerto en el que la API REST escuchará.  
**Por Defecto:** "7878"  

## SaveWorldOnCrash  
**Tipo:** Booleano  
**Descripción:** Guarda el mundo si Terraria se bloquea debido a una excepción no controlada.  
**Por Defecto:** "True"  

## ServerFullNoReservedReason  
**Tipo:** Cadena  
**Descripción:** Cadena utilizada al expulsar personas cuando el servidor está lleno sin espacios reservados.  
**Por Defecto:** "Servidor lleno. No hay espacios reservados abiertos."  

## ServerFullReason  
**Tipo:** Cadena  
**Descripción:** Cadena utilizada al expulsar personas cuando el servidor está lleno.  
**Por Defecto:** "Servidor lleno"  

## ServerName  
**Tipo:** Cadena  
**Descripción:** Utilizado al responder a una solicitud REST /status.  
**Por Defecto:** ""  

## ServerNickname  
**Tipo:** Cadena  
**Descripción:** Esto se utiliza cuando se consulta el punto final de la API /status.  
**Por Defecto:** "Servidor TShock"  

## ServerPassword  
**Tipo:** Cadena  
**Descripción:** Contraseña del servidor requerida para unirse al servidor  
**Por Defecto:** ""  

## ServerPort  
**Tipo:** Int32  
**Descripción:** El puerto en el que se ejecuta el servidor.  
**Por Defecto:** "7777"  

## ServerSideInventory  
**Tipo:** Booleano  
**Descripción:** Habilitar verificaciones de inventario del lado del servidor, EXPERIMENTAL  
**Por Defecto:** "False"  

## SpawnProtection  
**Tipo:** Booleano  
**Descripción:** Evita que se coloquen bloques dentro del radio de SpawnProtection del punto de inicio predeterminado.  
**Por Defecto:** "True"  

## SpawnProtectionRadius  
**Tipo:** Int32  
**Descripción:** Radio desde el punto de inicio para SpawnProtection.  
**Por Defecto:** "10"  

## StorageType  
**Tipo:** Cadena  
**Descripción:** Tipos válidos: "sqlite" y "mysql"  
**Por Defecto:** "sqlite"  

## SuperAdminChatPrefix  
**Tipo:** Cadena  
**Descripción:** Prefijo de chat del grupo de superadministradores  
**Por Defecto:** "(Admin) "  

## SuperAdminChatRGB  
**Tipo:** Single[]  
**Descripción:** #.#.#. = Rojo/Azul/Verde - Colores RGB para el color del chat de Administrador. Valor máximo: 255  
**Por Defecto:** "System.Single[]"  

## SuperAdminChatSuffix  
**Tipo:** Cadena  
**Descripción:** Sufijo de chat del grupo de superadministradores  
**Por Defecto:** ""  

## TileKillThreshold  
**Tipo:** Int32  
**Descripción:** Deshabilita/Revertir un jugador si supera este número de bloques destruidos en 1 segundo.  
**Por Defecto:** "60"  

## TileLiquidThreshold  
**Tipo:** Int32  
**Descripción:** Deshabilita a un jugador si supera este número de ajustes de líquido en 1 segundo.  
**Por Defecto:** "15"  

## TilePlaceThreshold  
**Tipo:** Int32  
**Descripción:** Deshabilita/Revertir un jugador si supera este número de colocaciones de bloques en 1 segundo.  
**Por Defecto:** "20"  
