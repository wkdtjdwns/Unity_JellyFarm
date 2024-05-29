using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int jelatin;                                   // 젤라틴 변수
    public int gold;                                      // 골드 변수
    public List<Jelly> jelly_list = new List<Jelly>();    // GameManager 스크립트에 젤리를 저장할 List 변수 선언
    public List<Data> jelly_data_list = new List<Data>(); // GameManager 스크립트에 데이터를 저장할 List 변수 선언
    public bool[] jelly_unlock_list;                      // 해금 되었는지의 여부를 저장할 리스트

    public int max_jelatin; // 최대 젤라틴 수
    public int max_gold;    // 최대 골드 수

    public bool isSell;     // 젤리를 파는지 여부
    public bool isLive;     // 게임의 활성화/비활성화 상태를 구분 (젤리의 컨트롤을 제한하기 위함)

    public Sprite[] jelly_spritelist;    // 모든 젤리의 종류를 저장할 배열
    public string[] jelly_namelist;      // 모든 젤리의 이름을 저장할 배열
    public int[] jelly_jelatinlist;      // 모든 젤리의 필요한 젤라틴 수를 저장할 배열
    public int[] jelly_goldlist;         // 모든 젤리의 필요한 골드 수를 저장할 배열

    public Text page_text;               // 상점 페이지 수
    public Image unlock_group_jelly_img; // 해금한 젤리의 이미지
    public Text unlock_group_gold_text;  // 해금한 젤리의 필요한 골드 수
    public Text unlock_group_name_text;  // 해금한 젤리의 이름

    public GameObject lock_group;        // 해금하지 못한 젤리의 페이지를 관리할 오브젝트
    public Image lock_group_jelly_img;   // 해금하지 못한 젤리의 이미지
    public Text lock_group_jelatin_text; // 해금하지 못한 젤리의 필요한 젤라틴 수

    // Num Group의 변수들
    public Text num_sub_text;      // 젤리 아파트의 서브 텍스트 변수 (기능을 나타냄)
    public Text num_btn_text;      // 젤리 아파트의 버튼 텍스트 (골드를 나타냄)
    public Button num_btn;         // 젤리 아파트의 버튼 변수
    public int num_level;          // 젤리 아파트의 레벨 변수
    public int[] num_gold_list;    // 젤리 아파트의 골드 배열

    // Click Group의 변수들
    public Text click_sub_text;    // 젤리 꾹꾹이의 서브 텍스트 변수 (기능을 나타냄)
    public Text click_btn_text;    // 젤리 꾹꾹이의 버튼 텍스트 (골드를 나타냄)
    public Button click_btn;       // 젤리 꾹꾹이의 버튼 변수
    public int click_level;        // 젤리 꾹꾹이의 레벨 변수
    public int[] click_gold_list;  // 젤리 꾹꾹이의 골드 배열

    public RuntimeAnimatorController[] level_ac; // Animator 변경을 관리하기 위해 ChangeAc() 함수와 함께 추가  /  ChangeAc() 함수는 Jelly 스크립트에서 Animator 객체와 level을 받아와 runtimeAnimatorController를 통해 해당 젤리의 레벨에 따라 Animator를 변경하는 역할을 수행함

    public Text jelatin_text;  // 젤라틴 수 (Text)
    public Text gold_text;     // 골드 수 (Text)

    // Jelly Panel과 Plant Panel의 Animator를 이용해야 해서
    // GameManager 스크립트에 각각의 오브젝트 객체를 가져오기 위한 변수를 생성함
    public Image jelly_panel;  // 젤리 오브젝트를 가져 오기 위한 변수
    public Image plant_panel;  // 플랜트 오브젝트를 가져 오기 위한 변수
    public Image option_panel; // 옵션 오브젝트를 가져 오기 위한 변수
    public Image bgm_panel;    // 배경음 오브젝트를 가져 오기 위한 변수

    public GameObject prefab;  // 프리팹 변수  //  프리팹 : 게임 오브젝트를 생성, 설정 및 저장할 수 있음 / 해당 게임 오브젝트의 모든 컴포넌트, 프로퍼티 값, 자식 게임 오브젝트를 재사용 가능한 에셋으로 만들 수 있게 해줌

    public GameObject data_manager_obj; // DataManager를 쉽게 관리하기 위한 변수

    DataManager data_manager;           // DataManager를 쉽게 관리하기 위한 변수

    Animator jelly_anim; // 젤리 오브젝트의 애니매이션을 가져 오기 위한 변수
    Animator plant_anim; // 플랜트 오브젝트의 애니매이션을 가져 오기 위한 변수
    Animator bgm_anim;   // 배경음 오브젝트의 애니매이션을 가져 오기 위한 변수

    bool isJellyClick;   // 젤리 버튼이 눌렸는지 여부
    bool isPlantClick;   // 플랜트 버튼이 눌렸는지 여부
    bool isBgmClick;     // 브금 버튼이 눌렸는지 여부
    bool isOption;       // ESC 창이 뜨게 할지 말지 여부

    public int page;     // 상점 페이지 변수

    // 랜덤 (젤라틴)에 대한 변수들
    public int random_jelatin_variable;   // 0 ~ 5까지의 값 중 하나
    public int random_jelatin;            // 10 ~ 1000까지의 값 중 하나
    public int random_jelatin_value;      // 위에 두 변수를 곱한 값으로 / 실질적으로 얻는 젤라틴 수

    public GameObject random_jelatin_obj;  // 젤라틴을 얻었다는 사실을 알려주는 텍스트를 관리할 오브젝트
    public Text random_jelatin_text;       // 젤라틴을 얻었다는 사실을 알려주는 텍스트
    public string random_J_text;           // 얻어온 젤라틴의 개수에 따라 달라지는 텍스트

    int jelatin_delay;                     // 젤라틴을 얻는 간격
    int jelatin_text_delay;                // 젤라틴 획득 텍스트를 띄우는 시간

    bool isGetJelatin;                     // 젤라틴을 얻었는지 여부
    bool isJ_Text;                         // 젤라틴 획득 텍스트를 띄우는지 여부

    // 랜덤 (골드)에 대한 변수들
    public int random_gold_variable;      // 0 ~ 5까지의 값 중 하나
    public int random_gold;               // 0 ~ 2500까지의 값 중 하나
    public int random_gold_value;         // 위에 두 변수를 곱한 값으로 / 실질적으로 얻는 골드 수

    public GameObject random_gold_obj;    // 골드를 얻었다는 사실을 알려주는 텍스트를 관리할 오브젝트
    public Text random_gold_text;         // 골드를 얻었다는 사실을 알려주는 텍스트
    public string random_G_text;          // 얻어온 골드의 개수에 따라 달라지는 텍스트

    int gold_delay;                       // 골드를 얻는 간격
    int gold_text_delay;                  // 골드 획득 텍스트를 띄우는 시간

    bool isGetGold;                       // 골드를 얻었는지 여부
    bool isG_Text;                        // 골드 획득 텍스트를 띄우는지 여부

    AudioSource bgm_player;               // bgm_player의 AudioSource
    public Button[] bgm_button;           // 배경음 변경버튼의 배열

    public int index;                     // 변경된 배경음을 쉽게 저장할 수 있게 하는 변수

    void Awake() // 시작하자 마자
    {
        instance = this;

        // 애니매이션들을 초기화 함
        jelly_anim = jelly_panel.GetComponent<Animator>();
        plant_anim = plant_panel.GetComponent<Animator>();
        bgm_anim = bgm_panel.GetComponent<Animator>();

        isLive = true;

        jelatin_text.text = jelatin.ToString();                         // int형인 jelatin 변수를 String형으로 바꿔줌
        gold_text.text = gold.ToString();                               // int형인 gold 변수를 String형으로 바꿔줌
        unlock_group_gold_text.text = jelly_goldlist[0].ToString();     // int형 배열이였던 jelly_goldlist의 0번째 인덱스를 String형으로 바꿔줌
        lock_group_jelatin_text.text = jelly_jelatinlist[0].ToString(); // int형 배열이였던 jelly_jelatinlist의 0번째 인덱스를 String형으로 바꿔줌

        data_manager = data_manager_obj.GetComponent<DataManager>();    // data_manager 변수 초기화

        page = 0;
        jelly_unlock_list = new bool[12];

        bgm_player = GameObject.Find("BGM Player").GetComponent<AudioSource>();
    }

    void Start() // Awake() 함수 다음으로
    {
        // DataManager에 의해 데이터가 로드되기 전에 GameManager가 활성화 되어 빈 데이터를 참조하는 현상을 방지함
        Invoke("LoadData", 0.1f); // LoadData 함수를 지연시킴   /  Invoke() -> 함수 시작 시간을 지연시킴
    }

    void Update() // 매 프라임마다
    {
        if (Input.GetButtonDown("Cancel"))          // ESC 버튼을 눌렀을 때 (ESC를 누르면 true를 반환)
        {
            if (isJellyClick) ClickJellyBtn();      // 젤리 UI가 켜져 있었다면 (젤리 버튼이 눌렀을 때) 함수를 사용해서 UI를 내리고
            else if (isPlantClick) ClickPlantBtn(); // 플랜트 UI가 켜져 있었다면 (플랜트 버튼이 눌렀을 때) 함수를 사용해서 UI를 내리고
            else if (isBgmClick) ClickBgmBtn();     // 배경음 UI가 켜져 있었다면 (배경음 버튼이 눌렸을 때) 함수를 사용해서 UI를 내리고 
            else Option();                          // 아무 UI도 켜져 있지 않았다면 ESC 창을 올린다
        }

        // 치트키 (지구 젤리가 5개 생성됨)
        if (Input.GetKeyDown(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.F))
        {
            for (int i = 0; i < 5; i++)
            {
                GameObject obj = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
                Jelly jelly = obj.GetComponent<Jelly>();
                obj.name = "Jelly " + 11;
                jelly.id = 11;
                jelly.sprite_renderer.sprite = jelly_spritelist[11];

                jelly_list.Add(jelly);

                SoundManager.instance.PlaySound("Buy");
            }
        }
    }

    void FixedUpdate() // 일정한 간격마다
    {
        if (!isGetJelatin) StartCoroutine(GetRandomJelatin()); // 코루틴 함수 실행

        if (!isGetGold) StartCoroutine(GetRandomGold());       // 코루틴 함수 실행
    }

    void LateUpdate() // 모든 Update 함수들이 호출된 다음
    {
        // Format 함수를 통해 숫자 텍스트의 표현식을 지정, SmoothStep 함수를 통해 숫자가 변환되는 사이에 적절한 애니메이션을 추가함
        // jelatin_text와 gold_text의 경우 접근 지정자를 public으로 설정하여 유니티 프로그램에서 따로 지정함
        jelatin_text.text = string.Format("{0:n0}", (int)Mathf.SmoothStep(float.Parse(jelatin_text.text), jelatin, 0.5f));
        gold_text.text = string.Format("{0:n0}", (int)Mathf.SmoothStep(float.Parse(gold_text.text), gold, 0.5f));

        num_sub_text.text = "젤리 수용량 " + num_level * 2;                                // 기능을 알려주는 텍스트를 알맞게 바꿔준 다음
        if (num_level >= 5) num_btn.gameObject.SetActive(false);                          // 이미 젤리 아파트의 레벨이 최고 레벨이라면 버튼을 없애고
        else num_btn_text.text = string.Format("{0:n0}", num_gold_list[num_level]);       // 그게 아니라면 업그레이드에 필요한 골드를 띄움

        click_sub_text.text = "클릭 생산량 X " + click_level;                              // 기능을 알려주는 텍스트를 알맞게 바꿔준 다음
        if (click_level >= 5) click_btn.gameObject.SetActive(false);                      // 이미 젤리 꾹꾹이의 레벨이 최고 레벨이라면 버튼을 없애고
        else click_btn_text.text = string.Format("{0:n0}", click_gold_list[click_level]); // 그게 아니라면 업그레이드에 필요한 골드를 띄움
    }

    public void ChangeAc(Animator anim, int level)            // 위에 있는 배열과 함께 Animator 변경을 관리함
    {
        anim.runtimeAnimatorController = level_ac[level - 1]; // level_ac 배열의 최대 크기는 3 (애니매이션의 개수)이며, level 값은 1부터 3까지로 level 값이 배열의 크기를 벗어나지 않도록 -1 (배열은 0부터 시작)을 해주어야 함
        SoundManager.instance.PlaySound("Grow");              // 성장하는 사운드 출력
    }

    public void GetJelatin(int id, int level)      // 젤라틴 획득을 GameManager에서 관리하게 함
    {
        jelatin += (id + 1) * level * click_level; // 젤라틴이 id와 level, click_level, clear_boss에 비례해서 증가함

        if (jelatin > max_jelatin)                 // 만약 젤라틴이 한계를 넘었다면
            jelatin = max_jelatin;                 // 현재 젤라티의 수를 한계점과 같게 만듦
    }

    public void GetGold(int id, int level, Jelly jelly) // 골드 획득 또한 GameManager에서 관리하게 함
    {
        gold += jelly_goldlist[id] * level;             // 젤리의 id와 level, clear_boss에 따라서 골드 획득 후

        if (gold > max_gold)                            // 만약 골드가 한계를 넘었다면
            gold = max_gold;                            // 현재 골드의 수를 한계점과 같게 만든 뒤

        jelly_list.Remove(jelly);                       // 젤리를 없앰

        SoundManager.instance.PlaySound("Get Gold");    // 골드를 얻는 사운드 출력
    }

    public void CheckSell()
    {
        isSell = isSell == false;
    }

    // 각각 버튼이 클릭 될 시 발생하며 각각의 UI 창을 오르 내리는 역할을 수행
    public void ClickJellyBtn() // 젤리 버튼이 눌렀을 때
    {
        if (isPlantClick)                                   // 플랜트 버튼이 눌려 있다면
        {
            plant_anim.SetTrigger("doHide");                // 플랜트 UI를 숨기고
            SoundManager.instance.PlaySound("Pause Out");   // UI를 내리는 사운드 출력

            isPlantClick = false;                           // 플랜트 클릭을 비활성화시키고
            isLive = true;                                  // 젤리를 컨트롤 할 수 있게 함
        }

        if (isBgmClick)                                     // 배경음 버튼이 눌려 있다면
        {
            bgm_anim.SetTrigger("doHide");                  // 배경음 UI를 숨기고
            SoundManager.instance.PlaySound("Pause Out");   // UI를 내리는 사운드 출력

            isBgmClick = false;                             // 배경음 클릭을 비활성화시키고
            isLive = true;                                  // 젤리를 컨트롤 할 수 있게 함
        }

        if (isJellyClick)                                   // 젤리 버튼이 이미 눌려 있다면
        {
            jelly_anim.SetTrigger("doHide");                // 젤리 UI를 숨기고
            SoundManager.instance.PlaySound("Pause Out");   // UI를 내리는 사운드 출력
        }

        else                                                // 눌려 있지 않았다면
        {
            jelly_anim.SetTrigger("doShow");                // 젤리 UI를 보여주고
            SoundManager.instance.PlaySound("Pause In");    // UI를 올리는 사운드 출력
        }

        isJellyClick = !isJellyClick;                       // 그 다음 젤리 클릭을 반대 상태로 만들어 준다 (true 상태였으면 false로, false 상태였으면 true로 바꿈)
        isLive = !isLive;                                   // 라이브를 반대 상태로 만들어 준다 (true 상태였으면 false로, false 상태였으면 true로 바꿈)
    }


    public void ClickPlantBtn() // 플랜트 버튼이 눌렸을 때
    {
        if (isJellyClick)                                   // 젤리 버튼이 눌려 있다면
        {
            jelly_anim.SetTrigger("doHide");                // 젤리 UI를 숨기고
            SoundManager.instance.PlaySound("Pause Out");   // UI를 내리는 사운드 출력

            isJellyClick = false;                           // 젤리 클릭을 비활성화 시키고
            isLive = true;                                  // 젤리를 컨트롤 할 수 있게 함
        }

        if (isBgmClick)                                     // 배경음 버튼이 눌려 있다면
        {
            bgm_anim.SetTrigger("doHide");                  // 배경음 UI를 숨기고
            SoundManager.instance.PlaySound("Pause Out");   // UI를 내리는 사운드 출력

            isBgmClick = false;                             // 배경음 클릭을 비활성화시키고
            isLive = true;                                  // 젤리를 컨트롤 할 수 있게 함
        }

        if (isPlantClick)                                   // 플랜트 버튼이 이미 눌려 있다면
        {
            plant_anim.SetTrigger("doHide");                // 플랜트 UI를 숨기고
            SoundManager.instance.PlaySound("Pause Out");   // UI를 내리는 사운드 출력
        }
        else                                                // 눌려 있지 않았다면
        { 
            plant_anim.SetTrigger("doShow");                // 플랜트 UI를 보여주고
            SoundManager.instance.PlaySound("Pause In");    // UI를 올리는 사운드 출력
        }

        isPlantClick = !isPlantClick;                       // 그 다음 플랜트 클릭을 반대 상태로 만들어 준다 (true 상태였으면 false로, false 상태였으면 true로 바꿈)
        isLive = !isLive;                                   // 라이브를 반대 상태로 만들어 준다 (true 상태였으면 false로, false 상태였으면 true로 바꿈)
    }

    public void ClickBgmBtn() // 배경음 버튼이 눌렸을 때
    {
        if (isJellyClick)                                   // 젤리 버튼이 눌려 있다면
        {
            jelly_anim.SetTrigger("doHide");                // 젤리 UI를 숨기고
            SoundManager.instance.PlaySound("Pause Out");   // UI를 내리는 사운드 출력

            isJellyClick = false;                           // 젤리 클릭을 비활성화 시키고
            isLive = true;                                  // 젤리를 컨트롤 할 수 있게 함
        }

        if (isPlantClick)                                   // 플랜트 버튼이 이미 눌려 있다면
        {
            plant_anim.SetTrigger("doHide");                // 플랜트 UI를 숨기고
            SoundManager.instance.PlaySound("Pause Out");   // UI를 내리는 사운드 출력

            isPlantClick = false;                           // 플랜트 클릭을 비활성화시키고
            isLive = true;                                  // 젤리를 컨트롤 할 수 있게 함
        }

        if (isBgmClick)                                     // 배경음 버튼이 눌려 있다면
        {
            bgm_anim.SetTrigger("doHide");                  // 배경음 UI를 숨기고
            SoundManager.instance.PlaySound("Pause Out");   // UI를 내리는 사운드 출력
        }
        else                                                // 눌려 있지 않았다면
        {
            bgm_anim.SetTrigger("doShow");                  // 배경음 UI를 보여주고
            SoundManager.instance.PlaySound("Pause In");    // UI를 올리는 사운드 출력
        }

        isBgmClick = !isBgmClick;                           // 그 다음 배경음 클릭을 반대 상태로 만들어 준다 (true 상태였으면 false로, false 상태였으면 true로 바꿈)
        isLive = !isLive;                                   // 라이브를 반대 상태로 만들어 준다 (true 상태였으면 false로, false 상태였으면 true로 바꿈)
    }

    // 각각 버튼이 클릭 될 시 발생하며 각각의 UI 창을 오르 내리는 역할을 수행
    public void Option() // ESC 키를 눌렀을 때
    {
        isOption = !isOption;                                       // 옵션을 반대 상태로 만들어준다 (true 상태였으면 false로, false 상태였으면 true로 바꿈)
        isLive = !isLive;                                           // 라이브를 반대 상태로 만들어 준다 (true 상태였으면 false로, false 상태였으면 true로 바꿈)

        option_panel.gameObject.SetActive(isOption);                // 그 다음 옵션이 true 상태로 바뀌었다면 비활성화 되어 있던 ESC 창 UI를 활성화 시킴 
        Time.timeScale = isOption == true ? 0 : 1;                  // ESC 창이 열려 있다면 시간을 멈추고 아니라면 시간을 흐르게 함

        isGetJelatin = !isGetJelatin;
        random_jelatin_obj.gameObject.SetActive(isGetJelatin);      // 젤라틴 획득 텍스트를 잠시 비활성화 시킴

        isGetGold = !isGetGold;
        random_gold_obj.gameObject.SetActive(isGetGold);            // 골드 획득 텍스트를 잠시 비활성화 시킴
        
        if (!isOption) // 옵션 창이 켜져 있지 않다면
        {
            random_jelatin_obj.gameObject.SetActive(!isGetJelatin); // 젤라틴 획득 텍스트를 영구적으로 비활성화 시킴 (텍스트만 비활성화 시켰기만 했기 때문에 골드는 정상적으로 들어옴)

            random_gold_obj.gameObject.SetActive(!isGetGold);       // 골드 획득 텍스트를 영구적으로 비활성화 시킴 (텍스트만 비활성화 시켰기만 했기 때문에 골드는 정상적으로 들어옴)
        }
    }

    public void PageUp() // 다음 페이지 버튼을 누를 시
    {
        if (page >= 11)                              // 만약 최대 페이지를 넘겼으면 실행 X
        {   
            SoundManager.instance.PlaySound("Fail"); // 어떠한 동작에 실패했다는 사운드 출력
            return;   
        }

        ++page;                                      // 페이지 수 증가 (다음 페이지로 넘김)
        ChangePage();                                // 페이지를 바꾸는 함수 호출
    }

    public void PageDown() // 이전 페이지 버튼은 누를 시
    {
        if (page <= 0)                               // 0 페이지 전으로 넘기려 하면 실행 X
        {
            SoundManager.instance.PlaySound("Fail"); // 어떠한 동작에 실패했다는 사운드 출력
            return;    
        }    
        --page;                                      // 페이지 수 감소 (이전 페이지로 넘김)
        ChangePage();                                // 페이지를 바꾸는 함수
    }

    void ChangePage() // 이전 / 다음 페이지로 넘길 때 페이지가 바뀌는 함수
    {
        lock_group.gameObject.SetActive(!jelly_unlock_list[page]); // 해금하지 못한 페이지로 넘어가면 lock_group 이라는 오브젝트를 활성화함

        page_text.text = string.Format("#{0:00}", (page + 1)); // 페이지 수 증가시킴
        // 재화의 양을 문자열로 표시하거나, 스킬 데미지 비율 (백분율)과 같은 값들을 string.Format()을 이용해서 문자열의 형식을 편리하게 정의 할 수 있음
        // Format("...{0}, ... {1}", string , 22) -> ...string ... 22 / 중괄호 ({ }) 사이에 있는 숫자는 콤마 (,) 뒤에 정의해 놓은 실제 변수 이름 (또는 값)의 순서를 의미함 / 배열과 같이 0 부터 시작함

        // 각 오브젝트의 Sprite 또는 Text를 변경하여 마치 다음 페이지로 넘어가는 것처럼 구현함

        if (lock_group.activeSelf) // 해금하지 못한 페이지면 (lock_group 오브젝트가 활성화 되어 있으면)
        {
            lock_group_jelly_img.sprite = jelly_spritelist[page];                            // 해금하지 못한 젤리의 이미지 (Sprite)로 변경
            lock_group_jelatin_text.text = string.Format("{0:n0}", jelly_jelatinlist[page]); // 해금하지 못한 젤리의 이름 (Text)으로 변경
                                                                                             // 해금하지 못했으니 가격을 띄우지 않음

            lock_group_jelly_img.SetNativeSize();                                            // 이미지가 원래 크기로 돌아가도록 해서 이미지가 깨지는 현상을 방지함
        }
        else                       // 이미 해금한 페이지면 (lock_group 오브젝트가 활성화 되어 있지 않으면)
        {
            unlock_group_jelly_img.sprite = jelly_spritelist[page];                          // 해금한 젤리의 이미지 (Sprite)로 변경
            unlock_group_name_text.text = jelly_namelist[page];                              // 해금한 젤리의 이름 (Text)으로 변경
            unlock_group_gold_text.text = string.Format("{0:n0}", jelly_goldlist[page]);     // 젤리의 가격을 page 번째의 해금한 젤리의 가격으로 띄워줌

            unlock_group_jelly_img.SetNativeSize();                                          // 이미지가 원래 크기로 돌아가도록 해서 이미지가 깨지는 현상을 방지함
        }

        SoundManager.instance.PlaySound("Button");                                           // 버튼을 누르는 사운드 출력
    }

    public void Unlock() // 해금시키기
    {
        if (jelatin < jelly_jelatinlist[page])         // 현재 젤라틴이 해금하기 위한 젤라틴 수보다 적으면 
        {            
            SoundManager.instance.PlaySound("Fail");   // 어떠한 동작에 실패했다는 사운드 출력 후
            return;                                    // 실행 X
        }

        jelly_unlock_list[page] = true;                // 현재 페이지의 젤리를 해금하고
        ChangePage();                                  // 페이지를 바꿈

        jelatin -= jelly_jelatinlist[page];            // 젤라틴을 해금하기 위한 젤라틴의 수 만큼 지불함

        SoundManager.instance.PlaySound("Unlock");     // 젤리를 해금하는 사운드 출력
    }

    public void BuyJelly() // 젤리 구매 기능
    {
        if (gold < jelly_goldlist[page] || jelly_list.Count >= num_level * 2)            // 현재 가지고 있는 돈이 필요한 골드 수보다 적거나 현재 가지고 있는 젤리가 젤리 수용량의 한계보다 같거나 높으면 
        {  
            SoundManager.instance.PlaySound("Fail");                                     // 어떠한 동작에 실패했다는 사운드 출력 후 
            return;                                                                      // 실행 X
        }

        gold -= jelly_goldlist[page];                                                    // 골드를 구매에 필요한 골드 수 만큼 지불한 후

        GameObject obj = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity); // Instantiate로 Quaternion.identity 즉, 회전이 없는 오브젝트 obj 생성 
        Jelly jelly = obj.GetComponent<Jelly>();                                         // 방금 생성된 obj 오브젝트가 젤리의 컴퍼넌트를 가지게 함
        obj.name = "Jelly " + page;                                                      // obj 오브젝트의 이름을 "젤리 + 페이지 수" 로 만듦
        jelly.id = page;                                                                 // 젤리의 id를 페이지 수로 저장
        jelly.sprite_renderer.sprite = jelly_spritelist[page];                           // 해당 페이지에 있는 젤리가 생성되도록 함

        jelly_list.Add(jelly);                                                           // 그 다음 젤리를 생성함

        SoundManager.instance.PlaySound("Buy");                                          // 젤리를 구매하는 사운드 출력
    }

    public void NumUpgrade() // 젤리를 수용할 수 있는 한계를 늘려주는 함수
    {
        if (gold < num_gold_list[num_level])                                        // 현재 가지고 있는 돈이 필요한 골드 수보다 적으면
        { 
            SoundManager.instance.PlaySound("Fail");                                // 어떠한 동작에 실패했다는 사운드 출력 후
            return;                                                                 // 실행 X   
        }

        gold -= num_gold_list[num_level++];                                         // 골드를 구매에 필요한 골드 수 만큼 지불한 후

        num_sub_text.text = "젤리 수용량 " + num_level * 2;                          // 기능을 알려주는 텍스트를 띄움

        if (num_level >= 5) num_btn.gameObject.SetActive(false);                    // 만약 방금의 업그레이드로 최고 레벨을 달성한 상태가 된다면 버튼을 없애고
        else num_btn_text.text = string.Format("{0:n0}", num_gold_list[num_level]); // 그게 아니라면 그 다음 레벨의 골드를 보여줌

        SoundManager.instance.PlaySound("Buy");                                     // 업그레이드 하는 사운드 출력
    }

    public void ClickUpgrade() // 젤리를 클릭할 때 얻는 재화를 늘려주는 함수
    {
        if (gold < click_gold_list[click_level])                                          // 현재 가지고 있는 돈이 필요한 골드 수보다 적으면    
        {
            SoundManager.instance.PlaySound("Fail");                                      // 어떠한 동작에 실패했다는 사운드 출력 후 
            return;                                                                       // 실행 X                           
        }

        gold -= click_gold_list[click_level++];                                           // 골드를 구매에 필요한 골드 수 만큼 지불한 후

        click_sub_text.text = "클릭 생산량 X " + click_level;                              // 기능을 알려주는 텍스트를 띄움

        if (click_level >= 5) click_btn.gameObject.SetActive(false);                      // 만약 방금의 업그레이드로 최고 레벨을 달성한 상태가 된다면 버튼을 없애고
        else click_btn_text.text = string.Format("{0:n0}", click_gold_list[click_level]); // 그게 아니라면 그 다음 레벨의 골드를 보여줌

        SoundManager.instance.PlaySound("Buy");                                           // 업그레이드 하는 사운드 출력
    }

    public void SetRandomJelatin() // 랜덤하게 얻을 젤라틴의 값을 설정하고 그 설정 값에 따른 텍스트 변경
    {
        random_jelatin = Random.Range(10, 1001);      // 10 ~ 1000까지의 숫자중 하나 
        random_jelatin_variable = Random.Range(0, 6); // 0 ~ 5까지의 숫자중 하나
        random_jelatin_value = random_jelatin * random_jelatin_variable;

        // 랜덤으로 얻는 젤라틴 수에 따른 텍스트 변경
        if (random_jelatin_value == 5000) random_J_text = "최대 젤라틴입니다! 젤리, 그는 신이야!";
        else if (random_jelatin_value >= 4000) random_J_text = "좀만 더 가져오면 최대 젤라틴인데요?!";
        else if (random_jelatin_value > 2500) random_J_text = "최대 젤라틴의 절반 이상을 가져왔어요!";
        else if (random_jelatin_value >= 2000) random_J_text = "조금만 더 가져왔으면...쩝";
        else if (random_jelatin_value >= 1000) random_J_text = "그래도 이게 어디에요!";
        else if (random_jelatin_value >= 500) random_J_text = "살짝 아쉽네요!";
        else if (random_jelatin_value == 0) random_J_text = "젤리 멍청이~~~";
        else random_J_text = "너무 조금 가져오는거 아닌가요?! 짜다 짜~";

        random_jelatin_text.text = string.Format("젤리가 젤라틴 {0}개를 가져왔습니다!\n {1}", random_jelatin_value, random_J_text); // 텍스트 변경하기
    }

    IEnumerator GetRandomJelatin() // SetRandomJelatin() 함수에서 설정한 젤라틴 값을 받는 함수  /  코루틴 함수
    {
        jelatin_delay = Random.Range(20, 41);                                          // 젤라틴을 얻는 간격 -> 20 ~ 40초 사이
        jelatin_text_delay = 3;                                                        // 젤라틴을 얻었다는 사실을 알려주는 텍스트가 화면에 띄워져 있는 시간

        isGetJelatin = true;
        SetRandomJelatin();                                                            // 얻을 젤라틴 값을 설정하고

        isJ_Text = true;
        random_jelatin_obj.gameObject.SetActive(isJ_Text);                             // 텍스트 띄운 다음

        yield return new WaitForSeconds(jelatin_text_delay);                           // 텍스트를 띄우는 시간만큼 기다린 후

        isJ_Text = false;
        random_jelatin_obj.gameObject.SetActive(isJ_Text);                             // 텍스트를 내리고

        jelatin += random_jelatin_value;                                               // 젤라틴 획득 후

        if (random_jelatin_value != 0) SoundManager.instance.PlaySound("Get Jelatin"); // 얻는 젤라틴이 0이 아니면 젤라틴을 획득하는 소리 출력하고
        else SoundManager.instance.PlaySound("Touch");                                 // 얻는 젤라틴이 0이면 터치하는 소리를 내고 나서 (터치하는 소리지만 어울림)

        yield return new WaitForSeconds(jelatin_delay);                                // 젤라틴을 얻는 간격만큼 기다림
        isGetJelatin = false;
    }

    public void SetRandomGold() // 랜덤하게 얻을 골드의 값을 설정하고 그 설정 값에 따른 텍스트 변경
    {
        random_gold = Random.Range(0, 2501);       // 0 ~ 2500까지의 숫자중 하나 
        random_gold_variable = Random.Range(0, 6); // 0 ~ 5까지의 숫자중 하나
        random_gold_value = random_gold * random_gold_variable;

        // 랜덤으로 얻는 골드 수에 따른 텍스트 변경
        if (random_gold_value == 12500) random_G_text = "최대 골드입니다! 젤리, 그는 신이야!";
        else if (random_gold_value >= 10000) random_G_text = "좀만 더 가져오면 최대 골드인데요?!";
        else if (random_gold_value > 6250) random_G_text = "최대 골드의 절반 이상을 가져왔어요!";
        else if (random_gold_value >= 4500) random_G_text = "조금만 더 가져왔으면...쩝";
        else if (random_gold_value >= 3250) random_G_text = "그래도 이게 어디에요!";
        else if (random_gold_value >= 1500) random_G_text = "살짝 아쉽네요!";
        else if (random_gold_value == 0) random_G_text = "젤리 멍청이~~~";
        else random_G_text = "너무 조금 가져오는거 아닌가요?! 짜다 짜~";

        random_gold_text.text = string.Format("젤리가 골드 {0}개를 가져왔습니다!\n {1}", random_gold_value, random_G_text); // 텍스트 변경하기
    }

    IEnumerator GetRandomGold() // SetRandomGold() 함수에서 설정한 골드 값을 받는 함수  /  코루틴 함수
    {
        gold_delay = Random.Range(30, 46);                                       // 골드를 얻는 간격 -> 30 ~ 45초 사이
        gold_text_delay = 3;                                                     // 골드를 얻었다는 사실을 알려주는 텍스트가 화면에 띄워져 있는 시간

        isGetGold = true;
        SetRandomGold();                                                         // 얻을 골드 값을 설정하고

        isG_Text = true;
        random_gold_obj.gameObject.SetActive(isG_Text);                          // 텍스트 띄운 다음

        yield return new WaitForSeconds(gold_text_delay);                        // 텍스트를 띄우는 시간만큼 기다린 후

        isG_Text = false; 
        random_gold_obj.gameObject.SetActive(isG_Text);                          // 텍스트를 내리고

        gold += random_gold_value;                                               // 골드 획득 후

        if (random_gold_value != 0) SoundManager.instance.PlaySound("Get Gold"); // 얻는 골드가 0이 아니면 골드를 획득하는 소리 출력하고
        else SoundManager.instance.PlaySound("Touch");                           // 얻는 골드가 0이면 터치하는 소리를 내고 나서 (터치하는 소리지만 어울림)

        yield return new WaitForSeconds(gold_delay);                             // 골드를 얻는 간격만큼 기다림
        isGetGold = false;
    }

    public void ChangeBgm(int bgm_button) // 배경음 변경 버튼을 눌렀을 때  /  배열 bgm_button 중 누른 버튼의 인덱스 값을 int형으로 받아서 매개변수로 사용함 
    {
        index = bgm_button;                                       // 저장하기 쉽게 bgm_button의 인덱스 값을 index 변수로 담음
        bgm_player.clip = SoundManager.instance.bgm_clips[index]; // 배경음 플레이어에 방금 저장한 index 값에 해당하는 배경음으로 설정 후
        bgm_player.Play();                                        // 배경음을 플레이함
    }

    void LoadData() // 데이터 불러오기
    {
        lock_group.gameObject.SetActive(!jelly_unlock_list[page]); // 해금 되어 있지 않은 젤리 리스트를 불러옴

        bgm_player.clip = SoundManager.instance.bgm_clips[index];  // 배경음 불러옴
        bgm_player.Play();                                         // 배경음 불러옴

        for (int i = 0; i < jelly_data_list.Count; ++i) // 저장 할 때 있었던 젤리의 개수 만큼 반복
        {
            GameObject obj = Instantiate(prefab, jelly_data_list[i].pos, Quaternion.identity); // 각각의 젤리들이 있었던 위치에 젤리들 생성
            Jelly jelly = obj.GetComponent<Jelly>();                                           // 각각의 젤리들에게 Jelly 컴퍼넌트를 가지게 함
            jelly.id = jelly_data_list[i].id;                                                  // 각각의 젤리들에게 id부여
            jelly.level = jelly_data_list[i].level;                                            // 각각의 젤리들에게 level부여
            jelly.exp = jelly_data_list[i].exp;                                                // 각각의 젤리들에게 경험치부여
            jelly.sprite_renderer.sprite = jelly_spritelist[jelly.id];                         // 각각의 젤리들에게 id에 맞는 종류의 젤리로 렌더링함
            jelly.anim.runtimeAnimatorController = level_ac[jelly.level - 1];                  // 각각의 젤리들에게 자신의 레벨에 맞는 애니매이션 (크기)을 가지게 함
            obj.name = "Jelly " + jelly.id;                                                    // 각각의 젤리들에게 맞는 이름 부여

            jelly_list.Add(jelly);                                                             // 위에서 만들었던 젤리들을 모두 생성함
        }
    }

    public void Exit()                                // 게임에서 나갈 때
    {
        data_manager.JsonSave();                      // 저장함

        SoundManager.instance.PlaySound("Pause Out"); // 나가는 사운드 출력

        Application.Quit();                           // 게임에서 나가짐 (유니티 프로그램에서는 Application.Quit() 함수가 작동 하지 않는 것이 정상임)
    }
}
