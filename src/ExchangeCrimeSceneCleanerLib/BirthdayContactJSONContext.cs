// Roger Briggen license this file to you under the MIT license.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Roger.Briggen.ExchangeCrimeSceneCleanerLib;


// Enable Source generation for JSON so we can trim it
[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(BirthdayContact))]
[JsonSerializable(typeof(List<BirthdayContact>))]
internal partial class BirthdayContactJSONContext : JsonSerializerContext
{
}
