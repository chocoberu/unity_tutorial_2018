using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    // Start is called before the first frame update
    // 적 캐릭터의 상태를 표한하기 위한 열거형 변수 정의
    public enum State
    {
        PATROL, TRACE, ATTACK, DIE
    }
    // 상태를 저장할 변수
    public State state = State.PATROL;

    // 주인공의 위치를 저장할 변수
    private Transform playerTr;
    // 적 캐릭터의 위치를 저장할 변수 
    private Transform enemyTr;
    private Animator animator; // Animator 컴포넌트를 저장할 변수

    // 공격 사정거리
    public float attackDist = 5.0f;
    // 추적 사정거리
    public float traceDist = 10.0f;

    // 사망 여부를 판단할 변수
    public bool isDie = false;

    // 코루틴에서 사용할 지연시간 변수
    private WaitForSeconds ws;

    // 이동을 제어하는 MoveAgent 클래스를 저장할 변수
    private MoveAgent moveAgent;

    private EnemyFire enemyFire; // 총알 발사를 제어하는 EnemyFire 클래스를 저장할 변수
    // 애니메이터 컨트롤러에 정의한 파라미터의 해시값을 미리 추출
    private readonly int hashMove = Animator.StringToHash("IsMove");
    private readonly int hashSpeed = Animator.StringToHash("Speed");
    private readonly int hashDie = Animator.StringToHash("Die");
    private readonly int hashDieIdx = Animator.StringToHash("DieIdx");

    private void Awake() // 가장 먼저 수행되는 함수 
    {
        // 주인공 게임오브젝트 추출
        var player = GameObject.FindGameObjectWithTag("PLAYER");
        // 주인공 Transform 컴포넌트 추출
        if (player != null)
            playerTr = player.GetComponent<Transform>();
        // 적 캐릭터의 Transform 컴포넌트 추출
        enemyTr = GetComponent<Transform>();
        animator = GetComponent<Animator>(); // Animator 컴포넌트 추출

        // 코루틴의 지연시간 생성
        ws = new WaitForSeconds(0.3f);
        moveAgent = GetComponent<MoveAgent>(); // 이동을 제어하는 MoveAgent 클래스를 추출
        enemyFire = GetComponent<EnemyFire>(); // 총알 발사를 제어하는 EnemyFire 클래스를 추출
    }
    void OnEnable() // Awake 이후 Statr 이전에 실행되는 함수
    {
        // CheckState 코루틴 함수 실행
        StartCoroutine(CheckState());
        StartCoroutine(Action()); // Action 코루틴 실행
    }
    IEnumerator CheckState()
    {
        // 적 캐릭터가 사망하기 전까지 도는 무한루프
        while (!isDie)
        {
            if (state == State.DIE) // 상태가 사망이면 코루틴 함수를 종료시킴
                yield break;
            float dist = Vector3.Distance(playerTr.position, enemyTr.position); // 주인공과 적 캐릭터 간의 거리를 계산

            // 공격 사정거리 이내인 경우
            if (dist <= attackDist)
            {
                state = State.ATTACK;
            }
            // 추적 사정거리 이내인 경우 
            else if (dist <= traceDist)
            {
                state = State.TRACE;
            }
            else
            {
                state = State.PATROL;
            }

            yield return ws; // 0.3초 동안 대기하는 동안 제어권을 양보
        }
    }

    IEnumerator Action()
    {
        while(!isDie) // 적 캐릭터가 사망할 때 까지 무한루프
        {
            yield return ws; 

            switch(state) // 상태에 따라 분기 처리
            {
                case State.PATROL: // 순찰 모드 활성화
                    enemyFire.isFire = false; // 총알 발사 정지
                    moveAgent.patrolling = true;
                    animator.SetBool(hashMove, true);
                    break;
                case State.TRACE: // 주인공의 위치를 넘겨 추적 모드로 변경
                    enemyFire.isFire = false; // 총알 발사 정지
                    moveAgent.traceTarget = playerTr.position;
                    animator.SetBool(hashMove, true);
                    break;
                case State.ATTACK: // 순찰 및 추적을 정지
                    moveAgent.Stop();
                    animator.SetBool(hashMove, false);
                    if (enemyFire.isFire == false) // 총알 발사 시작
                        enemyFire.isFire = true;
                    break;
                case State.DIE:
                    isDie = true;
                    enemyFire.isFire = false; // 순찰 및 추적을 정지
                    moveAgent.Stop();
                    animator.SetInteger(hashDieIdx, Random.Range(0,3)); // 사망 애니메이션의 종류를 지정
                    animator.SetTrigger(hashDie);
                    GetComponent<CapsuleCollider>().enabled = false; // Capsule Collider 컴포넌트 비활성화

                    break;

            }
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat(hashSpeed, moveAgent.speed); // Speed 파라미터에 이동 속도를 전달 
    }
}
