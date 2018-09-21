using UnityEngine;
using System.Collections;
using Lemon.Team.Entity;

using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using Assets.Scripts.Services;
using Lemon.Team.Entity.Enum;
using Assets.Scripts;

public class Page_Task_SearchCreate : NavigatePage
{
    public ListViewControl ProjectList;
    public ListViewControl PlanList;
    public ListViewControl StateList;
    public ListViewControl YXJList;

    public Button PreBtn;
    public Button NextBtn;
    public Button SubmitBtn;

    private int Step = 1;//1,2,3,4
    private EUserSearch NewSearch;
    private List<EProjectTaskStep> ProjectStepList;
    protected override void Init()
    {
        base.Init();
        Step = 1;
        NewSearch = new EUserSearch() { UserID = Session.UserID };
        PageTitle.Init("创建查询", Color.gray, App.Instance.Theme.TitleFontColor, BtnBack);
        EventListener.Get(PreBtn.gameObject).onClick = OnPreBtnClicked;
        EventListener.Get(NextBtn.gameObject).onClick = OnNextBtnClicked;
        EventListener.Get(SubmitBtn.gameObject).onClick = OnSubmitBtnClicked;
        BindStep();
    }

    void OnPreBtnClicked(GameObject g)
    {
        if (Step <= 1)
            return;
        Step--;
        BindStep();
    }

    void OnNextBtnClicked(GameObject g)
    {
        if (Step >= 4)
            return;
        Step++;
        BindStep();
    }

    void OnSubmitBtnClicked(GameObject g)
    {
        App.Instance.DialogBox.ShowImportBox("创建查询", 350, 200, s =>
        {
            NewSearch.Name = App.Instance.DialogBox.Field.text;
            BaseOperation_Service.Create<EUserSearch>(NewSearch, t =>
            {
                App.Instance.PageGroup.ShowPage("Page_Task", true, NewSearch);
            });
        }, c =>
        {
            App.Instance.PageGroup.ShowPage("Page_Task", true, NewSearch);
        });
    }

    void BtnBack(GameObject g)
    {
        App.Instance.PageGroup.ClosePage();
    }

    void BindStep()
    {
        switch (Step)
        {
            case 1:
                BindProject();
                ProjectList.gameObject.SetActive(true);
                PlanList.gameObject.SetActive(false);
                StateList.gameObject.SetActive(false);
                YXJList.gameObject.SetActive(false);

                PreBtn.interactable = false;
                NextBtn.interactable = true;
                SubmitBtn.interactable = false;
                break;
            case 2:
                BindPlan();
                ProjectList.gameObject.SetActive(false);
                PlanList.gameObject.SetActive(true);
                StateList.gameObject.SetActive(false);
                YXJList.gameObject.SetActive(false);

                PreBtn.interactable = true;
                NextBtn.interactable = true;
                SubmitBtn.interactable = false;
                break;
            case 3:
                BindTaskState();
                ProjectList.gameObject.SetActive(false);
                PlanList.gameObject.SetActive(false);
                StateList.gameObject.SetActive(true);
                YXJList.gameObject.SetActive(false);

                PreBtn.interactable = true;
                NextBtn.interactable = true;
                SubmitBtn.interactable = false;
                break;
            case 4:
                BindTaskPriority();
                ProjectList.gameObject.SetActive(false);
                PlanList.gameObject.SetActive(false);
                StateList.gameObject.SetActive(false);
                YXJList.gameObject.SetActive(true);

                PreBtn.interactable = true;
                NextBtn.interactable = false;
                SubmitBtn.interactable = false;
                break;
        }
    }

    private void BindProject()
    {
        ProjectList.Clear();
        Project_Service.ListTeamProject(t =>
        {
            List<EProject> plist = t.GetData() as List<EProject>;
            ProjectList.BindData<EProject>("ProjectItemSelect", plist, (i, e) =>
            {
                i.name = "ProjectItem_" + e.ID.ToString();
                i.transform.Find("TIContent").GetComponent<Text>().text = e.Name;
            }, true, true);
            ProjectList.OnItemClicked = OnProjectClicked;
        });
    }

    private void BindPlan()
    {
        PlanList.Clear();
        Plan_Service.ListMyProjectID(NewSearch.ProjectID, t =>
        {
            PlanList.BindData<EPlan>("PlanItemSelect", t.GetData() as List<EPlan>, (i, e) =>
            {
                i.name = "PlanItem_" + e.ID;
                i.transform.Find("Name").GetComponent<Text>().text = e.Name;
            }, true, true);
            PlanList.OnItemClicked = OnPlanClicked;
        });
    }

    private void BindTaskState()
    {
        ProjectTaskStep_Service.ListByProjectID(NewSearch.ProjectID, t =>
        {
            ProjectStepList = t.GetData() as List<EProjectTaskStep>;
            ProjectStepList.Insert(0, new EProjectTaskStep() { Name = "全部", Value = 0 });
           
            StateList.Clear();
            StateList.OnItemClicked = StateItem_OnClicked;
            StateList.BindData<EProjectTaskStep>("TextItem", ProjectStepList, (i, e) =>
            {
                i.name = "TextItem_" + e.Name;
                i.transform.Find("Name").GetComponent<Text>().text = e.Name;
            }, true, true);
        });
    }

    private void BindTaskPriority()
    {
        List<string> pList = EnumMapping.ListAll<TaskPriority>();
        pList.Insert(0, "全部");
        YXJList.Clear();
        YXJList.OnItemClicked = YXJItem_OnClicked;
        YXJList.BindData<string>("TextItem", pList, (i, e) =>
        {
            i.name = "TextItem_" + e;
            i.transform.Find("Name").GetComponent<Text>().text = e;
        }, true, true);
    }

    void OnProjectClicked(GameObject g)
    {
        NewSearch.ProjectID = int.Parse(g.name.Split('_')[1]);
    }

    void OnPlanClicked(GameObject g)
    {
        NewSearch.PlanID = int.Parse(g.name.Split('_')[1]);
    }

    void StateItem_OnClicked(GameObject g)
    {
        string stateStr = g.name.Split('_')[1];
        if (stateStr == "全部")
            NewSearch.TaskState = 0;
        else
            NewSearch.TaskState = ProjectStepList.FirstOrDefault(c=>c.Name==stateStr).Value;
    }

    void YXJItem_OnClicked(GameObject g)
    {
        SubmitBtn.interactable = true;
        string str = g.name.Split('_')[1];
        if (str == "全部")
            NewSearch.TaskPriority = 0;
        else
            NewSearch.TaskPriority = EnumMapping.GetValue<TaskPriority>(str);
    }
}
