using DeltaTrack.Services;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Linq;
using System.Threading.Tasks;

namespace DeltaTrackTests
{
    [TestClass]
    public class StorageServiceTests
    {
        [TestMethod]
        public async Task StoreStationsIntoXml_ValidInput_Success()
        {
            CtaService ctaService = new CtaService();
            var result = await ctaService.GetStations();

            
            if (!result.Any())
            {
                Assert.Fail("Returned no Stations!");
            }

            var storageService = new StorageService();
            await storageService.StoreStationsIntoXml(result);

            Assert.IsTrue(true);

        }

        [TestMethod]
        public async Task RetrieveStations_ValidInput_Success()
        {
            var storageService = new StorageService();

            var results = await storageService.RetrieveStations();

            Assert.IsTrue(results.Any());

        }

        [TestMethod]
        public async Task StationsStoreAndRetrieve_ValidInput_Success()
        {
            CtaService ctaService = new CtaService();
            var result = await ctaService.GetStations();

            
            if (!result.Any())
            {
                Assert.Fail("Returned no Stations!");
            }

            var storageService = new StorageService();

            await storageService.StoreStationsIntoXml(result);


            var results = await storageService.RetrieveStations();

            Assert.IsTrue(results.Any());

        }
    }
}
