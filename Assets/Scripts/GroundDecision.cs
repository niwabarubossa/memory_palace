using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDecision : MonoBehaviour
{

    //Playerを変数にする
    GameObject player;
    //PlayerControllerを変数にする
    PlayerController pc;

    void Start()
    {
        //親オブジェクトのPlayerを検索して変数に入れる
        player = transform.root.gameObject;
        //PlayerにアタッチしているPlayerControllerスクリプトを入れる
        pc = player.GetComponent<PlayerController>();
    }

    //地面に接している間、PlayerControllerの
    //GroundDecisionTrue();を呼び続ける
    void OnTriggerStay(Collider other)
    {
        pc.GroundDecisionTrue();
    }

    //地面から離れた時、PlayerControllerの
    //GroundDecisionFalse();を呼び続ける
    void OnTriggerExit(Collider other)
    {
        pc.GroundDecisionFalse();
    }
}