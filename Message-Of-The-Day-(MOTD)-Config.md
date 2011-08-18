Using TShock, a more powerful Message Of The Day system is provided than what is currently implemented by default Terraria. In the **\tshock** folder resides a **motd.txt** configuration file that allows you to edit the message of the day to contain any message you want.

The following variables can be used inside your message:

* **%map%** - This displays the name of your world that your server is running.
* **%players%** - This lists the name of all the players who are currently logged into your server.

Example:

    Welcome to my server!
    Current map: %map%
    Current players: %players%

You may also change the color of the lines displayed by using the %r,g,b% variable. Replace the "r", "g", and "b" with 3-digit numeric values from 0-255, with 0 having no saturation and 255 having maximum saturation.

* **%255,000,000%** = Red
* **%000,255,000%** = Green
* **%000,000,255%** = Blue

([View more RGB color codes here](http://www.tayloredmktg.com/rgb/))

Example:

    %255,000,000%Welcome to my server!
    %255,000,000%Current map: %map%
    %000,255,000%Current players: %players%