using System;

namespace Network.Data
{
    /// <summary>
    /// 프로토콜
    /// </summary>
    public enum PROTOCOL : UInt64
    {
        DISCONNECT = 0xFFFFFFFFFFFFFFFF,
        GLOBAL = 0x0001000000000000,
        MNG_LOGIN = 0x0002000000000000,
        MNG_INGAME = 0x0004000000000000
    }

    public enum PROTOCOL_GLOBAL : UInt64
    {
        // 서브 프로토콜
        HEART_BEAT = 0x0000000100000000,
        ENCRYPTION_KEY = 0x0000000200000000,
    };

    public enum PROTOCOL_LOGIN : UInt64
    {
        // 서브 프로토콜
        LOGIN = 0x0000000100000000, // 로그인
        LOGOUT = 0x0000000200000000, // 로그아웃
        REGISTER = 0x0000000400000000, // 회원가입
        MATCH = 0x0000000800000000, // 매치

        // 디테일 프로토콜
        SUCCESS = 0x0000000000010000, // 무언가에 대한 긍정적인 반응 
        FAILED = 0x0000000000020000, // 부정적인 반응
        START = 0x0000000000040000, // 시작
        STOP = 0x0000000000080000, // 중지
        RESULT = 0x0000000000100000  // 완료
    };

    public enum PROTOCOL_INGAME : UInt64
    {
        // 서브 프로토콜
        HEARTBEAT   = 0x0000000100000000, // 하트비트
        READY       = 0x0000000200000000, // 준비됐다
        START       = 0x0000000400000000, // 게임 시작
        INPUT       = 0x0000000800000000, // 입력
        SHOT        = 0x0000001000000000, // 사격
        SESSION     = 0x0000002000000000, // 세션 상태 변경
        END         = 0x0000004000000000, // 게임 끝
        ITEM        = 0x0000008000000000, // 아이템 관련
        SKILL       = 0x0000010000000000, // 스킬 관련

        // 디테일 프로토콜
        SHOT_FIRE   = 0x0000000000000001, // 사격 실행
        SHOT_HIT    = 0x0000000000000002, // 사격 피해
        SHOT_STUN   = 0x0000000000000004, // 스턴 피해
        SS_ROUND_READY  = 0x0000000000000001, // 세션 - 라운드 변경
        SS_ROUND_START  = 0x0000000000000002, // 세션 - 라운드 시작
        SS_ROUND_END    = 0x0000000000000004, // 세션 - 라운드 종료
        SS_ROUND_SPAWN_ITEMS = 0x0000000000000008, // 세션 - 아이템 일괄 스폰
        ITEM_GET        = 0x0000000000000001, // 플레이어의 아이템 획득 사인
        SKILL_QUSTION   = 0x0000000000000001, // 스킬 사용 질의
        SKILL_USE       = 0x0000000000000002, // 스킬 사용
        SKILL_STOP      = 0x0000000000000004, // 스킬 중지

    };
}