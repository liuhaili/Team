using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Lemon.Team.Entity;

using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using Lemon.Team.Entity.Enum;
using Assets.Scripts.Services;

public class Page_Task_Info : NavigatePage
{
    public Text TaskState;
    public Text TaskPriority;
    public Text TaskName;
    public Text TaskContent;
    public Text TaskBeginTime;
    public Text TaskEndTime;
    public Text RemindTime;
    public Text TaskHeadPersonName;
    public Text ExcuterName;
    public ListViewControl AttachmentList;

    ETask TheTask;

    protected override void Init()
    {
        base.Init();

        PageTitle.Init("任务详情", App.Instance.Theme.TitleBgColor, App.Instance.Theme.TitleFontColor, BtnBack);
        int taskId = GetPar<int>(0);
        Task_Service.Get(taskId, t =>
        {
            TheTask = t.GetData() as ETask;
            PageTitle.AddButton("", App.Instance.ImageManger.ImageList[7], GotoTaskTransfer);
            if (TheTask.CreaterID == Session.UserID && !TheTask.IsComplated)
                PageTitle.AddButton("", App.Instance.ImageManger.ImageList[13], GotoTaskEditor);
            BindInfo();
        });
    }

    void GotoTaskTransfer(GameObject g)
    {
        App.Instance.PageGroup.ShowPage("Page_Task_Transfer", false, TheTask.ID);
    }

    void GotoTaskEditor(GameObject g)
    {
        App.Instance.PageGroup.ReplacePage("Page_Task_Editor", PageInfoMode.Editor, TheTask.ID, TheTask.PlanID, TheTask.ProjectID);
    }

    void BtnBack(GameObject g)
    {
        App.Instance.PageGroup.ClosePage();
    }

    void BindInfo()
    {
        ETask t = TheTask;
        TaskState.text = t.StepName;
        TaskPriority.text = EnumMapping.GetText<TaskPriority>((int)t.Priority);
        TaskName.text = t.Title;
        TaskContent.text = t.Content;
        TaskBeginTime.text = t.BeginTime.ToString("yyyy-MM-dd hh:mm:ss");
        TaskEndTime.text = t.EndTime.ToString("yyyy-MM-dd hh:mm:ss");
        BindHeadName(t.TaskHeadID);
        BindExcuterName(t.ExecutorID);
        BindAttachment(t.Attachment);
        BindRemindTime(t.Remind);
    }

    void BindHeadName(int headId)
    {
        if (headId == 0)
        {
            TaskHeadPersonName.text = "";
            return;
        }

        if (TheTask == null)
            TaskHeadPersonName.text = "";
        else
            TaskHeadPersonName.text = TheTask.TaskHeadName;
    }

    void BindExcuterName(int excuterId)
    {
        if (excuterId == 0)
        {
            ExcuterName.text = "";
            return;
        }
        if (TheTask == null)
            ExcuterName.text = "";
        else
            ExcuterName.text = TheTask.ExecutorName;
    }

    void BindAttachment(string attachment)
    {
        if (string.IsNullOrEmpty(attachment))
        {
            AttachmentList.Clear();
            return;
        }
        AttachmentList.OnItemClicked = OnAttachmentItemClicked;
        AttachmentList.BindData<string>("AttachmentItem", attachment.Split('|').ToList(), (i, e) =>
        {
            string path = e;
            i.name = path;
            i.transform.Find("Name").GetComponent<Text>().text = System.IO.Path.GetFileName(path);
            i.transform.Find("Button").gameObject.SetActive(false);
        });
    }

    void OnAttachmentItemClicked(GameObject g)
    {
        Application.OpenURL(ServiceManager.ServiceUrl + g.name);
    }

    void BindRemindTime(string fzStr)
    {
        if (string.IsNullOrEmpty(fzStr))
        {
            RemindTime.text = "不提醒";
            return;
        }
        int fz = int.Parse(fzStr);
        switch (fz)
        {
            case 9:
                RemindTime.text = "早上9点";
                break;
            case 12:
                RemindTime.text = "中午12点";
                break;
            case 15:
                RemindTime.text = "下午3点";
                break;
            case 18:
                RemindTime.text = "下午6点";
                break;
            case 21:
                RemindTime.text = "晚上9点";
                break;
        }
    }
}
