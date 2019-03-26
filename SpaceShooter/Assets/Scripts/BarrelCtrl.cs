using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelCtrl : MonoBehaviour
{
    public GameObject expEffect; // 폭발 효과 프리팹을 저장할 변수
    private int hitCount; // 총알 맞은 횟수
    private Rigidbody rb; // Rigidbody 컴포넌트를 저장할 변수

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Rigidbody 컴포넌트를 추출해 저장
    }

    private void OnCollisionEnter(Collision coll) // 충돌이 발생했을 때 한번 호출되는 콜백 함수
    {
        if(coll.collider.CompareTag("BULLET")) // 충돌한 게임오브젝트가 BULLET이면
        {
            if(++hitCount == 3)
            {
                ExpBarrel(); // hitCount을 증가시키고 hitCount가 3이면 폭발 효과
            }
        }
    }

    void ExpBarrel() // 폭발효과 함수
    {
        GameObject effect = Instantiate(expEffect, transform.position, Quaternion.identity); // 폭발 효과 프리팹을 동적으로 생성
        Destroy(effect, 2.0f); // 2초 후 효과 삭제
        rb.mass = 1.0f; // mass를 수정하여 무게를 가볍게 함
        rb.AddForce(Vector3.up * 1000.0f); // 위로 솟구치는 힘을 가함
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
