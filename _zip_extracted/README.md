# 🎮 RogueLite Unity - Projeto Completo

**Roguelite top-down inspirado em Vampire Survivors**

---

## 📋 O Que É Este Projeto?

Este é um **projeto Unity 6 completo e jogável** de um roguelite top-down com:

- ✅ Sistema de movimento e combate
- ✅ Spawn de inimigos com ondas progressivas
- ✅ Auto-ataque com múltiplas armas
- ✅ Sistema de XP e level up
- ✅ Upgrades aleatórios a cada nível
- ✅ Arquitetura data-driven com ScriptableObjects
- ✅ Object Pooling para performance
- ✅ Sistema de eventos desacoplado
- ✅ **30 scripts C# completos**
- ✅ **Documentação extensa**

---

## 🚀 Como Usar Este Projeto

### Opção 1: Setup Rápido (15 minutos)
Siga o guia: **[Assets/_Project/QUICK_START.md](Assets/_Project/QUICK_START.md)**

### Opção 2: Setup Completo (30 minutos)
Siga o guia: **[Assets/_Project/SCENE_SETUP_GUIDE.md](Assets/_Project/SCENE_SETUP_GUIDE.md)**

### Opção 3: Entender Arquitetura Primeiro
Leia: **[Assets/_Project/PROJECT_OVERVIEW.md](Assets/_Project/PROJECT_OVERVIEW.md)**

---

## 📚 Documentação

Toda a documentação está em `Assets/_Project/`:

1. **[INDEX.md](Assets/_Project/INDEX.md)** - Índice de toda documentação
2. **[QUICK_START.md](Assets/_Project/QUICK_START.md)** - Começar em 15 minutos
3. **[PROJECT_OVERVIEW.md](Assets/_Project/PROJECT_OVERVIEW.md)** - Visão geral do projeto
4. **[README.md](Assets/_Project/README.md)** - Referência completa (tutoriais, FAQ)
5. **[SCENE_SETUP_GUIDE.md](Assets/_Project/SCENE_SETUP_GUIDE.md)** - Como montar a cena
6. **[SCRIPTABLEOBJECT_EXAMPLES.md](Assets/_Project/SCRIPTABLEOBJECT_EXAMPLES.md)** - Exemplos prontos
7. **[IMPLEMENTATION_CHECKLIST.md](Assets/_Project/IMPLEMENTATION_CHECKLIST.md)** - Checklist

---

## 🗂️ Estrutura do Projeto

```
/app/
├── Assets/_Project/
│   ├── Scripts/                 (30 scripts C#)
│   │   ├── Core/               (Bootstrap, EventBus, Pooling)
│   │   ├── Gameplay/           (Player, Enemies, Combat, Progression)
│   │   ├── UI/                 (HUD, LevelUp, GameOver)
│   │   └── Utils/              (Extensions, Helpers)
│   ├── ScriptableObjects/      (Templates para dados)
│   ├── Prefabs/                (Player, Enemies, Projectiles)
│   ├── Scenes/                 (Cena Game)
│   └── Docs/                   (7 arquivos .md)
└── README.md                   (Este arquivo)
```

---

## ⚙️ Requisitos

- **Unity 6** (6000.3.3f1) ou superior
- **TextMeshPro** (vem com Unity)
- Nenhuma dependência externa

---

## 🎯 Features Implementadas

### Core
- ✅ Bootstrap system
- ✅ Service Locator
- ✅ Event Bus
- ✅ Object Pooling

### Gameplay
- ✅ Player com movimento top-down
- ✅ Sistema de HP e dano
- ✅ Auto-ataque com cooldown
- ✅ 2 tipos de armas (Projectile, Orbital)
- ✅ Projéteis com comportamentos variáveis
- ✅ Knockback e crit system
- ✅ AI de inimigos (seek player)
- ✅ Spawn system com waves
- ✅ Curvas de dificuldade

### Progression
- ✅ XP orbs com magnet effect
- ✅ Level up system
- ✅ 4 tipos de upgrades (NewWeapon, LevelUp, PassiveStat, Evolution)
- ✅ Seleção aleatória com pesos

