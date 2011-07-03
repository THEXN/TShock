Very simple configurations, but hasn't really been documented.
**User Made**

#Users.txt
So you'll wanna add users with special permissions (for permission configuration, check groups.txt; this is the documentation [Permissions and Groups Setup](https://github.com/TShock/TShock/wiki/Permissions))

Adding users is VERY easy.
**Here's an example with all groups**.

`484.15.185.12 superadmin`

`87.154.454.2 trustedadmin`

`541.012.24.1 admin`

`87.1.12.3 newadmin`

`1.1.1.2 vip`

**Note, superadmin is NOT a configurable group, and is HARD-CODED.**

So, to re-cap.
Add a user in this format:
`userip rank`

#Whitelisting

Whitelisting is SO easy.  To whitelist a player, enter in an IP.  Remember, you should skip lines to avoid confusion.
**Example**
`192.168.1.0`

`192.168.1.1`

`192.168.1.2`

`192.168.1.3`

`192.168.1.4`

**You can't edit the whitelist file while the server is online, so whitelist like this.**
As an admin, or someone with the permission to whitelist, go in-game and type `/whitelist <ip>`

#Item bans
Banning items means they won't be able to use em' on the server.

To ban items, all you have to do is enter the item ID's into the file.
[TShock Item ID's](https://github.com/TShock/TShock/wiki/Item-List)

`123`

`164`

`232`

#Bans/Blacklisting
**Note, if you have Whitelist on, you can just remove their IP.  But banning is probably easier.**
The ban system is mostly all in-game stuff.  
To ban, just the player, type `/ban <playername>`
To ban an IP, type `/banip <playername>`

Bans will look like this in your bans.txt
75.93.34.103|ImNotANutter|Misbehaviour.  -  Banned through /ban - **Currently, this IP bans**
imnotanutter||Manually added IP address ban.  - Banned through /banip - **Doesn't work, stick with the /ban**

#Conclusion
Uh, so yeah.  If there's anything missing, add it in.

**User created ;)**