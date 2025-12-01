using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class MyWindow : EditorWindow
{
    string name;
    string team;
    int jerseynum;
    bool healthy;

    public enum POSITIONS
    {
        CENTRE = 0,
        LEFTWING = 1,
        RIGHTWING = 2,
        DEFENCE = 3,
        GOALIE = 4
    }

    POSITIONS pos;
    bool active;

    float speed;
    float strength;
    float agility;

    int yearsInLeague;
    int goals;
    int assists;

    public enum SHAPES
    {
        CUBE = 0,
        PLANE = 1,
        SPHERE = 2,
        CAPSULE = 3,
        QUAD = 4,
        PARTICLE = 5
    }
    SHAPES newshape;
    List<SHAPES> s = new List<SHAPES>();

    [MenuItem("PROG56693/My Window")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(MyWindow));
    }

    private void OnGUI()
    {
        GUILayout.Label("Hockey Player Editor", EditorStyles.boldLabel);

        name = EditorGUILayout.TextField("Player's full name", name);
        team = EditorGUILayout.TextField("Team", team);
        pos = (POSITIONS)EditorGUILayout.EnumPopup("Position", pos);
        jerseynum = EditorGUILayout.IntSlider("Jersey Num", jerseynum, 0, 100);

        active = EditorGUILayout.BeginToggleGroup("Not Retired", active);
            healthy = EditorGUILayout.Toggle("Healthy: ", healthy);
            speed = EditorGUILayout.Slider("Speed", speed, 0, 100);
            strength = EditorGUILayout.Slider("Strength", strength, 0, 100);
            agility = EditorGUILayout.Slider("Agility", agility, 0, 100);
            yearsInLeague = EditorGUILayout.IntSlider("Years", yearsInLeague, 0, 100);
            goals = EditorGUILayout.IntSlider("Goals", goals, 0, 500);
            assists = EditorGUILayout.IntSlider("Assists", assists, 0, 500);
        EditorGUILayout.EndToggleGroup();

        if (GUILayout.Button("Save Entry"))
        {
            Debug.Log("Saving Data...");

            GameData g = new GameData();
            g.playername = name;
            g.team = team;
            g.jerseynum = jerseynum;
            g.pos = (int)pos;

            g.active = active;
            g.healthy = healthy;
            g.speed = speed;
            g.strength = strength;
            g.agility = agility;
            g.yearsInLeague = yearsInLeague;
            g.goals = goals;
            g.assists = assists;

            GameDataManager gm = new GameDataManager();
            gm.gd = g;
            gm.writeFile();
        }

        if(GUILayout.Button("Retrieve Data"))
        {
            Debug.Log("Retrieving Data....");
            GameDataManager gm = new GameDataManager();
            gm.gd = new GameData();
            gm.readFile();

            GameData g = gm.gd;

            name = g.playername;
            team = g.team;
            jerseynum = g.jerseynum;
            pos = (POSITIONS)g.pos;

            active = g.active;
            healthy = g.healthy;
            speed = g.speed;
            strength = g.strength;
            agility = g.agility;
            yearsInLeague = g.yearsInLeague;
            goals = g.goals;
            assists = g.assists;



        }


        GUILayout.Label("Name: " + name);
        GUILayout.Label("Team: " + team);
        GUILayout.Label("Jersey: " + jerseynum);
        GUILayout.Label("Position: " + pos);

        if (active)
        {
            GUILayout.Label("Status: Active");
            GUILayout.Label("Speed: " + speed);
            GUILayout.Label("Strength: " + strength);
            GUILayout.Label("Agility: " + agility);
            GUILayout.Label("Years: " + yearsInLeague);
            GUILayout.Label("Goals: " + goals);
            GUILayout.Label("Assists: " + assists);

        }
        else
        {
            GUILayout.Label("Status: Retired");
        }

        newshape = (SHAPES)EditorGUILayout.EnumPopup("Choose Shape To Add", newshape);
        if (GUILayout.Button("Add New Object"))
        {
            Debug.Log("Inserting...");
            // Create a custom game object

            GameObject g;

            if (newshape == SHAPES.PLANE)
                g = GameObject.CreatePrimitive(PrimitiveType.Plane);
            else if (newshape == SHAPES.CUBE)
            {
                g = GameObject.CreatePrimitive(PrimitiveType.Cube);
                g.transform.position = new Vector3(0, 0.5f, 0);
            }
            else if (newshape == SHAPES.SPHERE)
            {
                g = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                g.transform.position = new Vector3(0, 1.5f, 0);
            }
            else if (newshape == SHAPES.QUAD)
            {
                g = GameObject.CreatePrimitive(PrimitiveType.Quad);
                g.transform.position = new Vector3(2, 1, 0);
            }
            else
            {
                g = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                g.transform.position = new Vector3(-2, 1, 0);

            }

            // Ensure it gets reparented if this was a context click (otherwise does nothing)
            GameObjectUtility.SetParentAndAlign(g, Selection.activeObject as GameObject);
            // Register the creation in the undo system
            Undo.RegisterCreatedObjectUndo(g, "Create " + g.name);
            Selection.activeObject = g;
        }



    }



}
