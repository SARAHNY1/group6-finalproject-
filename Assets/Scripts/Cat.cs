using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

using System;

using System.Linq;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.UI;
using Unity.XR.CoreUtils;

public class Cat : MonoBehaviour
{
    public float speed;
    public GameObject fbx;
    GameObject player;
    //自定义多次位移目标位置和判定范围
    public Vector3 pos1;
    public Vector3 euler1;
    public Vector3 targetPos1;
    public Vector3 pos2;
    public Vector3 euler2;
    public Vector3 targetPos2;
    public Vector3 pos3;
    public Vector3 euler3;
    public Vector3 targetPos3;
    bool trans1;
    bool trans2;
    bool trans3;
    //是否旋转
    bool isZ;
    public Vector3 rotatePos1;
    public Vector3 rotatePos2;

    public Text HintText;

    void Start()
    {
        print(GameObject.FindGameObjectsWithTag("222").Length);//
        player = GameObject.FindGameObjectWithTag("Player");
        trans1 = false;
        trans2 = false;
        isZ = false;
    }

    // Update is called once per frame
    void Update()
    {
        //QE控制走向
        if (Input.GetAxisRaw("XRI_Right_SecondaryButton") >0f)
        {
            if (isZ)
            {
                transform.Translate(transform.right * -1f * speed * Time.deltaTime);
            }
            else
                transform.Translate(transform.forward * -1f * speed * Time.deltaTime);
        }
        else if (Input.GetAxisRaw("XRI_Right_PrimaryButton") > 0f)
        {
            if (isZ)
            {
                transform.Translate(transform.right * 1f * speed * Time.deltaTime);
            }
            else
                transform.Translate(transform.forward * 1f * speed * Time.deltaTime);
        }
        //调试玩家位置视角和猫的位置
        if (Input.GetMouseButtonDown(0))
            print("玩家位置：" + player.transform.position + "玩家视角：" + Camera.main.transform.eulerAngles + "Cat位置" + transform.position);
        //位移
        if (trans1)
        {
            if (posAndrotIsTrue(player.transform.position, Camera.main.transform.eulerAngles))
            {
                if ((Input.GetAxisRaw("XRI_Left_PrimaryButton") > 0f))
                {
                    transform.position = targetPos1;
                    //调整猫位移后的朝向
                    transform.eulerAngles = new Vector3(0, 90, 0);
                    trans1 = false;
                    isZ = true;
                    HintText.text = "";
                }
            }
        }
        if (trans2)
        {
            if (posAndrotIsTrue2(player.transform.position, Camera.main.transform.eulerAngles))
            {
                if (Input.GetAxisRaw("XRI_Left_PrimaryButton") > 0f)
                {
                    transform.position = targetPos2;
                    transform.eulerAngles = new Vector3(0, -90, 0);
                    trans2 = false;
                    isZ = true;
                    HintText.text = "";

                }
            }
        }
        if (trans3)
        {
            if (posAndrotIsTrue3(player.transform.position, Camera.main.transform.eulerAngles))
            {
                if ((Input.GetAxisRaw("XRI_Left_PrimaryButton") > 0f))
                {
                    transform.position = targetPos3;
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    trans3 = false;
                    isZ = false;
                    HintText.text = "";

                }
            }
        }

    }
    bool posAndrotIsTrue(Vector3 p, Vector3 e)
    {
        print("正在判定");
        //if ((p.x < (pos.x + 2)) && (p.x > (pos.x - 2)) && (p.z < (pos.z + 2)) && (p.z > (pos.z - 2)))
        if (CircleDistance(p, pos1, 2))
        {
            HintText.text = "Walk around :)";
            print("Pos");
            if ( (e.y < (euler1.y + 15f)) && (e.y > (euler1.y - 15f)))
            {
                HintText.text = "press X to move the cat";
                print("View");
                return true;
            }
        }
        else
        {
            HintText.text = "Walk around :)";
            return false;
        }
        return false;
    }
    bool posAndrotIsTrue2(Vector3 p, Vector3 e)
    {
        print("正在判定");
        //if ((p.x < (pos2.x + 2)) && (p.x > (pos2.x - 2)) && (p.z < (pos2.z + 2)) && (p.z > (pos2.z - 2)))
        if (CircleDistance(p,pos2,2))
        {
            print("Pos2");
            HintText.text = "Walk around :)";
            if ( (e.x < (euler2.x + 15f)) && (e.x > (euler2.x - 15f)))
            {
                print("View");
                HintText.text = "press X";
                return true;
            }
        }
        else
        {
            HintText.text = "Walk around :)";
            return false;
        }
        return false;
    }
    bool posAndrotIsTrue3(Vector3 p, Vector3 e)
    {
        print("正在判定");
        if (CircleDistance(p, pos3, 2))
        {
            print("Pos3");
            HintText.text = "Walk around :)";
            if ((e.x < (euler3.x + 15f)) && (e.x > (euler3.x - 15f)))
            {
                print("View");
                HintText.text = "press X";
                return true;
            }
        }
        else
        {
            HintText.text = "Walk around :)";
            return false;
        }
        return false;
    }

    //旋转与位移的碰撞检测
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "111")
        {
            //调整box的旋转方向
            fbx.transform.DORotate(new Vector3(-90, 0, 0), 1f);
            //调整猫旋转后的朝向
            transform.DORotate(new Vector3(0, 180, 0), 1f);
            Destroy(collision.gameObject);
            Camera.main.transform.localPosition = new Vector3(0, 0.7f, 0);
        }
        if (collision.gameObject.tag == "222")
        {
            trans1 = true;
        }
        if (collision.gameObject.tag == "333")
        {
            trans2 = true;
        }
        if (collision.gameObject.tag == "444")
        {
            fbx.transform.DORotate(new Vector3(0,-90,90), 0.1f);
            transform.DORotate(new Vector3(0, 270, 0), 1f);
            transform.position = rotatePos2;
            Destroy(collision.gameObject);
            Camera.main.transform.localPosition = new Vector3(0, 0.7f, 0);
        }
        if (collision.gameObject.tag == "555")
        {
            trans3 = true;
        }
    }
    public bool CircleDistance(Vector3 attacked, Vector3 skillPosition, float radius)
    {
        float distance = Vector3.Distance(attacked, skillPosition);
        if (distance < radius)
        {
            return true;
        }
        return false;
    }
}

