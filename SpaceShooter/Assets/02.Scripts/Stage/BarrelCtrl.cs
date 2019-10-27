using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelCtrl : MonoBehaviour
{
    public GameObject expEffect; // 폭발 효과 프리팹을 저장할 변수
    private int hitCount = 0; // 총알 맞은 횟수
    private Rigidbody rb; // Rigidbody 컴포넌트를 저장할 변수
    public Mesh[] meshes; // 찌그러진 드럼통의 메쉬를 저장할 배열
    private MeshFilter meshFilter; // meshFilter 컴포넌트를 저장할 변수
    public Texture[] textures; // 드럼통의 텍스처를 저장할 배열
    private MeshRenderer _renderer; // MeshRenderer 컴포넌트를 저장할 변수

    public float expRadius = 10.0f; // 폭발 반경

    private AudioSource _audio; //AudioSource 컴포넌트를 저장할 변수
    public AudioClip expSfx; // 폭발음 오디오 클립

    private Shake shake; // Shake 클래스를 저장할 변수 


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Rigidbody 컴포넌트를 추출해 저장
        meshFilter = GetComponent<MeshFilter>(); // MeshFilter 컴포넌트를 추출해 저장
        _renderer = GetComponent<MeshRenderer>(); // MeshRenderer 컴포넌트를 추출해 저장
        _renderer.material.mainTexture = textures[Random.Range(0, textures.Length)]; // 난수를 발생시켜 불규칙적인 텍스처 적용
        _audio = GetComponent<AudioSource>(); // AudioSource 컴포넌트를 추출해 저장

        shake = GameObject.Find("CameraRig").GetComponent<Shake>(); // Shake 스크립트를 추출
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
        //rb.mass = 1.0f; // mass를 수정하여 무게를 가볍게 함
        //rb.AddForce(Vector3.up * 1000.0f); // 위로 솟구치는 힘을 가함

        IndirectDamage(transform.position); // 폭발력 생성

        int idx = Random.Range(0, meshes.Length); // 난수 발생시킴
        meshFilter.sharedMesh = meshes[idx]; // 찌그러진 메쉬를 적용
        GetComponent<MeshCollider>().sharedMesh = meshes[idx];

        _audio.PlayOneShot(expSfx, 1.0f); // 폭발음 발생

        StartCoroutine(shake.ShakeCamera(0.1f, 0.2f, 0.5f)); // 셰이크 효과 호출
    }
    
    void IndirectDamage(Vector3 pos)
    {
        Collider[] colls = Physics.OverlapSphere(pos, expRadius, 1 << 8); // 주변에 있는 드럼통을 모두 추출
        foreach(var coll in colls)
        {
            var _rb = coll.GetComponent<Rigidbody>(); // 폭발 범위에 포함된 드럼통의 Rigidbody 컴포넌트를 추출
            _rb.mass = 1.0f; // 드럼통의 무게를 가볍게 함
            _rb.AddExplosionForce(1200.0f, pos, expRadius, 1000.0f); // 폭발력을 전달
        }
    }
}
