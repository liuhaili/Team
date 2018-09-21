using UnityEngine;
using System.Collections;
using Lemon.Team.Entity;

using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using Assets.Scripts.Services;

public class Page_Task_Search : DetailPage
{
    public PageTitleControl PageTitle;
    public ListViewControl SearchList;
    public Button AddBtn;

    private bool IsEdit = false;
    protected override void Init()
    {
        base.Init();
        IsEdit = false;
        PageTitle.Init("任务查找", Color.gray, App.Instance.Theme.TitleFontColor, BtnBack);
        PageTitle.AddButton("", App.Instance.ImageManger.ImageList[13], OnEditClicked);
        EventListener.Get(AddBtn.gameObject).onClick = OnSearchAddClicked;
        BindData();
    }

    void BtnBack(GameObject g)
    {
        App.Instance.PageGroup.ClosePage();
    }

    void OnEditClicked(GameObject g)
    {
        if (!IsEdit)
        {
            IsEdit = true;
            g.GetComponent<Image>().sprite = App.Instance.ImageManger.ImageList[3];
        }
        else
        {
            IsEdit = false;
            g.GetComponent<Image>().sprite = App.Instance.ImageManger.ImageList[13];
        }
        BindData();
    }

    void OnDelClicked(GameObject g)
    {
        string idstr = g.transform.parent.name.Split('_')[1];

        App.Instance.DialogBox.Show("提示信息", "", "你确定要删除该成员吗？", 300, 150, c =>
        {
            BaseOperation_Service.Delete<EUserSearch>(int.Parse(idstr), gg =>
            {
                BindData();
            });
        }, null);
    }

    protected override void Free()
    {
        base.Free();
    }

    void OnSearchAddClicked(GameObject g)
    {
        App.Instance.PageGroup.ShowPage("Page_Task_SearchCreate", false);
    }

    void OnItemClicked(GameObject g)
    {
        string idstr = g.name.Split('_')[1];
        UserSearch_Service.GetByID(int.Parse(idstr), c =>
        {
            EUserSearch uSearch = c.GetData() as EUserSearch;
            App.Instance.PageGroup.ShowPage("Page_Task", true, uSearch);
        });
    }

    void BindData()
    {
        UserSearch_Service.ListMy(t =>
        {
            List<EUserSearch> plist = t.GetData() as List<EUserSearch>;
            SearchList.BindData<EUserSearch>("SeachTaskItem", plist, (i, e) =>
            {
                i.name = "SeachTaskItem_" + e.ID.ToString();
                i.transform.Find("Name").GetComponent<Text>().text = e.Name;

                GameObject delBtn = i.transform.Find("NavIcon").gameObject;
                if (IsEdit)
                    delBtn.SetActive(true);
                else
                    delBtn.SetActive(false);
                EventListener.Get(delBtn).onClick = OnDelClicked;

                GameObject defaultBtn = i.transform.Find("Icon").gameObject;
                if (e.IsDefault)
                {
                    defaultBtn.GetComponent<RawImage>().color = Color.green;
                    EventListener.Get(defaultBtn).onClick = null;
                }
                else
                {
                    defaultBtn.GetComponent<RawImage>().color = Color.white;
                    EventListener.Get(defaultBtn).onClick = OnSetDefaultClicked;
                }


            }, true);
            SearchList.OnItemClicked = OnItemClicked;
        });
    }

    void OnSetDefaultClicked(GameObject g)
    {
        string idstr = g.transform.parent.name.Split('_')[1];
        UserSearch_Service.SetDefault(int.Parse(idstr), t =>
        {
            BindData();
        });
    }
}
