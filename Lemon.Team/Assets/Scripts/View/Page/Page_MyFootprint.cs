using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Lemon.Team.Entity;

using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using Lemon.Team.Entity.Enum;
using Assets.Scripts.Services;

public class Page_MyFootprint : Page_MenuBase
{
    public ListViewLoadingControl TransferList;

    private int PageIndex = 0;
    private int PageSize = 20;
    private List<ETaskTransfer> TaskTransferList;
    protected override void Init()
    {
        base.Init();
        PageTitle.Init("我的足迹", App.Instance.Theme.TitleBgColor, App.Instance.Theme.TitleFontColor, null, MenuClicked);

        TransferList.OnAddData = AddData;
        TransferList.OnUpdateData = UpdateData;
        BindData();
    }

    void MenuClicked(GameObject g)
    {
        App.Instance.DetailPageBox.Show("Page_Navigation", null, null);
    }

    private void UpdateData()
    {
        PageIndex = 0;
        BindData();
    }
    private void AddData()
    {
        TaskTransfer_Service.ListByUserID(Session.UserID, (PageIndex + 1), PageSize, t =>
           {
               List<ETaskTransfer> plist = t.GetData() as List<ETaskTransfer>;
               TaskTransferList.AddRange(plist);
               List<System.DateTime> datelist = TaskTransferList.GroupBy(c => c.CreateTime.Date).Select(c => c.Key).OrderByDescending(c => c).ToList();
               datelist = datelist.Where(c => !TransferList.DataList.Any(b => (System.DateTime)b == c)).ToList();
               TransferList.AddData(datelist.Select(c => (object)c).ToList());
               if (plist.Count >= 0)
                   PageIndex++;
           });
    }

    private void BindData()
    {
        TaskTransfer_Service.ListByUserID(Session.UserID, PageIndex, PageSize, t =>
        {
            TaskTransferList = t.GetData() as List<ETaskTransfer>;
            List<System.DateTime> datelist = TaskTransferList.GroupBy(c => c.CreateTime.Date).Select(c => c.Key).OrderByDescending(c => c).ToList();
            List<object> objlist = datelist.Select(c => (object)c).ToList();
            TransferList.BindData("MyFootprintItem", objlist, (i, gg) =>
            {
                System.DateTime e = (System.DateTime)gg;
                i.name = "MyFootprintItem_" + e.Date.ToString();
                i.transform.Find("Day").GetComponent<Text>().text = e.Date.ToString("yyyy.MM.dd");
                BindTransferOne(i.transform.Find("Panel").Find("TransferOneList").GetComponent<ListViewControl>(), e.Date);
                int rowCount = TaskTransferList.Count(c => c.CreateTime >= e.Date && c.CreateTime < e.Date.AddDays(1));
                i.GetComponent<LayoutElement>().preferredHeight = rowCount * 70 + 15;
            }, false);
        });
    }

    private void BindTransferOne(ListViewControl listview, System.DateTime date)
    {
        List<ETaskTransfer> plist = TaskTransferList.Where(c => c.CreateTime >= date && c.CreateTime < date.AddDays(1)).ToList();
        listview.BindData<ETaskTransfer>("MyFootprintItemOne", plist, (i, e) =>
        {
            i.name = "MyFootprintItemOne_" + e.ID;
            i.gameObject.SetActive(true);
            i.transform.Find("TaskName").GetComponent<Text>().text = e.TaskName;
            i.transform.Find("Idea").GetComponent<Text>().text = e.Note;
            i.transform.Find("Time").GetComponent<Text>().text = e.CreateTime.ToString("hh:mm");
            if (e.AssignedPersonID != 0)
            {
                i.transform.Find("ToOther").gameObject.SetActive(true);
                i.transform.Find("DoProcess").gameObject.SetActive(false);
                i.transform.Find("ToOther").Find("Name (1)").GetComponent<Text>().text = e.AssignedName;
                App.Instance.ShowImage(i.transform.Find("ToOther").Find("Icon (2)").GetComponent<RawImage>(), e.AppointFace, 11);
            }
            else
            {
                i.transform.Find("ToOther").gameObject.SetActive(false);
                i.transform.Find("DoProcess").gameObject.SetActive(true);
                i.transform.Find("DoProcess").Find("Name (1)").GetComponent<Text>().text = e.StepName;
            }
        }, false);
    }
}
