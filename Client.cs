using System;
using UnityEngine;

// 클라이언트 정의.
public class Client : HNET.CHNetConnector
{
    public bool connect = false;
    public bool already_started = false;
    public Vector3 vecDir;
    public float temp_angle;
    public float current_angle;
    public GameObject player;
    public float[] starts = new float[2]{0.0f, 0.0f};
    public float[] temp_starts = new float[2]{0.0f, 0.0f};
    public bool accessible = false;
    public override void OnConnect()
    {
        UnityEngine.Debug.Log("▶ connect");
    }

    public override void OnDisconnect()
    {
        UnityEngine.Debug.Log("▶ disconnect");
    }

    // 패킷 수신 함수
    public override void OnMessage(HNetPacket Packet)
    {
        // if (Packet.Type() == 666) {

        //     // 패킷에서 정보 얻기.
        //     float a1 = 0.0f;
        //     float a2 = 0.0f;
        //     float a3 = 0.0f;
        //     // float b1 = 0.0f;
        //     // float b2 = 0.0f;
        //     // float b3 = 0.0f;
        //     Packet.Out(ref a1); //
        //     Packet.Out(ref a2); // → 서버의 패킷 수신
        //     Packet.Out(ref a3); //
        //     // Packet.Out(ref b1); //
        //     // Packet.Out(ref b2); //
        //     // Packet.Out(ref b3); //

        //     // 정보 출력.
        //     // string s = string.Format("▶ ({0}, {1}, {2})", a1, a2, a3); // 유니티 콘솔에 패킷 출력
        //     // UnityEngine.Debug.Log(s);

        // } else if (Packet.Type() == 888) {

        //     // 패킷에서 정보 얻기.
        //     float a1 = 0.0f;
        //     float a2 = 0.0f;
        //     float a3 = 0.0f;
        //     Packet.Out(ref a1); //
        //     Packet.Out(ref a2); // → 서버의 패킷 수신
        //     Packet.Out(ref a3); //

        //     // 정보 출력.
        //     // string s = string.Format("▶ ({0}, {1}, {2}, {3}, {4}, {5})", a1, a2, a3, b1, b2, b3); // 유니티 콘솔에 패킷 출력
        //     // UnityEngine.Debug.Log(s);
        // } else {
        //     return;
        // }

        return;
    }

    // 유니티 게임 화면의 버튼 기능 함수
    public void OnGUI()
    {
        // 접속 버튼을 누르면
        if (GUI.Button(new Rect(200, 100, 300, 100), "접속"))
        {
            // 서버와 클라이언트 연결
            Connect();
            connect = true; // 연결을 '참'으로 저장
        }

        // 끊기 버튼을 누르면
        if (GUI.Button(new Rect(200, 220, 300, 100), "끊기"))
        {
            // 서버와 클라이언트 연결 끊기
            Disconnect();
            connect = false; // 연결을 '거짓'으로 저장
        }

        // 보내기 버튼을 누르면
        if (GUI.Button(new Rect(200, 340, 300, 100), "보내기"))
        {
            // 이미 보내는 중이 아니라면
            if (already_started == false) {
                accessible = true;
                InvokeRepeating("transmit_position", 0f, 0.2f);
                InvokeRepeating("transmit_direction", 0f, 0.2f);
                already_started = true; // 이미 보내는 중이라 저장
            } else { // 이미 보내는 중이라면
                CancelInvoke("transmit_position"); // 패킷 전송 중지
                already_started = false; // 이미 보내는 중이 아니라 저장
            }

            // InvokeRepeating("continuous_Update", 0f, 0.1f);
        }
    }

    public void FixedUpdate() { // 주기적으로 반복되는 함수. → 여기서는 데드레커닝 기법을 구현한 지점.
     
        starts[0] = Input.GetAxisRaw("Vertical");
        starts[1] = Input.GetAxisRaw("Horizontal");
    
        transmit_direction();

        current_angle = player.transform.eulerAngles.y;

        if (Math.Abs(temp_angle - current_angle) > 2) {
            // Debug.Log("Angle exceeded");

            temp_angle = current_angle;

            accessible = true; // 스타트 값에 변화를 주어, transmit_direction이 패킷을 보내도록 유도
            transmit_direction();
        }
    }

    // 패킷 송신 함수
    public void transmit_position()
    {
        // 777 타입의 패킷 송신
        HNET.NewPacket Out = new HNET.NewPacket(777);

        Vector3 pos; // 플레이어 오브젝트의 위치 저장 변수 (x, y, z)
        pos = this.gameObject.transform.position; // 변수에 위치 대입

        // 플레이어 오브젝트의 위치를 송신
        Out.In(pos.x);
        Out.In(pos.y);
        Out.In(pos.z);
        Send(Out);

        // 연결이 '거짓'이면
        if (connect == false) {
            CancelInvoke("transmit_position"); // 패킷 전송 중지
            already_started = false; // 이미 보내는 중이 아니라 저장
        }
    }

    // 패킷 송신 함수
    public void transmit_direction()
    {
        if (temp_starts[0] != starts[0] || temp_starts[1] != starts[1] || accessible == true) {
            // Debug.Log("difference detacted");

            // 999 타입의 패킷 송신
            HNET.NewPacket Out = new HNET.NewPacket(999);

            if (starts[0] != 0 || starts[1] != 0) {  
                vecDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
                vecDir = player.transform.TransformDirection(vecDir);
                vecDir *= 5.0f;
            } else {
                Debug.Log("stop");
                vecDir = new Vector3(0, 0, 0);
            }

            // 플레이어 오브젝트의 위치를 송신
            Out.In(vecDir.x);
            Out.In(vecDir.y);
            Out.In(vecDir.z);
            Send(Out);

            temp_starts[0] = starts[0];
            temp_starts[1] = starts[1];
            
            temp_angle = player.transform.eulerAngles.y;

            accessible = false;
        }
    }
}