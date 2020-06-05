using System.Collections.Generic;
using template.Api.Models;

namespace template.Api.DataAccess
{
	public interface IExampleDataRepository
    {
        void Delete(UserModel model);
        void Insert(UserModel model);
        IEnumerable<UserModel> SelectAll();
        UserModel SelectOneByEmail(string email);
        UserModel SelectOneById(int id);
        void Update(UserModel model);
        bool ContainsId(int id);
        bool ContainsEmail(string email);
        int NextId();
    }
}
