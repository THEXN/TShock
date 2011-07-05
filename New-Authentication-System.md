In previous versions of TShock, and in other server modifications, administrators were identified by IP addresses, which posed problems for users who had dynamic IP addresses, and for administrators uncomfortable with an entire household having administrative access on a server.

The new authentication system is both more secure and robust than the last system. First, we now accept usernames and passwords for each user, and secondly, passwords are stored in SHA512 format, a good trade off without adding salt to something such as SHA1.

The new auth file format is:

    username:sha512-hash group

Because there is a need for backwards compatability, existing administrators will work until the transition to MySQL database is complete.

The following is a user that has the password "tshock" for the initial account creation:

    tshock:c35619528248455406596b8712e86ad3247d603f0449293abba0f739c8051742b15ab1429c23fbd8f185eba3de82239023ab5989c6a607ca580821a37f1c5499 superadmin