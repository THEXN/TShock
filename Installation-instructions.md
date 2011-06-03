Hello! Thanks for choosing TShock. As you walk through the installation procedure, note the 'Issues' tab at the top. If you have any problems at all, we can handle them there. If the problem is something you'd rather not share, or you don't want to create a [Github](http://github.com/) account, you can email your suggestions to _shank [at] shankshock [dot] com_.

## Preparation
Start off by downloading the packages needed for your server installation.
* [TerrariaServerAPI.dll](https://github.com/TShock/TShock/blob/master/TShockAPI/TerrariaServerAPI.dll?raw=true)
* [TerrariaServer.exe](https://github.com/TShock/TShock/blob/master/TShockAPI/TerrariaServer.exe?raw=true)

Place those two files anywhere you would like to run your server from, and double click the server file. When the server opens, allow it to create the folder 'serverplugins' and start it. Choose a world, type monkeybuisness for a password, use the default port, and let it launch. Type exit.

## Installing TShock
Download TShock from GitHub. Click on the source tab at the top, and click the big downloads button on the right hand side. You want the download labeled latest. Once downloaded, use [7Zip](http://7zip.org/) or another archiving program to extract the files.

Place TShockAPI.dll inside the 'serverplugins' folder. Place Newtonsoft.Json.Dll in the same folder as the server. Once again, start the server and verify that everything is running okay. Connect to your server by using the IP address 'localhost' and port 7777. Take note of the IP address you're connecting from (Example: 127.0.0.1, *not with anything after the four octets*), you'll need it in a later step.

## Make an admin or two
Now that you have run the server once, a new folder called 'tshock' exists. Open that folder, and edit admins.txt so that the first line has your IP from the previous step in it. (Not an IPv6 IP, or an IP with a : at the end)

## Finishing
Your server should now successfully start and run TShock. Familiarize yourself with the [[Commands]], and have a fun time administrating your new server.