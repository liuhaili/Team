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

public class Page_Login : NavigatePage
{
    public InputField IptLoginPhone;
    public InputField IptLoginPsw;
    public Button BtnLogin;
    public Button BtnQQLogin;
    public Toggle RememberMe;

    private string OpenID;
    protected override void Init()
    {
        base.Init();
        PageTitle.Init("登录", App.Instance.Theme.TitleBgColor, App.Instance.Theme.TitleFontColor);
        PageTitle.AddButton("注册", null, GoRegist);
        EventListener.Get(BtnLogin.gameObject).onClick = LoginEvent;
        EventListener.Get(BtnQQLogin.gameObject).onClick = LoginQQEvent;

        PlatformCallBackListener.Instance.OnLogin = OnLoginComplated;
        PlatformCallBackListener.Instance.OnGetUserInfo = OnGetUserInfoComplated;
    }

    void GoRegist(GameObject g)
    {
        App.Instance.PageGroup.ShowPage("Page_Regist", false);
    }

    void OnLoginServerBack(EUser user)
    {
        if (user == null)
        {
            App.Instance.HintBox.Show("用户名或密码错误！");
            return;
        }
        Session.UserID = user.ID;
        Session.CurrentUser = user;
        App.Instance.CanShowNavigatePage = true;
        if (RememberMe.isOn)
        {
            //PlayerPrefs.SetString("Session", LitJson.JsonMapper.ToJson(user));
            PlayerPrefs.SetString("Session",JsonConvert.SerializeObject(user));
        }
        string toPageName = this.GetPar<string>(0);
        App.Instance.PageGroup.ShowPage(toPageName, true);
    }

    void LoginEvent(GameObject g)
    {
        User_Service.Login(IptLoginPhone.text, IptLoginPsw.text, PlatformDifferences.GetPushClientID(), t =>
        {
            EUser user = t.GetData() as EUser;
            OnLoginServerBack(user);
        });
    }

    void LoginQQEvent(GameObject g)
    {
        PlatformDifferences.Login();
    }

    void OnLoginComplated(string openid)
    {
        OpenID = openid;
        PlatformDifferences.GetUserInfo();
    }

    void OnGetUserInfoComplated(string name, string face)
    {
        User_Service.PlatformLogin(OpenID, name, face
            , PlatformDifferences.GetPushClientID(), t =>
        {
            EUser user = t.GetData() as EUser;
            OnLoginServerBack(user);
        });
    }
}