### UI
- ✅ HUD completo
- ✅ Level Up screen
- ✅ Game Over / Victory screen

---

## 📖 Como Adicionar Conteúdo

### Criar Nova Arma
Veja: [Assets/_Project/README.md](Assets/_Project/README.md) (seção "Como Adicionar Conteúdo")

### Criar Novo Inimigo
Veja: [Assets/_Project/README.md](Assets/_Project/README.md) (seção "Como Adicionar Conteúdo")

### Criar Novo Upgrade
Veja: [Assets/_Project/README.md](Assets/_Project/README.md) (seção "Como Adicionar Conteúdo")

### Exemplos Prontos
Veja: [Assets/_Project/SCRIPTABLEOBJECT_EXAMPLES.md](Assets/_Project/SCRIPTABLEOBJECT_EXAMPLES.md)

---

## 🧪 Como Testar

1. Copie `Assets/_Project/` para seu projeto Unity 6
2. Configure Tags, Layers e Physics 2D (veja QUICK_START.md)
3. Crie pelo menos 1 arma, 1 inimigo, 1 upgrade, 1 stage (veja SCRIPTABLEOBJECT_EXAMPLES.md)
4. Monte a cena Game (veja SCENE_SETUP_GUIDE.md)
5. Aperte Play!

---

## 🏗️ Arquitetura

- **Data-Driven**: Todo conteúdo em ScriptableObjects
- **Baixo Acoplamento**: Interfaces e ServiceLocator
- **Event-Driven**: EventBus para comunicação
- **Performance-First**: Object Pooling desde o início
- **Extensível**: Fácil adicionar conteúdo sem mexer em código

Mais detalhes: [Assets/_Project/PROJECT_OVERVIEW.md](Assets/_Project/PROJECT_OVERVIEW.md)

---

## 🐛 Troubleshooting

Problemas comuns e soluções: [Assets/_Project/QUICK_START.md](Assets/_Project/QUICK_START.md) (seção Troubleshooting)

---

## 📜 Scripts Principais

**30 scripts C# organizados em:**
- Core (5): Bootstrap, ServiceLocator, EventBus, ObjectPoolManager, Constants
- Player (4): Controller, Stats, Combat, Input
- Combat (5): Weapon, Projectile, Orbital, Interfaces, StatBlock
- Enemies (2): Enemy, Spawner
- Progression (4): XP, Level, Upgrades
- Definitions (5): Weapon, Projectile, Enemy, Upgrade, Stage
- UI (3): HUD, LevelUp, GameOver
- Utils (2): Extensions, Debug

---

## 🎮 Como Jogar (Depois de Configurado)

- **WASD**: Movimento
- **Automático**: Ataque
- **Mouse**: Selecionar upgrades ao subir de nível
- **Objetivo**: Sobreviver o máximo possível

---

## 📞 Suporte

Consulte a documentação em `Assets/_Project/`:
- Para começar: **QUICK_START.md**
- Para dúvidas: **README.md** (FAQ)
- Para referência: **INDEX.md**

---

## 🏆 Checklist de Implementação

Use: [Assets/_Project/IMPLEMENTATION_CHECKLIST.md](Assets/_Project/IMPLEMENTATION_CHECKLIST.md)

---

## 🚀 Próximos Passos

Após o setup básico:
1. Adicionar mais armas (5-10)
2. Adicionar mais inimigos (5-8)
3. Adicionar mais upgrades (15-20)
4. Polish (VFX, SFX, animações)
5. Balanceamento
6. Meta-progression
7. Boss fights
8. Menu principal

---

## 📄 Licença

Este projeto é um template educacional. Use livremente para seus projetos.

---

## 🎯 Objetivo do Projeto

Fornecer um **projeto Unity roguelite completo, funcional e extensível** com:
- Código limpo e organizado
- Arquitetura sólida
- Documentação extensa
- Fácil de entender e expandir

**Pronto para desenvolver seu próprio roguelite! 🎮**

---

**Desenvolvido para Unity 6 - Arquivo gerado em 2025**
