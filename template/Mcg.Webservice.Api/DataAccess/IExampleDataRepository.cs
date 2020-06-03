using System.Collections.Generic;
using Mcg.Webservice.Api.Models;

namespace Mcg.Webservice.Api.DataAccess
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
