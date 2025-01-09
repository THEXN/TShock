Soporte a Español por: [FrankV22_GitHub](https://github.com/itsFrankV22) [Discord](https://discord.gg/qdBSvagzPz
)

# Aquí están las verificaciones que realiza TShock si habilitas la opción de expulsar/bloquear tramposos o saboteadores. (Esto se mantiene actualizado con las versiones de lanzamiento).

## Tramposos
* **Verificación de salud**: Si la salud actual o máxima (HP/Max HP) es mayor a 400, se toma acción.
* **Verificación de maná**: Si el maná actual o máximo (Mana/Max Mana) es mayor a 400, se toma acción.
* **Verificación de inventario**: Si la cantidad de un objeto excede el máximo permitido (por ejemplo, tener más de 250 piedras), se toma acción.
* **Verificación de aumento de salud**: Si la salud máxima (Max HP) aumenta en más de 20, se toma acción.
* **Verificación de aumento de maná**: Si el maná máximo (Max Mana) aumenta en más de 20, se toma acción.
* **Verificación de origen de chat**: Si un jugador intenta enviar texto como otro jugador, se toma acción.

## Saboteadores
* **Abuso de eliminación de bloques**: Si un jugador destruye más de 20 bloques en una actualización (1/60 de segundo), se toma acción. - **Esto puede causar falsos positivos con grandes cantidades de lag.**
