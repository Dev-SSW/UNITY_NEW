using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeController : CloseWeaponController
{
    //활성화 여부
    public static bool isActivate = true;

    private void Start()
    {
        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentCloseWeapon.anim;
    }
    void Update()
    {
        if (isActivate)
            TryAttack();

    }

    protected override IEnumerator HitCoroutine()
    {
        {
            while (isSwing) //팔을 접을때까지
            {
                if (CheckObject())
                {
                    //충돌했음
                    if(hitinfo.transform.tag == "Rock")
                    {
                        hitinfo.transform.GetComponent<Rock>().Mining();
                    }
                    else if (hitinfo.transform.tag == "WeakAnimal")
                    {
                        SoundManager.instance.PlaySE("Animal_Hit");
                        hitinfo.transform.GetComponent<WeakAnimal>().Damage(1, transform.position);
                    }
                    isSwing = false;
                    Debug.Log(hitinfo.transform.name); //충돌 했으면 이름을 가져옴
                }
                yield return null;

            }
        }
    }

    public override void CloseWeaponChange(CloseWeapon _closehand)
    {
        base.CloseWeaponChange(_closehand);
        isActivate = true;
    }
}