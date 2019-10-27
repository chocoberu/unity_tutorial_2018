using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform target; // 추적할 대상
    public float moveDamping = 15.0f; // 이동 속도 계수
    public float rotateDamping = 10.0f; // 회전 속도 계수
    public float distance = 5.0f; // 추적 대상과의 거리
    public float height = 4.0f; // 추적 대상과의 높이
    public float targetOffset = 2.0f; // 추적 좌표의 오프셋

    private Transform tr; // CameraRig의 Transform 컴포넌트
    void Start()
    {
        tr = GetComponent<Transform>(); // CameraRig의 Transform 컴포넌트를 추출
    }

    
    void LateUpdate()
    {
        var camPos = target.position - (target.forward * distance) + (target.up * height); // 카메라의 높이와 거리를 계산
        tr.position = Vector3.Slerp(tr.position, camPos, Time.deltaTime * moveDamping); // 이동할 때의 속도 계수를 적용
        tr.rotation = Quaternion.Slerp(tr.rotation, target.rotation, Time.deltaTime * rotateDamping); // 회전할 때의 속도 계수를 적용
        tr.LookAt(target.position + (target.up * targetOffset)); // 카메라를 추적 대상으로 Z축을 회전시킴
    }

    void OnDrawGizmos() // 추적할 좌표를 시각적으로 표현
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(target.position + (target.up * targetOffset), 0.1f); // 추적 및 시야를 맞출 위치를 표시
        Gizmos.DrawLine(target.position + (target.up * targetOffset), transform.position); // 메인 카메라와 추적 지점 간의 선을 표시
    }
}
