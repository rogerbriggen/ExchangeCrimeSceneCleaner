// Roger Briggen license this file to you under the MIT license.
//

using Roger.Briggen.ExchangeCrimeSceneCleanerLib;
using Microsoft.Extensions.Configuration;

namespace Roger.Briggen.ExchangeCrimeSceneCleanerCli;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Hello from ExchangeCrimeSceneCleaner!");

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

        if (appId == null)
        {
            Console.WriteLine("appId is missing in appsettings.json...exiting");
            return;
        }


        // Initialize Graph client
        MSGraphHelper.Initialize(appId, scopes, (code, cancellation) => {
            Console.WriteLine(code.Message);
            return Task.FromResult(0);
        });

        //var accessToken = MSGraphHelper.GetAccessTokenAsync(scopes).Result;

        // Get signed in user
        var user = MSGraphContactsHelper.GetMeAsync().Result;
        Console.WriteLine($"Welcome {user!.DisplayName} {user!.Birthday}  {user.MailboxSettings?.TimeZone}!\n");

        //Get all contacts
        var contactList = MSGraphContactsHelper.GetAllContactsAsync().Result;

        if (contactList != null)
        {
            //Write out all contacts
            MSGraphContactsHelper.WriteContactListToFile("allContacts.json", contactList);
            //Filter only those with a birthday
            var birthdayContactList = MSGraphContactsHelper.FilterOnlyBirthday(contactList);
            MSGraphContactsHelper.WriteContactListToFile("filteredContacts.json", birthdayContactList);
            Console.WriteLine($"Birthday contact count: {birthdayContactList!.Count}\n");

            //Write a list with contacts with a brithday but only very limited fields
            var birthdayContactListShort = new List<BirthdayContact>();
            foreach(var contact in birthdayContactList)
            {
                birthdayContactListShort.Add(new BirthdayContact(contact));
            }
            BirthdayContactJSON.WriteBirthdayContactListToFile("filteredBirthdayContacts.json", birthdayContactListShort);
        }

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
