# 🚀 QUICK START GUIDE - RogueLite Unity

**Comece a desenvolver em 15 minutos!**

---

## 🚦 ANTES DE COMEÇAR

### Requisitos
- Unity 6.3 (6000.3.3f1) instalado
- 30 minutos de tempo
- Vontade de criar um roguelite!

---

## 📝 PASSO 1: CRIAR PROJETO (2 min)

1. Abra Unity Hub
2. "New Project" → Template "2D Core"
3. Unity Version: 6.3
4. Nome: "MyRoguelite"
5. Create Project

---

## 💾 PASSO 2: COPIAR ARQUIVOS (1 min)

1. Copie a pasta `Assets/_Project/` para `Assets/` do seu projeto
2. Unity vai importar automaticamente
3. Aguarde compilação (pode levar 1-2 min)

---

## ⚙️ PASSO 3: CONFIGURAR UNITY (3 min)

### 3.1 Tags
`Edit > Project Settings > Tags and Layers`

**Tags:**
- Player
- Enemy  
- Projectile

### 3.2 Layers
**Layers:**
- Player (Layer 6)
- Enemy (Layer 7)
- Projectile (Layer 8)
- Pickup (Layer 9)

### 3.3 Physics 2D
`Edit > Project Settings > Physics 2D > Layer Collision Matrix`

Marque APENAS:
- Player ↔ Enemy
- Enemy ↔ Projectile
- Player ↔ Pickup

### 3.4 TextMeshPro
`Window > TextMeshPro > Import TMP Essential Resources`

---

## 🎮 PASSO 4: CRIAR CONTEÚDO MÍNIMO (5 min)

### 4.1 Projectile
1. `Assets/_Project/ScriptableObjects/Projectiles/` → Right Click
2. `Create > RogueLite > Projectile Definition`
3. Nome: "Projectile_Basic"
4. Configure:
   ```
   Behavior: Straight
   Speed: 10
   Lifetime: 3
   Hit Behavior: SingleHit
   ```
5. Prefab: criaremos depois

### 4.2 Weapon
1. `Assets/_Project/ScriptableObjects/Weapons/`
2. `Create > RogueLite > Weapon Definition`
3. Nome: "Weapon_BasicShot"
4. Configure:
   ```
   Weapon ID: basic_shot
   Weapon Name: Basic Shot
   Weapon Type: Projectile
   Base Damage: 10
   Base Cooldown: 1
   Projectile: Projectile_Basic
   ```

### 4.3 Enemy
1. `Assets/_Project/ScriptableObjects/Enemies/`
2. `Create > RogueLite > Enemy Definition`
3. Nome: "Enemy_Basic"
4. Configure:
   ```
   Max HP: 20
   Move Speed: 2
   Damage: 5
   XP Reward: 1
   ```

### 4.4 Upgrade (Arma Inicial)
1. `Assets/_Project/ScriptableObjects/Upgrades/`
2. `Create > RogueLite > Upgrade Definition`
3. Nome: "Upgrade_BasicShot"
4. Configure:
   ```
   Upgrade Type: NewWeapon
   Weapon: Weapon_BasicShot
   Weight: 100
   ```

### 4.5 Stage
1. `Assets/_Project/ScriptableObjects/Stages/`
2. `Create > RogueLite > Stage Definition`
3. Nome: "Stage_Test"
4. Configure:
   ```
   Duration: 300 (5 min)
   Max Active Enemies: 50
   
   Waves > Size: 1
   [0] Start Time: 0
   [0] Spawn Rate: 1
   [0] Available Enemies: [Enemy_Basic]
   ```

---

## 🔨 PASSO 5: CRIAR PREFABS (4 min)

### 5.1 XP Orb
1. Hierarchy → Create Empty → "XPOrb"
2. Add Component:
   - Rigidbody2D (Gravity: 0)
   - Circle Collider 2D (Trigger: true, Radius: 0.3)
   - Sprite Renderer (qualquer sprite amarelo)
   - Script: XPOrb
3. Salve em `Assets/_Project/Prefabs/Drops/XPOrb.prefab`
4. Delete da Hierarchy

### 5.2 Projectile
1. Create Empty → "Projectile_Basic"
2. Add Component:
   - Rigidbody2D (Gravity: 0)
   - Circle Collider 2D (Trigger: true, Radius: 0.2)
   - Sprite Renderer (sprite azul/branco pequeno)
   - Script: Projectile
   - Tag: Projectile
3. Salve em `Assets/_Project/Prefabs/Projectiles/`
4. Delete da Hierarchy
5. **Conecte ao Projectile_Basic SO** (campo Prefab)

