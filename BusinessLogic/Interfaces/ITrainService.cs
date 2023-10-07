using EAD_APP.Core.Models;

namespace EAD_APP.BusinessLogic.Interfaces;

public interface ITrainService
{
    Task<List<Train>> GetAllTrains();
    Task<Train> GetTrainById(string id);
    Task<bool> CreateTrain(Train user);
    Task<bool> UpdateTrain(Train user);
    Task<bool> DeleteTrain(string id);
}