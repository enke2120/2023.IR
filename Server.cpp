/*

#define  _CRT_SECURE_NO_WARNINGS

// 라이브러리 경로 세팅.(프로젝트 파일(.vcxproj)의 위치에서의 상대 경로)
#define HNETWORK_LIB_PATH "../../HNetwork/Lib"

// 헤더 파일 포함.
#include "../../HNetwork/HNetwork.h"
#include <ctime>

// 서버 정의.
class CServer : public HNET::Acceptor
{
    public: int sent_record = 0;
    public: int sc_sent = 0;

    void OnConnect(NetId netId) override
    {
        HAPI::PrintG(1, sent_record, L"- connect");
        sent_record += 1;
        sc_sent += 1;
    }

    void OnDisconnect(NetId netId, const wchar_t* pReason) override
    {
        HAPI::PrintR(1, sent_record, L"- disconnect");
        sent_record += 1;
        sc_sent += 1;
    }

    // 패킷 수신 함수
    void OnMessage(NetId netId, const HNetPacket& Packet) override
    {
        if (Packet.Type() == 777) {
            // 패킷에서 정보 얻기.
            float   a1;
            float   a2;
            float   a3;
            Packet.Out(a1); //
            Packet.Out(a2); // → 클라이언트로부터 패킷 수신
            Packet.Out(a3); //

            // 클라이언트로 패킷 정보 보내기.
            HNET::NewPacket Out(666);
            Out->In(a1); //
            Out->In(a2); // → 모든 클라이언트로 패킷 송신
            Out->In(a3); //
            SendAll(Out);
            sent_record += 1;

            // 현재 시간 정보 저장
            time_t timer;
            struct tm* t;
            timer = time(NULL);
            t = localtime(&timer);

            if (sc_sent != 0) {
                printf("\n\n");
                sc_sent = 0;
            }

            printf(" pos: (%f, %f, %f)", a1, a2, a3);

            // 패킷 전송 시점의 시간 출력
            printf(" (%d:%d:%d)\n", t->tm_hour, t->tm_min, t->tm_sec);            

}
        else if (Packet.Type() == 999) {
            // 패킷에서 정보 얻기.
            float   a1;
            float   a2;
            float   a3;
            Packet.Out(a1); //
            Packet.Out(a2); // → 클라이언트로부터 패킷 수신
            Packet.Out(a3); //

            // 클라이언트로 패킷 정보 보내기.
            HNET::NewPacket Out(888);
            Out->In(a1); //
            Out->In(a2); // → 모든 클라이언트로 패킷 송신
            Out->In(a3); //
            SendAll(Out);
            sent_record += 1;

            // 현재 시간 정보 저장
            time_t timer;
            struct tm* t;
            timer = time(NULL);
            t = localtime(&timer);

            if (sc_sent != 0) {
                printf("\n\n");
                sc_sent = 0;
            }

            // 서버 콘솔에 패킷 출력
            printf(" dir: (%f, %f, %f)", a1, a2, a3);

            // 패킷 전송 시점의 시간 출력
            printf(" (%d:%d:%d)\n", t->tm_hour, t->tm_min, t->tm_sec);

        }
        else {
            return;
        }
    }
};

void main()
{
    CServer Server;
    if (true == Server.Listen()) {
        HAPI::PrintG(1, 0, L"* SERVER ON *");
        Server.sent_record += 1;
    }

    _getch();
}

*/


#define  _CRT_SECURE_NO_WARNINGS

// 라이브러리 경로 세팅.(프로젝트 파일(.vcxproj)의 위치에서의 상대 경로)
#define HNETWORK_LIB_PATH "../../HNetwork/Lib"

// 헤더 파일 포함.
#include "../../HNetwork/HNetwork.h"
#include <ctime>

// 서버 정의.
class CServer : public HNET::Acceptor
{
public: int sent_record = 0;
public: int sc_sent = 0;

      void OnConnect(NetId netId) override
      {
          HAPI::PrintG(1, sent_record, L"- connect");
          sent_record += 1;
          sc_sent += 1;
      }

      void OnDisconnect(NetId netId, const wchar_t* pReason) override
      {
          HAPI::PrintR(1, sent_record, L"- disconnect");
          sent_record += 1;
          sc_sent += 1;
      }

      // 패킷 수신 함수
      void OnMessage(NetId netId, const HNetPacket& Packet) override
      {
          if (Packet.Type() == 777) {
              // 패킷에서 정보 얻기.
              float   a1;
              float   a2;
              float   a3;
              float   b1;
              float   b2;
              Packet.Out(a1); //
              Packet.Out(a2); // → 클라이언트로부터 패킷 수신
              Packet.Out(a3); //
              Packet.Out(b1); //
              Packet.Out(b2); //

              // 클라이언트로 패킷 정보 보내기.
              HNET::NewPacket Out(666);
              Out->In(a1); //
              Out->In(a2); //
              Out->In(a3); // → 모든 클라이언트로 패킷 송신
              Out->In(b1); //
              Out->In(b2); //
              SendAll(Out);
              sent_record += 1;

              // 현재 시간 정보 저장
              time_t timer;
              struct tm* t;
              timer = time(NULL);
              t = localtime(&timer);

              if (sc_sent != 0) {
                  printf("\n\n");
                  sc_sent = 0;
              }

              printf(" pos: (%f, %f, %f)", a1, a2, a3);

              // 패킷 전송 시점의 시간 출력
              printf(" (%d:%d:%d)\n", t->tm_hour, t->tm_min, t->tm_sec);

          } else {
              return;
          }
      }
};

void main()
{
    CServer Server;
    if (true == Server.Listen()) {
        HAPI::PrintG(1, 0, L"* SERVER ON *");
        Server.sent_record += 1;
    }

    _getch();
}
