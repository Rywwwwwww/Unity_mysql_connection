/*========================================
    ログイン処理
  ========================================*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//UIで用意したオブジェクトを操作する
using UnityEngine.UI;
//ハッシュ化用モジュール
using System.Security.Cryptography;
using System.Text;
//Mysql関連
using System;
using MySql.Data;
using MySql.Data.MySqlClient;


public class Login_Func : MonoBehaviour{
  
  //ログインボタン押下時
  public void TextAppare(){
    
    //変数宣言
    string Mail_Str;  //メール
    string Pass_Str;  //パスワード

    //DB接続設定
    string connCmd =
                      "server=localhost;" +           // 接続先サーバ
                      "database=unity_test;" +        // 接続先データベース
                      "port=3306;" +                  // 接続ポート
                      "userid=root;" +                // 接続ユーザーID
                      "password=root";                // 接続パスワード

    Debug.Log("ログイン処理を開始");

    //Mail_Inputに入力された文字を取得(メールアドレス)
    Text MailData = GameObject.Find("Mail_Input/Text").GetComponent<Text>();
    //Password_Inputに入力された文字を取得(パスワード)
    Text PasswordData = GameObject.Find("Password_Input/Text").GetComponent<Text>();

    //文字型に移行
    Mail_Str = MailData.text.ToString();
    Pass_Str = PasswordData.text.ToString();

    //入力チェック(どちらかが未入力なら処理終了)
    if( Mail_Str == "" || Pass_Str == "" ){
      Debug.Log("ERROR：入力フォーム未入力");
      return;
    }

    /****** ハッシュ化処理 ******/
    //パスワードをバイト配列に変換
    var ByteArray = Encoding.UTF8.GetBytes(Pass_Str);
    //ハッシュ計算処理(SHA256)
    var csp = new SHA256CryptoServiceProvider();
    var hashBytes = csp.ComputeHash(ByteArray);
    //バイト配列を文字列に変換
    var Pass_Hash = new StringBuilder();
    foreach (var hashByte in hashBytes) {
      Pass_Hash.Append(hashByte.ToString("x2"));
    }

    //入力データ確認
    Debug.Log("INFO：メールアドレス【" + Mail_Str + "】");
    Debug.Log("INFO：パスワード【" + Pass_Str + "】");
    Debug.Log("INFO：ハッシュ化【" + Pass_Hash + "】");
  
  
    //DB接続処理
    Debug.Log("MySQLと接続中...");
    using (var conn = new MySqlConnection(connCmd)){
      try{
        //mysqlに接続開始
        conn.Open();
        //selectコマンドをmysqlに送信
        using(var cmd = new MySqlCommand("select * from users;", conn)){
          using(var dr = cmd.ExecuteReader()){
            while (dr.Read()){
              //順番に結果をログに出力
              Debug.Log(dr[0] + "," + dr[1] + "," + dr[2] + "," + dr[3]);
            }
            dr.Close();
          }
        }
      }
      catch (Exception ex){
        Debug.Log(ex.ToString());
      }
      finally{
        try{
          //mysql接続終了
          conn.Close();
          Debug.Log("接続を終了しました");
        }
        catch{
          Debug.Log("Close Error");
        }
      }
    }
  }
}


