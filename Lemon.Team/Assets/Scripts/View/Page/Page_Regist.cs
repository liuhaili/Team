using UnityEngine;
using System.Collections;
using Lemon.Team.Entity;

using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using Assets.Scripts;
using UnityEngine.SceneManagement;
using Assets.Scripts.Services;

public class Page_Regist : NavigatePage
{
    public InputField IptRegistPhone;
    public InputField IptRegistName;
    public InputField IptRegistPsw;
    public InputField IptRegistRePsw;
    public Button BtnRegist;

    protected override void Init()
    {
        base.Init();
        PageTitle.Init("注册", App.Instance.Theme.TitleBgColor, App.Instance.Theme.TitleFontColor, BtnBack);
        EventListener.Get(BtnRegist.gameObject).onClick = RegistEvent;
    }

    void BtnBack(GameObject g)
    {
        App.Instance.PageGroup.ClosePage();
    }

    void RegistEvent(GameObject g)
    {
        EUser user = new EUser();
        user.Phone = IptRegistPhone.text;
        user.Name = IptRegistName.text;
        user.Password = IptRegistPsw.text;
        User_Service.Regist(user, t =>
        {
            App.Instance.HintBox.Show("注册成功！");
        });
    }
}
