using UnityEngine;
using System.Collections;
using Lemon.Team.Entity;
using System.Collections.Generic;
using UnityEngine.UI;

using Lemon.Team.Entity.Enum;
using System.Linq;
using Assets.Scripts;
using Assets.Scripts.Services;

public class Page_Project : Page_MenuBase
{
    public ListViewControl ProjectList;
    protected override void Init()
    {
        base.Init();
        PageTitle.Init("项目", App.Instance.Theme.TitleBgColor, App.Instance.Theme.TitleFontColor, null, MenuClicked);
        PageTitle.AddButton("", App.Instance.ImageManger.ImageList[1], OnAddClicked);
        ProjectList.OnItemClicked = OnItemClicked;
        BindData();
    }

    void MenuClicked(GameObject g)
    {
        App.Instance.DetailPageBox.Show("Page_Navigation", null, null);
    }

    private void BindData()
    {
        Project_Service.ListMyProject(t =>
        {
            List<EProject> plist = t.GetData() as List<EProject>;
            ProjectList.BindData<EProject>("ProjectItem", plist, (i, e) =>
            {
                i.name = "ProjectItem_" + e.ID.ToString();
                i.transform.Find("TIContent").GetComponent<Text>().text = e.Name;
                i.transform.Find("Image").Find("Process").GetComponent<Image>().fillAmount = e.Progress / 100.0f;
                i.transform.Find("Text").GetComponent<Text>().text = e.Progress.ToString() + "%";

                App.Instance.ShowImage(i.GetComponent<RawImage>(), e.Face,12);
            });
        });
    }

    void OnItemClicked(GameObject g)
    {
        App.Instance.PageGroup.ShowPage("Page_Project_Plan", false, int.Parse(g.name.Split('_')[1]));
    }

    void OnAddClicked(GameObject g)
    {
        App.Instance.DialogBox.Show("添加项目", "Page_AddProject", "", 350, 200, c =>
        {
            EProject p = new EProject();
            p.Name = App.Instance.DialogBox.Content.GetComponentInChildren<InputField>().text;
            p.CreaterID = Session.UserID;

            Project_Service.Create(p, t =>
            {
                BindData();
                App.Instance.DialogBox.Hide();
                App.Instance.HintBox.Show("操作成功！");
            });
        }, null);
    }
}
