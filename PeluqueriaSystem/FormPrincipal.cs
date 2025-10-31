namespace PeluqueriaSystem;

/// <summary>
/// Formulario MDI principal del sistema
/// </summary>
public partial class FormPrincipal : Form
{
    public FormPrincipal()
    {
        InitializeComponent();
    }

    private void UsuariosToolStripMenuItem_Click(object sender, EventArgs e)
    {
        FormUsuarios f = DependencyInjectionContainer.ObtenerServicio<FormUsuarios>();
        f.MdiParent = this;
        f.Show();
        f.Left = (this.Left + this.Width - f.Width) / 2;
        f.Top = (this.Top + this.Height - f.Height) / 2;
    }
}
