namespace AsyncStateMachine.Services;

public interface IService
{
    Task<object> GetParseLocalJSONAsync();
    object GetParseLocalJSON();
}