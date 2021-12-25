// Roger Briggen license this file to you under the MIT license.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;
using Microsoft.Graph;

namespace Roger.Briggen.ExchangeCrimeSceneCleanerLib;

public class MSGraphHelper
{
    //public const string BaseURL = "https://graph.microsoft.com/v1.0/me/";


    private static TokenCredential? tokenCredential;
    //private static VisualStudioCredential? tokenCredential;

    public static GraphServiceClient? graphClient;

    public static void Initialize(string clientId,
                                  string[] scopes,
                                  Func<DeviceCodeInfo, CancellationToken, Task> callBack)
    {
        tokenCredential = new DeviceCodeCredential(callBack, "common", clientId);
        graphClient = new GraphServiceClient(tokenCredential, scopes);
    }

    public static async Task<string> GetAccessTokenAsync(string[] scopes)
    {
        var context = new TokenRequestContext(scopes);
        var response = await tokenCredential!.GetTokenAsync(context, new CancellationToken());
        return response.Token;
    }
}
