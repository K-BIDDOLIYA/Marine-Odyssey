# 🌊 Marine Odyssey

> **A submarine adventure beneath the surface.**

**Marine Odyssey** is a 2D underwater game built in Unity where you take control of a submarine and navigate the depths while managing your health, avoiding environmental dangers, and surviving the journey.

The game combines exploration, survival mechanics, environmental hazards, score progression, and interactive gameplay into a compact underwater experience.

**Status: ✅ Completed**

---

## 🎮 About the Game

The ocean can look peaceful from above, but beneath the surface, things get much more dangerous.

In **Marine Odyssey**, you control a submarine travelling through an underwater environment. Your submarine has limited health, and the environment itself can become a threat.

You need to keep an eye on your health, deal with hazards such as coral, use hiding mechanics when necessary, and collect health boosts to stay alive for as long as possible.

The goal is simple:

**Survive. Explore. Keep moving.**

---

## ✨ Features

### 🚢 Submarine Gameplay

The submarine is the main playable character and has its own health and movement systems.

The submarine's health starts at:

**1000 HP**

Health is continuously tracked during gameplay and displayed through the HUD.

---

### ❤️ Health System

Marine Odyssey features a complete health-management system.

The submarine has:

* **1000 maximum health**
* Damage handling
* Healing
* Death detection
* Low-health state
* Health UI updates
* Environmental health drain

Health cannot exceed the submarine's maximum health.

The health system is handled by the `SubmarineHealth` component.

```text
Maximum Health: 1000
Low Health Threshold: 400
```

When health reaches zero, the submarine is considered dead and the game switches to the death state.

---

### 🪸 Coral Hazard

Coral isn't just part of the scenery.

The submarine can lose health from the coral-related hazard system.

The game tracks how long the submarine remains in the relevant coral state and applies health damage at intervals.

The default coral damage configuration is:

```text
Health Drain: 40
Damage Interval: 4 seconds
```

This makes the environment itself something the player has to pay attention to.

---

### 🫥 Submarine Hiding

The submarine also has a hiding mechanic.

When the submarine is hidden, the coral-related health behaviour changes.

The health system communicates with the submarine's hiding system through `SubmarineHide`, allowing environmental interactions to depend on whether the submarine is currently hidden.

---

### ⚠️ Low Health System

When the submarine's health drops below **400 HP**, the game enters a low-health state.

The UI can display a low-health overlay to warn the player.

The submarine's movement is also affected while in the low-health state.

The configured speed multiplier is:

```text
0.7× normal speed
```

Once health rises above the threshold, normal movement speed is restored.

This creates an additional consequence for allowing the submarine's health to get too low.

---

### ❤️ Health Boosts

Health boosts appear as collectible heart pickups.

Collecting one restores:

**+300 HP**

The boost uses the submarine's existing healing system, meaning health is automatically capped at the submarine's maximum health.

For example:

```text
Current Health: 650
Health Boost: +300
Result: 950 / 1000
```

If the submarine already has 850 HP:

```text
850 + 300 = 1150
Maximum = 1000

Result: 1000 / 1000
```

Health boosts are spawned dynamically through the `HealthSpawner` system.

The spawner controls:

* Spawn interval
* Random X position
* Random Y position
* Random movement speed

Health boosts move through the level and are automatically destroyed after passing the configured boundary.

---

## 📊 Health HUD

The game HUD provides visual feedback about the submarine's current health.

It includes:

### Health Bar

The health bar changes according to the submarine's current health.

```text
Current Health / Maximum Health
```

### Numerical Health Display

The HUD also displays the exact value, for example:

```text
1000 / 1000
```

or:

```text
650 / 1000
```

This makes the health system easy to understand at a glance.

---

## 🏆 Score System

Marine Odyssey includes a live score system.

The score increases during gameplay and is displayed on the HUD.

The game also keeps track of the player's **High Score** using Unity's `PlayerPrefs` system.

When the player dies, the death screen displays:

* Final Score
* High Score

If the player beats the previous high score, the new score is saved.

---

## 💀 Death System

When submarine health reaches zero, the game enters the death state.

The death system:

1. Marks the player as dead.
2. Stops normal gameplay.
3. Pauses the game.
4. Displays the death panel.
5. Shows the player's score.
6. Checks and updates the high score.
7. Displays the high score.

The player can then restart the game or return to the home screen.

---

## ⚙️ Settings

Marine Odyssey includes an in-game settings system.

Players can configure:

* 🎵 Music volume
* 🔊 SFX volume
* 🎮 Controller/cursor mode

Settings are stored using Unity's `PlayerPrefs`, allowing them to persist between gameplay sessions.

---

## 🎮 Controller / Cursor Mode

The game includes an input mode setting that allows the player to switch between the available control styles.

The selected mode is saved so that the preference isn't lost when changing scenes or restarting the game.

The UI also manages cursor visibility and locking depending on the current state of the game.

---

## ⏸️ Pause System

The game includes a pause menu accessible during gameplay.

When paused:

* Gameplay stops.
* The pause panel appears.
* The cursor becomes available.
* The player can access settings.
* The game can be resumed.

The game uses Unity's `Time.timeScale` to control the paused state.

---

## 🏠 Home Screen

Marine Odyssey includes a dedicated home/menu scene.

From the home screen, players can access the game and its associated menu functionality.

The game also supports returning to the home scene after gameplay.

---

## 🔄 Restart System

After dying, the player can restart the current game.

The restart system reloads the active scene and restores normal gameplay time.

This allows the player to quickly start another run without manually returning to the editor or restarting the application.

---

# 🧩 Project Architecture

Marine Odyssey is built using a collection of focused Unity components rather than putting the entire game inside one script.

