Using TShock, a more powerful message of the day system is provided than what is currently implemented inside Terraria server. For this reason, in '/tshock', there resides a configuration file that allows you to edit the message of the day to contain any message you want, in any order.

If you would like to insert the current map name, simply add %map% to the file where you want the map name. If you want to add the players, use %players%.

If you want to change the color of a line, you can prefix the line with a rgb code:

%r,g,b%

Each value must be 3 characters long, so for values less than one hundred, use preceding zeros to make each value total to three characters.

Example:

%255,010,000%