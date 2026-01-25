namespace SurvayBacket.Api.Helper
{
    public class EmailBodyBuilder
    {
        public static string GenerateEmailBody(string templete , Dictionary<string , string> templeteModel)
        {
            var templetePath = $"{Directory.GetCurrentDirectory()}/Templates/{templete}.html";
            var streamReader = new StreamReader(templetePath);
            var emailBody = streamReader.ReadToEnd();
            streamReader.Close();

            foreach (var item in templeteModel)
            {
                emailBody = emailBody.Replace(item.Key, item.Value);
            }
            return emailBody;
        }
    }
}
