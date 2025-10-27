using ABS.Application;
using DOM;

namespace PeluqueriaSystem;

/// <summary>
/// Formulario para dar de alta un nuevo usuario
/// </summary>
public partial class FormAltaUsuario : Form
{
    private readonly IUsuarioService _usuarioService;

    public FormAltaUsuario(IUsuarioService usuarioService)
    {
        InitializeComponent();
        _usuarioService = usuarioService;

        // Inicializar ComboBox de roles
        cmbRol.DataSource = Enum.GetValues(typeof(Usuario.RolUsuario));
        cmbRol.SelectedIndex = 0; // Cliente por defecto
    }

    private void BtnGuardar_Click(object sender, EventArgs e)
    {
        try
        {
            // Deshabilitar botones mientras se procesa
            btnGuardar.Enabled = false;
            btnCancelar.Enabled = false;

            // Crear Request
            var request = new CrearUsuarioRequest(
                Nombre: txtNombre.Text,
                Apellido: txtApellido.Text,
                Email: txtEmail.Text,
                Clave: txtClave.Text,
                Rol: (Usuario.RolUsuario)(cmbRol.SelectedItem ?? Usuario.RolUsuario.Cliente)
            );

            // Llamar al servicio
            var resultado = _usuarioService.CrearUsuario(request);

            if (resultado.Exitoso)
            {
                MessageBox.Show(resultado.Mensaje, "Éxito",
               MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                // Mostrar errores de validación
                var mensajeError = resultado.Mensaje;
                if (resultado.Errores.Count != 0)
                {
                    mensajeError += "\n\nDetalles:\n" + string.Join("\n", resultado.Errores);
                }

                MessageBox.Show(mensajeError, "Error de Validación",
                   MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error inesperado: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            // Rehabilitar botones
            btnGuardar.Enabled = true;
            btnCancelar.Enabled = true;
        }
    }

    private void BtnCancelar_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
