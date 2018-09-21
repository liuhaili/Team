using UnityEngine;
using System.Collections;
using Lemon.Team.Entity;

using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using Assets.Scripts.Services;

public class Page_UserSelected : DialogPage
{
    public InputField SearchWord;
    public ListViewControl List;

    protected override void Init()
    {
        base.Init();
        List.OnItemClicked = OnItemClicked;
        List.Clear();
        List<EKeyName> plist = this.GetPar<List<EKeyName>>(0);
        BindData(plist);
        SearchWord.onValueChanged.AddListener(delegate { OnSearchWordChanged(); });
    }

    protected override void Free()
    {
        base.Free();
    }

    void OnItemClicked(GameObject g)
    {
        this.SelectedData = g.name.Split('_')[1];
        this.SelectedData2 = g.transform.Find("Name").GetComponent<Text>().text;
    }

    void OnSearchWordChanged()
    {
        string searchword = SearchWord.text;
        List.Clear();
        List<EKeyName> plist = this.GetPar<List<EKeyName>>(0);
        plist = plist.Where(c => c.ID.ToString().Contains(searchword) || c.Name.Contains(searchword)).ToList();
        BindData(plist);
    }

    void BindData(List<EKeyName> plist)
    {
        List.BindData<EKeyName>("UserSelectedItem", plist, (i, e) =>
        {
            i.name = "UserSelectedItem_" + e.ID.ToString();
            i.transform.Find("Name").GetComponent<Text>().text = e.Name;
            App.Instance.ShowImage(i.transform.Find("Icon").GetComponent<RawImage>(), e.Other, 11);
        }, true, true);
    }
}
