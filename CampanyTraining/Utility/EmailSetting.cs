namespace CompanyTraining.Utility
{
    public class EmailSetting
    {
        public string SmtpServer { get; set; }
        public string SenderEmail { get; set; }

        public string Password { get; set; }
        public int Port { get; set; }
        public string SenderName { get; set; }
    }
}
