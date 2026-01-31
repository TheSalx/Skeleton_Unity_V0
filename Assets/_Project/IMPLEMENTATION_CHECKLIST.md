# CHECKLIST DE IMPLEMENTAÇÃO

Use este checklist para garantir que implementou tudo corretamente.

---

## ☑️ FASE 1: SETUP INICIAL DO PROJETO

- [ ] Projeto Unity 6 criado
- [ ] Pasta `Assets/_Project/` copiada
- [ ] Scripts compilando sem erros
- [ ] TextMeshPro importado (Window > TextMeshPro > Import TMP Essential Resources)

---

## ☑️ FASE 2: CONFIGURAÇÃO DO UNITY

### Tags
- [ ] Tag "Player" criada
- [ ] Tag "Enemy" criada
- [ ] Tag "Projectile" criada

### Layers
- [ ] Layer "Player" criado
- [ ] Layer "Enemy" criado
- [ ] Layer "Projectile" criado
- [ ] Layer "Pickup" criado

### Physics 2D Matrix
- [ ] Player colide com Enemy
- [ ] Enemy colide com Player + Projectile
- [ ] Projectile colide com Enemy
- [ ] Pickup colide com Player
- [ ] Outras colisões desabilitadas

---

## ☑️ FASE 3: SCRIPTABLEOBJECTS

### Projectiles
- [ ] Criado pelo menos 1 ProjectileDefinition
- [ ] Prefab de projétil criado e conectado

### Weapons
- [ ] Criado pelo menos 2 WeaponDefinitions
- [ ] Arma tipo Projectile configurada
- [ ] Arma tipo Orbital configurada (opcional)
- [ ] Projectiles conectados às armas

### Enemies
- [ ] Criado pelo menos 2 EnemyDefinitions
- [ ] Prefabs de inimigos criados e conectados
- [ ] Stats balanceadas (HP, damage, speed)

### Upgrades
- [ ] Criado upgrade NewWeapon para cada arma
- [ ] Criado upgrade WeaponLevelUp para cada arma
- [ ] Criado pelo menos 2 upgrades PassiveStat
- [ ] Pesos (weight) configurados

### Stage
- [ ] Criado 1 StageDefinition
- [ ] Waves configuradas (mínimo 2)
- [ ] Inimigos adicionados às waves
- [ ] Curvas de dificuldade ajustadas

---

## ☑️ FASE 4: PREFABS

### XP Orb
- [ ] Prefab criado
- [ ] Rigidbody2D + CircleCollider2D (trigger)
- [ ] Script XPOrb.cs adicionado
- [ ] Sprite amarelo/dourado

### Projectiles
- [ ] Prefab para cada tipo de projétil
- [ ] Rigidbody2D + Collider2D (trigger)
- [ ] Script Projectile.cs adicionado
- [ ] Tag "Projectile"

### Enemies
- [ ] Prefab para cada tipo de inimigo
- [ ] Rigidbody2D + Collider2D (não trigger)
- [ ] Script Enemy.cs adicionado
- [ ] Tag "Enemy" + Layer "Enemy"
- [ ] Sprite diferenciado por tipo

### Orbital Weapons (se usar)
- [ ] Prefab de arma orbital
- [ ] Collider2D (trigger)
- [ ] Script OrbitalWeapon.cs

---

## ☑️ FASE 5: CENA "GAME"

### Bootstrap
- [ ] GameObject "Bootstrap" criado
- [ ] Script Bootstrap.cs adicionado

### Player
- [ ] GameObject "Player" criado
- [ ] Tag "Player" e Layer "Player"
- [ ] Rigidbody2D configurado (Gravity 0)
- [ ] CircleCollider2D adicionado
- [ ] SpriteRenderer com sprite
- [ ] PlayerController.cs
- [ ] PlayerStats.cs
- [ ] PlayerCombat.cs

### Managers
- [ ] GameObject "XPManager" com script
- [ ] XP Orb Prefab conectado
- [ ] GameObject "LevelSystem" com script
- [ ] GameObject "UpgradeSystem" com script
- [ ] All Upgrades array preenchido
- [ ] GameObject "EnemySpawner" com script
- [ ] StageDefinition conectado
- [ ] Player Transform conectado

### Camera
- [ ] Main Camera configurada (Orthographic)
- [ ] Size ajustado (8-12)
- [ ] Script CameraFollow (opcional)

