Hello! Thanks for choosing TShock. As you walk through the installation procedure, note the 'Issues' tab at the top. If you have any problems at all, we can handle them there. If the problem is something you'd rather not share, or you don't want to create a [Github](http://github.com/) account, you can email your suggestions to _shank [at] shankshock [dot] com_.

[Click here for a YouTube tutorial on how to install TShock.](http://www.youtube.com/watch?v=MpKexsNmefs)

## Step One - Download and Extract

Get the latest package of TShock from the TShock github page, located at [[https://github.com/TShock/TShock/archives/master]]. Extract all the files in the archive to a new folder, from which the server will be run. Everything is already included.

Next, download the API TShock runs on: [[http://dl.dropbox.com/u/29760911/TerrariaServerAPI.rar]]

Once downloaded, copy an existing version of TerrariaServer (1.0.5) into the folder, along with the patcher. Run the patcher, and proceed to the next step.

## Install
In the new folder you have made, run TerrariaServer.exe, It will automatically create all the necessary files/folders on startup.
Now close the TerrariaServer.exe , you've just generated the necessary files and folders.
Move the **TShockAPI.dll** and **XNAHelpers.dll** to the ServerPlugins folder and run the TerrariaServer.exe again. All other DLL's stay in the base folder.

## Become an Admin
Once the server starts the console window will give an auth code.
Join the server and enter /auth [authcode]. This will give you the ability to create a user account on the server, giving you access to all commands. 

Enter /user add [username]:[password] superadmin. This will create a new user account with superadmin privileges. 
After logging in with /login [username] [password] you have access to all commands and can add more users.

## Finishing
Your server should now be successfully running TShock. Familiarize yourself with the [[Commands]], and have a fun time administrating your new server.

