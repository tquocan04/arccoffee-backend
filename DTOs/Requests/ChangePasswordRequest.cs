namespace DTOs.Requests
{
    public record ChangePasswordRequest
    {
        public string CurrentPassword { get; init; } = null!;
        public string NewPassword { get; init; } = null!;
    }
}
