namespace ClosestCity.Examples
{
    using System.Collections.Generic;
    using NetTopologySuite.Geometries;
    using Swashbuckle.AspNetCore.Filters;

    public class ByLocationExamples : IMultipleExamplesProvider<Coordinate>
    {
        public IEnumerable<SwaggerExample<Coordinate>> GetExamples()
        {
            var examples = new SwaggerExample<Coordinate>();
            examples.Name = "From London";
            examples.Value = new Coordinate(-0.188142, 51.533007);
            yield return examples;

            throw new System.NotImplementedException();
        }
    }
}