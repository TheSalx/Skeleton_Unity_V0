# EXEMPLOS DE SCRIPTABLEOBJECTS

Este arquivo contém exemplos prontos de ScriptableObjects para copiar/configurar.

---

## PROJECTILE DEFINITIONS

### 1. Magic Bolt (Homing)
```
Create: Assets/_Project/ScriptableObjects/Projectiles/Projectile_MagicBolt.asset

Behavior: Homing
Speed: 10
Lifetime: 3
Acceleration: 0

Homing Strength: 5
Homing Delay: 0.1

Hit Behavior: SingleHit
Max Pierce Count: 0
Max Bounces: 0

Prefab: [Seu prefab de projétil mágico]
Scale: (1, 1, 1)
Rotate Towards Direction: true
```

### 2. Arrow (Straight + Pierce)
```
Create: Assets/_Project/ScriptableObjects/Projectiles/Projectile_Arrow.asset

Behavior: Straight
Speed: 15
Lifetime: 2
Acceleration: 0

Hit Behavior: Pierce
Max Pierce Count: 3
Max Bounces: 0

Prefab: [Seu prefab de flecha]
Scale: (1, 0.3, 1)
Rotate Towards Direction: true
```

### 3. Fireball (Fast + Exploding)
```
Create: Assets/_Project/ScriptableObjects/Projectiles/Projectile_Fireball.asset

Behavior: Straight
Speed: 20
Lifetime: 1.5
Acceleration: 0

Hit Behavior: SingleHit
Max Pierce Count: 0

Prefab: [Seu prefab de bola de fogo]
Scale: (1.5, 1.5, 1)
Rotate Towards Direction: false

Hit Effect Prefab: [Particle System de explosão]
```

---

## WEAPON DEFINITIONS

### 1. Magic Bolt
```
Create: Assets/_Project/ScriptableObjects/Weapons/Weapon_MagicBolt.asset

Weapon ID: magic_bolt
Weapon Name: Magic Bolt
Description: Dispara projéteis mágicos que seguem inimigos próximos
Rarity: Common
Weapon Type: Projectile

Base Stats:
- Base Damage: 10
- Base Cooldown: 1
- Base Projectile Count: 1
- Base Range: 15

Scaling per Level:
- Damage Per Level: 0.1
- Cooldown Reduction Per Level: 0.05
- Projectiles Per Level: 0

Combat Properties:
- Knockback Force: 2
- Pierce Count: 0
- Crit Chance: 0.05
- Crit Multiplier: 2

Projectile: Projectile_MagicBolt
```

### 2. Orbiting Blades
```
Create: Assets/_Project/ScriptableObjects/Weapons/Weapon_OrbitingBlades.asset

Weapon ID: orbiting_blades
Weapon Name: Orbiting Blades
Description: Lâminas orbitam ao redor do jogador causando dano contínuo
Rarity: Uncommon
Weapon Type: Orbital

Base Stats:
- Base Damage: 8
- Base Cooldown: 0 (não usado em orbital)
- Base Projectile Count: 0
- Base Range: 0

Scaling per Level:
- Damage Per Level: 0.15
- Cooldown Reduction Per Level: 0
- Projectiles Per Level: 0

Combat Properties:
- Knockback Force: 1.5
- Pierce Count: 0
- Crit Chance: 0
- Crit Multiplier: 1

Orbital:
- Orbital Radius: 2
- Orbital Speed: 180
- Orbital Count: 3

Weapon Prefab: [Seu prefab de lâmina orbital]
```

### 3. Arrow Barrage (Multi-projectile)
```
Create: Assets/_Project/ScriptableObjects/Weapons/Weapon_ArrowBarrage.asset

Weapon ID: arrow_barrage
Weapon Name: Arrow Barrage
Description: Dispara múltiplas flechas perfurantes
Rarity: Rare
Weapon Type: Projectile

Base Stats:
- Base Damage: 15
- Base Cooldown: 1.5
- Base Projectile Count: 5
- Base Range: 20

Scaling per Level:
- Damage Per Level: 0.1
- Cooldown Reduction Per Level: 0.05
- Projectiles Per Level: 1

Combat Properties:
- Knockback Force: 3
- Pierce Count: 3
- Crit Chance: 0.1
- Crit Multiplier: 2.5

Projectile: Projectile_Arrow
```

---

## UPGRADE DEFINITIONS

