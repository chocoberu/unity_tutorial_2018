using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PlayerSfx // 총알 발사와 재장전 오디오 클립을 저장할 구조체
{
    public AudioClip[] fire;
    public AudioClip[] reload;

}
public class FireCtrl : MonoBehaviour
{

    public enum WeaponType
    {
        RIFLE = 0,
        SHOTGUN
    }
    public WeaponType currWeapon = WeaponType.SHOTGUN; // 주인공이 현재 들고 있는 무기를 저장할 변수

    public GameObject bullet; // 총알 프리팹
    public Transform firePos; // 총알 발사 좌표
    public ParticleSystem cartridge; // 탄피 추출 파티클
    private ParticleSystem muzzleFlash; // 총구 화염 파티클
    private AudioSource _audio; // AudioSource 컴포넌트를 저장할 변수
    public PlayerSfx playerSfx; // 오디오 클립을 저장할 변수

    // Start is called before the first frame update
    void Start()
    { 
        muzzleFlash = firePos.GetComponentInChildren<ParticleSystem>(); // FirePos 하위 컴포넌트 추출
        _audio = GetComponent<AudioSource>();
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
        FireSfx(); // 사운드 발생
    }
    void FireSfx()
    {
        var _sfx = playerSfx.fire[(int)currWeapon]; // 현재 들고 있는 무기의 오디오 클립을 가져옴
        _audio.PlayOneShot(_sfx, 1.0f); // 사운드 발생
    }
}
