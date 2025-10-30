using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour //Parallax(視差)サブ背景の動きを管理
{

    public Camera cam;
    public Transform followTarget;  //追う対象者

    //視差が始まる場所
    Vector2 startingPosition;  

    //視差が始まるZの値, 2Dの場合、Z軸は奥行き
    float startingZ;

    // => と書くことで、毎フレーム(呼ばれるごとに)自分でアップデートできる変数が作れる。
    Vector2 camMoveSinceStart => (Vector2)cam.transform.position - startingPosition;    //カメラがスタートしてからどれくらい動いたかを示す変数。

    //対象者とのZ軸の距離
    float zDistanceFromTarget => (transform.position.z - followTarget.transform.position.z) * 10;   

    //近いクリップか遠いクリップの値どちらを選ぶかを、distanceFromSubjectの値がプラス+かマイナス-かをもとにnearかfarClipPlaneを決める。
    float clippingPlane => (cam.transform.position.z + (zDistanceFromTarget > 0 ? cam.farClipPlane : cam.nearClipPlane));   
    //parallaxオブジェクト(視差オブジェクト)がプレイヤーの前にいたら-の値(nearClipPlane)、後ろにいたら＋の値(farClipPlane)

    //視差の割合
    float parallaxFactor => Mathf.Abs(zDistanceFromTarget)   / clippingPlane;

    void Start()
    {
        startingPosition = transform.position;
        
        startingZ = transform.position.z;
    }

    void Update()
    {
        //Target対象者が動いたとき、背景も動かす。
        Vector2 newPosition = startingPosition + camMoveSinceStart * parallaxFactor;

        //X軸とY軸は対象者の移動スピードによって変化するが、Z軸はそのまま。
        transform.position = new Vector3(newPosition.x, newPosition.y, startingZ);
    }
}
