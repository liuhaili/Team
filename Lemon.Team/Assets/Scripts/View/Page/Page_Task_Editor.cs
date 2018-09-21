using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Lemon.Team.Entity;

using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using Lemon.Team.Entity.Enum;
using Assets.Scripts.Services;

public class Page_Task_Editor : NavigatePage
{
    public Dropdown TaskState;
    public Dropdown TaskPriority;
    public Dropdown RemindTime;
    public InputField TaskName;
    public InputField TaskContent;
    public DatePickerSelectButton TaskBeginTime;
    public DatePickerSelectButton TaskEndTime;
    public Button AddFile;

    public Button AddHeadPerson;

    public Button AddExcuter;

    public ListViewControl AttachmentList;

    private int ExcuterID;
    private int HeadPersonID;
    PageInfoMode PageInfoMode;
    ETask TheTask;
    int PlanID;
    int ProjectID;
    public string SelectFilePath;
    private List<EProjectTaskStep> ProjectStepList;

    protected override void Init()
    {
        base.Init();
        PageTitle.Init("任务详情", App.Instance.Theme.TitleBgColor, App.Instance.Theme.TitleFontColor, BtnBack);
        EventListener.Get(AddHeadPerson.gameObject).onClick = OnAddHeadPersonClicked;
        EventListener.Get(AddExcuter.gameObject).onClick = OnAddExcuterClicked;
        EventListener.Get(AddFile.gameObject).onClick = OnAddFileClicked;
        PageInfoMode = GetPar<PageInfoMode>(0);
        int taskId = GetPar<int>(1);
        PlanID = GetPar<int>(2);
        ProjectID = GetPar<int>(3);
        PlatformCallBackListener.Instance.OnUploadComplated = OnUploadComplated;
        AttachmentList.OnItemClicked = OnAttachmentItemClicked;

        ProjectTaskStep_Service.ListByProjectID(ProjectID, tt =>
        {
            ProjectStepList = tt.GetData() as List<EProjectTaskStep>;

            InitUI();
            if (PageInfoMode == PageInfoMode.Add)
            {
                PageTitle.AddButton("", App.Instance.ImageManger.ImageList[5], OnAddTask);
                BindInfo(new ETask() { State = 1 });
            }
            else if (PageInfoMode == PageInfoMode.Editor)
            {
                Task_Service.Get(taskId, t =>
                {
                    TheTask = t.GetData() as ETask;
                    BindInfo(TheTask);

                    if (TheTask.CreaterID == Session.UserID)
                        PageTitle.AddButton("", App.Instance.ImageManger.ImageList[2], OnDeleteTask);
                    PageTitle.AddButton("", App.Instance.ImageManger.ImageList[5], OnSaveTask);
                });
            }
        });
    }

    void BtnBack(GameObject g)
    {
        App.Instance.PageGroup.ClosePage();
    }

