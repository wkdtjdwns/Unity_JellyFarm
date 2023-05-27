using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int jelatin;                                   // ����ƾ ����
    public int gold;                                      // ��� ����
    public List<Jelly> jelly_list = new List<Jelly>();    // GameManager ��ũ��Ʈ�� ������ ������ List ���� ����
    public List<Data> jelly_data_list = new List<Data>(); // GameManager ��ũ��Ʈ�� �����͸� ������ List ���� ����
    public bool[] jelly_unlock_list;                      // �ر� �Ǿ������� ���θ� ������ ����Ʈ

    public int max_jelatin; // �ִ� ����ƾ ��
    public int max_gold;    // �ִ� ��� ��

    public bool isSell;     // ������ �Ĵ��� ����
    public bool isLive;     // ������ Ȱ��ȭ/��Ȱ��ȭ ���¸� ���� (������ ��Ʈ���� �����ϱ� ����)

    public Sprite[] jelly_spritelist;    // ��� ������ ������ ������ �迭
    public string[] jelly_namelist;      // ��� ������ �̸��� ������ �迭
    public int[] jelly_jelatinlist;      // ��� ������ �ʿ��� ����ƾ ���� ������ �迭
    public int[] jelly_goldlist;         // ��� ������ �ʿ��� ��� ���� ������ �迭

    public Text page_text;               // ���� ������ ��
    public Image unlock_group_jelly_img; // �ر��� ������ �̹���
    public Text unlock_group_gold_text;  // �ر��� ������ �ʿ��� ��� ��
    public Text unlock_group_name_text;  // �ر��� ������ �̸�

    public GameObject lock_group;        // �ر����� ���� ������ �������� ������ ������Ʈ
    public Image lock_group_jelly_img;   // �ر����� ���� ������ �̹���
    public Text lock_group_jelatin_text; // �ر����� ���� ������ �ʿ��� ����ƾ ��

    // Num Group�� ������
    public Text num_sub_text;      // ���� ����Ʈ�� ���� �ؽ�Ʈ ���� (����� ��Ÿ��)
    public Text num_btn_text;      // ���� ����Ʈ�� ��ư �ؽ�Ʈ (��带 ��Ÿ��)
    public Button num_btn;         // ���� ����Ʈ�� ��ư ����
    public int num_level;          // ���� ����Ʈ�� ���� ����
    public int[] num_gold_list;    // ���� ����Ʈ�� ��� �迭

    // Click Group�� ������
    public Text click_sub_text;    // ���� �ڲ����� ���� �ؽ�Ʈ ���� (����� ��Ÿ��)
    public Text click_btn_text;    // ���� �ڲ����� ��ư �ؽ�Ʈ (��带 ��Ÿ��)
    public Button click_btn;       // ���� �ڲ����� ��ư ����
    public int click_level;        // ���� �ڲ����� ���� ����
    public int[] click_gold_list;  // ���� �ڲ����� ��� �迭

    public RuntimeAnimatorController[] level_ac; // Animator ������ �����ϱ� ���� ChangeAc() �Լ��� �Բ� �߰�  /  ChangeAc() �Լ��� Jelly ��ũ��Ʈ���� Animator ��ü�� level�� �޾ƿ� runtimeAnimatorController�� ���� �ش� ������ ������ ���� Animator�� �����ϴ� ������ ������

    public Text jelatin_text;  // ����ƾ �� (Text)
    public Text gold_text;     // ��� �� (Text)

    // Jelly Panel�� Plant Panel�� Animator�� �̿��ؾ� �ؼ�
    // GameManager ��ũ��Ʈ�� ������ ������Ʈ ��ü�� �������� ���� ������ ������
    public Image jelly_panel;  // ���� ������Ʈ�� ���� ���� ���� ����
    public Image plant_panel;  // �÷�Ʈ ������Ʈ�� ���� ���� ���� ����
    public Image option_panel; // �ɼ� ������Ʈ�� ���� ���� ���� ����
    public Image bgm_panel;    // ����� ������Ʈ�� ���� ���� ���� ����

    public GameObject prefab;  // ������ ����  //  ������ : ���� ������Ʈ�� ����, ���� �� ������ �� ���� / �ش� ���� ������Ʈ�� ��� ������Ʈ, ������Ƽ ��, �ڽ� ���� ������Ʈ�� ���� ������ �������� ���� �� �ְ� ����

    public GameObject data_manager_obj; // DataManager�� ���� �����ϱ� ���� ����

    DataManager data_manager;           // DataManager�� ���� �����ϱ� ���� ����

    Animator jelly_anim; // ���� ������Ʈ�� �ִϸ��̼��� ���� ���� ���� ����
    Animator plant_anim; // �÷�Ʈ ������Ʈ�� �ִϸ��̼��� ���� ���� ���� ����
    Animator bgm_anim;   // ����� ������Ʈ�� �ִϸ��̼��� ���� ���� ���� ����

    bool isJellyClick;   // ���� ��ư�� ���ȴ��� ����
    bool isPlantClick;   // �÷�Ʈ ��ư�� ���ȴ��� ����
    bool isBgmClick;     // ��� ��ư�� ���ȴ��� ����
    bool isOption;       // ESC â�� �߰� ���� ���� ����

    public int page;     // ���� ������ ����

    // ���� (����ƾ)�� ���� ������
    public int random_jelatin_variable;   // 0 ~ 5������ �� �� �ϳ�
    public int random_jelatin;            // 10 ~ 1000������ �� �� �ϳ�
    public int random_jelatin_value;      // ���� �� ������ ���� ������ / ���������� ��� ����ƾ ��

    public GameObject random_jelatin_obj;  // ����ƾ�� ����ٴ� ����� �˷��ִ� �ؽ�Ʈ�� ������ ������Ʈ
    public Text random_jelatin_text;       // ����ƾ�� ����ٴ� ����� �˷��ִ� �ؽ�Ʈ
    public string random_J_text;           // ���� ����ƾ�� ������ ���� �޶����� �ؽ�Ʈ

    int jelatin_delay;                     // ����ƾ�� ��� ����
    int jelatin_text_delay;                // ����ƾ ȹ�� �ؽ�Ʈ�� ���� �ð�

    bool isGetJelatin;                     // ����ƾ�� ������� ����
    bool isJ_Text;                         // ����ƾ ȹ�� �ؽ�Ʈ�� ������ ����

    // ���� (���)�� ���� ������
    public int random_gold_variable;      // 0 ~ 5������ �� �� �ϳ�
    public int random_gold;               // 0 ~ 2500������ �� �� �ϳ�
    public int random_gold_value;         // ���� �� ������ ���� ������ / ���������� ��� ��� ��

    public GameObject random_gold_obj;    // ��带 ����ٴ� ����� �˷��ִ� �ؽ�Ʈ�� ������ ������Ʈ
    public Text random_gold_text;         // ��带 ����ٴ� ����� �˷��ִ� �ؽ�Ʈ
    public string random_G_text;          // ���� ����� ������ ���� �޶����� �ؽ�Ʈ

    int gold_delay;                       // ��带 ��� ����
    int gold_text_delay;                  // ��� ȹ�� �ؽ�Ʈ�� ���� �ð�

    bool isGetGold;                       // ��带 ������� ����
    bool isG_Text;                        // ��� ȹ�� �ؽ�Ʈ�� ������ ����

    AudioSource bgm_player;               // bgm_player�� AudioSource
    public Button[] bgm_button;           // ����� �����ư�� �迭

    public int index;                     // ����� ������� ���� ������ �� �ְ� �ϴ� ����

    void Awake() // �������� ����
    {
        instance = this;

        // �ִϸ��̼ǵ��� �ʱ�ȭ ��
        jelly_anim = jelly_panel.GetComponent<Animator>();
        plant_anim = plant_panel.GetComponent<Animator>();
        bgm_anim = bgm_panel.GetComponent<Animator>();

        isLive = true;

        jelatin_text.text = jelatin.ToString();                         // int���� jelatin ������ String������ �ٲ���
        gold_text.text = gold.ToString();                               // int���� gold ������ String������ �ٲ���
        unlock_group_gold_text.text = jelly_goldlist[0].ToString();     // int�� �迭�̿��� jelly_goldlist�� 0��° �ε����� String������ �ٲ���
        lock_group_jelatin_text.text = jelly_jelatinlist[0].ToString(); // int�� �迭�̿��� jelly_jelatinlist�� 0��° �ε����� String������ �ٲ���

        data_manager = data_manager_obj.GetComponent<DataManager>();    // data_manager ���� �ʱ�ȭ

        page = 0;
        jelly_unlock_list = new bool[12];

        bgm_player = GameObject.Find("BGM Player").GetComponent<AudioSource>();
    }

    void Start() // Awake() �Լ� ��������
    {
        // DataManager�� ���� �����Ͱ� �ε�Ǳ� ���� GameManager�� Ȱ��ȭ �Ǿ� �� �����͸� �����ϴ� ������ ������
        Invoke("LoadData", 0.1f); // LoadData �Լ��� ������Ŵ   /  Invoke() -> �Լ� ���� �ð��� ������Ŵ
    }

    void Update() // �� �����Ӹ���
    {
        if (Input.GetButtonDown("Cancel"))          // ESC ��ư�� ������ �� (ESC�� ������ true�� ��ȯ)
        {
            if (isJellyClick) ClickJellyBtn();      // ���� UI�� ���� �־��ٸ� (���� ��ư�� ������ ��) �Լ��� ����ؼ� UI�� ������
            else if (isPlantClick) ClickPlantBtn(); // �÷�Ʈ UI�� ���� �־��ٸ� (�÷�Ʈ ��ư�� ������ ��) �Լ��� ����ؼ� UI�� ������
            else if (isBgmClick) ClickBgmBtn();     // ����� UI�� ���� �־��ٸ� (����� ��ư�� ������ ��) �Լ��� ����ؼ� UI�� ������ 
            else Option();                          // �ƹ� UI�� ���� ���� �ʾҴٸ� ESC â�� �ø���
        }
    }

    void FixedUpdate() // ������ ���ݸ���
    {
        if (!isGetJelatin) StartCoroutine(GetRandomJelatin()); // �ڷ�ƾ �Լ� ����

        if (!isGetGold) StartCoroutine(GetRandomGold());       // �ڷ�ƾ �Լ� ����
    }

    void LateUpdate() // ��� Update �Լ����� ȣ��� ����
    {
        // Format �Լ��� ���� ���� �ؽ�Ʈ�� ǥ������ ����, SmoothStep �Լ��� ���� ���ڰ� ��ȯ�Ǵ� ���̿� ������ �ִϸ��̼��� �߰���
        // jelatin_text�� gold_text�� ��� ���� �����ڸ� public���� �����Ͽ� ����Ƽ ���α׷����� ���� ������
        jelatin_text.text = string.Format("{0:n0}", (int)Mathf.SmoothStep(float.Parse(jelatin_text.text), jelatin, 0.5f));
        gold_text.text = string.Format("{0:n0}", (int)Mathf.SmoothStep(float.Parse(gold_text.text), gold, 0.5f));

        num_sub_text.text = "���� ���뷮 " + num_level * 2;                               // ����� �˷��ִ� �ؽ�Ʈ�� �˸°� �ٲ��� ����
        if (num_level >= 5) num_btn.gameObject.SetActive(false);                          // �̹� ���� ����Ʈ�� ������ �ְ� �����̶�� ��ư�� ���ְ�
        else num_btn_text.text = string.Format("{0:n0}", num_gold_list[num_level]);       // �װ� �ƴ϶�� ���׷��̵忡 �ʿ��� ��带 ���

        click_sub_text.text = "Ŭ�� ���귮 X " + click_level;                             // ����� �˷��ִ� �ؽ�Ʈ�� �˸°� �ٲ��� ����
        if (click_level >= 5) click_btn.gameObject.SetActive(false);                      // �̹� ���� �ڲ����� ������ �ְ� �����̶�� ��ư�� ���ְ�
        else click_btn_text.text = string.Format("{0:n0}", click_gold_list[click_level]); // �װ� �ƴ϶�� ���׷��̵忡 �ʿ��� ��带 ���
    }

    public void ChangeAc(Animator anim, int level)            // ���� �ִ� �迭�� �Բ� Animator ������ ������
    {
        anim.runtimeAnimatorController = level_ac[level - 1]; // level_ac �迭�� �ִ� ũ��� 3 (�ִϸ��̼��� ����)�̸�, level ���� 1���� 3������ level ���� �迭�� ũ�⸦ ����� �ʵ��� -1 (�迭�� 0���� ����)�� ���־�� ��
        SoundManager.instance.PlaySound("Grow");              // �����ϴ� ���� ���
    }

    public void GetJelatin(int id, int level)      // ����ƾ ȹ���� GameManager���� �����ϰ� ��
    {
        jelatin += (id + 1) * level * click_level; // ����ƾ�� id�� level, click_level, clear_boss�� ����ؼ� ������

        if (jelatin > max_jelatin)                 // ���� ����ƾ�� �Ѱ踦 �Ѿ��ٸ�
            jelatin = max_jelatin;                 // ���� ����Ƽ�� ���� �Ѱ����� ���� ����
    }

    public void GetGold(int id, int level, Jelly jelly) // ��� ȹ�� ���� GameManager���� �����ϰ� ��
    {
        gold += jelly_goldlist[id] * level;             // ������ id�� level, clear_boss�� ���� ��� ȹ�� ��

        if (gold > max_gold)                            // ���� ��尡 �Ѱ踦 �Ѿ��ٸ�
            gold = max_gold;                            // ���� ����� ���� �Ѱ����� ���� ���� ��

        jelly_list.Remove(jelly);                       // ������ ����

        SoundManager.instance.PlaySound("Get Gold");    // ��带 ��� ���� ���
    }

    public void CheckSell()
    {
        isSell = isSell == false;
    }

    // ���� ��ư�� Ŭ�� �� �� �߻��ϸ� ������ UI â�� ���� ������ ������ ����
    public void ClickJellyBtn() // ���� ��ư�� ������ ��
    {
        if (isPlantClick)                                   // �÷�Ʈ ��ư�� ���� �ִٸ�
        {
            plant_anim.SetTrigger("doHide");                // �÷�Ʈ UI�� �����
            SoundManager.instance.PlaySound("Pause Out");   // UI�� ������ ���� ���

            isPlantClick = false;                           // �÷�Ʈ Ŭ���� ��Ȱ��ȭ��Ű��
            isLive = true;                                  // ������ ��Ʈ�� �� �� �ְ� ��
        }

        if (isBgmClick)                                     // ����� ��ư�� ���� �ִٸ�
        {
            bgm_anim.SetTrigger("doHide");                  // ����� UI�� �����
            SoundManager.instance.PlaySound("Pause Out");   // UI�� ������ ���� ���

            isBgmClick = false;                             // ����� Ŭ���� ��Ȱ��ȭ��Ű��
            isLive = true;                                  // ������ ��Ʈ�� �� �� �ְ� ��
        }

        if (isJellyClick)                                   // ���� ��ư�� �̹� ���� �ִٸ�
        {
            jelly_anim.SetTrigger("doHide");                // ���� UI�� �����
            SoundManager.instance.PlaySound("Pause Out");   // UI�� ������ ���� ���
        }

        else                                                // ���� ���� �ʾҴٸ�
        {
            jelly_anim.SetTrigger("doShow");                // ���� UI�� �����ְ�
            SoundManager.instance.PlaySound("Pause In");    // UI�� �ø��� ���� ���
        }

        isJellyClick = !isJellyClick;                       // �� ���� ���� Ŭ���� �ݴ� ���·� ����� �ش� (true ���¿����� false��, false ���¿����� true�� �ٲ�)
        isLive = !isLive;                                   // ���̺긦 �ݴ� ���·� ����� �ش� (true ���¿����� false��, false ���¿����� true�� �ٲ�)
    }


    public void ClickPlantBtn() // �÷�Ʈ ��ư�� ������ ��
    {
        if (isJellyClick)                                   // ���� ��ư�� ���� �ִٸ�
        {
            jelly_anim.SetTrigger("doHide");                // ���� UI�� �����
            SoundManager.instance.PlaySound("Pause Out");   // UI�� ������ ���� ���

            isJellyClick = false;                           // ���� Ŭ���� ��Ȱ��ȭ ��Ű��
            isLive = true;                                  // ������ ��Ʈ�� �� �� �ְ� ��
        }

        if (isBgmClick)                                     // ����� ��ư�� ���� �ִٸ�
        {
            bgm_anim.SetTrigger("doHide");                  // ����� UI�� �����
            SoundManager.instance.PlaySound("Pause Out");   // UI�� ������ ���� ���

            isBgmClick = false;                             // ����� Ŭ���� ��Ȱ��ȭ��Ű��
            isLive = true;                                  // ������ ��Ʈ�� �� �� �ְ� ��
        }

        if (isPlantClick)                                   // �÷�Ʈ ��ư�� �̹� ���� �ִٸ�
        {
            plant_anim.SetTrigger("doHide");                // �÷�Ʈ UI�� �����
            SoundManager.instance.PlaySound("Pause Out");   // UI�� ������ ���� ���
        }
        else                                                // ���� ���� �ʾҴٸ�
        { 
            plant_anim.SetTrigger("doShow");                // �÷�Ʈ UI�� �����ְ�
            SoundManager.instance.PlaySound("Pause In");    // UI�� �ø��� ���� ���
        }

        isPlantClick = !isPlantClick;                       // �� ���� �÷�Ʈ Ŭ���� �ݴ� ���·� ����� �ش� (true ���¿����� false��, false ���¿����� true�� �ٲ�)
        isLive = !isLive;                                   // ���̺긦 �ݴ� ���·� ����� �ش� (true ���¿����� false��, false ���¿����� true�� �ٲ�)
    }

    public void ClickBgmBtn() // ����� ��ư�� ������ ��
    {
        if (isJellyClick)                                   // ���� ��ư�� ���� �ִٸ�
        {
            jelly_anim.SetTrigger("doHide");                // ���� UI�� �����
            SoundManager.instance.PlaySound("Pause Out");   // UI�� ������ ���� ���

            isJellyClick = false;                           // ���� Ŭ���� ��Ȱ��ȭ ��Ű��
            isLive = true;                                  // ������ ��Ʈ�� �� �� �ְ� ��
        }

        if (isPlantClick)                                   // �÷�Ʈ ��ư�� �̹� ���� �ִٸ�
        {
            plant_anim.SetTrigger("doHide");                // �÷�Ʈ UI�� �����
            SoundManager.instance.PlaySound("Pause Out");   // UI�� ������ ���� ���

            isPlantClick = false;                           // �÷�Ʈ Ŭ���� ��Ȱ��ȭ��Ű��
            isLive = true;                                  // ������ ��Ʈ�� �� �� �ְ� ��
        }

        if (isBgmClick)                                     // ����� ��ư�� ���� �ִٸ�
        {
            bgm_anim.SetTrigger("doHide");                  // ����� UI�� �����
            SoundManager.instance.PlaySound("Pause Out");   // UI�� ������ ���� ���
        }
        else                                                // ���� ���� �ʾҴٸ�
        {
            bgm_anim.SetTrigger("doShow");                  // ����� UI�� �����ְ�
            SoundManager.instance.PlaySound("Pause In");    // UI�� �ø��� ���� ���
        }

        isBgmClick = !isBgmClick;                           // �� ���� ����� Ŭ���� �ݴ� ���·� ����� �ش� (true ���¿����� false��, false ���¿����� true�� �ٲ�)
        isLive = !isLive;                                   // ���̺긦 �ݴ� ���·� ����� �ش� (true ���¿����� false��, false ���¿����� true�� �ٲ�)
    }

    // ���� ��ư�� Ŭ�� �� �� �߻��ϸ� ������ UI â�� ���� ������ ������ ����
    public void Option() // ESC Ű�� ������ ��
    {
        isOption = !isOption;                                       // �ɼ��� �ݴ� ���·� ������ش� (true ���¿����� false��, false ���¿����� true�� �ٲ�)
        isLive = !isLive;                                           // ���̺긦 �ݴ� ���·� ����� �ش� (true ���¿����� false��, false ���¿����� true�� �ٲ�)

        option_panel.gameObject.SetActive(isOption);                // �� ���� �ɼ��� true ���·� �ٲ���ٸ� ��Ȱ��ȭ �Ǿ� �ִ� ESC â UI�� Ȱ��ȭ ��Ŵ 
        Time.timeScale = isOption == true ? 0 : 1;                  // ESC â�� ���� �ִٸ� �ð��� ���߰� �ƴ϶�� �ð��� �帣�� ��

        isGetJelatin = !isGetJelatin;
        random_jelatin_obj.gameObject.SetActive(isGetJelatin);      // ����ƾ ȹ�� �ؽ�Ʈ�� ��� ��Ȱ��ȭ ��Ŵ

        isGetGold = !isGetGold;
        random_gold_obj.gameObject.SetActive(isGetGold);            // ��� ȹ�� �ؽ�Ʈ�� ��� ��Ȱ��ȭ ��Ŵ

        // ��������� �ϸ� �������� �ؽ�Ʈ�� ESC â�� ���� ������ �ٽ� ����� ��
        // �׷��� if���� �ϳ� �����

        if (!isOption) // �ɼ� â�� ���� ���� �ʴٸ�
        {
            random_jelatin_obj.gameObject.SetActive(!isGetJelatin); // ����ƾ ȹ�� �ؽ�Ʈ�� ���������� ��Ȱ��ȭ ��Ŵ (�ؽ�Ʈ�� ��Ȱ��ȭ ���ױ⸸ �߱� ������ ���� ���������� ����)

            random_gold_obj.gameObject.SetActive(!isGetGold);       // ��� ȹ�� �ؽ�Ʈ�� ���������� ��Ȱ��ȭ ��Ŵ (�ؽ�Ʈ�� ��Ȱ��ȭ ���ױ⸸ �߱� ������ ���� ���������� ����)
        }
    }

    public void PageUp() // ���� ������ ��ư�� ���� ��
    {
        if (page >= 11)                              // ���� �ִ� �������� �Ѱ����� ���� X
        {   
            SoundManager.instance.PlaySound("Fail"); // ��� ���ۿ� �����ߴٴ� ���� ���
            return;   
        }

        ++page;                                      // ������ �� ���� (���� �������� �ѱ�)
        ChangePage();                                // �������� �ٲٴ� �Լ� ȣ��
    }

    public void PageDown() // ���� ������ ��ư�� ���� ��
    {
        if (page <= 0)                               // 0 ������ ������ �ѱ�� �ϸ� ���� X
        {
            SoundManager.instance.PlaySound("Fail"); // ��� ���ۿ� �����ߴٴ� ���� ���
            return;    
        }    
        --page;                                      // ������ �� ���� (���� �������� �ѱ�)
        ChangePage();                                // �������� �ٲٴ� �Լ�
    }

    void ChangePage() // ���� / ���� �������� �ѱ� �� �������� �ٲ�� �Լ�
    {
        lock_group.gameObject.SetActive(!jelly_unlock_list[page]); // �ر����� ���� �������� �Ѿ�� lock_group �̶�� ������Ʈ�� Ȱ��ȭ��

        page_text.text = string.Format("#{0:00}", (page + 1)); // ������ �� ������Ŵ
        // ��ȭ�� ���� ���ڿ��� ǥ���ϰų�, ��ų ������ ���� (�����)�� ���� ������ string.Format()�� �̿��ؼ� ���ڿ��� ������ ���ϰ� ���� �� �� ����
        // Format("...{0}, ... {1}", string , 22) -> ...string ... 22 / �߰�ȣ ({ }) ���̿� �ִ� ���ڴ� �޸� (,) �ڿ� ������ ���� ���� ���� �̸� (�Ǵ� ��)�� ������ �ǹ��� / �迭�� ���� 0 ���� ������

        // �� ������Ʈ�� Sprite �Ǵ� Text�� �����Ͽ� ��ġ ���� �������� �Ѿ�� ��ó�� ������

        if (lock_group.activeSelf) // �ر����� ���� �������� (lock_group ������Ʈ�� Ȱ��ȭ �Ǿ� ������)
        {
            lock_group_jelly_img.sprite = jelly_spritelist[page];                            // �ر����� ���� ������ �̹��� (Sprite)�� ����
            lock_group_jelatin_text.text = string.Format("{0:n0}", jelly_jelatinlist[page]); // �ر����� ���� ������ �̸� (Text)���� ����
                                                                                             // �ر����� �������� ������ ����� ����

            lock_group_jelly_img.SetNativeSize();                                            // �̹����� ���� ũ��� ���ư����� �ؼ� �̹����� ������ ������ ������
        }
        else                       // �̹� �ر��� �������� (lock_group ������Ʈ�� Ȱ��ȭ �Ǿ� ���� ������)
        {
            unlock_group_jelly_img.sprite = jelly_spritelist[page];                          // �ر��� ������ �̹��� (Sprite)�� ����
            unlock_group_name_text.text = jelly_namelist[page];                              // �ر��� ������ �̸� (Text)���� ����
            unlock_group_gold_text.text = string.Format("{0:n0}", jelly_goldlist[page]);     // ������ ������ page ��°�� �ر��� ������ �������� �����

            unlock_group_jelly_img.SetNativeSize();                                          // �̹����� ���� ũ��� ���ư����� �ؼ� �̹����� ������ ������ ������
        }

        SoundManager.instance.PlaySound("Button");                                           // ��ư�� ������ ���� ���
    }

    public void Unlock() // �رݽ�Ű��
    {
        if (jelatin < jelly_jelatinlist[page])         // ���� ����ƾ�� �ر��ϱ� ���� ����ƾ ������ ������ 
        {            
            SoundManager.instance.PlaySound("Fail");   // ��� ���ۿ� �����ߴٴ� ���� ��� ��
            return;                                    // ���� X
        }

        jelly_unlock_list[page] = true;                // ���� �������� ������ �ر��ϰ�
        ChangePage();                                  // �������� �ٲ�

        jelatin -= jelly_jelatinlist[page];            // ����ƾ�� �ر��ϱ� ���� ����ƾ�� �� ��ŭ ������

        SoundManager.instance.PlaySound("Unlock");     // ������ �ر��ϴ� ���� ���
    }

    public void BuyJelly() // ���� ���� ���
    {
        if (gold < jelly_goldlist[page] || jelly_list.Count >= num_level * 2)            // ���� ������ �ִ� ���� �ʿ��� ��� ������ ���ų� ���� ������ �ִ� ������ ���� ���뷮�� �Ѱ躸�� ���ų� ������ 
        {  
            SoundManager.instance.PlaySound("Fail");                                     // ��� ���ۿ� �����ߴٴ� ���� ��� �� 
            return;                                                                      // ���� X
        }

        gold -= jelly_goldlist[page];                                                    // ��带 ���ſ� �ʿ��� ��� �� ��ŭ ������ ��

        GameObject obj = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity); // Instantiate�� Quaternion.identity ��, ȸ���� ���� ������Ʈ obj ���� 
        Jelly jelly = obj.GetComponent<Jelly>();                                         // ��� ������ obj ������Ʈ�� ������ ���۳�Ʈ�� ������ ��
        obj.name = "Jelly " + page;                                                      // obj ������Ʈ�� �̸��� "���� + ������ ��" �� ����
        jelly.id = page;                                                                 // ������ id�� ������ ���� ����
        jelly.sprite_renderer.sprite = jelly_spritelist[page];                           // �ش� �������� �ִ� ������ �����ǵ��� ��

        jelly_list.Add(jelly);                                                           // �� ���� ������ ������

        SoundManager.instance.PlaySound("Buy");                                          // ������ �����ϴ� ���� ���
    }

    public void NumUpgrade() // ������ ������ �� �ִ� �Ѱ踦 �÷��ִ� �Լ�
    {
        if (gold < num_gold_list[num_level])                                        // ���� ������ �ִ� ���� �ʿ��� ��� ������ ������
        { 
            SoundManager.instance.PlaySound("Fail");                                // ��� ���ۿ� �����ߴٴ� ���� ��� ��
            return;                                                                 // ���� X   
        }

        gold -= num_gold_list[num_level++];                                         // ��带 ���ſ� �ʿ��� ��� �� ��ŭ ������ ��

        num_sub_text.text = "���� ���뷮 " + num_level * 2;                         // ����� �˷��ִ� �ؽ�Ʈ�� ���

        if (num_level >= 5) num_btn.gameObject.SetActive(false);                    // ���� ����� ���׷��̵�� �ְ� ������ �޼��� ���°� �ȴٸ� ��ư�� ���ְ�
        else num_btn_text.text = string.Format("{0:n0}", num_gold_list[num_level]); // �װ� �ƴ϶�� �� ���� ������ ��带 ������

        SoundManager.instance.PlaySound("Buy");                                     // ���׷��̵� �ϴ� ���� ���
    }

    public void ClickUpgrade() // ������ Ŭ���� �� ��� ��ȭ�� �÷��ִ� �Լ�
    {
        if (gold < click_gold_list[click_level])                                          // ���� ������ �ִ� ���� �ʿ��� ��� ������ ������    
        {
            SoundManager.instance.PlaySound("Fail");                                      // ��� ���ۿ� �����ߴٴ� ���� ��� �� 
            return;                                                                       // ���� X                           
        }

        gold -= click_gold_list[click_level++];                                           // ��带 ���ſ� �ʿ��� ��� �� ��ŭ ������ ��

        click_sub_text.text = "Ŭ�� ���귮 X " + click_level;                             // ����� �˷��ִ� �ؽ�Ʈ�� ���

        if (click_level >= 5) click_btn.gameObject.SetActive(false);                      // ���� ����� ���׷��̵�� �ְ� ������ �޼��� ���°� �ȴٸ� ��ư�� ���ְ�
        else click_btn_text.text = string.Format("{0:n0}", click_gold_list[click_level]); // �װ� �ƴ϶�� �� ���� ������ ��带 ������

        SoundManager.instance.PlaySound("Buy");                                           // ���׷��̵� �ϴ� ���� ���
    }

    public void SetRandomJelatin() // �����ϰ� ���� ����ƾ�� ���� �����ϰ� �� ���� ���� ���� �ؽ�Ʈ ����
    {
        random_jelatin = Random.Range(10, 1001);      // 10 ~ 1000������ ������ �ϳ� 
        random_jelatin_variable = Random.Range(0, 6); // 0 ~ 5������ ������ �ϳ�
        random_jelatin_value = random_jelatin * random_jelatin_variable;

        // �������� ��� ����ƾ ���� ���� �ؽ�Ʈ ����
        if (random_jelatin_value == 5000) random_J_text = "�ִ� ����ƾ�Դϴ�! ����, �״� ���̾�!";
        else if (random_jelatin_value >= 4000) random_J_text = "���� �� �������� �ִ� ����ƾ�ε���?!";
        else if (random_jelatin_value > 2500) random_J_text = "�ִ� ����ƾ�� ���� �̻��� �����Ծ��!";
        else if (random_jelatin_value >= 2000) random_J_text = "���ݸ� �� ����������...��";
        else if (random_jelatin_value >= 1000) random_J_text = "�׷��� �̰� ��𿡿�!";
        else if (random_jelatin_value >= 500) random_J_text = "��¦ �ƽ��׿�!";
        else if (random_jelatin_value == 0) random_J_text = "���� ��û��~~~";
        else random_J_text = "�ʹ� ���� �������°� �ƴѰ���?! ¥�� ¥~";

        random_jelatin_text.text = string.Format("������ ����ƾ {0}���� �����Խ��ϴ�!\n {1}", random_jelatin_value, random_J_text); // �ؽ�Ʈ �����ϱ�
    }

    IEnumerator GetRandomJelatin() // SetRandomJelatin() �Լ����� ������ ����ƾ ���� �޴� �Լ�  /  �ڷ�ƾ �Լ�
    {
        jelatin_delay = Random.Range(20, 41);                                            // ����ƾ�� ��� ���� -> 20 ~ 40�� ����
        jelatin_text_delay = 3;                                                        // ����ƾ�� ����ٴ� ����� �˷��ִ� �ؽ�Ʈ�� ȭ�鿡 ����� �ִ� �ð�

        isGetJelatin = true;
        SetRandomJelatin();                                                            // ���� ����ƾ ���� �����ϰ�

        isJ_Text = true;
        random_jelatin_obj.gameObject.SetActive(isJ_Text);                             // �ؽ�Ʈ ��� ����

        yield return new WaitForSeconds(jelatin_text_delay);                           // �ؽ�Ʈ�� ���� �ð���ŭ ��ٸ� ��

        isJ_Text = false;
        random_jelatin_obj.gameObject.SetActive(isJ_Text);                             // �ؽ�Ʈ�� ������

        jelatin += random_jelatin_value;                                               // ����ƾ ȹ�� ��

        if (random_jelatin_value != 0) SoundManager.instance.PlaySound("Get Jelatin"); // ��� ����ƾ�� 0�� �ƴϸ� ����ƾ�� ȹ���ϴ� �Ҹ� ����ϰ�
        else SoundManager.instance.PlaySound("Touch");                                 // ��� ����ƾ�� 0�̸� ��ġ�ϴ� �Ҹ��� ���� ���� (��ġ�ϴ� �Ҹ����� ��︲)

        yield return new WaitForSeconds(jelatin_delay);                                // ����ƾ�� ��� ���ݸ�ŭ ��ٸ�
        isGetJelatin = false;
    }

    public void SetRandomGold() // �����ϰ� ���� ����� ���� �����ϰ� �� ���� ���� ���� �ؽ�Ʈ ����
    {
        random_gold = Random.Range(0, 2501);       // 0 ~ 2500������ ������ �ϳ� 
        random_gold_variable = Random.Range(0, 6); // 0 ~ 5������ ������ �ϳ�
        random_gold_value = random_gold * random_gold_variable;

        // �������� ��� ��� ���� ���� �ؽ�Ʈ ����
        if (random_gold_value == 12500) random_G_text = "�ִ� ����Դϴ�! ����, �״� ���̾�!";
        else if (random_gold_value >= 10000) random_G_text = "���� �� �������� �ִ� ����ε���?!";
        else if (random_gold_value > 6250) random_G_text = "�ִ� ����� ���� �̻��� �����Ծ��!";
        else if (random_gold_value >= 4500) random_G_text = "���ݸ� �� ����������...��";
        else if (random_gold_value >= 3250) random_G_text = "�׷��� �̰� ��𿡿�!";
        else if (random_gold_value >= 1500) random_G_text = "��¦ �ƽ��׿�!";
        else if (random_gold_value == 0) random_G_text = "���� ��û��~~~";
        else random_G_text = "�ʹ� ���� �������°� �ƴѰ���?! ¥�� ¥~";

        random_gold_text.text = string.Format("������ ��� {0}���� �����Խ��ϴ�!\n {1}", random_gold_value, random_G_text); // �ؽ�Ʈ �����ϱ�
    }

    IEnumerator GetRandomGold() // SetRandomGold() �Լ����� ������ ��� ���� �޴� �Լ�  /  �ڷ�ƾ �Լ�
    {
        gold_delay = Random.Range(30, 46);                                       // ��带 ��� ���� -> 30 ~ 45�� ����
        gold_text_delay = 3;                                                     // ��带 ����ٴ� ����� �˷��ִ� �ؽ�Ʈ�� ȭ�鿡 ����� �ִ� �ð�

        isGetGold = true;
        SetRandomGold();                                                         // ���� ��� ���� �����ϰ�

        isG_Text = true;
        random_gold_obj.gameObject.SetActive(isG_Text);                          // �ؽ�Ʈ ��� ����

        yield return new WaitForSeconds(gold_text_delay);                        // �ؽ�Ʈ�� ���� �ð���ŭ ��ٸ� ��

        isG_Text = false; 
        random_gold_obj.gameObject.SetActive(isG_Text);                          // �ؽ�Ʈ�� ������

        gold += random_gold_value;                                               // ��� ȹ�� ��

        if (random_gold_value != 0) SoundManager.instance.PlaySound("Get Gold"); // ��� ��尡 0�� �ƴϸ� ��带 ȹ���ϴ� �Ҹ� ����ϰ�
        else SoundManager.instance.PlaySound("Touch");                           // ��� ��尡 0�̸� ��ġ�ϴ� �Ҹ��� ���� ���� (��ġ�ϴ� �Ҹ����� ��︲)

        yield return new WaitForSeconds(gold_delay);                             // ��带 ��� ���ݸ�ŭ ��ٸ�
        isGetGold = false;
    }

    public void ChangeBgm(int bgm_button) // ����� ���� ��ư�� ������ ��  /  �迭 bgm_button �� ���� ��ư�� �ε��� ���� int������ �޾Ƽ� �Ű������� ����� 
    {
        index = bgm_button;                                       // �����ϱ� ���� bgm_button�� �ε��� ���� index ������ ����
        bgm_player.clip = SoundManager.instance.bgm_clips[index]; // ����� �÷��̾ ��� ������ index ���� �ش��ϴ� ��������� ���� ��
        bgm_player.Play();                                        // ������� �÷�����
    }

    void LoadData() // ������ �ҷ�����
    {
        lock_group.gameObject.SetActive(!jelly_unlock_list[page]); // �ر� �Ǿ� ���� ���� ���� ����Ʈ�� �ҷ���

        bgm_player.clip = SoundManager.instance.bgm_clips[index];  // ����� �ҷ���
        bgm_player.Play();                                         // ����� �ҷ���

        for (int i = 0; i < jelly_data_list.Count; ++i) // ���� �� �� �־��� ������ ���� ��ŭ �ݺ�
        {
            GameObject obj = Instantiate(prefab, jelly_data_list[i].pos, Quaternion.identity); // ������ �������� �־��� ��ġ�� ������ ����
            Jelly jelly = obj.GetComponent<Jelly>();                                           // ������ �����鿡�� Jelly ���۳�Ʈ�� ������ ��
            jelly.id = jelly_data_list[i].id;                                                  // ������ �����鿡�� id�ο�
            jelly.level = jelly_data_list[i].level;                                            // ������ �����鿡�� level�ο�
            jelly.exp = jelly_data_list[i].exp;                                                // ������ �����鿡�� ����ġ�ο�
            jelly.sprite_renderer.sprite = jelly_spritelist[jelly.id];                         // ������ �����鿡�� id�� �´� ������ ������ ��������
            jelly.anim.runtimeAnimatorController = level_ac[jelly.level - 1];                  // ������ �����鿡�� �ڽ��� ������ �´� �ִϸ��̼� (ũ��)�� ������ ��
            obj.name = "Jelly " + jelly.id;                                                    // ������ �����鿡�� �´� �̸� �ο�

            jelly_list.Add(jelly);                                                             // ������ ������� �������� ��� ������
        }
    }

    public void Exit()                                // ���ӿ��� ���� ��
    {
        data_manager.JsonSave();                      // ������

        SoundManager.instance.PlaySound("Pause Out"); // ������ ���� ���

        Application.Quit();                           // ���ӿ��� ������ (����Ƽ ���α׷������� Application.Quit() �Լ��� �۵� ���� �ʴ� ���� ������)
    }
}