using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Lemon.Team.Entity;

using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using Lemon.Team.Entity.Enum;
using Assets.Scripts.Services;

public class Page_Task_Handle : NavigatePage
{
    public Button ChooseHeadPerson;
    public Text TaskExcuterName;
    public InputField HandleContent;
    public Dropdown TaskState;

    int ExcuterId;
    ETask TheTask;
    private List<EProjectTaskStep> ProjectStepList;

    protected override void Init()
    {
        base.Init();
        PageTitle.Init("处理任务", App.Instance.Theme.TitleBgColor, App.Instance.Theme.TitleFontColor, BtnBack);
        EventListener.Get(ChooseHeadPerson.gameObject).onClick = OnChooseHeadPersonClicked;
        int taskId = GetPar<int>(0);
        Task_Service.Get(taskId, t =>
        {
            TheTask = t.GetData() as ETask;
            ProjectTaskStep_Service.ListByProjectID(TheTask.ProjectID, tt =>
            {
                ProjectStepList = tt.GetData() as List<EProjectTaskStep>;
                BindInfo();
                PageTitle.AddButton("", App.Instance.ImageManger.ImageList[5], OnHandleTask);
            });
        });
    }

    void BtnBack(GameObject g)
    {
        App.Instance.PageGroup.ClosePage();
    }

    void OnChooseHeadPersonClicked(GameObject g)
    {
        ProjectTeam_Service.ListByProjectID(TheTask.ProjectID, t =>
        {
            List<EKeyName> userList = new List<EKeyName>();
            List<EProjectTeam> projectTeam = t.GetData() as List<EProjectTeam>;
            projectTeam.ForEach(u =>
            {
                userList.Add(new EKeyName() { ID = u.UserID, Name = u.UserName, Other = u.UserFace });
            });

            App.Instance.DialogBox.Show("选择成员", "Page_UserSelected", "", 300, 500, gg =>
            {
                DialogPage dpg = App.Instance.DialogBox.ContentPage.GetComponent<DialogPage>();
                string uid = dpg.SelectedData;
                if (!string.IsNullOrEmpty(uid))
                {
                    ExcuterId = int.Parse(uid);
                    TaskExcuterName.text = TheTask.ExecutorName;
                }
            }, null, userList);
        });
    }

    void OnHandleTask(GameObject g)
    {
        EProjectTaskStep step = ProjectStepList.FirstOrDefault(c => c.Name == TaskState.captionText.text);
        Task_Service.TaskProcess(TheTask.ID, ExcuterId, step.Value, HandleContent.text, t =>
        {
            App.Instance.HintBox.Show("操作成功！");
            App.Instance.PageGroup.ClosePage();
        });
    }

    void BindInfo()
    {
        TaskState.options.Clear();
        foreach (var item in ProjectStepList)
        {
            TaskState.options.Add(new Dropdown.OptionData(item.Name));
        }

        EProjectTaskStep step = ProjectStepList.FirstOrDefault(c => c.Value == TheTask.State);
        if (step != null)
        {
            int index = ProjectStepList.IndexOf(step);
            TaskState.captionText.text = step.Name;
            TaskState.value = index;
        }

        ExcuterId = TheTask.ExecutorID;
        TaskExcuterName.text = TheTask.ExecutorName;
    }
}
