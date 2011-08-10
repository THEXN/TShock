Hello! Thanks for choosing TShock. 

If you have any problems with this installation, post an issue on our [Forums](http://tshock.co/xf/index.php) and if you encounter an unusual error that looks like a bug please post it on our [Issue Tracker](http://ci.tshock.co:8080/secure/IssueNavigator.jspa?mode=hide&requestId=10001).

## Download and Extract
1. Download the latest package of TShock from the [TShock Downloads](https://github.com/TShock/TShock/downloads).
2. Download the [TShock API](http://dl.dropbox.com/u/29760911/TerrariaServerAPI.rar).
3. Extract all files from both archives into the same folder. In this example, we'll use **C:\TShock**.
4. Copy an existing version of TerrariaServer.exe into C:\TShock. If you don't have a working copy of TerrariaServer.exe, [download it here](http://www.mediafire.com/file/r72o6088tqusj0b/TerrariaServer.zip).

## Install
1. In C:\TShock, run **TerrariaServerPatcher.exe** as an Administrator (Right click -> Select "Run as Administrator")
 * _Note: a "TerrariaServer hash not found" error can be solved by retrieving a fresh TerrariaServer.exe from Step #4._
2. Wait until the program reports "Success", then close the program.

## Become an Admin
1. Run **TerrariaServerAPI.exe**. This is your server. TerrariaServer.exe should not be used.
2. Select the world you want to use or create a new one.
3. After selecting your world, you will be asked for input on 3 settings: Max players, Server port, and Server password.
 * _Note: Default settings are: Max players (8), Server port (7777), and Server password (none). You may hit your ENTER key at each input to select the default setting in parenthesis ()._
4. Once the server starts the console window will have the following message: **To become superadmin, join the game and type /auth [authcode]**
5. Run Terraria.exe, join your server, and enter: **/auth [authcode]**. This will give you the ability to create a user account on the server, giving you access to all commands. 
 * _Note: In order to join your own server, you can enter: **localhost** or **127.0.0.1** (which are the same thing - assuming the server is being run on the same machine that you run Terraria), or you can use your own External IP address, which can be found simply by going to a website such as [WhatIsMyIP.com](http://www.whatismyip.com/)._
6. Next, enter: **/user add [username]:[password] superadmin**
 * Example: **/user add john:qwerty superadmin** (remember to include the colon "**:**" between the username and password)
7. Log in with the superadmin user you just created by entering: **/login [username] [password]** (without a colon)
 * Example: **/login john qwerty**
8. Finally, to remove the "auth" message on server startup, enter: **/auth-verify**

## Adding Users/Admins
To add a new user or admin, enter the following: **/user add [username]:[password] [group]**

* Example #1 (Add a VIP user): **/user add sarah:connor vip**
* Example #2 (Add another Admin): **/user add sarah:connor newadmin**

Alternatively, if the user you wish to add has already registered (using /register [username] [password]) you may also edit that user's Usergroup in the **C:\TShock\tshock\tshock.sqlite** file (Users.Usergroup - see below). _Users who have their access added or changed need only re-join the server for their permissions to take effect._

## Finishing Up
Your server should now be successfully running TShock. Here are a few things you may like to do:

* Familiarize yourself with the [[Commands & Permissions]].
* Configure whatever options you choose in the **C:\TShock\tshock\config.json** file. For information on what each option does, check out [[Configuration file setup]].
 * _Note: you can open the config.json file with Notepad, or its superior, [Notepad++](http://notepad-plus-plus.org/)_
* Make your server noticeable! With your External IP address ([WhatIsMyIP.com](http://www.whatismyip.com/)), you can sign up to have your server listed on [TerrariaNet](http://terrarianet.com/), and you can have your own sub-forum (moderated by you) with [TerrariaZone](http://terrariazone.com/).
* Convert your External IP into a DNS: Using a free service like [No-IP.com](http://www.no-ip.com/), you can change your external IP address into a readable DNS name. In other words, instead of forcing your users to connect with 192.9.9.250, they can use "yourserver.no-ip.com".
 * _Protip: Because your IP will likely change once in a while, at some point your players will be unable to connect to you, and you may not even notice. You can download [No-IP's Dynamic DNS Client](http://www.no-ip.com/downloads.php) to keep your IP in sync with the DNS name you make with them._
* Get yourself acquainted with TShock's data storage within the **C:\TShock\tshock\tshock.sqlite** file. All users, regions, permissions, and other data is kept here. [Twitchy's TShock DB Editor](https://github.com/TShock/TShock/downloads) is very user-friendly and is consistently being updated, and [SQLite Manager](https://addons.mozilla.org/en-US/firefox/addon/sqlite-manager/) can be run directly through Firefox:
 1. Open Firefox, go to the Web Development Menu, and open SQLite Manager.
 2. In the menu at the top, select **Database** -> **Connect Database**
 3. Browse to **C:\TShock\tshock** and open **tshock.sqlite**. You may now click on **Tables** in the left panel, and double click on a database record in the **Browse & Search** tab to edit it.
**WARNING: Edit data with caution! Making mistakes inside this file can easily result in a corrupted database, which will prevent your server from functioning properly. Edit data at your own risk! It is highly recommended to make frequent backups of this file!**

## Frequently Asked Questions & Problems

* **Q: I get an error about a hash not being found!** A: You're running the patcher on an already patched or out of date version of the server.
* **Q: The process requires elevation!** A: Run the patcher as administrator or move the patcher and the server files to a folder you have write access to.
* **Q: Function/class not found!** A: Make sure XNAHelpers.dll is inside the ServerPlugins folder and TerrariaServerAPI.dll is in the same folder as the server itself. Also make sure you aren't intermingling old versions of XNAHelpers and TerrariaServerAPI.
* **Q: Linux?** A: We don't support it, as we're a mod and we have no desire to re-create the entire XNA libraries. Our biggest suggestion is to virtualize Windows Server 2008 R2 with .NET framework 4 and XNA 4.
* **Q: Encryption/Hash algorithm not supported on Windows XP.** A: In the configuration file, change the hash algorithm to sha256 or md5.