using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Lemon.Team.Entity;

using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using Lemon.Team.Entity.Enum;
using Assets.Scripts.Services;

public class Page_Project_Plan_Info : NavigatePage
{
    public InputField PalnName;
    public DatePickerSelectButton PalnBeginTime;
    public DatePickerSelectButton PalnEndTime;

    EPlan ThePlan;
    protected override void Init()
    {
        base.Init();
        PageTitle.Init("计划详情", App.Instance.Theme.TitleBgColor, App.Instance.Theme.TitleFontColor, BtnBack);
        PageTitle.AddButton("", App.Instance.ImageManger.ImageList[5], OnSavePlan);
        BaseOperation_Service.Get<EPlan>(this.GetPar<int>(0), t =>
        {
            ThePlan = t.GetData() as EPlan;
            BindData();
        });
    }

    void BtnBack(GameObject g)
    {
        App.Instance.PageGroup.ClosePage();
    }

    void OnSavePlan(GameObject g)
    {
        if (string.IsNullOrEmpty(PalnName.text))
            return;
        ThePlan.Name = PalnName.text;
        ThePlan.BeginTime = PalnBeginTime.GetTime();
        ThePlan.EndTime = PalnEndTime.GetTime();
        BaseOperation_Service.Change<EPlan>(ThePlan, t =>
        {
            App.Instance.HintBox.Show("操作成功！");
            App.Instance.PageGroup.ClosePage();
        });
    }

    void BindData()
    {
        PalnName.text = ThePlan.Name;
        PalnBeginTime.SetTime(ThePlan.BeginTime);
        PalnEndTime.SetTime(ThePlan.EndTime);
    }
}
