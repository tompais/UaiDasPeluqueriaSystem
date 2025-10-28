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
            lblTitulo.Location = new Point(10, 7);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(145, 25);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Nuevo Usuario";
            // 
            // lblNombre
            // 
            lblNombre.AutoSize = true;
            lblNombre.Location = new Point(10, 45);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(59, 15);
            lblNombre.TabIndex = 1;
            lblNombre.Text = "Nombre:*";
            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(105, 43);
            txtNombre.Margin = new Padding(3, 2, 3, 2);
            txtNombre.MaxLength = 50;
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(263, 23);
            txtNombre.TabIndex = 2;
            // 
            // lblApellido
            // 
            lblApellido.AutoSize = true;
            lblApellido.Location = new Point(10, 75);
            lblApellido.Name = "lblApellido";
            lblApellido.Size = new Size(59, 15);
            lblApellido.TabIndex = 3;
            lblApellido.Text = "Apellido:*";
            // 
            // txtApellido
            // 
            txtApellido.Location = new Point(105, 73);
            txtApellido.Margin = new Padding(3, 2, 3, 2);
            txtApellido.MaxLength = 80;
            txtApellido.Name = "txtApellido";
            txtApellido.Size = new Size(263, 23);
            txtApellido.TabIndex = 4;
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.Location = new Point(10, 105);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(44, 15);
            lblEmail.TabIndex = 5;
            lblEmail.Text = "Email:*";
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(105, 103);
            txtEmail.Margin = new Padding(3, 2, 3, 2);
            txtEmail.MaxLength = 180;
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(263, 23);
            txtEmail.TabIndex = 6;
            // 
            // lblClave
            // 
            lblClave.AutoSize = true;
            lblClave.Location = new Point(10, 135);
            lblClave.Name = "lblClave";
            lblClave.Size = new Size(44, 15);
            lblClave.TabIndex = 7;
            lblClave.Text = "Clave:*";
            // 
            // txtClave
            // 
            txtClave.Location = new Point(105, 133);
            txtClave.Margin = new Padding(3, 2, 3, 2);
            txtClave.MaxLength = 11;
            txtClave.Name = "txtClave";
            txtClave.Size = new Size(263, 23);
            txtClave.TabIndex = 8;
            txtClave.UseSystemPasswordChar = true;
            // 
            // lblRol
            // 
            lblRol.AutoSize = true;
            lblRol.Location = new Point(10, 165);
            lblRol.Name = "lblRol";
            lblRol.Size = new Size(32, 15);
            lblRol.TabIndex = 9;
            lblRol.Text = "Rol:*";
            // 
            // cmbRol
            // 
            cmbRol.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRol.FormattingEnabled = true;
            cmbRol.Location = new Point(105, 163);
            cmbRol.Margin = new Padding(3, 2, 3, 2);
            cmbRol.Name = "cmbRol";
            cmbRol.Size = new Size(263, 23);
            cmbRol.TabIndex = 10;
            // 
            // btnGuardar
            // 
            btnGuardar.Location = new Point(192, 202);
            btnGuardar.Margin = new Padding(3, 2, 3, 2);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(88, 26);
            btnGuardar.TabIndex = 11;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += BtnGuardar_Click;
            // 
            // btnCancelar
            // 
            btnCancelar.Location = new Point(289, 202);
            btnCancelar.Margin = new Padding(3, 2, 3, 2);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(79, 26);
            btnCancelar.TabIndex = 12;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            btnCancelar.Click += BtnCancelar_Click;
            // 
            // FormAltaUsuario
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(380, 241);
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
            Margin = new Padding(3, 2, 3, 2);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormAltaUsuario";
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
