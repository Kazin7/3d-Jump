# 3D 액션 프로젝트 (WIP)

플레이어의 이동/점프/달리기, 체력·스태미너 UI, 환경 조사, 점프대, 아이템 시스템, 3인칭 시점, 무빙 플랫폼 탑승,사다리까지 구현된 Unity 프로젝트입니다.

## ✨ 주요 기능

- **기본 이동 & 점프** — `Input System`, `Rigidbody (ForceMode)`  
  - WASD 이동, Space 점프, Shift 달리기(스태미너 소모).
- **체력/스태미너 UI** — `UI`  
  - 달리기 시 스태미너 감소, 0이면 달리기 불가.
- **동적 환경 조사** — `Raycast`, `OverlapSphere`, `UI`  
  - 시야/주변 오브젝트의 이름·설명을 HUD에 표시.
- **점프대** — `Rigidbody.AddForce(ForceMode.Impulse)`  
  - 밟으면 위로 강하게 튀어 오름.
- **아이템 데이터** — `ScriptableObject`  
  - 이름/설명/효과 등 아이템 정의를 데이터 에셋으로 관리.
- **아이템 사용** — `Coroutine`  
  - 일정 시간 동안 지속되는 버프(예: 스피드 부스트).
- **3인칭 시점**  
  - 마우스 이동으로 시점 회전, 카메라 상하 각도 제한.
- **무빙 플랫폼 탑승**  
  - 시간에 따라 이동하는 발판 구현.  
  - 플레이어가 위에 서면 자연스럽게 함께 이동(플랫폼 속도 가산 방식).
- **사다리**  
  - 트리거를 활용하여 플레이어가 사다리 앞에서 ws를 통해 올라가고 내려가도록 설계


## 🧩 기술 스택

- **엔진**: Unity (2021.3+ 권장)  
- **패키지**: Input System, TextMeshPro(권장)  
- **코어**: Rigidbody 물리, Raycast/OverlapSphere, Coroutine, ScriptableObject, Unity UI, Animator

## 🎮 조작법

| 동작 | 키 |
|---|---|
| 이동 | W / A / S / D |
| 달리기 | Left Shift |
| 점프 | Space |
| 시점 회전 | 마우스 이동 |
| 상호작용 | E |

## 📁 프로젝트 구조

| 폴더/파일 경로 | 설명 |
|---|---|
| `Assets/Animations/` | 캐릭터/환경 애니메이션 클립, 애니메이터 컨트롤러 |
| `Assets/Externals/` | 외부 에셋(사운드, 머티리얼 등 서드파티 리소스) |
| `Assets/Image/` | UI 스프라이트, 아이콘 등 이미지 리소스 |
| `Assets/InputActions/` | Input System 액션 맵(`*.inputactions`) |
| `Assets/Prefabs/` | 재사용 프리팹(예: `Player`, `MovingPlatform`, `JumpPad`) |
| `Assets/Scenes/` | 씬 파일(예: `MainScene.unity`) |
| `Assets/ScriptableObject/` | 아이템/설정 데이터 `ScriptableObject` 에셋 |
| `Assets/Scripts/` | C# 스크립트 루트 |
| ├─ `Player/` | `PlayerController.cs`, `PlayerCondition.cs` 등 플레이어 로직 |
| ├─ `Environment/` | `MovingBlock.cs`, `JumpPad.cs` 등 환경/기믹 |
| ├─ `UI/` | `UICondition.cs`, 조사 HUD 등 UI 스크립트 |
| └─ `Items/` | `ItemData.cs`, 버프/아이템 사용 로직 |
| `Assets/Supercyan Character Pack Free Sample/` | 샘플 캐릭터 리소스(모델/애니메이션) |
| `Assets/TextMesh Pro/` | TMP 리소스, 폰트 에셋 |
| `Packages/` | 패키지 매니페스트/캐시(자동 생성/관리) |



## ⚙️ 셋업 가이드

1. **프로젝트 열기**  
   - Unity Hub에서 열기 (2021.3 LTS 이상 권장).
2. **Input System 활성화**  
   - `Edit > Project Settings > Player > Active Input Handling`을 **Input System** 또는 **Both**로 설정.  
   - 플레이어 오브젝트에 `Player Input`(또는 직접 스크립트 바인딩)을 연결.
3. **카메라 세팅**  
   - `PlayerController.cameraContainer`에 카메라 피벗(빈 오브젝트) 또는 카메라 트랜스폼 할당.
4. **UI**  
   - 체력/스태미너 바: `UICondition`에 슬라이더/이미지 참조 연결.
5. **무빙 플랫폼**  
   - `MovingBlock` 컴포넌트를 **플랫폼 오브젝트**에 붙임.  
   - `carrierRoot`에 플랫폼을 담는 Empty(캐리어)를 지정(없으면 부모/자기 자신을 사용).  
   - **플레이어가 올라타면** `MovingBlock`이 공개하는 `PlatformVelocity`를 `PlayerController`가 받아 이동에 더해 동일하게 움직임.




## 🐞 알려진 이슈

- 특정 물리 설정(Interpolation/Collision Detection) 조합에서 고속 플랫폼 위 미세 떨림이 발생할 수 있음 → Rigidbody Interpolate 권장.  
- 부모-자식만으로는 물리 객체가 플랫폼을 완전히 추종하지 않음 → 현재는 **플랫폼 속도 가산 방식**으로 해결.

