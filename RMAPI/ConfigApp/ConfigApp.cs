namespace RMAPI.ConfigApp
{
    public class ConfigApp
    {
        public static string DBConnection { get; set; } = "";

        public static void GetConfigSetting(WebApplicationBuilder builder)
        {
            try
            {
                DBConnection = builder.Configuration.GetConnectionString("DefaultConnection") ?? "";
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
