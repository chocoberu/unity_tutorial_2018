using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    private float h = 0.0f;
    private float v = 0.0f;

    private Transform tr; // 접근해야 하는 컴포넌트는 반드시 변수에 할당한 후 사용
    public float moveSpeed = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>(); // tr에 트랜스폼 컴포넌트 할당
    }

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        Debug.Log("h = " + h.ToString());
        Debug.Log("v = " + v.ToString());

        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h); // 전후좌우 이동 방향 벡터 계산
        //Translate(이동방향 * 속도 * 변위값 * Time.deltaTime, 기준좌표)
        tr.Translate(moveDir.normalized * moveSpeed * Time.deltaTime, Space.Self); //self는 로컬좌표
    }
}
