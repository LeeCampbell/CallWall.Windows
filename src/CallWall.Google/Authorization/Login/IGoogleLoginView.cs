using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallWall.Google.Authorization.Login
{
    public interface IGoogleLoginView : Microsoft.Practices.Prism.IActiveAware
    {
        GoogleLoginViewModel ViewModel { get; }
    }
}
