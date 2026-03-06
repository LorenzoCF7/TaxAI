# Documentación de TaxAI

## Índice
1. [GDD (Game Design Document)](#gdd-game-design-document)
2. [Diagrama de Clases UML](#diagrama-de-clases-uml)
3. [Memoria de Arquitectura](#memoria-de-arquitectura)
4. [Informe de Rendimiento](#informe-de-rendimiento)

---

## GDD (Game Design Document)

### Resumen Básico del Juego

**TaxAI** es un juego narrativo interactivo (Visual Novel/Text Adventure) enfocado en la educación y entretenimiento mediante historias ramificadas. El jugador toma decisiones que afectan el desarrollo de la trama y el acceso a nuevas opciones basadas en items obtenidos durante la experiencia.

### Mecánicas Principales

#### 1. **Sistema de Diálogos Animados**
- Los textos aparecen progresivamente (escritura animada) en la barra inferior
- Cada frase contiene información del personaje que habla (nombre y color de texto personalizado)
- El jugador puede acelerar o pasar el diálogo mediante click/espacio

#### 2. **Sistema de Elecciones Ramificadas**
- En momentos clave, el jugador enfrenta opciones de elección
- Las opciones pueden requerir items específicos del inventario para desbloquearse
- Cada elección lleva a diferentes ramas narrativas

#### 3. **Sistema de Inventario**
- Los items se adquieren durante la narrativa como recompensas de elecciones
- Los items desbloquean nuevas opciones en futuras elecciones
- El inventario persiste durante toda la sesión de juego

#### 4. **Animación de Personajes**
- Los personajes pueden:
  - **APPEAR**: Aparecer en escena
  - **MOVE**: Moverse a nuevas coordenadas
  - **DISAPPEAR**: Desaparecer de escena
- Las animaciones incluyen velocidad de movimiento configurable

#### 5. **Gestión de Audio**
- Sistema de música ambiental con transiciones suaves (fade in/out)
- Efectos de sonido para acciones específicas
- Sincronización de audio con eventos narrativos

### Dinámicas de Juego

1. **Progresión Lineal con Ramificaciones**: El jugador avanza secuencialmente a través de escenas, con opciones que crean múltiples caminos
2. **Recompensa y Consecuencia**: Las decisiones tienen impacto tangible (obtención de items e impacto en opciones futuras)
3. **Exploración de Alternativas**: Se incentiva al jugador a rejugar para explorar diferentes finales

### Diseño de Niveles (Escenas)

- **StoryScene**: Escenas narrativas con diálogos secuenciales, transiciones de personajes y música
- **ChooseScene**: Puntos de ramificación donde el jugador elige su camino
- **Estructura**: Cada escena apunta a la siguiente, formando un árbol narrativo

### Más Información

Para detalles completos sobre la narrativa, mecánicas avanzadas y plan de contenidos, consultar documentación adicional en:
- [Documentación Online](#) (Clickup, Wiki, etc.)
- [README del Proyecto](./README.md)

---

## Diagrama de Clases UML

### Estructura General de Clases

```
┌─────────────────────────────────────────────────────────────────────┐
│                          ARQUITECTURA DEL JUEGO                      │
└─────────────────────────────────────────────────────────────────────┘

                           ┌──────────────────┐
                           │  GameController  │ (MonoBehaviour)
                           │  - currentScene  │
                           │  - state: State  │
                           │  - bottomBar     │
                           │  - chooseCtrl    │
                           └────────┬─────────┘
                                    │
                    ┌───────────────┼───────────────┐
                    │               │               │
            ┌───────────────┐  ┌────────────┐  ┌─────────────┐
            │ AudioController│  │BottomBar   │  │ChooseControl│
            │ - musicSource  │  │Controller  │  │ - labels    │
            │ - soundSource  │  │- barText   │  │ - sprites   │
            │ - SwitchMusic()│  │- sprites   │  │- SetupChoose│
            └───────────────┘  └────────────┘  └─────────────┘
                                      │
                                      │
                    ┌─────────────────┴──────────────┐
                    │                                │
            ┌──────────────────┐          ┌─────────────────┐
            │SpriteSwitcher    │          │SpriteController │
            │ - image1, image2 │          │ - transform     │
            │ - animator       │          │ - SetPosition() │
            │ - SwitchImage()  │          └─────────────────┘
            └──────────────────┘


┌──────────────────────────────────────────────────────────────────────┐
│                      SCRIPTABLE OBJECTS (Datos)                      │
└──────────────────────────────────────────────────────────────────────┘

            ┌──────────────────┐
            │   GameScene      │ (Base ScriptableObject)
            │  (clase base)    │
            └────────┬─────────┘
                     │
            ┌────────┴──────────┐
            │                   │
     ┌─────────────────┐  ┌──────────────┐
     │  StoryScene     │  │ ChooseScene  │
     │- sentences[]    │  │ - labels[]   │
     │- background     │  │- backScene   │
     │- nextScene      │  └──────────────┘
     └────────┬────────┘
              │
              ├─► Sentence (Struct)
              │   - text
              │   - speaker: Speaker
              │   - actions[]
              │   - music, sound
              │
              └─► Action (Struct)
                  - speaker
                  - spriteIndex
                  - actionType: Type (NONE, APPEAR, MOVE, DISAPPEAR)
                  - coords, moveSpeed


            ┌──────────────┐           ┌────────────┐
            │   Speaker    │           │   Item     │
            │ ScriptableObj│           │ScriptableObj
            │- speakerName │           │- itemName  │
            │- textColor   │           │- description
            │- sprites[]   │           │- icon     │
            │- prefab      │           └────────────┘
            └──────────────┘


┌──────────────────────────────────────────────────────────────────────┐
│                      SINGLETON - PATRÓN USADO                        │
└──────────────────────────────────────────────────────────────────────┘

            ┌─────────────────────────┐
            │ InventoryController     │
            │ + Instance (static)     │
            │ - items: HashSet<Item>  │
            │ + AddItem(Item)         │
            │ + RemoveItem(Item)      │
            │ + HasItem(Item)         │
            │ + GetAllItems()         │
            └─────────────────────────┘
```

### Relaciones y Flujos

**Flujo Principal:**
1. `GameController` + `BottomBarController` → Reproducen escenas narrativas (`StoryScene`)
2. `BottomBarController` → Ejecuta acciones de personajes animados
3. En puntos de elección → `ChooseController` toma el control
4. `InventoryController` valida opciones según items disponibles
5. Selecciones llevan a nuevas `StoryScene` o `ChooseScene`

**Comunicación entre Sistemas:**
- **GameController** actúa como orquestador principal
- **InventoryController (Singleton)** proporciona acceso global al inventario
- **Controllers comunican mediante referencias directas** en Inspector (simpleza)
- **ScriptableObjects desacoplan contenido de lógica**

---

## Memoria de Arquitectura

### Patrones de Diseño Implementados

#### 1. **Patrón Singleton**
**Implementación:** `InventoryController`

```csharp
public class InventoryController : MonoBehaviour
{
    public static InventoryController Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
```

**Justificación:**
- El inventario debe ser único y persistente durante toda la sesión
- `DontDestroyOnLoad` preserva el inventario entre cambios de escena
- Proporciona acceso global y seguro al estado del inventario
- Evita múltiples instancias competidoras

**Casos de Uso:**
- Verificación de items en `ChooseController.SetupChoose()`
- Otorgación de items en `ChooseController.PerformChoose()`
- Consultas de inventario desde cualquier parte del código

#### 2. **Patrón ScriptableObject**
**Implementación:** `Item`, `Speaker`, `StoryScene`, `ChooseScene`

**Beneficios:**
- **Separación de Contenido y Código**: Los datos narrativos se definen sin escribir código
- **Reutilización**: Un mismo personaje (`Speaker`) puede usarse en múltiples escenas
- **Serialización Automática**: Unity maneja la persistencia en disco
- **Interfaz Visual en Inspector**: Los diseñadores pueden crear contenido sin tocar código
- **Bajo Acoplamiento**: Los Controllers no conocen detalles específicos de contenido

**Ejemplos:**
- `Item`: Define nombre, descripción e icono de objetos recolectables
- `Speaker`: Contiene nombre, color, sprites y prefab de personaje
- `StoryScene`: Agrupa frases, personajes, animaciones y audio
- `ChooseScene`: Define opciones disponibles y items requeridos

#### 3. **Patrón MVC (Model-View-Controller)**
**Estructura del Proyecto:**

| Capa | Componentes | Responsabilidad |
|------|-------------|-----------------|
| **Model** | StoryScene, ChooseScene, Item, Speaker, GameScene | Datos del juego |
| **View** | SpriteSwitcher, BottomBarController, ChooseController | Representación visual |
| **Controller** | GameController, AudioController | Lógica y orquestación |

**Beneficios:**
- Separación clara de responsabilidades
- Fácil de testear y mantener
- Permite cambios visuales sin afectar lógica

#### 4. **Patrón State**
**Implementación:** `GameController` y `BottomBarController`

```csharp
// En GameController
private enum State { IDLE, ANIMATE, CHOOSE }
private State state = State.IDLE;

// En BottomBarController
private enum State { PLAYING, SPEEDED_UP, COMPLETED }
private State state = State.COMPLETED;
```

**Propósito:**
- Control del flujo de entrada según el estado actual
- Prevención de inputs durante animaciones o escritura
- Transiciones suaves entre modos de juego

#### 5. **Patrón Coroutine (Unity-Specific)**
**Uso Principal:** `AudioController` y Animaciones

```csharp
private IEnumerator SwitchMusic(AudioClip music)
{
    // Fade out anterior
    // Cambio de música
    // Fade in nueva música
    // Todo en múltiples frames
}
```

**Ventajas:**
- Transiciones suaves sin bloquear el game loop
- Control temporal granular (fade en múltiples segundos)
- Código más legible que callbacks anidados

#### 6. **Patrón Prefab + Pooling Implícito**
**Implementación:** `ChooseLabelController`

```csharp
ChooseLabelController newLabel = Instantiate(label.gameObject, transform)
    .GetComponent<ChooseLabelController>();
```

**Características:**
- Instanciación dinámica de opciones
- Destrucción de opciones antiguas en `DestroyLabels()`
- Escalabilidad para diferentes números de opciones

### Decisiones Arquitectónicas Clave

| Decisión | Justificación |
|----------|---------------|
| **Sin Event System formal** | La lógica directa es suficiente para este proyecto; evita sobre-ingeniería |
| **Controllers vinculados en Inspector** | Claridad visual, fácil debug, apropiado para proyectos medianos |
| **ScriptableObjects como source of truth** | Permite que diseñadores gestionen contenido sin código |
| **Singleton solo para Inventario** | Solo una instancia debe existir; solución simple y directa |
| **Animaciones manejadas por Animator** | Aprovecha herramientas nativas de Unity |

### Flujo de Datos

```
ScriptableObject (StoryScene)
    ↓
GameController.Start() [Inicialización]
    ↓
BottomBarController.PlayScene() [Reproducción]
    ↓
Sentence → ChosenAction → SpriteController [Animación]
         → AudioController [Audio]
         → BottomBarController [Diálogo]
    ↓
¿Última frase? NO → PlayNextSentence()
                → AudioController.PlayAudio()
                → loop
    ↓
¿Última frase? SÍ → ChooseController [Elección]
                  → Validar items → InventoryController
                  → Mostrar opciones
    ↓
Seleccionar opción
    ↓
InventoryController.AddItem() [Si aplica]
    ↓
GameController.PlayScene(nextScene) [Nueva escena]
```

---

## Informe de Rendimiento

### Hardware de Prueba
- **Dispositivo**: PC (Especificaciones típicas de desarrollo)
- **Motor**: Unity 2023 LTS
- **Plataforma**: Standalone (Windows)

### Escenarios de Prueba

#### 1. **Escena Estándar (Diálogo Normal)**
- Juego en estado IDLE esperando input
- Un personaje visible
- Música ambiental reproduciendo

**Métricas Esperadas:**
- **FPS**: 60 FPS (limitado por vsync)
- **CPU**: 1-3% (escritura de diálogos consume poco)
- **Memoria**: 200-300 MB (contenido narrativo es ligero)

#### 2. **Escritura Animada Activa**
- Texto apareciendo letra por letra
- Animaciones de personajes en paralelo
- Audio reproduciéndose

**Métricas Esperadas:**
- **FPS**: 55-60 FPS
- **CPU**: 2-5%
- **Memoria**: 250-320 MB

#### 3. **Transición de Música (Fade)**
- Música anterior haciéndose fade out (IEnumerator)
- Cambio de escena
- Nueva música haciéndose fade in

**Métricas Esperadas:**
- **FPS**: 50-60 FPS
- **CPU**: 3-6% (Coroutines activas)
- **Memoria**: 260-330 MB

#### 4. **Pantalla de Elección**
- Múltiples opciones renderizadas (5-10 botones)
- Validación de items en tiempo real
- Animaciones de UI

**Métricas Esperadas:**
- **FPS**: 55-60 FPS
- **CPU**: 2-4%
- **Memoria**: 270-340 MB

### Análisis de Optimizaciones

#### ✅ Implementadas
1. **ScriptableObjects**: Evitan instanciar datos en memoria
2. **Singleton Pattern**: Único inventario en memoria
3. **Coroutines**: Transiciones suaves sin hilos pesados
4. **DontDestroyOnLoad**: Evita recarga innecesaria de datos

#### 🔄 Mejoras Posibles
1. **Object Pooling**: Para opciones frecuentes (`ChooseLabelController`)
2. **Async Loading**: Para cargar escenas complejas sin bloquear
3. **Text Mesh Pro Optimization**: Pre-generar meshes si hay muchos diálogos
4. **Audio Pooling**: Reutilizar instancias de AudioSource