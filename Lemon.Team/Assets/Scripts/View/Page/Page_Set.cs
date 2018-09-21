using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Lemon.Team.Entity;

using System.Collections.Generic;
using System.Linq;
using Lemon.Team.Entity.Enum;
using Assets.Scripts.Services;
using Assets.Scripts;

public class Page_Set : Page_MenuBase
{
    public Button BtnAbout;
    public Button BtnFeedback;
    public Button BtnVersionCheck;
    public Toggle BtnIsSendNotic;
    public Button BtnChangePsw;
    public Button BtnChangeUserIfo;
    public Button BtnExitAccount;
    public RawImage UserFace;
    public Text UserName;
    protected override void Init()
    {
        base.Init();
        PageTitle.Init("设置", App.Instance.Theme.TitleBgColor, App.Instance.Theme.TitleFontColor, null, MenuClicked);
        EventListener.Get(BtnAbout.gameObject).onClick = OnAboutClicked;
        EventListener.Get(BtnFeedback.gameObject).onClick = OnFeedbackClicked;
        EventListener.Get(BtnVersionCheck.gameObject).onClick = OnBtnVersionCheckClicked;
        EventListener.Get(BtnIsSendNotic.gameObject).onClick = OnBtnIsSendNoticClicked;
        EventListener.Get(BtnChangePsw.gameObject).onClick = OnBtnChangePswClicked;
        EventListener.Get(BtnChangeUserIfo.gameObject).onClick = OnBtnChangeUserIfoClicked;
        EventListener.Get(BtnExitAccount.gameObject).onClick = OnBtnExitAccountClicked;
        BindData();
        UserName.text = Session.CurrentUser.Name;
        App.Instance.ShowImage(UserFace, Session.CurrentUser.Face,11);
    }

    void MenuClicked(GameObject g)
    {
        App.Instance.DetailPageBox.Show("Page_Navigation", null, null);
    }

    void BindData()
    {
        Configure_Service.GetValue("IsSendNotic", c => 
        {
            EConfigure config = c.GetData() as EConfigure;
            BtnIsSendNotic.isOn = bool.Parse(config.Value);
        });
    }

    void OnAboutClicked(GameObject g)
    {
        App.Instance.PageGroup.ShowPage("Page_Set_About", false);
    }

    void OnFeedbackClicked(GameObject g)
    {
        App.Instance.PageGroup.ShowPage("Page_Set_Feedback", false);
    }

    void OnBtnVersionCheckClicked(GameObject g)
    {
        App.Instance.HintBox.Show("当前是最新版本！");
    }

    void OnBtnChangePswClicked(GameObject g)
    {
        App.Instance.PageGroup.ShowPage("Page_Set_ChangePassword", false);
    }

    void OnBtnChangeUserIfoClicked(GameObject g)
    {
        App.Instance.PageGroup.ShowPage("Page_Set_UserInfo", false);
    }

    void OnBtnExitAccountClicked(GameObject g)
    {
        Session.UserID = 0;
        PlayerPrefs.SetString("Session", "");
        if (Session.UserID <= 0)
        {
            App.Instance.PageGroup.ShowPage("Page_Login", true,"Page_Set");
        }
    }

    void OnBtnIsSendNoticClicked(GameObject g)
    {
        Configure_Service.GetValue("IsSendNotic", c =>
        {
            EConfigure config = c.GetData() as EConfigure;
            if (BtnIsSendNotic.isOn)
                config.Value = "True";
            else
                config.Value = "False";
            BaseOperation_Service.Change<EConfigure>(config, t =>
            {
                App.Instance.HintBox.Show("设置成功！");
            });
        });
    }
}
