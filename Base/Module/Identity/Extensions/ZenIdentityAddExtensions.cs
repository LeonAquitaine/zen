﻿using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Zen.Base.Module.Identity.Collections;
using Zen.Base.Module.Identity.Model;
using Zen.Base.Module.Identity.Store;

namespace Zen.Base.Module.Identity.Extensions
{
    public static class ZenIdentityAddExtensions
    {
        public static IdentityBuilder AddZenIdentityProvider<TUser>(this IServiceCollection services)
            where TUser : User
        {
            return AddZenIdentityProvider<TUser, Role>(services, x => { });
        }

        public static IdentityBuilder AddZenIdentityProvider<TUser>(this IServiceCollection services, Action<ZenIdentityOptions> setupDatabaseAction)
            where TUser : User
        {
            return AddZenIdentityProvider<TUser, Role>(services, setupDatabaseAction);
        }

        public static IdentityBuilder AddZenIdentityProvider<TUser, TRole>(this IServiceCollection services, Action<ZenIdentityOptions> setupDatabaseAction)
            where TUser : User
            where TRole : Role
        {
            return AddZenIdentityProvider<TUser, TRole>(services, x => { }, setupDatabaseAction);
        }

        public static IdentityBuilder AddZenIdentityProvider(this IServiceCollection services, Action<IdentityOptions> setupIdentityAction, Action<ZenIdentityOptions> setupDatabaseAction) { return AddZenIdentityProvider<User, Role>(services, setupIdentityAction, setupDatabaseAction); }

        public static IdentityBuilder AddZenIdentityProvider<TUser>(this IServiceCollection services, Action<IdentityOptions> setupIdentityAction, Action<ZenIdentityOptions> setupDatabaseAction)
            where TUser : User
        {
            return AddZenIdentityProvider<TUser, Role>(services, setupIdentityAction, setupDatabaseAction);
        }

        public static IdentityBuilder AddZenIdentityProvider<TUser, TRole>(this IServiceCollection services, Action<IdentityOptions> setupIdentityAction, Action<ZenIdentityOptions> setupDatabaseAction)
            where TUser : User
            where TRole : Role
        {
            var dbOptions = new ZenIdentityOptions();
            setupDatabaseAction(dbOptions);

            var builder = services.AddIdentity<TUser, TRole>(setupIdentityAction ?? (x => { }));

            builder.AddRoleStore<RoleStore<TRole>>()
                .AddUserStore<UserStore<TUser, TRole>>()
                .AddUserManager<UserManager<TUser>>()
                .AddRoleManager<RoleManager<TRole>>()
                .AddDefaultTokenProviders();

            var userCollection = new IdentityUserCollection<TUser>();
            var roleCollection = new IdentityRoleCollection<TRole>();

            services.AddTransient<IIdentityUserCollection<TUser>>(x => userCollection);
            services.AddTransient<IIdentityRoleCollection<TRole>>(x => roleCollection);

            // Identity Services
            services.AddTransient<IUserStore<TUser>>(x => new UserStore<TUser, TRole>(userCollection, roleCollection, x.GetService<ILookupNormalizer>()));
            services.AddTransient<IRoleStore<TRole>>(x => new RoleStore<TRole>(roleCollection));

            return builder;
        }
    }
}