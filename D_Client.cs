using System;
using System.IO;
using UnityEngine;

public class D_Client : HNET.CHNetConnector // 클라이언트 정의
{
    public Vector3 vecDir;
    public Vector3 pos;
    public bool jumping;
    public bool connect = false;
    public bool already_started = false;
    public float a1 = 0.0f;
    public float a2 = 0.0f;
    public float a3 = 0.0f;
    public float b1 = 0.0f;
    public float b2 = 0.0f;
    public int count;
    public float constant;
    public int packet_count;

    string fullpth = "Assets/Others/test1";
    StreamWriter sw;

    public void Start() {
        count = 0;
        constant = 0;

        if (false == File.Exists(fullpth)) {
            sw = new StreamWriter(fullpth + ".txt");
        }
    }
    
    public override void OnConnect()    // 서버와 연결했을 때 실행
    {
        UnityEngine.Debug.Log("▶ connect"); // 콘솔에 출력
    }
    public override void OnDisconnect() // 서버와의 연결을 해제했을 때 실행
    {
        sw.WriteLine(packet_count);
        sw.Flush();
        sw.Close();
        UnityEngine.Debug.Log("▶ disconnect");  // 콘솔에 출력
    }
    public override void OnMessage(HNetPacket Packet)   // 패킷 수신 함수
    {
        if (Packet.Type() == 888 && jumping == false) // 패킷 타입이 888일 때
        { 
            packet_count += 1;
            Packet.Out(ref b1); //
            Packet.Out(ref b2); // → 서버의 패킷 수신
        }
        else if (Packet.Type() == 666) // 패킷 타입이 666일 때
        {
            Packet.Out(ref a1); //
            Packet.Out(ref a2); // → 서버의 패킷 수신
            Packet.Out(ref a3); //
            pos = gameObject.transform.position;
            sw.WriteLine("pred: " + pos.x + ", " + pos.y + ", " + pos.z);
            gameObject.transform.position = new Vector3(a1, a2, a3);
            sw.WriteLine("real: " + a1 + ", " + a2 + ", " + a3);
        }
        else if (Packet.Type() == 444) {
            if (jumping == false) {
                jumping = true;
                constant = 5f;
            }            
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
                constant -= 0.2f;
            } 
            else
            {
                constant = 0;
                count = 0;
                b1 = 0;
                b2 = 0;
                jumping = false;
            }
            count += 1;        
        }
        gameObject.transform.Translate((new Vector3(b1, constant, b2)) * Time.deltaTime); // 오브젝트의 이동 경로를 방향 벡터로 지정
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
