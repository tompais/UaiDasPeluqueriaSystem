namespace APP
{
    public class appUsuario
    {
        public List<DOM.domUsuario> Traer()
        {
            return REPO.repoUsuario.Traer();
        }
    }
}
