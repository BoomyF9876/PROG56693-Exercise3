using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MenuExercise : MonoBehaviour
{
    static string path = "E:\\Google Drive\\work\\College_Teaching\\Sheridan\\PROG56693_DataDrivenGaming\\Sample Code\\week5\\W5WPFtoSQLite\\bin\\Debug\\";
    static System.Diagnostics.Process proc = new System.Diagnostics.Process();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [MenuItem("PROG56693/Say Hello")]
    static void SayHello()
    {
        Debug.Log("Hello World");
    }

    [MenuItem("PROG56693/Log Selected Transform Name")]
    static void LogSelectedTransformName()
    {
        Debug.Log("Selected Transform is on " + Selection.activeTransform.gameObject.name + ".");
    }

    [MenuItem("PROG56693/Log Selected Transform Name", true)]
    static bool ValidateLogSelectedTransformName()
    {
        // Return false if no transform is selected.
        return Selection.activeTransform != null;
    }

    [MenuItem("PROG56693/Do Something with a Shortcut Key %g")]
    static void DoSomethingWithAShortcutKey()
    {
        Debug.Log("Doing something with a Shortcut Key...");
    }

    [MenuItem("CONTEXT/Rigidbody/Double Mass")]
    static void DoubleMass(MenuCommand command)
    {
        Rigidbody body = (Rigidbody)command.context;
        body.mass = body.mass * 2;
        Debug.Log("Doubled Rigidbody's Mass to " + body.mass + " from Context Menu.");
    }

    [MenuItem("GameObject/MyCategory/Custom Game Object", false, 10)]
    static void CreateCustomGameObject(MenuCommand menuCommand)
    {
        // Create a custom game object
        GameObject go = new GameObject("Custom Game Object");
        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
    }


   

    [MenuItem("PROG56693/Launch My Character Editor")]
    static void LaunchCharacterEditor()
    {
        Debug.Log("Loading My Character Editor");

        proc.StartInfo.FileName = path + "W4WPFtoMongoDB.exe";
        proc.StartInfo.Arguments = path;
        proc.Start();
    }

    [MenuItem("PROG56693/Load Dialogbox")]
    static void LoadDialogbox()
    {
        EditorUtility.DisplayDialog("Place Selection On Surface?",
               "Are you sure you want to place " + Selection.activeTransform.name
               + " on the surface?", "Place", "Do Not Place");

    }

    [MenuItem("PROG56693/Load Dialogbox", true)]
    static bool ValidateLoadDialogbox()
    {
        return Selection.activeTransform != null;
    }

    [MenuItem("PROG56693/Get Help On the Editor")]
    static void LoadHelp()
    {
        Help.ShowHelpForObject(Selection.activeTransform);
    }

    [MenuItem("PROG56693/Get Help From The Web")]
    static void LoadHelpWeb()
    {
        Help.BrowseURL("https://forum.unity3d.com/search.php");

    }

}
