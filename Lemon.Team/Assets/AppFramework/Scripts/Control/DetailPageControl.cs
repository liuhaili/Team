using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using PathologicalGames;

public class DetailPageControl : ObjectBase
{
    public GameObject BoxContent;
    private Transform ContentChild;
    private System.Action<string> OnClosed;
    public bool IsShow = false;
    public void Show(string contentName, System.Action<string> onClosed, object data = null)
    {
        IsShow = true;
        base.ExcuteInit();
        OnClosed = onClosed;
        this.gameObject.SetActive(true);
        if (!string.IsNullOrEmpty(contentName))
        {
            Transform item = SignalObjectManager.Instance.Spawn(contentName);
            item.parent = BoxContent.transform;
            item.GetComponent<RectTransform>().offsetMin = Vector2.zero;
            item.GetComponent<RectTransform>().offsetMax = Vector2.zero;
            item.GetComponent<DetailPage>().SetPar(0, data);
            item.GetComponent<DetailPage>().ExcuteInit();
            ContentChild = item;
        }
        EventListener.Get(this.gameObject).onClick = OnBackGroundClicked;
    }

    void OnBackGroundClicked(GameObject g)
    {
        Hide();
    }

    public void Hide()
    {
        IsShow = false;
        base.ExcuteFree();
        if (ContentChild != null)
        {
            ContentChild.GetComponent<DetailPage>().ExcuteFree();
            SignalObjectManager.Instance.Despawn(ContentChild);
            if (OnClosed != null)
                OnClosed(ContentChild.GetComponent<DetailPage>().SelectedData);
        }
        this.gameObject.SetActive(false);
    }
}
