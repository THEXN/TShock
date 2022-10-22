TShock supports automatic language detection based on your system regional settings. If you want to override this, set the `TSHOCK_LANGUAGE` environment variable to a different language. Currently, only a limited number of [Toki Pona](https://tokipona.org/) translations are available, by setting `TSHOCK_LANGUAGE` to `tok`. Translation work is underway to translate TShock into different languages.

If you can't load other languages, make sure that you still have a folder called `i18n` and that your language folder of choice contains a `.mo` file. If it does, those translations should load successfully if you override the language settings using `TSHOCK_LANGUAGE` or if you set your regional settings to the right language.

This system is separate from Terraria's language system. You may still want to pass `-lang` on the command line to set the language for Terraria.

The base for the i18n system was built by Janet Blackquill ([@pontaoski](https://github.com/pontaoski)). A small donation in her honor was made to the [KDE project](https://kde.org/) as a thankyou for this work.

## Supported language projects

The following language projects are currently in-progress, with significant progress:

* Chinese (Simplified) (`zh_CN`)
* Russian (`ru_RU`)
* Spanish (`es_ES`)

The following are active projects without significant progress:

* German (`de_DE`)
* Turkish (`tr_TR`)
* French (`fr_FR`)
* French, Quebec (`fr_QC`)
* Japanese (`ja_JP`)
* Russian (Ukraine) (`ru_UA`)
* Toki Pona (`tok`)

If you'd like your language to be supported, please let us know in Discord.

We are particularly interested in `zh_CN`, `ru_RU`, and `es_ES` because these are the most commonly used languages in conjunction with TShock.
