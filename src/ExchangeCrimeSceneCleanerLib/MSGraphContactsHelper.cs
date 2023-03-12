// Roger Briggen license this file to you under the MIT license.
//

using System.Text.Json;
using Microsoft.Graph;
using Microsoft.Graph.Models;

namespace Roger.Briggen.ExchangeCrimeSceneCleanerLib;

public class MSGraphContactsHelper
{
    public const string ContactURL = "contacts?";

    public static async Task<List<Contact>?> GetAllContactsAsync()
    {
        try
        {
            List<Contact> allContacts = new List<Contact>();
            // GET /contacts
            var contacts = await MSGraphHelper.graphClient!.Me.Contacts.GetAsync();
            if (contacts != null)
            {
                Console.WriteLine($"Contact count {contacts.OdataCount}!\n");

                var pageIterator = PageIterator<Contact, ContactCollectionResponse>
                    .CreatePageIterator(
                        MSGraphHelper.graphClient!,
                        contacts,
                        // Callback executed for each item in the collection
                        (contact) =>
                        {
                            Console.WriteLine($"Contact {contact.DisplayName}  {contact.Birthday}!\n");
                            allContacts.Add(contact);
                            return true;
                        }
                    );

                await pageIterator.IterateAsync();
            }
            
            return allContacts;
        }
        catch (ServiceException ex)
        {
            Console.WriteLine($"Error getting signed-in user: {ex.Message}");
            return null;
        }
    }


    public static async Task<User?> GetMeAsync()
    {
        try
        {
            // GET /me
            return await MSGraphHelper.graphClient!.Me
                .GetAsync(requestConfiguration =>
                {
                    requestConfiguration.QueryParameters.Select = new string[] {"displayName", "MailboxSettings" };
                });
        }
        catch (ServiceException ex)
        {
            Console.WriteLine($"Error getting signed-in user: {ex.Message}");
            return null;
        }
    }

    public static List<Contact> FilterOnlyBirthday(List<Contact> allContacts)
    {
        return allContacts.Where(u => u.Birthday != null).ToList();
    }

    public static void WriteContactListToFile(string filename, List<Contact> contacts)
    {
        var jsonString = JsonSerializer.Serialize(contacts);
        File.WriteAllText(filename, jsonString);
    }

    public static List<Contact> ReadContactListFromFile(string filename)
    {
        var jsonString = File.ReadAllText(filename);
        List<Contact>? contacts = JsonSerializer.Deserialize<List<Contact>>(jsonString);
        contacts ??= new List<Contact>();
        return contacts;
    }

}
