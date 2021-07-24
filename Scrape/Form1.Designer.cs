
namespace Scrape
{
    partial class Form1
    {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Pulire le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            this.Live = new System.Windows.Forms.Button();
            this.DettagliPartite = new System.Windows.Forms.Button();
            this.ClassificheProva = new System.Windows.Forms.Button();
            this.Paesi = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Live
            // 
            this.Live.Location = new System.Drawing.Point(444, 15);
            this.Live.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Live.Name = "Live";
            this.Live.Size = new System.Drawing.Size(141, 47);
            this.Live.TabIndex = 1;
            this.Live.Text = "Live";
            this.Live.UseVisualStyleBackColor = true;
            this.Live.Click += new System.EventHandler(this.Live_Click);
            // 
            // DettagliPartite
            // 
            this.DettagliPartite.Location = new System.Drawing.Point(297, 15);
            this.DettagliPartite.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.DettagliPartite.Name = "DettagliPartite";
            this.DettagliPartite.Size = new System.Drawing.Size(141, 47);
            this.DettagliPartite.TabIndex = 5;
            this.DettagliPartite.Text = "Dettagli";
            this.DettagliPartite.UseVisualStyleBackColor = true;
            this.DettagliPartite.Click += new System.EventHandler(this.DettagliPartite_Click);
            // 
            // ClassificheProva
            // 
            this.ClassificheProva.Location = new System.Drawing.Point(150, 15);
            this.ClassificheProva.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ClassificheProva.Name = "ClassificheProva";
            this.ClassificheProva.Size = new System.Drawing.Size(141, 47);
            this.ClassificheProva.TabIndex = 6;
            this.ClassificheProva.Text = "Classifiche";
            this.ClassificheProva.UseVisualStyleBackColor = true;
            this.ClassificheProva.Click += new System.EventHandler(this.ClassificheProva_Click);
            // 
            // Paesi
            // 
            this.Paesi.Location = new System.Drawing.Point(3, 14);
            this.Paesi.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Paesi.Name = "Paesi";
            this.Paesi.Size = new System.Drawing.Size(141, 47);
            this.Paesi.TabIndex = 8;
            this.Paesi.Text = "Paesi";
            this.Paesi.UseVisualStyleBackColor = true;
            this.Paesi.Click += new System.EventHandler(this.Paesi_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(598, 72);
            this.Controls.Add(this.Paesi);
            this.Controls.Add(this.ClassificheProva);
            this.Controls.Add(this.DettagliPartite);
            this.Controls.Add(this.Live);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Live;
        private System.Windows.Forms.Button DettagliPartite;
        private System.Windows.Forms.Button ClassificheProva;
        private System.Windows.Forms.Button Paesi;
    }
}

