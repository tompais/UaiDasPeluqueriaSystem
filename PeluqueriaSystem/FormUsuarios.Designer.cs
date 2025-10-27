namespace PeluqueriaSystem
{
    partial class FormUsuarios
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
      dgvUsuarios = new DataGridView();
      colId = new DataGridViewTextBoxColumn();
    colNombre = new DataGridViewTextBoxColumn();
      colApellido = new DataGridViewTextBoxColumn();
      colEmail = new DataGridViewTextBoxColumn();
      colEstado = new DataGridViewTextBoxColumn();
      colRol = new DataGridViewTextBoxColumn();
      colFechaCreacion = new DataGridViewTextBoxColumn();
      btnNuevo = new Button();
            btnRefrescar = new Button();
   lblTitulo = new Label();
  ((System.ComponentModel.ISupportInitialize)dgvUsuarios).BeginInit();
 SuspendLayout();
 // 
       // dgvUsuarios
       // 
     dgvUsuarios.AllowUserToAddRows = false;
        dgvUsuarios.AllowUserToDeleteRows = false;
     dgvUsuarios.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
   dgvUsuarios.AutoGenerateColumns = false;
    dgvUsuarios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      dgvUsuarios.Columns.AddRange(colId, colNombre, colApellido, colEmail, colEstado, colRol, colFechaCreacion);
       dgvUsuarios.Location = new Point(12, 80);
   dgvUsuarios.Name = "dgvUsuarios";
    dgvUsuarios.ReadOnly = true;
      dgvUsuarios.RowHeadersWidth = 51;
    dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
dgvUsuarios.Size = new Size(960, 450);
        dgvUsuarios.TabIndex = 0;
      // 
      // colId
      //
      colId.DataPropertyName = "Id";
colId.HeaderText = "ID";
      colId.Name = "colId";
      colId.ReadOnly = true;
      colId.Width = 50;
      // 
      // colNombre
      //
      colNombre.DataPropertyName = "Nombre";
      colNombre.HeaderText = "Nombre";
      colNombre.Name = "colNombre";
      colNombre.ReadOnly = true;
      colNombre.Width = 150;
      // 
      // colApellido
      //
      colApellido.DataPropertyName = "Apellido";
      colApellido.HeaderText = "Apellido";
      colApellido.Name = "colApellido";
      colApellido.ReadOnly = true;
      colApellido.Width = 150;
    // 
      // colEmail
      //
      colEmail.DataPropertyName = "Email";
      colEmail.HeaderText = "Email";
      colEmail.Name = "colEmail";
      colEmail.ReadOnly = true;
      colEmail.Width = 200;
      // 
      // colEstado
      //
      colEstado.DataPropertyName = "Estado";
      colEstado.HeaderText = "Estado";
      colEstado.Name = "colEstado";
      colEstado.ReadOnly = true;
      colEstado.Width = 80;
      // 
      // colRol
    //
  colRol.DataPropertyName = "Rol";
      colRol.HeaderText = "Rol";
  colRol.Name = "colRol";
 colRol.ReadOnly = true;
      colRol.Width = 100;
      // 
      // colFechaCreacion
      //
      colFechaCreacion.DataPropertyName = "FechaCreacion";
      colFechaCreacion.HeaderText = "Fecha Creación";
      colFechaCreacion.Name = "colFechaCreacion";
      colFechaCreacion.ReadOnly = true;
      colFechaCreacion.Width = 150;
      colFechaCreacion.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
      // 
      // btnNuevo
     // 
    btnNuevo.Location = new Point(12, 40);
    btnNuevo.Name = "btnNuevo";
      btnNuevo.Size = new Size(120, 30);
     btnNuevo.TabIndex = 1;
    btnNuevo.Text = "Nuevo Usuario";
       btnNuevo.UseVisualStyleBackColor = true;
   btnNuevo.Click += btnNuevo_Click;
  // 
   // btnRefrescar
      // 
      btnRefrescar.Location = new Point(140, 40);
     btnRefrescar.Name = "btnRefrescar";
     btnRefrescar.Size = new Size(100, 30);
      btnRefrescar.TabIndex = 2;
       btnRefrescar.Text = "Refrescar";
   btnRefrescar.UseVisualStyleBackColor = true;
  btnRefrescar.Click += BtnRefrescar_Click;
    // 
    // lblTitulo
   // 
   lblTitulo.AutoSize = true;
     lblTitulo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
   lblTitulo.Location = new Point(12, 9);
    lblTitulo.Name = "lblTitulo";
       lblTitulo.Size = new Size(197, 28);
         lblTitulo.TabIndex = 3;
   lblTitulo.Text = "Gestión de Usuarios";
    // 
      // FormUsuarios
  // 
   AutoScaleDimensions = new SizeF(8F, 20F);
      AutoScaleMode = AutoScaleMode.Font;
   ClientSize = new Size(984, 541);
     Controls.Add(lblTitulo);
      Controls.Add(btnRefrescar);
        Controls.Add(btnNuevo);
    Controls.Add(dgvUsuarios);
          Name = "FormUsuarios";
 StartPosition = FormStartPosition.CenterParent;
  Text = "Usuarios";
  Load += FrmUsuarios_Load;
    ((System.ComponentModel.ISupportInitialize)dgvUsuarios).EndInit();
      ResumeLayout(false);
     PerformLayout();
}

        #endregion

   private DataGridView dgvUsuarios;
   private DataGridViewTextBoxColumn colId;
   private DataGridViewTextBoxColumn colNombre;
private DataGridViewTextBoxColumn colApellido;
   private DataGridViewTextBoxColumn colEmail;
   private DataGridViewTextBoxColumn colEstado;
   private DataGridViewTextBoxColumn colRol;
   private DataGridViewTextBoxColumn colFechaCreacion;
    private Button btnNuevo;
        private Button btnRefrescar;
private Label lblTitulo;
    }
}
