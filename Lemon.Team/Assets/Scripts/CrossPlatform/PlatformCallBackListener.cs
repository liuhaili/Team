using UnityEngine;
using System.Collections;
using Lemon.Team.Entity;
using UnityEngine.UI;
using Assets.Scripts.Services;
using Assets.Scripts;
using System.IO;

public class PlatformCallBackListener : MonoBehaviour
{
    public System.Action<string, byte[]> OnUploadComplated = null;
    public System.Action<string> OnLogin = null;
    public System.Action<string, string> OnGetUserInfo = null;
    public System.Action OnReceiveMessage = null;

    public static PlatformCallBackListener Instance;
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
    }

    void onChooseFileResult(string base64)
    {
        string[] pars = base64.Split(new string[] { "*|*" }, System.StringSplitOptions.RemoveEmptyEntries);
        string filedata = pars[0];
        string filename = Path.GetFileName(pars[1]);
        byte[] inputBytes = System.Convert.FromBase64String(filedata);

        if (OnUploadComplated != null)
            OnUploadComplated(filename, inputBytes);
    }

    void onLogin(string openid)
    {
        if (OnLogin != null)
            OnLogin(openid);
    }

    void onGetUserInfo(string userinfo)
    {
        if (OnGetUserInfo != null)
        {
            string[] pars = userinfo.Split('|');
            OnGetUserInfo(pars[0], pars[1]);
        }
    }

    void onReceiveMessage(string msg)
    {
        if (OnReceiveMessage != null)
            OnReceiveMessage();
    }
}
