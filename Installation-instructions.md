Hello! Thanks for choosing TShock. As you walk through the installation procedure, note the 'Issues' tab at the top. If you have any problems at all, we can handle them there. If the problem is something you'd rather not share, or you don't want to create a [Github](http://github.com/) account, you can email your suggestions to _shank [at] shankshock [dot] com_.

## Downloading TShock
Click on the source tab at the top, and click the big downloads button on the right hand side. You want the download labeled latest.

## Preparation
### Patcher
Extract TerrariaServerPatcher.exe and TerrariaServerAPI.dll into where TerrariaServer.exe is located. Make sure TerrariaServer.exe is the original file, go into Steam and Validate Local Cache. Run TerrariaServerPatcher.exe as an admin and wait for the patching to complete.

You should have gotten a TerrariaServerAPI.exe as a result. Double click on it. When the server opens, allow it to create the folder 'serverplugins' and start it. Choose a world, type monkeybuisness (if you've been following this guide literally) for a password, use the default port, and let it launch. Type exit.
### Pre-Patcher
Start off by downloading the packages needed for your server installation.

* [TerrariaServerAPI.dll](https://github.com/TShock/TShock/blob/master/TShockAPI/TerrariaServerAPI.dll?raw=true)
* [TerrariaServer.exe](https://github.com/TShock/TShock/blob/master/TShockAPI/TerrariaServer.exe?raw=true)

Place those two files anywhere you would like to run your server from, and double click the server file. When the server opens, allow it to create the folder 'serverplugins' and start it. Choose a world, type monkeybuisness (if you've been following this guide literally) for a password, use the default port, and let it launch. Type exit.

## Installing TShock
Use [7Zip](http://7zip.org/) or another archiving program to extract the files downloaded in **Downloading TShock**.

Place TShockAPI.dll inside the 'serverplugins' folder. Place Newtonsoft.Json.Dll in the same folder as the server. Once again, start the server and verify that everything is running okay. Connect to your server by using the IP address 'localhost' and port 7777. *Important:* Take note of the auth code mentioned on the first startup. When you join, you need to become admin by typing /auth [code]. This code is displayed once, and as soon as the server is restarted you will lose the ability to run the command. You can edit users.txt in the future to add more admins, or you can remove the auth.lck file and restart the server to get another auth code to use.

## Finishing
Your server should now successfully start and run TShock. Familiarize yourself with the [[Commands]], and have a fun time administrating your new server.