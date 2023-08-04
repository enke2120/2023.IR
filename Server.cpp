/*

#define  _CRT_SECURE_NO_WARNINGS

// ���̺귯�� ��� ����.(������Ʈ ����(.vcxproj)�� ��ġ������ ��� ���)
#define HNETWORK_LIB_PATH "../../HNetwork/Lib"

// ��� ���� ����.
#include "../../HNetwork/HNetwork.h"
#include <ctime>

// ���� ����.
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

    // ��Ŷ ���� �Լ�
    void OnMessage(NetId netId, const HNetPacket& Packet) override
    {
        if (Packet.Type() == 777) {
            // ��Ŷ���� ���� ���.
            float   a1;
            float   a2;
            float   a3;
            Packet.Out(a1); //
            Packet.Out(a2); // �� Ŭ���̾�Ʈ�κ��� ��Ŷ ����
            Packet.Out(a3); //

            // Ŭ���̾�Ʈ�� ��Ŷ ���� ������.
            HNET::NewPacket Out(666);
            Out->In(a1); //
            Out->In(a2); // �� ��� Ŭ���̾�Ʈ�� ��Ŷ �۽�
            Out->In(a3); //
            SendAll(Out);
            sent_record += 1;

            // ���� �ð� ���� ����
            time_t timer;
            struct tm* t;
            timer = time(NULL);
            t = localtime(&timer);

            if (sc_sent != 0) {
                printf("\n\n");
                sc_sent = 0;
            }

            printf(" pos: (%f, %f, %f)", a1, a2, a3);

            // ��Ŷ ���� ������ �ð� ���
            printf(" (%d:%d:%d)\n", t->tm_hour, t->tm_min, t->tm_sec);            

}
        else if (Packet.Type() == 999) {
            // ��Ŷ���� ���� ���.
            float   a1;
            float   a2;
            float   a3;
            Packet.Out(a1); //
            Packet.Out(a2); // �� Ŭ���̾�Ʈ�κ��� ��Ŷ ����
            Packet.Out(a3); //

            // Ŭ���̾�Ʈ�� ��Ŷ ���� ������.
            HNET::NewPacket Out(888);
            Out->In(a1); //
            Out->In(a2); // �� ��� Ŭ���̾�Ʈ�� ��Ŷ �۽�
            Out->In(a3); //
            SendAll(Out);
            sent_record += 1;

            // ���� �ð� ���� ����
            time_t timer;
            struct tm* t;
            timer = time(NULL);
            t = localtime(&timer);

            if (sc_sent != 0) {
                printf("\n\n");
                sc_sent = 0;
            }

            // ���� �ֿܼ� ��Ŷ ���
            printf(" dir: (%f, %f, %f)", a1, a2, a3);

            // ��Ŷ ���� ������ �ð� ���
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

// ���̺귯�� ��� ����.(������Ʈ ����(.vcxproj)�� ��ġ������ ��� ���)
#define HNETWORK_LIB_PATH "../../HNetwork/Lib"

// ��� ���� ����.
#include "../../HNetwork/HNetwork.h"
#include <ctime>

// ���� ����.
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

      // ��Ŷ ���� �Լ�
      void OnMessage(NetId netId, const HNetPacket& Packet) override
      {
          if (Packet.Type() == 777) {
              // ��Ŷ���� ���� ���.
              float   a1;
              float   a2;
              float   a3;
              float   b1;
              float   b2;
              Packet.Out(a1); //
              Packet.Out(a2); // �� Ŭ���̾�Ʈ�κ��� ��Ŷ ����
              Packet.Out(a3); //
              Packet.Out(b1); //
              Packet.Out(b2); //

              // Ŭ���̾�Ʈ�� ��Ŷ ���� ������.
              HNET::NewPacket Out(666);
              Out->In(a1); //
              Out->In(a2); //
              Out->In(a3); // �� ��� Ŭ���̾�Ʈ�� ��Ŷ �۽�
              Out->In(b1); //
              Out->In(b2); //
              SendAll(Out);
              sent_record += 1;

              // ���� �ð� ���� ����
              time_t timer;
              struct tm* t;
              timer = time(NULL);
              t = localtime(&timer);

              if (sc_sent != 0) {
                  printf("\n\n");
                  sc_sent = 0;
              }

              printf(" pos: (%f, %f, %f)", a1, a2, a3);

              // ��Ŷ ���� ������ �ð� ���
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
