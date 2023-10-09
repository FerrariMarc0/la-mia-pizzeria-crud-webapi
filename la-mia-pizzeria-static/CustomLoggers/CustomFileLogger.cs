namespace la_mia_pizzeria_static.CustomLoggers
{
    public class CustomFileLogger : ICustomLogger
    {
        public void WriteLog(string message)
        {
            File.AppendAllText("C:\\Users\\MarcoF\\source\\repos\\la-mia-pizzeria-static\\la-mia-pizzeria-static\\my-log-txt", $"LOG: {message} - {DateTime.Now.ToString("dd-MM-yyy HH:mm:ss")} -\n");
        }
    }
}
