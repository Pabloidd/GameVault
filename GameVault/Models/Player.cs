namespace GameVault.Models
{
    public class Player
    {
        public string Nickname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime RegistrationDate { get; set; }
        public int Level { get; set; }
    }
}