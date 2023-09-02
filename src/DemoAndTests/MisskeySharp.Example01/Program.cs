using MisskeySharp.Entities;
using MisskeySharp.Streaming;
using System.Collections.Specialized;
using System.Net;

namespace MisskeySharp.Example01
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // 初期化
            var misskey = new MisskeyService("https://misskey.io/");
            var accessToken = String.Empty;

            Console.WriteLine("取得済みのアクセストークンがある場合は、入力してください。");
            Console.Write(">");
            accessToken = Console.ReadLine();

            
            // 認証
            if (String.IsNullOrEmpty(accessToken))
            {
                // 認証用の URL の取得
                var authUri = misskey.GetAuthorizeUri(
                    "サンプル (my app) " + DateTime.Now.ToString("yyyyMMdd-HHmmsss-fff"),
                    "https://www.a32kita.net/favicon.ico",
                    "https://dummy.a32kita.net/callback",
                    MisskeyPermissions.Write_notes | MisskeyPermissions.Read_account);
                Console.WriteLine("下記認証用 URL を開いて、アプリのアクセスを [承認] してください ...");
                Console.WriteLine(authUri.Uri);
                Console.WriteLine();

                // 認証 (アクセストークンの取得)
                auth:
                Console.WriteLine("[承認] したら [Enter] キーを押下して続行します ...");
                Console.ReadLine();
                try
                {
                    await misskey.AuthorizeWithAuthorizeUriAsync(authUri);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("認証に失敗しました。");
                    Console.WriteLine(ex.Message);
                    goto auth;
                }
                Console.WriteLine("認証に成功しました;");
                Console.WriteLine("Access Token: {0}", s_maskString(misskey.AccessToken));
                Console.WriteLine();
            }
            else
            {
                await misskey.AuthorizeWithAccessTokenAsync(accessToken);
            }


            // 自分の情報を取得
            Console.WriteLine("自身の情報を取得します ...");
            var userId = String.Empty;
            try
            {
                var resp = await misskey.I.Get();
                Console.WriteLine("I: {0} (@{1}) id={2}", resp.Name, resp.Username, resp.Id);

                userId = resp.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.GetType().Name}");
                Console.WriteLine($"       {ex.Message}");
                Console.WriteLine();
                Console.WriteLine("自身の情報の取得に失敗したため、デモを終了します。");
                Environment.Exit(1);
            }


            // デモ投稿
#if false
            var demoText = "(Debug) API リクエスト テスト\nこれは Misskey API のコール試験投稿です。\n" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") + "\n\n#a32kita_debug_notes";
            //Console.WriteLine("次の内容を投稿してもよろしいですか？");
            Console.WriteLine(demoText);
            Console.WriteLine();
            //Console.WriteLine("[Enter] キーを押下して続行します ...");
            //Console.ReadLine();
            try
            {
                var resp = await misskey.Notes.Create(new Note()
                {
                    Text = demoText,
                    //Visibility = "followers",
                });

                Console.WriteLine("ノートの投稿が完了しました: {0}", resp.CreatedNote.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.GetType().Name}");
                Console.WriteLine($"       {ex.Message}");
            }
            Console.WriteLine();
#endif

            // デモ検索
#if true
            var demoKeyword = "#a32kita_debug_notes";
            Console.WriteLine("次のキーワードでノートを検索します: {0}", demoKeyword);
            //Console.WriteLine("[Enter] キーを押下して続行します ...");
            //Console.ReadLine();
            try
            {
                var resp = await misskey.Notes.Search(new NoteSearchQuery()
                {
                    Query = demoKeyword,
                    Limit = 20,
                });

                Console.WriteLine("ノート検索結果;");
                foreach (var note in resp)
                {
                    Console.WriteLine(" {0} | {1}", note.User.Username.PadRight(10), note.Text.Replace("\n", " "));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.GetType().Name}");
                Console.WriteLine($"       {ex.Message}");
            }
            Console.WriteLine();
#endif

            // フォロワー取得
#if true
            Console.WriteLine("フォローを取得します。");
            //Console.WriteLine("[Enter] キーを押下して続行します ...");
            //Console.ReadLine();
            try
            {
                var resp = await misskey.Users.Following(new UsersFollowingFollowersQuery()
                {
                    UserId = userId,
                });

                Console.WriteLine("フォロー取得結果;");
                foreach (var follow in resp)
                {
                    Console.WriteLine(" {0} | {1}", follow.Followee.Username.PadRight(20), follow.Followee.Name);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.GetType().Name}");
                Console.WriteLine($"       {ex.Message}");
            }
            Console.WriteLine();
#endif

            // ユーザーのノートの取得
#if true
            Console.WriteLine("ユーザーのノートの取得");
            //Console.WriteLine("[Enter] キーを押下して続行します ...");
            //Console.ReadLine();
            try
            {
                var resp = await misskey.Users.Notes(new UsersNoteQuery()
                {
                    UserId = userId,
                });

                Console.WriteLine("ユーザーノート取得結果;");
                foreach (var note in resp)
                {
                    Console.WriteLine(" {0} | {1}", note.User.Username.PadRight(10), note.Text?.Replace("\n", " "));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.GetType().Name}");
                Console.WriteLine($"       {ex.Message}");
            }
