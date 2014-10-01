﻿using System;
using System.Reflection;
using Glass.Mapper.Sc;
using Jabberwocky.Glass.Factory.Builder;
using Jabberwocky.Glass.Factory.Configuration;
using Jabberwocky.Glass.Factory.Implementation;
using NSubstitute;
using NUnit.Framework;

namespace Jabberwocky.Glass.Tests.Factory.Builder
{
	[TestFixture]
	public class DefaultGlassFactoryBuilderTests
	{
		private IImplementationFactory _mockImplFactory;
		private ISitecoreService _mockService;

		private IConfigurationOptions _mockOptions;

		// SUT
		private DefaultGlassFactoryBuilder _builder;

		[SetUp]
		public void TestSetup()
		{
			_mockOptions = Substitute.For<IConfigurationOptions>();
			_mockOptions.Assemblies.Returns(new[] {Assembly.GetAssembly(GetType()).FullName});

			_mockImplFactory = Substitute.For<IImplementationFactory>();
			_mockService = Substitute.For<ISitecoreService>();

			_builder = new DefaultGlassFactoryBuilder(_mockOptions);
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void Constructor_NullOptions_Throws()
		{
			new DefaultGlassFactoryBuilder(null);
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void BuildFactory_NullImplFactory_Throws()
		{
			_builder.BuildFactory(null, () => _mockService);
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void BuildFactory_NullServiceFactory_Throws()
		{
			_builder.BuildFactory(_mockImplFactory, null);
		}

		[Test]
		public void BuildFactory_ReturnsFactory()
		{
			var factory = _builder.BuildFactory(_mockImplFactory, () => _mockService);
			Assert.IsNotNull(factory);
		}

		[Test]
		public void BuildFactory_EmptyAssemblies_ReturnsFactory()
		{
			_mockOptions.Assemblies.Returns(new string[0]);

			var factory = _builder.BuildFactory(_mockImplFactory, () => _mockService);
			Assert.IsNotNull(factory);
		}

		[Test]
		public void BuildFactory_BadAssembly_ReturnsFactory()
		{
			_mockOptions.Assemblies.Returns(new[] { "bad, I don't exist" });

			var factory = _builder.BuildFactory(_mockImplFactory, () => _mockService);
			Assert.IsNotNull(factory);
		}
	}
}
