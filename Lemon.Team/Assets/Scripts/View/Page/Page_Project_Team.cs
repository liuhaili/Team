using UnityEngine;
using System.Collections;
using Lemon.Team.Entity;
using System.Collections.Generic;
using UnityEngine.UI;

using Lemon.Team.Entity.Enum;
using System.Linq;
using Assets.Scripts;
using Assets.Scripts.Services;

public class Page_Project_Team : NavigatePage
{
    public ListViewControl ProjectList;
    public Button QQYQ;

    private EProject EProject;
    protected override void Init()
    {
        base.Init();
        EventListener.Get(QQYQ.gameObject).onClick = onQQYQ;
        BaseOperation_Service.Get<EProject>(GetPar<int>(0), t =>
        {
            EProject = t.GetData() as EProject;
            PageTitle.Init("项目团队", App.Instance.Theme.TitleBgColor, App.Instance.Theme.TitleFontColor, BtnBack);
            PageTitle.AddButton("", App.Instance.ImageManger.ImageList[1], OnAddClicked);
            ProjectList.OnItemClicked = OnItemClicked;
            BindData();
        });
    }

    void BtnBack(GameObject g)
    {
        App.Instance.PageGroup.ClosePage();
    }

    void onQQYQ(GameObject g)
    {
        PlatformDifferences.QQShare(Session.CurrentUser.Name + "邀请你加入团队", "", "http://shouji.baidu.com/software/10526673.html", "http://47.90.21.98:8080/Images/lemon.png");
    }

    private void BindData()
    {
        ProjectTeam_Service.ListByProjectID(EProject.ID, t =>
        {
            List<EProjectTeam> plist = t.GetData() as List<EProjectTeam>;
            ProjectList.OnItemClicked = OnPeopleClicked;
            ProjectList.BindData<EProjectTeam>("PeopleItem", plist, (i, e) =>
            {
                i.name = "PeopleItem_" + e.UserID;
                i.transform.Find("Name").GetComponent<Text>().text = e.UserName;
                i.transform.Find("Phone").GetComponent<Text>().text = "";// user.Phone;
                i.transform.Find("tgbtn").gameObject.SetActive(false);
                i.transform.Find("jjbtn").gameObject.SetActive(false);
                App.Instance.ShowImage(i.transform.Find("Icon").GetComponent<RawImage>(), e.UserFace, 11);
            });
        });
    }

    void OnPeopleClicked(GameObject g)
    {
        int id = int.Parse(g.name.Split('_')[1]);
        App.Instance.PageGroup.ShowPage("Page_People_Info", false, id, false, EProject.ID);
    }

    void OnItemClicked(GameObject g)
    {
        App.Instance.PageGroup.ShowPage("Page_Project_Plan", false, int.Parse(g.name.Split('_')[1]));
    }

    void OnAddClicked(GameObject g)
    {
        People_Service.ListMyPeople(p =>
        {
            List<EKeyName> userList = new List<EKeyName>();
            List<EPeople> plist = p.GetData() as List<EPeople>;
            plist = plist.Where(c => c.State == PeopleState.Normal).ToList();
            plist.ForEach(u =>
            {
                userList.Add(new EKeyName() { ID = u.PeopleID, Name = u.PeopleName, Other = u.PeopleFace });
            });
            EUser myself = Session.CurrentUser;
            userList.Add(new EKeyName() { ID = myself.ID, Name = myself.Name, Other = myself.Face });
            App.Instance.DialogBox.Show("选择成员", "Page_UserSelected", "", 300, 500, gg =>
            {
                DialogPage dpg = App.Instance.DialogBox.ContentPage.GetComponent<DialogPage>();
                string uid = dpg.SelectedData;
                if (!string.IsNullOrEmpty(uid))
                {
                    ProjectTeam_Service.ListByProjectID(EProject.ID, tm =>
                    {
                        List<EProjectTeam> teamList = tm.GetData() as List<EProjectTeam>;
                        if (!teamList.Any(c => c.UserID == int.Parse(uid)))
                        {
                            EProjectTeam teamItem = new EProjectTeam();
                            teamItem.ProjectID = EProject.ID;
                            teamItem.UserID = int.Parse(uid);
                            BaseOperation_Service.Create<EProjectTeam>(teamItem, t =>
                            {
                                BindData();
                            });
                        }
                    });
                }
            }, null, userList);
        });
    }
}
