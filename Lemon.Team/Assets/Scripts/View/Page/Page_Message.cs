using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Lemon.Team.Entity;

using System.Collections.Generic;
using System.Linq;
using Lemon.Team.Entity.Enum;
using Assets.Scripts;
using Assets.Scripts.Services;

public class Page_Message : MenuPage
{
    public TabControl Tab;

    public GameObject PageContent1;
    public ListViewControl ProjectListView;

    public GameObject PageContent2;
    public ListViewControl PeopleView;

    public GameObject PageContent3;
    public ListViewControl SystemMessageView;

    public Image ProjectTips;
    public Image PeopleTips;
    public Image SystemTips;

    List<ENewTips> TipsList = null;
    protected override void Init()
    {
        base.Init();
        PageTitle.Init("消息", App.Instance.Theme.TitleBgColor, App.Instance.Theme.TitleFontColor, null, MenuClicked);

        ProjectTips.gameObject.SetActive(false);
        PeopleTips.gameObject.SetActive(false);
        SystemTips.gameObject.SetActive(false);
        PageTitle.BtnMenu.transform.Find("tips").gameObject.SetActive(false);
        Message_Service.ListMyTips(t =>
        {
            TipsList = t.GetData() as List<ENewTips>;
            if (TipsList != null && TipsList.Count > 0)
                PageTitle.BtnMenu.transform.Find("tips").gameObject.SetActive(true);
            else
                PageTitle.BtnMenu.transform.Find("tips").gameObject.SetActive(false);

            ReBindTips();
            Tab.Init(OnTabClicked, App.Instance.Theme.SelectedItemBgColor);
        });
        PlatformCallBackListener.Instance.OnReceiveMessage = OnReceiveMessage;
    }

    protected override void Free()
    {
        base.Free();
        PlatformCallBackListener.Instance.OnReceiveMessage = null;
    }

    void OnReceiveMessage()
    {
        Message_Service.ListMyTips(t =>
        {
            TipsList = t.GetData() as List<ENewTips>;
            if (TipsList != null && TipsList.Count > 0)
                PageTitle.BtnMenu.transform.Find("tips").gameObject.SetActive(true);
            else
                PageTitle.BtnMenu.transform.Find("tips").gameObject.SetActive(false);
            ReBindTips();
            if(PageContent1.activeSelf)
                BindProjectList();
            else if(PageContent2.activeSelf)
                BindPeopleList();
            //else if(PageContent3.activeSelf)
            //    BindSystemMessageList();
        });
    }

    void ReBindTips()
    {
        if (TipsList.Count(c => c.Type == MessageType.Project) > 0)
            ProjectTips.gameObject.SetActive(true);
        else
            ProjectTips.gameObject.SetActive(false);

        if (TipsList.Count(c => c.Type == MessageType.OneToOne) > 0)
            PeopleTips.gameObject.SetActive(true);
        else
            PeopleTips.gameObject.SetActive(false);

        if (TipsList.Count(c => c.Type == MessageType.System) > 0)
            SystemTips.gameObject.SetActive(true);
        else
            SystemTips.gameObject.SetActive(false);
    }

    void MenuClicked(GameObject g)
    {
        App.Instance.DetailPageBox.Show("Page_Navigation", null, null);
    }

    void OnTabClicked(GameObject g)
    {
        if (g.name == "TabItem_1")
        {
            PageContent1.SetActive(true);
            PageContent2.SetActive(false);
            PageContent3.SetActive(false);
            BindProjectList();
        }
        else if (g.name == "TabItem_2")
        {
            PageContent1.SetActive(false);
            PageContent2.SetActive(true);
            PageContent3.SetActive(false);
            BindPeopleList();
        }
        else
        {
            PageContent1.SetActive(false);
            PageContent2.SetActive(false);
            PageContent3.SetActive(true);
            BindSystemMessageList();
        }
    }

