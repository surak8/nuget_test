// https://docs.nuget.org/create/creating-and-publishing-a-package
namespace NSNuGetTest {
    public class TemperatureConverter {
        public decimal ToFahrenheit(decimal t) { return (t * 1.8m) + 32; }
        public decimal ToCelsius(decimal t) { return (t - 32) / 1.8m; }
    }
}