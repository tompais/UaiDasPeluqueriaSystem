namespace PeluqueriaSystem
{
    partial class FormAltaUsuario
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
      lblTitulo = new Label();
   lblNombre = new Label();
  txtNombre = new TextBox();
       lblApellido = new Label();
        txtApellido = new TextBox();
       lblEmail = new Label();
 txtEmail = new TextBox();
    lblClave = new Label();
txtClave = new TextBox();
            lblRol = new Label();
     cmbRol = new ComboBox();
     btnGuardar = new Button();
   btnCancelar = new Button();
SuspendLayout();
      // 
 // lblTitulo
      // 
       lblTitulo.AutoSize = true;
       lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitulo.Location = new Point(12, 9);
     lblTitulo.Name = "lblTitulo";
       lblTitulo.Size = new Size(190, 32);
    lblTitulo.TabIndex = 0;
    lblTitulo.Text = "Nuevo Usuario";
    // 
      // lblNombre
   // 
     lblNombre.AutoSize = true;
    lblNombre.Location = new Point(12, 60);
            lblNombre.Name = "lblNombre";
    lblNombre.Size = new Size(73, 20);
 lblNombre.TabIndex = 1;
    lblNombre.Text = "Nombre:*";
    // 
       // txtNombre
       // 
 txtNombre.Location = new Point(120, 57);
   txtNombre.MaxLength = 50;
       txtNombre.Name = "txtNombre";
     txtNombre.Size = new Size(300, 27);
  txtNombre.TabIndex = 2;
       // 
    // lblApellido
     // 
       lblApellido.AutoSize = true;
    lblApellido.Location = new Point(12, 100);
   lblApellido.Name = "lblApellido";
      lblApellido.Size = new Size(75, 20);
  lblApellido.TabIndex = 3;
       lblApellido.Text = "Apellido:*";
    // 
    // txtApellido
       // 
       txtApellido.Location = new Point(120, 97);
    txtApellido.MaxLength = 80;
    txtApellido.Name = "txtApellido";
      txtApellido.Size = new Size(300, 27);
            txtApellido.TabIndex = 4;
            // 
   // lblEmail
  // 
    lblEmail.AutoSize = true;
            lblEmail.Location = new Point(12, 140);
       lblEmail.Name = "lblEmail";
      lblEmail.Size = new Size(55, 20);
     lblEmail.TabIndex = 5;
       lblEmail.Text = "Email:*";
  // 
     // txtEmail
   // 
       txtEmail.Location = new Point(120, 137);
      txtEmail.MaxLength = 180;
       txtEmail.Name = "txtEmail";
     txtEmail.Size = new Size(300, 27);
    txtEmail.TabIndex = 6;
       // 
    // lblClave
     // 
            lblClave.AutoSize = true;
     lblClave.Location = new Point(12, 180);
      lblClave.Name = "lblClave";
    lblClave.Size = new Size(102, 20);
  lblClave.TabIndex = 7;
    lblClave.Text = "Clave (11 c.):*";
            // 
      // txtClave
    // 
     txtClave.Location = new Point(120, 177);
   txtClave.MaxLength = 11;
     txtClave.Name = "txtClave";
   txtClave.Size = new Size(300, 27);
   txtClave.TabIndex = 8;
       txtClave.UseSystemPasswordChar = true;
       // 
            // lblRol
     // 
    lblRol.AutoSize = true;
       lblRol.Location = new Point(12, 220);
  lblRol.Name = "lblRol";
lblRol.Size = new Size(38, 20);
       lblRol.TabIndex = 9;
   lblRol.Text = "Rol:*";
          // 
   // cmbRol
      // 
            cmbRol.DropDownStyle = ComboBoxStyle.DropDownList;
       cmbRol.FormattingEnabled = true;
            cmbRol.Location = new Point(120, 217);
 cmbRol.Name = "cmbRol";
            cmbRol.Size = new Size(300, 28);
            cmbRol.TabIndex = 10;
       // 
       // btnGuardar
 // 
       btnGuardar.Location = new Point(220, 270);
      btnGuardar.Name = "btnGuardar";
       btnGuardar.Size = new Size(100, 35);
     btnGuardar.TabIndex = 11;
   btnGuardar.Text = "Guardar";
       btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += BtnGuardar_Click;
    // 
// btnCancelar
  // 
  btnCancelar.Location = new Point(330, 270);
   btnCancelar.Name = "btnCancelar";
         btnCancelar.Size = new Size(90, 35);
       btnCancelar.TabIndex = 12;
            btnCancelar.Text = "Cancelar";
   btnCancelar.UseVisualStyleBackColor = true;
            btnCancelar.Click += BtnCancelar_Click;
   // 
        // frmAltaUsuario
  // 
  AutoScaleDimensions = new SizeF(8F, 20F);
  AutoScaleMode = AutoScaleMode.Font;
     ClientSize = new Size(434, 321);
       Controls.Add(btnCancelar);
       Controls.Add(btnGuardar);
      Controls.Add(cmbRol);
       Controls.Add(lblRol);
   Controls.Add(txtClave);
       Controls.Add(lblClave);
            Controls.Add(txtEmail);
    Controls.Add(lblEmail);
            Controls.Add(txtApellido);
      Controls.Add(lblApellido);
            Controls.Add(txtNombre);
    Controls.Add(lblNombre);
   Controls.Add(lblTitulo);
       FormBorderStyle = FormBorderStyle.FixedDialog;
   MaximizeBox = false;
       MinimizeBox = false;
       Name = "frmAltaUsuario";
       StartPosition = FormStartPosition.CenterParent;
  Text = "Alta de Usuario";
     ResumeLayout(false);
            PerformLayout();
        }

#endregion

        private Label lblTitulo;
        private Label lblNombre;
   private TextBox txtNombre;
        private Label lblApellido;
      private TextBox txtApellido;
   private Label lblEmail;
     private TextBox txtEmail;
        private Label lblClave;
   private TextBox txtClave;
private Label lblRol;
   private ComboBox cmbRol;
    private Button btnGuardar;
        private Button btnCancelar;
    }
}
