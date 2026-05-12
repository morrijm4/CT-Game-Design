# Armistice

[![Watch the Armistice trailer](AdditionalMaterials/MainMenu.png)](https://www.youtube.com/watch?v=Fko-PupBILw)

**Trailer:** [Watch on YouTube](https://www.youtube.com/watch?v=Fko-PupBILw)

`Armistice` is a local multiplayer Unity tank game centered on combat, survival, and prisoner dilemma decision moments.

**Builds:** Download playable builds from the [GitHub Releases](https://github.com/morrijm4/CT-Game-Design/releases) section.

## Repository Guide

- `AdditionalMaterials/` contains the final presentation PDF and gameplay screenshots.
- `Assets/Scenes/` contains the main Unity scenes: `MainMenu`, `Instructions`, and `Arena`.
- `Assets/Game Assemblies/Prefabs/Players/Tank.prefab` is the main tank prefab and a useful entry point for understanding how the player object is assembled.
- `Assets/Scripts/` contains the core gameplay and UI scripts.
  Helpful scripts to review first:
  - `Health.cs`
  - `Projectile.cs`
  - `Shooter/Shooter.cs`
  - `Shooter/PelletShooter.cs`
  - `Shooter/BombShooter.cs`
  - `CeasefireManager.cs`
  - `CeaseFire/CeasefireController.cs`
  - `Respawner.cs`

## Screenshot Gallery

<table>
  <tr>
    <td align="center" width="50%">
      <img src="AdditionalMaterials/MainMenu.png" alt="Main Menu" width="100%" />
    </td>
    <td align="center" width="50%">
      <img src="AdditionalMaterials/MovementControls.png" alt="Movement Controls" width="100%" />
    </td>
  </tr>
  <tr>
    <td align="center" width="50%">
      <img src="AdditionalMaterials/CeasefireBetrayal.png" alt="Ceasefire Betrayal Screen" width="100%" />
    </td>
    <td align="center" width="50%">
      <img src="AdditionalMaterials/TankExplosion.png" alt="Tank Explosion" width="100%" />
    </td>
  </tr>
  <tr>
    <td align="center" width="50%">
      <img src="AdditionalMaterials/GameOver.png" alt="Game Over Screen" width="100%" />
    </td>
    <td align="center" width="50%"></td>
  </tr>
</table>

## Technical Note

- Unity version: `6000.3.9f1`
