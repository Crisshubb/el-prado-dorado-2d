# El Prado Dorado

Videojuego 2D original de plataformas realizado en Unity para el laboratorio. Se construye desde cero con recursos gráficos procedurales y audio sintetizado en tiempo de ejecución; no reutiliza las escenas, sprites ni scripts del juego anterior.

## Requisitos cubiertos

- Contador de néctar y puntos.
- Tres vidas; colisiones laterales con enemigos causan daño y retroceso.
- Saltar sobre enemigos los vence y otorga puntos.
- Música ambiental y efectos originales para néctar, pisotón, daño y botones.
- Menú con botones ilustrados **Jugar** y **Salir**, escena de partida y escena de resultados.
- Exportación Windows x64. Android está fuera del alcance de esta entrega.

## Abrir y ejecutar en Unity

1. Abrir esta carpeta con Unity `6000.3.16f1`.
2. En el menú del editor, elegir **Entrega → Preparar juego nuevo** (genera las escenas y las ordena para el build).
3. Abrir `Assets/Scenes/Menu.unity` y pulsar Play.
4. Controles: `A/D` o flechas para moverse; `Espacio`, `W` o `↑` para saltar.

Recoge las cinco gotas de néctar, pisa los pinchos y alcanza el portal. Hay tres vidas; el HUD muestra el néctar y la puntuación.

## Exportar para Windows

En Unity, ejecutar **Entrega → Compilar ejecutable Windows**. El resultado completo se crea en `Entregables/ElPradoDorado_Windows/`; el ZIP de entrega debe incluir toda esa carpeta, no solo el `.exe`.

El proyecto usa únicamente código y recursos originales generados por sus propios scripts. No requiere paquetes de audio o arte de terceros.
