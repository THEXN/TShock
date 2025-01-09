**Nota del Traductor:**
> Hola la verdad me da pereza entender los sistemas raros de pull request y cosas si si me equivoco en algo o hago algo mal porfa contactame! [Github](https://github.com/itsFrankV22) 
**Click-> <a href="https://discord.com/invite/qdBSvagzPz"><img alt="Discord" src="https://img.shields.io/badge/Discord-%235865F2.svg?&logo=discord&logoColor=white"></a>**

**Advertencia: La wiki se trasladará a Confluence pronto. Mientras realizamos la transición de las páginas, la edición estará restringida a aquellos con cuentas proporcionadas por Shank (@nicatronTg). Esta wiki será desactivada eventualmente, sin embargo, la edición se habilitará en Confluence cuando esto ocurra. (Ver http://ci.tshock.co:8080/)**

** ************ESTA PÁGINA ESTÁ DESACTUALIZADA****************** **
[Permisos actualizados](https://github.com/TShock/TShock/wiki/All-permissions)

TShock requiere que estés logueado como un usuario de nivel administrador para ejecutar la mayoría de los comandos. 
La mayoría de estos comandos pueden alterar significativamente la jugabilidad.

### Escribe /help 1-5 en el juego para ver el resto de los comandos. ###

### Lista completa de comandos:

## Comandos disponibles para todos
* /register [nombre de usuario] [contraseña] - Registrar una cuenta de usuario
* /login [nombre de usuario] [contraseña] - Iniciar sesión en una cuenta de usuario
* /help [página] - Proporciona una lista de comandos.
* /playing, /who, /online - Lista de jugadores en el servidor.
* /me - Chat en tercera persona.
* /p - Chat de equipo.
* /rules - Imprime las reglas del servidor.

## Comandos disponibles para usuarios registrados
* /password [contraseña anterior] [nueva contraseña] - Cambiar tu contraseña

## Comandos disponibles para grupos ##

### Permisos requeridos: Ingresado como grupo 'superadmin' ###

* /user add [nombre de cuenta:contraseña] [grupo] - Agrega una cuenta de usuario y asigna un grupo.
* /user add [IP.add.re.ss] [grupo] - Agrega una dirección IP de usuario y asigna un grupo.
* /user del [nombre de cuenta] - Elimina una cuenta de usuario
* /user del [IP.add.re.ss] - Elimina una dirección IP de usuario
* /user group [nombre de cuenta] [nuevo grupo] - Asigna un nuevo grupo a un usuario
* /user password [nombre de cuenta] [contraseña] - Asigna una nueva contraseña a un usuario
* /auth-verify [CÓDIGO] - Verifica las cuentas de superadmin iniciales. [CÓDIGO] se muestra en la consola del servidor.
* /ip [jugador] - Muestra de manera privada la dirección IP de un jugador

### Permisos requeridos: kick, ban, unban ###

* /kick [jugador] [razón] - Usa "comillas" para nombres de jugadores con espacios. (Ejemplo: /kick "sarah connor" abuso)
* /ban [jugador] [razón] - Usa "comillas" para nombres de jugadores con espacios.
* /ban [ip] [razón]
* /unban [jugador] - Usa "" para nombres de jugadores con espacios.
* /unbanip [ip]

### Permisos requeridos: mantenimiento ###

* /off - Apaga el servidor después de guardar.
* /off-nosave - Mata el servidor sin guardar.
* /checkupdates - Verifica si hay actualizaciones de TShock.
* /clearbans -1 - Elimina todos los baneos

### Permisos requeridos: whitelist ###

* /whitelist [jugador] - Mientras "EnableWhitelist: true" esté en config.json, solo los jugadores agregados a la lista blanca con este comando podrán entrar al servidor. Estos nombres se almacenan en \tshock\whitelist.txt.

### Permisos requeridos: causeevents ###

* /dropmeteor - Lanza un meteorito en el mapa.
* /star - Intenta traer una estrella a tu ubicación (requiere estar de noche).
* /bloodmoon - Inicia un evento de luna de sangre.
* /invade - Activa una invasión goblin.
* /itemdrop(/idrop) - Deja caer un ítem (o grupo de ítems) aleatoriamente por el cielo.

### Permisos requeridos: spawnboss ###

* /eater - Invoca al Eater of Worlds.
* /eye - Invoca al Eye of Cthulhu.
* /skeletron - Invoca a Skeletron.
* /king - Invoca al Slime King.
* /hardcore - Invoca al Eater of Worlds, Eye of Cthulhu, Slime King y Skeletron.

### Permisos requeridos: spawnmob ###

* /spawnmob [npc-id o nombre] [cantidad] - Invoca un NPC de la [[lista de NPCs]] - Usa "" para nombres de mobs con espacios.

### Permisos requeridos: tp ###

* /tp [jugador] - Teletransporta al jugador solicitado.
* /spawn, /home - Teletransporta a casa. (Cama o spawn)

### Permisos requeridos: tphere ###

* /tphere [jugador] - Teletransporta al jugador solicitado hacia ti.

### Permisos requeridos: warp ###

* /warp list - Lista los destinos de warp disponibles.
* /warp [destino] - Teletransporta al destino nombrado.

### Permisos requeridos: managewarp ###

* /setwarp [destino] - Establece un warp con nombre en tu posición actual.
* /delwarp [destino] - Elimina un warp con nombre.

### Permisos requeridos: managegroup ###

* /addgroup <nombre de grupo> [permisos opcionales, separados por espacio o coma, no ambos] - Permite agregar tipos de grupo adicionales, además de los grupos predeterminados (VIP, newadmin, trustedadmin, etc.).
 * Ejemplo: /addgroup tester ban kick heal 
 * O: /addgroup tester ban,kick,heal 
 * NO: /addgroup tester ban, kick, heal
* /delgroup <nombre de grupo> - Elimina un grupo existente.
* /modgroup add|del <nombre de grupo> <permiso para agregar o eliminar, solo uno a la vez, lo siento!> - <permiso> es el permiso requerido para usar un conjunto de comandos.
 * Ejemplo: /modgroup add vip managewarp

### Permisos requeridos: manageitem ###
* /addItem <nombre o id del ítem> - Agrega un ítem a la lista de baneados.
 * Ejemplo: /addItem "gold coin"
* /delItem <nombre o id del ítem> - Elimina un ítem existente de la lista de baneados.
 * Ejemplo: /delItem "gold coin"

### Permisos requeridos: cfg ###

* /setspawn - Establece el punto de spawn del mapa en tu posición actual.
* /reload - Recarga el archivo de configuración.
* /showconfig - Muestra la configuración actual.
* /serverpassword - Cambia la contraseña del servidor de manera remota.
* /save - Guarda el servidor.
* /maxspawns [cantidad] - Cambia la cantidad máxima de spawns por ola. Números más altos resultan en más mobs por ola de spawn.
* /spawnrate [cantidad] - Cambia la tasa de spawn de los monstruos. Menor número resulta en tasas de spawn más rápidas.
* /broadcast [mensaje] - Transmite un mensaje desde el <Servidor> nombre del jugador.
* /convert - Este comando eliminará todos los usuarios de ambas Regiones y Warps, además cambiará todos los Mundos para hacer referencia a este WorldID. Debes corregir manualmente las configuraciones de múltiples mundos.

### Permisos requeridos: time ###

* /time [día|noche|dusk|mediodía|medianoche] - Establece una hora específica del mundo.

### Permisos requeridos: pvpfun ###

* /slap [jugador] <daño> - Da una bofetada a [jugador], por defecto 5 de daño si no se proporciona - Usa "comillas" para nombres de jugadores con espacios.

### Permisos requeridos: editspawn ###

* /antibuild - Activa/desactiva la protección de construcción.
* /protectspawn - Activa/desactiva la protección del spawn.
* /region set [1/2] - Establece puntos temporales de región
* /region name - Muestra el nombre de la región en la que te encuentras
* /region define [nombre] - Agrega puntos temporales de región a la lista de regiones
* /region delete [nombre] - Elimina la región especificada
* /region allow [nombre] [nombre de región] - Permite a un jugador construir en la región especificada
* /region list - Muestra una lista con nombre de todas las regiones para ese mundo
* /region help - Muestra ayuda sobre regiones

### Permisos requeridos: logs ###
_Nota: Por defecto, los grupos con este permiso verán automáticamente todos los mensajes de la consola del servidor (comandos "/")._

* /displaylogs - Activa y desactiva los mensajes de la consola del servidor.

### Permisos requeridos: whisper ###
_Nota: los grupos con el permiso "logs" (arriba) podrán ver todos los comandos utilizados, y por lo tanto verán los susurros._

* /whisper, /w, /tell [jugador] [mensaje] - Envía un susurro privado a otro jugador
* /reply, /r [mensaje] - Responde al último susurro recibido.

### Permisos requeridos: annoy ###

* /annoy [jugador] [tiempo en segundos] - Reproduce el sonido de un cojín sonoro a un jugador durante [tiempo en segundos]. No puede ser escuchado por otros jugadores.

### Permisos requeridos: kill, butcher, item, heal ###

* /kill [jugador] - Mata al [jugador] proporcionado - Usa "" para nombres de ítems/jugadores con espacios.
* /butcher - Mata a todos los NPCs que no sean de pueblo.
* /item [id o nombre] <cantidad> o /i [id o nombre] <cantidad> - Te da [id o nombre] (Usa "comillas" para nombres con espacios)
 * Ejemplo: /i "molten pick"
* /give [id o nombre] [jugador] <cantidad> - Le da a [jugador] el ítem [id o nombre] (Usa "comillas" para ítems/nombres con espacios)
 * Ejemplo: /give "molten pick" "sarah connor" 1
* /heal [jugador] - Te cura a ti, o si se define, a [jugador] (Usa "comillas" para nombres de jugadores con espacios)

### Otros permisos ###
_Nota: Estos no son comandos en el juego, pero aún se pueden agregar a los permisos de un grupo._

* canwater - Permite que este grupo use agua
* canlava - Permite que este grupo use lava
* canspike - Permite que este grupo coloque picos
* canbuild - Permite que este grupo construya (no omite otras verificaciones, como regiones protegidas)
* adminchat - Colorea y agrega un prefijo al chat del grupo. Este prefijo y color se establecen en \tshock\config.json.
* immunetokick - El grupo no puede ser expulsado
* immunetoban - El grupo no puede ser baneado
* ignorecheatdetection - Permite que el grupo haga trampa (trampas de salud/mana) 
* ignoregriefdetection - Permite que el grupo haga grief (use explosivos, agua, lava aunque no tengan permiso para ello)
* usebanneditem - Permite que el grupo use ítems baneados
* reservedslot - Permite que este grupo se una al servidor, independientemente de si está lleno o no
