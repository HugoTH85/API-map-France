using System;
using MySql.Data.MySqlClient;

class DBcontext
{
    MySqlConnection? conn;
    public DBcontext(){
        string myConnectionString = "server=172.23.0.3;uid=root;pwd=root;database=projet_archi";
        this.conn = null;

        try{
            this.conn = new MySqlConnection(myConnectionString);
            this.conn.Open();
            Console.WriteLine("Connexion réussie !");
        }
        catch (MySqlException ex){
            AfficherErreur("Erreur de connexion : " + ex.Message);
        }
    }

    private static void AfficherErreur(string message)
    {
        Console.WriteLine("ERR - " + message);
    }

    public List<object>? ExecuteQuery(string query){
        try
        {
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    // Traitement des résultats ici
                    int columnCount = reader.FieldCount;
                    List<object> L = new List<object>();

                    while (reader.Read())
                    {
                        List<object> row = new List<object>();
                        for (int i = 0; i < columnCount; i++)
                        {
                            // Accéder aux colonnes avec reader[i]
                            //Console.Write(reader[i] + "\t");
                            row.Add(reader[i]);
                        }
                        L.Add(row);
                        //Console.WriteLine();
                    }
                    return L;
                }
            }
        }
        catch (MySqlException ex)
        {
            AfficherErreur("Erreur d'exécution de la requête : " + ex.Message);
            return null;
        }
    }

    public void CloseConnection(){
        if (this.conn!=null){
            this.conn.Close();
            Console.WriteLine("Fermeture de la connexion à la base de données.");
        }
    }
}