﻿using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Zen.Base.Startup {
    public static class ZenUseExtensions
    {
        public static void UseZen(this IApplicationBuilder app, Action<IZenBuilder> configuration)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            var optionsProvider = app.ApplicationServices.GetService<IOptions<ZenOptions>>();

            var options = new ZenOptions(optionsProvider.Value);

            var builder = new ZenBuilder(app, options);

            configuration.Invoke(builder);
        }

    }
}