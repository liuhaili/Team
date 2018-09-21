using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using Lemon.Team.Entity;
using Assets.Scripts.Services;

public class Page_Set_ChangePassword : NavigatePage
{
    public InputField OldPsw;
    public InputField NewPsw;
    public InputField NewPswCheck;
    protected override void Init()
    {
        base.Init();
        PageTitle.Init("修改密码", App.Instance.Theme.TitleBgColor, App.Instance.Theme.TitleFontColor, BtnBack);
        PageTitle.AddButton("", App.Instance.ImageManger.ImageList[5], OnSubmit);
    }

    void BtnBack(GameObject g)
    {
        App.Instance.PageGroup.ClosePage();
    }

    void OnSubmit(GameObject g)
    {
        EUser user = Session.CurrentUser;
        if (user.Password != OldPsw.text)
        {
            App.Instance.HintBox.Show("原密码不正确！");
            return;
        }

        if (NewPsw.text != NewPswCheck.text)
        {
            App.Instance.HintBox.Show("新密码和确认密码不同！");
            return;
        }
        user.Password = NewPsw.text;
        BaseOperation_Service.Change<EUser>(user, t =>
        {
            App.Instance.HintBox.Show("修改密码成功！");
            App.Instance.PageGroup.ClosePage();
        });
    }
}
