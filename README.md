[![CI](https://github.com/AzureAD/microsoft-identity-abstractions-for-dotnet/actions/workflows/dotnetcore.yml/badge.svg)](https://github.com/AzureAD/microsoft-identity-abstractions-for-dotnet/actions/workflows/dotnetcore.yml)

# Microsoft.Identity.Abstractions

Microsoft.Identity.Abstractions contain interfaces and POCO classes used in the Microsoft .NET authentication
libraries (Microsoft.IdentityModel, MSAL.NET and Microsoft.Identity.Web). It exposes concepts in three domains:

1. Application options and credentials loader

   The application options are typically the options that you find in configuration files like the appsettings.json
   file. They describe the authentication aspects of your application. The library offers two layer. A standard
   layer, and a Microsoft Identity platform specialization.

   ![image](https://user-images.githubusercontent.com/13203188/215366542-b2a8488b-ff3c-4de7-9634-7f6f29a9b2d3.png)

   An important part of the application options are the credentials. In addition to the credential descriptions, the
   library offers extensibility mechanisms so that implementers can add their own credential source loaders.
   
   ![image](https://user-images.githubusercontent.com/13203188/206561108-acee3ef2-0183-4390-9238-a053a6e70aee.png)

   There can be several application options with different names (for instance in ASP.NET Core these would be different
   authentication schemes)

2. Acquire tokens from Azure AD

   Once configured, an application can acquire tokens from the Identity provider. This is a low level API, in the sense that
   you would probably prefer to call downstream web APIs without having to be preoccupied about the authentication aspects. If you
   really want to use the lower level API, you should:
   - get hold of a ITokenAcquirerFactory. Implementations can provide a TokenAcquirerFactory for instance, with a singleton.
   - get a ITokenAcquirer (by its name, for instance). This corresponds to the application options
   - From the token acquirer get a token for on behalf of the user, or the app. If you don't specify any AcquireTokenOptions, 
     the implementation should do its best effort. The AcquireTokenOptions enable you to override the defaults.

   ![image](https://user-images.githubusercontent.com/13203188/215366777-e405d34b-637c-4d38-a190-1217e0de4b47.png)

3. Calling downstream web APIs

   It's also possible (and recommended) to use higher level APIs:
   - IDownstreamApi enables you to call a downstream web API and let the implementation handle the serialization of the
     input parameter (if any), handling the getting the authorization header and attaching it to the HttpClient, call
     the downstream web API, handle errors, deserialize the answer and return it as a strongly typed object. You can
     use customize all these steps, for instance by providing your own serializer / deserializer.
   - IAuthorizationHeaderProvider is the component that provides the authorization header, delegating to the ITokenAcquirer.
     Whereas ITokenAcquirer only knows about tokens, IAuthorizationHeaderProvider knows about protocols (for instance bearer,
     Pop, etc ...)

   ![image](https://user-images.githubusercontent.com/13203188/215366832-911a24a9-e077-4ede-b2b4-67c5fa06a82d.png)

## Contributing

This project welcomes contributions and suggestions.  Most contributions require you to agree to a
Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us
the rights to use your contribution. For details, visit https://cla.opensource.microsoft.com.

When you submit a pull request, a CLA bot will automatically determine whether you need to provide
a CLA and decorate the PR appropriately (e.g., status check, comment). Simply follow the instructions
provided by the bot. You will only need to do this once across all repos using our CLA.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or
contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

## Trademarks

This project may contain trademarks or logos for projects, products, or services. Authorized use of Microsoft 
trademarks or logos is subject to and must follow 
[Microsoft's Trademark & Brand Guidelines](https://www.microsoft.com/en-us/legal/intellectualproperty/trademarks/usage/general).
Use of Microsoft trademarks or logos in modified versions of this project must not cause confusion or imply Microsoft sponsorship.
Any use of third-party trademarks or logos are subject to those third-party's policies.
