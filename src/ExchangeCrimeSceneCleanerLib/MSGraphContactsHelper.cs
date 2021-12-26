// Roger Briggen license this file to you under the MIT license.
//

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;
using Microsoft.Graph;
using TimeZoneConverter;

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
            var contacts = await MSGraphHelper.graphClient!.Me.Contacts.Request().GetAsync();
            Console.WriteLine($"Contact count {contacts.Count}!\n");

            var pageIterator = PageIterator<Contact>
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
            return await MSGraphHelper.graphClient!.Me.Request()
                .Select(u => new {
                    u.DisplayName,
                    u.MailboxSettings
                })
                .GetAsync();
                
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

    public static void writeContactListToFile(string filename, List<Contact> contacts)
    {
        string jsonString = JsonSerializer.Serialize(contacts);
        System.IO.File.WriteAllText(filename, jsonString);
    }

    public static List<Contact> readContactListFromFile(string filename)
    {
        string jsonString = System.IO.File.ReadAllText(filename);
        List<Contact>? contacts = JsonSerializer.Deserialize<List<Contact>>(jsonString);
        if (contacts == null)
        {
            contacts = new List<Contact>();
        }
        return contacts;
    }

}