Some of the important systems include:

| Script                   | Purpose                                                               |
| ------------------------ | --------------------------------------------------------------------- |
| `SubmarineHealth.cs`     | Handles submarine health, damage, healing, low-health state and death |
| `SubmarineController.cs` | Handles submarine movement                                            |
| `SubmarineHide.cs`       | Handles the submarine's hiding behaviour                              |
| `GameUIManager.cs`       | Controls gameplay UI, score, health UI, pause, settings and death UI  |
| `HealthBoost.cs`         | Controls collectible health boosts                                    |
| `HealthSpawner.cs`       | Dynamically spawns health boosts                                      |
| `GameAudioManager.cs`    | Handles game audio and volume settings                                |

The systems communicate with each other rather than duplicating functionality.

For example:

```text
Health Boost
     ↓
HealthBoost.cs
     ↓
SubmarineHealth.Heal(300)
     ↓
Health updated
     ↓
GameUIManager.UpdateHealthUI()
     ↓
Health bar + numerical display updated
```

---

# ❤️ Health System Flow

The complete health loop works approximately like this:

```text
                 ┌──────────────────┐
                 │ Submarine starts │
                 │    at 1000 HP    │
                 └────────┬─────────┘
                          │
              ┌───────────┴───────────┐
              ↓                       ↓
       Take environmental       Collect Health
           damage                  Boost
              │                       │
              ↓                       ↓
       Health decreases        +300 Health
              │                       │
              └───────────┬───────────┘
                          ↓
                 Update Health UI
                          │
                          ↓
               Is health below 400?
                    /          \
                  Yes           No
                  ↓              ↓
            Low-health       Normal state
              state
                  │
                  ↓
             Health = 0?
                  │
                 Yes
                  ↓
              Player Dies
                  │
                  ↓
             Death Screen
```

---

# 🖥️ User Interface

The game's UI is managed centrally through `GameUIManager`.

The UI system handles:

* Health bar
* Health number
* Score
* High score
* Warning messages
* Low-health overlay
* Pause panel
* Settings panel
* Death panel
* Pause button

This keeps gameplay systems separate from the presentation layer.

---

# 🔊 Audio

Marine Odyssey uses a dedicated audio manager for game audio.

The settings system allows the player to independently control:

```text
Music Volume
SFX Volume
```

These settings are stored using `PlayerPrefs`.

---

# 🛠️ Built With

### Game Engine

**Unity**

### Programming Language

**C#**

### UI

**Unity UI / TextMeshPro**

### Input

**Unity Input System**

### Version Control

**Git / GitHub**

---

# 📁 Project Structure

The project follows a component-based Unity structure.

A simplified version looks like:

```text
Marine Odyssey/
│
├── Assets/
│   ├── Scenes/
│   ├── Scripts/
│   │   ├── SubmarineHealth.cs
│   │   ├── SubmarineController.cs
│   │   ├── SubmarineHide.cs
│   │   ├── HealthBoost.cs
│   │   ├── HealthSpawner.cs
│   │   ├── GameUIManager.cs
│   │   └── GameAudioManager.cs
│   │
│   ├── Prefabs/
│   ├── Sprites/
│   ├── Audio/
│   └── UI/
│
└── ProjectSettings/
```

---

# 🧠 Design Philosophy

One of the main goals of Marine Odyssey was to make the game's systems modular.

Instead of having one enormous player script responsible for everything, individual systems are responsible for their own jobs.

For example:

```text
Movement → SubmarineController
Health → SubmarineHealth
Hiding → SubmarineHide
UI → GameUIManager
Audio → GameAudioManager
Health Pickups → HealthBoost
Spawning → HealthSpawner
```

This makes the project easier to debug, expand and maintain.

---

# 🎯 Gameplay Loop

The basic gameplay loop is:

```text
START
  ↓
Control submarine
  ↓
Explore underwater environment
  ↓
Deal with environmental hazards
  ↓
Manage health
  ↓
Collect health boosts when available
  ↓
Continue surviving
  ↓
Health reaches zero
  ↓
Death screen
  ↓
Check high score
  ↓
Restart / Return Home
```

---

# 🏁 Completion

Marine Odyssey is now **officially completed**.

The project evolved from individual gameplay systems into a complete playable game, with:

* A playable submarine
* Health management
* Environmental hazards
* Hiding mechanics
* Health pickups
* Dynamic health pickup spawning
* Health UI
* Low-health feedback
* Score tracking
* High-score saving
* Death handling
* Pause menu
* Settings
* Audio controls
* Input mode settings
* Restart functionality
* Home navigation
* Persistent settings

What started as individual mechanics eventually came together as a complete underwater game.

---

# 🚀 Running the Project

To run the project locally:

1. Clone the repository.
2. Open the project using the appropriate Unity version.
3. Open the project's main scene.
4. Press **Play** in Unity.

For a built version, use the released game build rather than opening the Unity project.

---

# 🧪 Development

Marine Odyssey was developed as a Unity game with a focus on learning how separate gameplay systems can work together.

The project involved implementing and connecting:

* Player movement
* Health and damage
* Healing
* Environmental interactions
* Pickups
* Spawning systems
* UI
* Audio
* Menus
* Game states
* Score persistence

The final result is a complete playable project rather than just a collection of isolated prototypes.

---

# 📌 Project Status

**🟢 COMPLETE**

Marine Odyssey is no longer in active development.

The core gameplay, UI, health systems, pickups, menus, settings and supporting systems have been implemented and integrated into the final game.

---

## 🌊 Marine Odyssey

**Dive deeper. Stay alive.**

*Built with Unity and C#.*
