using ABS.Application;

namespace PeluqueriaSystem;

/// <summary>
/// Formulario de listado de usuarios
/// </summary>
public partial class FormUsuarios : Form
{
    private readonly IUsuarioService _usuarioService;

    public FormUsuarios(IUsuarioService usuarioService)
    {
        InitializeComponent();
        _usuarioService = usuarioService;
    }

    private void FrmUsuarios_Load(object sender, EventArgs e)
    {
        APP.appUsuario usuario = new APP.appUsuario();
        dgvUsuarios.DataSource = usuario.Traer();
        usuario = null;
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
