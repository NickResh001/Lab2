using Lab2.DTOs;
using Lab2.Models;

namespace Lab2.Services
{
    public interface IHeatingModelingService
    {
        public Task SetSettings(SettingsDto settingsDto);
        public Task<double[][][]> Solve();
    }

    public class HeatingModelingService : IHeatingModelingService
    {
        private HeatingModelSettings _hmSettings;
        private bool isParallel;

        public HeatingModelingService()
        {
            _hmSettings = new();
            isParallel = false;
        }

        public async Task SetSettings(SettingsDto settingsDto)
        {
            _hmSettings = new(settingsDto);
            isParallel = settingsDto.IsParallel;
        }

        public async Task<double[][][]> Solve()
        {
            HeatingModeling3d solver = new();

            if (!_hmSettings.isStable)
                return null;
            if (isParallel)
                solver.SolveParallel(_hmSettings);
            else
                solver.Solve(_hmSettings);

            return solver.st.uNew;
        }
    }
}
