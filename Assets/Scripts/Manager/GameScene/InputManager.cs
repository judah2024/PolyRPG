using System;
using System.Collections;
using EnumDef;
using UnityEngine;
using UnityEngine.InputSystem;

// TODO : 계획
/*
 * [Completed] 1. 적AI 완성
 *      - 순찰 : 목적지 이동 -> 일정시간 대기(정지, 탐색 애니메이션 재생) -> 새 목적지 지정 -> 반복 (부분 완성)
 *      - 전투 : (공격 범위 밖 : 추격(Player 위치로 이동) -> (공격 범위 안 : 공격 -> 일정시간 대기)
 *      - 피격 : 피격 애니메이션 재생 -> 일정시간 정지 -> 전투 상태
 *      - 사망 : (isAlive == false) 상태를 만들 필요 없음, 사망이 모든 행동 정지 -> 일정시간 후 제거
 * [Completed] 2. 적 스폰 시스템
 *      - 스폰 : 주기적으로 생성, 스포너의 생성 최대치면 대기, 스포너의 자식 몬스터가 사망하면 일정시간 뒤 생성
 * [Completed] 3. 스텟 시스템 
 *      - 기본적인 구현 : 공격력, 방어력, 체력
 * [Completed] 4. 드롭 시스템
 *      - 적 사망시 드롭아이템 생성
 *      - TableManager에 DropTable를 만들어 데이터에 따라 아이템 생성
 *      - DropTable : 적ID, (확률, 아이템 Resources 경로(3D 오브젝트 Prefab))
 * [Completed] 5. 타이틀 연출
 *      - 맵을 보여주는 연출 : TimeLine?, 위치 이동시 페이드 효과도 넣고싶다...
 */
/* [Completed] 6. 알림 완성
 *      - 드롭 알림이 Manager는 아니지... UI에 옮겨 주자
 * [Completed] 7. 불필요한 스크립트 정리
 *      - 버튼 스크립트 : 현재 버튼들의 기능이 단순하므로, 개별 스크립트는 과하다. 버튼의 부모 오브젝트가 관리하도록
 *      - Manager : 매니저가 너무 많다! 광범위한 호출이 필요한 녀석만 Manager로 할것
 *      - 그 외 : 쓰지 않는 스크립트는 없에자
 */

public class InputManager : MonoBehaviour
{
    static public InputManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public InputAction skipTextAction;
    
    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }

    public bool isInAction;
    public Action OnDodge;
    public Action OnAttack;

    
    private Coroutine coExecuteAction;

    void Start()
    {
        PlayerInput input = GetComponent<PlayerInput>();
        InputActionMap uiMap = input.actions.FindActionMap("UI");
        skipTextAction = uiMap.FindAction("SkipText");
    }
    

    /// <summary>
    /// Move input 값을 갱신하는 함수
    /// </summary>
    /// <param name="context"> Move 액션이 발생할 경우의 컨텍스트</param>
    public void UpdateMoveInput(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// Look input 값을 갱신하는 함수
    /// </summary>
    /// <param name="context"> Look 액션이 발생할 경우의 컨텍스트</param>
    public void UpdateLookInput(InputAction.CallbackContext context)
    {
        LookInput = context.ReadValue<Vector2>();
    }
    
    /// <summary>
    /// 회피키 입력에 이벤트를 발생시킨다.
    /// </summary>
    /// <param name="context"> 회피키 입력 컨텍스트</param>
    public void TriggerDodgeInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ExecuteAction(OnDodge);
        }
    }

    public void TriggerAttackInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ExecuteAction(OnAttack);
        }
    }

    public void ExecuteAction(Action action)
    {
        if (coExecuteAction != null)
        {
            StopCoroutine(coExecuteAction);
        }

        coExecuteAction = StartCoroutine(CoExecuteAction(action));
    }
    
    IEnumerator CoExecuteAction(Action action)
    {
        yield return new WaitUntil(() => isInAction == false);
        if (Mng.play.state == GameState.Playing)
        {
            action?.Invoke();
            isInAction = true;
        }
    }
}
