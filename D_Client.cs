using System;
using UnityEngine;
public class D_Client : HNET.CHNetConnector // 클라이언트 정의
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
    public float b3 = 0.0f;

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

            givenPos = new Vector3(a1, a2, a3);
            transform.position = givenPos;

            string s = string.Format("▶ ({0}, {1}, {2})", a1, a2, a3); // 유니티 콘솔에 패킷 출력
            // UnityEngine.Debug.Log(s);

        } else if (888 == Packet.Type()) {  // 패킷 타입이 888일 때
            
            CancelInvoke("move");   // move 함수 중지

            Packet.Out(ref b1); //
            Packet.Out(ref b2); // → 서버의 패킷 수신
            Packet.Out(ref b3); //

            transform.position = givenPos;

            InvokeRepeating("move", 0.0f, 0.01f); // move 함수 반복

            string s = string.Format("▶ ({0}, {1}, {2})", b1, b2, b3); // 유니티 콘솔에 패킷 출력
            // UnityEngine.Debug.Log(s);

        } else { // 패킷 타입이 666도 888도 아닐 때
            return; // 값 반환 없이 종료
        }
    }

    void move() {   // move 함수(종속 클라이언트의 움직임 처리)
        // bool air = false;   // 공중에 떠있는지를 저장(변화하는 시점에서만 코드를 처리하기 위한 도구)

        // if (!SelectPlayer.isGrounded) { // 오브젝트가 바닥에 있지 않으면
        //     a2 -= 10.0f * Time.deltaTime;   // 방향 벡터의 y성분에서 중력가속도에 시간 변화량을 곱한 만큼을 뺀다
        //     air = true;     // 공중에 떠있다고 저장
        // } else if (air == true && SelectPlayer.isGrounded) {    // 공중에 떠있다고 저장되어 있었고 오브젝트가 바닥에 있으면
        //     a2 += 10.0f * Time.deltaTime;   // 방향 벡터의 y성분에서 중력가속도에 시간 변화량을 곱한 만큼을 더한다
        //     air = false;    // 공중에 떠있지 않다고 저장
        // }

        givenDir = new Vector3(b1, b2, b3); // 전송받은 방향 벡터 저장
        SelectPlayer.Move(givenDir * Time.deltaTime); // 오브젝트의 이동 경로를 방향 벡터로 지정
        // transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -2.41f, 100.0f), transform.position.z);    // 단, 오브젝트의 y좌표는 -2.41보다 작을 수 없음.
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