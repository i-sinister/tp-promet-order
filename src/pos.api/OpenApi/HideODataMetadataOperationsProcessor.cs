namespace Pos.Api.OpenApi
{
	using NSwag.Generation.Processors;
	using NSwag.Generation.Processors.Contexts;
	using System;

	internal class HideODataMetadataOperations : IOperationProcessor
	{
		public bool Process(OperationProcessorContext context) =>
			!context.OperationDescription.Path.Equals("/api", StringComparison.InvariantCultureIgnoreCase)
				&& !context.OperationDescription.Path.Equals("/api/$metadata", StringComparison.InvariantCultureIgnoreCase);
	}
}
