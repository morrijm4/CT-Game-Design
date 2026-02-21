# Flag Raid

A 2-player top-down tank game built in Unity using the Game Assemblies library. Players compete to either capture 3 enemy flags or destroy the opponent's tank.

---

## Win Conditions

- Capture 3 flags from the enemy base and return them to your home base, **or**
- Reduce the opponent's tank health to zero

Both conditions are live simultaneously. The first player to meet either condition wins.

---

## Game Mechanics

```
[Pellet Station] → collect pellets (max 12) → [Bomb Forge] → craft bombs (4 pellets = 1 bomb, max 1)
                                                    ↓
                                          fire at enemy tank  →  -5 HP (pellet) / -30 HP (bomb)

[Enemy Base] → pick up flag → drive home → [Home Base] → +1 flag count + health replenished
```

---

## Resources

| Resource | Carry Limit | Damage | Description |
|----------|-------------|--------|-------------|
| Pellet   | 12 | 5 HP | Basic ammo. Collected from the Pellet Station. Used to shoot or craft bombs. |
| Bomb     | 1 | 30 HP | Crafted weapon. Costs 4 pellets. Has a small blast radius. |
| Flag     | 1 | — | Objective item. One per base. Must be carried back to your home base to score. |

---

## Stations

| Station | Type | Function |
|---------|------|----------|
| Pellet Station | Source | Spawns pellets on a timer near each base |
| Bomb Forge | Converter | Takes 4 pellets, outputs 1 bomb |
| Home Base | Output | Accepts returned flags, tracks score, triggers win at 3 flags. Replenishes tank health on visit. |

---

## Players

Two players share one keyboard.

| Action | Player 1 | Player 2 |
|--------|----------|----------|
| Move | WASD | Arrow Keys |
| Fire | Space | Enter |
| Interact with station | E | Numpad 0 |

Each tank starts with **100 HP**. Health does not regenerate.

---

## Strategy Notes

- **Aggressive:** Stockpile bombs via the Forge and focus on eliminating the enemy tank.
- **Objective:** Use pellets defensively while making quick flag runs to win on captures.
- **Mixed:** At 2 flags, the enemy must abandon their strategy to stop you — use this pressure.

---

## Tech Stack

- Unity 2D (top-down)
- Game Assemblies Library for resource and station management
- Single-machine local multiplayer

---

## Team

Cornell Tech — Resource Management Simulation Assignment