using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using Lemon.Team.Entity;
using Assets.Scripts.Services;

public class Page_Set_Feedback : NavigatePage
{
    public InputField Content;
    protected override void Init()
    {
        base.Init();
        PageTitle.Init("问题反馈", App.Instance.Theme.TitleBgColor, App.Instance.Theme.TitleFontColor, BtnBack);
        PageTitle.AddButton("", App.Instance.ImageManger.ImageList[5], OnSubmit);
    }

    void BtnBack(GameObject g)
    {
        App.Instance.PageGroup.ClosePage();
    }

    void OnSubmit(GameObject g)
    {
        EFeedback feedback = new EFeedback()
        {
            SendUserID = Session.UserID,
            Content = Content.text,
            CrateTime = System.DateTime.Now
        };
        BaseOperation_Service.Create<EFeedback>(feedback, t =>
        {
            App.Instance.HintBox.Show("反馈成功！");
            App.Instance.PageGroup.ClosePage();
        });
    }
}
