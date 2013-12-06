// --------------------------------------------------------------------------
// <copyright file="BundleConfig.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.06.14">Support bundle mode release</change>
// </history>
namespace Fanex.NAP.Settlement.App_Start
{
    #region

    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using System.Web.Optimization;
    using BundleTransformer.Core.Orderers;
    using BundleTransformer.Core.Transformers;

    #endregion

    /// <summary>
    /// Bundle Config.
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.ReadabilityRules", "SA1116:SplitParametersMustStartOnLineAfterDeclaration",
             Justification = "Reviewed. Suppression is OK here.")]
    public class BundleConfig
    {
        /// <summary>
        /// Register Bundles.
        /// </summary>
        /// <param name="bundles">Bundle Collection.</param>
        public static void RegisterBundles(BundleCollection bundles)
        {
            CSSBundles(bundles);
            JSBundles(bundles);
        }

        /// <summary>
        /// CSS Bundles.
        /// </summary>
        /// <param name="bundles">Bundle Collection.</param>
        private static void CSSBundles(BundleCollection bundles)
        {
            bundles.AddAndInclude("~/css/jquery-all",
             "~/Content/libraries/jquery-ui/jquery-ui.css",
             "~/Content/extension/jquery-block-ui.ext.css",
              "~/Content/extension/jquery-ui.ext.css",
              "~/Content/plugin/tipsy/tipsy.css");
            bundles.AddAndInclude("~/css/jquery-ui",
           "~/Content/libraries/jquery-ui/jquery-ui.css",
           "~/Content/extension/jquery-block-ui.ext.css");
            bundles.AddAndInclude("~/css/log-settlement",
              "~/Content/sites/grid26.css",
              "~/Content/extension/jquery-ui.ext.css",
              "~/Content/sites/global.css",
              "~/Content/sites/global.grid.css",
              "~/Content/sites/settlement-log.css");
            bundles.AddAndInclude("~/css/outright",
              "~/Content/sites/grid26.css",
              "~/Content/sites/global.css",
              "~/Content/sites/global.grid.css",
              "~/Content/sites/rule.css",
              "~/Content/sites/outright.css");
            bundles.AddAndInclude("~/css/settlement",
              "~/Content/sites/grid26.css",
              "~/Content/sites/global.css",
              "~/Content/sites/global.grid.css",
              "~/Content/sites/rule.css");
            bundles.AddAndInclude("~/css/calc",
             "~/Content/sites/grid26.css",
             "~/Content/sites/global.css",
             "~/Content/sites/global.grid.css",
             "~/Content/sites/rule.css",
             "~/Content/sites/calc.css");
            bundles.AddAndInclude("~/css/refund",
             "~/Content/sites/grid26.css",
             "~/Content/sites/global.css",
             "~/Content/sites/global.grid.css",
             "~/Content/sites/rule.css",
             "~/Content/sites/refund.css");
            bundles.AddAndInclude("~/css/action-log",
            "~/Content/sites/grid26.css",
            "~/Content/sites/global.css",
            "~/Content/sites/global.grid.css",
            "~/Content/sites/settlement-log.css",
            "~/Content/sites/log-action.css");
        }

        /// <summary>
        /// JQuery bundles.
        /// </summary>
        /// <param name="bundles">Bundle Collection.</param>
        private static void JqueryBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/scripts/jquery-all").Include(
              "~/Scripts/libraries/jquery.js",
              "~/Scripts/libraries/jquery-ui.js",
              "~/Scripts/libraries/jquery.blockUI.js",
              "~/Scripts/extension/jquery.ext.js",
              "~/Scripts/extension/jquery-ui.autocomplete.ext.js",
              "~/Scripts/plugin/validators/validators.js",
              "~/Scripts/plugin/tipsy/jquery.tipsy.js"));
            bundles.Add(new ScriptBundle("~/scripts/jquery-ui").Include(
              "~/Scripts/libraries/jquery.js",
              "~/Scripts/libraries/jquery-ui.js",
              "~/Scripts/libraries/jquery.blockUI.js",
              "~/Scripts/extension/jquery.ext.js",
              "~/Scripts/plugin/validators/validators.js"));
            bundles.Add(new ScriptBundle("~/scripts/blockUI").Include(
              "~/Scripts/libraries/jquery.js",
              "~/Scripts/libraries/jquery.blockUI.js"));
        }

        /// <summary>
        /// JS Bundles.
        /// </summary>
        /// <param name="bundles">Bundle Collection.</param>
        private static void JSBundles(BundleCollection bundles)
        {
            JqueryBundles(bundles);
            bundles.Add(new ScriptBundle("~/scripts/commonJs").Include(
               "~/Scripts/libraries/jquery.js",
               "~/Scripts/sites/global.js",
               "~/Scripts/sites/nap.message-box.js",
               "~/Scripts/sites/dialog-box.js",
               "~/Scripts/sites/languages.js"));
            bundles.Add(new ScriptBundle("~/scripts/log-settlement").Include(
               "~/Scripts/sites/global.js",
               "~/Scripts/sites/nap.message-box.js",
               "~/Scripts/sites/dialog-box.js",
               "~/Scripts/sites/settlement-log.js",
               "~/Scripts/sites/languages.js"));
            bundles.Add(new ScriptBundle("~/scripts/outright").Include(
               "~/Scripts/sites/global.js",
               "~/Scripts/sites/nap.message-box.js",
               "~/Scripts/sites/dialog-box.js",
               "~/Scripts/sites/calculate.js",
               "~/Scripts/sites/outright.js",
               "~/Scripts/sites/languages.js"));
            bundles.Add(new ScriptBundle("~/scripts/settlement").Include(
              "~/Scripts/sites/global.js",
              "~/Scripts/sites/nap.message-box.js",
              "~/Scripts/sites/dialog-box.js",
              "~/Scripts/sites/calculate.js",
              "~/Scripts/sites/multiple-ticket.js",
              "~/Scripts/sites/type.js",
              "~/Scripts/sites/languages.js"));
            bundles.Add(new ScriptBundle("~/scripts/calc").Include(
             "~/Scripts/sites/global.js",
             "~/Scripts/sites/nap.message-box.js",
             "~/Scripts/sites/dialog-box.js",
             "~/Scripts/sites/calc.js",
             "~/Scripts/sites/single-ticket.js",
             "~/Scripts/sites/type.js",
             "~/Scripts/sites/languages.js"));
            bundles.Add(new ScriptBundle("~/scripts/refund").Include(
             "~/Scripts/sites/global.js",
             "~/Scripts/sites/nap.message-box.js",
             "~/Scripts/sites/dialog-box.js",
             "~/Scripts/sites/refund.js",
             "~/Scripts/sites/multiple-ticket.js",
             "~/Scripts/sites/single-ticket.js",
             "~/Scripts/sites/type.js",
             "~/Scripts/sites/languages.js"));
            bundles.Add(new ScriptBundle("~/scripts/action-log").Include(
            "~/Scripts/sites/global.js"));
        }
    }

    /// <summary>
    /// Bundle Extensions.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
             "Microsoft.StyleCop.CSharp.MaintainabilityRules",
             "SA1402:FileMayOnlyContainASingleClass",
             Justification = "Splitting this file into classes would get too confusing.")]
    internal static class BundleExtensions
    {
        /// <summary>
        /// Add And Include.
        /// </summary>
        /// <param name="bundles">Bundle Collection.</param>
        /// <param name="bundleVirtualPath">BundleV Virtual Path.</param>
        /// <param name="virtualPaths">Virtual Path.</param>
        public static void AddAndInclude(
            this BundleCollection bundles, string bundleVirtualPath, params string[] virtualPaths)
        {
            bundles.Add(new Bundle(bundleVirtualPath)
            {
                Transforms = { new CssTransformer(), new CssMinify() },
                Orderer = new NullOrderer()
            }.Include(virtualPaths));
        }

        /// <summary>
        /// Relative From Absolute Path.
        /// </summary>
        /// <param name="context">Context value.</param>
        /// <param name="path">Path value.</param>
        /// <returns>Return string.</returns>
        public static string RelativeFromAbsolutePath(this HttpContextBase context, string path)
        {
            var request = context.Request;
            var applicationPath = request.PhysicalApplicationPath;
            var virtualDir = request.ApplicationPath;
            virtualDir = virtualDir == "/" ? virtualDir : (virtualDir + "/");
            return path.Replace(applicationPath, virtualDir).Replace(@"\", "/");
        }
    }
}