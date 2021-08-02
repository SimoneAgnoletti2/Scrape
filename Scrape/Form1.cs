
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
            SqlConnection connection;
            string connectionString;
            connectionString = @"Data Source=pinexp.ns0.it\MIOSERVER,65004;" + "Initial Catalog=Soccer;" + @"User id=sa;" + "Password=Pinexp93;";
            connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    SqlCommand cmd = new SqlCommand("TRUNCATE TABLE Partita", connection);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex) { }
            connection.Close();

            getLive fd = new getLive();
            List<Partita> partite = new List<Partita>();
            fd.live();
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
