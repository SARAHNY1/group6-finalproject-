using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Cat : MonoBehaviour
{
    public float speed;
    public GameObject fbx;
    GameObject player;
    //�Զ�����λ��Ŀ��λ�ú��ж���Χ
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
    //�Ƿ���ת
    bool isZ;
    public Vector3 rotatePos1;
    public Vector3 rotatePos2;

    public Text HintText;

    private Animation anim;

    void Start()
    {
        print(GameObject.FindGameObjectsWithTag("222").Length);//
        player = GameObject.FindGameObjectWithTag("Player");
        trans1 = false;
        trans2 = false;
        isZ = false;
        anim = gameObject.GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        //QE��������
        if (Input.GetAxisRaw("XRI_Right_SecondaryButton") > 0f)
        {
            //anim.Play("cu_cat@A_walk");
            if (isZ)
            {
                transform.Translate(transform.right * -1f * speed * Time.deltaTime);
                anim.Play("cu_cat@A_walk");
            }
            else
                transform.Translate(transform.forward * -1f * speed * Time.deltaTime);
                anim.Play("cu_cat@A_walk");
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
        //�������λ���ӽǺ�è��λ��
        if (Input.GetMouseButtonDown(0))
            print("���λ�ã�" + player.transform.position + "����ӽǣ�" + Camera.main.transform.eulerAngles + "Catλ��" + transform.position);
        //λ��
//        if (trans1)
//        {
//            if (posAndrotIsTrue(player.transform.position, Camera.main.transform.eulerAngles))
//            {
//                if (Input.GetKeyDown(KeyCode.V))
//                {
//                    transform.position = targetPos1;
//                    //����èλ�ƺ�ĳ���
//                    transform.eulerAngles = new Vector3(0, 90, 0);
//                    trans1 = false;
//                    isZ = true;
//                    HintText.text = "";
//                }
//            }
//        }
//        if (trans2)
//       {
//            if (posAndrotIsTrue2(player.transform.position, Camera.main.transform.eulerAngles))
//            {
//                if (Input.GetKeyDown(KeyCode.V))
//                {
//                    transform.position = targetPos2;
//                   transform.eulerAngles = new Vector3(0, -90, 0);
//                    trans2 = false;
//                    isZ = true;
//                   HintText.text = "";

//                }
//            }
//        }
//        if (trans3)
//        {
//            if (posAndrotIsTrue3(player.transform.position, Camera.main.transform.eulerAngles))
//            {
//                if (Input.GetKeyDown(KeyCode.V))
//                {
//                    transform.position = targetPos3;
//                    transform.eulerAngles = new Vector3(0, 0, 0);
//                    trans3 = false;
//                    isZ = false;
//                    HintText.text = "";
//
//                }
//            }
//        }

    }
    bool posAndrotIsTrue(Vector3 p, Vector3 e)
    {
        print("�����ж�");
        //if ((p.x < (pos.x + 2)) && (p.x > (pos.x - 2)) && (p.z < (pos.z + 2)) && (p.z > (pos.z - 2)))
        if (CircleDistance(p, pos1, 2))
        {
            HintText.text = "Walk around :)";
            print("Pos");
            if ((e.y < (euler1.y + 15f)) && (e.y > (euler1.y - 15f)))
            {
                HintText.text = "press V to move the cat";
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
        print("�����ж�");
        //if ((p.x < (pos2.x + 2)) && (p.x > (pos2.x - 2)) && (p.z < (pos2.z + 2)) && (p.z > (pos2.z - 2)))
        if (CircleDistance(p, pos2, 2))
        {
            print("Pos2");
            HintText.text = "Walk around :)";
            if ((e.x < (euler2.x + 15f)) && (e.x > (euler2.x - 15f)))
            {
                print("View");
                HintText.text = "press V";
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
        print("�����ж�");
        if (CircleDistance(p, pos3, 2))
        {
            print("Pos3");
            HintText.text = "Walk around :)";
            if ((e.x < (euler3.x + 15f)) && (e.x > (euler3.x - 15f)))
            {
                print("View");
                HintText.text = "press V";
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

    //��ת��λ�Ƶ���ײ���
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "111")
        {
            //����box����ת����
            fbx.transform.DORotate(new Vector3(-90, 0, 0), 1f);
            //����è��ת��ĳ���
            transform.DORotate(new Vector3(0, 180, 0), 1f);
            Destroy(collision.gameObject);
            Camera.main.transform.localPosition = new Vector3(0, 0.7f, 0);
            //fbx.transform.position = new Vector3(0f, 0f, 0f);
        }
        if (collision.gameObject.tag == "222")
        {
            transform.position = targetPos1;
            //����èλ�ƺ�ĳ���
            transform.eulerAngles = new Vector3(0, 90, 0);
            trans1 = false;
            isZ = true;
            HintText.text = "";
        }
        if (collision.gameObject.tag == "333")
        {
            transform.position = targetPos2;
            transform.eulerAngles = new Vector3(0, -90, 0);
            trans2 = false;
            isZ = true;
            HintText.text = "";
        }
        if (collision.gameObject.tag == "444")
        {
            fbx.transform.DORotate(new Vector3(0, -90, 90), 0.1f);
            transform.DORotate(new Vector3(0, 270, 0), 1f);
            transform.position = rotatePos2;
            Destroy(collision.gameObject);
            Camera.main.transform.localPosition = new Vector3(0, 0.7f, 0);
        }
        if (collision.gameObject.tag == "555")
        {
                                transform.position = targetPos3;
                                transform.eulerAngles = new Vector3(0, 0, 0);
                                trans3 = false;
                               isZ = false;
                                HintText.text = "";
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

