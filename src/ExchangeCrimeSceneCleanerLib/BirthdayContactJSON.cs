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


public class BirthdayContactJSON
{
    public static void writeBirthdayContactListToFile(string filename, List<BirthdayContact> contacts)
    {
        string jsonString = JsonSerializer.Serialize(contacts, typeof(List<BirthdayContact>), BirthdayContactJSONContext.Default);
        System.IO.File.WriteAllText(filename, jsonString);
    }

    public static List<BirthdayContact> readBirthdayContactListFromFile(string filename)
    {
        string jsonString = System.IO.File.ReadAllText(filename);
        //List<BirthdayContact>? contacts = JsonSerializer.Deserialize<List<BirthdayContact>>(jsonString);
        List<BirthdayContact>? contacts = JsonSerializer.Deserialize(jsonString, typeof(List<BirthdayContact>), BirthdayContactJSONContext.Default) as List<BirthdayContact>;
        if (contacts == null)
        {
            contacts = new List<BirthdayContact>();
        }
        return contacts;
    }
}
