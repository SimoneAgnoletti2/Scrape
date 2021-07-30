
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
            this.Leghe = new System.Windows.Forms.Button();
            this.CLassifiche = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Live
            // 
            this.Live.Location = new System.Drawing.Point(231, 11);
            this.Live.Margin = new System.Windows.Forms.Padding(2);
            this.Live.Name = "Live";
            this.Live.Size = new System.Drawing.Size(106, 38);
            this.Live.TabIndex = 1;
            this.Live.Text = "Live";
            this.Live.UseVisualStyleBackColor = true;
            this.Live.Click += new System.EventHandler(this.Live_Click);
            // 
            // DettagliPartite
            // 
            this.DettagliPartite.Location = new System.Drawing.Point(341, 11);
            this.DettagliPartite.Margin = new System.Windows.Forms.Padding(2);
            this.DettagliPartite.Name = "DettagliPartite";
            this.DettagliPartite.Size = new System.Drawing.Size(106, 38);
            this.DettagliPartite.TabIndex = 5;
            this.DettagliPartite.Text = "Dettagli";
            this.DettagliPartite.UseVisualStyleBackColor = true;
            this.DettagliPartite.Click += new System.EventHandler(this.DettagliPartite_Click);
            // 
            // Leghe
            // 
            this.Leghe.Location = new System.Drawing.Point(11, 11);
            this.Leghe.Margin = new System.Windows.Forms.Padding(2);
            this.Leghe.Name = "Leghe";
            this.Leghe.Size = new System.Drawing.Size(106, 38);
            this.Leghe.TabIndex = 9;
            this.Leghe.Text = "Leghe";
            this.Leghe.UseVisualStyleBackColor = true;
            this.Leghe.Click += new System.EventHandler(this.Leghe_Click);
            // 
            // CLassifiche
            // 
            this.CLassifiche.Location = new System.Drawing.Point(121, 11);
            this.CLassifiche.Margin = new System.Windows.Forms.Padding(2);
            this.CLassifiche.Name = "CLassifiche";
            this.CLassifiche.Size = new System.Drawing.Size(106, 38);
            this.CLassifiche.TabIndex = 10;
            this.CLassifiche.Text = "Classifiche";
            this.CLassifiche.UseVisualStyleBackColor = true;
            this.CLassifiche.Click += new System.EventHandler(this.Classifiche_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(452, 53);
            this.Controls.Add(this.CLassifiche);
            this.Controls.Add(this.Leghe);
            this.Controls.Add(this.DettagliPartite);
            this.Controls.Add(this.Live);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Live;
        private System.Windows.Forms.Button DettagliPartite;
        private System.Windows.Forms.Button Leghe;
        private System.Windows.Forms.Button CLassifiche;
    }
}

