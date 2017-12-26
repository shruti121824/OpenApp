﻿using System.Text;
using Abp.Dependency;
using Abp.Extensions;
using Abp.Web.Api.Modeling;
using Abp.Web.Api.ProxyScripting.Generators;
using NJsonSchema.Generation;
using System.Threading.Tasks;
using System.Linq;
using Abp;
using System;

namespace Satrabel.OpenApp.ProxyScripting
{

    class JsonSchemaProxyScriptGenerator : IProxyScriptGenerator, ITransientDependency
    {
        /// <summary>
        /// "jquery".
        /// </summary>
        public const string Name = "json";

        public string CreateScript(ApplicationApiDescriptionModel model)
        {
            var script = new StringBuilder();

            script.AppendLine("/* This file is automatically generated by ABP framework. */");
            script.AppendLine();
            script.AppendLine("var abp = abp || {};");
            script.AppendLine("abp.schemas = abp.schemas || {};");

            foreach (var module in model.Modules.Values)
            {
                script.AppendLine();
                AddModuleScript(script, module);
            }



            //script.Append(Newtonsoft.Json.JsonConvert.SerializeObject(model));

            return script.ToString();
        }

        private static void AddModuleScript(StringBuilder script, ModuleApiDescriptionModel module)
        {
            script.AppendLine($"// module '{module.Name.ToCamelCase()}'");
            script.AppendLine("(function(){");
            script.AppendLine();
            script.AppendLine($"  abp.schemas.{module.Name.ToCamelCase()} = abp.schemas.{module.Name.ToCamelCase()} || {{}};");

            foreach (var controller in module.Controllers.Values)
            {
                script.AppendLine();
                AddControllerScript(script, module, controller);
            }

            script.AppendLine();
            script.AppendLine("})();");
        }

        private static void AddControllerScript(StringBuilder script, ModuleApiDescriptionModel module, ControllerApiDescriptionModel controller)
        {
            script.AppendLine($"  // controller '{controller.Name.ToCamelCase()}'");
            script.AppendLine("  (function(){");
            script.AppendLine();

            script.AppendLine($"    abp.schemas.{module.Name.ToCamelCase()}.{controller.Name.ToCamelCase()} = abp.schemas.{module.Name.ToCamelCase()}.{controller.Name.ToCamelCase()} || {{}};");

            foreach (var action in controller.Actions.Values)
            {
                AddActionScript(script, module, controller, action);
            }

            script.AppendLine();
            script.AppendLine("  })();");
        }

        private static void AddActionScript(StringBuilder script, ModuleApiDescriptionModel module, ControllerApiDescriptionModel controller, ActionApiDescriptionModel action)
        {
            script.AppendLine($"    // action '{action.Name.ToCamelCase()}'");
            var settings = new JsonSchemaGeneratorSettings();
            settings.FlattenInheritanceHierarchy = true;
            settings.DefaultPropertyNameHandling = NJsonSchema.PropertyNameHandling.CamelCase;
            settings.DefaultReferenceTypeNullHandling = NJsonSchema.ReferenceTypeNullHandling.NotNull;

            var generator = new JsonSchemaGenerator(settings);
            script.AppendLine($"    abp.schemas.{module.Name.ToCamelCase()}.{controller.Name.ToCamelCase()}.{action.Name.ToCamelCase()} = abp.schemas.{module.Name.ToCamelCase()}.{controller.Name.ToCamelCase()}.{action.Name.ToCamelCase()} || {{}};");
            var type = action.ReturnValue.Type;
            if (type != typeof(Task))
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Task<>))
                {
                    type = type.GetGenericArguments()[0]; // use this...
                }


                //var schema = await generator.GenerateAsync<JsonSchema4>(typeof(Person));

                var schema = generator.GenerateAsync(type).GetAwaiter().GetResult();
                var schemaData = schema.ToJson();

