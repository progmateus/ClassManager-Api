namespace ClassManager.Domain
{
  public static class Configuration
  {
    public static string PrivateKey { get; set; } = "698dc19d489c4e4db73e28a713eab07b";
    public static string BaseUrl { get; set; } = "http://192.168.15.8:5062";
    public static SecretsConfiguration Secrets { get; set; } = new();
    public static DatabaseConfiguration Database { get; set; } = new();
    public static SendGridConfiguration SendGrid { get; set; } = new();
    public static EmailConfiguration Email { get; set; } = new();
    public static RabbitMqConfiguration RabbitMq { get; set; } = new();
    public static StripeConfiguration Stripe { get; set; } = new();


    public class SecretsConfiguration
    {
      public string ApiKey { get; set; } = string.Empty;
      public string JwtPrivateKey { get; set; } = string.Empty;
      public string PasswordSaltKey { get; set; } = string.Empty;
    }

    public class DatabaseConfiguration
    {
      public string ConnectionString { get; set; } = string.Empty;
    }

    public class EmailConfiguration
    {
      public string DefaultFromEmail { get; set; } = "test@balta.io";
      public string DefaultFromName { get; set; } = "balta.io";
    }

    public class SendGridConfiguration
    {
      public string ApiKey { get; set; } = string.Empty;
    }

    public class RabbitMqConfiguration
    {
      public string Uri { get; set; } = "amqp://localhost:5672";
      public string Username { get; set; } = "guest";
      public string Password { get; set; } = "guest";
    }


    public class StripeConfiguration
    {
      public string ApiKey { get; set; } = string.Empty;
      public string PublishableKey { get; set; } = string.Empty;
      public string SecretKey { get; set; } = string.Empty;
    }
  }
}

