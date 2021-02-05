using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axis : MonoBehaviour
{

    //X軸の角度を制限するための変数
    float angleUp = 60f;
    float angleDown = -60f;

    //Player、Main Camera、ユニティちゃんを登録します。
    [SerializeField] GameObject player;
    [SerializeField] Camera came;
    [SerializeField] GameObject unityChan;

    //Cameraが回転するスピード
    [SerializeField] float rotate_speed = 3;
    //Axisの位置を指定する変数
    [SerializeField] Vector3 axisPos;
    //三人称の際のCameraの位置
    [SerializeField] Vector3 thirdPos;
    //三人称の際のCameraにスクロールで伸びた距離を足した値を入れる
    [SerializeField] Vector3 thirdPosAdd;
    //ホイールの値を0.1から10まで
    //[SerializeField, Range(0.1f, 10f)] private float wheelSpeed = 1f;

    //マウスホイールの値を入れる
    [SerializeField] float scroll;
    //
    //[SerializeField] float scrollAdd;
    //マウスホイールの値を保存
    [SerializeField] float scrollLog;

    //三人称と一人称を切り替える
    [SerializeField] bool switching = true;
    //一度だけ一人称にするため
    [SerializeField] bool toFirst = false;
    //一度だけ三人称にするため
    [SerializeField] bool toThird = false;

    //Vector3.SmoothDampで現在のスピードを入れる
    private Vector3 velocity = Vector3.zero;
    //Vector3.SmoothDampで目的地に到達するための時間
    [SerializeField] float smoothTime = 1.5f;

    //public float time;

    void Start()
    {
        //Cameraの最初の位置
        came.transform.localPosition = new Vector3(0, 0, -3);
        toFirst = true;
    }

    void Update()
    {
        //Axisの位置はPlayerの位置＋α、Inspectorで設定する
        transform.position = player.transform.position + axisPos;
        //三人称の時のCameraの位置にマウススクロールの値を足して位置を調整
        thirdPosAdd = thirdPos + new Vector3(0, 0, scrollLog);

        //Fキーで三人称と一人称を切り替える
        if (Input.GetKeyDown(KeyCode.F))
        {
            switching = !switching;
        }

        //switchingがTrueなら三人称、Falseなら一人称
        if (switching)
        {
            //三人称の時はユニティちゃんを表示する
            unityChan.SetActive(true);
            //三人称の時のCameraの移動はswitchingがTrueになった時の一度だけ
            if (toThird)
            {
                //Cameraの位置を変更する
                //(Cameraの今の位置、目的地、現在のスピード、かかる時間）
                came.transform.localPosition = Vector3.SmoothDamp(
                    came.transform.localPosition,
                    new Vector3(0, 0,
                    thirdPosAdd.z),
                    ref velocity,
                    smoothTime);

                //Axisの角度調整
                //(現在の角度、Playerの角度、動く割合）
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    player.transform.rotation,
                    Time.deltaTime);

                //2秒後に下のToThirdSwitch()関数を実行
                Invoke("ToThirdSwitch", 2);
            }

            //下のprivate void ThirdPersonMode()を実行
            ThirdPersonMode();
        }
        else
        {
            //一人称の時はユニティちゃんを消す
            unityChan.SetActive(false);
            //switchingがFalseになった時に一度だけ実行
            if (toFirst)
            {
                //CameraをAxisと同じ位置へ
                came.transform.position = Vector3.SmoothDamp(
                    came.transform.position,
                    transform.position,
                    ref velocity,
                    smoothTime);

                //三人称の時と同じくAxisの角度はPlayerと同じにする
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    player.transform.rotation,
                    Time.deltaTime);

                //2秒後に下のToFirstSwitch()関数を実行
                Invoke("ToFirstSwitch", 2);
            }

            //下のprivate void FirstPersonMode()を実行
            FirstPersonMode();
        }

        //Mouse Yはマウスの縦移動でX軸が回転して上下にカメラが動く
        //Mouse Xはマウスの横移動でY軸が回転してカメラが左右に回る
        transform.eulerAngles += new Vector3(
            Input.GetAxis("Mouse Y") * rotate_speed,
            Input.GetAxis("Mouse X") * rotate_speed
            , 0);

        float angle_x = transform.eulerAngles.x;
        //X軸の値を180度超えたら360引くことで制限しやすくする
        if (angle_x >= 180)
        {
            angle_x = angle_x - 360;
        }
        //Mathf.Clamp(値、最小値、最大値）でX軸の値を制限する
        transform.eulerAngles = new Vector3(
            Mathf.Clamp(angle_x, angleDown, angleUp),
            transform.eulerAngles.y,
            transform.eulerAngles.z
        );
    }

    //三人称の時に作動
    private void ThirdPersonMode()
    {
        //マウススクロールの値を入れる
        scroll = Input.GetAxis("Mouse ScrollWheel");
        //scrollAdd += Input.GetAxis("Mouse ScrollWheel") * -10;
        //マウススクロールの値は動かさないと0になるのでここで保存する
        scrollLog += Input.GetAxis("Mouse ScrollWheel");

        //Cameraの位置、Z軸にスクロール分を加える
        came.transform.localPosition
            = new Vector3(came.transform.localPosition.x,
            came.transform.localPosition.y,
            came.transform.localPosition.z + scroll);
    }

    //一人称の時に作動
    private void FirstPersonMode()
    {
        //一人称の時はカメラの向きに合わせてPlayerが向きを変えるようにする
        player.transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }

    //三人称になった時実行
    void ToThirdSwitch()
    {
        toThird = false;
        toFirst = true;
    }

    //一人称になった時実行
    void ToFirstSwitch()
    {
        toThird = true;
        toFirst = false;
    }
}