                //var js = new Newtonsoft.Json.Schema.Generation.JSchemaGenerator();
                //var schema = js.Generate(action.ReturnValue.Type);
                //schema.Title = action.ReturnValue.Type.Name;
                script.Append($"    abp.schemas.{module.Name.ToCamelCase()}.{controller.Name.ToCamelCase()}.{action.Name.ToCamelCase()}.returnValue =  ");
                script.AppendLine(schemaData);
            }
            if (action.Parameters.Any())
            {
                script.Append($"    abp.schemas.{module.Name.ToCamelCase()}.{controller.Name.ToCamelCase()}.{action.Name.ToCamelCase()}.parameters = ");
                script.AppendLine("{");
                foreach (var parameter in action.Parameters)
                {
                    type = parameter.Type;
                    var schema = generator.GenerateAsync(type).GetAwaiter().GetResult();
                    if (parameter.BindingSourceId.IsIn(ParameterBindingSources.ModelBinding, ParameterBindingSources.Query))
                    {
                        schema.Title = parameter.Name;
                        schema.Default = parameter.DefaultValue;
                        //schema.re = parameter.IsOptional
                    }
                    var schemaData = schema.ToJson();


                    //script.Append($"    abp.schemas.{module.Name.ToCamelCase()}.{controller.Name.ToCamelCase()}.{action.Name.ToCamelCase()}.{parameter.Name.ToCamelCase()} =  ");
                    script.Append($"    {parameter.Name.ToCamelCase()} :  ");
                    script.Append(schemaData);
                    script.AppendLine(",");
                }
                script.AppendLine("    };");
            }
            //script.AppendLine("        url: abp.appPath + '" + ProxyScriptingHelper.GenerateUrlWithParameters(action) + "',");

            //var body = GenerateBody(action);
            //if (!body.IsNullOrEmpty())
            //{
            //    script.AppendLine(",");
            //    script.Append("        data: JSON.stringify(" + body + ")");
            //}
            //else
            //{
            //    var formData = GenerateFormPostData(action, 8);
            //    if (!formData.IsNullOrEmpty())
            //    {
            //        script.AppendLine(",");
            //        script.Append("        data: " + formData);
            //    }
            //}




            //var parameterList = ProxyScriptingJsFuncHelper.GenerateJsFuncParameterList(action, "ajaxParams");

            //script.AppendLine($"    // action '{action.Name.ToCamelCase()}'");
            //script.AppendLine($"    abp.schemas.{module.Name.ToCamelCase()}.{controller.Name.ToCamelCase()}{ProxyScriptingJsFuncHelper.WrapWithBracketsOrWithDotPrefix(action.Name.ToCamelCase())} = function({parameterList}) {{");
            //script.AppendLine("      return abp.ajax($.extend(true, {");

            //AddAjaxCallParameters(script, controller, action);

            //script.AppendLine("      }, ajaxParams));;");
            //script.AppendLine("    };");
        }

        private static void AddAjaxCallParameters(StringBuilder script, ControllerApiDescriptionModel controller, ActionApiDescriptionModel action)
        {
            //var httpMethod = action.HttpMethod?.ToUpperInvariant() ?? "POST";

            //script.AppendLine("        url: abp.appPath + '" + Abp.Web.Api.ProxyScripting.Generators.ProxyScriptingHelper.GenerateUrlWithParameters(action) + "',");
            //script.Append("        type: '" + httpMethod + "'");

            //if (action.ReturnValue.Type == typeof(void))
            //{
            //    script.AppendLine(",");
            //    script.Append("        dataType: null");
            //}

            //var headers = ProxyScriptingHelper.GenerateHeaders(action, 8);
            //if (headers != null)
            //{
            //    script.AppendLine(",");
            //    script.Append("        headers: " + headers);
            //}

            //var body = ProxyScriptingHelper.GenerateBody(action);
            //if (!body.IsNullOrEmpty())
            //{
            //    script.AppendLine(",");
            //    script.Append("        data: JSON.stringify(" + body + ")");
            //}
            //else
            //{
            //    var formData = ProxyScriptingHelper.GenerateFormPostData(action, 8);
            //    if (!formData.IsNullOrEmpty())
            //    {
            //        script.AppendLine(",");
            //        script.Append("        data: " + formData);
            //    }
            //}

            //script.AppendLine();
        }
    }
}