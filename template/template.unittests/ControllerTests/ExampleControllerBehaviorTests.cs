﻿using System;
using System.Reflection;
using FluentAssertions;
using template.Api.Controllers;
using template.Api.Models;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace Mcg.Webservice.UnitTests.ControllerTests
{
	[TestFixture]
	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
	public class ExampleControllerBehaviorTests
	{
		[Test]
		public void Ctor_throws_ArgumentNullException()
		{
			Action action = () =>
			{
				_ = new UserController(null, Helpers.MockIAppSettings());
			};

			action.Should().Throw<ArgumentNullException>();
		}

		[Test]
		public void Ctor_returns_new_DefaultController_instance()
		{
			var target = new UserController(Helpers.MockExampleBusinessLogic(), Helpers.MockIAppSettings());

			target.Should().NotBeNull()
				.And.BeAssignableTo<ControllerBase>();
		}

		[Test]
		public void DefaultController_is_decorated_with_expected_attributes()
		{
			typeof(UserController).Should()
				.BeDecoratedWith<ApiControllerAttribute>()
				.And.BeDecoratedWith<ProducesAttribute>(p => p.ContentTypes.Contains("application/json"))
				.And.BeDecoratedWith<RouteAttribute>(r => r.Template == "api/[controller]");

		}

		[Test]
		public void DefaultController_Get_is_decorated_with_expected_attributes()
		{
			MethodInfo target = typeof(UserController)
				.GetMethod("Get", new Type[] { });

			target.Should().BeDecoratedWith<HttpGetAttribute>()
			.And.BeDecoratedWith<ProducesResponseTypeAttribute>(p => (p.StatusCode == 200 && p.Type == typeof(string)))
			.And.BeDecoratedWith<ProducesResponseTypeAttribute>(p => (p.StatusCode == 400 && p.Type == typeof(string)))
			.And.BeDecoratedWith<ProducesResponseTypeAttribute>(p => (p.StatusCode == 401 && p.Type == typeof(string)))
			.And.BeDecoratedWith<ProducesResponseTypeAttribute>(p => (p.StatusCode == 500 && p.Type == typeof(string)))
			.And.BeDecoratedWith<ProducesResponseTypeAttribute>(p => (p.StatusCode == 502 && p.Type == typeof(string)));
		}



		[Test]
		public void DefaultController_Get_by_id_is_decorated_with_expected_attributes()
		{
			MethodInfo target = typeof(UserController)
				.GetMethod("GetById", new Type[] { typeof(int) });

			target.Should()
				.BeDecoratedWith<HttpGetAttribute>(h => h.Template == "id/{id}")
				.And.BeDecoratedWith<ProducesResponseTypeAttribute>(p => (p.StatusCode == 200 && p.Type == typeof(string)))
				.And.BeDecoratedWith<ProducesResponseTypeAttribute>(p => (p.StatusCode == 400 && p.Type == typeof(string)))
				.And.BeDecoratedWith<ProducesResponseTypeAttribute>(p => (p.StatusCode == 401 && p.Type == typeof(string)))
				.And.BeDecoratedWith<ProducesResponseTypeAttribute>(p => (p.StatusCode == 404 && p.Type == typeof(string)))
				.And.BeDecoratedWith<ProducesResponseTypeAttribute>(p => (p.StatusCode == 500 && p.Type == typeof(string)))
				.And.BeDecoratedWith<ProducesResponseTypeAttribute>(p => (p.StatusCode == 502 && p.Type == typeof(string)));
		}

		[Test]
		public void DefaultController_Get_by_email_is_decorated_with_expected_attributes()
		{
			MethodInfo target = typeof(UserController)
				.GetMethod("GetByEmail", new Type[] { typeof(string) });

			target.Should()
				.BeDecoratedWith<HttpGetAttribute>(h => h.Template == "email/{email}")
				.And.BeDecoratedWith<ProducesResponseTypeAttribute>(p => (p.StatusCode == 200 && p.Type == typeof(string)))
				.And.BeDecoratedWith<ProducesResponseTypeAttribute>(p => (p.StatusCode == 400 && p.Type == typeof(string)))
				.And.BeDecoratedWith<ProducesResponseTypeAttribute>(p => (p.StatusCode == 401 && p.Type == typeof(string)))
				.And.BeDecoratedWith<ProducesResponseTypeAttribute>(p => (p.StatusCode == 404 && p.Type == typeof(string)))
				.And.BeDecoratedWith<ProducesResponseTypeAttribute>(p => (p.StatusCode == 500 && p.Type == typeof(string)))
				.And.BeDecoratedWith<ProducesResponseTypeAttribute>(p => (p.StatusCode == 502 && p.Type == typeof(string)));
		}

		[Test]
		public void DefaultController_Post_is_decorated_with_expected_attributes()
		{
			MethodInfo target = typeof(UserController)
				.GetMethod("Post", new Type[] { typeof(UserModel) });

			target.Should()
				.BeDecoratedWith<HttpPostAttribute>()
				.And.BeDecoratedWith<ProducesResponseTypeAttribute>(p => (p.StatusCode == 202))
				.And.BeDecoratedWith<ProducesResponseTypeAttribute>(p => (p.StatusCode == 400 && p.Type == typeof(string)))
				.And.BeDecoratedWith<ProducesResponseTypeAttribute>(p => (p.StatusCode == 401 && p.Type == typeof(string)))
				.And.BeDecoratedWith<ProducesResponseTypeAttribute>(p => (p.StatusCode == 409 && p.Type == typeof(string)))
				.And.BeDecoratedWith<ProducesResponseTypeAttribute>(p => (p.StatusCode == 412 && p.Type == typeof(string)))
				.And.BeDecoratedWith<ProducesResponseTypeAttribute>(p => (p.StatusCode == 422 && p.Type == typeof(string)))
				.And.BeDecoratedWith<ProducesResponseTypeAttribute>(p => (p.StatusCode == 500 && p.Type == typeof(string)))
				.And.BeDecoratedWith<ProducesResponseTypeAttribute>(p => (p.StatusCode == 502 && p.Type == typeof(string)));
		}

		[Test]
		public void DefaultController_Put_is_decorated_with_expected_attributes()
		{
			MethodInfo target = typeof(UserController)
				.GetMethod("Put", new Type[] { typeof(UserModel) });

			target.Should()
				.BeDecoratedWith<HttpPutAttribute>()
				.And.BeDecoratedWith<ProducesResponseTypeAttribute>(p => (p.StatusCode == 204))
				.And.BeDecoratedWith<ProducesResponseTypeAttribute>(p => (p.StatusCode == 400 && p.Type == typeof(string)))
				.And.BeDecoratedWith<ProducesResponseTypeAttribute>(p => (p.StatusCode == 401 && p.Type == typeof(string)))
				.And.BeDecoratedWith<ProducesResponseTypeAttribute>(p => (p.StatusCode == 404 && p.Type == typeof(string)))
				.And.BeDecoratedWith<ProducesResponseTypeAttribute>(p => (p.StatusCode == 412 && p.Type == typeof(string)))
				.And.BeDecoratedWith<ProducesResponseTypeAttribute>(p => (p.StatusCode == 500 && p.Type == typeof(string)))
				.And.BeDecoratedWith<ProducesResponseTypeAttribute>(p => (p.StatusCode == 502 && p.Type == typeof(string)));
		}

		[Test]
		public void DefaultController_Delete_is_decorated_with_expected_attributes()
		{
			MethodInfo target = typeof(UserController)
				.GetMethod("Delete", new Type[] { typeof(UserModel) });

			target.Should()
				.BeDecoratedWith<HttpDeleteAttribute>()
				.And.BeDecoratedWith<ProducesResponseTypeAttribute>(p => (p.StatusCode == 204))
				.And.BeDecoratedWith<ProducesResponseTypeAttribute>(p => (p.StatusCode == 400 && p.Type == typeof(string)))
				.And.BeDecoratedWith<ProducesResponseTypeAttribute>(p => (p.StatusCode == 401 && p.Type == typeof(string)))
				.And.BeDecoratedWith<ProducesResponseTypeAttribute>(p => (p.StatusCode == 412 && p.Type == typeof(string)))
				.And.BeDecoratedWith<ProducesResponseTypeAttribute>(p => (p.StatusCode == 500 && p.Type == typeof(string)))
				.And.BeDecoratedWith<ProducesResponseTypeAttribute>(p => (p.StatusCode == 502 && p.Type == typeof(string)));
		}
	}
}
