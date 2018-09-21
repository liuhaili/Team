using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Lemon.Team.Entity;

using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using Assets.Scripts.Services;

public class Page_People_Info : NavigatePage
{
    public RawImage ImgFace;
    public Text TbName;
    public Text TbRole;

    private EPeople EPeople;
    private EProjectTeam ETeamOne;

    private bool IsPeopleOrTeam = true;
    protected override void Init()
    {
        base.Init();
        PageTitle.Init("成员信息", App.Instance.Theme.TitleBgColor, App.Instance.Theme.TitleFontColor, BtnBack);
        PageTitle.AddButton("", App.Instance.ImageManger.ImageList[2], DeletePeople);

        int userid = this.GetPar<int>(0);
        IsPeopleOrTeam = this.GetPar<bool>(1);
        int projectID = this.GetPar<int>(2);
        if (IsPeopleOrTeam)
        {
            People_Service.GetMyOnePeople(userid, t =>
            {
                EPeople = t.GetData() as EPeople;
                BindData();
            });
        }
        else
        {
            ProjectTeam_Service.GetByProjectID(userid, projectID, t =>
            {
                ETeamOne = t.GetData() as EProjectTeam;
                BindData();
            });
        }
    }

    void BindData()
    {
        if (IsPeopleOrTeam)
        {
            TbName.text = EPeople.PeopleName;
            App.Instance.ShowImage(ImgFace, EPeople.PeopleFace, 11);
        }
        else
        {
            TbName.text = ETeamOne.UserName;
            App.Instance.ShowImage(ImgFace, ETeamOne.UserFace, 11);
        }
    }

    void BtnBack(GameObject g)
    {
        App.Instance.PageGroup.ClosePage();
    }

    void DeletePeople(GameObject g)
    {
        App.Instance.DialogBox.Show("提示信息", "", "你确定要删除该成员吗？", 300, 150, c =>
        {
            if (IsPeopleOrTeam)
            {
                People_Service.DisConnect(EPeople.PeopleID, t =>
                {
                    App.Instance.PageGroup.ClosePage();
                    App.Instance.DialogBox.Hide();
                    App.Instance.HintBox.Show("联系移除成功！");
                });
            }
            else
            {
                BaseOperation_Service.Delete<EProjectTeam>(ETeamOne, t =>
                {
                    App.Instance.PageGroup.ClosePage();
                    App.Instance.DialogBox.Hide();
                    App.Instance.HintBox.Show("操作成功！");
                });
            }

        }, null);
    }
}