    void OnProjectClicked(GameObject g)
    {
        int projectID = int.Parse(g.name.Split('_')[1]);
        string pname = g.transform.Find("Name").GetComponent<Text>().text;
        App.Instance.PageGroup.ShowPage("Page_Message_Info", false, MessageType.Project, projectID, pname);

        Message_Service.ClearMyTips(MessageType.Project, projectID, t =>
        {
            TipsList = t.GetData() as List<ENewTips>;
            ReBindTips();
        });
    }

    void OnPeopleClicked(GameObject g)
    {
        int peopleID = int.Parse(g.name.Split('_')[1]);
        string pname = g.transform.Find("Name").GetComponent<Text>().text;
        App.Instance.PageGroup.ShowPage("Page_Message_Info", false, MessageType.OneToOne, peopleID, pname);

        Message_Service.ClearMyTips(MessageType.OneToOne, peopleID, t =>
        {
            TipsList = t.GetData() as List<ENewTips>;
            ReBindTips();
        });
    }

    void BindProjectList()
    {
        Project_Service.ListTeamProject(t =>
        {
            ProjectListView.OnItemClicked = OnProjectClicked;
            ProjectListView.BindData<EProject>("ProjectIconItem", t.GetData() as List<EProject>, (i, e) =>
            {
                i.name = "ProjectIconItem_" + e.ID.ToString();
                i.transform.Find("Name").GetComponent<Text>().text = e.Name;

                App.Instance.ShowImage(i.transform.Find("Icon").GetComponent<RawImage>(), e.Face, 12);

                if (TipsList != null && TipsList.Count(c => c.Type == MessageType.Project && c.OwnerID == e.ID) > 0)
                    i.transform.Find("tips").gameObject.SetActive(true);
                else
                    i.transform.Find("tips").gameObject.SetActive(false);

            }, true);
        });
    }

    void BindPeopleList()
    {
        People_Service.ListMyPeople(t =>
        {
            PeopleView.OnItemClicked = OnPeopleClicked;
            List<EPeople> plist = t.GetData() as List<EPeople>;
            plist = plist.Where(c => c.State == PeopleState.Normal).ToList();
            PeopleView.BindData<EPeople>("PeopleMsgItem", plist, (i, e) =>
            {
                i.name = "PeopleMsgItem_" + e.PeopleID;
                i.transform.Find("Name").GetComponent<Text>().text = e.PeopleName;

                App.Instance.ShowImage(i.transform.Find("Icon").GetComponent<RawImage>(), e.PeopleFace, 11);

                if (TipsList != null && TipsList.Count(c => c.Type == MessageType.OneToOne && c.OwnerID == e.PeopleID) > 0)
                    i.transform.Find("tips").gameObject.SetActive(true);
                else
                    i.transform.Find("tips").gameObject.SetActive(false);
            });
        });
    }

    void BindSystemMessageList()
    {
        Message_Service.ListByTargetID(MessageType.System, 0, t =>
        {
            SystemMessageView.BindData<EMessage>("MessageItem", t.GetData() as List<EMessage>, (i, e) =>
            {
                i.name = "MessageItem_" + e.ID.ToString();
                i.transform.Find("Text (1)").GetComponent<Text>().text = e.CreateTime.ToString("yyyy-MM-dd hh:mm:ss");
                i.transform.Find("Text").GetComponent<Text>().text = "系统";
                Transform bg = i.transform.Find("Image (1)");
                Text content = bg.Find("Text").GetComponent<Text>();
                content.text = e.Content;
                float textH = content.preferredHeight + 20;
                if (e.SenderID == Session.UserID)
                {
                    i.transform.Find("Image (2)").GetComponent<Image>().color = new Color(178 / 255.0f, 255 / 255.0f, 195 / 255.0f);
                    bg.GetComponent<Image>().color = new Color(178 / 255.0f, 255 / 255.0f, 195 / 255.0f);
                }
                else
                {
                    bg.GetComponent<Image>().color = Color.white;
                    i.transform.Find("Image (2)").GetComponent<Image>().color = Color.white;
                }
                i.GetComponent<LayoutElement>().preferredHeight = textH + 32;
            }, true);
        });
    }
}
