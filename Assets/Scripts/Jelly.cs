using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Jelly : MonoBehaviour
{
    // ������ ���� ������
    public int id;                         // ������ id
    public int level;                      // ������ ����
    public float exp;                      // ���� ����ġ
    public float required_exp;             // �������� �ʿ��� ����ġ
    public float max_exp;                  // �ִ� ����ġ

    // GameManager�� ���� ������
    public GameObject game_manager_obj;    // GameManager�� ��ȣ �ۿ��ϱ� ���� ����
    public GameManager game_manager;       // GameManager���� �����ϴ� ���� ������ Ȱ���ϱ� ���� ��ü�� �ҷ���

    // ���� ������ å������ ������
    public GameObject left_top;            // ���� �� ������Ʈ
    public GameObject right_bottom;        // ������ �Ʒ� ������Ʈ

    // ���ӿ� ���� ������
    public SpriteRenderer sprite_renderer; // ��������Ʈ�� ������ �ϴ� ����
    public Animator anim;                  // �ִϸ��̼��� �ҷ����� ����

    float pick_time;                       // ������ ��� �ִ� �ð�

    int move_delay;                        // �̵��� �̵� ������ ������
    int move_time;                         // �̵��ϴ� �ð�

    float speed_x;                         // x�� ���� �̵� �ӵ�
    float speed_y;                         // y�� ���� �̵� �ӵ�

    bool isWandering;                      // �����ִ��� ����
    bool isWalking;                        // �����̴��� ����

    GameObject shadow;                     // ������ �׸���
    float shadow_pos_y;                    // �׸����� y��ǥ (��ġ)

    int jelatin_delay;                     // �ڵ����� ��� ����ƾ�� ������
    bool isGetting;                        // ����ƾ�� ������� ����

    void Awake()
    {
        left_top = GameObject.Find("LeftTop").gameObject;              // ���� �� ������Ʈ�� ã�Ƴ� �� ������ ������
        right_bottom = GameObject.Find("RightBottom").gameObject;      // ������ �Ʒ� ������Ʈ�� ã�Ƴ� �� ������ ������
        game_manager_obj = GameObject.Find("GameManager").gameObject;  // GameManager ������Ʈ�� ã�Ƽ� game_manager_obj�� ������
        game_manager = game_manager_obj.GetComponent<GameManager>();   // game_manager ������ GameManager�� ���۳�Ʈ�� ������ ��

        sprite_renderer = GetComponent<SpriteRenderer>();              // sprite ������ SpriteRenderer�� ���۳�Ʈ�� ������ ��
        anim = GetComponent<Animator>();                               // anim ������ Animator�� ���۳�Ʈ�� ������ ��

        // ������ �ʱ�ȭ
        isWandering = false;
        isWalking = false;
        isGetting = false;
        // ������ �ʱ�ȭ

        shadow = transform.Find("Shadow").gameObject; // ���� ������Ʈ �� "Shadow"��� ������Ʈ�� ã�Ƽ� ���� shadow�� ������ (���� ������Ʈ�� �ڽ� ������Ʈ�� ������)
        switch (id)                                   // ������ ������ ���� �׸����� ��ġ�� �޶���
        {   
            // �׸����� ��ġ�� �ٲ�            
            case 0: shadow_pos_y = -0.05f; break;     
            case 6: shadow_pos_y = -0.12f; break;
            case 3: shadow_pos_y = -0.14f; break;
            case 10: shadow_pos_y = -0.16f; break;
            case 11: shadow_pos_y = -0.16f; break;
            default: shadow_pos_y = -0.05f; break;
        }
        // �׸��ڴ� ���� ������Ʈ �ȿ� (�ڽ� ������Ʈ)�ֱ� ������ localPosition���� ��ġ�� ������
        shadow.transform.localPosition = new Vector3(0, shadow_pos_y, 0); // ������ �׸��� ��ġ�� ���� ���� y��ǥ�� �̵���Ŵ (y��ǥ�� �̵����Ѿ� �ؼ� �ٸ� ������ 0��)
    }

    void Update() // �� �����Ӹ���
    {
        if(exp < max_exp)          // ���� ����ġ�� �ִ� ����ġ�� ���� �ʾ�����
            exp += Time.deltaTime; // exp ������ �ʴ� 1�� �������鼭 �ð��� ������ ���� �ڿ������� ������ �����

        if (exp > required_exp * level && level < 3) // ������ ����Ͽ� ���� ���� �䱸�Ǵ� exp�� ũ�⸦ ������Ŵ
            game_manager.ChangeAc(anim, ++level);    // ������ ũ�� �ִϸ��̼��� �ٲٰ� (ũ�� Ŀ��) ������ ������Ŵ

        if (!isGetting)                              // ����ƾ�� ���� ���� �ʾҴٸ�
            StartCoroutine(GetJelatin());            // �ڷ�ƾ �Լ��� ������
            
    }

    void FixedUpdate() // ������ ���ݸ���
    {
        if (!isWandering)             // ���� ������
            StartCoroutine(Wander()); // �ڷ�ƾ �Լ� ����

        if (isWalking)                // �������� ���ΰ� �����Ǹ�
            Move();                   // ������

        float pos_x = transform.position.x;
        float pos_y = transform.position.y;

        // ���� ���� ������ ������ ���ϰ� ��
        if (pos_x < left_top.transform.position.x || pos_x > right_bottom.transform.position.x) // ������ x��ǥ�� LeftTop�� x��ǥ���� ���ų� (���ʿ� �ְų�) RightBottom�� x��ǥ���� ������ (�����ʿ� ������)
            speed_x = -speed_x;                                                                 // x��ǥ�� �ݴ�� ��

        if (pos_y > left_top.transform.position.y || pos_y < right_bottom.transform.position.y) // ������ y��ǥ�� LeftTop�� y��ǥ���� ���ų� (���� �ְų�) RightBottom�� y��ǥ���� ������ (�Ʒ��� ������)
            speed_y = -speed_y;                                                                 // y��ǥ�� �ݴ�� ��
    }

    void OnMouseDown() // ������ Ŭ���ϰ� ���� ����
    {
        if (!game_manager.isLive) return;         // ������ ���� ���� ������ ���� (���� ������ ���� X)

        isWalking = false;                        // �������� ���߰�
        anim.SetBool("isWalk", false);            // �̵� �ִϸ��̼ǵ� ���� ��
        anim.SetTrigger("doTouch");               // Ŭ�� �ִϸ��̼��� ������ ����

        if(exp < max_exp) ++exp;                  // ���� ����ġ�� �ִ� ����ġ�� ���� �ʾ����� ����ġ�� 1�� �����ϸ�

        game_manager.GetJelatin(id, level);       // GameManager���� �����ϴ� ����ƾ�� ��� �Լ��� ȣ�� (�ҷ���)�ؼ� id�� level�� ����� ����ƾ�� ȹ����

        SoundManager.instance.PlaySound("Get Jelatin"); // ��ġ �ϴ� ���� ���
    }

    void OnMouseDrag() // ������ �巡�� �� ��
    {
        if (!game_manager.isLive) return; // ������ ���� ���� ������ ���� (���� ������ ���� X)

        pick_time += Time.deltaTime;      // ��� �ִ� �ð��� �ʴ� 1�� ����

        if (pick_time < 0.2f) return;     // ���� ��� �ִ� �ð��� 0.2�� ���� ������ �ܼ� Ŭ������ �Ǵ���

                                          // ���� �ܼ� Ŭ���� �ƴ� �巡�׷� �Ǵ� �� ��
        isWalking = false;                // �̵��� ���߰�
        anim.SetBool("isWalk", false);    // �̵� �ִϸ��̼ǵ� ���� ��
        anim.SetTrigger("doTouch");       // ������ �� �ִϸ��̼� ����

        Vector3 mouse_pos = Input.mousePosition;                                                            // ���콺�� ��ġ ���� (mouse_pos)

        // ScreenToWorldPoint�� ���� ���콺�� ��ġ�� ���� ��ǥ��� ����
        // �������̽��� �ִ� ���� ��ũ�� ��ǥ�� / ���� ��Ʈ �̹����� �ִ� ���� ���� ��ǥ��
        Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(mouse_pos.x, mouse_pos.y, mouse_pos.y)); // x, y, z ��ǥ�� ���콺�� ��ġ�� ���� �����ϰ� �� ���� ������ ���� (point)

        transform.position = point;                                                                         // ���콺�� ������ �巡���ϰ� ������ ������ ���콺�� ����
    }

    void OnMouseUp() // ������ Ŭ������ ��
    {
        if (!game_manager.isLive) return;          // ������ ���� ���� ������ ���� (���� ������ ���� X)

        pick_time = 0;                             // ��� �ִ� �ð� �ʱ�ȭ

        if (game_manager.isSell)                   // ������ �� ä�� ���콺 �����͸� �Ǹ� ��ư ���� �ű� �� GameManager�� isSell ������ True�� �ٲ�� �ǰ�
        {
            game_manager.GetGold(id, level, this); // �� ���¿��� ������ ���� ��� GetGold() �Լ��� �ҷ���(ȣ��)���ν� ��带 ������ ���ÿ�

            Destroy(gameObject);                   // ���� ������Ʈ�� Destroy() �Լ��� ���� �����
        }

        float pos_x = transform.position.x;
        float pos_y = transform.position.y;

        
        if (pos_x < left_top.transform.position.x || pos_x > right_bottom.transform.position.x || // ���� �巡�� ��, ������ ������ �� ��ġ�� ���� ���� ���̶��
            pos_y > left_top.transform.position.y || pos_y < right_bottom.transform.position.y)   // ���� �巡�� ��, ������ ������ �� ��ġ�� ���� ���� ���̶��

            transform.position = new Vector3(0, -1, 0);                                           // ���� �߾����� �̵�  
    }

    void Move()                                         // Jelly�� �������� �����ϱ� ���� Move() �Լ�, �ڷ�ƾ �Լ� (Wander())�� �߰�        // �ڷ�ƾ �Լ�: �ð��� ����� ���� ����� �ְ���� �� ����ϰ� �Ǵ� �Լ�
    {
        if (speed_x != 0)
            sprite_renderer.flipX = speed_x < 0;        // x�� �ӵ��� ���� ������ �̹����� ������ ��

        transform.Translate(speed_x, speed_y, speed_y); // ���� �̵�
    }

    IEnumerator Wander() // �ڷ�ƾ �Լ�  /  �̵��ϱ�
    {
        move_delay = Random.Range(3, 6); // �̵��� �̵� ������ ������ (3 ~ 5��)
        move_time = Random.Range(3, 6);  // �̵��ð� (3 ~ 5��)
        
        // Translate�� �̵��� �� Object�� �ڷ���Ʈ �ϴ� ���� �����ϱ� ���� Time.deltaTime�� ������
        speed_x = Random.Range(-0.8f, 0.8f) * Time.deltaTime;
        speed_y = Random.Range(-0.8f, 0.8f) * Time.deltaTime;

        isWandering = true;                          // ���� ������                                           -----|
//                                                                                                                 |
        yield return new WaitForSeconds(move_delay); // ������ ������ �̵��� �̵� ������ �����̸�ŭ ��ٸ���  -----|
//                                                                                                                 |
        isWalking = true;                            // �ð��� ���� �����̱� �����ϸ�                         -----|
        anim.SetBool("isWalk", true);                // �̵� �ִϸ��̼� �����ϰ�                              -----|
//                                                                                                                 |--------------- �� ��� ������ �ݺ���
        yield return new WaitForSeconds(move_time);  // �̵��ð���ŭ ��ٸ� ��                                -----|
//                                                                                                                 |
        isWalking = false;                           // �������� ���߸�                                       -----|
        anim.SetBool("isWalk", false);               // �̵� �ִϸ��̼� ���� �� ��                            -----|
//                                                                                                                 |
        isWandering = false;                         // ����                                                  -----|
    }

    IEnumerator GetJelatin() // �ڷ�ƾ �Լ�  //  �ڵ����� ����ƾ ���
    {
        jelatin_delay = 3;                              // 3�ʸ���                                                  -----|
//                                                                                                                       |
        isGetting = true;                               // ����ƾ ȹ�� ���θ� true�� �ٲٰ�                         -----|
        game_manager.GetJelatin(id, level);             // id�� level�� ����ؼ� ����ƾ�� ȹ���� �ڿ�               -----|--------------- �� ��� ������ �ݺ���
//                                                                                                                       |
        yield return new WaitForSeconds(jelatin_delay); // ���� �ִ� �ڵ����� ��� ����ƾ�� �����̸�ŭ ��ٸ� ����  -----|
//                                                                                                                       |
        isGetting = false;                              // ����ƾ ȹ�� ���θ� false�� �ٲ�                          -----|
    }
}