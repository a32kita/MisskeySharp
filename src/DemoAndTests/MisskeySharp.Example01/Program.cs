﻿using System.Net;

namespace MisskeySharp.Example01
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // 初期化
            var misskey = new MisskeyService("https://misskey.io/");

            // 認証用の URL の取得
            var authUri = misskey.GetAuthorizeUri(
                "サンプル (my app) " + DateTime.Now.ToString("yyyyMMdd-HHmmsss-fff"),
                "https://www.a32kita.net/favicon.ico",
                "https://dummy.a32kita.net/callback",
                MisskeyPermissions.Write_notes | MisskeyPermissions.Read_account);
            Console.WriteLine("下記認証用 URL を開いて、アクセスを [承認] してください ...");
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

            // デモ投稿
            var demoText = "(Debug) API リクエスト テスト\nこれは Misskey API のコール試験投稿です。\n" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff");
            Console.WriteLine("次の内容を投稿してもよろしいですか？");
            Console.WriteLine(demoText);
            Console.WriteLine();
            Console.WriteLine("[Enter] キーを押下して続行します ...");
            Console.ReadLine();

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