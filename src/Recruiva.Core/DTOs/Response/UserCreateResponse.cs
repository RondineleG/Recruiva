namespace Recruiva.Core.DTOs.Response
{
    public class UserCreateResponse
    {
        public UserCreateResponse()
        {
            Errors = [];
        }

        public UserCreateResponse(bool sucesso = true) : this()
        {
            Success = sucesso;
        }

        public List<string> Errors { get; private set; }

        public bool Success { get; private set; }

        public void AddErros(IEnumerable<string> errors)
        {
            Errors.AddRange(errors);
        }
    }
}