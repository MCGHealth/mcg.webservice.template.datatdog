﻿using System;
using System.Collections.Generic;
using Mcg.Webservice.Api.DataAccess;
using Mcg.Webservice.Api.Infrastructure.Instrumentation;
using Mcg.Webservice.Api.Infrastructure.Logging;
using Mcg.Webservice.Api.Infrastructure.Tracing;
using Mcg.Webservice.Api.Models;

namespace Mcg.Webservice.Api.Services
{
	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class ExampleBusinessService: IExampleBusinessService
    {
        internal IExampleDataRepository DataAccess { get; set; }

        public ExampleBusinessService(IExampleDataRepository dataAccess)
        {
            DataAccess = dataAccess ?? throw new ArgumentNullException(nameof(dataAccess));
        }

        [Trace, Instrument, Log]
        public (bool ok, string error) Delete(ExampleModel model)
        {
            if (model == null)
            {
                return (ok: false, error: "The model is null.");
            }

            DataAccess.Delete(model);

            return (ok: true, error: null);
        }

        [Trace, Instrument, Log]
        public (bool ok, string error, ExampleModel newModel) Insert(ExampleModel model)
        {
            if (DataAccess.Contains(model.ID))
            {
                return (ok: false, error: $"A model with the id {model.ID} already exists.", newModel: null);
            }

            if (DataAccess.Contains(model.EmailAddress))
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

        [Trace, Instrument, Log]
        public IEnumerable<ExampleModel> SelectAll()
        {
            return DataAccess.SelectAll();
        }

        [Trace, Instrument, Log]
        public ExampleModel SelectByEmail(string email)
        {
            return DataAccess.SelectOneByEmail(email);
        }

        [Trace, Instrument, Log]
        public ExampleModel SeledtById(int id)
        {
            return DataAccess.SelectOneById(id);
        }

        [Trace, Instrument, Log]
        public (bool ok, string error) Update(ExampleModel model)
        {
            if (!DataAccess.Contains(model.ID))
            {
                return (ok: false, error: $"Entity not found in data store.");
            }

            return (ok: true, error: null);
        }
    }
}
