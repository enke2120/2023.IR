using System;
using UnityEngine;

// 클라이언트 정의.
public class Client2 : HNET.CHNetConnector
{
    public bool connect = false;
    public bool already_started = false;
    public Vector3 vecDir;
    Vector3 pos;
    public float axis_tan;
    public float tri_tan;
    public float dir_tan;
    public GameObject player;
    public float distance;
    public int trial = 0;

    public override void OnConnect()
    {
        float[] temp = new float[2]{423.0f, 253.0f};
        UnityEngine.Debug.Log("▶ connect");
        Debug.Log(temp[0]);
    }

    public override void OnDisconnect()
    {
        UnityEngine.Debug.Log("▶ disconnect");
    }

    // 패킷 수신 함수
    public override void OnMessage(HNetPacket Packet)
    {
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

    //     // 끊기 버튼을 누르면
    //     if (GUI.Button(new Rect(200, 220, 300, 100), "끊기"))
    //     {
    //         // 서버와 클라이언트 연결 끊기
    //         Disconnect();
    //         connect = false; // 연결을 '거짓'으로 저장
    //     }

    //     // 보내기 버튼을 누르면
    //     if (GUI.Button(new Rect(200, 340, 300, 100), "보내기"))
    //     {
    //         // 이미 보내는 중이 아니라면
    //         if (already_started == false) {
    //             InvokeRepeating("transmit", 0f, 0.5f);
    //             already_started = true; // 이미 보내는 중이라 저장
    //         } else { // 이미 보내는 중이라면
    //             CancelInvoke("transmit"); // 패킷 전송 중지
    //             already_started = false; // 이미 보내는 중이 아니라 저장
    //         }
    //     }
    // }

    // public float[] func_calc(float A1, float A2, float B1, float B2) {
    //     // if (tri_tan == NaN) {
    //     //     dir_tan = (float)(Math.Tan(player.transform.eulerAngles.y - 90));
    //     // } else {
    //         distance = (float)(Math.Sqrt(Math.Pow(A1-B1, 2) + Math.Pow(A2-B2, 2)));

    //         tri_tan = (A2-B2)/(A1-B1);

    //         axis_tan = (float)(Math.Tan(player.transform.eulerAngles.y));

    //         dir_tan = (axis_tan - tri_tan) / (1 + axis_tan * tri_tan);

    //         float[] values = new float[2]{dir_tan, distance};

    //         return values;
    //     // }
    // }

    // // public float coverNaN(float tan) {
    // //     if (tan == NaN) {
            
    // //     }
    // // }

    // // 패킷 송신 함수
    // public void transmit()
    // {
    //     float[] outputs = new float[2]{0, 0};

    //     if (trial >= 2) {
    //         // 777 타입의 패킷 송신
    //         HNET.NewPacket Out = new HNET.NewPacket(777);

    //         pos = this.gameObject.transform.position; // 변수에 위치 대입

    //         // 플레이어 오브젝트의 위치를 송신
    //         Out.In(pos.x);
    //         Out.In(pos.y);
    //         Out.In(pos.z);
    //         Out.In(outputs[0]);
    //         Out.In(outputs[1]);
    //         Send(Out);

    //         // 연결이 '거짓'이면
    //         if (connect == false) {
    //             CancelInvoke("transmit"); // 패킷 전송 중지
    //             already_started = false; } // 이미 보내는 중이 아니라 저장

    //     } else { trial += 1; }

    //     if (temp[0] == -100 && temp[1] == -100) {
    //         Debug.Log("ready");

    //         pos = this.gameObject.transform.position; // 변수에 위치 대입
    //         temp[0] = pos.x;
    //         temp[1] = pos.z;
    //     } else {
    //         Debug.Log("start");

    //         pos = this.gameObject.transform.position; // 변수에 위치 대입

    //         Debug.Log(temp[0]);
    //         Debug.Log(temp[1]);
    //         Debug.Log(pos.x);
    //         Debug.Log(pos.z);

    //         outputs = func_calc(temp[0], temp[1], pos.x, pos.z);
    //         temp[0] = pos.x;
    //         temp[1] = pos.z;
    //     }
    
    }
}