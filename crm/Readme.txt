It should be possible to build, package restore and run the API locally in Visual Studio by simply pulling down the solution folder. Local host is spun up at https://localhost:44365/swagger/

TODO Productionisation tasks

Gif.Service\AuthConfig.cs - This will need plumbing into a hosted keystore (such as Azure Key Vault) with the desired credentials for API access (currently hardcoded for development purposes). The key/secret can be any desired values. This is for external clients to connect to *this* API.

Gif.Service\Startup.cs (line 95, AddDeveloperSigningCredential) - This should be swapped out to use a certificate for production instances, so the bearer access tokens work across multiple hosted instances and persist across service restarts. 

Gif.Service\Startup.cs (Line 104, AddIdentityServerAuthentication). Authority should represent the current url of the hosted api. Once api hosting is understood and environments have been created for dev/test/uat/production etc this will need swapping out accordingly.

Similairly appsettings.json has values which let the API connect to CRM. There will either need to be replacement of these values by a (currently unknown) deployment tool or a build transform done against the file. These values can be obtained per CRM environment by registering new app registrations against the main NHSD Azure active directory. 