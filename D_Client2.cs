using System;
using UnityEngine;
public class D_Client2 : HNET.CHNetConnector // 클라이언트 정의
{
    public bool connect = false;
    public bool already_started = false;
    public CharacterController SelectPlayer;
    public Vector3 vecDir;
    public Vector3 givenPos;
    public Vector3 givenDir;
    public float a1 = 0.0f;
    public float a2 = 0.0f;
    public float a3 = 0.0f;
    public float b1 = 0.0f;
    public float b2 = 0.0f;

    public override void OnConnect()    // 서버와 연결했을 때 실행
    {
        UnityEngine.Debug.Log("▶ connect"); // 콘솔에 출력
    }

    public override void OnDisconnect() // 서버와의 연결을 해제했을 때 실행
    {
        UnityEngine.Debug.Log("▶ disconnect");  // 콘솔에 출력
    }
    public override void OnMessage(HNetPacket Packet)   // 패킷 수신 함수
    {
        if (666 == Packet.Type()) { // 패킷 타입이 666일 때
            Packet.Out(ref a1); //
            Packet.Out(ref a2); // → 서버의 패킷 수신
            Packet.Out(ref a3); //
            Packet.Out(ref b1); //
            Packet.Out(ref b2); //

            givenPos = new Vector3(a1, a2, a3);
            transform.position = givenPos;
            
        } else { // 패킷 타입이 666이 아닐 때
            return; // 값 반환 없이 종료
        }
    }

    void FixedUpdate() {
        float distance = b2;
        float X = distance / 100;

        if (X < distance) {
            transform.position += new Vector3(X, 0.0f, factual_func(b1, b2, X));        
            X += distance / 100;
        }
    }

    public float factual_func(float dir, float dis, float X) {
        return - (dir / dis) * X * (X - dis);
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