### 5.3 Enemy
1. Create Empty → "Enemy_Basic"
2. Add Component:
   - Rigidbody2D (Gravity: 0, Freeze Rotation Z)
   - Circle Collider 2D (Radius: 0.5)
   - Sprite Renderer (sprite vermelho)
   - Script: Enemy
   - Tag: Enemy
   - Layer: Enemy
3. Salve em `Assets/_Project/Prefabs/Enemies/`
4. Delete da Hierarchy
5. **Conecte ao Enemy_Basic SO** (campo Prefab)

---

## 🎬 PASSO 6: MONTAR CENA (5 min)

### 6.1 Bootstrap
1. Create Empty → "Bootstrap"
2. Add Component: Bootstrap

### 6.2 Player
1. Create Empty → "Player"
2. Tag: Player, Layer: Player
3. Add Component:
   - Rigidbody2D (Gravity: 0, Freeze Rotation Z)
   - Circle Collider 2D (Radius: 0.4)
   - Sprite Renderer (sprite azul/branco)
   - PlayerController
   - PlayerStats
   - PlayerCombat

### 6.3 Managers
1. Create Empty → "XPManager"
   - Add Script: XPManager
   - XP Orb Prefab: Arraste XPOrb prefab

2. Create Empty → "LevelSystem"
   - Add Script: LevelSystem

3. Create Empty → "UpgradeSystem"
   - Add Script: UpgradeSystem
   - All Upgrades: Size 1, arraste Upgrade_BasicShot

4. Create Empty → "EnemySpawner"
   - Add Script: EnemySpawner
   - Stage Definition: Stage_Test
   - Player: Arraste o GameObject Player

### 6.4 Camera
- Já existe
- Projection: Orthographic
- Size: 10

### 6.5 UI (Simples)
1. Create → UI → Canvas
2. Dentro do Canvas:
   - UI → Text - TextMeshPro (nome: "DebugText")
   - Anchor: Top-Left
   - Text: "Use WASD to move"

*(UI completa: veja SCENE_SETUP_GUIDE.md)*

---

## ⚡ PASSO 7: ARMA INICIAL (1 min)

**Problema:** Player começa sem arma!

**Solução rápida:**

1. No GameObject "Player", no Inspector
2. Script "PlayerCombat"
3. Add no `Start()` (via código OU):

Crie um script `StartingWeaponGiver.cs`:

```csharp
using UnityEngine;
using RogueLite.Data;
using RogueLite.Gameplay.Player;

public class StartingWeaponGiver : MonoBehaviour
{
    [SerializeField] private WeaponDefinition startingWeapon;
    
    private void Start()
    {
        if (startingWeapon != null)
        {
            var combat = GetComponent<PlayerCombat>();
            combat?.AddWeapon(startingWeapon);
        }
    }
}
```

Adicione ao Player e conecte Weapon_BasicShot.

---

## 🎮 PASSO 8: TESTAR!

1. **Save Scene** (`Ctrl+S`) em `Assets/_Project/Scenes/Game.unity`
2. **Build Settings** → Add Open Scenes
3. **PLAY!**

### O que deve acontecer:
✅ Player move com WASD  
✅ Arma dispara automaticamente  
✅ Inimigos spawnam e perseguem  
✅ Projéteis matam inimigos  
✅ XP orbs aparecem  
✅ XP é coletado  

---

## 🐛 TROUBLESHOOTING

### Player não move
- Verifique Rigidbody2D (Gravity Scale: 0)
- Verifique Input settings (Edit > Project Settings > Input Manager)

### Arma não dispara
- Verifique se StartingWeaponGiver está conectado
- Console: procure erros

### Inimigos não spawnam
- Verifique EnemySpawner: Stage conectado?
- Verifique Stage: Waves configuradas?
- Verifique Enemy_Basic: Prefab conectado?

### Projéteis não causam dano
- Physics 2D Matrix: Projectile colide com Enemy?
- Tags corretas? (Enemy tag, Projectile tag)

### Console cheio de erros
- Leia o erro
- Verifique referências null no Inspector
- Confirme que todos os SOs têm prefabs conectados

---

## 🚀 PRÓXIMOS PASSOS

### Adicionar UI completa
Siga: **SCENE_SETUP_GUIDE.md** (seção UI)

### Adicionar mais conteúdo
Siga: **SCRIPTABLEOBJECT_EXAMPLES.md**

### Balanceamento
Leia: **README.md** (seção Parâmetros)

---

## 🎉 PARABÉNS!

Você tem um roguelite funcional!

Agora é só adicionar:
- Mais armas (5-10)
- Mais inimigos (5-8)
- Mais upgrades (15-20)
- VFX e SFX
- Polish!

**Divirta-se desenvolvendo! 🎮**
