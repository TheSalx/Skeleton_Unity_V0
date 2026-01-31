# GUIA DE SETUP DA CENA "GAME"

Este guia mostra como montar a cena jogável do zero.

---

## 1. CRIAR NOVA CENA

1. File > New Scene
2. Salve como "Game" em `Assets/_Project/Scenes/`

---

## 2. BOOTSTRAP (CORE SYSTEM)

### GameObject: Bootstrap
- Empty GameObject
- Nome: "Bootstrap"
- Componente: `Bootstrap.cs`
- Don't Destroy On Load: true (marque no Inspector)

---

## 3. CAMERA

### Main Camera
- Já existe na cena
- Projection: Orthographic
- Size: 8-12 (ajuste conforme preferência)
- Background: Cor escura (ex: #1a1a1a)

### Script: CameraFollow (opcional, crie se quiser)
```csharp
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10);

    private void LateUpdate()
    {
        if (target == null) return;
        Vector3 desired = target.position + offset;
        Vector3 smoothed = Vector3.Lerp(transform.position, desired, smoothSpeed * Time.deltaTime);
        transform.position = smoothed;
    }
}
```

---

## 4. PLAYER

### GameObject: Player
- Empty GameObject
- Nome: "Player"
- Tag: **Player**
- Layer: **Player**
- Position: (0, 0, 0)

### Componentes:
1. **Rigidbody2D**
   - Body Type: Dynamic
   - Gravity Scale: 0
   - Constraints: Freeze Rotation Z

2. **CircleCollider2D**
   - Radius: 0.4-0.5

3. **SpriteRenderer**
   - Sprite: Círculo branco/azul (placeholder)
   - Sorting Layer: "Player" (crie se não existir)

4. **Scripts:**
   - `PlayerController.cs`
   - `PlayerStats.cs`
   - `PlayerCombat.cs`

### Child: WeaponContainer (opcional)
- Empty GameObject
- Nome: "WeaponContainer"
- Parent: Player
- Position: (0, 0, 0)
- Arraste para o campo "Weapon Container" em PlayerCombat

---

## 5. MANAGERS

### GameObject: GameManagers
- Empty GameObject (apenas para organização)
- Nome: "--- MANAGERS ---"

### Child: XPManager
- Nome: "XPManager"
- Script: `XPManager.cs`
- XP Orb Prefab: Arraste seu prefab de XP Orb (veja seção Prefabs)

### Child: LevelSystem
- Nome: "LevelSystem"
- Script: `LevelSystem.cs`
- Base XP Required: 10
- XP Scaling Factor: 1.5

### Child: UpgradeSystem
- Nome: "UpgradeSystem"
- Script: `UpgradeSystem.cs`
- All Upgrades: Arraste todos os UpgradeDefinitions criados

### Child: EnemySpawner
- Nome: "EnemySpawner"
- Script: `EnemySpawner.cs`
- Stage Definition: Arraste seu StageDefinition
- Player: Arraste o GameObject Player

---

## 6. UI CANVAS

### GameObject: Canvas
- UI > Canvas
- Render Mode: Screen Space - Overlay
- Canvas Scaler:
  - UI Scale Mode: Scale With Screen Size
  - Reference Resolution: 1920x1080

### CanvasScaler + GraphicRaycaster já vem automaticamente

---

## 7. HUD (dentro do Canvas)

### GameObject: HUD
- Empty GameObject
- Nome: "HUD"
- RectTransform: Anchor Stretch (toda tela)

### Child: HPBar
- UI > Slider
- Anchor: Top-Left
- Position: (20, -20, 0)
- Width: 300, Height: 30
- Configure:
  - Min Value: 0
  - Max Value: 1
  - Value: 1
  - Interactable: false
- Personalize cores (Fill = vermelho/verde)

### Child: HPText (dentro do HPBar)
- UI > Text - TextMeshPro
- Position: Center do slider
- Text: "100 / 100"
- Alignment: Center

### Child: XPBar
- UI > Slider
- Anchor: Bottom-Center
- Position: (0, 20, 0)
- Width: 600, Height: 20
- Min/Max/Value: 0, 1, 0
- Interactable: false
- Cores: Fill = amarelo/dourado

### Child: LevelText
- UI > Text - TextMeshPro
- Anchor: Top-Left
- Position: (20, -60, 0)
- Text: "Lv 1"
- Font Size: 36

### Child: TimerText
- UI > Text - TextMeshPro
- Anchor: Top-Center
- Position: (0, -20, 0)
- Text: "00:00"
- Font Size: 48
- Alignment: Center

### Adicione o Script GameHUD
- No GameObject "HUD", adicione `GameHUD.cs`
- Arraste as referências:
  - HP Bar → Slider do HPBar
  - HP Text → Text do HPText
  - XP Bar → Slider do XPBar
  - Level Text → LevelText
  - Timer Text → TimerText

---

## 8. LEVEL UP UI (dentro do Canvas)

### GameObject: LevelUpPanel
- UI > Panel
- Nome: "LevelUpPanel"
- RectTransform: Anchor Stretch (toda tela)
- Background: Cor escura semi-transparente (ex: #000000, Alpha: 200)
- **Desative este GameObject** (será ativado pelo script)

### Child: Title
- UI > Text - TextMeshPro
- Anchor: Top-Center
- Position: (0, -100, 0)
- Text: "LEVEL UP!"
- Font Size: 72
- Alignment: Center

### Child: UpgradeCardsContainer
- Empty GameObject
- Nome: "UpgradeCardsContainer"
- Anchor: Center
- Layout: Horizontal Layout Group
  - Spacing: 30
  - Child Alignment: Middle Center

### Child: UpgradeCard_1, UpgradeCard_2, UpgradeCard_3 (dentro do Container)
Para cada card:
- UI > Button
- Width: 300, Height: 400
- Adicione componente `UpgradeCard` (subclass em LevelUpUI.cs)

#### Estrutura interna de cada Card:
1. **Background (Image)**
   - Fill: branco
   
2. **RarityBorder (Image)**
   - Anchor: Stretch all
   - Border colorido (será mudado pelo script)
   
3. **Icon (Image)**
   - Top-Center
   - Size: 128x128
   
4. **NameText (TextMeshPro)**
   - Below icon
   - Font Size: 24
   - Alignment: Center
   
5. **DescriptionText (TextMeshPro)**
   - Below name
   - Font Size: 16
   - Alignment: Center
   - Word Wrap: true

### Adicione o Script LevelUpUI
- No GameObject "LevelUpPanel", adicione `LevelUpUI.cs`
- Panel: Arraste o próprio LevelUpPanel
- Upgrade Cards: Array de 3, arraste os 3 UpgradeCard GameObjects

---

## 9. GAME OVER UI (dentro do Canvas)

### GameObject: GameOverPanel
- UI > Panel
- Nome: "GameOverPanel"
- RectTransform: Anchor Stretch
- Background: Preto semi-transparente
- **Desative este GameObject**

### Child: TitleText
- UI > Text - TextMeshPro
- Anchor: Top-Center
- Position: (0, -150, 0)
- Text: "GAME OVER"
- Font Size: 96
- Alignment: Center

### Child: StatsText
- UI > Text - TextMeshPro
- Anchor: Center
- Text: "Time: 00:00\nLevel: 1\nKills: 0"
- Font Size: 36
- Alignment: Center

### Child: RestartButton
- UI > Button
- Anchor: Bottom-Center
- Position: (0, 150, 0)
- Width: 300, Height: 80
- Text: "Restart"

### Child: MainMenuButton
- UI > Button
- Anchor: Bottom-Center
- Position: (0, 50, 0)
- Width: 300, Height: 80
- Text: "Main Menu"

### Adicione o Script GameOverUI
- No GameObject "GameOverPanel", adicione `GameOverUI.cs`
- Conecte todas as referências

---

## 10. PREFABS NECESSÁRIOS

### XP Orb Prefab
1. Crie GameObject "XPOrb"
2. Adicione:
   - `Rigidbody2D` (Gravity Scale: 0)
   - `CircleCollider2D` (Is Trigger: true, Radius: 0.3)
   - `SpriteRenderer` (sprite: círculo amarelo pequeno)
   - Script: `XPOrb.cs`
3. Salve como prefab em `Assets/_Project/Prefabs/Drops/XPOrb.prefab`
4. Delete da cena

### Projectile Prefabs
Para cada tipo de projétil:
1. GameObject com nome descritivo (ex: "Projectile_MagicBolt")
2. Componentes:
   - `Rigidbody2D` (Gravity: 0)
   - `CircleCollider2D` ou `BoxCollider2D` (Is Trigger: true)
   - `SpriteRenderer` (sprite placeholder: círculo/seta colorida)
   - Script: `Projectile.cs`
   - Tag: "Projectile"
3. Salve em `Assets/_Project/Prefabs/Projectiles/`

### Enemy Prefabs
Para cada tipo de inimigo:
1. GameObject (ex: "Enemy_Slime")
2. Componentes:
   - `Rigidbody2D` (Gravity: 0, Freeze Rotation Z)
   - `CircleCollider2D` (não trigger)
   - `SpriteRenderer` (sprite placeholder: quadrado/círculo vermelho)
   - Script: `Enemy.cs`
   - Tag: "Enemy"
   - Layer: "Enemy"
3. Salve em `Assets/_Project/Prefabs/Enemies/`

### Orbital Weapon Prefabs
1. GameObject (ex: "Orbital_Blade")
2. Componentes:
   - `CircleCollider2D` (Is Trigger: true)
   - `SpriteRenderer` (sprite: lâmina/estrela pequena)
   - Script: `OrbitalWeapon.cs`
3. Salve em `Assets/_Project/Prefabs/Projectiles/`

---

## 11. CENÁRIO (OPCIONAL)

### Tilemap ou Background simples
- Adicione um Quad/Plane grande com textura ou cor de fundo
- Ou use Tilemap para criar chão
- Não precisa de colisões (é roguelike open field)

---

## 12. CHECKLIST FINAL

- [ ] Tags configuradas: Player, Enemy, Projectile
- [ ] Layers configurados: Player, Enemy, Projectile, Pickup
- [ ] Physics 2D Matrix configurada
- [ ] Bootstrap na cena com script
- [ ] Player completo com 3 scripts
- [ ] 4 Managers: XPManager, LevelSystem, UpgradeSystem, EnemySpawner
- [ ] UI: HUD, LevelUpPanel, GameOverPanel com scripts
- [ ] Prefabs: XPOrb, pelo menos 1 projectile, 1 enemy
- [ ] ScriptableObjects: 2 armas, 2 projéteis, 3 upgrades, 2 inimigos, 1 stage
- [ ] Referências conectadas no Inspector

---

## 13. TESTAR

1. Aperte Play
2. Player deve se mover com WASD
3. Inimigos devem começar a spawnar após alguns segundos
4. Colete XP dos inimigos mortos (se tiver arma)
5. Ao subir de nível, UI de upgrade deve aparecer
6. Selecione um upgrade
7. Continue jogando até morrer ou vencer

**Se algo não funcionar:**
- Verifique Console para erros
- Confirme que todas as referências estão conectadas
- Verifique se criou pelo menos 1 arma e adicionou aos upgrades
- Certifique-se de que StageDefinition tem waves configuradas

---

**Cena montada! Agora é só adicionar conteúdo e balancear.**