#endif

            // ユーザーの検索
#if false
            Console.WriteLine("ユーザーの検索");
            try
            {
                var resp = await misskey.Users.Search(new UsersSearchQuery()
                {
                    Query = "a32kita"
                });

                Console.WriteLine("ユーザーの検索結果");
                foreach (var user in resp)
                {
                    Console.WriteLine(" {0} (@{1})", user.Name, user.Username);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.GetType().Name}");
                Console.WriteLine($"       {ex.Message}");
            }
#endif

            // 他人のユーザー情報の取得
#if true
            Console.WriteLine("他のユーザーの情報の取得");
            var utataneBotUserId = String.Empty;
            try
            {
                var resp = await misskey.Users.Show(new UsersShowParameter()
                {
                    Username = "utatane_live_bot",
                });

                Console.WriteLine("ユーザー情報の取得結果;");
                Console.WriteLine(" {0} (@{1}) / {2}", resp.Name, resp.Username, resp.Id);

                utataneBotUserId = resp.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.GetType().Name}");
                Console.WriteLine($"       {ex.Message}");
            }
#endif

            // 通知の取得
#if true
            Console.WriteLine("通知の取得");
            try
            {
                var resp = await misskey.I.Notifications(new NotificationParameter()
                {

                });

                Console.WriteLine("通知の取得結果;");
                foreach (var n in resp)
                {
                    Console.WriteLine(" {0}: {1} by @{2}", n.CreatedAt.ToString("MM/dd HH:mm:ss"), n.Type, n.User?.Username);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.GetType().Name}");
                Console.WriteLine($"       {ex.Message}");
            }
#endif

            // タイムラインの取得
#if false
            Console.WriteLine("タイムラインの取得");
            var latestNoteId = String.Empty;
            try
            {
                var resp = await misskey.Notes.Timeline(new NotesTimelineParameter()
                {

                });

                Console.WriteLine("タイムライン取得結果;");
                foreach (var n in resp)
                {
                    var note = n;
                    if (note.Renote != null)
                    {
                        note = note.Renote;
                    }

                    if (String.IsNullOrEmpty(latestNoteId))
                    {
                        latestNoteId = note.Id;
                    }

                    Console.WriteLine(" {0} | {1}", note.User?.Username?.PadRight(10), note.Text?.Replace("\n", " "));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.GetType().Name}");
                Console.WriteLine($"       {ex.Message}");
            }
#endif

            // トレンドの取得
#if false
            Console.WriteLine("トレンドの取得");
            try
            {
                var resp = await misskey.Hashtags.Trend();

                Console.WriteLine("トレンド取得結果");
                foreach (var trend in resp)
                {
                    Console.WriteLine(" {0}", trend.Tag);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.GetType().Name}");
                Console.WriteLine($"       {ex.Message}");
            }
#endif


            // ふぁぼ
#if false
            Console.WriteLine("お気に入り登録");
            try
            {
                await misskey.Notes.Favorites.Create(new NotesFavoriteCreateParameter()
                {
                    NoteId = latestNoteId
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.GetType().Name}");
                Console.WriteLine($"       {ex.Message}");
            }
#endif

#if false
            // ストリーミング
            Console.WriteLine("ストリーミング");
            try
            {
                var noteReceived = new Action<MisskeyNoteReceivedEventArgs>(e =>
                {
                    var note = e.NoteMessage.Body.Body;
                    if (note == null)
                        return;

                    var rn = note.Renote != null;
                    if (rn)
                        note = note.Renote;

                    Console.WriteLine("R {0}: (@{1}) {2}", rn ? "RENOTE" : "NORMAL", note?.User?.Username, note?.Text);
                });

                misskey.Streaming.NoteReceived += (sender, e) => noteReceived(e);
                misskey.Streaming.ConnectionClosed += (sender, e) => Console.WriteLine("Streaming connection: closed.");
                
                var st = misskey.Streaming.Connect(MisskeyStreamingChannels.HybridTimeline);
                
                Console.ReadLine();

                misskey.Streaming.Disconnect(st);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.GetType().Name}");
                Console.WriteLine($"       {ex.Message}");
            }
#endif

#if true
            Console.WriteLine("フォロー");
            try
            {
                var resp = await misskey.Following.Create(new FollowRequestParameter()
                {
                    UserId = utataneBotUserId,
                });

                Console.WriteLine("ユーザー情報の取得結果;");
                Console.WriteLine(" {0} (@{1}) / {2}", resp?.Name, resp?.Username, resp?.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.GetType().Name}");
                Console.WriteLine($"       {ex.Message}");
            }
#endif


            // デモ終わり
            Console.WriteLine("サンプル プログラムによるデモが完了しました。");
            Console.WriteLine("[Enter] キーを押下して終了します ...");
            Console.ReadLine();
        }

        static string s_maskString(string s)
        {
            var unmaskLen = 4;
            return s.Substring(0, unmaskLen) + new String('*', s.Length - unmaskLen);
        }
    }
}