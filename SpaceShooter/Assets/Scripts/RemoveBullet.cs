using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBullet : MonoBehaviour
{
    public GameObject sparkEffect; // 스파크 프리팹을 저장할 변수

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "BULLET")
        {
            ShowEffect(collision); // 스파크 효과 함수 호출
            Destroy(collision.gameObject); // 만약 부딛친 물체의 태그가 BULLET이라면 그 물체를 제거한다
        }
    }
    void ShowEffect(Collision collision)
    {
        ContactPoint contact = collision.contacts[0]; // 충돌 지점의 정보를 추출
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal); // 법선 벡터가 이루는 회전 각도를 추출

        Instantiate(sparkEffect, contact.point, rot); // 스파크 효과를 생성
    }
}
