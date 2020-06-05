using System.Collections.Generic;
using template.Api.Models;

namespace template.Api.Services
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