    void OnAddHeadPersonClicked(GameObject g)
    {
        ProjectTeam_Service.ListByProjectID(ProjectID, t =>
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
                    HeadPersonID = int.Parse(uid);
                    BindHeadName(HeadPersonID, dpg.SelectedData2);
                }
            }, null, userList);
        });
    }

    void OnAddExcuterClicked(GameObject g)
    {
        ProjectTeam_Service.ListByProjectID(ProjectID, t =>
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
                    ExcuterID = int.Parse(uid);

                    BindExcuterName(ExcuterID, dpg.SelectedData2);
                }
            }, null, userList);
        });
    }

    void OnAddTask(GameObject g)
    {
        if (string.IsNullOrEmpty(TaskName.text))
            return;
        ETask p = new ETask();
        p.Title = TaskName.text;
        p.Content = TaskContent.text;
        p.ProjectID = ProjectID;
        p.PlanID = PlanID;
        p.CreaterID = Session.UserID;
        p.CreateTime = System.DateTime.Now;
        p.BeginTime = TaskBeginTime.GetTime();
        p.EndTime = TaskEndTime.GetTime();
        p.State = ProjectStepList.FirstOrDefault(c => c.Name == TaskState.captionText.text).Value;
        p.Priority = (TaskPriority)EnumMapping.GetValue<TaskPriority>(TaskPriority.captionText.text);
        p.TaskHeadID = HeadPersonID;
        p.ExecutorID = ExcuterID;
        p.Attachment = SelectFilePath;
        BaseOperation_Service.Create<ETask>(p, t =>
        {
            App.Instance.HintBox.Show("操作成功！");
            App.Instance.PageGroup.ClosePage();
        });
    }

    void OnSaveTask(GameObject g)
    {
        if (string.IsNullOrEmpty(TaskName.text))
            return;
        TheTask.Title = TaskName.text;
        TheTask.Content = TaskContent.text;
        TheTask.ProjectID = ProjectID;
        TheTask.PlanID = PlanID;
        TheTask.CreaterID = Session.UserID;
        TheTask.CreateTime = System.DateTime.Now;
        TheTask.BeginTime = TaskBeginTime.GetTime();
        TheTask.EndTime = TaskEndTime.GetTime();
        TheTask.State = ProjectStepList.FirstOrDefault(c => c.Name == TaskState.captionText.text).Value;
        TheTask.Priority = (TaskPriority)EnumMapping.GetValue<TaskPriority>(TaskPriority.captionText.text);
        TheTask.TaskHeadID = HeadPersonID;
        TheTask.ExecutorID = ExcuterID;
        string fz = "";
        switch (RemindTime.captionText.text)
        {
            case "不提醒":
                fz = "";
                break;
            case "早上9点":
                fz = "9";
                break;
            case "中午12点":
                fz = "12";
                break;
            case "下午3点":
                fz = "15";
                break;
            case "下午6点":
                fz = "18";
                break;
            case "晚上9点":
                fz = "21";
                break;
        }
        TheTask.Remind = fz;
        TheTask.IsReminded = false;
        BaseOperation_Service.Change<ETask>(TheTask, t =>
        {
            App.Instance.HintBox.Show("操作成功！");
            App.Instance.PageGroup.ClosePage();

        });
    }

    void OnDeleteTask(GameObject g)
    {
        App.Instance.DialogBox.Show("提示信息", "", "你确定要删除该任务吗？", 300, 150, c =>
        {
            BaseOperation_Service.Delete<ETask>(TheTask, t =>
            {
                App.Instance.PageGroup.ClosePage();
                App.Instance.DialogBox.Hide();
                App.Instance.HintBox.Show("操作成功！");
            });
        }, null);
    }

    void BindHeadName(int headId, string name)
    {
        if (headId == 0)
        {
            AddHeadPerson.GetComponentInChildren<Text>().text = "";
            return;
        }
        AddHeadPerson.GetComponentInChildren<Text>().text = name;
    }

    void BindExcuterName(int excuterId, string name)
    {
        if (excuterId == 0)
        {
            AddExcuter.GetComponentInChildren<Text>().text = "";
            return;
        }
        AddExcuter.GetComponentInChildren<Text>().text = name;
    }

    void BindAttachment()
    {
        if (string.IsNullOrEmpty(SelectFilePath))
        {
            AttachmentList.Clear();
            return;
        }
        AttachmentList.BindData<string>("AttachmentItem", SelectFilePath.Split('|').ToList(), (i, e) =>
        {
            string path = e;
            i.name = path;
            i.transform.Find("Name").GetComponent<Text>().text = System.IO.Path.GetFileName(path);
            i.transform.Find("Button").gameObject.SetActive(true);
            EventListener.Get(i.transform.Find("Button").gameObject).onClick = OnDelAttachmentClicked;
        });
    }

    void BindInfo(ETask t)
    {
        if (ProjectStepList.Count > 0)
        {
            EProjectTaskStep step = ProjectStepList.FirstOrDefault(c => c.Value == t.State);
            if (step != null)
            {
                int index = ProjectStepList.IndexOf(step);
                TaskState.captionText.text = step.Name;
                TaskState.value = index;
            }
        }

        TaskPriority.captionText.text = EnumMapping.GetText<TaskPriority>((int)t.Priority);

        TaskPriority.value = (int)t.Priority - 1;

        TaskName.text = t.Title;
        TaskContent.text = t.Content;
        TaskBeginTime.SetTime(t.BeginTime);
        TaskEndTime.SetTime(t.EndTime);
        HeadPersonID = t.TaskHeadID;
        ExcuterID = t.ExecutorID;
        SelectFilePath = t.Attachment;

        BindHeadName(HeadPersonID, t.TaskHeadName);
        BindExcuterName(ExcuterID, t.ExecutorName);
        BindAttachment();
        BindRemindTime(t.Remind);
    }

    void InitUI()
    {
        TaskState.options.Clear();
        foreach (var item in ProjectStepList)
        {
            TaskState.options.Add(new Dropdown.OptionData(item.Name));
        }

        TaskPriority.options.Clear();
        foreach (string item in EnumMapping.ListAll<TaskPriority>())
        {
            TaskPriority.options.Add(new Dropdown.OptionData(item));
        }
    }

    void OnAddFileClicked(GameObject g)
    {
        PlatformDifferences.OpenFile();
    }

    void OnUploadComplated(string filename, byte[] filedata)
    {
        App.Instance.UploadFile<string>(t =>
        {
            App.Instance.HintBox.Show("文件上传成功！");
            SelectFilePath += "|" + t.GetData().ToString();
            SelectFilePath = SelectFilePath.TrimStart('|');

            if (TheTask != null)
            {
                TheTask.Attachment = SelectFilePath;
                BaseOperation_Service.Change<ETask>(TheTask, ct =>
                {
                    App.Instance.HintBox.Show("修改成功！");
                    BindAttachment();
                });
            }
            else
                BindAttachment();
        }, "UploadFiles/TaskAttach", filename, filedata);
    }

    void OnAttachmentItemClicked(GameObject g)
    {
        Application.OpenURL(ServiceManager.ServiceUrl + g.name);
    }

    void OnDelAttachmentClicked(GameObject g)
    {
        string fpath = g.transform.parent.name;
        string newAttachment = "";
        foreach (var a in SelectFilePath.Split('|'))
        {
            if (a != fpath)
                newAttachment += "|" + a;
        }
        SelectFilePath = newAttachment.TrimStart('|');
        if (TheTask != null)
        {
            TheTask.Attachment = SelectFilePath;
            BaseOperation_Service.Change<ETask>(TheTask, t =>
            {
                App.Instance.HintBox.Show("操作成功！");
                BindAttachment();
            });
        }
        else
            BindAttachment();
    }

    void BindRemindTime(string fzStr)
    {
        if (string.IsNullOrEmpty(fzStr))
        {
            RemindTime.captionText.text = "不提醒";
            RemindTime.value = 0;
            return;
        }
        int fz = int.Parse(fzStr);
        switch (fz)
        {
            case 9:
                RemindTime.captionText.text = "早上9点";
                RemindTime.value = 1;
                break;
            case 12:
                RemindTime.captionText.text = "中午12点";
                RemindTime.value = 2;
                break;
            case 15:
                RemindTime.captionText.text = "下午3点";
                RemindTime.value = 3;
                break;
            case 18:
                RemindTime.captionText.text = "下午6点";
                RemindTime.value = 4;
                break;
            case 21:
                RemindTime.captionText.text = "晚上9点";
                RemindTime.value = 5;
                break;
        }
    }
}
