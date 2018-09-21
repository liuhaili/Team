using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Lemon.Team.Entity;

using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using Lemon.Team.Entity.Enum;
using Assets.Scripts.Services;

public class Page_Task : Page_MenuBase
{
    public InputField IptQuickTaskContent;
    public Button BtnQuickTaskAdd;
    public ListViewControl PlanListView;
    public Text txtPosition;
    public Toggle ShowComplatedTask;

    private EUserSearch UserSearch;
    protected override void Init()
    {
        base.Init();

        EventListener.Get(BtnQuickTaskAdd.gameObject).onClick = OnQuickTaskAdd;
        PlanListView.OnItemClicked = OnTaskClicked;
        EventListener.Get(ShowComplatedTask.gameObject).onClick = ShowComplatedTaskClicked;
        
        UserSearch = this.GetPar<EUserSearch>(0);
        if (UserSearch != null)
        {
            PageTitle.Init(UserSearch.Name, App.Instance.Theme.TitleBgColor, App.Instance.Theme.TitleFontColor, null, MenuClicked);
            PageTitle.AddButton("", App.Instance.ImageManger.ImageList[0], TaskSearch);
            BindData();
        }
        else
        {
            UserSearch_Service.GetDefault(t =>
            {
                UserSearch = t.GetData() as EUserSearch;
                if (UserSearch == null)
                    PageTitle.Init("全部任务", App.Instance.Theme.TitleBgColor, App.Instance.Theme.TitleFontColor, null, MenuClicked);
                else
                    PageTitle.Init(UserSearch.Name, App.Instance.Theme.TitleBgColor, App.Instance.Theme.TitleFontColor, null, MenuClicked);
                PageTitle.AddButton("", App.Instance.ImageManger.ImageList[0], TaskSearch);
                BindData();
            });
        }
    }

    void ShowComplatedTaskClicked(GameObject g)
    {
        BindData();
    }

    void MenuClicked(GameObject g)
    {
        App.Instance.DetailPageBox.Show("Page_Navigation", null, null);
    }

    void OnTaskClicked(GameObject g)
    {
        int taskid = int.Parse(g.name.Split('_')[1]);
        App.Instance.PageGroup.ShowPage("Page_Task_Info", false, taskid);
    }

    void OnQuickTaskAdd(GameObject g)
    {
        if (string.IsNullOrEmpty(IptQuickTaskContent.text))
            return;
        ETask p = new ETask();
        p.Title = IptQuickTaskContent.text;
        p.ProjectID = UserSearch == null ? 0 : UserSearch.ProjectID;
        p.PlanID = UserSearch == null ? 0 : UserSearch.PlanID;
        p.ExecutorID = Session.UserID;
        p.TaskHeadID = Session.UserID;
        p.CreaterID = Session.UserID;
        p.CreateTime = System.DateTime.Now;
        p.State = 1;
        BaseOperation_Service.Create<ETask>(p, t =>
        {
            BindData();
            IptQuickTaskContent.text = "";
            App.Instance.HintBox.Show("操作成功！");
        });
    }

    void OnQuickTaskProcess(GameObject g)
    {
        int taskid = int.Parse(g.transform.parent.gameObject.name.Split('_')[1]);
        Task_Service.SetComplated(taskid, t =>
            {
                BindData();
                App.Instance.HintBox.Show("操作成功！");
                App.Instance.DialogBox.Hide();
            });
    }

    void TaskSearch(GameObject g)
    {
        App.Instance.PageGroup.ShowPage("Page_Task_Search", false);
    }

