﻿using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;
using Volo.CmsKit.Features;
using Volo.CmsKit.Pages;
using Volo.CmsKit.Public.Pages;

namespace Volo.CmsKit.Public.Web;

public class DefaultHomePageMiddleware : IMiddleware, ITransientDependency
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var featureChecker = context.RequestServices.GetRequiredService<IFeatureChecker>();

        if (await featureChecker.IsEnabledAsync(CmsKitFeatures.PageEnable))
        {
            if (context.Request.Path.Value == "/")
            {
                var pagePublicAppService = context.RequestServices.GetRequiredService<IPagePublicAppService>();

                var page = await pagePublicAppService.FindDefaultHomePageAsync();
                if (page != null)
                {
                    context.Request.Path = $"{PageConsts.UrlPrefix}{page.Slug}";
                }
            }
        }

        await next(context);
    }
}