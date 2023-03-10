using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Craft
{
    public string craftName; //이름
    public GameObject go_Prefab; //실제 설치될 프리팹
    public GameObject go_PreviewPrefab; //미리보기 프리팹
}

public class CraftManual : MonoBehaviour
{
    //상태변수
    private bool isActivated = false;
    private bool isPreviewActivated = false;
    
    [SerializeField]
    private GameObject go_BaseUI; //기본 베이스 UI

    [SerializeField]
    private Craft[] craft_fire; //모닥불용 탭

    private GameObject go_preview; //미리보기 프리팹을 담을 변수
    private GameObject go_Prefab; //실제 생성될 프리팹을 담은 변수


    [SerializeField]
    private Transform tf_Player; //플레이어 위치

    //raycast필요 변수
    private RaycastHit hitInfo;
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private float range;


    public void SlotClick(int _slotNumber)
    {
        GameManager.isBuilding = true;
        go_preview = Instantiate(craft_fire[_slotNumber].go_PreviewPrefab, tf_Player.position + tf_Player.forward, Quaternion.identity);
        go_Prefab = craft_fire[_slotNumber].go_Prefab;
        isPreviewActivated  = true;
        go_BaseUI.SetActive(false); //슬롯창 잠깐 끄기
        GameManager.isOpenCraftManual = false;
    }
    
    
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !isPreviewActivated)
        {
            Window();
        }
        if (isPreviewActivated)
        {
            PreviewPositionUpdate();
        }
        if (Input.GetButtonDown("Fire1"))
        {
            
            Build();
            
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cancle();
        }
    }
    private void Build()
    {
        if (isPreviewActivated && go_preview.GetComponent<PreviewObject>().isBuildable())
        {
            Instantiate(go_Prefab, hitInfo.point,Quaternion.identity);
            Destroy(go_preview);
            isActivated = false;
            isPreviewActivated = false;
            go_preview = null;
            go_Prefab = null;
            go_BaseUI.SetActive(false);
            GameManager.isBuilding = false;
        }
    }

    private void PreviewPositionUpdate()
    {
        if(Physics.Raycast(tf_Player.position,tf_Player.forward,out hitInfo,range ,layerMask)) 
        {
            if(hitInfo.transform != null)
            {
                Vector3 _location = hitInfo.point;
                go_preview.transform.position = _location;
            }
        }
    }

    private void Cancle()
    {
        if (isPreviewActivated)
        {
            Destroy(go_preview);
        }
        isActivated = false;
        isPreviewActivated = false;
        go_preview = null;
        go_BaseUI.SetActive(false);
        GameManager.isOpenCraftManual = false;
    }

    private void Window()
    {
        isActivated = !isActivated;
        if (isActivated)
        {
            OpenWindow();
        }
        else
        {
            CloseWindow();
        }
    } 
    private void OpenWindow()
    {
        GameManager.isOpenCraftManual = true;
        go_BaseUI.SetActive(true);
    }
    private void CloseWindow()
    {
        GameManager.isOpenCraftManual = false;     
        go_BaseUI.SetActive(false);
    }

}
