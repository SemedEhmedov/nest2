using System.Reflection.Metadata.Ecma335;

namespace Nest1.ViewModels.Account
{
    public record ConfirmEmailVm
    {
        public string ConfirmKey { get; set; }
    }
}
