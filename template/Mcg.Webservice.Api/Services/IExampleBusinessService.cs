using System.Collections.Generic;
using Mcg.Webservice.Api.Models;

namespace Mcg.Webservice.Api.Services
{
    public interface IExampleBusinessService
    {
        (bool ok, string error) Delete(UserModel model);
        (bool ok, string error, UserModel newModel) Insert(UserModel model);
        IEnumerable<UserModel> SelectAll();
        UserModel SelectByEmail(string email);
        UserModel SeledtById(int id);
        (bool ok, string error) Update(UserModel model);
    }
}
