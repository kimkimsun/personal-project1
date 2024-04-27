# 첫 개인프로젝트
> ### **개발 환경**

![Windows](https://img.shields.io/badge/Windows-0078D6?style=for-the-badge&logo=windows&logoColor=white)
![Unity](https://img.shields.io/badge/unity-%23000000.svg?style=for-the-badge&logo=unity&logoColor=white)
![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=csharp&logoColor=white)
![Visual Studio](https://img.shields.io/badge/Visual%20Studio-5C2D91.svg?style=for-the-badge&logo=visual-studio&logoColor=white)
# 프로젝트 특징
#### 약 1달간에 유니티 공부를 한 뒤, 시작한 첫 개인 프로젝트 입니다.
# 게임 특징
#### 컴퓨터 한대로 하는 2인용 게임입니다.
#### 상호작용키를 다르게 설정하여 협동하면서 플레이하는 2D 퍼즐 플랫포머 게임입니다.
# 게임 영상 (이미지 클릭)
[![IMAGE ALT TEXT HERE](https://img.youtube.com/vi/H5aW5KRRN3I/0.jpg)](https://youtu.be/H5aW5KRRN3I)
# **핵심 기능**
## 첫 번째 기능
### 기능설명
1. 플레이어 1은 몬스터와의 충돌이 일어나면 HP가 닳습니다.
2. 그와 반대로 플레이어 2는 몬스터와의 충돌이 일어나도 아무렇지 않으며, 몬스터의 머리를 밟을 경우, 몬스터의 HP를 닳게 합니다.
3. 즉, 몬스터는 피해를 줄 수 있으며, 받을 수도 있습니다.

### 다이어그램
![갠프 다이어그램](https://github.com/kimkimsun/1st-TeamProject/assets/116052108/96ed0980-276d-4112-8f0e-55af3e67149f)
##### 위와 같은 구조로 이루어져있습니다.

##### 확장성을 고려하여 인터페이스로 상속을 받았고 객체지향으로서의 유연성을 고려하였습니다.

<details>
    <summary>코드</summary>
    
### 코드
```C#
public interface IAttackable
{
    void Attack(IHitable hitobj);
    float Damage
    {
        get;
        set;
    }
}
public interface IHitable
{
    void Hit(float damage);
}
```

```C#
public class Player : MonoBehaviour, IReversalObj, IHitable
{
    public virtual void Hit(float damage)
    {
        playerRb.AddRelativeForce(Vector2.right * 5 * Time.deltaTime, ForceMode2D.Impulse);
    }
}

public class Monster : MonoBehaviour, IAttackable, IHitable
{
    public void Attack(IHitable hitobj)
    {
        hitobj.Hit(damage);
    }

    public void Hit(float damage)
    {
        Hp -= damage;
        gameObject.layer = hitLayer;
        gameObject.transform.GetChild(0).gameObject.layer = hitLayer;
        monsterRb.AddRelativeForce(Vector2.up * 3, ForceMode2D.Impulse);
        monsterRb.AddRelativeForce(Vector2.left * 3, ForceMode2D.Impulse);
        StartCoroutine(ChangeColor());
    }
}
```
</details>


## 두 번째 기능
### 기능 설명
1. 이 게임에 목적은 2인용 협동 게임이기 때문에 중력반전이라는 요소를 넣었습니다.
2. 플레이어들은 IReversalObj라는 인터페이스를 상속받고 있으며, 중력반전이 되는 오브젝트는 IReversal이라는 인터페이스를 상속받고 있습니다.
3. 중력반전이 되는 오브젝트 입장에서 IReversalObj와의 충돌이 일어나면 그 충돌대상이 중력이 반전되게끔 Reversal이라는 함수를 실행시키게 하였습니다.
4. 확장성을 고려하여 추후에 플레이어가 아닌 상자나 다른 오브젝트를 중력 반전을 시켜 풀어야 하는 퍼즐이 있을시에 유용하게 작동하도록 하였습니다.

### 사진 설명
![갠프 사진](https://github.com/kimkimsun/1st-TeamProject/assets/116052108/2317f560-7450-4755-8daa-7f522d35d05b)

### 다이어그램
![갠프 중력반전 다이어그램](https://github.com/kimkimsun/1st-TeamProject/assets/116052108/4f24cf5f-8755-475a-8ce0-ee81e0ceb61e)
##### 위와 같은 구조로 이루어져있습니다.

<details>
    <summary>코드</summary>
    
### 코드
```C#
public interface IReversalAble
{
    void Reversal(IReversalObj reversalObj);
}

public interface IReversalObj
{
    void ReversalObj();
}

```

```C#
public class ReversalZone : MonoBehaviour, IReversalAble
{
    public void Reversal(IReversalObj reversalObj)
    {
        reversalObj.ReversalObj();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out IReversalObj obj))
            Reversal(obj);
    }
}

public class Player : MonoBehaviour, IReversalObj, IHitable
{
    public void ReversalObj()
    {
        playerMove.IsGravity = !playerMove.IsGravity;
        transform.localScale = new Vector2(transform.localScale.x, (transform.localScale.y * -1));
        playerRb.gravityScale *= -1;
    }
}
```
</details>

## 세 번째 기능
### 기능설명
1. 플레이어는 Inventory를 가지고 있고 Inventory는 Slot을 가지고 있습니다.
2. 만약 플레이어가 Item을 먹을 경우 AddItem 함수가 실행됩니다.
3. 만약 Inventory가 갖고있는 Slot중에 빈 Slot이 있다면 for문을 실행시켜 아이템이 알아서 자리매김을 합니다.
### 코드
```C#
public class Inventory : MonoBehaviour
{
    public Slot[] slot = new Slot[5];
    public void AddItem(Item iitem)
    {
        for (int i = 0; i < slot.Length; i++)
        {
            if (slot[i].item == null) 
            {
                slot[i].SetItem(iitem);
                return;
            }
        }
    }
}

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void SetItem(Item iitem)
    {
        item = iitem;      
        if(item == null)
            image.sprite = null;
        else
        {
            image.sprite = item.sprite;
            itemExplanation.text = item.explanation;
            item.owenrSlotImage = image;
        }
    }
}
```
#### 첫 팀프로젝트때의 난잡했던 인벤토리(퀵 슬롯)를 간편하게 구현하였습니다.
#### 이 기능은 쉬운 코드로 구현하였기 때문에 클래스 다이어그램 보다는 코드로써 보여드렸습니다.
