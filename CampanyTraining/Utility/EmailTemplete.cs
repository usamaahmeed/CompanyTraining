namespace CompanyTraining.Utility
{
    public static class EmailTemplete
    {
         public static string WelcomeEmailBody(string userName, HostString host, string scheme)
        {
            return $@"
    <div style='font-family: Arial, sans-serif; padding: 20px; color: #333;'>
        <h2 style='color: #4CAF50;'>👋 Welcome Aboard, {userName}!</h2>
        <p>We're excited to have you join <strong>The Company Training</strong>.</p>

        <p>Get ready to explore our powerful training platform where your company can host and manage courses to upskill employees efficiently.</p>

        <a href='{scheme}://{host}/Customer/Home/Index' 
           style='display:inline-block; padding:10px 20px; background-color:#4CAF50; color:white; text-decoration:none; border-radius:5px; margin-top:20px;'>
            📚 Visit Training Platform
        </a>

        <p style='margin-top:30px;'>If you ever need help, just reply to this email. We're here for you. ❤️</p>
        <p>– The Training Platform Team</p>
    </div>";
        }
    }
}
