﻿using System;
using Autofac;
using Glass.Mapper.Sc;
using Jabberwocky.Glass.Autofac.Factory.Builder;
using Jabberwocky.Glass.Autofac.Factory.Implementation;
using Jabberwocky.Glass.Factory;
using Jabberwocky.Glass.Factory.Configuration;
using Jabberwocky.Glass.Factory.Implementation;
using Jabberwocky.Glass.Factory.Interceptors;

namespace Jabberwocky.Glass.Autofac.Extensions
{
	public static class FactoryRegistrationExtensions
	{
		/// <summary>
		/// Registers the Glass Interface Factory, and all Glass Factory Interfaces and Models
		/// 
		/// </summary>
		/// <param name="builder">The container builder.</param><param name="assemblyNames">Assemblies to scan for Glass Interfaces and Models.</param>
		/// <returns>
		/// Container Builder
		/// </returns>
		public static ContainerBuilder RegisterGlassFactory(this ContainerBuilder builder, params string[] assemblyNames)
		{
			// If necessary, register Glass SitecoreService
			builder.RegisterType<ISitecoreContext>().As<ISitecoreService>().PreserveExistingDefaults().ExternallyOwned();

			builder.RegisterType<FallbackInterceptor>();

			builder.RegisterType<AutofacImplementationFactory>().As<IImplementationFactory>();
			builder.Register(c => new AutofacGlassFactoryBuilder(new ConfigurationOptions(assemblyNames), c.Resolve<IContainer>())).As<AutofacGlassFactoryBuilder>();

			builder.Register(c => c.Resolve<AutofacGlassFactoryBuilder>().BuildFactory(c.Resolve<IImplementationFactory>(), c.Resolve<Func<ISitecoreService>>()))
				.As<IGlassInterfaceFactory>()
				.SingleInstance();

			builder.Register(c => c.Resolve<IGlassInterfaceFactory>() as IGlassTemplateCache)
				.As<IGlassTemplateCache>()
				.SingleInstance();

			return builder;
		}
	}
}
