# 3D Helix (`Proyecto_UDG_3D_Helix`)

Videojuego tridimensional tipo *helix* desarrollado en **Unity** como proyecto final para **UDG Virtual**. El jugador controla una esfera en un entorno 3D, encadena aterrizajes en plataformas para sumar puntos, activa una racha dinámica de velocidad con estela visual (`Trail Renderer`) y supera tres niveles con dificultad progresiva.

## Características principales

* **Mecánica principal y físicas en 3D (`PlayerController.cs`):** Desplazamiento en los ejes X y Z, gravedad acelerada al caer (`fallMultiplier`), rebote al aterrizar sobre plataformas (`contact.normal.y > 0.5f`) y conteo de puntuación únicamente al tocar plataformas nuevas.
* **Sistema de racha y efecto de estela (`Trail Renderer`):** Al encadenar plataformas consecutivas (`streakTarget`), el personaje incrementa dinámicamente su velocidad de traslación y activa un `Trail Renderer` con duración controlada (máximo de 3 segundos).
* **Tres niveles con dificultad progresiva (`GameManager.cs`):**
  * **Nivel 1 (`GameplayScene`):** Velocidad base moderada y transición al siguiente nivel al alcanzar el puntaje objetivo.
  * **Nivel 2 (`Level2Scene`):** Mayor velocidad base y escalado progresivo de velocidad por punto.
  * **Nivel 3 (`Level3Scene`):** Curva de mayor exigencia para cerrar el circuito de niveles.
* **Interfaz de usuario y navegación (`UI Canvas` / `SceneMenuController.cs`):** Menú principal (`MainMenuScene`), indicador del nivel actual (`Nivel 1`, `Nivel 2`, `Nivel 3`), marcador de puntuación (`Score`) y récord persistente (`Best` / `HighScore` guardado mediante `PlayerPrefs`), además de botón para regresar al menú principal desde los niveles.
* **Compilación para Windows:** Configurado en `Build Profiles` y `Player Settings` con las escenas ordenadas y verificado mediante ejecución directa del `.exe` compilado.

## Estructura del proyecto

* `Assets/Scenes/`: `MainMenuScene`, `GameplayScene`, `Level2Scene` y `Level3Scene`.
* `Assets/Scripts/`: `PlayerController.cs`, `GameManager.cs`, `SceneMenuController.cs` y scripts de soporte del entorno/generación.
* `Assets/Materials/` y `Assets/Prefabs/`: Materiales, recursos visuales y elementos reutilizables de escena.

## Controles y cómo jugar

1. Entra a la pestaña **Releases** del repositorio y descarga el archivo `.zip` correspondiente a la versión publicada (por ejemplo, `v1.0.0`).
2. Extrae todo el contenido del `.zip` en una carpeta de tu equipo en Windows (conservando el `.exe` junto con su carpeta de datos asociada).
3. Ejecuta el archivo `.exe` para jugar directamente sin necesidad de abrir Unity Editor.
4. Usa las teclas de movimiento horizontal/vertical o flechas (`Horizontal` / `Vertical`) para guiar la esfera entre plataformas.
5. Aterriza en plataformas para sumar puntos y alcanzar la racha que activa el impulso de velocidad y el `Trail Renderer`.
6. Supera el puntaje requerido para avanzar de **Nivel 1** a **Nivel 2** y **Nivel 3**, o usa el botón **Menú** para volver al inicio.

## Autor

* **Ivan Alejandro Hernandez Hernandez** — Estudiante de UDG Virtual (`Ivan03-maker`).