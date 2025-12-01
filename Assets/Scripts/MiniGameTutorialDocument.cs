using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MiniGameTutorialDocument : MonoBehaviour
{
    private string[] PlayerNames = { "Jim Kirk", "Bones McKoy", "Uhura", "Spock", "Jean Luc Picard", "Will Riker" };
    private string[] TutorialText = {"Welcome To The Mini Game", "Press A for Attack 1", "Press S for Attack 2",
                                     "Press D for Attack 3", "Press Space To Fire Projectiles",
                                     "Press Space Rapidly To Fire Lots Of Projectiles",
                                     "Enjoy!"};
    public Texture2D[] GundamLogos;
    public string[] GundamNames;

    private int TutorialNum = 0;

    //   public Text TutorialLabel;
    //   public GameObject TutorialWindow;

    private UIDocument uiDocument;
    private VisualElement frame;
    private VisualElement frame2;
    private VisualElement frame3;
    private Label label;
    private Button button;
    private Button btnTutReload;
    private ProgressBar progress;
    private ListView lstOptions;
    private Toggle cbListEnable;
    private Foldout flGundams;
    private VisualElement flGundamVisualElement;
    private ScrollView scFoldout;

    private void OnEnable()
    {
        //   if (PlayerPrefs.HasKey("MiniGameTutorialDone"))
        //    {
        uiDocument = GetComponent<UIDocument>();
        var rootVisualElement = uiDocument.rootVisualElement;
        frame = rootVisualElement.Q<VisualElement>("VisualElement");
        frame2 = rootVisualElement.Q<VisualElement>("VisualElement2");

        label = frame.Q<Label>("lblMiniGameTutorialText");
        button = frame.Q<Button>("btnMGTutNext");
        

        label.text = TutorialText[TutorialNum];
        button.RegisterCallback<ClickEvent>(ev => NextTutorialMessage());

        btnTutReload = frame2.Q<Button>("btnMgShowTutorial");
        btnTutReload.RegisterCallback<ClickEvent>(ev => ReloadTutorial());

        progress = frame.Q<ProgressBar>("prTutorials");
        progress.value = TutorialNum + 1;
        progress.highValue = TutorialText.Length;

        if (PlayerPrefs.GetInt("MiniGameTutorialDone") == 1) // not necessary
        {
            
            frame.style.display = DisplayStyle.None;
        }

        lstOptions = rootVisualElement.Q<ListView>("lstOptions");

        cbListEnable = frame2.Q<Toggle>("cbList");
        cbListEnable.RegisterCallback<ChangeEvent<bool>>(ListEnableEvent);
        lstOptions.style.display = DisplayStyle.None;

        /*
        Func<VisualElement> makeItem = () => new Label();
        Action<VisualElement, int> bindItem = (e, i) => (e as Label).text = PlayerNames[i];
         lstOptions.makeItem = makeItem;
        lstOptions.bindItem = bindItem;
        lstOptions.itemsSource = PlayerNames;
        lstOptions.selectionType = SelectionType.Multiple;
        */

        Func<VisualElement> makeItem = () =>
        {
            var box = new VisualElement();
            box.style.flexDirection = FlexDirection.Row;
        //    box.style.alignItems = Align.FlexStart;
            box.Add(new Image());
            box.Add(new Label());
            box.Add(new Button());
            box.style.height = 60;
            flGundamVisualElement = box;
            return box;
        };

        Action<VisualElement, int> bindItem = (e, i) =>
        {
            (e.ElementAt(0) as Image).image = GundamLogos[i];
         //   (e.ElementAt(0) as Image).style.alignContent = Align.FlexStart;
            (e.ElementAt(1) as Label).text = GundamNames[i].ToString();
            (e.ElementAt(1) as Label).style.width = 200;
           (e.ElementAt(2) as Button).text = "Save";
            (e.ElementAt(2) as Button).RegisterCallback<ClickEvent>(ev => ListItemClicked(i));
        };

        lstOptions.makeItem = makeItem;
        lstOptions.bindItem = bindItem;
        lstOptions.itemsSource = GundamNames;
        lstOptions.selectionType = SelectionType.Multiple;

        // ListView listView = rootVisualElement.Q<ListView>(className: "the-uxml-listview");
        //var listView = rootVisualElement.Q<ListView>();


        // Callback invoked when the user double clicks an item
        lstOptions.onItemsChosen += Debug.Log;

        // Callback invoked when the user changes the selection inside the ListView
        lstOptions.onSelectionChange += Debug.Log;

        scFoldout = rootVisualElement.Q<ScrollView>("scFoldout");
        frame3 = rootVisualElement.Q<VisualElement>("VisualElement3");
        flGundams = frame3.Q<Foldout>("flGundams");
        flGundams.text = "Gundam Names";
        flGundams.value = false;

        for (int i = 0; i < GundamNames.Length; i++)
        {
            var box = new VisualElement();
          

            var f = new Foldout();
            f.text = GundamNames[i];
            f.value = false;
            
            var subbox = new VisualElement();
            subbox.style.flexDirection = FlexDirection.Row;
            Image im = new Image();
            im.image = GundamLogos[i];
            im.style.height = 30;
            subbox.Add(im);
          //  Label l = new Label();
          //  l.text = GundamNames[i];
          //  box.Add(l);
            Button b = new Button();
            b.text = "Save";
            //b.RegisterCallback<ClickEvent>(ev => ListItemClicked(i));
            subbox.Add(b);
           // subbox.style.height = 60;
            f.contentContainer.Add(subbox);
            f.style.height = 60;
            box.contentContainer.Add(f);
            box.style.height = 60;
            
            flGundams.contentContainer.Add(box);
        }

    }

    private void ListEnableEvent(ChangeEvent<bool> evt)
    {
        Debug.Log("Toggle Event: " + evt.newValue);

        if(evt.newValue == true)
        {
            lstOptions.style.display = DisplayStyle.Flex;
            scFoldout.style.display = DisplayStyle.None;
        }
        else
        {
            lstOptions.style.display = DisplayStyle.None;
            scFoldout.style.display = DisplayStyle.Flex;
        }
    }

    private void ListItemClicked(int i)
    {
        Debug.Log("Gundam Saved: " + GundamNames[i]);
    }

    private void ReloadTutorial()
    {
        PlayerPrefs.SetInt("MiniGameTutorialDone", 0);
        frame.style.display = DisplayStyle.Flex;
        TutorialNum = 0;
        label.text = TutorialText[TutorialNum];

        progress.value = TutorialNum + 1;

    }
    private void NextTutorialMessage()
    {
        TutorialNum++;
        progress.value = TutorialNum + 1;
        if (TutorialNum >= TutorialText.Length)
        {
            PlayerPrefs.SetInt("MiniGameTutorialDone", 1);
           // uiDocument.rootVisualElement.style.display = DisplayStyle.None;
            frame.style.display = DisplayStyle.None;
        }
        else
        {
            label.text = TutorialText[TutorialNum];
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
