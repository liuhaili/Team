using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Lemon.Team.Entity;

using System.Collections.Generic;
using System.Linq;
using Lemon.Team.Entity.Enum;
using Assets.Scripts.Services;

public class Page_Project_Plan_Task : NavigatePage
{
    public TabControl Tab;
    public ListViewControl PeopleSelectListView;

    public GameObject PageContent1;
    public ListViewControl TaskListView;

    public GameObject PageContent2;
    public ListViewControl TaskTimeView;

    public GameObject PageContent3;
    public ListViewControl TaskGanttTimeView;
    public ListViewControl ListGanttTaskView;

    public RectTransform CpntentRect;

    private EPlan EPlan;
    private int selectPeopleID;
    protected override void Init()
    {
        base.Init();
        BaseOperation_Service.Get<EPlan>(GetPar<int>(0), t =>
        {
            EPlan = t.GetData() as EPlan;
            PageTitle.Init(EPlan.Name, App.Instance.Theme.TitleBgColor, App.Instance.Theme.TitleFontColor, BtnBack);
            PageTitle.AddButton("", App.Instance.ImageManger.ImageList[1], AddTask);
            PageTitle.AddButton("", App.Instance.ImageManger.ImageList[2], DeletePlan);
            PageTitle.AddButton("", App.Instance.ImageManger.ImageList[10], GoToChart);
            PageTitle.AddButton("", App.Instance.ImageManger.ImageList[8], GoToSet);
            Tab.Init(OnTabClicked, App.Instance.Theme.SelectedItemBgColor);
            selectPeopleID = -1;
            BindPeopleSelect();
            BindTaskListView();
        });
    }

    void OnTabClicked(GameObject g)
    {
        if (g.name == "TabItem_1")
        {
            PageContent1.SetActive(true);
            PageContent2.SetActive(false);
            PageContent3.SetActive(false);
            BindTaskListView();
        }
        else if (g.name == "TabItem_2")
        {
            PageContent1.SetActive(false);
            PageContent2.SetActive(true);
            PageContent3.SetActive(false);
            BindTaskTimeView();
        }
        else
        {
            PageContent1.SetActive(false);
            PageContent2.SetActive(false);
            PageContent3.SetActive(true);
            BindTaskGanttView();
        }
    }

    void GoToChart(GameObject g)
    {
        App.Instance.PageGroup.ShowPage("Page_Project_Plan_Chart", false, EPlan.ID);
    }

    void BtnBack(GameObject g)
    {
        App.Instance.PageGroup.ClosePage();
    }

    void GoToSet(GameObject g)
    {
        App.Instance.PageGroup.ShowPage("Page_Project_Plan_Info", false, EPlan.ID);
    }

    void DeletePlan(GameObject g)
    {
        App.Instance.DialogBox.Show("提示信息", "", "你确定要删除计划吗？", 300, 150, c =>
        {
            BaseOperation_Service.Delete<EPlan>(EPlan, t =>
            {
                App.Instance.PageGroup.ClosePage();
                App.Instance.DialogBox.Hide();
                App.Instance.HintBox.Show("操作成功！");
            });
        }, null);
    }

    void AddTask(GameObject g)
    {
        App.Instance.PageGroup.ShowPage("Page_Task_Editor", false, PageInfoMode.Add, 0, EPlan.ID,EPlan.ProjectID);
    }

    void OnTaskClicked(GameObject g)
    {
        int taskid = int.Parse(g.name.Split('_')[1]);
        App.Instance.PageGroup.ShowPage("Page_Task_Editor", false, PageInfoMode.Editor, taskid, EPlan.ID, EPlan.ProjectID);
    }

