using System;
using UnityEngine;

public class Client : HNET.CHNetConnector // 클라이언트 클래스 정의(HNetwork 클래스 상속)
{
    public bool connect = false; // 서버와의 연결 여부를 저장하는 변수
    public bool already_in_transmit = false; // 서버와의 통신 진행 여부를 저장하는 변수
    
    public GameObject player; // 유니티의 플레이어 오브젝트를 저장하는 변수
    public Vector3 PlayerDirection; // 플레이어의 방향을 저장하는 변수
    public int[] Key_inputs = new int[2]{0, 0}; // 수직, 수평 방향키 입력 확인용 배열
    public bool already_jumped = false; // 플레이어 오브젝트가 점프 중인지 저장하는 변수
        
    public float previous_angle; // 방향 패킷을 전송하는 순간 플레이어가 바라보는 방향(각도)
    public float current_angle; // 방향 패킷을 전송한 이후에 플레이어가 바라보는 방향(각도)
    
    public void OnGUI() // 유니티 화면의 버튼 기능 설정
    {
        if (GUI.Button(new Rect(200, 100, 300, 100), "접속")) // 접속 버튼을 누르면
        {
            Connect(); // 서버와 클라이언트 연결
            connect = true; // 연결을 '참'으로 저장
        }

        if (GUI.Button(new Rect(200, 220, 300, 100), "끊기")) // 끊기 버튼을 누르면
        {
            Disconnect(); // 서버와 클라이언트 연결 끊기
            connect = false; // 연결을 '거짓'으로 저장
        }

        if (GUI.Button(new Rect(200, 340, 300, 100), "보내기")) // 보내기 버튼을 누르면
        {
            if (already_in_transmit == false) // 보내는 중이 아니라면
            {
                InvokeRepeating("transmit_position", 0f, 1.0f); // 서버로 위치 패킷 즉시 전송, 1초를 주기로 계속 전송
                already_in_transmit = true; // 보내는 중이라 저장
            }
            else // 보내는 중이라면
            { 
                CancelInvoke("transmit_position"); // 패킷 전송 중지
                already_in_transmit = false; // 보내는 중이 아니라 저장
            }
        }
    }
   
    public void FixedUpdate() // 주기적으로 반복되는 함수(기본 설정 0.02초 주기)
    { 
        if (already_jumped == false && Input.GetButton("Jump")) // 점프 중이 아닐 때 점프 키가 입력되었다면
        {
            transmit_jump(); // 점프 패킷 송신
            Invoke("input_reset", 1.0f); // 1초 후에 방향 초기화
        }
        
        if (Key_inputs[0] != Input.GetAxisRaw("Vertical") || Key_inputs[1] != Input.GetAxisRaw("Horizontal")) // 방향키가 입력되었다면
        {
            transmit_direction(); // 방향 패킷 송신
            Key_inputs[0] = Input.GetAxisRaw("Vertical"); // 배열 갱신
            Key_inputs[1] = Input.GetAxisRaw("Horizontal"); // ''
        }

        current_angle = player.transform.eulerAngles.y; // 현재 플레이어가 바라보는 방향 저장

        if (Math.Abs(previous_angle - current_angle) > 3) // 현재 방향이 방향 패킷 전송 당시의 방향과 3도 만큼 차이 나면
        {
            previous_angle = current_angle; // 방향 갱신
            transmit_direction(); // 방향 패킷 송신
        }
    }

    public void input_reset() // 키 입력 상태 초기화
    {
        Key_inputs[0] = 0;
        Key_inputs[1] = 0;
    }

    public void transmit_position() // 위치 패킷 송신 함수
    {
        HNET.NewPacket Out = new HNET.NewPacket(777); // 777 타입의 패킷 송신

        // 플레이어 오브젝트의 위치를 송신
        Out.In(player.transform.position.x);
        Out.In(player.transform.position.y);
        Out.In(player.transform.position.z);
        Send(Out);

        if (Input.GetAxisRaw("Vertical") == 0 || Input.GetAxisRaw("Horizontal") == 0) {
            transmit_direction();
            Key_inputs[0] = Input.GetAxisRaw("Vertical");
            Key_inputs[1] = Input.GetAxisRaw("Horizontal");
        }

        // 연결이 '거짓'이면
        if (connect == false)
        {
            CancelInvoke("transmit_position"); // 패킷 전송 중지
            already_in_transmit = false; // 이미 보내는 중이 아니라 저장
        }
    }

    // 패킷 송신 함수
    public void transmit_direction()
    {
        // 999 타입의 패킷 송신
        HNET.NewPacket Out = new HNET.NewPacket(999);

        PlayerDirection.x = Input.GetAxisRaw("Horizontal");
        PlayerDirection.z = Input.GetAxisRaw("Vertical");
        PlayerDirection = player.transform.TransformDirection(PlayerDirection);
        PlayerDirection *= 5.0f;

        // 플레이어 오브젝트의 방향을 송신
        Out.In(PlayerDirection.x);
        Out.In(PlayerDirection.z);
        Send(Out);
        
        previous_angle = player.transform.eulerAngles.y;

        // 연결이 '거짓'이면
        if (connect == false)
        {
            CancelInvoke("transmit_position"); // 패킷 전송 중지
        }
    }

    public void transmit_jump()
    {
        HNET.NewPacket Out = new HNET.NewPacket(555); // 555 <-> 444
        Send(Out);
    }
}
