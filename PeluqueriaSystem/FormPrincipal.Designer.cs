namespace PeluqueriaSystem
{
    partial class FormPrincipal
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
     /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
     if (disposing && (components != null))
    {
 components.Dispose();
            }
  base.Dispose(disposing);
    }

        #region Windows Form Designer generated code

        /// <summary>
     /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
  private void InitializeComponent()
  {
    menuStrip = new MenuStrip();
      administracionToolStripMenuItem = new ToolStripMenuItem();
            usuariosToolStripMenuItem = new ToolStripMenuItem();
        menuStrip.SuspendLayout();
   SuspendLayout();
            // 
      // menuStrip
    // 
      menuStrip.ImageScalingSize = new Size(20, 20);
            menuStrip.Items.AddRange(new ToolStripItem[] { administracionToolStripMenuItem });
 menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
         menuStrip.Size = new Size(1000, 28);
      menuStrip.TabIndex = 1;
            menuStrip.Text = "menuStrip1";
         // 
            // administracionToolStripMenuItem
   // 
      administracionToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { usuariosToolStripMenuItem });
            administracionToolStripMenuItem.Name = "administracionToolStripMenuItem";
        administracionToolStripMenuItem.Size = new Size(128, 24);
      administracionToolStripMenuItem.Text = "Administración";
        // 
   // usuariosToolStripMenuItem
         // 
            usuariosToolStripMenuItem.Name = "usuariosToolStripMenuItem";
   usuariosToolStripMenuItem.Size = new Size(224, 26);
            usuariosToolStripMenuItem.Text = "Usuarios";
     usuariosToolStripMenuItem.Click += UsuariosToolStripMenuItem_Click;
            // 
       // frmPrincipal
 // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
    ClientSize = new Size(1000, 600);
            Controls.Add(menuStrip);
    IsMdiContainer = true;
            MainMenuStrip = menuStrip;
        Name = "frmPrincipal";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Sistema de Gestión - Peluquería";
            WindowState = FormWindowState.Maximized;
            menuStrip.ResumeLayout(false);
     menuStrip.PerformLayout();
        ResumeLayout(false);
      PerformLayout();
        }

    #endregion

  private MenuStrip menuStrip;
private ToolStripMenuItem administracionToolStripMenuItem;
        private ToolStripMenuItem usuariosToolStripMenuItem;
    }
}