---

## ☑️ FASE 6: UI

### Canvas
- [ ] Canvas criado (Screen Space - Overlay)
- [ ] CanvasScaler configurado (1920x1080)

### HUD
- [ ] GameObject "HUD" criado
- [ ] HPBar (Slider) criado e configurado
- [ ] HPText (TextMeshPro) criado
- [ ] XPBar (Slider) criado e configurado
- [ ] LevelText (TextMeshPro) criado
- [ ] TimerText (TextMeshPro) criado
- [ ] Script GameHUD.cs adicionado
- [ ] Todas as referências conectadas

### LevelUpUI
- [ ] GameObject "LevelUpPanel" criado (desativado)
- [ ] Background semi-transparente
- [ ] Title text criado
- [ ] 3 UpgradeCards criados
- [ ] Cada card tem: Icon, NameText, DescriptionText, RarityBorder, Button
- [ ] Script LevelUpUI.cs adicionado
- [ ] Referências conectadas

### GameOverUI
- [ ] GameObject "GameOverPanel" criado (desativado)
- [ ] TitleText criado
- [ ] StatsText criado
- [ ] RestartButton criado e conectado
- [ ] MainMenuButton criado e conectado
- [ ] Script GameOverUI.cs adicionado
- [ ] Referências conectadas

---

## ☑️ FASE 7: TESTE BÁSICO

- [ ] Cena salva
- [ ] Build Settings: Cena "Game" adicionada
- [ ] Apertou Play
- [ ] Player move com WASD
- [ ] Console sem erros críticos

---

## ☑️ FASE 8: TESTE COMPLETO

### Spawn & Combat
- [ ] Inimigos começam a spawnar
- [ ] Player recebe arma inicial (criar upgrade e aplicar manualmente ou ao iniciar)
- [ ] Arma ataca automaticamente
- [ ] Projéteis causam dano
- [ ] Inimigos morrem
- [ ] XP orbs spawnam

### Progression
- [ ] XP orbs são coletados
- [ ] XP Bar atualiza
- [ ] Level Up ocorre
- [ ] UI de upgrade aparece
- [ ] Jogo pausa
- [ ] 3 upgrades aparecem
- [ ] Ao clicar, upgrade é aplicado
- [ ] Jogo despausa

### Combat Feedback
- [ ] Player recebe dano de inimigos
- [ ] HP Bar atualiza
- [ ] Invulnerabilidade funciona
- [ ] Knockback funciona

### Game Over
- [ ] Player morre ao HP zerar
- [ ] GameOver UI aparece
- [ ] Botões funcionam

### Victory (se duration > 0)
- [ ] Ao completar duração do stage, Victory aparece

---

## ☑️ FASE 9: POLISH (OPCIONAL)

- [ ] Adicionados efeitos visuais (particles)
- [ ] Adicionados sons (SFX)
- [ ] Música de fundo
- [ ] Animações de UI
- [ ] Feedbacks visuais (screen shake, flash, etc)
- [ ] Mais armas (5-10 total)
- [ ] Mais inimigos (5-8 tipos)
- [ ] Mais upgrades (15-20 total)
- [ ] Boss fight (opcional)

---

## ☑️ FASE 10: BALANCEAMENTO

- [ ] XP progression testada (níveis 1-10)
- [ ] Dificuldade das waves ajustada
- [ ] Dano de armas balanceado
- [ ] HP de inimigos balanceado
- [ ] Curvas de dificuldade suaves
- [ ] Upgrades variados e úteis
- [ ] Nenhum upgrade "obrigatório" ou "inútil"

---

## ☑️ FASE 11: FINALIZAÇÃO

- [ ] Todas as referências conectadas
- [ ] Sem erros no Console
- [ ] Sem warnings críticos
- [ ] Build de teste gerado
- [ ] Jogo jogável do início ao fim
- [ ] README.md revisado

---

## 🎮 READY TO PLAY!

Se todos os checkboxes acima estão marcados, seu roguelite está funcional e pronto para expansão!

**Próximos passos:**
- Adicionar mais conteúdo
- Implementar meta-progression
- Adicionar persisência de dados
- Criar menu principal
- Adicionar múltiplos stages
- Boss fights
- Achievements
- Leaderboards

---
