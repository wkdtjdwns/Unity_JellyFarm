using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Jelly : MonoBehaviour
{
    // 젤리에 대한 변수들
    public int id;                         // 젤리의 id
    public int level;                      // 젤리의 레벨
    public float exp;                      // 현재 경험치
    public float required_exp;             // 레벨업에 필요한 경험치
    public float max_exp;                  // 최대 경험치

    // GameManager에 대한 변수들
    public GameObject game_manager_obj;    // GameManager와 상호 작용하기 위한 변수
    public GameManager game_manager;       // GameManager에서 관리하는 여러 값들을 활용하기 위해 객체를 불러옴

    // 맵의 범위를 책임지는 변수들
    public GameObject left_top;            // 왼쪽 위 오브젝트
    public GameObject right_bottom;        // 오른쪽 아래 오브젝트

    // 게임에 대한 변수들
    public SpriteRenderer sprite_renderer; // 스프라이트를 렌더링 하는 변수
    public Animator anim;                  // 애니매이션을 불러오는 변수

    float pick_time;                       // 젤리를 잡고 있는 시간

    int move_delay;                        // 이동과 이동 사이의 딜레이
    int move_time;                         // 이동하는 시간

    float speed_x;                         // x축 방향 이동 속도
    float speed_y;                         // y축 방향 이동 속도

    bool isWandering;                      // 멈춰있는지 여부
    bool isWalking;                        // 움직이는지 여부

    GameObject shadow;                     // 젤리의 그림자
    float shadow_pos_y;                    // 그림자의 y좌표 (위치)

    int jelatin_delay;                     // 자동으로 얻는 젤라틴의 딜레이
    bool isGetting;                        // 젤라틴을 얻었는지 여부

    void Awake()
    {
        left_top = GameObject.Find("LeftTop").gameObject;              // 왼쪽 위 오브젝트를 찾아낸 뒤 변수에 저장함
        right_bottom = GameObject.Find("RightBottom").gameObject;      // 오른쪽 아래 오브젝트를 찾아낸 뒤 변수에 저장함
        game_manager_obj = GameObject.Find("GameManager").gameObject;  // GameManager 오브젝트를 찾아서 game_manager_obj에 저장함
        game_manager = game_manager_obj.GetComponent<GameManager>();   // game_manager 변수가 GameManager의 컴퍼넌트를 가지게 함

        sprite_renderer = GetComponent<SpriteRenderer>();              // sprite 변수가 SpriteRenderer의 컴퍼넌트를 가지게 함
        anim = GetComponent<Animator>();                               // anim 변수가 Animator의 컴퍼넌트를 가지게 함

        // 변수들 초기화
        isWandering = false;
        isWalking = false;
        isGetting = false;
        // 변수들 초기화

        shadow = transform.Find("Shadow").gameObject; // 게임 오브젝트 중 "Shadow"라는 오브젝트를 찾아서 변수 shadow에 저장함 (젤리 오브젝트의 자식 오브젝트로 존재함)
        switch (id)                                   // 젤리의 종류에 따라서 그림자의 위치가 달라짐
        {   
            // 그림자의 위치를 바꿈            
            case 0: shadow_pos_y = -0.05f; break;     
            case 6: shadow_pos_y = -0.12f; break;
            case 3: shadow_pos_y = -0.14f; break;
            case 10: shadow_pos_y = -0.16f; break;
            case 11: shadow_pos_y = -0.16f; break;
            default: shadow_pos_y = -0.05f; break;
        }
        // 그림자는 젤리 오브젝트 안에 (자식 오브젝트)있기 때문에 localPosition으로 위치를 조정함
        shadow.transform.localPosition = new Vector3(0, shadow_pos_y, 0); // 젤리의 그림자 위치를 새로 정한 y좌표로 이동시킴 (y좌표만 이동시켜야 해서 다른 값들은 0임)
    }

    void Update() // 매 프라임마다
    {
        if(exp < max_exp)          // 현재 경험치가 최대 경험치를 넘지 않았으면
            exp += Time.deltaTime; // exp 변수에 초당 1씩 더해지면서 시간이 지남에 따라 자연스럽게 레벨이 상승함

        if (exp > required_exp * level && level < 3) // 레벨에 비례하여 레벨 업에 요구되는 exp의 크기를 증가시킴
            game_manager.ChangeAc(anim, ++level);    // 젤리의 크기 애니매이션을 바꾸고 (크기 커짐) 레벨을 증가시킴

        if (!isGetting)                              // 젤라틴을 지금 얻지 않았다면
            StartCoroutine(GetJelatin());            // 코루틴 함수를 실행함
            
    }

    void FixedUpdate() // 일정한 간격마다
    {
        if (!isWandering)             // 멈춰 있으면
            StartCoroutine(Wander()); // 코루틴 함수 실행

        if (isWalking)                // 움직임의 여부가 결정되면
            Move();                   // 움직임

        float pos_x = transform.position.x;
        float pos_y = transform.position.y;

        // 맵의 범위 밖으로 나가지 못하게 함
        if (pos_x < left_top.transform.position.x || pos_x > right_bottom.transform.position.x) // 젤리의 x좌표가 LeftTop의 x좌표보다 낮거나 (왼쪽에 있거나) RightBottom의 x좌표보다 높으면 (오른쪽에 있으면)
            speed_x = -speed_x;                                                                 // x좌표를 반대로 함

        if (pos_y > left_top.transform.position.y || pos_y < right_bottom.transform.position.y) // 젤리의 y좌표가 LeftTop의 y좌표보다 높거나 (위에 있거나) RightBottom의 y좌표보다 낮으면 (아래에 있으면)
            speed_y = -speed_y;                                                                 // y좌표를 반대로 함
    }

    void OnMouseDown() // 젤리를 클릭하고 떼는 순간
    {
        if (!game_manager.isLive) return;         // 게임이 멈춰 있지 않으면 실행 (멈춰 있으면 실행 X)

        isWalking = false;                        // 움직임을 멈추고
        anim.SetBool("isWalk", false);            // 이동 애니메이션도 멈춘 뒤
        anim.SetTrigger("doTouch");               // 클릭 애니매이션을 실행한 다음

        if(exp < max_exp) ++exp;                  // 현재 경험치가 최대 경험치를 넘지 않았으면 경험치가 1씩 증가하며

        game_manager.GetJelatin(id, level);       // GameManager에서 관리하는 젤리틴을 얻는 함수를 호출 (불러옴)해서 id와 level에 비례해 젤라틴을 획득함

        SoundManager.instance.PlaySound("Get Jelatin"); // 터치 하는 사운드 출력
    }

    void OnMouseDrag() // 젤리를 드래그 할 시
    {
        if (!game_manager.isLive) return; // 게임이 멈춰 있지 않으면 실행 (멈춰 있으면 실행 X)

        pick_time += Time.deltaTime;      // 잡고 있는 시간을 초당 1씩 증가

        if (pick_time < 0.2f) return;     // 만약 잡고 있는 시간이 0.2초 보다 작으면 단순 클릭으로 판단함

                                          // 만약 단순 클릭이 아닌 드래그로 판단 할 시
        isWalking = false;                // 이동을 멈추고
        anim.SetBool("isWalk", false);    // 이동 애니매이션도 멈춘 뒤
        anim.SetTrigger("doTouch");       // 만졌을 때 애니매이션 실행

        Vector3 mouse_pos = Input.mousePosition;                                                            // 마우스의 위치 변수 (mouse_pos)

        // ScreenToWorldPoint를 통해 마우스의 위치를 월드 좌표계로 변경
        // 인터페이스가 있는 곳은 스크린 좌표계 / 실제 도트 이미지가 있는 곳은 월드 자표계
        Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(mouse_pos.x, mouse_pos.y, mouse_pos.y)); // x, y, z 좌표를 마우스의 위치에 따라서 결정하고 그 값을 저정할 변수 (point)

        transform.position = point;                                                                         // 마우스로 젤리를 드래그하고 있으면 젤리가 마우스를 따라감
    }

    void OnMouseUp() // 젤리를 클릭했을 때
    {
        if (!game_manager.isLive) return;          // 게임이 멈춰 있지 않으면 실행 (멈춰 있으면 실행 X)

        pick_time = 0;                             // 잡고 있는 시간 초기화

        if (game_manager.isSell)                   // 젤리를 든 채로 마우스 포인터를 판매 버튼 위로 옮길 시 GameManager의 isSell 변수는 True로 바뀌게 되고
        {
            game_manager.GetGold(id, level, this); // 그 상태에서 젤리를 놓을 경우 GetGold() 함수를 불러옴(호출)으로써 골드를 얻음과 동시에

            Destroy(gameObject);                   // 젤리 오브젝트는 Destroy() 함수에 의해 사라짐
        }

        float pos_x = transform.position.x;
        float pos_y = transform.position.y;

        
        if (pos_x < left_top.transform.position.x || pos_x > right_bottom.transform.position.x || // 만약 드래그 후, 젤리를 놓았을 때 위치가 맵의 범위 밖이라면
            pos_y > left_top.transform.position.y || pos_y < right_bottom.transform.position.y)   // 만약 드래그 후, 젤리를 놓았을 때 위치가 맵의 범위 밖이라면

            transform.position = new Vector3(0, -1, 0);                                           // 맵의 중앙으로 이동  
    }

    void Move()                                         // Jelly의 움직임을 구현하기 위한 Move() 함수, 코루틴 함수 (Wander())를 추가        // 코루틴 함수: 시간의 경과에 따른 명령을 주고싶을 때 사용하게 되는 함수
    {
        if (speed_x != 0)
            sprite_renderer.flipX = speed_x < 0;        // x축 속도에 따라 젤리의 이미지를 뒤집은 뒤

        transform.Translate(speed_x, speed_y, speed_y); // 젤리 이동
    }

    IEnumerator Wander() // 코루틴 함수  /  이동하기
    {
        move_delay = Random.Range(3, 6); // 이동과 이동 사이의 딜레이 (3 ~ 5초)
        move_time = Random.Range(3, 6);  // 이동시간 (3 ~ 5초)
        
        // Translate로 이동할 시 Object가 텔레포트 하는 것을 방지하기 위해 Time.deltaTime을 곱해줌
        speed_x = Random.Range(-0.8f, 0.8f) * Time.deltaTime;
        speed_y = Random.Range(-0.8f, 0.8f) * Time.deltaTime;

        isWandering = true;                          // 멈춰 있으면                                           -----|
//                                                                                                                 |
        yield return new WaitForSeconds(move_delay); // 위에서 정해진 이동과 이동 사이의 딜레이만큼 기다리고  -----|
//                                                                                                                 |
        isWalking = true;                            // 시간이 지나 움직이기 시작하면                         -----|
        anim.SetBool("isWalk", true);                // 이동 애니메이션 실행하고                              -----|
//                                                                                                                 |--------------- 이 모든 과정을 반복함
        yield return new WaitForSeconds(move_time);  // 이동시간만큼 기다린 뒤                                -----|
//                                                                                                                 |
        isWalking = false;                           // 움직임이 멈추며                                       -----|
        anim.SetBool("isWalk", false);               // 이동 애니메이션 종료 한 뒤                            -----|
//                                                                                                                 |
        isWandering = false;                         // 멈춤                                                  -----|
    }

    IEnumerator GetJelatin() // 코루틴 함수  //  자동으로 젤라틴 얻기
    {
        jelatin_delay = 3;                              // 3초마다                                                  -----|
//                                                                                                                       |
        isGetting = true;                               // 젤라틴 획득 여부를 true로 바꾸고                         -----|
        game_manager.GetJelatin(id, level);             // id와 level에 비례해서 젤라틴을 획득한 뒤에               -----|--------------- 이 모든 과정을 반복함
//                                                                                                                       |
        yield return new WaitForSeconds(jelatin_delay); // 위에 있는 자동으로 얻는 젤라틴의 딜레이만큼 기다린 다음  -----|
//                                                                                                                       |
        isGetting = false;                              // 젤라틴 획득 여부를 false로 바꿈                          -----|
    }
}