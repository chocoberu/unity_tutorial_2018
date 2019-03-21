using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCtrl : MonoBehaviour
{

    public GameObject bullet; // 총알 프리팹
    public Transform firePos; // 총알 발사 좌표
    public ParticleSystem cartridge; // 탄피 추출 파티클
    private ParticleSystem muzzleFlash; // 총구 화염 파티클

    // Start is called before the first frame update
    void Start()
    { 
        muzzleFlash = firePos.GetComponentInChildren<ParticleSystem>(); // FirePos 하위 컴포넌트 추출
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Fire(); // 마우스 좌측버튼 클릭시 총알 발사
        }
    }
    void Fire()
    {
        Instantiate(bullet, firePos.position, firePos.rotation); // Bullet 프리팹을 동적 생성
        cartridge.Play(); // 파티클 실행
        muzzleFlash.Play(); // 총구 화염 파티클 실행
    }
}
