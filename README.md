# El Prado Dorado

Videojuego original de plataformas 2D realizado en Unity para el laboratorio. Incluye menú principal, nivel jugable y pantalla de resultado. Los escenarios, formas gráficas, botones e indicaciones musicales se generan con código en tiempo de ejecución; no reutiliza literalmente el juego de la entrega anterior. Android está fuera del alcance solicitado.

## Controles

- Moverse: `A/D` o flechas izquierda/derecha.
- Saltar: `Espacio`, `W` o flecha arriba. El salto admite una breve tolerancia al borde y al pulsar justo antes de aterrizar.
- Pausa/reanudar: `Esc`.
- Captura de ejecución: `F12`.

Recoge las cinco gotas de néctar, esquiva el contacto lateral con los pinchos y rebótales encima para derrotarlos. Cada gota suma 100 puntos y cada enemigo derrotado, 150. El portal se activa al reunir todo el néctar. Hay tres vidas.

## Abrir y ejecutar en Unity

1. Abrir la carpeta del proyecto con Unity `6000.3.16f1`.
2. Ejecutar **Entrega → Preparar juego nuevo** para regenerar las tres escenas vacías (`Menu`, `Pradera` y `Final`) en el orden de build.
3. Abrir `Assets/Scenes/Menu.unity` y pulsar Play.

El código de `Assets/Scripts/InicializadorRuntimeNuevo.cs` construye cada escena al cargarla. Controles, contador, colisiones, audio e interfaces se implementan en scripts propios bajo `Assets/Scripts/`.

## Validación y exportación para Windows

- **Entrega → Validar escenas y build** comprueba que existan las tres escenas y que no haya componentes faltantes.
- **Entrega → Compilar ejecutable Windows** genera el jugador x64 en `Entregables/ElPradoDorado_Windows/`.
- La entrega ejecutable debe comprimirse como `Entregables/ElPradoDorado_Windows.zip` incluyendo todo el contenido de esa carpeta: `.exe`, `_Data`, DLL y subcarpetas de Unity.
- `Assets/Scripts/PruebaRuntimeNuevo.cs` permite automatizar comprobaciones de menú, pausa, daño, pisotón, contador, audio y resultado al ejecutar el build con `--smoke-test` o `--smoke-test-death`. `--captura-entrega` crea una captura del nivel durante la ejecución.

El proyecto usa código y audio sintetizado propio; no depende de archivos externos de arte ni de audio.
