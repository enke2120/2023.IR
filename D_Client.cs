using System;
using System.IO;
using UnityEngine;

public class D_Client : HNET.CHNetConnector // 클라이언트 클래스 정의(HNetwork 클래스 상속)
{
    public bool connect;
    public int direction_packet_count;
    
    public Vector3 PlayerPosition;
    public bool jumping;

    public float received_x;
    public float received_y;
    public float received_z;

    public float delta_x;
    public float delta_y;
    public float delta_z;
    public int count;

    string fullpth = "Assets/Others/test1";
    StreamWriter sw;

    public void Start() {
        if (false == File.Exists(fullpth)) {
            sw = new StreamWriter(fullpth + ".txt");
        }
    }
    
    public override void OnDisconnect() // 서버와의 연결을 해제했을 때 실행
    {
        sw.WriteLine(direction_packet_count);
        sw.Flush();
        sw.Close();
    }
    public override void OnMessage(HNetPacket Packet)   // 패킷 수신 함수
    {
        if (Packet.Type() == 888 && jumping == false) // 패킷 타입이 888일 때
        { 
            direction_packet_count += 1;
            Packet.Out(ref delta_x); //
            Packet.Out(ref delta_z); // → 서버의 패킷 수신
        }
        else if (Packet.Type() == 666) // 패킷 타입이 666일 때
        {
            Packet.Out(ref received_x); //
            Packet.Out(ref received_y); // → 서버의 패킷 수신
            Packet.Out(ref received_z); //
            PlayerPosition = gameObject.transform.position;
            sw.WriteLine("pred: " + PlayerPosition.x + ", " + PlayerPosition.y + ", " + PlayerPosition.z);
            gameObject.transform.position = new Vector3(received_x, received_y, received_z);
            sw.WriteLine("real: " + received_x + ", " + received_y + ", " + received_z);
        }
        else if (Packet.Type() == 444) {
            jumping = true;
            delta_y = 5f;         
        }
        else// 패킷 타입이 666도 888도 아닐 때
        {
            return; // 값 반환 없이 종료
        }
    }

    public void FixedUpdate()
    {
        if (jumping == true) {
            if (count < 50)
            {
                delta_y -= 0.2f;
            } 
            else
            {
                delta_y = 0;
                count = 0;
                delta_x = 0;
                delta_z = 0;
                jumping = false;
            }
            count += 1;        
        }
        gameObject.transform.Translate((new Vector3(delta_x, delta_y, delta_z)) * Time.deltaTime); // 오브젝트의 이동 경로를 방향 벡터로 지정
    }

    public void OnGUI() // 유니티 게임 화면의 버튼 기능 함수
    {
        if (GUI.Button(new Rect(200, 100, 300, 100), "접속"))   // 접속 버튼을 누르면
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
}
