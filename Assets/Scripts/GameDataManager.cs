using System.Collections;
using System.Collections.Generic;
using System.IO;
using Mono.Data.Sqlite;
using System.Data;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    private string fileLocation = Application.persistentDataPath + "/gd.json";
    public GameData gd = new GameData();

    public List<GameData> prevPlayers = new List<GameData>();

    public void readFile()
    {

        if (File.Exists(fileLocation))
        {
            string fileContents = File.ReadAllText(fileLocation);
            Debug.Log("file contents retrieved: " + fileContents);



            gd = JsonUtility.FromJson<GameData>(fileContents);
        }


            IDbConnection dbConnection = CreateAndOpenDatabase(); 
            IDbCommand dbCommandReadValues = dbConnection.CreateCommand(); 
            dbCommandReadValues.CommandText = "SELECT * FROM HockeyPlayers"; 
            IDataReader dataReader = dbCommandReadValues.ExecuteReader(); 

            while (dataReader.Read()) 
            {
                GameData g = new GameData();

                g.playername = dataReader.GetString(1);
                g.team = dataReader.GetString(2);
                g.jerseynum = dataReader.GetInt32(3);
                g.healthy = dataReader.GetBoolean(4);
                g.pos = dataReader.GetInt32(5);
                g.active = dataReader.GetBoolean(6);
                g.speed = dataReader.GetFloat(7);
                g.strength = dataReader.GetFloat(8);
                g.agility = dataReader.GetFloat(9);
                g.yearsInLeague = dataReader.GetInt32(10);
                g.goals = dataReader.GetInt32(11);
                g.assists = dataReader.GetInt32(12);

                prevPlayers.Add(g);
                Debug.Log("Player Retrieved: " + g.playername);
            }
            Debug.Log("Total Players Retrieved: " + prevPlayers.Count);
            
            dbConnection.Close(); 

        
    }

    public void writeFile()
    {

        string jsonString = JsonUtility.ToJson(gd);
        Debug.Log("JSON string is: " + jsonString);
        File.WriteAllText(fileLocation, jsonString);
        Debug.Log("Data Saved to: " + fileLocation);

        // Insert a value into the table.
        IDbConnection dbConnection = CreateAndOpenDatabase();
        IDbCommand cmd = dbConnection.CreateCommand();
        cmd.CommandText = "INSERT OR REPLACE INTO HockeyPlayers VALUES (null, '" 
                           + gd.playername + "', '" + gd.team + "', " + gd.jerseynum + ", "
                           + b2i(gd.healthy) + ", " + gd.pos + ", " + b2i(gd.active) + ", "
                           + gd.speed + ", " + gd.strength + ", " + gd.agility + ", " 
                           + gd.yearsInLeague + ", " + gd.goals + ", " + gd.assists + ");";
   

        cmd.ExecuteNonQuery();

        // Remember to always close the connection at the end.
        dbConnection.Close();
    }

    private int b2i(bool b)
    {
        return (b == true) ? 1 : 0;
    }

    private bool i2b(int i)
    {
        return (i == 1) ? true : false;
    }
    

    private IDbConnection CreateAndOpenDatabase() 
    {
       
        string dbUri = "URI=file:MyDatabase.sqlite"; 
        IDbConnection dbConnection = new SqliteConnection(dbUri); 
        dbConnection.Open(); 

        IDbCommand dbCommandCreateTable = dbConnection.CreateCommand(); 
        dbCommandCreateTable.CommandText = "CREATE TABLE IF NOT EXISTS HockeyPlayers "
                                        + "(id INTEGER PRIMARY KEY, name text, team text, jerseynum integer, healthy integer, "
                                        + "position integer, active integer, speed real, strength real, agility real, "
                                        + "years integer, goals integer, assists integer)";
        dbCommandCreateTable.ExecuteReader(); 
           
        return dbConnection;
    } 
}




   

