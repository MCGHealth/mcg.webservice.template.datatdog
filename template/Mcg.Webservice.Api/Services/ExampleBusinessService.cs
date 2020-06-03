using System;
using System.Collections.Generic;
using Mcg.Webservice.Api.DataAccess;
using Mcg.Webservice.Api.Infrastructure.Instrumentation;
using Mcg.Webservice.Api.Infrastructure.Logging;
using Mcg.Webservice.Api.Infrastructure.Tracing;
using Mcg.Webservice.Api.Models;

namespace Mcg.Webservice.Api.Services
{
    [Trace, Metrics, Log]
    public class UserBusinessService : IExampleBusinessService
    {
        internal IExampleDataRepository DataAccess { get; set; }

        public UserBusinessService(IExampleDataRepository dataAccess)
        {
            DataAccess = dataAccess ?? throw new ArgumentNullException(nameof(dataAccess));
        }

        public (bool ok, string error) Delete(UserModel model)
        {
            if (model == null)
            {
                return (ok: false, error: "The model is null.");
            }

            DataAccess.Delete(model);

            return (ok: true, error: null);
        }

        public (bool ok, string error, UserModel newModel) Insert(UserModel model)
        {
            if (DataAccess.ContainsId(model.ID))
            {
                return (ok: false, error: $"A model with the id {model.ID} already exists.", newModel: null);
            }

            if (DataAccess.ContainsEmail(model.EmailAddress))
            {
                return (ok: false, error: $"A model with the email {model.EmailAddress} already exists.", newModel: null);
            }

            if (model.ID < 1)
            {
                model.ID = DataAccess.NextId();
            }

            DataAccess.Insert(model);

            return (ok: true, error: null, model);
        }

        public IEnumerable<UserModel> SelectAll()
        {
            return DataAccess.SelectAll();
        }

        public UserModel SelectByEmail(string email)
        {
            return DataAccess.SelectOneByEmail(email);
        }

        public UserModel SeledtById(int id)
        {
            return DataAccess.SelectOneById(id);
        }

        public (bool ok, string error) Update(UserModel model)
        {
            if (!DataAccess.ContainsId(model.ID))
            {
                return (ok: false, error: $"Entity not found in data store.");
            }

            return (ok: true, error: null);
        }
    }
}
