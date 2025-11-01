using APP;
using DOM;

namespace PeluqueriaSystem;

/// <summary>
/// Formulario para dar de alta un nuevo usuario
/// </summary>
public partial class FormAltaUsuario : Form
{
    private readonly appUsuario _appUsuario;

    public FormAltaUsuario(appUsuario appUsuario)
    {
        InitializeComponent();
        _appUsuario = appUsuario;

        // Inicializar ComboBox de roles
        cmbRol.DataSource = Enum.GetValues(typeof(domUsuario.RolUsuario));
        cmbRol.SelectedIndex = 0; // Cliente por defecto

        // Suscribir eventos para validación en tiempo real
        txtNombre.TextChanged += ValidarFormulario;
        txtApellido.TextChanged += ValidarFormulario;
        txtEmail.TextChanged += ValidarFormulario;
        txtClave.TextChanged += ValidarFormulario;

        // Validación inicial
        ValidarFormulario(null, EventArgs.Empty);
    }

    private void ValidarFormulario(object sender, EventArgs e)
    {
        var errores = ObtenerErroresValidacion();
        btnGuardar.Enabled = errores.Count == 0;
    }

    private List<string> ObtenerErroresValidacion()
    {
        var errores = new List<string>();

        if (string.IsNullOrWhiteSpace(txtNombre.Text))
            errores.Add("El nombre es obligatorio");
        else if (txtNombre.Text.Length > 50)
            errores.Add("El nombre no puede superar los 50 caracteres");

        if (string.IsNullOrWhiteSpace(txtApellido.Text))
            errores.Add("El apellido es obligatorio");
        else if (txtApellido.Text.Length > 80)
            errores.Add("El apellido no puede superar los 80 caracteres");

        if (string.IsNullOrWhiteSpace(txtEmail.Text))
            errores.Add("El email es obligatorio");
        else if (txtEmail.Text.Length > 180)
            errores.Add("El email no puede superar los 180 caracteres");

        if (string.IsNullOrWhiteSpace(txtClave.Text))
            errores.Add("La clave es obligatoria");
        else if (txtClave.Text.Length != 11)
            errores.Add("La clave debe tener exactamente 11 caracteres");

        return errores;
    }

    private void BtnGuardar_Click(object sender, EventArgs e)
    {
        var exitoso = false;

        try
        {
            // Deshabilitar botones mientras se procesa
            btnGuardar.Enabled = false;
            btnCancelar.Enabled = false;

            // Validar nuevamente por seguridad
            var errores = ObtenerErroresValidacion();

            if (errores.Count == 0)
            {
                // Verificar que el email no exista
                if (!_appUsuario.ExisteEmail(txtEmail.Text))
                {
                    // Crear usuario
                    var rol = (domUsuario.RolUsuario)(cmbRol.SelectedItem ?? domUsuario.RolUsuario.Cliente);
                    _appUsuario.Crear(txtNombre.Text, txtApellido.Text, txtEmail.Text, txtClave.Text, rol);

                    MessageBox.Show("Usuario creado exitosamente", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    exitoso = true;
                }
                else
                {
                    MessageBox.Show("El email ya está registrado", "Error de Validación",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Error de validación:\n\n" + string.Join("\n", errores),
                    "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

            if (exitoso)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }

    private void BtnCancelar_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
