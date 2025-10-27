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
        // Buscar si ya existe el formulario de usuarios abierto
        var formUsuariosExistente = MdiChildren.OfType<FormUsuarios>().FirstOrDefault();

        if (formUsuariosExistente != null)
        {
            formUsuariosExistente.Activate();
        }
        else
        {
            // Crear nuevo formulario de usuarios desde DI
            var formUsuarios = DependencyInjectionContainer.ObtenerServicio<FormUsuarios>();
            formUsuarios.MdiParent = this;
            formUsuarios.Show();
        }
    }
}
