using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // 인스펙터 뷰에 노출됨
public class PlayerAnim
{
    public AnimationClip idle;
    public AnimationClip runF;
    public AnimationClip runB;
    public AnimationClip runL;
    public AnimationClip runR;
}
public class PlayerCtrl : MonoBehaviour
{
    private float h = 0.0f;
    private float v = 0.0f;
    private float r = 0.0f;

    private Transform tr; // 접근해야 하는 컴포넌트는 반드시 변수에 할당한 후 사용
    public float moveSpeed = 10.0f; // 이동속도
    public float rotSpeed = 80.0f; // 회전속도

    public PlayerAnim playerAnim; // 인스펙터 뷰에 표시할 애니메이션 클래스 변수
    [HideInInspector]
    public Animation anim; // Animation 컴포넌트를 저장하기 위한 변수

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>(); // tr에 트랜스폼 컴포넌트 할당
        anim = GetComponent<Animation>(); // anim에 Animation 컴포넌트 할당
        anim.clip = playerAnim.idle; // Animation 컴포넌트의 애니메이션 클립을 지정하고 실행
        anim.Play();
    }

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        r = Input.GetAxis("Mouse X");

        Debug.Log("h = " + h.ToString());
        Debug.Log("v = " + v.ToString());

        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h); // 전후좌우 이동 방향 벡터 계산
        //Translate(이동방향 * 속도 * 변위값 * Time.deltaTime, 기준좌표)
        tr.Translate(moveDir.normalized * moveSpeed * Time.deltaTime, Space.Self); //self는 로컬좌표
        tr.Rotate(Vector3.up * rotSpeed * Time.deltaTime * r);

        //키보드 입력값을 기준으로 동작할 애니메이션 수행
        if (v >= 0.1f)
        {
            anim.CrossFade(playerAnim.runF.name, 0.1f); // 전진 애니메이션
        }
        else if(v <= -0.1f)
        {
            anim.CrossFade(playerAnim.runB.name, 0.1f); // 후진 애니메이션
        }
        else if(h >= 0.1f)
        {
            anim.CrossFade(playerAnim.runR.name, 0.1f); // 오른쪽 이동 애니메이션
        }
        else if(h <= -0.1f)
        {
            anim.CrossFade(playerAnim.runL.name, 0.1f); // 왼쪽 이동 애니메이션
        }
        else
        {
            anim.CrossFade(playerAnim.idle.name, 0.1f); // 정지 시 idle 애니메이션
        }
    }
}
