using System.Collections.Generic;

namespace CarDealer.DTO
{
    public class CarDto
    {
        public CarDto()
        {
            PartsId = new List<int>();
        }

        public string Make { get; set; }

        public string Model { get; set; }

        public long TravelledDistance { get; set; }

        public List<int> PartsId { get; set; }
    }
}