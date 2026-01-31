# PROJECT OVERVIEW - ROGUELITE UNITY

## 📋 Sumário Executivo

**Tipo:** Roguelite top-down action (estilo Vampire Survivors)  
**Engine:** Unity 6 (6000.3.3f1)  
**Linguagem:** C#  
**Arquitetura:** Data-driven com ScriptableObjects  

---

## 🎯 Features Implementadas

### Core Systems
✅ Bootstrap e inicialização de sistemas  
✅ ServiceLocator para dependências globais  
✅ EventBus para comunicação desacoplada  
✅ Object Pooling para performance  

### Gameplay
✅ Player com movimento top-down (WASD)  
✅ Sistema de HP, dano e invulnerabilidade  
✅ Sistema de combate auto-fire  
✅ 2 tipos de armas: Projectile e Orbital  
✅ Projéteis com comportamentos variáveis (Straight, Homing)  
✅ Knockback e crit system  

### Enemies
✅ AI simples (seek player)  
✅ Spawn system com waves  
✅ Curvas de dificuldade (HP, Damage, Spawn Rate)  
✅ Pool de inimigos otimizado  

### Progression
✅ XP Orbs com magnet effect  
✅ Level up system com curva exponencial  
✅ Upgrade system com 4 tipos:  
   - NewWeapon  
   - WeaponLevelUp  
   - PassiveStat  
   - Evolution  
✅ Seleção aleatória de 3 upgrades por level  
✅ Sistema de raridade e pesos  

### UI
✅ HUD completo (HP, XP, Level, Timer)  
✅ Level Up UI com cards  
✅ Game Over / Victory UI  

---

## 📚 Documentação

1. **README.md** - Documentação principal  
2. **SCENE_SETUP_GUIDE.md** - Como montar a cena passo a passo  
3. **SCRIPTABLEOBJECT_EXAMPLES.md** - Exemplos de configs  
4. **IMPLEMENTATION_CHECKLIST.md** - Checklist completo  
5. **PROJECT_OVERVIEW.md** - Este arquivo  

---

## 📁 Estrutura de Arquivos

```
Assets/_Project/
├── Scripts/ (30 arquivos C#)
│   ├── Core/ (5 arquivos)
│   ├── Gameplay/ (18 arquivos)
│   ├── UI/ (3 arquivos)
│   └── Utils/ (2 arquivos)
├── ScriptableObjects/ (5 pastas)
├── Prefabs/ (5 pastas)
├── Scenes/
└── Docs/ (5 arquivos .md)
```

---

## 🛠️ Scripts Principais

### Core (5)
- Bootstrap.cs
- ServiceLocator.cs
- EventBus.cs
- ObjectPoolManager.cs
- GameConstants.cs

### Player (4)
- PlayerController.cs
- PlayerStats.cs
- PlayerCombat.cs
- IInputProvider.cs

### Combat (5)
- WeaponRuntime.cs
- Projectile.cs
- OrbitalWeapon.cs
- CombatInterfaces.cs
- StatBlock.cs

### Enemies (2)
- Enemy.cs
- EnemySpawner.cs

### Progression (4)
- XPOrb.cs
- XPManager.cs
- LevelSystem.cs
- UpgradeSystem.cs

### ScriptableObjects (5)
- WeaponDefinition.cs
- ProjectileDefinition.cs
- UpgradeDefinition.cs
- EnemyDefinition.cs
- StageDefinition.cs

### UI (3)
- GameHUD.cs
- LevelUpUI.cs
- GameOverUI.cs

### Utils (2)
- Extensions.cs
- DebugHelper.cs

**Total: 30 scripts**

---

## 📦 Dependências

- **Unity 6.0** (6000.3.3f1) ou superior
- **TextMeshPro** (built-in)
- **Nenhuma dependência externa**

---

## 🎮 Fluxo de Jogo

1. **Bootstrap** inicializa sistemas
2. **Player** spawna na cena
3. **EnemySpawner** começa a spawnar inimigos baseado em waves
4. **Player** ataca automaticamente com armas equipadas
5. **Inimigos** morrem e dropam XP
6. **XPManager** spawna XP Orbs
7. **Player** coleta XP e sobe de nível
8. **LevelSystem** pausa e mostra UI de upgrade
9. **UpgradeSystem** oferece 3 opções aleatórias
10. **Player** seleciona upgrade
11. Loop continua até morte ou vitória
12. **GameOverUI** aparece com stats

---

## 🔌 Arquitetura

### Padrões Utilizados
- **Service Locator** para sistemas globais
- **Event Bus** para comunicação desacoplada
- **Object Pool** para performance
- **Data-Driven Design** com ScriptableObjects
- **Separação Config/Runtime** (SO vs MonoBehaviour)
- **Interface Segregation** (IDamageable, ITargetProvider, etc)

### Princípios
- Baixo acoplamento
- Alta extensão via dados
- Performance-first (pooling desde o início)
- Inspector-friendly (configurável sem código)

---

## 🚀 Como Começar

1. Leia **README.md**
2. Siga **SCENE_SETUP_GUIDE.md**
3. Use **SCRIPTABLEOBJECT_EXAMPLES.md** para criar conteúdo
4. Marque itens em **IMPLEMENTATION_CHECKLIST.md**
5. Jogue e teste!

---

## 📊 Status do Projeto

✅ **Core completo** - Todos os sistemas funcionais  
✅ **Gameplay loop completo** - Spawn, combat, progression  
✅ **UI completa** - HUD, LevelUp, GameOver  
✅ **Data-driven** - ScriptableObjects implementados  
✅ **Documentação completa** - 5 arquivos .md  
✅ **Extensibilidade** - Fácil adicionar conteúdo  

### Próximos Passos Sugeridos
🟡 Adicionar mais conteúdo (10+ armas, 20+ upgrades)  
🟡 Polish (VFX, SFX, animações)  
🟡 Meta-progression (upgrades permanentes)  
🟡 Boss fights  
🟡 Múltiplos stages  
🟡 Menu principal  
🟡 Persistência de dados  
🟡 Achievements  

---

## 📝 Notas Técnicas

### Performance
- Usa pooling para 100% dos objetos dinâmicos
- Máximo recomendado: 200 inimigos simultâneos
- Otimizado para 60 FPS em hardware médio

### Extensão
- **Nova arma**: Crie WeaponDefinition + ProjectileDefinition
- **Novo inimigo**: Crie EnemyDefinition + Prefab
- **Novo upgrade**: Crie UpgradeDefinition
- **Novo comportamento**: Estenda enums e adicione cases

### Input
- Interface `IInputProvider` isola input system
- Fácil trocar para novo Input System
- Suporta gamepad via Input Manager

---

## 👥 Créditos

**Desenvolvido para Unity 6**  
**Arquitetura:** Modular, extensiva, data-driven  
**Inspiração:** Vampire Survivors  

---

## 💬 Suporte

Se tiver dúvidas:
1. Leia README.md
2. Verifique SCENE_SETUP_GUIDE.md
3. Consulte SCRIPTABLEOBJECT_EXAMPLES.md
4. Revise IMPLEMENTATION_CHECKLIST.md

---

**🎮 Projeto pronto para desenvolvimento e expansão! 🎮**
