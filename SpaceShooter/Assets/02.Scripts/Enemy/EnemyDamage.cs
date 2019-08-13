using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{

    private const string bulletTag = "BULLET";
    private float hp = 100.0f; // 생명 게이지
    private GameObject bloodEffect; // 피격 시 사용할 혈흔 효과

    // Start is called before the first frame update
    void Start()
    {
        bloodEffect = Resources.Load<GameObject>("BulletImpactFleshBigEffect"); // 혈흔 효과 프리팹을 로드
    }

    void OnCollisionEnter(Collision coll)
    {
        if(coll.collider.tag == bulletTag)
        {
            ShowBloodEffect(coll); // 혈흔 효과를 생성하는 함수 호출
            Destroy(coll.gameObject); // 총알 삭제
            hp -= coll.gameObject.GetComponent<BulletCtrl>().damage; // 생명 게이지 차감

            if(hp <= 0.0f)
            {
                GetComponent<EnemyAI>().state = EnemyAI.State.DIE; // 적 캐릭터의 상태를 DIE로 변경
            }
        }
    }
    // Update is called once per frame
    
    void ShowBloodEffect(Collision coll)
    {
        Vector3 pos = coll.contacts[0].point; // 총알이 충돌한 지점 산출
        Vector3 _normal = coll.contacts[0].normal; // 총알이 충돌했을 때 법선 벡터
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, _normal); // 총알의 충돌 시 방향 벡터의 회전값 계산

        GameObject blood = Instantiate<GameObject>(bloodEffect, pos, rot); //  혈흔 효과 생성
        Destroy(blood, 1.0f);
    }
}
