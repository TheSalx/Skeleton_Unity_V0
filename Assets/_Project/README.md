# 🎮 RogueLite Unity Project

**Projeto Unity roguelite top-down inspirado em Vampire Survivors**

---

## 📋 Índice

1. [Como Rodar o Projeto](#como-rodar-o-projeto)
2. [Arquitetura](#arquitetura)
3. [Como Adicionar Conteúdo](#como-adicionar-conteúdo)
   - [Criar Nova Arma](#1-criar-nova-arma)
   - [Criar Novo Projétil](#2-criar-novo-projétil)
   - [Criar Novo Upgrade](#3-criar-novo-upgrade)
   - [Criar Novo Inimigo](#4-criar-novo-inimigo)
   - [Ajustar Dificuldade do Stage](#5-ajustar-dificuldade-do-stage)
4. [Parâmetros Principais](#parâmetros-principais-para-balanceamento)
5. [Estrutura de Pastas](#estrutura-de-pastas)
6. [FAQ](#faq)

---

## 🚀 Como Rodar o Projeto

### Requisitos
- **Unity 6 (6000.3.3f1)** ou superior
- TextMeshPro (será importado automaticamente)

### Passos

1. **Crie um novo projeto Unity 6**
   - Abra Unity Hub
   - Clique em "New Project"
   - Selecione template "2D Core" (ou 3D se preferir)
   - Escolha Unity 6.3 (6000.3.3f1)
   - Nomeie o projeto (ex: "RogueLiteGame")

2. **Copie os arquivos do projeto**
   - Copie a pasta `Assets/_Project/` completa para dentro da pasta `Assets/` do seu projeto Unity
   - Unity irá detectar e importar automaticamente

3. **Configure as Tags e Layers**
   - Vá em `Edit > Project Settings > Tags and Layers`
   - Adicione as seguintes Tags:
     - `Player`
     - `Enemy`
     - `Projectile`
   - Adicione as seguintes Layers:
     - `Player`
     - `Enemy`
     - `Projectile`
     - `Pickup`

4. **Configure a Matrix de Colisão**
   - Vá em `Edit > Project Settings > Physics 2D`
   - Configure as colisões:
     - `Player` deve colidir com: `Enemy`
     - `Enemy` deve colidir com: `Player`, `Projectile`
     - `Projectile` deve colidir com: `Enemy`
     - `Pickup` deve colidir com: `Player`

5. **Crie os ScriptableObjects básicos**
   - Siga as instruções em [Como Adicionar Conteúdo](#como-adicionar-conteúdo)
   - Crie pelo menos 2 armas, 2 projéteis, 3 upgrades, 2 inimigos, 1 stage

6. **Monte a cena "Game"**
   - Crie uma cena nova chamada "Game"
   - Adicione um GameObject vazio chamado "Bootstrap" com o script `Bootstrap.cs`
   - Crie o Player:
     - GameObject com Tag "Player"
     - Adicione `Rigidbody2D` (Gravity Scale = 0)
     - Adicione `CircleCollider2D`
     - Adicione scripts: `PlayerController`, `PlayerStats`, `PlayerCombat`
     - Adicione um `SpriteRenderer` com sprite placeholder (círculo)
   - Crie os Managers:
     - GameObject "XPManager" com script `XPManager`
     - GameObject "LevelSystem" com script `LevelSystem`
     - GameObject "UpgradeSystem" com script `UpgradeSystem`
     - GameObject "EnemySpawner" com script `EnemySpawner`
   - Crie a UI:
     - Canvas com `GameHUD`, `LevelUpUI`, `GameOverUI`

7. **Configure os Prefabs**
   - Crie prefabs para:
     - XP Orb (esfera pequena amarela)
     - Projéteis (formas simples coloridas)
     - Inimigos (cubos/esferas vermelhos)
     - Armas orbitais (pequenas lâminas ou esferas)

8. **Aperte Play!**

---

## 🏗️ Arquitetura

### Princípios
- **Data-Driven**: Todo conteúdo em ScriptableObjects
- **Baixo Acoplamento**: Interfaces e ServiceLocator
- **Pooling**: Todos os objetos dinâmicos usam ObjectPool
- **Event-Driven**: EventBus para comunicação desacoplada

### Core Systems
- **Bootstrap**: Inicializa todos os sistemas
- **ServiceLocator**: Gerencia dependências globais
- **EventBus**: Sistema de eventos (OnEnemyKilled, OnXPGained, etc)
- **ObjectPoolManager**: Pool de objetos reutilizáveis

### Gameplay Systems
- **Player**: Movimento, stats, combate
- **Enemies**: AI simples (seek player), spawn, dano
- **Combat**: Armas, projéteis, dano, knockback
- **Progression**: XP, level up, upgrades
- **Spawning**: Spawner com curvas de dificuldade

### Separação Config vs Runtime
- **Definitions (ScriptableObjects)**: Dados imutáveis
- **Runtime (MonoBehaviour)**: Estado e lógica de execução

---

## 📦 Como Adicionar Conteúdo

### 1. Criar Nova Arma

**Passo a passo:**

1. **Crie o ProjectileDefinition (se for arma de projétil)**
   - Clique direito em `Assets/_Project/ScriptableObjects/Projectiles/`
   - `Create > RogueLite > Projectile Definition`
   - Nomeie: `Projectile_MagicBolt`

2. **Configure o ProjectileDefinition**
   ```
   Behavior: Homing
   Speed: 10
   Lifetime: 3
   Homing Strength: 5
   Hit Behavior: SingleHit
   Prefab: (crie um prefab simples - esfera azul)
   ```

3. **Crie o WeaponDefinition**
   - Clique direito em `Assets/_Project/ScriptableObjects/Weapons/`
   - `Create > RogueLite > Weapon Definition`
   - Nomeie: `Weapon_MagicBolt`

4. **Configure o WeaponDefinition**
   ```
   Weapon ID: magic_bolt
   Weapon Name: Magic Bolt
   Description: Dispara proj\u00e9teis m\u00e1gicos que seguem inimigos
   Rarity: Common
   Weapon Type: Projectile
   
   Base Stats:
   - Base Damage: 10
   - Base Cooldown: 1
   - Base Projectile Count: 1
   - Base Range: 15
   
   Scaling per Level:
   - Damage Per Level: 0.1 (10%)
   - Cooldown Reduction Per Level: 0.05 (5%)
   - Projectiles Per Level: 0 (ou 1 se quiser adicionar proj\u00e9teis)
   
   Combat Properties:
   - Knockback Force: 2
   - Pierce Count: 0
   - Crit Chance: 0.05
   - Crit Multiplier: 2
   
   Projectile: Projectile_MagicBolt
   ```

5. **Crie o Prefab da Arma**
   - Se for orbital: crie um GameObject com sprite e collider
   - Se for projétil: não precisa (projétil já tem prefab)

6. **Adicione ao UpgradeSystem**
   - Crie um UpgradeDefinition para esta arma (veja seção de Upgrades)

**Pronto!** A arma está criada.

---

### 2. Criar Novo Projétil

**Passo a passo:**

1. **Crie o ProjectileDefinition**
   - `Assets/_Project/ScriptableObjects/Projectiles/`
   - `Create > RogueLite > Projectile Definition`

2. **Configure os parâmetros:**
   ```
   Movement:
   - Behavior: Straight / Homing / Boomerang
   - Speed: 10-20 (rápido vs lento)
   - Lifetime: 2-5 segundos
   - Acceleration: 0 (ou positivo para acelerar)
   
   Homing (se Behavior = Homing):
   - Homing Strength: 5
   - Homing Delay: 0.1
   
   Hit Behavior:
   - Hit Behavior: SingleHit / Pierce / Bounce
   - Max Pierce Count: 0-5
   - Max Bounces: 0-3
   
   Visual:
   - Prefab: Seu prefab de projétil (GameObject com Rigidbody2D + Collider2D + SpriteRenderer)
   - Scale: (1, 1, 1)
   - Rotate Towards Direction: true
   ```

3. **Crie o Prefab do Projétil**
   - GameObject na cena
   - Adicione `Rigidbody2D` (Gravity Scale = 0)
   - Adicione `CircleCollider2D` ou `BoxCollider2D` (Is Trigger = true)
   - Adicione `SpriteRenderer` com sprite
   - Adicione script `Projectile.cs`
   - Salve como prefab em `Assets/_Project/Prefabs/Projectiles/`
   - Delete da cena

4. **Conecte o Prefab ao ProjectileDefinition**
   - Arraste o prefab para o campo "Prefab"

---

### 3. Criar Novo Upgrade

**Passo a passo:**

1. **Decida o tipo de upgrade:**
   - **NewWeapon**: Adiciona uma arma nova
   - **WeaponLevelUp**: Sobe nível de uma arma existente
   - **PassiveStat**: Aumenta stats do jogador
   - **Evolution**: Evolui uma arma

2. **Crie o UpgradeDefinition**
   - `Assets/_Project/ScriptableObjects/Upgrades/`
   - `Create > RogueLite > Upgrade Definition`

3. **Configure conforme o tipo:**

   **Exemplo 1: Nova Arma**
   ```
   Upgrade ID: unlock_magic_bolt
   Upgrade Name: Magic Bolt
   Description: Dispara proj\u00e9teis m\u00e1gicos
   Rarity: Common
   Upgrade Type: NewWeapon
   Weapon Definition: Weapon_MagicBolt
   Weight: 50
   Min Player Level: 1
   ```

   **Exemplo 2: Level Up de Arma**
   ```
   Upgrade ID: levelup_magic_bolt
   Upgrade Name: Magic Bolt +
   Description: Aumenta dano e velocidade
   Upgrade Type: WeaponLevelUp
   Weapon Definition: Weapon_MagicBolt
   ```

   **Exemplo 3: Stat Passivo**
   ```
   Upgrade ID: increase_speed
   Upgrade Name: Speed Boost
   Description: +20% movimento
   Upgrade Type: PassiveStat
   
   Stat Modifiers:
   - Stat Type: MoveSpeed
   - Value: 0.2
   - Is Percentage: true
   ```

   **Exemplo 4: Evolução**
   ```
   Upgrade ID: evolve_magic_bolt
   Upgrade Name: Arcane Barrage
   Description: Magic Bolt evolui
   Upgrade Type: Evolution
   Base Weapon: Weapon_MagicBolt
   Evolved Weapon: Weapon_ArcaneBarrage
   Required Weapon Level: 5
   ```

4. **Adicione ao UpgradeSystem**
   - Selecione o GameObject "UpgradeSystem" na cena
   - No Inspector, no array "All Upgrades"
   - Aumente o size e arraste seu UpgradeDefinition

---

### 4. Criar Novo Inimigo

**Passo a passo:**

1. **Crie o EnemyDefinition**
   - `Assets/_Project/ScriptableObjects/Enemies/`
   - `Create > RogueLite > Enemy Definition`
   - Nomeie: `Enemy_Slime`

2. **Configure as stats:**
   ```
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
   - Spawn Weight: 50 (maior = mais comum)
   
   Visual:
   - Prefab: (seu prefab de inimigo)
   - Scale: (1, 1, 1)
   ```

3. **Crie o Prefab do Inimigo**
   - GameObject na cena
   - Tag: "Enemy"
   - Layer: "Enemy"
   - Adicione `Rigidbody2D` (Gravity Scale = 0, Freeze Rotation Z)
   - Adicione `CircleCollider2D` ou `BoxCollider2D`
   - Adicione `SpriteRenderer` com sprite (quadrado vermelho, por exemplo)
   - Adicione script `Enemy.cs`
   - Salve como prefab em `Assets/_Project/Prefabs/Enemies/`
   - Delete da cena

4. **Conecte ao EnemyDefinition**
   - Arraste o prefab para o campo "Prefab"

5. **Adicione ao Stage**
   - Abra seu StageDefinition
   - Em alguma Wave, adicione este EnemyDefinition ao array "Available Enemies"

---

### 5. Ajustar Dificuldade do Stage

**Passo a passo:**

1. **Abra o StageDefinition**
   - `Assets/_Project/ScriptableObjects/Stages/Stage_Default`

2. **Ajuste Duration**
   ```
   Duration: 600 (10 minutos) ou 0 para infinito
   ```

3. **Ajuste Spawn Settings**
   ```
   Max Active Enemies: 50-200 (performance vs desafio)
   Min Spawn Distance: 12 (longe do player)
   Max Spawn Distance: 15
   ```

4. **Configure as Waves**
   ```
   Wave 0 (início):
   - Start Time: 0
   - Spawn Rate: 1 (1 inimigo/segundo)
   - Available Enemies: [Enemy_Slime]
   
   Wave 1 (meio do jogo):
   - Start Time: 180 (3 minutos)
   - Spawn Rate: 2
   - Available Enemies: [Enemy_Slime, Enemy_FastRunner]
   
   Wave 2 (final):
   - Start Time: 480 (8 minutos)
   - Spawn Rate: 3
   - Available Enemies: [Enemy_Slime, Enemy_FastRunner, Enemy_Tank]
   ```

5. **Ajuste Difficulty Curves** (AnimationCurves)
   
   **HP Multiplier Curve:**
   - Tempo 0: Value 1 (HP normal)
   - Tempo 1: Value 2-3 (HP dobra/triplica no final)
   
   **Damage Multiplier Curve:**
   - Tempo 0: Value 1
   - Tempo 1: Value 1.5-2
   
   **Spawn Rate Multiplier Curve:**
   - Tempo 0: Value 1
   - Tempo 1: Value 2-4 (spawn rate dobra/quadruplica)

6. **Ajuste Pooling**
   ```
   Prewarm Count: 20-50 (quantos inimigos pré-instanciar)
   ```

---

## 🎮 Parâmetros Principais para Balanceamento

### Player Stats (PlayerStats.cs)
```csharp
maxHP: 100           // HP máximo
moveSpeed: 5         // Velocidade de movimento
pickupRadius: 1.5    // Raio de coleta de XP
armor: 0             // Armadura (reduz dano)
hpRegen: 0           // Regeneração por segundo
damageMultiplier: 1  // Multiplicador de dano
```

### Weapon Stats (WeaponDefinition)
```
baseDamage: 10              // Dano base
baseCooldown: 1             // Cooldown em segundos
baseProjectileCount: 1      // Número de projéteis
baseRange: 10               // Alcance
damagePerLevel: 0.1         // +10% dano por nível
cooldownReductionPerLevel: 0.05  // -5% cooldown por nível
knockbackForce: 2           // Força de knockback
pierceCount: 0              // Quantos inimigos atravessa
critChance: 0.05            // 5% chance de crit
critMultiplier: 2           // 2x dano no crit
```

### Enemy Stats (EnemyDefinition)
```
maxHP: 20              // HP máximo
moveSpeed: 2           // Velocidade
damage: 5              // Dano por toque
xpReward: 1            // XP ao morrer
spawnWeight: 50        // Peso no spawn (maior = mais comum)
knockbackResistance: 1 // Resistência a knockback (maior = menos)
```

### XP System (LevelSystem)
```
baseXPRequired: 10         // XP base para nível 2
xpScalingFactor: 1.5       // Multiplicador por nível
```

### Stage Difficulty (StageDefinition)
```
maxActiveEnemies: 100      // Limite de inimigos simultâneos
spawnRate: 1-3             // Inimigos por segundo
hpMultiplier: 1-3          // Multiplicador de HP ao longo do tempo
damageMultiplier: 1-2      // Multiplicador de dano
spawnRateMultiplier: 1-4   // Multiplicador de spawn rate
```

---

## 📁 Estrutura de Pastas

```
Assets/_Project/
├── Scripts/
│   ├── Core/                    # Sistemas fundamentais
│   │   ├── Bootstrap.cs         # Inicialização
│   │   ├── ServiceLocator.cs    # Gerenciador de dependências
│   │   ├── EventBus.cs          # Sistema de eventos
│   │   ├── ObjectPoolManager.cs # Pool de objetos
│   │   └── GameConstants.cs     # Constantes
│   ├── Gameplay/
│   │   ├── Player/              # Player
│   │   │   ├── IInputProvider.cs
│   │   │   ├── PlayerController.cs
│   │   │   ├── PlayerStats.cs
│   │   │   └── PlayerCombat.cs
│   │   ├── Enemies/             # Inimigos
│   │   │   ├── Enemy.cs
│   │   │   └── EnemyDefinition.cs
│   │   ├── Combat/              # Combate
│   │   │   ├── CombatInterfaces.cs
│   │   │   ├── StatBlock.cs
│   │   │   ├── WeaponDefinition.cs
│   │   │   ├── WeaponRuntime.cs
│   │   │   ├── ProjectileDefinition.cs
│   │   │   ├── Projectile.cs
│   │   │   └── OrbitalWeapon.cs
│   │   ├── Progression/         # Progressão
│   │   │   ├── XPOrb.cs
│   │   │   ├── XPManager.cs
│   │   │   ├── LevelSystem.cs
│   │   │   ├── UpgradeDefinition.cs
│   │   │   └── UpgradeSystem.cs
│   │   └── Spawning/            # Spawn
│   │       ├── StageDefinition.cs
│   │       └── EnemySpawner.cs
│   ├── UI/                      # Interface
│   │   ├── GameHUD.cs
│   │   ├── LevelUpUI.cs
│   │   └── GameOverUI.cs
│   └── Utils/                   # Utilit\u00e1rios
│       ├── Extensions.cs
│       └── DebugHelper.cs
├── ScriptableObjects/           # Dados
│   ├── Weapons/
│   ├── Projectiles/
│   ├── Upgrades/
│   ├── Enemies/
│   └── Stages/
├── Prefabs/                     # Prefabs
│   ├── Player/
│   ├── Enemies/
│   ├── Projectiles/
│   ├── Drops/
│   └── UI/
├── Scenes/                      # Cenas
│   └── Game.unity
└── Settings/                    # Configura\u00e7\u00f5es
```

---

## ❓ FAQ

### Como adicionar mais armas simultaneamente?
No `PlayerCombat`, não há limite. Cada arma é independente.

### Como criar uma arma que dispara em círculo?
No `WeaponRuntime.FireProjectiles()`, modifique o loop para espalhar projéteis em 360°:
```csharp
for (int i = 0; i < projectileCount; i++)
{
    float angle = (360f / projectileCount) * i;
    Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);
    SpawnProjectile(direction);
}
```

### Como adicionar um boss?
1. Crie um `EnemyDefinition` com `EnemyType: Boss`
2. Aumente drasticamente HP, damage, e scale
3. No `StageDefinition`, crie uma Wave específica no tempo final com apenas o boss

### Como fazer projéteis que explodem?
No `Projectile.cs`, ao atingir algo, instancie uma área de dano (outro GameObject com collider grande) que aplica dano a todos os inimigos na área.

### Como adicionar sons?
Adicione `AudioSource` nos prefabs e chame `audioSource.PlayOneShot(clip)` nos eventos (disparo, hit, morte).

### Como salvar progresso?
Use `PlayerPrefs` ou JSON para salvar stats desbloqueadas, armas permanentes, etc. Crie um `ProgressionManager` que lê/escreve ao iniciar/fechar.

### Como adicionar efeitos visuais?
Use Particle Systems nos campos `hitEffectPrefab`, `deathEffectPrefab`, `trailEffectPrefab`.

### Como mudar para Input System novo?
Crie uma classe que implementa `IInputProvider` usando `UnityEngine.InputSystem` e substitua no `PlayerController`.

---

## 🎯 Próximos Passos Sugeridos

1. **Conteúdo**: Crie 5-10 armas, 10-15 upgrades, 5-8 tipos de inimigos
2. **Balanceamento**: Teste e ajuste curvas de dificuldade
3. **Polish**: Adicione efeitos visuais, sons, feedbacks
4. **UI**: Melhore a interface com animações e ícones customizados
5. **Meta-Progression**: Sistema de upgrades permanentes entre partidas
6. **Persistência**: Salvar/carregar progresso

---

## 📝 Notas Técnicas

### Performance
- Pool pre-warmed: 20-50 inimigos, 50 XP orbs, 30 projéteis
- Máximo de 100-200 inimigos simultâneos
- Use `GameObject.FindGameObjectsWithTag` com moderação (cachear quando possível)

### Compilação
- Todos os scripts devem compilar sem erros
- Namespace: `RogueLite.*`
- Compatível com Unity 6+

### Extensibilidade
- Para adicionar novo tipo de arma, estenda `WeaponType` enum e adicione case em `WeaponRuntime.Update()`
- Para adicionar novo comportamento de projétil, estenda `ProjectileBehavior` e `Projectile.Update()`
- Para adicionar novo stat, adicione em `StatModifierType` e método getter em `StatBlock`

---

**Desenvolvido para Unity 6 - Pronto para expandir!**
