# Skill System
## Terms
- **Skill**: Data của một skill: name, id, prefab, etc
- **Attribute**: Thuộc tính của các Skill Level và Skill Behavior - damage, speed
- **ModifierOperator**: phép toán để modify giá trị của Attribute: Add, Multiply, Override
- **SkillLevel**: Data của một skill level: skillId, index, attributes (damage, speed,...) và modifyOperator
- **SkillBehavior**: Behavior của game object để thực hiện skill
- **SkillSlot**: Logic để gắn Skill Behavior nhất định vào game object, thay đổi giá trị của Skill Behavior dựa theo Skill Level
- **SkillSystem**: Behavior quản lý các skills của một game object bảo gồm: Enable Skill, Disable Skill, thiết lập Input cho Skill Behavior
- **SkillLibrary**: SkillLibrary là một thư viện cho phép GET/SET các Skill và Skill Level của theo SkilId và LevelIndex. SkillLibrary được configure bằng ScriptableObject - SkillLibrarySettings và được Inject vào GameManager.

## Sequence Diagram
https://sequencediagram.org/ 
### Init Skill cho Hero

```
title Init MoveSkill cho Hero

HeroSkillSystem->MoveSkillSlot: AttachSkillSlot
note over MoveSkillSlot: Get Skill, Skill Level trong Library theo SkillId
MoveSkillSlot ->SkillLibrary: Get Skill and SkillLevel
note over MoveSkillSlot: Dùng data của Skill để thiết lập Behavior
MoveSkillSlot-> HeroGameObject: AddComponent<MoveSkillBehavior>
note over MoveSkillSlot: Dùng data của SkillLevel để modify attribute của Behavior
MoveSkillSlot-> MoveSkillBehavior: LevelUp(SkillLevel)
HeroSkillSystem->MoveSkillBehavior: SetInput
```

### Control SkillBehavior thông qua Input

```
title Control Behavior qua Input Stream

GameInput -> GameInput: Define MoveStream
MoveSkillBehavior->GameInput: Subscribe MoveStream
note over MoveSkillBehavior: Stream chỉ thực sự chạy khi  có Subscriber.
MoveSkillBehavior->MoveSkillBehavior: Unsubscribe khi Destroy

```

## NOTES:

### Performance
- Không nên dùng Abstract Class cho Skill Behavior - Các SkillBehavior nên luôn là sealed class
- Không nên tạo nhiều SkillBehavior giống nhau cho nhiều gameobject. Ví dụ MoveSkillBehavior cho 1 Enemy nên chuyển thành MoveSkillBehavior cho GroupEnemy nếu số lượng Enemy nhiều. Reference: https://blog.unity.com/technology/1k-update-calls

### Using POCO - KISS ( Keep It Simple Stupid)
- Skill, SkillLevel, SkillLibrary không nên là các ScriptableObject mà chỉ nên là các POCO (Plain Old C Object) - tức là các Data Object dùng để thay đổi các thuộc tính của Behavior.
- Nếu cần configure Skill, SkillLevel hoặc SkillLibrary qua Inspector thì có thể bọc bằng các ScriptableObject - SkillSettings, SkillLevelSettings, SkillLibrarySettings. Ngoài ra có thể đưa các POCO này vào các prefabs.
