using ABS.Application;
using APP;

namespace PeluqueriaSystem;

/// <summary>
/// Formulario de listado de usuarios
/// </summary>
public partial class FormUsuarios : Form
{
    private readonly IUsuarioService _usuarioService;
    private readonly appUsuario _appUsuario;

    public FormUsuarios(IUsuarioService usuarioService, appUsuario appUsuario)
    {
        InitializeComponent();
        _usuarioService = usuarioService;
        _appUsuario = appUsuario;
    }

    private void FrmUsuarios_Load(object sender, EventArgs e)
    {
        try
        {
            dgvUsuarios.DataSource = _appUsuario.Traer();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al cargar usuarios: {ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnNuevo_Click(object sender, EventArgs e)
    {
        var formAlta = DependencyInjectionContainer.ObtenerServicio<FormAltaUsuario>();

        if (formAlta.ShowDialog() == DialogResult.OK)
        {
            // Recargar la lista despu�s de crear un usuario
            FrmUsuarios_Load(null, EventArgs.Empty);
        }
    }

    private void BtnRefrescar_Click(object sender, EventArgs e)
    {
        FrmUsuarios_Load(null, EventArgs.Empty);
    }
}
