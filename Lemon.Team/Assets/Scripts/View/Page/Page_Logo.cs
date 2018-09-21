using UnityEngine;
using System.Collections;
using Lemon.Team.Entity;

using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using Assets.Scripts;
using UnityEngine.SceneManagement;
using Assets.Scripts.Services;
using Newtonsoft.Json;

public class Page_Logo : ContentPage
{
    protected override void Init()
    {
        base.Init();
        this.Invoke("StartApp", 3);

        //sdk初始化
        //string appKey = "E33EE689-3669-DE0C-0DD1-076C52990CAB";
        //string appSecret = "97864736a14863615664db14fa73ab43";
        //string privateKey = "56F494230A83C07F88534C6628FB77B0";
        //string oauthLoginServer = "http://oauth.anysdk.com/api/OauthLoginDemo/Login.php";
        //AnySDK.getInstance().init(appKey, appSecret, privateKey, oauthLoginServer);
    }

    void StartApp()
    {
        string sessionStr = PlayerPrefs.GetString("Session");
        if (string.IsNullOrEmpty(sessionStr))
        {
            App.Instance.PageGroup.ShowPage("Page_Login", true, "Page_Task");
        }
        else
        {
            //Session.CurrentUser = LitJson.JsonMapper.ToObject<EUser>(sessionStr);
            Session.CurrentUser = JsonConvert.DeserializeObject<EUser>(sessionStr);
            Session.UserID = Session.CurrentUser.ID;
            App.Instance.CanShowNavigatePage = true;
            App.Instance.PageGroup.ShowPage("Page_Task", true);
        }
    }
}
