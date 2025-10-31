using Prague_Parking_V2;

namespace PragueParkingV2.Tests
{
    [TestClass]
    public sealed class ParkingGarageTests
    {
        //Testa metoden CountOccupiedSpots()
        [TestMethod]
        public void CountOccupiedSpots_EmptyGarage_ShouldReturnZero()
        {
            // Arange
            Config config = new Config();
            
            ParkingGarage ParkingGarage = new ParkingGarage(config);

            // Act
            int result = ParkingGarage.CountOccupiedSpots();
            int expected = 0;

            // Assert
            Assert.AreEqual(expected, result);

        }

        //Testa metoden SearchVehicleByRegNumber()
        [TestMethod]
        public void SearchVehicleByRegNumber_EmptyGarage_ShouldReturn_minus1()
        {
            // Arange
            Config config = new Config();
            ParkingGarage ParkingGarage = new ParkingGarage(config);
            string regNumberToSearch = "ABC123";

            // Act
            int result = ParkingGarage.SearchVehicleByRegNumber(regNumberToSearch);
            int expected = -1;

            // Assert
            Assert.AreEqual(expected, result);
        }
    }
}
