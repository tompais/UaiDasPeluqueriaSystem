using APP;

namespace PeluqueriaSystem;

/// <summary>
/// Formulario de listado de usuarios
/// </summary>
public partial class FormUsuarios : Form
{
    private readonly AppUsuario _appUsuario;

    public FormUsuarios(AppUsuario appUsuario)
    {
        InitializeComponent();
        _appUsuario = appUsuario;
    }

    private void FrmUsuarios_Load(object? sender, EventArgs e)
    {
        try
        {
            dgvUsuarios.DataSource = _appUsuario.Traer();
            ActualizarEstadoBotones();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al cargar usuarios: {ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnNuevo_Click(object sender, EventArgs e)
    {
        var formAlta = DependencyInjectionContainer.ObtenerServicio<FormAltaUsuario>();
        formAlta.ID = 0; // Modo alta

        if (formAlta.ShowDialog() == DialogResult.OK)
        {
            // Recargar la lista después de crear un usuario
            FrmUsuarios_Load(null, EventArgs.Empty);
        }
    }

    private void BtnModificar_Click(object sender, EventArgs e)
    {
        if (dgvUsuarios.CurrentRow == null)
        {
            MessageBox.Show("Debe seleccionar un usuario para modificar", "Validación",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            int id = Convert.ToInt32(dgvUsuarios.CurrentRow.Cells["colId"].Value);
            var formAlta = DependencyInjectionContainer.ObtenerServicio<FormAltaUsuario>();
            formAlta.ID = id; // Modo modificación

            if (formAlta.ShowDialog() == DialogResult.OK)
            {
                // Recargar la lista después de modificar
                FrmUsuarios_Load(null, EventArgs.Empty);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al abrir formulario de modificación: {ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnEliminar_Click(object sender, EventArgs e)
    {
        if (dgvUsuarios.CurrentRow == null)
        {
            MessageBox.Show("Debe seleccionar un usuario para eliminar", "Validación",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            int id = Convert.ToInt32(dgvUsuarios.CurrentRow.Cells["colId"].Value);
            string nombre = dgvUsuarios.CurrentRow.Cells["colNombre"].Value?.ToString() ?? "";
            string apellido = dgvUsuarios.CurrentRow.Cells["colApellido"].Value?.ToString() ?? "";

            var resultado = MessageBox.Show(
    $"¿Está seguro que desea eliminar al usuario '{nombre} {apellido}'?\n\nEsta acción no se puede deshacer.",
     "Confirmar eliminación",
    MessageBoxButtons.YesNo,
 MessageBoxIcon.Warning);

            if (resultado == DialogResult.Yes)
    {
      _appUsuario.Eliminar(id);
       MessageBox.Show("Usuario eliminado exitosamente", "Éxito",
        MessageBoxButtons.OK, MessageBoxIcon.Information);
  FrmUsuarios_Load(null, EventArgs.Empty);
        }
 }
      catch (Exception ex)
        {
  MessageBox.Show($"Error al eliminar usuario: {ex.Message}", "Error",
         MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
 }

    private void BtnRefrescar_Click(object sender, EventArgs e)
    {
        FrmUsuarios_Load(null, EventArgs.Empty);
    }

    private void ActualizarEstadoBotones()
  {
        bool haySeleccion = dgvUsuarios.CurrentRow != null;
        btnModificar.Enabled = haySeleccion;
        btnEliminar.Enabled = haySeleccion;
    }
}
