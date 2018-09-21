using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Lemon.Team.Entity;

using System.Collections.Generic;
using System.Linq;
using Lemon.Team.Entity.Enum;
using Assets.Scripts.Services;

public class Page_Project_Plan_Chart : NavigatePage
{
    public ListViewControl PeopleSelectListView;
    public ListViewControl TaskChartView;

    private EPlan EPlan;
    private int selectPeopleID;
    protected override void Init()
    {
        base.Init();
        BaseOperation_Service.Get<EPlan>(GetPar<int>(0), t =>
        {
            EPlan = t.GetData() as EPlan;
            PageTitle.Init("完成情况-" + EPlan.Name, App.Instance.Theme.TitleBgColor, App.Instance.Theme.TitleFontColor, BtnBack);
            selectPeopleID = -1;
            BindPeopleSelect();
            BindTaskListView();
        });
    }

    void BtnBack(GameObject g)
    {
        App.Instance.PageGroup.ClosePage();
    }

    void OnPeopleSelectClicked(GameObject g)
    {
        selectPeopleID = int.Parse(g.name.Split('_')[1]);
        BindTaskListView();
    }

    void BindPeopleSelect()
    {
        ProjectTeam_Service.ListByProjectID(EPlan.ProjectID, t => 
        {
            PeopleSelectListView.OnItemClicked = OnPeopleSelectClicked;
            List<EProjectTeam> projectTeam = t.GetData() as List<EProjectTeam>;
            projectTeam.Insert(0, new EProjectTeam() { ProjectID = EPlan.ProjectID, UserID = -1 });
            PeopleSelectListView.BindData<EProjectTeam>("PeopleSelectItem", projectTeam, (i, e) =>
            {
                if (e.UserID == -1)
                {
                    i.name = "PeopleSelectItem_" + e.UserID.ToString();
                    i.transform.Find("Text").GetComponent<Text>().text = "全部";
                }
                else
                {
                    i.name = "PeopleSelectItem_" + e.UserID.ToString();
                    i.transform.Find("Text").GetComponent<Text>().text = e.UserName;
                }
                i.gameObject.SetActive(true);
            }, true, true);
        });
    }

    void BindTaskListView()
    {
        Task_Service.ListByPlanID(EPlan.ID, selectPeopleID, t => 
        {
            List<ETask> plist = t.GetData() as List<ETask>;
            List<KeyValuePair<string, int>> taskStateGroup = new List<KeyValuePair<string, int>>();
            foreach (var g in plist.GroupBy(c => c.StepName))
                taskStateGroup.Add(new KeyValuePair<string, int>(g.Key, g.Count()));

            TaskChartView.BindData<KeyValuePair<string, int>>("PlanChartItem", taskStateGroup, (i, e) =>
            {
                i.name = "PlanChartItem_" + e.Value.ToString();

                float bfb = (float)e.Value / plist.Count;
                float h = (int)(bfb * 300);

                i.transform.Find("Text").GetComponent<Text>().text = e.Value + "(" + (int)(bfb * 100) + "%)";
                i.transform.Find("Text (1)").GetComponent<Text>().text = e.Key;
                i.GetComponent<LayoutElement>().preferredHeight = h;
                i.GetComponent<Image>().color = Color.green;
            });
        });
    }
}
