Soporte a Español por: [FrankV22_GitHub](https://github.com/itsFrankV22) [Discord](https://discord.gg/qdBSvagzPz
)

## Archivo groups.txt:  

> **Formato**  
> nombre padre permiso1 permiso2 permisoN  
> si no hay padre, pon null en su lugar  
> los grupos heredan permisos de sus padres  
> pon un ! antes de un permiso para negarlo  
> No elimines el grupo default  
> No nombres a un grupo SuperAdmin, ya que está codificado en el código y otorga todos los permisos  
> SIEMPRE DECLARA EL PADRE DE UN GRUPO ANTES DE DECLARAR EL GRUPO  

> **Permisos disponibles actualmente:**  
> reservedslot	        - ranura reservada para jugador  
> canwater		- permite a los jugadores usar agua  
> canlava		- permite a los jugadores usar lava  
> warp			- permite a los jugadores usar teletransportes  
> kick			- expulsar usuarios  
> ban			- prohibir jugadores por nombre o IP  
> unban			- desprohibir jugadores por nombre o IP  
> whitelist		- agregar IP a la lista blanca  
> maintenance	        - verificar actualizaciones/apagar el servidor  
> causeevents	        - permitir al jugador generar estrellas/meteoritos y activar invasiones/luna de sangre  
> spawnboss		- permitir al jugador invocar jefes  
> spawnmob		- permitir al jugador invocar cualquier NPC  
> tp			- permitir al jugador teletransportarse  
> tphere		- permitir a un jugador teletransportar a otros a su posición  
> managewarp		- permitir al jugador agregar/eliminar ubicaciones de teletransporte  
> editspawn		- permitir al jugador habilitar/deshabilitar protección de construcción  
> cfg			- permitir al jugador ver/cambiar la configuración de tshock  
> time			- permitir al jugador cambiar el tiempo  
> pvpfun		- habilitar comandos de diversión PvP  
> logs			- notificar al jugador cuando se ejecuta algún comando  
> kill			- permitir al jugador matar a otros  
> butcher		- permitir al jugador matar a todos los monstruos vivos  
> cheat			- permitir al jugador generar objetos y curarse  
> immunetokick	        - el jugador no puede ser expulsado  
> immunetoban	        - el jugador no puede ser baneado  
> ignorecheatdetection  - permitir al jugador hacer trampas (salud/mana)  
> ignoregriefdetection  - permitir al jugador hacer destrozos (usar explosivos, agua, lava, incluso si no tiene permiso)  
> usebanneditem         - permite al jugador usar objetos prohibidos  

> **Ejemplo de configuración de grupos:**  
> default null canwater  
> vip default reservedslot  
> newadmin default kick editspawn reservedslot  
> admin newadmin ban unban whitelist causeevents spawnboss spawnmob managewarp time tp pvpfun kill logs immunetokick tphere  
> trustedadmin admin maintenance cfg butcher cheat immunetoban ignorecheatdetection ignoregriefdetection usebanneditem  

> "guest, user, moderator, admin, owner, opadmin (*)"  