### 1. Unlock Magic Bolt
```
Create: Assets/_Project/ScriptableObjects/Upgrades/Upgrade_UnlockMagicBolt.asset

Upgrade ID: unlock_magic_bolt
Upgrade Name: Magic Bolt
Description: Dispara projéteis mágicos que seguem inimigos
Rarity: Common
Upgrade Type: NewWeapon

Weapon: Weapon_MagicBolt

Weight: 50
Min Player Level: 1
Max Player Level: 0
```

### 2. Magic Bolt Level Up
```
Create: Assets/_Project/ScriptableObjects/Upgrades/Upgrade_MagicBoltPlus.asset

Upgrade ID: magic_bolt_plus
Upgrade Name: Magic Bolt +
Description: Aumenta dano e reduz cooldown
Rarity: Common
Upgrade Type: WeaponLevelUp

Weapon: Weapon_MagicBolt

Weight: 40
Min Player Level: 1
```

### 3. Speed Boost (Passive)
```
Create: Assets/_Project/ScriptableObjects/Upgrades/Upgrade_SpeedBoost.asset

Upgrade ID: speed_boost
Upgrade Name: Speed Boost
Description: +20% velocidade de movimento
Rarity: Common
Upgrade Type: PassiveStat

Stat Modifiers:
- Size: 1
  - [0] Stat Type: MoveSpeed
  - [0] Value: 0.2
  - [0] Is Percentage: true

Weight: 30
Min Player Level: 1
```

### 4. Max HP Increase (Passive)
```
Create: Assets/_Project/ScriptableObjects/Upgrades/Upgrade_MaxHP.asset

Upgrade ID: max_hp_increase
Upgrade Name: Vitality
Description: +30 HP máximo
Rarity: Common
Upgrade Type: PassiveStat

Stat Modifiers:
- Size: 1
  - [0] Stat Type: MaxHP
  - [0] Value: 30
  - [0] Is Percentage: false

Weight: 40
Min Player Level: 1
```

### 5. Unlock Orbiting Blades
```
Create: Assets/_Project/ScriptableObjects/Upgrades/Upgrade_UnlockOrbitingBlades.asset

Upgrade ID: unlock_orbiting_blades
Upgrade Name: Orbiting Blades
Description: Lâminas orbitam causando dano contínuo
Rarity: Uncommon
Upgrade Type: NewWeapon

Weapon: Weapon_OrbitingBlades

Weight: 30
Min Player Level: 2
```

### 6. Crit Chance (Passive)
```
Create: Assets/_Project/ScriptableObjects/Upgrades/Upgrade_CritChance.asset

Upgrade ID: crit_chance
Upgrade Name: Critical Strike
Description: +10% chance de crítico
Rarity: Uncommon
Upgrade Type: PassiveStat

Stat Modifiers:
- Size: 1
  - [0] Stat Type: CritChance
  - [0] Value: 0.1
  - [0] Is Percentage: false

Weight: 25
Min Player Level: 3
```

### 7. Damage Multiplier (Passive)
```
Create: Assets/_Project/ScriptableObjects/Upgrades/Upgrade_DamageBoost.asset

Upgrade ID: damage_boost
Upgrade Name: Power Surge
Description: +15% dano total
Rarity: Rare
Upgrade Type: PassiveStat

Stat Modifiers:
- Size: 1
  - [0] Stat Type: DamageMultiplier
  - [0] Value: 0.15
  - [0] Is Percentage: true

Weight: 20
Min Player Level: 5
```

---

## ENEMY DEFINITIONS

### 1. Slime (Basic)
```
Create: Assets/_Project/ScriptableObjects/Enemies/Enemy_Slime.asset

Enemy ID: slime
Enemy Name: Slime
Enemy Type: Basic

Stats:
- Max HP: 20
- Move Speed: 2
- Damage: 5
- Attack Cooldown: 1

Behavior:
- Attack Range: 1
- Detection Range: 15
- Can Knockback: true
- Knockback Resistance: 1

Rewards:
- XP Reward: 1
- Drop Chance: 0.1

Spawning:
- Spawn Weight: 60

Prefab: [Seu prefab de slime]
Scale: (1, 1, 1)
```

