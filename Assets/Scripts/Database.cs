using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using System;

public class Database : MonoBehaviour
{
    // Start is called before the first frame update
    private string dbFileName = "2048.sqlite";
    private string dbPath;

    public static Database instance;
    private void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        // Thiết lập đường dẫn cơ sở dữ liệu
        dbPath = Path.Combine(Application.persistentDataPath, dbFileName);
    Debug.Log("start"+ dbPath);
        // Kiểm tra và sao chép cơ sở dữ liệu từ StreamingAssets
        CopyDatabaseIfNotExists();
    }

    private void CopyDatabaseIfNotExists()
{
    if (!File.Exists(dbPath))
    {
        string streamingDbPath = Path.Combine(Application.streamingAssetsPath, dbFileName);

        if (Application.platform == RuntimePlatform.Android)
        {
            CopyDatabaseFromStreamingAssetsToPersistent();
        }
        else
        {
            File.Copy(streamingDbPath, dbPath);
            Debug.Log("Database copied to: " + dbPath);
        }
    }
    else
    {
        Debug.Log("Database already exists at: " + dbPath);
    }
}

    private void CopyDatabaseFromStreamingAssetsToPersistent()
    {
        string sourcePath = Path.Combine(Application.streamingAssetsPath, dbFileName);
        
        if (sourcePath.Contains("://") || sourcePath.Contains(":///"))
        {
            // Đọc file từ stream nếu nó nằm trong file .apk (trên Android)
            WWW reader = new WWW(sourcePath);
            while (!reader.isDone) { }

            // Ghi lại file vào persistentDataPath
            File.WriteAllBytes(dbPath, reader.bytes);
            Debug.Log("Database copied to: " + dbPath);
        }
        else
        {
            // Nếu không thì dùng phương thức bình thường
            File.Copy(sourcePath, dbPath);
        }
    }

    public string GetSkipCell(int id)
    {
        string skipCell = "";

        using (var connection = new SqliteConnection("URI=file:" + dbPath))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                // Query to get the list of unlocked cars for the player
                command.CommandText = "SELECT CellSkip FROM Map WHERE id = @Id";
                command.Parameters.AddWithValue("@Id", id);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        skipCell = reader["CellSkip"].ToString();
                    }
                }
            }
        }
        return skipCell;
    }
    public int GetMaxN(int id)
    {
        int MaxN = 0;

        using (var connection = new SqliteConnection("URI=file:" + dbPath))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                // Query to get the list of unlocked cars for the player
                command.CommandText = "SELECT MaxN FROM Map WHERE id = @Id";
                command.Parameters.AddWithValue("@Id", id);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        MaxN = Convert.ToInt16(reader["MaxN"]);
                    }
                }
            }
        }
        return MaxN;
    }

    public int GetMaxD(int id)
    {
        int MaxD = 0;

        using (var connection = new SqliteConnection("URI=file:" + dbPath))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                // Query to get the list of unlocked cars for the player
                command.CommandText = "SELECT MaxD FROM Map WHERE id = @Id";
                command.Parameters.AddWithValue("@Id", id);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        MaxD = Convert.ToInt16(reader["MaxD"]);
                    }
                }
            }
        }
        return MaxD;
    }

    public int GetScore(int id)
    {
        int Score = 0;
        using (var connection = new SqliteConnection("URI=file:" + dbPath))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                // Query to get the list of unlocked cars for the player
                command.CommandText = "SELECT Score FROM Map WHERE id = @Id";
                command.Parameters.AddWithValue("@Id", id);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Score = Convert.ToInt16(reader["Score"]);
                    }
                }
            }
        }
        return Score;
    }

    public int getHightestLever()
    {
        int highestLevel = 0;
        using (var connection = new SqliteConnection("URI=file:" + dbPath))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                // Query to get the list of unlocked cars for the player
                command.CommandText = "SELECT MAX(id) FROM Map WHERE Active = 1";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        highestLevel = reader.GetInt16(0);
                    }
                }
            }
        }
        return highestLevel;
    }

    public int getMoney()
    {
        int money = 0;
        using (var connection = new SqliteConnection("URI=file:" + dbPath))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                // Query to get the list of unlocked cars for the player
                command.CommandText = "SELECT Money FROM User WHERE id = 1";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        money = reader.GetInt16(0);
                    }
                }
            }
        }
        return money;
    }

    public void updateMoney(int money){
        using (var connection = new SqliteConnection("URI=file:" + dbPath))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                // Query to get the list of unlocked cars for the player
                command.CommandText = "UPDATE User set Money = Money + @money WHERE id = 1";
                command.Parameters.AddWithValue("@money",money);
                command.ExecuteNonQuery();
            }
        }
    }

    public int getBoom()
    {
        int boom = 0;
        using (var connection = new SqliteConnection("URI=file:" + dbPath))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                // Query to get the list of unlocked cars for the player
                command.CommandText = "SELECT Boom FROM User WHERE id = 1";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        boom = reader.GetInt16(0);
                    }
                }
            }
        }
        return boom;
    }

    public void updateBoom(int value){
        using (var connection = new SqliteConnection("URI=file:" + dbPath))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                // Query to get the list of unlocked cars for the player
                command.CommandText = "UPDATE User set Boom = Boom + @value WHERE id = 1";
                command.Parameters.AddWithValue("@value",value);
                command.ExecuteNonQuery();
            }
        }
    }

    public int getUndo()
    {
        int undo = 0;
        using (var connection = new SqliteConnection("URI=file:" + dbPath))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                // Query to get the list of unlocked cars for the player
                command.CommandText = "SELECT Undo FROM User WHERE id = 1";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        undo = reader.GetInt16(0);
                    }
                }
            }
        }
        return undo;
    }

    public void updateUndo(int value){
        using (var connection = new SqliteConnection("URI=file:" + dbPath))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                // Query to get the list of unlocked cars for the player
                command.CommandText = "UPDATE User set Undo = Undo + @value WHERE id = 1";
                command.Parameters.AddWithValue("@value",value);
                command.ExecuteNonQuery();
            }
        }
    }

    public void updateLevel(int level){
        using (var connection = new SqliteConnection("URI=file:" + dbPath))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                // Query to get the list of unlocked cars for the player
                command.CommandText = "UPDATE Map set Active = 1 WHERE id = @level";
                command.Parameters.AddWithValue("@level",level);
                command.ExecuteNonQuery();
            }
        }
    }
}
