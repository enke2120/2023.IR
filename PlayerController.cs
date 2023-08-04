using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject Cam; // 제어할 캐릭터 컨트롤러
    public CharacterController SelectPlayer; // 제어할 캐릭터 컨트롤러
    public float Speed;  // 이동속도
    public float JumpPow;

    private float Gravity; // 중력   
    public Vector3 MoveDir; // 캐릭터의 움직이는 방향.
    private bool JumpButtonPressed;  //  최종 점프 버튼 눌림 상태

    // Start is called before the first frame update
    public void Start()
    {
        // 기본값
        Speed = 5.0f;
        Gravity = 10.0f;
        MoveDir = Vector3.zero;
        JumpPow = 5.0f;
        JumpButtonPressed = false;
    }
    
    // Update is called once per frame
    public void Update()
    {
        if (SelectPlayer == null) return;

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            var offset = Cam.transform.forward;
            offset.y = 0;
            transform.LookAt(SelectPlayer.transform.position + offset);
        }
        // 캐릭터가 바닥에 붙어 있는 경우만 작동합니다.
        // 캐릭터가 바닥에 붙어 있지 않다면 바닥으로 추락하고 있는 중이므로
        // 바닥 추락 도중에는 방향 전환을 할 수 없기 때문입니다.
        if (SelectPlayer.isGrounded)
        {
            // 키보드에 따른 X, Z 축 이동방향을 새로 결정합니다.
            MoveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            // 오브젝트가 바라보는 앞방향으로 이동방향을 돌려서 조정합니다.
            MoveDir = SelectPlayer.transform.TransformDirection(MoveDir);
            // 속도를 곱해서 적용합니다.
            MoveDir *= Speed;

            // 스페이스 버튼에 따른 점프 : 최종 점프버튼이 눌려있지 않았던 경우만 작동
            if (JumpButtonPressed == false && Input.GetButton("Jump"))
            {
                SelectPlayer.transform.rotation = Quaternion.Euler(0, 45, 0);
                JumpButtonPressed = true;
                MoveDir.y = JumpPow;
            }
        }
        // 캐릭터가 바닥에 붙어 있지 않다면
        else
        {
            // 중력의 영향을 받아 아래쪽으로 하강합니다.           
            MoveDir.y -= Gravity * Time.deltaTime;
        }

        // 점프버튼이 눌려지지 않은 경우
        if (!Input.GetButton("Jump"))
        {
            JumpButtonPressed = false;  // 최종점프 버튼 눌림 상태 해제
        }
        // 앞 단계까지는 캐릭터가 이동할 방향만 결정하였으며,
        // 실제 캐릭터의 이동은 여기서 담당합니다.
        SelectPlayer.Move(MoveDir * Time.deltaTime);
    }

    // public float GetObjectRotation()
    // {
    //     return transform.eulerAngles.y;
    // }
}