    void OnPeopleSelectClicked(GameObject g)
    {
        selectPeopleID = int.Parse(g.name.Split('_')[1]);
        BindTaskListView();
        BindTaskTimeView();
        BindTaskGanttView();
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
            }, true, true);
        });
    }

    void BindTaskListView()
    {
        Task_Service.ListByPlanID(EPlan.ID, selectPeopleID, t =>
         {
             List<ETask> plist = t.GetData() as List<ETask>;
             TaskListView.OnItemClicked = OnTaskClicked;
             TaskListView.BindData<ETask>("PlanTaskItem", plist, (i, e) =>
               {
                   i.name = "PlanTaskItem_" + e.ID.ToString();
                   i.transform.Find("TIState").GetComponent<Text>().text = e.StepName;
                   i.transform.Find("TIPriority").GetComponent<Text>().text = EnumMapping.GetText<TaskPriority>((int)e.Priority);
                   if (e.Priority == TaskPriority.First)
                       i.transform.Find("TIPriority").GetComponent<Text>().color = new Color(156 / 255f, 93 / 255f, 0 / 255f);
                   else if (e.Priority == TaskPriority.Hurry)
                       i.transform.Find("TIPriority").GetComponent<Text>().color = new Color(223 / 255f, 60 / 255f, 0 / 255f);
                   else if (e.Priority == TaskPriority.Urgent)
                       i.transform.Find("TIPriority").GetComponent<Text>().color = Color.red;
                   else
                       i.transform.Find("TIPriority").GetComponent<Text>().color = new Color(139 / 255f, 139 / 255f, 139 / 255f);
                   i.transform.Find("TIContent").GetComponent<Text>().text = e.Title;
                   i.transform.Find("TIBeginTime").GetComponent<Text>().text = e.BeginTime.ToString("yyyy-MM-dd");
                   i.transform.Find("TIEndTime").GetComponent<Text>().text = e.EndTime.ToString("yyyy-MM-dd");
                   if (e.TaskHeadID == 0)
                   {
                       i.transform.Find("TIPerson").GetComponent<Text>().text = "";
                   }
                   else
                   {
                       i.transform.Find("TIPerson").GetComponent<Text>().text = e.TaskHeadName;
                   }
               });
         });
    }

    void BindTaskTimeView()
    {
        Task_Service.ListByPlanID(EPlan.ID, selectPeopleID, t =>
        {
            List<ETask> plist = t.GetData() as List<ETask>;
            System.DateTime beginTime = System.DateTime.Now;
            System.DateTime endTime = System.DateTime.Now;
            if (plist.Count > 0)
            {
                beginTime = plist.Min(c => c.BeginTime);
                endTime = plist.Max(c => c.EndTime);
            }
            List<System.DateTime> datelist = new List<System.DateTime>();
            System.DateTime curTime = beginTime;
            while (curTime.Date <= endTime.Date)
            {
                datelist.Add(curTime.Date);
                curTime = curTime.AddDays(1);
            }
            datelist = datelist.OrderByDescending(c => c).ToList();
            TaskTimeView.BindData<System.DateTime>("TaskTimeItem", datelist, (i, e) =>
            {
                i.name = "TaskTimeItem_" + e.Date.ToString();
                i.transform.Find("Day").GetComponent<Text>().text = e.Date.ToString("yyyy.MM.dd");
                BindTaskTimeOne(i.transform.Find("Panel").Find("TaskTimeOneList").GetComponent<ListViewControl>(), e.Date, plist);
                int rowCount = plist.Count(c => e.Date >= c.BeginTime.Date && e.Date <= c.EndTime.Date);
                if (rowCount == 0)
                {
                    rowCount = 1;
                    i.transform.Find("Panel").GetComponent<Image>().color = new Color(140 / 255.0f, 140 / 255.0f, 140 / 255.0f);
                }
                else
                    i.transform.Find("Panel").GetComponent<Image>().color = new Color(105 / 255.0f, 197 / 255.0f, 251 / 255.0f);
                i.GetComponent<LayoutElement>().preferredHeight = rowCount * 50 + 16;
            }, false);
        });
    }

    void BindTaskTimeOne(ListViewControl listView, System.DateTime date, List<ETask> plist)
    {
        List<ETask> tasklist = plist.Where(c => date >= c.BeginTime.Date && date <= c.EndTime.Date).ToList();
        listView.BindData<ETask>("TaskTimeOneItem", tasklist, (i, e) =>
        {
            i.name = "TaskTimeOneItem_" + e.ID;
            i.SetActive(true);
            i.transform.Find("Idea").GetComponent<Text>().text = e.Title;
            App.Instance.ShowImage(i.transform.Find("Icon").GetComponent<RawImage>(), e.TaskHeadFace, 11);

        }, false);
    }

    void BindTaskGanttView()
    {
        Task_Service.ListByPlanID(EPlan.ID, selectPeopleID, t =>
        {
            List<ETask> plist = t.GetData() as List<ETask>;
            System.DateTime beginTime = System.DateTime.Now;
            System.DateTime endTime = System.DateTime.Now;
            if (plist.Count > 0)
            {
                beginTime = plist.Min(c => c.BeginTime);
                endTime = plist.Max(c => c.EndTime);
            }
            List<System.DateTime> datelist = new List<System.DateTime>();
            System.DateTime curTime = beginTime;
            while (curTime.Date <= endTime.Date)
            {
                datelist.Add(curTime.Date);
                curTime = curTime.AddDays(1);
            }
            datelist = datelist.OrderBy(c => c).ToList();


            TaskGanttTimeView.BindData<System.DateTime>("GanttTimeItem", datelist, (i, e) =>
            {
                i.transform.Find("Text").GetComponent<Text>().text = e.ToString("yyyy-MM-dd");
            }, false);

            ListGanttTaskView.BindData<ETask>("GanttTaskItem", plist.OrderBy(c => c.BeginTime).ToList(), (i, e) =>
            {
                float x = (float)(e.BeginTime.Date - beginTime.Date).TotalDays * 100.0f;
                float w = (float)(e.EndTime.Date.AddDays(1) - e.BeginTime.Date).TotalDays * 100.0f;
                i.transform.Find("Image").GetComponent<RectTransform>().anchoredPosition = new Vector2(x, 0);
                i.transform.Find("Image").GetComponent<RectTransform>().sizeDelta = new Vector2(w, 30);
                i.transform.Find("Image").Find("Text").GetComponent<Text>().text = e.Title;
            }, false);

            CpntentRect.sizeDelta = new Vector2(datelist.Count * 100, plist.Count * 40 + 50);
        });
    }
}
