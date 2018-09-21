package com.Lemon.Team;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.content.IntentFilter;
import android.content.Intent;
import android.graphics.Bitmap;
import android.net.Uri;
import android.widget.Toast;
import com.unity3d.player.UnityPlayer;
import com.unity3d.player.UnityPlayerActivity;
import java.io.File;
import java.io.FileInputStream;
import java.io.IOException;
import java.io.ByteArrayOutputStream;
import android.util.Base64;
import android.database.Cursor;
import android.provider.MediaStore;
import android.content.ContentResolver;

import com.tencent.connect.UserInfo;
import com.tencent.connect.common.Constants;
import com.tencent.tauth.IUiListener;
import com.tencent.tauth.Tencent;
import com.tencent.tauth.UiError;
import com.tencent.connect.share.QQShare;

import org.json.JSONException;
import org.json.JSONObject;

import com.igexin.sdk.PushManager;

public class MainActivity extends UnityPlayerActivity {

    protected boolean IsCropPhoto=true;
    protected int CropPhotoWidth=100;
    protected  int CropPhotoHeight=100;
    private Tencent mTencent;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        //setContentView(R.layout.activity_main);
        mTencent =  Tencent.createInstance("1105455181", this);
        // com.getui.demo.DemoPushService 为第三方自定义推送服务
        PushManager.getInstance().initialize(this.getApplicationContext(), com.Lemon.Team.TeamGTPushService.class);
        PushManager.getInstance().registerPushIntentService(this.getApplicationContext(), com.Lemon.Team.TeamGTIntentService.class);

//        IntentFilter intentFilter = new IntentFilter();
//        intentFilter.addAction("TeamGTPushReceiver");
//        registerReceiver(new TeamGTPushReceiver(), intentFilter);
    }
    //给U3D调用的打开相册
    public void SelectPhoto(boolean isCropPhoto,int cropPhotoWidth,int cropPhotoHeight){
        IsCropPhoto=isCropPhoto;
        CropPhotoWidth=cropPhotoWidth;
        CropPhotoHeight=cropPhotoHeight;
        //Intent就是应用之间，应用不同Activity之间交互。
        Intent getAlbum = new Intent(Intent.ACTION_GET_CONTENT);  //新建Intent，让用户选择特定类型的数据，并返回该数据的URI.

        getAlbum.setType("image/*");  //类型为图片

        startActivityForResult(getAlbum, 108); //调用相册，结果码108，如果有结果会返回108，这个值随便设置，只要>=0就行
    }

    public void SelectFile() {
        Intent intent = new Intent(Intent.ACTION_GET_CONTENT);
        intent.setType("*/*");
        intent.addCategory(Intent.CATEGORY_OPENABLE);

        try {
            startActivityForResult(Intent.createChooser(intent, "Select a File to Upload"), 110);
        } catch (android.content.ActivityNotFoundException ex) {
            Toast.makeText(this, "Please install a File Manager.",  Toast.LENGTH_SHORT).show();
        }
    }

    //重写Activity里的onActivityResult方法，这个方法和startActivityForResult是一对出现。
    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent intent) {
        if (requestCode == Constants.REQUEST_LOGIN ||
                requestCode == Constants.REQUEST_APPBAR) {
            Tencent.onActivityResultData(requestCode, resultCode, intent, loginListener);
        }
        // 用户没有进行有效的设置操作，返回系统固定的常量RESULT_CANCELED=0
        if (resultCode == RESULT_CANCELED) {
            Toast.makeText(getApplication(), "没有选择图片", Toast.LENGTH_LONG).show();
            return;
        }
        switch (requestCode) {
            case 108:
                Toast.makeText(getApplication(), "图片挑选完成", Toast.LENGTH_LONG).show();
                if(IsCropPhoto)
                    CropPhoto(intent.getData());
                else
                    SendSelectedFileToUnity(intent);
                break;
            case 109:
                Toast.makeText(getApplication(), "图片裁剪完成", Toast.LENGTH_LONG).show();
                try {
                    SendCropPhotoToUnity(intent);
                }
                catch (Exception e) {
                    e.printStackTrace();
                }
                break;
            case 110:
                Toast.makeText(getApplication(), "文件选择完成", Toast.LENGTH_LONG).show();
                SendSelectedFileToUnity(intent);
                break;
        }
        super.onActivityResult(requestCode, resultCode, intent);
    }

    //裁剪图片
    public void CropPhoto(Uri uri) {
        //调用裁剪器
        Intent intent = new Intent("com.android.camera.action.CROP");
        intent.setDataAndType(uri, "image/*");
        // 开启裁剪功能
        intent.putExtra("crop", "true");
        //设置宽高的比例
        intent.putExtra("aspectX", 1);
        intent.putExtra("aspectY", 1);
        //裁剪图片宽高
        intent.putExtra("outputX", CropPhotoWidth);
        intent.putExtra("outputY", CropPhotoHeight);
        //请求返回数据
        intent.putExtra("return-data", true);

        startActivityForResult(intent, 109);  //结果码109
    }

    //保存裁剪图片供U3D读取 使用 FileOutputStream 必须要捕获和处理错误 抛出异常IOException
    private String SendCropPhotoToUnity(Intent intent) throws IOException {
        //获取intent传过来的数据
        //Bundle类是键值对形式，传递数据  Intent也可以传递数据，两个区别：intent是Bundle的封装
        //Intent主要数据传递用，bundle主要存取数据用。
        Bundle extras = intent.getExtras();
        if (extras != null) {
            //取出数据
            Bitmap bitmap = extras.getParcelable("data");
            ByteArrayOutputStream baos = new ByteArrayOutputStream();
            bitmap.compress(Bitmap.CompressFormat.JPEG, 100, baos);
            byte[] bytes = baos.toByteArray();
            String base64str= Base64.encodeToString(bytes, 0);
            UnityPlayer.UnitySendMessage("PlatformCallBackListener","onChooseFileResult",base64str+"*|*temp.png");
        }
        return null;
    }

    private void SendSelectedFileToUnity(Intent intent){
        Uri uri = intent.getData();
        String path = getRealFilePath(uri);
        try {
            String base64str=encodeBase64File(path);
            UnityPlayer.UnitySendMessage("PlatformCallBackListener","onChooseFileResult",base64str+"*|*"+path);
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }

    public String getRealFilePath(final Uri uri ) {
        if ( null == uri )
            return null;
        final String scheme = uri.getScheme();
        String data = null;
        ContentResolver resolver = getContentResolver();
        if ( scheme == null )
            data = uri.getPath();
        else if ( resolver.SCHEME_FILE.equals( scheme ) ) {
            data = uri.getPath();
        } else if ( ContentResolver.SCHEME_CONTENT.equals( scheme ) ) {
            Cursor cursor = resolver.query( uri, new String[] { MediaStore.Images.Media.DATA }, null, null, null );
            if ( null != cursor ) {
                if ( cursor.moveToFirst() ) {
                    int index = cursor.getColumnIndex( MediaStore.Images.Media.DATA );
                    if ( index > -1 ) {
                        data = cursor.getString( index );
                    }
                }
                cursor.close();
            }
        }
        return data;
    }

    public String encodeBase64File(String path) throws Exception {
        File  file = new File(path);
        FileInputStream inputFile = new FileInputStream(file);
        byte[] buffer = new byte[(int)file.length()];
        inputFile.read(buffer);
        inputFile.close();
        return Base64.encodeToString(buffer,Base64.DEFAULT);
    }

    /**
     * 实现QQ第三方登录
     */
    IUiListener loginListener = new IUiListener() {
        @Override
        public void onComplete(Object o) {
            //登录成功后回调该方法,可以跳转相关的页面
            Toast.makeText(MainActivity.this, "登录成功", Toast.LENGTH_SHORT).show();
            JSONObject object = (JSONObject) o;
            try {
                String accessToken = object.getString("access_token");
                String expires = object.getString("expires_in");
                String openID = object.getString("openid");
                mTencent.setAccessToken(accessToken, expires);
                mTencent.setOpenId(openID);
                UnityPlayer.UnitySendMessage("PlatformCallBackListener","onLogin",openID);
            } catch (JSONException e) {
                e.printStackTrace();
            }
        }
        @Override
        public void onError(UiError uiError) {}

        @Override
        public void onCancel() {}
    };

    public void qqLogin() {
        mTencent.login(this, "all", loginListener);
    }

    public void qqGetUserInfo()
    {
        UserInfo info = new UserInfo(this, mTencent.getQQToken());
        info.getUserInfo(new IUiListener() {
            @Override
            public void onComplete(Object o) {
                try {
                    JSONObject info = (JSONObject) o;
                    String nickName = info.getString("nickname");//获取用户昵称
                    String iconUrl = info.getString("figureurl_qq_2");//获取用户头像的url
                    Toast.makeText(MainActivity.this,"昵称："+nickName, Toast.LENGTH_SHORT).show();
                    UnityPlayer.UnitySendMessage("PlatformCallBackListener","onGetUserInfo",nickName+"|"+iconUrl);
                    //Glide.with(MainActivity.this).load(iconUrl).transform(new GlideRoundTransform(MainActivity.this)).into(icon);//Glide解析获取用户头像
                } catch (JSONException e) {
                    e.printStackTrace();
                }
            }

            @Override
            public void onError(UiError uiError) {

            }

            @Override
            public void onCancel() {

            }
        });
    }

    public String GTClientID()
    {
        return PushManager.getInstance().getClientid(this.getApplicationContext());
    }

    public void qqShare(String title,String content,String url,String imgUrl)
    {
        final Bundle params = new Bundle();
        params.putInt(QQShare.SHARE_TO_QQ_KEY_TYPE, QQShare.SHARE_TO_QQ_TYPE_DEFAULT);
        params.putString(QQShare.SHARE_TO_QQ_TITLE, title);
        params.putString(QQShare.SHARE_TO_QQ_SUMMARY,  content);
        params.putString(QQShare.SHARE_TO_QQ_TARGET_URL,  url);
        params.putString(QQShare.SHARE_TO_QQ_IMAGE_URL,imgUrl);
        params.putString(QQShare.SHARE_TO_QQ_APP_NAME,  "team");
        params.putInt(QQShare.SHARE_TO_QQ_EXT_INT,  0);
        mTencent.shareToQQ(MainActivity.this, params, loginListener);
    }
}
