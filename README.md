
```
survival
│  ├─ Prefabs // Bao gom Model, Texture cua cac Game Object
│  │  ├─ Enemies
│  │  │  ├─ Chapter01
│  │  │  ├─ Chapter02
│  │  │  ├─ Chapter03
│  │  ├─ Enviroment
│  │  ├─ Object Pooling
│  │  ├─ Player
│  ├─ Scripts
│  │  ├─ Enemy // Behaviors and Settings cua Enemy 
│  │  ├─ Events // Events trong game
│  │  ├─ Chapters // Chapter Settings, quan ly load chapter, map va spaw enemy
│  │  ├─ Manager // Game Manager, TODO: Xoa folder nay va chi giu GameManager o Scripts Folder
│  │  ├─ HUD // UIManager va cac UIElements
│  │  ├─ Player // PlayerSettings, Saved Data
│  │  ├─ Hero // HeroSettings e.g Dante, etc
│  │  ├─ Patterns // Patterns e.g - Singleton
│  │  ├─ Skills // SkillSystem, SkillSettings, SkillBehaviors 
│  │  ├─ Utils // Static Class Extensions
```

## Skill System

Flow: (https://sequencediagram.org/)
```
title Skill System

note over UIManager: Buy a Skill When Starting
UIManager -> SkillSystem : GetBuyableSkills()
UIManager ->UIManager: User Buy a Skill
UIManager ->SkillSystem: Apply Bought Skill
SkillSystem ->SkillSystem: Apply Skill to Player
SkillSystem -> PlayerSettings: Update Gold, BoughtSkills
PlayerSettings ->PlayerSettings: Save Data
note over UIManager: Select a Skill In Game
UIManager -> SkillSystem : GetNextSkillsToSelect
UIManager ->UIManager: User Select a Skill
UIManager -> SkillSystem: Apply Select Skill
SkillSystem -> SkillSystem: Apply Skill to Player
```
<img width="307" alt="image" src="https://user-images.githubusercontent.com/1218572/179698057-27962056-cbf5-43cf-82a0-4c7a327d12ba.png">


