using Grpc.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Shape_area_calculator
{
    public class ShapeAreaCalculatorFunction
    {
        private readonly ILogger<ShapeAreaCalculatorFunction> _logger;

        public ShapeAreaCalculatorFunction(ILogger<ShapeAreaCalculatorFunction> logger)
        {
            _logger = logger;
        }

        [Function("ShapeAreaCalculator")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req , string shapeName , string? dimensions, string units , double? radius)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            // shared variables
            List<double> dimensionsListdoubles = new List<double>();
            double calculatedArea = 0;
            ObjectResult result;

            try
            {
                switch (shapeName)
                {
                    case nameof(ShapeEnums.Rectangle):
                        dimensionsListdoubles = PopulateDimensionsList(dimensions! , shapeName);
                        calculatedArea = dimensionsListdoubles[0] * dimensionsListdoubles[1];
                        break;


                    case nameof(ShapeEnums.Triangle):
                        dimensionsListdoubles = PopulateDimensionsList(dimensions!, shapeName);
                        double s = (dimensionsListdoubles[0] + dimensionsListdoubles[1] + dimensionsListdoubles[2]) / 2;
                        calculatedArea = Math.Sqrt(s * (s - dimensionsListdoubles[0]) * (s - dimensionsListdoubles[1]) * (s - dimensionsListdoubles[2])); // Heron's formula
                        break;


                    case nameof(ShapeEnums.Circle):
                        if(radius == null)
                        {
                             result = new ObjectResult("Radius Not provided");
                            result.StatusCode = StatusCodes.Status400BadRequest;
                            return result;
                        }
                        else
                        {
                            // Pi X R^2
                            calculatedArea = Math.PI * Math.Pow((double)radius, 2);
                        }
                        break;


                    default:
                        return new OkObjectResult("Not a supported shape");
                }
                calculatedArea = Math.Round(calculatedArea, 5); // Round to 5 decimal places


                // 200 success result built and returned
                var returnJsonObject = new
                {
                    Message = $"The area of your {shapeName} is {calculatedArea.ToString()} {units}²",
                    Value = calculatedArea
                };
                result = new ObjectResult(returnJsonObject);
                result.StatusCode = StatusCodes.Status200OK;
                return result;

            }
            catch(Exception ex)
            {

                result = new ObjectResult($"Internal Server Error, {ex.Message}" );
                result.StatusCode = StatusCodes.Status500InternalServerError;
                return result;
            }
        }

        public List<double> PopulateDimensionsList(string dimensions , string shapeName)
        {
            if(string.IsNullOrEmpty(dimensions))
            {
                throw new Exception("shape dimensions must be provided");
            }


            List<double> dlist = new List<double>();

            // Split up shape dimension using 'x' 
            if (shapeName == nameof(ShapeEnums.Rectangle) | shapeName == nameof(ShapeEnums.Triangle))
            {
                List<string> dimensionsList = dimensions.Split('x').ToList();
                foreach (var dimension in dimensionsList)
                {
                    dlist.Add(double.Parse(dimension));
                };
            }
            return dlist;
        }
    }
}
