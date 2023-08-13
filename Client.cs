using System;
using UnityEngine;

// 클라이언트 정의.
public class Client : HNET.CHNetConnector
{
    public bool connect = false;
    public bool already_started = false;
    public bool accessible = false;
    public bool already_jumped = false;
    public Vector3 vecDir;
    public GameObject player;
    public float Ydir;
    public float temp_angle;
    public float current_angle;
    public float[] starts = new float[2]{0.0f, 0.0f};
    public float[] temp_starts = new float[2]{0.0f, 0.0f};

    public override void OnConnect()
    {
        UnityEngine.Debug.Log("▶ connect");
    }

    public override void OnDisconnect()
    {
        UnityEngine.Debug.Log("▶ disconnect");
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
            if (already_started == false)
            {
                accessible = true;
                InvokeRepeating("transmit_position", 0f, 1.0f);
                //InvokeRepeating("transmit_direction", 0f, 0.2f);
                already_started = true; // 이미 보내는 중이라 저장
            } else { // 이미 보내는 중이라면
                //CancelInvoke("transmit_position"); // 패킷 전송 중지
                already_started = false; // 이미 보내는 중이 아니라 저장
            }
        }
    }

    public void FixedUpdate() // 주기적으로 반복되는 함수. → 여기서는 데드레커닝 기법을 구현한 지점.
    { 
        if (Input.GetButton("Jump") && already_jumped == false)
        {
            transmit_jump();
        }
        
        if (starts[0] != Input.GetAxisRaw("Vertical") || starts[1] != Input.GetAxisRaw("Horizontal"))
        {
            transmit_direction();
            starts[0] = Input.GetAxisRaw("Vertical");
            starts[1] = Input.GetAxisRaw("Horizontal");
        }

        current_angle = player.transform.eulerAngles.y;

        if (Math.Abs(temp_angle - current_angle) > 4)
        {
            temp_angle = current_angle;
            transmit_direction();
        }
    }

    // 패킷 송신 함수
    public void transmit_position()
    {
        HNET.NewPacket Out = new HNET.NewPacket(777); // 777 타입의 패킷 송신

        // 플레이어 오브젝트의 위치를 송신
        Out.In(player.transform.position.x);
        Out.In(player.transform.position.y);
        Out.In(player.transform.position.z);
        Send(Out);

        // 연결이 '거짓'이면
        if (connect == false)
        {
            CancelInvoke("transmit_position"); // 패킷 전송 중지
            already_started = false; // 이미 보내는 중이 아니라 저장
        }
    }

    // 패킷 송신 함수
    public void transmit_direction()
    {
        // 999 타입의 패킷 송신
        HNET.NewPacket Out = new HNET.NewPacket(999);

        vecDir.x = Input.GetAxisRaw("Horizontal");
        vecDir.z = Input.GetAxisRaw("Vertical");
        vecDir = player.transform.TransformDirection(vecDir);
        vecDir *= 5.0f;

        // 플레이어 오브젝트의 방향을 송신
        Out.In(vecDir.x);
        Out.In(vecDir.z);
        Send(Out);
        
        temp_angle = player.transform.eulerAngles.y;

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