    private void BindData()
    {
        int taskState = 0;
        if (UserSearch != null && UserSearch.TaskState != 0)
            taskState = UserSearch.TaskState;
        TaskPriority? taskPriority = null;
        if (UserSearch != null && UserSearch.TaskPriority != 0)
            taskPriority = (TaskPriority)UserSearch.TaskPriority;
        string pp = ((UserSearch == null || UserSearch.ProjectID == 0) ? "全部项目" : UserSearch.ProjectName) + " > " + ((UserSearch == null || UserSearch.PlanID==0) ? "全部计划" : UserSearch.PlanName);
        if (!string.IsNullOrEmpty(UserSearch.StepName))
            pp += "[" + UserSearch.StepName + "]";
        if (UserSearch.TaskPriority>0)
            pp += "[" + EnumMapping.GetText<TaskPriority>(UserSearch.TaskPriority) + "]";

        txtPosition.text = pp;

        Task_Service.ListMyHomeTask(UserSearch == null ? 0 : UserSearch.ProjectID, UserSearch == null ? 0 : UserSearch.PlanID, taskState, taskPriority, t =>
        {
            List<ETask> plist = t.GetData() as List<ETask>;
            if (!ShowComplatedTask.isOn)
                plist=plist.Where(c=>!c.IsComplated).ToList();
            plist = plist.OrderBy(c => c.IsComplated).ToList();
            PlanListView.BindData<ETask>("MainTaskItem", plist, (i, e) =>
            {
                i.name = "TaskItem_" + e.ID.ToString();
                i.transform.Find("TIState").GetComponent<Text>().text = e.StepName;
                //i.transform.Find("TIPriority").GetComponent<Text>().text = EnumMapping.GetText<TaskPriority>((int)e.Priority);
                //if (e.Priority == TaskPriority.First)
                //    i.transform.Find("TIPriority").GetComponent<Text>().color = new Color(156 / 255f, 93 / 255f, 0 / 255f);
                //else if (e.Priority == TaskPriority.Hurry)
                //    i.transform.Find("TIPriority").GetComponent<Text>().color = new Color(223 / 255f, 60 / 255f, 0 / 255f);
                //else if (e.Priority == TaskPriority.Urgent)
                //    i.transform.Find("TIPriority").GetComponent<Text>().color = Color.red;
                //else
                //    i.transform.Find("TIPriority").GetComponent<Text>().color = new Color(139 / 255f, 139 / 255f, 139 / 255f);
                if (e.Priority == TaskPriority.First)
                    i.transform.Find("PP").GetComponent<Image>().color = new Color(156 / 255f, 93 / 255f, 0 / 255f);
                else if (e.Priority == TaskPriority.Hurry)
                    i.transform.Find("PP").GetComponent<Image>().color = new Color(223 / 255f, 60 / 255f, 0 / 255f);
                else if (e.Priority == TaskPriority.Urgent)
                    i.transform.Find("PP").GetComponent<Image>().color = Color.red;
                else
                    i.transform.Find("PP").GetComponent<Image>().color = new Color(139 / 255f, 139 / 255f, 139 / 255f);

                i.transform.Find("TIContent").GetComponent<Text>().text = e.Title;

                i.transform.Find("TIBeginTime").GetComponent<Text>().text = e.BeginTime.ToString("yyyy-MM-dd");
                i.transform.Find("TIEndTime").GetComponent<Text>().text = e.EndTime.ToString("yyyy-MM-dd");
                if (e.ExecutorID == 0)
                {
                    i.transform.Find("TIPerson").GetComponent<Text>().text = "";
                }
                else
                {
                    i.transform.Find("TIPerson").GetComponent<Text>().text = e.TaskHeadName;
                }
                i.transform.Find("TIBtnDo").gameObject.SetActive(true);
                if (e.IsComplated)
                    i.transform.Find("TIBtnDo").GetComponent<Toggle>().isOn = true;
                else
                    i.transform.Find("TIBtnDo").GetComponent<Toggle>().isOn = false;
                EventListener.Get(i.transform.Find("TIBtnDo").gameObject).onClick = OnQuickTaskProcess;
                if (!e.IsComplated)
                {
                    i.transform.Find("ToUser").gameObject.SetActive(true);
                    EventListener.Get(i.transform.Find("ToUser").gameObject).onClick = OnGotoOther;
                }
                else
                    i.transform.Find("ToUser").gameObject.SetActive(false);
            });
        });
    }

    void OnGotoOther(GameObject g)
    {
        int taskid = int.Parse(g.transform.parent.gameObject.name.Split('_')[1]);
        App.Instance.PageGroup.ShowPage("Page_Task_Handle", false, taskid);
    }
}
