Here are the checks TShock performs if you enable kick/ban cheaters/griefers. (This is kept up-to-date with release builds)

# Cheaters
* Health check. If HP/Max HP is > 400, take action.
* Mana check. If Mana/Max Mana is > 400, take action.
* Inventory check. If item's stack is > max stack(for example, having more than 250 stone), take action.
* Health increase check. If Max HP is increased by more than 20, take action.
* Mana increase check. If Max Mana is increased by more than 20, take action.
* Chat origin check. If player attempts to send text as some one else, take action.

# Griefers
* Kill Tile abuse. If player destroys more than 20 blocks in one update (1/60th second), take action. - **May cause false positives with large amounts of lag**