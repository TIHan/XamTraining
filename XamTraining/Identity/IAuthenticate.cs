using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

namespace XamTraining
{
    public interface IAuthenticate
    {
        Task<MobileServiceUser> Authenticate();
    }
}
