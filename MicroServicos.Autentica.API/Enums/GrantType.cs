using System.ComponentModel;

namespace MicroServicos.Autentica.API.Enums
{
    public enum GrantType
    {
        [Description("refresh_token")]
        RefreshToken = 2,

        [Description("password")]
        Password = 1
    }
}
