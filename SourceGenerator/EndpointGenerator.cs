using Microsoft.CodeAnalysis;

namespace Server.Generators
{

    [Generator]
    public class EndpointGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            context.AddSource($"test.generated.cs", "// auto generated");
        }

        public void Initialize(GeneratorInitializationContext context)
        {

        }
    }
}
