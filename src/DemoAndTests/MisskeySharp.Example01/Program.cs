using MisskeySharp.Entities;
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
#if false
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
                    Console.WriteLine(" {0} | {1}", note.User.Username.PadRight(10), note.Text.Replace("\n", " "));
                }
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