### 2. Fast Runner
```
Create: Assets/_Project/ScriptableObjects/Enemies/Enemy_FastRunner.asset

Enemy ID: fast_runner
Enemy Name: Fast Runner
Enemy Type: Fast

Stats:
- Max HP: 15
- Move Speed: 4
- Damage: 3
- Attack Cooldown: 0.8

Behavior:
- Attack Range: 1
- Detection Range: 20
- Can Knockback: true
- Knockback Resistance: 0.7

Rewards:
- XP Reward: 2
- Drop Chance: 0.15

Spawning:
- Spawn Weight: 30

Prefab: [Seu prefab de runner]
Scale: (0.8, 1.2, 1)
```

### 3. Tank
```
Create: Assets/_Project/ScriptableObjects/Enemies/Enemy_Tank.asset

Enemy ID: tank
Enemy Name: Tank
Enemy Type: Tank

Stats:
- Max HP: 50
- Move Speed: 1
- Damage: 10
- Attack Cooldown: 1.5

Behavior:
- Attack Range: 1.5
- Detection Range: 12
- Can Knockback: true
- Knockback Resistance: 3

Rewards:
- XP Reward: 5
- Drop Chance: 0.25

Spawning:
- Spawn Weight: 15

Prefab: [Seu prefab de tank]
Scale: (1.5, 1.5, 1)
```

---

## STAGE DEFINITIONS

### Default Stage (10 minutos)
```
Create: Assets/_Project/ScriptableObjects/Stages/Stage_Default.asset

Stage ID: default_stage
Stage Name: Default Stage
Description: 10 minutos de sobrevivência

Duration: 600

Spawn Settings:
- Max Active Enemies: 100
- Min Spawn Distance: 12
- Max Spawn Distance: 15

Waves:
- Size: 3

  [0] Wave 0 - Early Game
  - Start Time: 0
  - Spawn Rate: 1
  - Available Enemies: [Enemy_Slime]
  
  [1] Wave 1 - Mid Game
  - Start Time: 180 (3 min)
  - Spawn Rate: 2
  - Available Enemies: [Enemy_Slime, Enemy_FastRunner]
  
  [2] Wave 2 - Late Game
  - Start Time: 420 (7 min)
  - Spawn Rate: 3
  - Available Enemies: [Enemy_Slime, Enemy_FastRunner, Enemy_Tank]

Difficulty Curves:

  HP Multiplier Curve:
  - Key 0: Time 0, Value 1
  - Key 1: Time 1, Value 2.5
  
  Damage Multiplier Curve:
  - Key 0: Time 0, Value 1
  - Key 1: Time 1, Value 1.8
  
  Spawn Rate Multiplier Curve:
  - Key 0: Time 0, Value 1
  - Key 1: Time 1, Value 3

Pooling:
- Prewarm Count: 30
```

### Endless Stage
```
Create: Assets/_Project/ScriptableObjects/Stages/Stage_Endless.asset

Stage ID: endless_stage
Stage Name: Endless Mode
Description: Sobreviva o máximo possível

Duration: 0 (infinito)

Spawn Settings:
- Max Active Enemies: 200
- Min Spawn Distance: 12
- Max Spawn Distance: 15

Waves:
- Size: 4

  [0] Start: 0, Rate: 1, Enemies: [Slime]
  [1] Start: 120, Rate: 2, Enemies: [Slime, FastRunner]
  [2] Start: 300, Rate: 3, Enemies: [Slime, FastRunner, Tank]
  [3] Start: 600, Rate: 5, Enemies: [All]

Difficulty Curves:
  - HP: Linear de 1 a 5 (cresce infinitamente)
  - Damage: Linear de 1 a 3
  - Spawn Rate: Linear de 1 a 10

Prewarm Count: 50
```

---

## DICAS DE CONFIGURAÇÃO

### Raridade e Pesos
- Common: Weight 40-60
- Uncommon: Weight 25-35
- Rare: Weight 15-20
- Epic: Weight 8-12
- Legendary: Weight 3-5

### Balanceamento de Dano
- Player Base Damage: 10
- Enemy HP Tier 1: 15-25
- Enemy HP Tier 2: 30-50
- Enemy HP Tier 3: 60-100
- Boss HP: 500-1000+

### Cooldowns
- Arma rápida: 0.5-0.8s
- Arma média: 1-1.5s
- Arma lenta/forte: 2-3s

### XP Progression
- Base XP: 10
- Scaling: 1.5x por nível
- Nível 2: 10 XP
- Nível 3: 15 XP
- Nível 4: 22 XP
- Nível 5: 33 XP
- etc.

---

**Use estes exemplos como base e customize conforme sua visão do jogo!**
