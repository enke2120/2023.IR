using System;
using UnityEngine;

public class Client : HNET.CHNetConnector // 클라이언트 클래스 정의(HNetwork 클래스 상속)
{
    public bool connect = false;             // 서버와의 연결 여부를 저장하는 변수
    public bool already_in_transmit = false; // 서버와의 통신 진행 여부를 저장하는 변수
    
    public GameObject player;                   // 유니티의 플레이어 오브젝트를 저장하는 변수
    public Vector3 PlayerDirection;             // 플레이어의 방향을 저장하는 변수
    public int[] Key_inputs = new int[2]{0, 0}; // 수직, 수평 방향키 입력 확인용 배열
    public bool already_jumped = false;         // 플레이어 오브젝트가 점프 중인지 저장하는 변수
        
    public float previous_angle; // 방향 패킷을 전송하는 순간 플레이어가 바라보는 방향(각도)
    public float current_angle;  // 방향 패킷을 전송한 이후에 플레이어가 바라보는 방향(각도)
    
    public void OnGUI() // 유니티 화면의 버튼 기능 설정
    {
        // 접속 버튼을 누르면
        if (GUI.Button(new Rect(200, 100, 300, 100), "접속"))
        {
            Connect();      // 서버와 클라이언트 연결
            connect = true; // 연결을 '참'으로 저장
        }

        // 끊기 버튼을 누르면
        if (GUI.Button(new Rect(200, 220, 300, 100), "끊기")) 
        {
            Disconnect();    // 서버와 클라이언트 연결 끊기
            connect = false; // 연결을 '거짓'으로 저장
        }

        // 보내기 버튼을 누르면
        if (GUI.Button(new Rect(200, 340, 300, 100), "보내기")) 
        {
            // 보내는 중이 아니라면
            if (already_in_transmit == false) 
            {
                InvokeRepeating("transmit_position", 0f, 1.0f); // 서버로 위치 패킷 즉시 전송, 1초를 주기로 계속 전송
                already_in_transmit = true;                     // 보내는 중이라 저장
            }
            // 보내는 중이라면
            else 
            { 
                CancelInvoke("transmit_position"); // 패킷 전송 중지
                already_in_transmit = false;       // 보내는 중이 아니라 저장
            }
        }
    }
   
    public void FixedUpdate() // 주기적으로 반복되는 함수(기본 설정 0.02초 주기)
    { 
        // 점프 중이 아닐 때 점프 키가 입력되었다면
        if (already_jumped == false && Input.GetButton("Jump"))
        {
            transmit_jump();             // 점프 패킷 송신
            Invoke("input_reset", 1.0f); // 1초 후에 방향 초기화
            already_jumped = true;
        }

        // 방향키가 입력되었다면
        if (Key_inputs[0] != Input.GetAxisRaw("Vertical") || Key_inputs[1] != Input.GetAxisRaw("Horizontal"))
        {
            Key_inputs[0] = (int)Input.GetAxisRaw("Vertical");   // 배열 갱신
            Key_inputs[1] = (int)Input.GetAxisRaw("Horizontal"); // ''
            transmit_direction();                           // 방향 패킷 송신
        }

        // 현재 플레이어가 바라보는 방향 저장
        current_angle = player.transform.eulerAngles.y; 

        // 현재 방향이 방향 패킷 전송 당시의 방향과 3도 만큼 차이 나면
        if (Math.Abs(previous_angle - current_angle) > 3) 
        {
            previous_angle = current_angle; // 방향 갱신
            transmit_direction();           // 방향 패킷 송신
        }
    }

    public void input_reset() // 키 입력 상태 초기화
    {
        Key_inputs[0] = 0;
        Key_inputs[1] = 0;
        already_jumped = false;
    }

    public void transmit_position() // 위치 패킷 송신 함수
    {
        // 777 타입의 패킷 송신
        HNET.NewPacket Out = new HNET.NewPacket(777);

        // 플레이어 오브젝트의 위치를 송신
        Out.In(player.transform.position.x);
        Out.In(player.transform.position.y);
        Out.In(player.transform.position.z);
        Send(Out);

        // 서버와의 연결이 해제되었으면
        if (connect == false) 
        {
            CancelInvoke("transmit_position"); // 패킷 전송 중지
            already_in_transmit = false;       // 보내는 중이 아니라 저장
        }
    }

    public void transmit_direction() // 방향 패킷 송신 함수
    {
        // 999 타입의 패킷 송신
        HNET.NewPacket Out = new HNET.NewPacket(999); 

        // 플레이어의 이동 경로 계산
        PlayerDirection.x = Key_inputs[1];
        PlayerDirection.z = Key_inputs[0];
        PlayerDirection = player.transform.TransformDirection(PlayerDirection);
        PlayerDirection *= 5.0f; // 속력 연산
        
        // 플레이어 오브젝트의 방향을 송신
        Out.In(PlayerDirection.x);
        Out.In(PlayerDirection.z);
        Send(Out);

        // 방향 패킷 송신 시점에서 플레이어가 바라보는 방향 저장
        previous_angle = player.transform.eulerAngles.y;

        // 서버와의 연결이 해제되었으면
        if (connect == false) 
        {
            CancelInvoke("transmit_position"); // 패킷 전송 중지
        }
    }

    public void transmit_jump() // 점프 패킷 송신 함수
    {
        // 555 타입의 패킷 송신
        HNET.NewPacket Out = new HNET.NewPacket(555); 
        Send(Out);
    }
}
