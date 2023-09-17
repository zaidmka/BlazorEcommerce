namespace BlazorEcommerce.Client.Service.AuthService
{
    public interface IAuthService
    {
        Task<ServiceResponse<int>> Register(UserRegister request);
    }
}
