namespace ModelLayer.Model
{
    public class ResetPasswordModel
    {
        public required string Token { get; set; }

        public required string NewPassword { get; set; }
    }
}