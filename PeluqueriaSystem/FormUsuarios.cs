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
        CargarUsuarios();
    }

    private void CargarUsuarios()
    {
        try
        {
            var usuarios = _usuarioService.ObtenerTodos();

            // Cargar datos en el DataGridView
            dgvUsuarios.DataSource = usuarios.Select(u => new
            {
                u.Id,
                u.Nombre,
                u.Apellido,
                u.Email,
                Estado = u.Estado.ToString(),
                Rol = u.Rol.ToString(),
                u.FechaCreacion
            }).ToList();
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
            // Recargar la lista después de crear un usuario
            CargarUsuarios();
        }
    }

    private void BtnRefrescar_Click(object sender, EventArgs e)
    {
        CargarUsuarios();
    }
}
