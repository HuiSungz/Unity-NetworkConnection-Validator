
namespace NetworkConnector.Editor
{
    internal static class NetworkConfigContent
    {
        public static class HTTP
        {
            public const string TITLE = "HTTP Settings";
            public const string URL_LIST = "URL List";
            public const string URL_TOOLTIP = "HTTP 연결 테스트에 사용할 URL입니다.";
            public const string TIMEOUT = "Timeout";
            public const string TIMEOUT_TOOLTIP = "HTTP 요청의 타임아웃 시간(초)입니다.";
            public const string EMPTY_WARNING = "최소 하나의 HTTP URL을 추가해주세요.";
        }

        public static class Ping
        {
            public const string TITLE = "Ping Settings";
            public const string HOST_LIST = "Host List";
            public const string HOST_TOOLTIP = "Ping 테스트에 사용할 호스트 주소입니다.";
            public const string TIMEOUT = "Timeout";
            public const string TIMEOUT_TOOLTIP = "Ping 요청의 타임아웃 시간(밀리초)입니다.";
            public const string EMPTY_WARNING = "최소 하나의 Ping Host를 추가해주세요.";
        }

        public static class Retry
        {
            public const string TITLE = "Retry Settings";
            public const string BASE_INTERVAL = "Base Retry Interval";
            public const string BASE_INTERVAL_TOOLTIP = "초기 재시도 간격(밀리초)입니다.";
            public const string MAX_INTERVAL = "Max Retry Interval";
            public const string MAX_INTERVAL_TOOLTIP = "재시도 간격의 최대값(밀리초)입니다.";
            public const string MULTIPLIER = "Backoff Multiplier";
            public const string MULTIPLIER_TOOLTIP = "재시도 간격의 증가 배율입니다.";
            public const string INTERVAL_ERROR = "최대 재시도 간격은 기본 재시도 간격보다 커야 합니다.";
        }

        public static class Debug
        {
            public const string TITLE = "Debug Settings";
            public const string VERBOSE_LOG = "Verbose Log";
            public const string VERBOSE_LOG_TOOLTIP = "자세한 로그 출력 여부";
        }

        public static class Common
        {
            public const string HEADER = "Network Configuration";
            public const string RESET_BUTTON = "초기화";
            public const string SAVE_BUTTON = "설정 저장";
            public const string RESET_TITLE = "설정 초기화";
            public const string RESET_MESSAGE = "모든 설정을 기본값으로 초기화하시겠습니까?";
            public const string SAVE_TITLE = "저장 완료";
            public const string SAVE_MESSAGE = "네트워크 설정이 저장되었습니다.";
            public const string CONFIRM = "확인";
            public const string CANCEL = "취소";
        }
    }
}