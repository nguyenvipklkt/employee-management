namespace Common.Enum
{
    public static class CoreEnum
    {
        public static Dictionary<int, string> StatusAccountDic = new Dictionary<int, string>()
        {
            { 0, "Không hoạt động" },
            { 1, "Hoạt động" }
        };

        public const int TRUE = 1;
        public const int FALSE = 0;
    }
}
