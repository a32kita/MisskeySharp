# MisskeySharp

## What's this ?
MisskeySharp is a client library for .NET applications to utilize Misskey.


## Example
The usage examples are as follows:

#### Get a new access token with miauth

** Step 1: Get a URL for authorize app page **

```csharp
var misskey = new MisskeyService("https://misskey.io/");
var authUri = misskey.GetAuthorizeUri(
                    "サンプル (my app) ", // App name
                    "https://www.a32kita.net/favicon.ico", // App icon url
                    "https://dummy.a32kita.net/callback", // Callback url
```

** Step 2: After the user [authorizes] the app, retrieve the access token **

```csharp
await misskey.AuthorizeWithAuthorizeUriAsync(authUri);

// misskey.AccessToken: "98eY****************************"; <= Access Token
```


#### Authenticate with the already obtained access token

```csharp
await misskey.AuthorizeWithAccessTokenAsync("98eY****************************");
```


#### Post a note

```csharp
await misskey.PostAsync<Note, Object>("notes/create", new Note()
            {
                Text = "hello, world !!",
            });
```
