Los siguientes parámetros se pueden agregar a TShock para cambiar la forma en la que el servidor se inicializa. Las opciones especificadas en la línea de comandos tendrán prioridad sobre cualquiera de sus contrapartes en el archivo de configuración. Estos pueden ser para uso personal o en un entorno GSP para un alojamiento más sencillo y sin problemas

* `-ip` - Inicia el servidor vinculándose a una dirección IPv4. Por ejemplo: `-ip 0.0.0.0` vinculará a todas las interfaces. En cambio, `-ip 127.0.0.1` solo vinculará `127.0.0.1`. Si tu interfaz tiene asignada otra dirección IP, deberías ser capaz de vincularla.
* `-port` - Inicia el servidor vinculándose al puerto especificado. Por ejemplo `-port 7777` usará el puerto predeterminado para Terraria `-port 25565` usará el puerto predeterminado de Minecraft. Puertos por debajo de `1000` suelen requerir de privilegios de administrador o root para vincularse en la mayoría de sistemas operativos. También tendrás que permitir el acceso a este puerto desde tu firewall, y abrir este puerto.
* `-maxplayers`, o `-players` - Inicia el servidor con la cantidad máxima de jugadores especificada. Por ejemplo: `-maxplayers 5` definirá la cantidad máxima de jugadores en 5. El máximo teórico es que soporta Terraria es `255`. Puedes definir `-maxplayers 255` para esto.
* `-world` - Inicia el servidor y carga inmediatamente un archivo del mundo especificado. Nota: este parámetro requiere una ruta absoluta. Por ejemplo: `-world C:\Terraria\MyWorld.wld` o `-world /root/MyWorld.wld`.
* `-worldselectpath` - Inicia el servidor y cambia la ubicación de los mundos a la ruta especificada. El servidor buscará dentro de esta ruta los mundos para cargar. Por ejemplo: `-worldselectpath /root` o `-worldselectpath C:\Terraria`.
* `-worldname` - Inicia el servidor usando el nombre del mundo que exista en el parámetro `world select path`. Por ejemplo, si `MyWorld.wld` está en `C:\Terraria\` y `-worldselectpath C:\Terraria\` esta definido, entonces `-worldname MyWorld` cargará ese mundo.
* `-autocreate` - Inicia el servidor y, si el archivo del mundo no se ha encontrado, creará automáticamente el mundo con el tamaño especificado entre 1 y 3, 1 es pequeño, 2 mediano y 3 grande. Por ejemplo, si especificas `-autocreate 3` se creará un mundo grande.
* `-config` - Inicia el servidor con el archivo de configuración especificado. Por ejemplo, `-config C:\Terraria\config.json`.
* `-ignoreversion` - Ignora la versión de la API de los plugins, lo que permite que plugins antiguos se cargen.
* `-forceupdate` - Fuerza al servidor a seguir funcionando, y que no se quede hibernando cuando no hay ningún jugador conectado. Esto provoca que el tiempo siga pasando, crezca la hierba, y la CPU siga usándose.
* `-pass`, `-password` - Define una contraseña para el servidor. Por ejemplo, `-pass mypassword` definirá la contraseña del servidor en `mypassword`.
* `-motd` - Establece el MOTD base del juego. Por ejemplo, `-motd "¡Bienvenido a mi servidor!"` establecerá el MOTD como `¡Bienvenido a mi servidor!`.
* `-autoshutdown` - Activa la característica del juego de apagado automático, esto apagará automáticamente el servidor cuando se desconecten todos los jugadores.
* `-secure` - Activa la característica del juego base de "antispam".
* `-configpath` - Especifica la ruta que TShock usa para localizar archivos de configuración, logs, y bases de datos SQLite. Por ejemplo, `-configpath C:\Terraria\` definirá la ruta de configuración a `C:\Terraria\`.
* `-logpath` - Cambia la ruta predeterminada para los archivos de logs y los guardará en este lugar. Por ejemplo, `-logpath C:\Terraria\` establecerá la ruta de los logs en `C:\Terraria\`.
* `-logformat` - Cambia el formato de los archivos de logs, sujeto a las abreviaturas de fecha estándar en C#. Por ejemplo, `-logformat "yyyy-MM-dd"` creará un archivo de log nuevo cada día. El formato predeterminado es `yyyy-MM-dd_HH-mm-ss`.
* `-logclear` - Sobrescribe la configuración antigua en caso de que exista. Útil para docker. Por ejemplo, `-logclear`.
* `-dump` - Crea un dump (o archivo de volcado) de todos los permisos y descripciones del archivo de configuración para su uso como documentación.
* `-worldevil` - Establece la maldad del mundo (`random`, `corrupt`, o `crimson`). Esto solo afecta a mundos nuevos. Por ejemplo, `-worldevil random` establecerá el mundo en una maldad aleatoria.
* `-heaptile` - Ejecuta el servidor usando el Tile provider 'heaptile'. Heaptile es un proveedor ITile alternativo que usa menos memoria que el predeterminado. Esto es experimental y podría causar algunos problemas. Para usarlo agrega el parámetro `-heaptile`.
* `-constileation` o `-c` - Ejecuta el servidor con el Tile provider 'Constileation'. Constileation es un proveedor ITile alternativo que usa menos memoria que el predeterminado. Esto es experimental y podria causar problemas. Para usarlo agrega el parámetro `-constileation`.
* `-difficulty` - Establece la dificultad del mundo (`0` para `normal`, `1` para `experto`, `2` para `maestro`, `3` para `viaje`). Esto solo afecta a mundos nuevos.
* `-loadlib` - Indica al juego base que cargue una librería en la ruta especificada. Por ejemplo, `-loadlib C:\Terraria\MyLibrary.dll` cargará la librería `MyLibrary.dll` desde la ruta `C:\Terraria\`.
* `-crashdir` - Indica al juego base de guardar los volcados de memoria en la ruta especificada. Por ejemplo, `-crashdir C:\Terraria\CrashDumps` guardará los volcados de memoria en `C:\Terraria\CrashDumps`.
* `-additionalplugins` - Indica al servidor de cargar plugins en el directorio especificado, adicionalmente a los plugins en la carpeta `ServerPlugins`. Por ejemplo, `-additionalplugins C:\Terraria\MyPlugins` cargará plugins desde `C:\Terraria\MyPlugins`. Esto es útil (generalmente) cuando tienes varios servidores que usan los mismos plugins.

Estos parámetros de la línea de comandos son además de los que ya ofrece Terraria (por ejemplo, `-lang` es ahora un parametro vanilla, y sigue funcionando).

## Iniciar TShock automáticamente

Si quieres iniciar TShock automáticamente a través de un script, y omitir la línea de comandos interactiva al iniciar, tienes que especificar una ruta al mundo con el parámetro `-world`, el puerto con `-port`, y la cantidad de jugadores con `-maxplayers`.

Por ejemplo: `TShock.Server.exe -world C:\Terraria\worlds\MyWorld.wld -port 7777 -maxplayers 8`