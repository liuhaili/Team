using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Lemon.Team.Entity;

using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Services;

public class Page_Project_Info : NavigatePage
{
    public InputField IptName;
    public InputField Project_Process;
    public RawImage ImgFace;
    public Button BtnUploadFace;
    public ListViewControl TaskStepList;
    public Button BtnAddStep;
    private EProject EProject;

    private int MaxStepValue = 0;
    private List<EProjectTaskStep> StepList;
    protected override void Init()
    {
        base.Init();
        PageTitle.Init("项目详情", App.Instance.Theme.TitleBgColor, App.Instance.Theme.TitleFontColor, BtnBack);
        PageTitle.AddButton("", App.Instance.ImageManger.ImageList[5], SaveProject);

        EventListener.Get(BtnUploadFace.gameObject).onClick = UploadProjectFace;
        EventListener.Get(BtnAddStep.gameObject).onClick = AddTaskStep;
        BaseOperation_Service.Get<EProject>(GetPar<int>(0), t =>
        {
            EProject = t.GetData() as EProject;
            IptName.text = EProject.Name;
            Project_Process.text = EProject.Progress.ToString();
            App.Instance.ShowImage(ImgFace, EProject.Face, 12);
            BindTaskStep();
        });

        PlatformCallBackListener.Instance.OnUploadComplated = OnUploadComplated;
    }

    void BtnBack(GameObject g)
    {
        App.Instance.PageGroup.ClosePage();
    }

    void AddTaskStep(GameObject g)
    {
        EProjectTaskStep step = new EProjectTaskStep()
        {
            ProjectID = EProject.ID,
            Name = "新阶段",
            Value = MaxStepValue + 1
        };
        BaseOperation_Service.Create<EProjectTaskStep>(step, t =>
        {
            App.Instance.HintBox.Show("添加成功！");
            BindTaskStep();
        });
    }

    void SaveProject(GameObject g)
    {
        EProject.Name = IptName.text;
        EProject.Progress = int.Parse(Project_Process.text);
        BaseOperation_Service.Change<EProject>(EProject, t =>
        {
            App.Instance.HintBox.Show("修改成功！");
            App.Instance.PageGroup.ClosePage();
        });
    }

    void UploadProjectFace(GameObject g)
    {
        PlatformDifferences.OpenPhoto(true, 200, 200);
    }

    void OnUploadComplated(string filename, byte[] filedata)
    {
        App.Instance.UploadFile<string>(t =>
        {
            App.Instance.HintBox.Show("文件上传成功！");
            string path = t.GetData().ToString();
            App.Instance.ShowImage(ImgFace, path, 11);

            EProject.Face = path;
            BaseOperation_Service.Change<EProject>(EProject, ct =>
            {
                App.Instance.HintBox.Show("修改成功！");
            });
        }, "UploadFiles/ProjectFace", EProject.ID + ".png", filedata);
    }

    void BindTaskStep()
    {
        BtnAddStep.interactable = false;
        ProjectTaskStep_Service.ListByProjectID(EProject.ID, t =>
        {
            BtnAddStep.interactable = true;
            StepList = t.GetData() as List<EProjectTaskStep>;
            if (StepList.Count > 0)
                MaxStepValue = StepList.Max(c => c.Value);
            TaskStepList.BindData<EProjectTaskStep>("ProjectTaskStepItem", StepList, (i, e) =>
            {
                i.name = "ProjectTaskStepItem_" + e.ID;
                i.transform.Find("Name").GetComponent<InputField>().text = e.Name;
                EventListener.Get(i.transform.Find("BtnDel").gameObject).onClick = DelTaskStep;
                EventListener.Get(i.transform.Find("BtnSave").gameObject).onClick = ChangeTaskStep;
            });
        });
    }

    void DelTaskStep(GameObject g)
    {
        int stepid = int.Parse(g.transform.parent.gameObject.name.Split('_')[1]);
        BaseOperation_Service.Delete<EProjectTaskStep>(stepid, t =>
        {
            App.Instance.HintBox.Show("删除成功！");
            BindTaskStep();
        });
    }

    void ChangeTaskStep(GameObject g)
    {
        int stepid = int.Parse(g.transform.parent.gameObject.name.Split('_')[1]);
        EProjectTaskStep stepObj=StepList.FirstOrDefault(c => c.ID == stepid);
        stepObj.Name = g.transform.parent.Find("Name").GetComponent<InputField>().text;
        BaseOperation_Service.Change<EProjectTaskStep>(stepObj, t =>
        {
            App.Instance.HintBox.Show("修改成功！");
            BindTaskStep();
        });
    }
}
