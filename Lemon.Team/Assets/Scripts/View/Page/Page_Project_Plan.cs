using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Lemon.Team.Entity;

using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Services;

public class Page_Project_Plan : NavigatePage
{
    public ListViewControl PlanListView;
    private EProject EProject;
    protected override void Init()
    {
        base.Init();
        int panid = GetPar<int>(0);
        Debug.Log("panid:" + panid);
        BaseOperation_Service.Get<EProject>(panid, t =>
        {
            EProject = t.GetData() as EProject;
            PageTitle.Init(EProject.Name, App.Instance.Theme.TitleBgColor, App.Instance.Theme.TitleFontColor, BtnBack);
            PageTitle.AddButton("", App.Instance.ImageManger.ImageList[1], AddPlan);
            PageTitle.AddButton("", App.Instance.ImageManger.ImageList[9], GoToTeam);
            PageTitle.AddButton("", App.Instance.ImageManger.ImageList[2], DeleteProject);
            PageTitle.AddButton("", App.Instance.ImageManger.ImageList[8], GoToSet);
            PlanListView.OnItemClicked = OnItemClicked;
            BindData();
        });
    }

    void BtnBack(GameObject g)
    {
        App.Instance.PageGroup.ClosePage();
    }

    void GoToSet(GameObject g)
    {
        App.Instance.PageGroup.ShowPage("Page_Project_Info", false, EProject.ID);
    }

    void GoToTeam(GameObject g)
    {
        App.Instance.PageGroup.ShowPage("Page_Project_Team", false, EProject.ID);
    }

    void DeleteProject(GameObject g)
    {
        App.Instance.DialogBox.Show("提示信息", "", "你确定要删除项目吗？", 300, 150, c =>
        {
            BaseOperation_Service.Delete<EProject>(EProject, t =>
            {
                App.Instance.PageGroup.ClosePage();
                App.Instance.DialogBox.Hide();
                App.Instance.HintBox.Show("操作成功！");
            });
        }, null);
    }

    void AddPlan(GameObject g)
    {
        App.Instance.DialogBox.Show("添加计划", "Page_AddPlan", "", 350, 200, btn =>
        {
            EPlan p = new EPlan();
            p.Name = App.Instance.DialogBox.Content.GetComponentInChildren<InputField>().text;
            p.ProjectID = EProject.ID;
            p.BeginTime = System.DateTime.Now;
            p.EndTime = System.DateTime.Now;

            BaseOperation_Service.Create<EPlan>(p, t =>
            {
                BindData();
                App.Instance.DialogBox.Hide();
                App.Instance.HintBox.Show("操作成功！");
            });
        }, null);
    }

    private void BindData()
    {
        Plan_Service.ListMyProjectID(EProject.ID, t =>
        {
            List<EPlan> plist = t.GetData() as List<EPlan>;
            PlanListView.BindData<EPlan>("PlanItem", plist, (i, e) =>
            {
                i.name = "PlanItem_" + e.ID;
                i.transform.Find("Name").GetComponent<Text>().text = e.Name;
                i.transform.Find("BeginTime").GetComponent<Text>().text = e.BeginTime.ToString("yyyy-MM-dd");
                i.transform.Find("EndTime").GetComponent<Text>().text = e.EndTime.ToString("yyyy-MM-dd");
            });
        });
    }

    void OnItemClicked(GameObject g)
    {
        int id = int.Parse(g.name.Split('_')[1]);
        App.Instance.PageGroup.ShowPage("Page_Project_Plan_Task", false, id);
    }
}
