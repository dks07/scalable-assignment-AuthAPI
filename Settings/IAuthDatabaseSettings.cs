﻿namespace AuthAPI.Settings;

public interface IAuthDatabaseSettings
{
  string UsersCollectionName { get; set; }
  string ConnectionString { get; set; }
  string DatabaseName { get; set; }
}