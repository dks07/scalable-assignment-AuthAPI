namespace AuthAPI.Settings
{
  public class AuthDatabaseSettings : IAuthDatabaseSettings
  {
    public string UsersCollectionName { get; set; }
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
  }
}
