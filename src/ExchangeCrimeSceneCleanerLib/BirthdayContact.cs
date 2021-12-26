// Roger Briggen license this file to you under the MIT license.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Roger.Briggen.ExchangeCrimeSceneCleanerLib;

public class BirthdayContact
{
    public BirthdayContact()
    {
        DisplayName = "";
        GivenName = "";
        Surname = "";
        Birthday = null;
        LastModifiedDateTime = null;
        ChangeKey = "";
        CreatedDateTime = null;
        Id = "";
    }

    public BirthdayContact(Microsoft.Graph.Contact contact)
    {
        DisplayName = contact.DisplayName;
        GivenName = contact.GivenName;
        Surname = contact.Surname;
        Birthday = contact.Birthday;
        LastModifiedDateTime = contact.LastModifiedDateTime;
        ChangeKey = contact.ChangeKey;
        CreatedDateTime = contact.CreatedDateTime;
        Id = contact.Id;
    }

    

    //
    // Zusammenfassung:
    //     Gets or sets display name. The contact's display name. You can specify the display
    //     name in a create or update operation. Note that later updates to other properties
    //     may cause an automatically generated value to overwrite the displayName value
    //     you have specified. To preserve a pre-existing value, always include it as displayName
    //     in an update operation.
    [JsonPropertyName("displayName")]
    public string DisplayName
    {
        get;
        set;
    }


    //
    // Zusammenfassung:
    //     Gets or sets given name. The contact's given name.
    [JsonPropertyName("givenName")]
    public string GivenName
    {
        get;
        set;
    }

    //
    // Zusammenfassung:
    //     Gets or sets surname.
    [JsonPropertyName("surname")]
    public string Surname
    {
        get;
        set;
    }

    //
    // Zusammenfassung:
    //     Gets or sets birthday. The contact's birthday. The Timestamp type represents
    //     date and time information using ISO 8601 format and is always in UTC time. For
    //     example, midnight UTC on Jan 1, 2014 is 2014-01-01T00:00:00Z
    [JsonPropertyName("birthday")]
    public DateTimeOffset? Birthday
    {
        get;
        set;
    }

    //
    // Zusammenfassung:
    //     Gets or sets change key. Identifies the version of the item. Every time the item
    //     is changed, changeKey changes as well. This allows Exchange to apply changes
    //     to the correct version of the object. Read-only.
    [JsonPropertyName("changeKey")]
    public string ChangeKey
    {
        get;
        set;
    }

    //
    // Zusammenfassung:
    //     Gets or sets created date time. The Timestamp type represents date and time information
    //     using ISO 8601 format and is always in UTC time. For example, midnight UTC on
    //     Jan 1, 2014 is 2014-01-01T00:00:00Z
    [JsonPropertyName("createdDateTime")]
    public DateTimeOffset? CreatedDateTime
    {
        get;
        set;
    }

    //
    // Zusammenfassung:
    //     Gets or sets last modified date time. The Timestamp type represents date and
    //     time information using ISO 8601 format and is always in UTC time. For example,
    //     midnight UTC on Jan 1, 2014 is 2014-01-01T00:00:00Z
    [JsonPropertyName("lastModifiedDateTime")]
    public DateTimeOffset? LastModifiedDateTime
    {
        get;
        set;
    }

    public string Id
    {
        get;
        set;
    }

    public static void writeBirthdayContactListToFile(string filename, List<BirthdayContact> contacts)
    {
        string jsonString = JsonSerializer.Serialize(contacts);
        System.IO.File.WriteAllText(filename, jsonString);
    }

    public static List<BirthdayContact> readBirthdayContactListFromFile(string filename)
    {
        string jsonString = System.IO.File.ReadAllText(filename);
        List<BirthdayContact>? contacts = JsonSerializer.Deserialize<List<BirthdayContact>>(jsonString);
        if (contacts == null)
        {
            contacts = new List<BirthdayContact>();
        }
        return contacts;
    }


}
