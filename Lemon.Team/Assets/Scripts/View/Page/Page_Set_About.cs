using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Lemon.Team.Entity;

using System.Collections.Generic;
using System.Linq;
using Lemon.Team.Entity.Enum;
using Assets.Scripts.Services;

public class Page_Set_About : NavigatePage
{
    public Text Content;
    protected override void Init()
    {
        base.Init();
        PageTitle.Init("关于我们", App.Instance.Theme.TitleBgColor, App.Instance.Theme.TitleFontColor, BtnBack);
        BindContent();
    }

    void BtnBack(GameObject g)
    {
        App.Instance.PageGroup.ClosePage();
    }

    void BindContent()
    {
        Configure_Service.GetValue("About", c =>
        {
            EConfigure config = c.GetData() as EConfigure;
            Content.text = config.Value;
        });

        
    }
}
