﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    // Start is called before the first frame update
    // 적 캐릭터의 상태를 표한하기 위한 열거형 변수 정의
    public enum State
    {
        PATROL, TRACE, ATTACK,DIE
    }
    // 상태를 저장할 변수
    public State state = State.PATROL;

    // 주인공의 위치를 저장할 변수
    private Transform playerTr;
    // 적 캐릭터의 위치를 저장할 변수 
    private Transform enemyTr;

    // 공격 사정거리
    public float attackDist = 5.0f;
    // 추적 사정거리
    public float traceDist = 10.0f;

    // 사망 여부를 판단할 변수
    public bool isDie = false;

    // 코루틴에서 사용할 지연시간 변수
    private WaitForSeconds ws;

    private void Awake()
    {
        // 주인공 게임오브젝트 추출
        var player = GameObject.FindGameObjectWithTag("PLAYER");
        // 주인공 Transform 컴포넌트 추출
        if (player != null)
            playerTr = player.GetComponent<Transform>();
        // 적 캐릭터의 Transform 컴포넌트 추출
        enemyTr = GetComponent<Transform>();

        // 코루틴의 지연시간 생성
        ws = new WaitForSeconds(0.3f);
    }
    void OnEnable()
    {
        // CheckState 코루틴 함수 실행
        StartCoroutine(CheckState());
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
            if(dist <= attackDist)
            {
                state = State.ATTACK;
            }
            // 추적 사정거리 이내인 경우 
            else if(dist <= traceDist)
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

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}