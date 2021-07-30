
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Scrape.Classi;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Scrape
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }
        private void Live_Click(object sender, EventArgs e)
        {
            getLive fd = new getLive();
            List<Partita> partite = new List<Partita>();
            partite = fd.live();

            SqlConnection connection;
            string connectionString;
            connectionString = @"Data Source=pinexp.ns0.it\MIOSERVER,65004;" + "Initial Catalog=Soccer;" + @"User id=sa;" + "Password=Pinexp93;";
            connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    SqlCommand cmd = new SqlCommand("TRUNCATE TABLE Match", connection);
                    cmd.ExecuteNonQuery();

                    string campionato = "";

                    for (int i = 0; i < partite.Count; i++)
                    {
                        string query = "SELECT id FROM Lega WHERE Nome = '" + partite[i].Campionato + "'";
                        SqlCommand command = new SqlCommand(query, connection);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                campionato = reader.GetString(0);
                            }
                        }

                        cmd = new SqlCommand("INSERT INTO Partita (id_Lega,NomeCasa,LinkCasa,NomeFuori,LinkFuori" +
                                                                   "Orario,Risultato,Data,Quota_1,Quota_X,Quota_2,Quota_Under05" +
                                                                   "Quota_Over05,Quota_Under15,Quota_Over15,Quota_Under25," +
                                                                   "Quota_Over25,Quota_Under35,Quota_Over35,Quota_1X,Quota_X2," +
                                                                   "Quota_12,Quota_Goal,Quota_NoGoal) " +
                                                                   "values " +
                                                                   "(@id_Lega,@NomeCasa,@LinkCasa,@NomeFuori,@LinkFuori" +
                                                                   "@Orario,@Risultato,@Data,@Quota_1,@Quota_X,@Quota_2,@Quota_Under05" +
                                                                   "@Quota_Over05,@Quota_Under15,@Quota_Over15,@Quota_Under25," +
                                                                   "@Quota_Over25,@Quota_Under35,@Quota_Over35,@Quota_1X,@Quota_X2," +
                                                                   "@Quota_12,@Quota_Goal,@Quota_NoGoal)", connection);
                        cmd.Parameters.AddWithValue("@id_Lega", campionato);
                        cmd.Parameters.AddWithValue("@NomeCasa", partite[i].NomeCasa);
                        cmd.Parameters.AddWithValue("@LinkCasa", partite[i].LinkCasa);
                        cmd.Parameters.AddWithValue("@NomeFuori", partite[i].NomeFuori);
                        cmd.Parameters.AddWithValue("@LinkFuori", partite[i].LinkFuori);
                        cmd.Parameters.AddWithValue("@Orario", partite[i].Orario);
                        cmd.Parameters.AddWithValue("@Risultato", partite[i].Risultato);
                        cmd.Parameters.AddWithValue("@Data", partite[i].Data);
                        cmd.Parameters.AddWithValue("@Quota_1", partite[i].Quota_1);
                        cmd.Parameters.AddWithValue("@Quota_X", partite[i].Quota_X);
                        cmd.Parameters.AddWithValue("@Quota_2", partite[i].Quota_2);
                        cmd.Parameters.AddWithValue("@Quota_Under05", partite[i].Quota_Under05);
                        cmd.Parameters.AddWithValue("@Quota_Over05", partite[i].Quota_Over05);
                        cmd.Parameters.AddWithValue("@Quota_Under15", partite[i].Quota_Under15);
                        cmd.Parameters.AddWithValue("@Quota_Over15", partite[i].Quota_Over15);
                        cmd.Parameters.AddWithValue("@Quota_Under25", partite[i].Quota_Under25);
                        cmd.Parameters.AddWithValue("@Quota_Over25", partite[i].Quota_Over25);
                        cmd.Parameters.AddWithValue("@Quota_Under35", partite[i].Quota_Under35);
                        cmd.Parameters.AddWithValue("@Quota_Over35", partite[i].Quota_Over35);
                        cmd.Parameters.AddWithValue("@Quota_1X", partite[i].Quota_1X);
                        cmd.Parameters.AddWithValue("@Quota_X2", partite[i].Quota_X2);
                        cmd.Parameters.AddWithValue("@Quota_12", partite[i].Quota_12);
                        cmd.Parameters.AddWithValue("@Quota_Goal", partite[i].Quota_Goal);
                        cmd.Parameters.AddWithValue("@Quota_NoGoal", partite[i].Quota_NoGoal);
                        cmd.ExecuteNonQuery();
                    }
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
            }

            connection.Close();
        }

        private void Classifiche_Click(object sender, EventArgs e)
        {
            getClassifiche fd = new getClassifiche();
            fd.classifiche();
        }

        private void DettagliPartite_Click(object sender, EventArgs e)
        {
            getDettaglio fd = new getDettaglio();
            fd.dettaglio();
        }

        private void Leghe_Click(object sender, EventArgs e)
        {
            getLeghe fd = new getLeghe();
            List<Lega> leghe = new List<Lega>();
            leghe = fd.leghe();

            SqlConnection connection;
            string connectionString;
            connectionString = @"Data Source=pinexp.ns0.it\MIOSERVER,65004;" + "Initial Catalog=Soccer;" + @"User id=sa;" + "Password=Pinexp93;";
            connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    SqlCommand cmd = new SqlCommand("TRUNCATE TABLE Lega", connection);
                    cmd.ExecuteNonQuery();

                    for (int i = 0; i < leghe.Count; i++)
                    {
                        cmd = new SqlCommand("INSERT INTO Lega (idPaese, Nome) values(@idPaese,@Nome)", connection);
                        cmd.Parameters.AddWithValue("@idPaese", leghe[i].idPaese);
                        cmd.Parameters.AddWithValue("@Nome", leghe[i].Nome);
                        cmd.ExecuteNonQuery();
                    }
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
            }

            connection.Close();
        }
    }
}
