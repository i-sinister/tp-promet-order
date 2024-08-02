namespace Pos.UI.OpenApi
{
	using Microsoft.AspNetCore.OData.Query;
	using NJsonSchema;
	using NSwag;
	using NSwag.Generation.Processors;
	using NSwag.Generation.Processors.Contexts;
	using System;
	using System.Linq;

	internal class ApplyODataOptionsProcessor : IOperationProcessor
	{
		public bool Process(OperationProcessorContext context)
		{
			var parameters = context.MethodInfo.GetParameters();
			var optionsParameter = parameters.FirstOrDefault(
				parameterInfo => typeof(ODataQueryOptions).Equals(parameterInfo.ParameterType.BaseType));
			if (optionsParameter is null)
			{
				return true;
			}

			var odataOptionsParameter = context.OperationDescription
				.Operation
				.Parameters
				.FirstOrDefault(
					odataParameter => odataParameter.Name.Equals(
						optionsParameter.Name, StringComparison.InvariantCultureIgnoreCase));
			if (odataOptionsParameter is null)
			{
				return true;
			}

			context.OperationDescription.Operation.Parameters.Remove(odataOptionsParameter);
			EnsureODataParameters(context);
			InjectODataParameters(context.Document, context.OperationDescription.Operation);
			return true;
		}

		private void EnsureODataParameters(OperationProcessorContext context)
		{
			var parameters = context.Document.Components.Parameters;
			if (parameters.ContainsKey("$select"))
			{
				return;
			}

			var selectParameter =
				new OpenApiParameter
				{
					DocumentPath = "/components/parameters/$select",
					Name = "$select",
					Kind = OpenApiParameterKind.Query,
					Type = JsonObjectType.String,
					Description = "Comma separated list of properties to select",
					Example = "Name,Type",
				};
			var filterParameter =
				new OpenApiParameter
				{
					DocumentPath = "/components/parameters/$filter",
					Name = "$filter",
					Kind = OpenApiParameterKind.Query,
					Type = JsonObjectType.String,
					Description = "OData v4.0 filter expression",
					Example = "Name eq 'Promet'",
				};
			var orderByParameter =
				new OpenApiParameter
				{
					DocumentPath = "/components/parameters/$orderby",
					Name = "$orderby",
					Kind = OpenApiParameterKind.Query,
					Type = JsonObjectType.String,
					Description = "OData v4.0 order by expression",
					Example = "Name desc",
				};
			var topParameter =
				new OpenApiParameter
				{
					DocumentPath = "/components/parameters/$top",
					Name = "$top",
					Kind = OpenApiParameterKind.Query,
					Type = JsonObjectType.Integer,
					Format = "int32",
					Description = "Number of items to take",
					Example = "10",
				};
			var skipParameter =
				new OpenApiParameter
				{
					DocumentPath = "/components/parameters/$skip",
					Name = "$skip",
					Kind = OpenApiParameterKind.Query,
					Type = JsonObjectType.Integer,
					Format = "int32",
					Description = "Number of items to skip",
					Example = "40",
				};
			var countParameter =
				new OpenApiParameter
				{
					DocumentPath = "/components/parameters/$count",
					Name = "$count",
					Kind = OpenApiParameterKind.Query,
					Description = "When set to 'true' total item count returned",
					Type = JsonObjectType.Boolean,
					Example = "true",
				};
			parameters.Add(selectParameter.Name, selectParameter);
			parameters.Add(filterParameter.Name, filterParameter);
			parameters.Add(orderByParameter.Name, orderByParameter);
			parameters.Add(topParameter.Name, topParameter);
			parameters.Add(skipParameter.Name, skipParameter);
			parameters.Add(countParameter.Name, countParameter);
		}

		private void InjectODataParameters(
			OpenApiDocument document, OpenApiOperation operation)
		{
			AddParameter("$select");
			AddParameter("$filter");
			AddParameter("$orderby");
			AddParameter("$top");
			AddParameter("$skip");
			AddParameter("$count");
			void AddParameter(string name)
			{
				var operationParameter =
					new OpenApiParameter
					{
						Reference = document.Parameters[name],
					};
				operation.Parameters.Add(operationParameter);
			}
		}
	}
}