using System;
using System.IO;
using UnityEngine;

public class D_Client : HNET.CHNetConnector // 클라이언트 클래스 정의(HNetwork 클래스 상속)
{
    public bool connect;           // 서버와의 연결 여부를 저장하는 변수
    public Vector3 PlayerPosition; // 플레이어 오브젝트 위치 저장 변수
    public bool jumping;           // 플레이어 오브젝트가 점프 중인지를 저장하는 변수
    public int count;              // y좌표 설정에 사용할, 화면 갱신 횟수 저장 변수
    
    // 위치 패킷 저장 변수
    public float received_x;
    public float received_y;
    public float received_z;

    // 방향 패킷 저장 변수
    public float delta_x;
    public float delta_y;
    public float delta_z;
    
    public void OnGUI() // 유니티 게임 화면의 버튼 기능 함수
    {
        // 접속 버튼을 누르면
        if (GUI.Button(new Rect(200, 100, 300, 100), "접속"))   
        {
            Connect();  // 서버와 클라이언트 연결
            connect = true; // 연결을 '참'으로 저장
        }
        if (GUI.Button(new Rect(200, 220, 300, 100), "끊기"))   // 끊기 버튼을 누르면
        {
            Disconnect();   // 서버와 클라이언트의 연결 해제
            connect = false;    // 연결을 '거짓'으로 저장
        }
    }

    public override void OnMessage(HNetPacket Packet) // 패킷 수신 함수(패킷이 전송되면 반응)
    {
        // 패킷 타입이 888(방향 패킷)이고 플레이어가 점프 중이 아닐 때
        if (Packet.Type() == 888 && jumping == false) 
        { 
            // direction_packet_count += 1;
            Packet.Out(ref delta_x); //
            Packet.Out(ref delta_z); // → 방향값 수신
        }
        // 패킷 타입이 666(위치 패킷)일 때
        else if (Packet.Type() == 666) 
        {
            Packet.Out(ref received_x); //
            Packet.Out(ref received_y); // → 위치값 수신
            Packet.Out(ref received_z); //
            gameObject.transform.position = new Vector3(received_x, received_y, received_z); // 스크립트가 연결된 오브젝트(플레이어 오브젝트)의 좌표를, 수신된 좌표로 변경
        }
        // 패킷 타입이 444(점프 패킷)일 때
        else if (Packet.Type() == 444) { 
            jumping = true; // 점프 중이라고 저장
            delta_y = 5f;   // y값 변화량 설정
        }
        // 설정해둔 패킷 타입과 맞지 않을 때
        else
        {
            return; // 값 반환 없이 종료
        }
    }

    public void FixedUpdate() // 주기적으로 반복되는 함수(기본 0.02초)
    {
        // 플레이어가 점프 중일 때
        if (jumping == true)
        {
            // 화면 갱신이 50번 미만으로 이루어졌다면
            if (count < 50) 
            {
                delta_y -= 0.2f; // y값 변화량 감소
            } 
            // 화면 갱신이 50번 이루어졌다면
            else
            {
                // 변수 초기화
                delta_y = 0;
                count = 0;
                delta_x = 0;
                delta_z = 0;
                jumping = false;
            }
            count += 1;        
        }

        // 매 프레임마다 정해진 방향대로 조금씩 이동
        gameObject.transform.Translate((new Vector3(delta_x, delta_y, delta_z)) * Time.deltaTime); 
    }
}
