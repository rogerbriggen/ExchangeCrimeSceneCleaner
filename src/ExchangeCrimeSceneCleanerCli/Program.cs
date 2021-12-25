// Roger Briggen license this file to you under the MIT license.
//

using System;
using System.Collections.Generic;
using Roger.Briggen.ExchangeCrimeSceneCleanerLib;
using Microsoft.Extensions.Configuration;

namespace Roger.Briggen.ExchangeCrimeSceneCleanerCli;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        var appConfig = LoadAppSettings();

        if (appConfig == null)
        {
            Console.WriteLine("Missing or invalid appsettings.json...exiting");
            return;
        }

        var appId = appConfig["appId"];
        var scopesString = appConfig["scopes"];
        scopesString += ";Contacts.Read";
        var scopes = scopesString.Split(';');
        

        // Initialize Graph client
        MSGraphHelper.Initialize(appId, scopes, (code, cancellation) => {
            Console.WriteLine(code.Message);
            return Task.FromResult(0);
        });

        //var accessToken = MSGraphHelper.GetAccessTokenAsync(scopes).Result;
        
        // Get signed in user
        var user = MSGraphContactsHelper.GetMeAsync().Result;
        Console.WriteLine($"Welcome {user!.DisplayName} {user!.Birthday}  {user.MailboxSettings.TimeZone}!\n");

        //Get all contacts
        var contactList = MSGraphContactsHelper.GetAllContactsAsync().Result;
        Console.WriteLine($"Contact count: {contactList!.Count}\n"); 
    }

    static IConfigurationRoot? LoadAppSettings()
    {
        var appConfig = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .Build();

        // Check for required settings
        if (string.IsNullOrEmpty(appConfig["appId"]) ||
            string.IsNullOrEmpty(appConfig["scopes"]))
        {
            return null;
        }

        return appConfig;
    }
}
