using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace playerMove { // 동일한 이름의 스크립트와 중복을 방지하기 위해 네임스페이스 정의
    public class PlayerController : MonoBehaviour // 플레이어 조작 스크립트(유니티 기본 클래스 상속)
    {
        public GameObject camera;          // 제어할 카메라 오브젝트
        public CharacterController player; // 제어할 플레이어 오브젝트

        public Vector3 PlayerDirection;    // 플레이어의 방향 저장 변수
        private bool jumping;              // 점프 상태 저장 변수
        
        public float Speed;                // 이동 속도 저장 변수
        public float JumpPow;              // 점프력 저장 변수
        private float Gravity;             // 중력 가속도 저장 변수  
        
        public void Start() // 스크립트의 시작과 같이 시작되는 함수
        {
            // 기본 값 설정
            Speed = 5.0f;
            Gravity = 10.0f;
            PlayerDirection = Vector3.zero;
            JumpPow = 5.0f;
            jumping = false;
        }
        
        public void Update() // 매 프레임마다 실행되는 함수
        {
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                var offset = camera.transform.forward;
                offset.y = 0;
                transform.LookAt(player.transform.position + offset);
            }

            // 캐릭터가 바닥에 붙어 있으면(붙어 있지 않을 때는 방향 전환이 불가능하므로 바닥에 붙이 있는지를 확인해야 함)
            if (player.isGrounded)
            {
                // 키보드에 따른 X, Z 축 이동방향을 새로 결정합니다.
                PlayerDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
                // 오브젝트가 바라보는 앞방향으로 이동방향을 돌려서 조정합니다.
                PlayerDirection = player.transform.TransformDirection(PlayerDirection);
                // 속도를 곱해서 적용합니다.
                PlayerDirection *= Speed;

                // 스페이스 버튼에 따른 점프 : 최종 점프버튼이 눌려있지 않았던 경우만 작동
                if (jumping == false && Input.GetButton("Jump"))
                {
                    jumping = true;
                    PlayerDirection.y = JumpPow;
                }
            }
            // 캐릭터가 바닥에 붙어 있지 않다면
            else
            {
                // 중력의 영향을 받아 아래쪽으로 하강합니다.           
                PlayerDirection.y -= Gravity * Time.deltaTime;
            }

            // 점프버튼이 눌려지지 않은 경우
            if (!Input.GetButton("Jump"))
            {
                jumping = false;  // 최종점프 버튼 눌림 상태 해제
            }
            // 앞 단계까지는 캐릭터가 이동할 방향만 결정하였으며,
            // 실제 캐릭터의 이동은 여기서 담당합니다.
            player.Move(PlayerDirection * Time.deltaTime);
        }
    }
}
