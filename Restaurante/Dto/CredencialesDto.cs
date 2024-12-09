namespace Restaurante.Dto
{
    public class CredencialesDto
    {
        private string user;
        private string password;
        public string User
        {
            get => this.user;

            set
            {
                this.ValidaCadenaDeCaracteres(value);
                this.user = value;
            }
        }
        public string Password
        {
            get => this.password;

            set
            {
                this.ValidaCadenaDeCaracteres(value);
                this.password = value;
            }
        }
        private void ValidaCadenaDeCaracteres(string value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                throw new Exception("El valor no puede ser nulo o con espacios en blanco");
            }
        }
    }
}
