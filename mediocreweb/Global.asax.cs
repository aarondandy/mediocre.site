using System;
using System.Collections.Generic;
using MediocreWeb.things;
using ServiceStack.Razor;
using ServiceStack.Text;
using ServiceStack.WebHost.Endpoints;

namespace MediocreWeb
{
    public class Global : System.Web.HttpApplication
    {

        public class AppHost : AppHostBase
        {
            public AppHost() : base("Mediocresoft", typeof(AppHost).Assembly) { }

            public override void Configure(Funq.Container container) {
                Plugins.Add(new RazorFormat());
                JsConfig.EmitCamelCaseNames = true;
                JsConfig.DateHandler = JsonDateHandler.ISO8601;
                container.Register(_ => new ThingIndex());

                SetConfig(new EndpointHostConfig {
                    AllowFileExtensions = new HashSet<string>(EndpointHostConfig.Instance.AllowFileExtensions) {
                        "zip",
                        "jar",
                        "pdf"
                    }
                });
            }

            public static bool IsDebug {
                get {
#if DEBUG
                    return true;
#else
                    return false;
#endif
                }
            }
        }

        protected void Application_Start(object sender, EventArgs e) {
            new AppHost().Init();
        }

        protected void Session_Start(object sender, EventArgs e) {

        }

        protected void Application_BeginRequest(object sender, EventArgs e) {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e) {

        }

        protected void Application_Error(object sender, EventArgs e) {

        }

        protected void Session_End(object sender, EventArgs e) {

        }

        protected void Application_End(object sender, EventArgs e) {

        }
    }
}