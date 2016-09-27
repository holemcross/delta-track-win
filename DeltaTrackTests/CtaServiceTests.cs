using DeltaTrack.Services;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Linq;
using System.Threading.Tasks;

namespace DeltaTrackTests
{
    [TestClass]
    public class CtaServiceTests
    {
        [TestMethod]
        public async Task GetArrivals_ValidInput_Success()
        {
            var mapId = 40730;
            CtaService service = new CtaService();
            var result = await service.GetArrivalsForMapId(mapId);

            
            if (result.ErrorCode != 0)
            {
                Assert.Fail("Error Code Returned from API");
            }

            Assert.IsTrue(result.TrainArrivals.Any());

        }

        [TestMethod]
        public async Task GetStations_ValidInput_Success()
        {
            CtaService service = new CtaService();
            var result = await service.GetStations();
            Assert.IsTrue(result.Any());

        }
    }
}
