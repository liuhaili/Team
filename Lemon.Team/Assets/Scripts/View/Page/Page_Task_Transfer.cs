using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Lemon.Team.Entity;

using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using Lemon.Team.Entity.Enum;
using Assets.Scripts.Services;

public class Page_Task_Transfer : NavigatePage
{
    public ListViewControl TransferList;

    private ETask TheTask;

    protected override void Init()
    {
        base.Init();
        PageTitle.Init("任务追踪", App.Instance.Theme.TitleBgColor, App.Instance.Theme.TitleFontColor, BtnBack);
        int taskId = GetPar<int>(0);
        BaseOperation_Service.Get<ETask>(taskId, t =>
        {
            TheTask = t.GetData() as ETask;
            BindData();
        });
    }

    void BtnBack(GameObject g)
    {
        App.Instance.PageGroup.ClosePage();
    }

    private void BindData()
    {
        TaskTransfer_Service.ListByTaskID(TheTask.ID, t =>
        {
            List<ETaskTransfer> plist = t.GetData() as List<ETaskTransfer>;
            List<System.DateTime> datelist = plist.GroupBy(c => c.CreateTime.Date).Select(c => c.Key).OrderByDescending(c => c).ToList();
            TransferList.BindData<System.DateTime>("TaskTransferItem", datelist, (i, e) =>
            {
                i.name = "TaskTransferItem_" + e.Date.ToString();
                i.transform.Find("Day").GetComponent<Text>().text = e.Date.ToString("yyyy.MM.dd");
                BindTransferOne(i.transform.Find("Panel").Find("TransferOneList").GetComponent<ListViewControl>(), e.Date);
                int rowCount = plist.Count(c => c.CreateTime >= e.Date && c.CreateTime < e.Date.AddDays(1));
                i.GetComponent<LayoutElement>().preferredHeight = rowCount * (53 + 15);
            }, false);
        });
    }

    private void BindTransferOne(ListViewControl listview, System.DateTime date)
    {
        TaskTransfer_Service.ListByTaskID(TheTask.ID, t =>
        {
            List<ETaskTransfer> plist = t.GetData() as List<ETaskTransfer>;
            plist = plist.Where(c => c.CreateTime >= date && c.CreateTime < date.AddDays(1)).ToList();
            listview.BindData<ETaskTransfer>("TaskTransferOneItem", plist, (i, e) =>
            {
                i.name = "TaskTransferOneItem_" + e.ID;
                App.Instance.ShowImage(i.transform.Find("Icon").GetComponent<RawImage>(), e.AppointFace, 11);
                i.transform.Find("Name").GetComponent<Text>().text = e.AppointName;
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
        });
    }
}
