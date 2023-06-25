# MisskeySharp

## What's this ?
MisskeySharp is a client library for .NET applications to utilize Misskey.


## Example
The usage examples are as follows:

### Get a new access token with "miauth"

** Step 1: Get a URL for authorize app page **  
We will prepare to obtain the access token using "miauth".

```csharp
var misskey = new MisskeyService("https://misskey.io/");
var authUri = misskey.GetAuthorizeUri(
                    "サンプル (my app) ", // App name
                    "https://www.a32kita.net/favicon.ico", // App icon url
                    "https://dummy.a32kita.net/callback", // Callback url
```

In 'miauth,' you can use any value for the GetAuthorizeUri parameter because it allows you to set the application name, icon, and callback URL during access token acquisition.

** Step 2: After the user approves the application in the browser, obtain the access token **

```csharp
await misskey.AuthorizeWithAuthorizeUriAsync(authUri);

// misskey.AccessToken: "98eY****************************"; <= Access Token
```
The obtained access token will be stored in the AccessToken property.


### Authenticate with the already obtained access token
If the initially obtained access token is being recorded, user approval will not be required from the second time onwards.

```csharp
await misskey.AuthorizeWithAccessTokenAsync("98eY****************************");
```


### Post a note
We will post a new note. In the following example, we will post a note with the content 'hello, world!!' to the timeline for followers.

```csharp
await misskey.PostAsync<Note, NoteCreated>("notes/create", new Note()
            {
                Text = "hello, world!!",
                Visibility = "followers",
            });
Console.WriteLine("Completed: {0}", resp.CreatedNote.Id);
```

For the parameters specified here, please refer to the official documentation:  
https://misskey-hub.net/docs/api/endpoints/notes/create.html