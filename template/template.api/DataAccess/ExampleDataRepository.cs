using System;
using System.Collections.Generic;
using System.Linq;
using template.Api.Infrastructure.Logging;
using template.Api.Infrastructure.Tracing;
using template.Api.Models;

namespace template.Api.DataAccess
{
	/// <summary>
	/// This is just a class to provide a simple in-memory data layer so the
	/// template can demonstrate accessing data.  Please delete this code file
	/// before testing.
	/// </summary>
	[Trace, Log]
    public class UserDataRepository : IExampleDataRepository
    {
        private readonly Dictionary<int, UserModel> Table = new Dictionary<int, UserModel>();

        public IEnumerable<UserModel> SelectAll()
        {
            return Table.Values.ToArray();
        }

        public UserModel SelectOneById(int id)
        {
            return Table[id];
        }

        public UserModel SelectOneByEmail(string email)
        {
            return Table.Values.FirstOrDefault(m => m.EmailAddress.Equals(email, StringComparison.InvariantCultureIgnoreCase));
        }

        public void Insert(UserModel model)
        {
            Table.Add(model.ID, model);
        }

        public void Update(UserModel model)
        {
            Table[model.ID] = model;
        }

        public void Delete(UserModel model)
        {
            if (!Table.ContainsKey(model.ID))
            {
                return;
            }

            Table.Remove(model.ID, out _);
        }

        public bool ContainsId(int id)
        {
            return Table.ContainsKey(id);
        }

        public bool ContainsEmail(string email)
        {
            return Table.Values.Any(m => m.EmailAddress.Equals(email, StringComparison.InvariantCultureIgnoreCase));
        }

        public int NextId()
        {
            if (Table.Count == 0)
            {
                return 1;
            }

            return Table.Count() + 1;
        }
    }
}
