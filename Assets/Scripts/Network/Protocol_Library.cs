using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.Data
{
    /// <summary>
    /// 프로토콜
    /// </summary>
    public enum PROTOCOL : UInt64
    {
        DISCONNECT  = 0xFFFFFFFFFFFFFFFF,
        GLOBAL      = 0x0001000000000000,
        MNG_LOGIN   = 0x0002000000000000,
        MNG_INGAME  = 0x0004000000000000
    }

    public enum PROTOCOL_GLOBAL : UInt64
    {
        // 서브 프로토콜
        HEART_BEAT      = 0x0000000100000000,
        ENCRYPTION_KEY  = 0x0000000200000000,
    };

    public enum PROTOCOL_LOGIN : UInt64
    {
        // 서브 프로토콜
        LOGIN       = 0x0000000100000000, // 로그인
        LOGOUT      = 0x0000000200000000, // 로그아웃
        REGISTER    = 0x0000000400000000, // 회원가입
        MATCH       = 0x0000000800000000, // 매치

        // 디테일 프로토콜
        SUCCESS     = 0x0000000000010000, // 무언가에 대한 긍정적인 반응 
        FAILED      = 0x0000000000020000, // 부정적인 반응
        START       = 0x0000000000040000, // 시작
        STOP        = 0x0000000000080000, // 중지
        RESULT      = 0x0000000000100000  // 완료
    };

    public enum PROTOCOL_INGAME : UInt64
    {
        // 서브 프로토콜
        HEARTBEAT   = 0x0000000100000000, // 하트비트
        READY       = 0x0000000200000000, // 준비됐다
        START       = 0x0000000400000000, // 게임 시작
        INPUT       = 0x0000000800000000, // 입력
        SHOT        = 0x0000001000000000, // 사격

        // 디테일 프로토콜
        SHOT_FIRE   = 0x0000000000000001, // 사격 실행
        SHOT_HIT    = 0x0000000000000002, // 사격 피해

    };
}