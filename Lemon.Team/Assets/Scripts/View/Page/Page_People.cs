using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Lemon.Team.Entity;

using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using Assets.Scripts.Services;

public class Page_People : Page_MenuBase
{
    public ListViewControl ListView;
    protected override void Init()
    {
        base.Init();

        PageTitle.Init("通讯录", App.Instance.Theme.TitleBgColor, App.Instance.Theme.TitleFontColor, null, MenuClicked);
        PageTitle.AddButton("", App.Instance.ImageManger.ImageList[0], SearchPeople);
        ListView.OnItemClicked = OnItemClicked;
        BindData();
    }

    void MenuClicked(GameObject g)
    {
        App.Instance.DetailPageBox.Show("Page_Navigation", null, null);
    }

    void SearchPeople(GameObject g)
    {
        App.Instance.PageGroup.ShowPage("Page_People_Search", false);
    }

    private void BindData()
    {
        People_Service.ListMyPeople(t =>
        {
            ListView.BindData<EPeople>("PeopleItem", t.GetData() as List<EPeople>, (i, e) =>
            {
                i.name = "PeopleItem_" + e.PeopleID + "_" + e.ID;
                i.transform.Find("Name").GetComponent<Text>().text = e.PeopleName;
                if (e.State == Lemon.Team.Entity.Enum.PeopleState.Request)
                    i.transform.Find("Phone").GetComponent<Text>().text = "请求中";// user.Phone;
                else
                    i.transform.Find("Phone").GetComponent<Text>().text = "";
                if (e.State == Lemon.Team.Entity.Enum.PeopleState.NeedConfirm)
                {
                    GameObject tgbtn = i.transform.Find("tgbtn").gameObject;
                    GameObject jjbtn = i.transform.Find("jjbtn").gameObject;
                    tgbtn.SetActive(true);
                    jjbtn.SetActive(true);
                    EventListener.Get(tgbtn).onClick = OnTGBtnClicked;
                    EventListener.Get(jjbtn).onClick = OnJJBtnClicked;
                }
                else
                {
                    i.transform.Find("tgbtn").gameObject.SetActive(false);
                    i.transform.Find("jjbtn").gameObject.SetActive(false);
                }
                App.Instance.ShowImage(i.transform.Find("Icon").GetComponent<RawImage>(), e.PeopleFace, 11);
            });
        });
    }

    void OnItemClicked(GameObject g)
    {
        int id = int.Parse(g.name.Split('_')[1]);
        App.Instance.PageGroup.ShowPage("Page_People_Info", false, id, true);
    }

    void OnTGBtnClicked(GameObject g)
    {
        int id = int.Parse(g.transform.parent.name.Split('_')[2]);

        People_Service.ConfirmConnect(id, true, t =>
        {
            BindData();
            App.Instance.HintBox.Show("已通过！");
        });
    }

    void OnJJBtnClicked(GameObject g)
    {
        int id = int.Parse(g.transform.parent.name.Split('_')[2]);

        People_Service.ConfirmConnect(id, false, t =>
        {
            BindData();
            App.Instance.HintBox.Show("已拒绝！");
        });
    }
}
