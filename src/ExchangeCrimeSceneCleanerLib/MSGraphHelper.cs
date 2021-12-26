// Roger Briggen license this file to you under the MIT license.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;
using Microsoft.Graph;

namespace Roger.Briggen.ExchangeCrimeSceneCleanerLib;

public class MSGraphHelper
{
    //public const string BaseURL = "https://graph.microsoft.com/v1.0/me/";
    public const string CacheFilename = "authrecord.json";


    private static InteractiveBrowserCredential? tokenCredential;
    //private static VisualStudioCredential? tokenCredential;

    public static GraphServiceClient? graphClient;

    public static void Initialize(string clientId,
                                  string[] scopes,
                                  Func<DeviceCodeInfo, CancellationToken, Task> callBack)
    {
        //tokenCredential = new DeviceCodeCredential(callBack, "common", clientId);
        // using Azure.Identity;
        var options = new InteractiveBrowserCredentialOptions
        {
            TenantId = "common",
            ClientId = clientId,
            AuthorityHost = AzureAuthorityHosts.AzurePublicCloud,
            // MUST be http://localhost or http://localhost:PORT
            // See https://github.com/AzureAD/microsoft-authentication-library-for-dotnet/wiki/System-Browser-on-.Net-Core
            RedirectUri = new Uri("http://localhost"),
        };

        // https://docs.microsoft.com/dotnet/api/azure.identity.interactivebrowsercredential
        tokenCredential = new InteractiveBrowserCredential(options);
        graphClient = new GraphServiceClient(tokenCredential, scopes);
    }


    //Use tokenCredential.Authenticate(); to get the AuthenticationRecord to use for silent authentication
    //Works only for school and work accounts, not for personal accounts
    public static void InitializeWithCache(string clientId,
                                 string[] scopes,
                                 Func<DeviceCodeInfo, CancellationToken, Task> callBack)
    {
        AuthenticationRecord? authRecord = null;

        // if the AuthenticationRecord has been persisted from a previous execution deserialize it
        // and pass it to the DeviceCodeCredentialOptions
        if (System.IO.File.Exists(CacheFilename))
        {
            using FileStream readStream = new FileStream(CacheFilename, FileMode.Create, FileAccess.Read);

            authRecord = AuthenticationRecord.Deserialize(readStream);
        }

        // tokenCredential = new DeviceCodeCredential(new DeviceCodeCredentialOptions
        tokenCredential = new InteractiveBrowserCredential(new InteractiveBrowserCredentialOptions
        {
            TokenCachePersistenceOptions = new TokenCachePersistenceOptions
            {
                Name = "ExchangeCrimeSceneCleanerCliTokenCache",
                UnsafeAllowUnencryptedStorage = true,
            },
            AuthenticationRecord = authRecord,
            ClientId = clientId,
            TenantId = "common"
            //DeviceCodeCallback = callBack,
        });

        // if no AuthenticationRecord was found we need to call AuthenticateAsync to authenticate a user
        // and serialize the AuthenticationRecord which was returned. 
        if (authRecord == null)
        {
            authRecord = tokenCredential.Authenticate();
 
            using FileStream writeStream = new FileStream(CacheFilename, FileMode.Create, FileAccess.Write);

            authRecord.Serialize(writeStream);

            writeStream.Flush();
        }

        graphClient = new GraphServiceClient(tokenCredential, scopes);
    }

    public static async Task<string> GetAccessTokenAsync(string[] scopes)
    {
        var context = new TokenRequestContext(scopes);
        var response = await tokenCredential!.GetTokenAsync(context, new CancellationToken());
        Console.WriteLine($"Your token: {response.Token}");
        return response.Token;
    }

    
}
