﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Migrations;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Scoping;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Infrastructure.Migrations.Upgrade;
using Umbraco.Cms.Infrastructure.Migrations;
using Microsoft.AspNetCore.Routing;
using Umbraco.Extensions;
using MediaRetention.Controllers;

namespace MediaRetention.NotificationHandlers
{
    public class ServerVariables : INotificationHandler<ServerVariablesParsingNotification>
    {
        private LinkGenerator _linkGenerator;

        public ServerVariables(LinkGenerator linkGenerator)
        {
            _linkGenerator  = linkGenerator;
        }

        public void Handle(ServerVariablesParsingNotification notification)
        {
            var serverVariables = notification.ServerVariables;

            var umbracoUrlsObject = serverVariables["umbracoUrls"];

            if (umbracoUrlsObject == null)
            {
                throw new ArgumentException("Null umbracoUrls");
            }

            if (!(umbracoUrlsObject is Dictionary<string, object> umbracoUrls)) 
            {
                throw new ArgumentException("Invalid umbracoUrls");
            }

            var mediaRetentionControllerUrl = _linkGenerator
                .GetUmbracoApiServiceBaseUrl<MediaRetentionApiController>(controller => controller.GetAll(0));

            umbracoUrls[Constants.PluginName + "BaseUrl"] = mediaRetentionControllerUrl;
        }
    }
}
