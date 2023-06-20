using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace CalculatorServer.Controllers
{
    [Route("api/[Controller]")]
    public class CalculatorController : Controller
    {
        [HttpGet("{operation}/{number1}/{number2}")]
        public double Operation(string operation, double number1, double number2)
        {
            double result;
            switch (operation)
            {
                case "add":
                    result = number1 + number2;
                    break;
                case "subtract":
                    result = number1 - number2;
                    break;
                case "multiply":
                    result = number1 * number2;
                    break;
                case "division":
                    result = number1 / number2;
                    break;
                default:
                    result = 0;
                    break;
            }

            var dbModel = new CalculatorDbModel();
            dbModel.Id = new Guid();
            dbModel.CreatedOn = DateTime.Now;
            dbModel.FirstNumber = number1;
            dbModel.SecondNumber = number2;
            dbModel.Operation = operation;
            dbModel.Result = result;
            InsertRecord(dbModel);

            return result;
        }

        public static void InsertRecord(CalculatorDbModel dbModel)
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("Calculator");
            var collections = database.GetCollection<CalculatorDbModel>("CalculatorDbModel");

            collections.InsertOne(dbModel);
        }
    }

    public class CalculatorDbModel
    {
        public Guid Id { get; set; }

        public double FirstNumber { get; set; }

        public double SecondNumber { get; set; }

        public string Operation { get; set; }

        public double Result { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}

