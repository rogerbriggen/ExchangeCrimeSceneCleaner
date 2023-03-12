// Roger Briggen license this file to you under the MIT license.
//

using System.Text.Json;

namespace Roger.Briggen.ExchangeCrimeSceneCleanerLib;


public class BirthdayContactJSON
{
    public static void WriteBirthdayContactListToFile(string filename, List<BirthdayContact> contacts)
    {
        var jsonString = JsonSerializer.Serialize(contacts, typeof(List<BirthdayContact>), BirthdayContactJSONContext.Default);
        File.WriteAllText(filename, jsonString);
    }

    public static List<BirthdayContact> ReadBirthdayContactListFromFile(string filename)
    {
        var jsonString = File.ReadAllText(filename);
        //List<BirthdayContact>? contacts = JsonSerializer.Deserialize<List<BirthdayContact>>(jsonString);
        var contacts = JsonSerializer.Deserialize(jsonString, typeof(List<BirthdayContact>), BirthdayContactJSONContext.Default) as List<BirthdayContact>;
        contacts ??= new List<BirthdayContact>();
        return contacts;
    }
}
