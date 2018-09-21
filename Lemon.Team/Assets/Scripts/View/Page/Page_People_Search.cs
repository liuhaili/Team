using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Lemon.Team.Entity;

using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using Assets.Scripts.Services;

public class Page_People_Search : NavigatePage
{
    public ListViewControl ListView;
    public InputField SearchWord;
    protected override void Init()
    {
        base.Init();
        PageTitle.Init("查找成员", App.Instance.Theme.TitleBgColor, App.Instance.Theme.TitleFontColor, BtnBack);
        SearchWord.onValueChanged.AddListener(OnUpdateSelect);
        BindData();
    }

    void BtnBack(GameObject g)
    {
        App.Instance.PageGroup.ClosePage();
    }

    void OnUpdateSelect(string val)
    {
        BindData();
    }

    private void BindData()
    {
        People_Service.ListMyPeople(p =>
        {
            List<EPeople> myPeople = p.GetData() as List<EPeople>;
            User_Service.SearchUser(SearchWord.text, u =>
            {
                List<EUser> plist = u.GetData() as List<EUser>;
                ListView.BindData<EUser>("PeopleSearchItem", plist, (i, e) =>
                {
                    i.name = "PlanItem_" + e.ID;
                    i.transform.Find("Name").GetComponent<Text>().text = e.Name;
                    i.transform.Find("Phone").GetComponent<Text>().text = e.Phone;
                    if (myPeople.Any(c => c.PeopleID == e.ID))
                    {
                        i.transform.Find("Button").GetComponent<Button>().gameObject.SetActive(false);
                    }
                    else
                    {
                        i.transform.Find("Button").GetComponent<Button>().gameObject.SetActive(true);
                        EventListener.Get(i.transform.Find("Button").gameObject).onClick = OnBtnAddClicked;
                    }
                    App.Instance.ShowImage(i.transform.Find("Icon").GetComponent<RawImage>(), e.Face, 11);
                });
            });
        });
    }

    void OnBtnAddClicked(GameObject g)
    {
        int pid = int.Parse(g.transform.parent.gameObject.name.Split('_')[1]);

        People_Service.AskConnect(pid, t =>
        {
            BindData();
            App.Instance.HintBox.Show("请求已经发出！");
        });
    }
}
